#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           RChilliParseResume.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          04-24-2023 20:59
// Last Updated On:     04-24-2023 21:27
// *****************************************/

#endregion

#region Using

using System.Net;

using Newtonsoft.Json;

using FileInfo = System.IO.FileInfo;

#endregion

namespace ProfSvc_AppTrack.Code;

public class RChilliParseResume
{
	public string ErrorCode
	{
		get;
		set;
	}

	public string ErrorMessage
	{
		get;
		set;
	}

	public bool IsError
	{
		get;
		set;
	}

	public string OutputJson
	{
		get;
		set;
	}

	public string ServiceUrl
	{
		get;
		set;
	}

	public byte[] ConvertToBase64(FileInfo fno)
	{
		try
		{
			long _numOfByte = fno.Length;
			FileStream fs = new(fno.FullName, FileMode.Open);
			BinaryReader br = new(fs);
			byte[] _dataFile = br.ReadBytes(Convert.ToInt32(_numOfByte));
			fs.Close();
			fs.Dispose();
			return _dataFile;
		}
		catch (Exception ex)
		{
			IsError = true;
			ErrorCode = "5000";
			ErrorMessage = "Exception occurred" + ex.Message;
		}

		byte[] error1 = new byte[1];
		error1[0] = (byte)' ';
		return error1;
	}

	public RChilliMapFields ParseResume(string jsonFile)
	{
		RChilliMapFields _obj = null;
		try
		{
			string _jsonContent = File.ReadAllText(jsonFile);
			_obj = JsonConvert.DeserializeObject<RChilliMapFields>(_jsonContent);
		}
		catch
		{
			//
		}
		return _obj;
	}

	public async Task<RChilliMapFields> ParseResume(string filePath, string userKey, string version, string subUserId)
	{
		try
		{
			FileInfo file = new(filePath);
			byte[] _dataFile = ConvertToBase64(file);
			OutputJson = await CallJavaApi(Convert.ToBase64String(_dataFile), file.Name, userKey, version, subUserId);
			if (OutputJson.Contains("\"error\":"))
			{
				IsError = true;
				ErrorClass _errorObj = JsonConvert.DeserializeObject<ErrorClass>(OutputJson);
				Error error = _errorObj.Error;
				ErrorCode = error.ErrorCode.ToString();
				ErrorMessage = error.ErrorMsg;
			}
			else
			{
				RChilliMapFields obj = JsonConvert.DeserializeObject<RChilliMapFields>(OutputJson);
				return obj;
			}
		}
		catch (Exception ex)
		{
			ErrorCode = "5001";
			ErrorMessage = ex.Message;
		}

		return new();
	}

	public async Task<RChilliMapFields> ParseResume(byte[] filePath, string fileName, string userKey, string version, string subUserId)
	{
		try
		{
			//FileInfo file = new(filePath);
			//byte[] _dataFile = ConvertToBase64(file);
			OutputJson = await CallJavaApi(Convert.ToBase64String(filePath), fileName, userKey, version, subUserId);
			if (OutputJson.Contains("\"error\":"))
			{
				IsError = true;
				ErrorClass _errorObj = JsonConvert.DeserializeObject<ErrorClass>(OutputJson);
				Error error = _errorObj.Error;
				ErrorCode = error.ErrorCode.ToString();
				ErrorMessage = error.ErrorMsg;
			}
			else
			{
				RChilliMapFields obj = JsonConvert.DeserializeObject<RChilliMapFields>(OutputJson);
				return obj;
			}
		}
		catch (Exception ex)
		{
			ErrorCode = "5001";
			ErrorMessage = ex.Message;
		}

		return new();
	}

	//private string CallJavaApi(string base64String, string fileName, string userKey, string version, string subUserId)
	//{
	//	ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
	//	string _strRequest = "{\"filedata\":\"" + base64String + "\",\"filename\":\"" + fileName + "\",\"userkey\":\"" + userKey + "\",\"version\":\"" + version + "\",\"subuserid\":\"" + subUserId +
	//						 "\"}";
	//	byte[] _byteArray = Encoding.UTF8.GetBytes(_strRequest);
	//	HttpWebRequest _httpRequest = (HttpWebRequest)WebRequest.Create(ServiceUrl);
	//	_httpRequest.Credentials = CredentialCache.DefaultCredentials;
	//	_httpRequest.Method = "POST";
	//	_httpRequest.ContentType = "application/json";
	//	_httpRequest.ContentLength = _byteArray.Length;
	//	_httpRequest.Timeout = 300000;
	//	Stream _dataStream = _httpRequest.GetRequestStream();
	//	_dataStream.Write(_byteArray, 0, _byteArray.Length);
	//	_dataStream.Close();

	//	HttpWebResponse _httpResponse = (HttpWebResponse)_httpRequest.GetResponse();
	//	StreamReader _streamReader = new(_httpResponse.GetResponseStream()!, Encoding.UTF8);
	//	string _jsonData = _streamReader.ReadToEnd();
	//	return _jsonData;
	//}

	private async Task<string> CallJavaApi(string base64String, string fileName, string userKey, string version, string subUserId)
	{
		ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
		string _strRequest = $@"{{""filedata"":""{base64String}"",""filename"":""{fileName}"",""userkey"":""{userKey}"",""version"":""{version}"",""subuserid"":""{subUserId}""}}";
		byte[] _byteArray = Encoding.UTF8.GetBytes(_strRequest);
		HttpClientHandler _httpClientHandler = new()
											   {
												   Credentials = CredentialCache.DefaultCredentials
											   };
		HttpClient _httpClient = new(_httpClientHandler)
								 {
									 Timeout = TimeSpan.FromMinutes(5)
								 };
		HttpRequestMessage _httpRequestMessage = new(HttpMethod.Post, ServiceUrl)
												 {
													 Content = new StringContent(_strRequest, Encoding.UTF8, "application/json"),
												 };
		_httpRequestMessage.Content.Headers.ContentLength = _byteArray.Length;
		HttpResponseMessage _httpResponseMessage = await _httpClient.SendAsync(_httpRequestMessage);
		string _jsonData = await _httpResponseMessage.Content.ReadAsStringAsync();
		return _jsonData;
	}

}