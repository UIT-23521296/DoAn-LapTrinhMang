using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using MonopolyWinForms.Room;

public class FirebaseService
{
    private readonly HttpClient _client;
    private readonly string baseUrl = "https://doanmang-8f5af-default-rtdb.asia-southeast1.firebasedatabase.app";

    public FirebaseService()
    {
        _client = new HttpClient();
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
}
