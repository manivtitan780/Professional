using Newtonsoft.Json;

using StackExchange.Redis;
// ReSharper disable UnusedMember.Global

namespace ProfSvc_Classes;

public class RedisService
{
    private readonly IDatabase _db;

    public RedisService(string hostName, int sslPort, string access)
    {
        ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect($"{hostName}:{sslPort},password={access},ssl=True,abortConnect=False");
        _db = _redis.GetDatabase();
    }

    public Task<bool> CheckKeyExists(string key)
    {
        return _db.KeyExistsAsync(key);
    }
    
    public async Task<T> GetOrCreateAsync<T>(string key, T createItems)
    {
        RedisValue _value = await _db.StringGetAsync(key);

        if (_value.HasValue)
        {
            return JsonConvert.DeserializeObject<T>(_value.ToString());
        }

        await _db.StringSetAsync(key, JsonConvert.SerializeObject(createItems), TimeSpan.FromHours(24), When.Always);
        return createItems;

    }

    public async Task RefreshAsync<T>(string key, List<T> createItems)
    {
        await _db.StringSetAsync(key, JsonConvert.SerializeObject(createItems), TimeSpan.FromHours(24));
    }
}