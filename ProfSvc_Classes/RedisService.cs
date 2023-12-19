﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           RedisService.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-15-2023 19:38
// Last Updated On:     12-19-2023 19:53
// *****************************************/

#endregion

#region Using

using Newtonsoft.Json;

using StackExchange.Redis;

#endregion

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

namespace ProfSvc_Classes;

/// <summary>
///     Provides services for interacting with a Redis database.
/// </summary>
/// <remarks>
///     This class is responsible for establishing a connection with a Redis database and providing methods to interact
///     with it.
/// </remarks>
public class RedisService
{
    /// <summary>
    ///     Initializes a new instance of the RedisService class.
    /// </summary>
    /// <param name="hostName">The hostname of the Redis server.</param>
    /// <param name="sslPort">The SSL port to connect to the Redis server.</param>
    /// <param name="access">The access password for the Redis server.</param>
    /// <remarks>
    ///     This constructor establishes a secure connection to the Redis server using the provided hostname, SSL port, and
    ///     access password.
    /// </remarks>
    public RedisService(string hostName, int sslPort, string access)
    {
        ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect($"{hostName}:{sslPort},password={access},ssl=True,abortConnect=False");
        _db = _redis.GetDatabase();
    }

    private readonly IDatabase _db;

    /// <summary>
    ///     Checks if a key exists in the Redis database.
    /// </summary>
    /// <param name="key">The key to check in the Redis database.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a boolean value indicating whether
    ///     the key exists.
    /// </returns>
    public Task<bool> CheckKeyExists(string key) => _db.KeyExistsAsync(key);

    /// <summary>
    ///     Asynchronously creates a batch set in Redis.
    /// </summary>
    /// <param name="keyArray">A list of keys for the items to be set.</param>
    /// <param name="items">A list of items to be set in Redis.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method will serialize each item in the items list and set it in Redis with the corresponding key from the
    ///     keyArray list.
    ///     The items will be set only if the key does not already exist in Redis.
    ///     Each item will have an expiration time of 3650 days.
    /// </remarks>
    public async Task CreateBatchSet(List<string> keyArray, List<object> items)
    {
        ITransaction _transaction = _db.CreateTransaction();
        for (int _i = 0; _i < items.Count; _i++)
        {
            await _transaction.StringSetAsync(keyArray[_i], JsonConvert.SerializeObject(items[_i]), TimeSpan.FromDays(3650), When.NotExists);
        }

        await _transaction.ExecuteAsync();
    }

    /// <summary>
    ///     Retrieves the value associated with the specified key from the Redis database. If the key does not exist, returns
    ///     the default value of the type parameter.
    /// </summary>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <returns>The value associated with the specified key, if it exists; otherwise, the default value of the type parameter.</returns>
    public async Task<T> GetAsync<T>(string key)
    {
        RedisValue _value = await _db.StringGetAsync(key);

        return _value.HasValue ? JsonConvert.DeserializeObject<T>(_value.ToString()) : default;
    }

    /// <summary>
    ///     Retrieves the value associated with the specified key from the Redis database. If the key does not exist, creates a
    ///     new item with the specified key and value.
    /// </summary>
    /// <param name="key">The key of the value to retrieve or create.</param>
    /// <param name="createItems">The value to create if the key does not exist.</param>
    /// <typeparam name="T">The type of the value to retrieve or create.</typeparam>
    /// <returns>The value associated with the specified key, if it exists; otherwise, the newly created value.</returns>
    /// <remarks>
    ///     If the key exists in the Redis database, this method will deserialize the value associated with the key and return
    ///     it.
    ///     If the key does not exist, this method will serialize the createItems parameter, set it in the Redis database with
    ///     the specified key, and return createItems.
    ///     The newly created item will have an expiration time of 3650 days.
    /// </remarks>
    public async Task<T> GetOrCreateAsync<T>(string key, T createItems)
    {
        RedisValue _value = await _db.StringGetAsync(key);

        if (_value.HasValue)
        {
            return JsonConvert.DeserializeObject<T>(_value.ToString());
        }

        await _db.StringSetAsync(key, JsonConvert.SerializeObject(createItems), TimeSpan.FromDays(3650), When.Always);
        return createItems;
    }

    /// <summary>
    ///     Asynchronously refreshes the value associated with the specified key in the Redis database.
    /// </summary>
    /// <param name="key">The key of the value to refresh.</param>
    /// <param name="createItems">The new value to set for the specified key.</param>
    /// <typeparam name="T">The type of the value to refresh.</typeparam>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method will serialize the provided list of items and set it in Redis with the specified key.
    ///     The new value will replace the existing value associated with the key.
    ///     The value will have an expiration time of 3650 days.
    /// </remarks>
    public async Task RefreshAsync<T>(string key, T createItems) => 
        await _db.StringSetAsync(key, JsonConvert.SerializeObject(createItems), TimeSpan.FromDays(3650), When.Always);
}