using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using MonopolyWinForms.Room;
using MonopolyWinForms.Services;
using Firebase.Database;
using MonopolyWinForms.GameLogic;
using System.Numerics;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

public sealed class FirebaseService
{
    private static readonly HttpClient _client = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(25)
    };
    private const string BaseUrl = "https://doanmang-8f5af-default-rtdb.asia-southeast1.firebasedatabase.app";
    private readonly Action<string>? _log;
    public FirebaseService(Action<string>? logAction = null)
    {
        if (!_client.DefaultRequestHeaders.Accept.Any())
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        _log = logAction;
    }
    private static readonly object _logLock = new();
    private void Log(string msg)
    {
        string line = $"[{DateTime.UtcNow:O}] {msg}";
        _log?.Invoke(line);
        lock (_logLock)
        {
            File.AppendAllText("log.txt", line + Environment.NewLine);
        }
    }
    public async Task<RoomInfo?> GetRoomAsync(string roomId)
    {
        string url = $"{BaseUrl}/rooms/{Uri.EscapeDataString(roomId)}.json";
        var json = await SafeGetStringAsync(url);
        return json is null ? null : JsonConvert.DeserializeObject<RoomInfo>(json);
    }
    public async Task<Dictionary<string, RoomInfo>?> GetAllRoomsAsync()
    {
        string url = $"{BaseUrl}/rooms.json";
        var json = await SafeGetStringAsync(url);
        return json is null ? null : JsonConvert.DeserializeObject<Dictionary<string, RoomInfo>>(json);
    }
    public Task CreateRoomAsync(string roomId, RoomInfo room) =>
            PutAsync($"{BaseUrl}/rooms/{Uri.EscapeDataString(roomId)}.json", room);
    public Task DeleteRoomAsync(string roomId) =>
        _client.DeleteAsync($"{BaseUrl}/rooms/{Uri.EscapeDataString(roomId)}.json");
    public async Task<GameState?> GetGameStateAsync(string roomId)
    {
        try
        {
            string url = $"{BaseUrl}/gameStates/{Uri.EscapeDataString(roomId)}.json";
            var json = await SafeGetStringAsync(url);
            if (json is null) return null;

            var dto = JsonConvert.DeserializeObject<FullGameStateDto>(json);
            if (dto?.info?.lastUpdateTime is null) return null;

            return new GameState
            {
                RoomId = roomId,
                CurrentPlayerIndex = dto.info.currentPlayerIndex,
                IsGameStarted = dto.info.isGameStarted,
                PlayTime = dto.info.playTime,
                LastUpdateTime = DateTime.Parse(dto.info.lastUpdateTime, null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal),
                Players = dto.players ?? new List<Player>(),
                Tiles = dto.tiles ?? new List<Tile>()
            };
        }
        catch (Exception ex)
        {
            Log($"GetGameStateAsync error: {ex.Message}");
            return null;
        }
    }
    public async Task UpdateGameStateAsync(GameState gs)
    {
        try
        {
            var body = new
            {
                info = new
                {
                    currentPlayerIndex = gs.CurrentPlayerIndex,
                    isGameStarted = gs.IsGameStarted,
                    playTime = gs.PlayTime,
                    lastUpdateTime = DateTime.UtcNow.ToString("O")
                },
                players = gs.Players,
                tiles = gs.Tiles
            };
            string url = $"{BaseUrl}/gameStates/{Uri.EscapeDataString(gs.RoomId)}.json";
            await PutAsync(url, body);
        }
        catch (Exception ex)
        {
            Log($"UpdateGameStateAsync error: {ex.Message}");
        }
    }
    // Thêm phương thức gửi tin nhắn chat
    public Task SendChatMessageAsync(string roomId, ChatMessage chat) =>
            PostAsync($"{BaseUrl}/chat/{Uri.EscapeDataString(roomId)}.json", chat);
    // Thêm phương thức lấy tin nhắn chat
    public async Task<List<ChatMessage>?> GetChatMessagesAsync(string roomId)
    {
        string url = $"{BaseUrl}/chat/{Uri.EscapeDataString(roomId)}.json";
        var json = await SafeGetStringAsync(url);
        if (json is null) return null;
        var dict = JsonConvert.DeserializeObject<Dictionary<string, ChatMessage>>(json);
        return dict?.Values.OrderBy(c => c.Timestamp).ToList();
    }
    public async Task CleanupGameDataAsync(string roomId)
    {
        try
        {
            // Xóa game state
            string gameStateUrl = $"{BaseUrl}/gameStates/{roomId}.json";
            await _client.DeleteAsync(gameStateUrl);

            // Xóa chat
            string chatUrl = $"{BaseUrl}/chat/{roomId}.json";
            await _client.DeleteAsync(chatUrl);

            // Xóa phòng
            string roomUrl = $"{BaseUrl}/rooms/{roomId}.json";
            await _client.DeleteAsync(roomUrl);
        }
        catch (Exception ex)
        {
            Log($"Error cleaning up game data: {ex.Message}");
            throw;
        }
    }

    // Lấy thông tin session của một user
    public async Task<Dictionary<string, dynamic>> GetSessionAsync(string userId)
    {
        try
        {
            string url = $"{BaseUrl}/sessions/{userId}.json";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(json) || json == "null")
                return null;

            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
        }
        catch (Exception ex)
        {
            Log($"Error getting session: {ex.Message}");
            return null;
        }
    }

    // Tạo session mới
    public async Task CreateSessionAsync(string userId, object sessionData)
    {
        try
        {
            string url = $"{BaseUrl}/sessions/{userId}.json";
            var content = new StringContent(
                JsonConvert.SerializeObject(sessionData),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Log($"Error creating session: {ex.Message}");
            throw;
        }
    }

    // Cập nhật session
    public async Task UpdateSessionAsync(string userId, object sessionData)
    {
        try
        {
            string url = $"{BaseUrl}/sessions/{userId}.json";
            var content = new StringContent(
                JsonConvert.SerializeObject(sessionData),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _client.PatchAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Log($"Error updating session: {ex.Message}");
            throw;
        }
    }
    // Xóa session
    public async Task DeleteSessionAsync(string userId)
    {
        try
        {
            string url = $"{BaseUrl}/sessions/{userId}.json";
            var response = await _client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            Log($"Error deleting session: {ex.Message}");
            throw;
        }
    }
    // Lấy tất cả session đang active
    public async Task<Dictionary<string, dynamic>> GetActiveSessionsAsync()
    {
        try
        {
            string url = $"{BaseUrl}/sessions.json";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(json) || json == "null")
                return null;

            var allSessions = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

            // Lọc chỉ lấy các session đang active
            return allSessions?.Where(s => s.Value.isActive == true)
                             .ToDictionary(k => k.Key, v => v.Value);
        }
        catch (Exception ex)
        {
            Log($"Error getting active sessions: {ex.Message}");
            return null;
        }
    }
    // Kiểm tra session có hợp lệ không
    public async Task<bool> ValidateSessionAsync(string userId)
    {
        try
        {
            var session = await GetSessionAsync(userId);
            if (session == null)
                return false;

            // Kiểm tra session có active không
            if (!session.ContainsKey("isActive") || !session["isActive"])
                return false;

            // Kiểm tra thời gian hoạt động cuối
            if (session.ContainsKey("lastActive"))
            {
                var lastActive = DateTime.Parse(session["lastActive"].ToString());
                var timeout = TimeSpan.FromMinutes(30); // Session timeout sau 30 phút
                if (DateTime.UtcNow - lastActive > timeout)
                {
                    // Session hết hạn, cập nhật trạng thái
                    await UpdateSessionAsync(userId, new { isActive = false });
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Log($"Error validating session: {ex.Message}");
            return false;
        }
    }
    private async Task<string?> SafeGetStringAsync(string url)
    {
        var resp = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (!resp.IsSuccessStatusCode) return null;
        var json = await resp.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(json) || json == "null" ? null : json;
    }
    private static Task PutAsync(string url, object obj) =>
        _client.PutAsync(url, ToJsonContent(obj));
    private static Task PostAsync(string url, object obj) =>
        _client.PostAsync(url, ToJsonContent(obj));
    private static HttpContent ToJsonContent(object o) =>
        new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
    // For frameworks lacking PatchAsync
    private sealed class FullGameStateDto
    {
        public InfoDto info { get; set; } = default!;
        public List<Player>? players { get; set; }
        public List<Tile>? tiles { get; set; }
    }
    private sealed class InfoDto
    {
        public int currentPlayerIndex { get; set; }
        public bool isGameStarted { get; set; }
        public int playTime { get; set; }
        public string lastUpdateTime { get; set; } = string.Empty;
    }
    public sealed class ChatMessage
    {
        public string SenderName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("O");
    }
}