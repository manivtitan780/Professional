#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           AESCryptography.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-23-2022 16:15
// Last Updated On:     10-24-2023 21:22
// *****************************************/

#endregion

#region Using

using System.Security.Cryptography;
using System.Text;

#endregion

namespace ProfSvc_Classes;

/// <summary>
///     Summary description for TripleDES
/// </summary>
public class AESCryptography
{
	/// <summary>
	///     Initializes a new instance of the AESCryptography class with the specified key and initialization vector.
	/// </summary>
	/// <param name="key">The secret key for the AES encryption algorithm. This key is used both for encryption and decryption.</param>
	/// <param name="iv">
	///     The initialization vector for the AES encryption algorithm. This vector is used to increase security
	///     of encryption.
	/// </param>
	public AESCryptography(byte[] key, byte[] iv)
	{
		_aes.Key = key;
		_aes.IV = iv;
	}

	/// <summary>
	///     Initializes a new instance of the AESCryptography class with the specified key and initialization vector.
	/// </summary>
	/// <param name="key">
	///     The secret key for the AES encryption algorithm. This key is used both for encryption and decryption.
	///     The key is converted from a string to a byte array using ASCII encoding.
	/// </param>
	/// <param name="iv">
	///     The initialization vector for the AES encryption algorithm. This vector is used to increase security
	///     of encryption. The initialization vector is converted from a string to a byte array using ASCII encoding.
	/// </param>
	public AESCryptography(string key, string iv)
	{
		_aes.Key = Encoding.ASCII.GetBytes(key);
		_aes.IV = Encoding.ASCII.GetBytes(iv);
	}

	/// <summary>
	///     Initializes a new instance of the AESCryptography class with a predefined key and initialization vector.
	/// </summary>
	/// <remarks>
	///     The key and initialization vector are hardcoded and are used for AES encryption and decryption.
	///     This constructor should be used when a specific key and initialization vector are not provided.
	/// </remarks>
	public AESCryptography()
	{
		_aes.Key = "~1@3$5^7*9)-+QwErTyUiOpAsDfGhJkL"u8.ToArray();
		_aes.IV = "ZxCvBnM,>':[};<="u8.ToArray();
	}

	private readonly Aes _aes = Aes.Create();

	private readonly UTF8Encoding _encoder = new();

	/// <summary>
	///     Decrypts the specified input byte array using the AES encryption algorithm.
	/// </summary>
	/// <param name="input">The input byte array to be decrypted.</param>
	/// <returns>The decrypted byte array.</returns>
	public byte[] Decrypt(byte[] input)
	{
		return Transform(input, _aes.CreateDecryptor());
	}

	/// <summary>
	///     Decrypts the specified input string using the AES encryption algorithm.
	/// </summary>
	/// <param name="text">
	///     The input string to be decrypted. The string is converted from a base64 string to a byte array
	///     before decryption.
	/// </param>
	/// <returns>The decrypted string.</returns>
	public string Decrypt(string text)
	{
		byte[] _input = Convert.FromBase64String(text);
		byte[] _bytes = Transform(_input, _aes.CreateDecryptor());

		return _encoder.GetString(_bytes);
	}

	/// <summary>
	///     Encrypts the specified input byte array using the AES encryption algorithm.
	/// </summary>
	/// <param name="input">The input byte array to be encrypted.</param>
	/// <returns>The encrypted byte array.</returns>
	public byte[] Encrypt(byte[] input)
	{
		return Transform(input, _aes.CreateEncryptor());
	}

	/// <summary>
	///     Encrypts the specified input string using the AES encryption algorithm.
	/// </summary>
	/// <param name="text">
	///     The input string to be encrypted. The string is converted to a byte array using UTF8 encoding before
	///     encryption.
	/// </param>
	/// <returns>The encrypted string, represented as a base64 string.</returns>
	public string Encrypt(string text)
	{
		byte[] _bytes = _encoder.GetBytes(text);

		return Convert.ToBase64String(Transform(_bytes, _aes.CreateEncryptor()));
	}

	/// <summary>
	///     Transforms the specified input byte array using the provided cryptographic transform.
	/// </summary>
	/// <param name="input">The input byte array to be transformed.</param>
	/// <param name="cryptoTransform">
	///     The cryptographic transform to be used for the transformation. This can be either an
	///     encryptor or decryptor.
	/// </param>
	/// <returns>The transformed byte array.</returns>
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