#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             LabelComponents
// File Name:           Extensions.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 18:50
// Last Updated On:     12-11-2023 21:10
// *****************************************/

#endregion

namespace LabelComponents;

/// <summary>
///     Provides extension methods for string and object manipulation.
/// </summary>
public static class Extensions
{
    /// <summary>
    ///     Determines whether the specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="s">The string to test.</param>
    /// <returns>true if the string is null or whitespace; otherwise, false.</returns>
    public static bool NullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);

    /// <summary>
    ///     Checks if the object is null or its string representation is whitespace.
    /// </summary>
    /// <param name="o">The object to be checked.</param>
    /// <returns>True if the object is null or its string representation is whitespace, otherwise false.</returns>
    public static bool NullOrWhiteSpace(this object o) => o == null || o.ToString().NullOrWhiteSpace();

    /// <summary>
    ///     Strips non-numeric characters from a phone number.
    /// </summary>
    /// <param name="s">The string containing the phone number to be stripped.</param>
    /// <returns>A string containing only the numeric characters from the input phone number.</returns>
    public static string StripPhoneNumber(this string s) => Regex.Replace(s, "[^0-9]", string.Empty);
}