using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using MonopolyWinForms.Room;

public class FirebaseService
{
    private readonly HttpClient _httpClient;
    private readonly string _firebaseUrl = "https://doanmang-8f5af-default-rtdb.asia-southeast1.firebasedatabase.app";

    public FirebaseService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<bool> CreateRoomAsync(string roomId, object room)
    {
        string url = $"{_firebaseUrl}/rooms/{roomId}.json";
        string json = JsonConvert.SerializeObject(room);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(url, content);
        return response.IsSuccessStatusCode;
    }

    public async Task<Dictionary<string, RoomInfo>> GetAllRoomsAsync()
    {
        string url = $"{_firebaseUrl}/rooms.json";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        string json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, RoomInfo>>(json);
    }
}
