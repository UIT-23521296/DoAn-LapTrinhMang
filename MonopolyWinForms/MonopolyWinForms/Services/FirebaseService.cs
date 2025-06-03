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

public class FirebaseService
{
    private readonly HttpClient _client;
    private readonly string baseUrl = "https://doanmang-8f5af-default-rtdb.asia-southeast1.firebasedatabase.app";
    private Action<string> _logAction;

    public FirebaseService(Action<string> logAction = null)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client.Timeout = TimeSpan.FromSeconds(30);
        _logAction = logAction;
    }

    private void Log(string message)
    {
        _logAction?.Invoke(message);
    }

    public async Task<RoomInfo> GetRoomAsync(string roomId)
    {
        string url = $"{baseUrl}/rooms/{roomId}.json";
        var response = await _client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        string json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(json) || json == "null")
            return null;

        var room = JsonConvert.DeserializeObject<RoomInfo>(json);
        return room;
    }

    public async Task<Dictionary<string, RoomInfo>> GetAllRoomsAsync()
    {
        string url = $"{baseUrl}/rooms.json";
        var response = await _client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        string json = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(json) || json == "null")
            return null;

        return JsonConvert.DeserializeObject<Dictionary<string, RoomInfo>>(json);
    }
    public async Task CreateRoomAsync(string roomId, RoomInfo room)
    {
        string url = $"{baseUrl}/rooms/{roomId}.json";
        string json = JsonConvert.SerializeObject(room);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRoomAsync(string roomId)
    {
        string url = $"{baseUrl}/rooms/{roomId}.json";
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    public async Task<GameState> GetGameStateAsync(string roomId)
    {
        try
        {
            // 1. Lấy thông tin cơ bản
            string infoUrl = $"{baseUrl}/gameStates/{roomId}/info.json";
            var infoResponse = await _client.GetAsync(infoUrl);
            if (!infoResponse.IsSuccessStatusCode)
            {
                string errorContent = await infoResponse.Content.ReadAsStringAsync();
                File.AppendAllText("log.txt", $"Error getting info: {errorContent}\n");
                return null;
            }
            string infoJson = await infoResponse.Content.ReadAsStringAsync();
            var info = JsonConvert.DeserializeObject<dynamic>(infoJson);

            // Kiểm tra xem có cần cập nhật không
            if (info == null || info.lastUpdateTime == null)
            {
                return null;
            }

            // Tạo GameState mới với thông tin cơ bản
            var gameState = new GameState
            {
                RoomId = roomId,
                CurrentPlayerIndex = info.currentPlayerIndex,
                IsGameStarted = info.isGameStarted,
                PlayTime = info.PlayTime,
                LastUpdateTime = info.lastUpdateTime
            };

            // 2. Lấy thông tin players
            string playersUrl = $"{baseUrl}/gameStates/{roomId}/players.json";
            var playersResponse = await _client.GetAsync(playersUrl);
            if (!playersResponse.IsSuccessStatusCode)
            {
                string errorContent = await playersResponse.Content.ReadAsStringAsync();
                File.AppendAllText("log.txt", $"Error getting players: {errorContent}\n");
                return null;
            }
            string playersJson = await playersResponse.Content.ReadAsStringAsync();
            var players = JsonConvert.DeserializeObject<List<Player>>(playersJson);
            gameState.Players = players;

            // 3. Lấy thông tin tiles
            string tilesUrl = $"{baseUrl}/gameStates/{roomId}/tiles.json";
            var tilesResponse = await _client.GetAsync(tilesUrl);
            if (!tilesResponse.IsSuccessStatusCode)
            {
                string errorContent = await tilesResponse.Content.ReadAsStringAsync();
                File.AppendAllText("log.txt", $"Error getting tiles: {errorContent}\n");
                return null;
            }
            string tilesJson = await tilesResponse.Content.ReadAsStringAsync();
            var tiles = JsonConvert.DeserializeObject<List<Tile>>(tilesJson);
            gameState.Tiles = tiles;

            return gameState;
        }
        catch (Exception ex)
        {
            File.AppendAllText("log.txt", $"Exception in GetGameStateAsync: {ex.Message}\n");
            throw;
        }
    }

    public async Task UpdateGameStateAsync(GameState gameState)
    {
        try
        {
            // Chỉ cập nhật khi có thay đổi
            var currentState = await GetGameStateAsync(gameState.RoomId);
            if (currentState != null && 
                currentState.LastUpdateTime == gameState.LastUpdateTime)
            {
                return; // Không có thay đổi, không cần cập nhật
            }

            var roomInfo = new
            {
                currentPlayerIndex = gameState.CurrentPlayerIndex,
                isGameStarted = gameState.IsGameStarted,
                PlayTime = gameState.PlayTime,
                lastUpdateTime = DateTime.UtcNow
            };

            // Gửi thông tin phòng
            string roomUrl = $"{baseUrl}/gameStates/{gameState.RoomId}/info.json";
            var response0 = await _client.PutAsync(roomUrl, new StringContent(JsonConvert.SerializeObject(roomInfo)));

            // Gửi thông tin người chơi dưới dạng mảng
            string playersUrl = $"{baseUrl}/gameStates/{gameState.RoomId}/players.json";
            var response1 = await _client.PutAsync(playersUrl, new StringContent(JsonConvert.SerializeObject(gameState.Players)));

            // Gửi thông tin ô đất dưới dạng mảng
            string tilesUrl = $"{baseUrl}/gameStates/{gameState.RoomId}/tiles.json";
            var response2 = await _client.PutAsync(tilesUrl, new StringContent(JsonConvert.SerializeObject(gameState.Tiles)));

            if (!response0.IsSuccessStatusCode || !response1.IsSuccessStatusCode || !response2.IsSuccessStatusCode)
            {
                string errorContent = await response0.Content.ReadAsStringAsync();
                File.AppendAllText("log.txt", $"Error updating game state: {errorContent}\n");
            }
        }
        catch (Exception ex)
        {
            File.AppendAllText("log.txt", $"Error in FirebaseService.UpdateGameStateAsync: {ex.Message}\n");
        }
    }

    // Thêm phương thức gửi tin nhắn chat
    public async Task SendChatMessageAsync(string roomId, object chatMessage)
    {
        try
        {
            string url = $"{baseUrl}/chat/{roomId}.json";
            var content = new StringContent(JsonConvert.SerializeObject(chatMessage), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            File.AppendAllText("log.txt", $"Error sending chat message: {ex.Message}\n");
            throw;
        }
    }

    // Thêm phương thức lấy tin nhắn chat
    public async Task<Dictionary<string, dynamic>> GetChatMessagesAsync(string roomId)
    {
        try
        {
            string url = $"{baseUrl}/chat/{roomId}.json";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            string json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(json) || json == "null")
                return null;

            // Thêm timestamp nếu chưa có
            var messages = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    if (message.Value.Timestamp == null)
                    {
                        message.Value.Timestamp = DateTime.UtcNow;
                    }
                }
            }

            return messages;
        }
        catch (Exception ex)
        {
            File.AppendAllText("log.txt", $"Error getting chat messages: {ex.Message}\n");
            return null;
        }
    }

    public async Task CleanupGameDataAsync(string roomId)
    {
        try
        {
            // Xóa game state
            string gameStateUrl = $"{baseUrl}/gameStates/{roomId}.json";
            await _client.DeleteAsync(gameStateUrl);

            // Xóa chat
            string chatUrl = $"{baseUrl}/chat/{roomId}.json";
            await _client.DeleteAsync(chatUrl);

            // Xóa phòng
            string roomUrl = $"{baseUrl}/rooms/{roomId}.json";
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
            string url = $"{baseUrl}/sessions/{userId}.json";
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
            string url = $"{baseUrl}/sessions/{userId}.json";
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
            string url = $"{baseUrl}/sessions/{userId}.json";
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
            string url = $"{baseUrl}/sessions/{userId}.json";
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
            string url = $"{baseUrl}/sessions.json";
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
}
