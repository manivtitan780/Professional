#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           IIPService.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          1-1-2024 16:6
// Last Updated On:     1-1-2024 16:6
// *****************************************/

#endregion

using System.Text.Json.Serialization;

namespace Profsvc_AppTrack.Components.Code;

public interface IIPService
{
	Task<string> GetIPAddressAsync();
}

public class IPAddress
{
	[JsonPropertyName("ip")]
	public string IP { get; set; }
}

public class ServerIPService(IHttpContextAccessor httpContextAccessor) : IIPService
{
	public Task<string> GetIPAddressAsync()
	{
		return Task.FromResult(httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
	}
}

// Client-side implementation
public class ClientIPService(HttpClient httpClient) : IIPService
{
	public async Task<string> GetIPAddressAsync()
	{
		IPAddress response = await httpClient.GetFromJsonAsync<IPAddress>("https://jsonip.com");
		return response.IP;
	}
}