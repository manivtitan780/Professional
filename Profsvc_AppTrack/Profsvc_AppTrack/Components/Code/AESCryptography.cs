#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           AESCryptography.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-16-2023 20:23
// Last Updated On:     12-11-2023 21:12
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Code;

/// <summary>
///     Summary description for TripleDES
/// </summary>
public class AESCryptography
{
    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    public AESCryptography(byte[] key, byte[] iv)
    {
        _aes.Key = key;
        _aes.IV = iv;
    }

    /// <summary>
    /// </summary>
    /// <param name="key">32-character Key string (256 bits)</param>
    /// <param name="iv">16-character Initialization Vector (128 bits)</param>
    public AESCryptography(string key, string iv)
    {
        _aes.Key = Encoding.ASCII.GetBytes(key);
        _aes.IV = Encoding.ASCII.GetBytes(iv);
    }

    public AESCryptography()
    {
        _aes.Key = Encoding.ASCII.GetBytes("~1@3$5^7*9)-+QwErTyUiOpAsDfGhJkL");
        _aes.IV = Encoding.ASCII.GetBytes("ZxCvBnM,>':[};<=");
    }

    private readonly Aes _aes = Aes.Create();

    private readonly UTF8Encoding _encoder = new();

    /// <summary>
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public byte[] Decrypt(byte[] input) => Transform(input, _aes.CreateDecryptor());

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string Decrypt(string text)
    {
        byte[] _input = Convert.FromBase64String(text);
        byte[] _bytes = Transform(_input, _aes.CreateDecryptor());

        return _encoder.GetString(_bytes);
    }

    /// <summary>
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public byte[] Encrypt(byte[] input) => Transform(input, _aes.CreateEncryptor());

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public string Encrypt(string text)
    {
        byte[] _bytes = _encoder.GetBytes(text);

        return Convert.ToBase64String(Transform(_bytes, _aes.CreateEncryptor()));
    }

    private static byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
    {
        using MemoryStream _stream2 = new();
        using CryptoStream _stream = new(_stream2, cryptoTransform, CryptoStreamMode.Write);
        _stream.Write(input, 0, input.Length);
        _stream.FlushFinalBlock();
        _stream2.Position = 0L;
        byte[] _buffer = new byte[(int)(_stream2.Length - 1L) + 1];
        _ = _stream2.Read(_buffer, 0, _buffer.Length);

        return _buffer;
    }
}