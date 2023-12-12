#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_WebAPI
// File Name:           Extensions.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          01-26-2022 19:30
// Last Updated On:     08-29-2022 20:46
// *****************************************/

#endregion

namespace ProfSvc_WebAPI.Code;

public static partial class Extensions
{
    #region Properties

    /// <summary>
    ///     Checks if the string is null or blank.
    /// </summary>
    /// <param name="s"> String </param>
    /// <returns> Boolean </returns>
    public static bool NullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

    /// <summary>
    ///     Checks if the object is null or blank.
    /// </summary>
    /// <param name="o"> Object </param>
    /// <returns> Boolean </returns>
    public static bool NullOrWhiteSpace(this object o) => o == null || o.ToString().NullOrWhiteSpace();

    /// <summary>
    ///     Converts a string containing 0 or 1 to Boolean
    /// </summary>
    /// <param name="s">string which needs to be converted</param>
    /// <returns>Boolean</returns>
    public static bool StringToBoolean(this string s) => s == "1";

    /*/// <summary>
    ///     Set Value to DBNull.Value if Double Value is null or Optionally "0".
    /// </summary>
    /// <param name="s"> String whose property has to be checked. </param>
    /// <param name="isZero"> If true additionally checks if String Value equals "0". </param>
    /// <returns> String Value or DBNull.Value </returns>
    public static object DBNull(this double? s, bool isZero = false) =>
        isZero ? s == null || s == 0 ? (object) System.DBNull.Value : s : s ?? (object) System.DBNull.Value;*/

    public static int RandomNumber(this Page p, bool negative = true) => !negative ? RandomNumberGenerator.GetInt32(0, 5000) : RandomNumberGenerator.GetInt32(-5000, 5000);

    /// <summary>
    ///     Set Value to DBNull.Value if Integer Value is 0.
    /// </summary>
    /// <param name="i"> Integer whose property has to be checked. </param>
    /// <returns> Integer Value or DBNull.Value </returns>
    public static object DbNull(this double? i) => i == null || i == 0 ? DBNull.Value : i;

    /// <summary>
    ///     Set Value to DBNull.Value if Integer Value is 0.
    /// </summary>
    /// <param name="i"> Integer whose property has to be checked. </param>
    /// <returns> Integer Value or DBNull.Value </returns>
    public static object DbNull(this int i) => i == 0 ? DBNull.Value : i;

    /// <summary>
    ///     Set Value to DBNull.Value if String Value is Empty or Optionally "0".
    /// </summary>
    /// <param name="s"> String whose property has to be checked. </param>
    /// <param name="isZero"> If true additionally checks if String Value equals "0". </param>
    /// <returns> String Value or DBNull.Value </returns>
    public static object DbNull(this string s, bool isZero = false) => isZero ? string.IsNullOrWhiteSpace(s) || s.Trim() == "0" ? DBNull.Value : s.Trim() :
                                                                       string.IsNullOrWhiteSpace(s) ? DBNull.Value : s.Trim();

    /// <summary>
    ///     Converts a HTML-encoded string to a normal string.
    /// </summary>
    /// <param name="s"> The string to decode. </param>
    /// <returns> String </returns>
    public static string HtmlDecode(this string s) => HttpUtility.HtmlDecode(s);

    /// <summary>
    ///     Converts a string to a HTML-encoded string.
    /// </summary>
    /// <param name="s"> The string to encode. </param>
    /// <returns> String containing HTML encoded text </returns>
    public static string HtmlEncode(this string s) => HttpUtility.HtmlEncode(s);

    public static string ToBase64ByteArray(this Stream s, out byte[] byteArray)
    {
        string _base64String;
        byte[] _binaryData;

        try
        {
            using BinaryReader _reader = new(s);
            _binaryData = _reader.ReadBytes((int)s.Length);
        }
        catch (Exception _exp)
        {
            byteArray = null;

            return _exp.Message;
        }

        try
        {
            _base64String = Convert.ToBase64String(_binaryData, 0, _binaryData.Length);
        }
        catch (Exception _exp)
        {
            byteArray = null;

            return _exp.Message;
        }

        try
        {
            byteArray = Convert.FromBase64String(_base64String);
        }
        catch (Exception _exp)
        {
            byteArray = null;

            return _exp.Message;
        }

        return ""; //no error message on return
    }

    /// <summary>
    ///     Decodes a string from URL Safe string.
    /// </summary>
    /// <param name="s"> String </param>
    /// <returns> string </returns>
    public static string UrlDecode(this string s) => HttpUtility.UrlDecode(s);

    /// <summary>
    ///     Encodes a string to URL Safe string.
    /// </summary>
    /// <param name="s"> String </param>
    /// <returns> string </returns>
    public static string UrlEncode(this string s) => HttpUtility.UrlEncode(s);

    /// <summary>
    ///     Strips non-numerical characters from the string.
    /// </summary>
    /// <param name="s">String to strip characters from.</param>
    /// <returns></returns>
    public static string StripPhoneNumber(this string s) => Regex.Replace(s, "[^0-9]", string.Empty);

    /// <summary>
    ///     Formats String to US Phone Number format.
    /// </summary>
    /// <param name="s">String to format.</param>
    /// <returns></returns>
    public static string FormatPhoneNumber(this string s) => s.ToInt64() > 0 ? $"{s.ToInt64():(###) ###-####}" : "";

    /// <summary>
    ///     Formats Date to default US short date.
    /// </summary>
    /// <param name="d">Date to format.</param>
    /// <param name="format">Format string.</param>
    /// <param name="c">Culture to use to format.</param>
    /// <returns></returns>
    public static string CultureDate(this DateTime d, string format = "d", CultureInfo c = null)
    {
        c ??= new("en-us");

        return d.ToString(format, c);
    }

    #endregion
}