﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           Extensions.To.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     08-22-2023 15:02
// *****************************************/

#endregion

#region Using

using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;

#endregion

namespace ProfSvc_Classes;

public static partial class Extensions
{
    /// <summary>
    ///     Returns a boolean value from the SqlDataReader based on the provided index.
    ///     If the value is DBNull, the method will return the provided nullReplaceValue.
    /// </summary>
    /// <param name="read">The SqlDataReader instance.</param>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <param name="nullReplaceValue">The value to return if the column value is DBNull. Default is false.</param>
    /// <returns>The boolean value of the specified column or nullReplaceValue if the column value is DBNull.</returns>
    public static bool NBoolean(this SqlDataReader read, int index, bool nullReplaceValue = false) => read.IsDBNull(index) ? nullReplaceValue : read.GetBoolean(index);

    /// <summary>
    ///     Returns a DateTime value from the SqlDataReader based on the provided index.
    ///     If the value is DBNull, the method will return the provided nullReplaceValue.
    /// </summary>
    /// <param name="read">The SqlDataReader instance.</param>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <param name="nullReplaceValue">The value to return if the column value is DBNull. Default is DateTime default value.</param>
    /// <returns>The DateTime value of the specified column or nullReplaceValue if the column value is DBNull.</returns>
    public static DateTime NDateTime(this SqlDataReader read, int index, DateTime nullReplaceValue = default) => read.IsDBNull(index) ? nullReplaceValue : read.GetDateTime(index);

    /// <summary>
    ///     Returns a decimal value from the SqlDataReader based on the provided index.
    ///     If the value is DBNull, the method will return the provided nullReplaceValue.
    /// </summary>
    /// <param name="read">The SqlDataReader instance.</param>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <param name="nullReplaceValue">The value to return if the column value is DBNull. Default is 0.</param>
    /// <returns>The decimal value of the specified column or nullReplaceValue if the column value is DBNull.</returns>
    public static decimal NDecimal(this SqlDataReader read, int index, decimal nullReplaceValue = 0) => read.IsDBNull(index) ? nullReplaceValue : read.GetDecimal(index);

    /// <summary>
    ///     Returns a short integer value from the SqlDataReader based on the provided index.
    ///     If the value is DBNull, the method will return the provided nullReplaceValue.
    /// </summary>
    /// <param name="read">The SqlDataReader instance.</param>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <param name="nullReplaceValue">The value to return if the column value is DBNull. Default is 0.</param>
    /// <returns>The short integer value of the specified column or nullReplaceValue if the column value is DBNull.</returns>
    public static short NInt16(this SqlDataReader read, int index, short nullReplaceValue = 0) => read.IsDBNull(index) ? nullReplaceValue : read.GetInt16(index);

    /// <summary>
    ///     Returns an integer value from the SqlDataReader based on the provided index.
    ///     If the value is DBNull, the method will return the provided nullReplaceValue.
    /// </summary>
    /// <param name="read">The SqlDataReader instance.</param>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <param name="nullReplaceValue">The value to return if the column value is DBNull. Default is 0.</param>
    /// <returns>The integer value of the specified column or nullReplaceValue if the column value is DBNull.</returns>
    public static int NInt32(this SqlDataReader read, int index, int nullReplaceValue = 0) => read.IsDBNull(index) ? nullReplaceValue : read.GetInt32(index);

    /// <summary>
    ///     Returns a string value from the SqlDataReader based on the provided index.
    ///     If the value is DBNull or an empty string (if checkEmptyString is true), the method will return the provided
    ///     nullReplaceValue.
    /// </summary>
    /// <param name="read">The SqlDataReader instance.</param>
    /// <param name="index">The zero-based column ordinal.</param>
    /// <param name="nullReplaceValue">
    ///     The value to return if the column value is DBNull or an empty string. Default is an
    ///     empty string.
    /// </param>
    /// <param name="checkEmptyString">A boolean value indicating whether to check for empty strings. Default is false.</param>
    /// <returns>
    ///     The string value of the specified column or nullReplaceValue if the column value is DBNull or an empty string
    ///     (if checkEmptyString is true).
    /// </returns>
    public static string NString(this SqlDataReader read, int index, string nullReplaceValue = "", bool checkEmptyString = false) =>
        checkEmptyString ? read.IsDBNull(index) || read.GetString(index) == string.Empty ? nullReplaceValue : read.GetString(index).Trim() :
        read.IsDBNull(index) ? nullReplaceValue : read.GetString(index).Trim();

    /// <summary>
    ///     Converts the string representation of a boolean value to its boolean equivalent.
    /// </summary>
    /// <param name="s">A string containing the value to convert.</param>
    /// <returns>
    ///     A boolean value that is equivalent to the boolean value contained in the string. If the string is null or
    ///     empty, or does not represent a valid boolean value, the method returns false.
    /// </returns>
    public static bool ToBoolean(this string s) => !string.IsNullOrEmpty(s) && bool.TryParse(s, out bool _outDate) && _outDate;

    /// <summary>
    ///     Converts the object representation of a boolean value to its boolean equivalent.
    /// </summary>
    /// <param name="o">An object containing the value to convert.</param>
    /// <returns>
    ///     A boolean value that is equivalent to the boolean value contained in the object. If the object is null, or
    ///     does not represent a valid boolean value, the method returns false.
    /// </returns>
    public static bool ToBoolean(this object o)
    {
        if (o is bool _b)
        {
            return _b;
        }

        return o != null && bool.TryParse(o.ToString(), out bool _outDate) && _outDate;
    }

    /// <summary>
    ///     Converts the boolean value to its string equivalent.
    /// </summary>
    /// <param name="b">The boolean value to convert.</param>
    /// <returns>
    ///     A string that represents the boolean value. If the boolean value is true, the method returns "true". If the
    ///     boolean value is false, the method returns "false".
    /// </returns>
    public static string ToBooleanString(this bool b) => b ? "true" : "false";

    /// <summary>
    ///     Converts the string representation of a byte value to its byte equivalent.
    /// </summary>
    /// <param name="s">A string containing the value to convert.</param>
    /// <param name="nullValue">
    ///     The value to return if the string is null or empty, or does not represent a valid byte value.
    ///     Default is 0.
    /// </param>
    /// <returns>
    ///     A byte value that is equivalent to the byte value contained in the string. If the string is null or empty, or
    ///     does not represent a valid byte value, the method returns the nullValue.
    /// </returns>
    public static byte ToByte(this string s, byte nullValue = 0) => string.IsNullOrEmpty(s) ? nullValue : byte.TryParse(s, out byte _outInt) ? _outInt : nullValue;

    /// <summary>
    ///     Converts the object representation of a DateTime value to its DateTime equivalent.
    /// </summary>
    /// <param name="o">An object containing the value to convert.</param>
    /// <returns>
    ///     A DateTime value that is equivalent to the DateTime value contained in the object. If the object is null, or
    ///     does not represent a valid DateTime value, the method returns DateTime.MinValue.
    /// </returns>
    public static DateTime ToDateTime(this object o)
    {
        if (o is DateTime _time)
        {
            return _time;
        }

        return o == null ? System.DateTime.MinValue :
               System.DateTime.TryParse(o.ToString(), out DateTime _outDate) ? _outDate : System.DateTime.MinValue;
    }

    /// <summary>
    ///     Converts the string representation of a date and time to its DateTime equivalent.
    ///     If the string is null or empty, or if the conversion fails, it returns DateTime.MinValue.
    /// </summary>
    /// <param name="s">A string containing a date and time to convert.</param>
    /// <returns>
    ///     The DateTime equivalent of the date and time contained in s, if the conversion succeeded, or DateTime.MinValue
    ///     if the string is null or empty, or the conversion failed.
    /// </returns>
    public static DateTime ToDateTime(this string s) => string.IsNullOrEmpty(s) ? System.DateTime.MinValue : System.DateTime.TryParse(s, out DateTime _outDate) ?
                                                            _outDate : System.DateTime.MinValue;

    /// <summary>
    ///     Converts the object representation of a value to its decimal equivalent.
    /// </summary>
    /// <param name="o">An object containing the value to convert.</param>
    /// <param name="nullValue">The value to return if the object is null. Default is 0.</param>
    /// <returns>
    ///     A decimal value that is equivalent to the value contained in the object. If the object is null, or does not
    ///     represent a valid decimal value, the method returns the nullValue.
    /// </returns>
    public static decimal ToDecimal(this object o, decimal nullValue = 0)
    {
        return o switch
               {
                   null => nullValue,
                   int => Convert.ToDecimal(o),
                   decimal => Convert.ToDecimal(o),
                   double => decimal.TryParse(o.ToString(), out decimal _out) ? _out : nullValue,
                   float => decimal.TryParse(o.ToString(), out decimal _out) ? _out : nullValue,
                   _ => decimal.TryParse(o.ToString(), out decimal _out) ? _out : nullValue
               };
    }

    /// <summary>
    ///     Converts the object representation of a value to its double equivalent.
    /// </summary>
    /// <param name="o">An object containing the value to convert.</param>
    /// <param name="nullValue">The value to return if the object is null. Default is 0.</param>
    /// <returns>
    ///     A double value that is equivalent to the value contained in the object. If the object is null, or does not
    ///     represent a valid double value, the method returns the nullValue.
    /// </returns>
    public static double ToDouble(this object o, double nullValue = 0)
    {
        return o switch
               {
                   null => nullValue,
                   int => Convert.ToDouble(o),
                   double => Convert.ToDouble(o),
                   decimal => double.TryParse(o.ToString(), out double _out) ? _out : nullValue,
                   float => double.TryParse(o.ToString(), out double _out) ? _out : nullValue,
                   _ => double.TryParse(o.ToString(), out double _out) ? _out : nullValue
               };
    }

    /// <summary>
    ///     Converts the string representation of a short integer value to its short integer equivalent.
    /// </summary>
    /// <param name="s">A string containing the value to convert.</param>
    /// <param name="nullValue">
    ///     The value to return if the string is null or empty, or does not represent a valid short integer
    ///     value. Default is 0.
    /// </param>
    /// <returns>
    ///     A short integer value that is equivalent to the short integer value contained in the string. If the string is
    ///     null or empty, or does not represent a valid short integer value, the method returns the nullValue.
    /// </returns>
    public static short ToInt16(this string s, short nullValue = 0) => string.IsNullOrEmpty(s) ? nullValue : short.TryParse(s, out short _outInt) ? _outInt : nullValue;

    /// <summary>
    ///     Converts the object representation of a short integer value to its short integer equivalent.
    /// </summary>
    /// <param name="o">An object containing the value to convert.</param>
    /// <param name="nullValue">
    ///     The value to return if the object is null or does not represent a valid short integer value.
    ///     Default is 0.
    /// </param>
    /// <returns>
    ///     A short integer value that is equivalent to the short integer value contained in the object. If the object is
    ///     null or does not represent a valid short integer value, the method returns nullValue.
    /// </returns>
    public static short ToInt16(this object o, short nullValue = 0)
    {
        return o switch
               {
                   null => nullValue,
                   short _i => _i,
                   decimal => (short)(decimal.TryParse(o.ToString(), out decimal _outDecimal) ? _outDecimal : nullValue),
                   double => (short)(double.TryParse(o.ToString(), out double _outDouble) ? _outDouble : nullValue),
                   float => (short)(float.TryParse(o.ToString(), out float _outFloat) ? _outFloat : nullValue),
                   _ => short.TryParse(o.ToString(), out short _outInt) ? _outInt : nullValue
               };
    }

    /// <summary>
    ///     Converts the object representation of a number to its 32-bit signed integer equivalent.
    /// </summary>
    /// <param name="o">An object containing the number to convert.</param>
    /// <param name="nullValue">The value to return if the object is null or does not represent a valid number. Default is 0.</param>
    /// <returns>
    ///     A 32-bit signed integer that is equivalent to the number contained in the object. If the object is null or
    ///     does not represent a valid number, the method returns nullValue.
    /// </returns>
    public static int ToInt32(this object o, int nullValue = 0)
    {
        return o switch
               {
                   null => nullValue,
                   int _i => _i,
                   decimal _ => (int)(decimal.TryParse(o.ToString(), out decimal _outDecimal) ? _outDecimal : nullValue),
                   double _ => (int)(double.TryParse(o.ToString(), out double _outDouble) ? _outDouble : nullValue),
                   float _ => (int)(float.TryParse(o.ToString(), out float _outFloat) ? _outFloat : nullValue),
                   _ => int.TryParse(o.ToString(), out int _outInt) ? _outInt : nullValue
               };
    }

    /// <summary>
    ///     Converts the string representation of a number to its 32-bit signed integer equivalent.
    /// </summary>
    /// <param name="s">A string containing a number to convert.</param>
    /// <param name="nullValue">
    ///     A number to return if the string is null or empty, or does not represent a valid integer.
    ///     Default is 0.
    /// </param>
    /// <returns>
    ///     A 32-bit signed integer that is equivalent to the number contained in the string, or nullValue if the string
    ///     is null or empty, or does not represent a valid integer.
    /// </returns>
    public static int ToInt32(this string s, int nullValue = 0) => string.IsNullOrEmpty(s) ? nullValue
                                                                   : int.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat,
                                                                                  out int _outInt) ? _outInt : nullValue;

    /// <summary>
    ///     Converts the given string to a long (Int64) value.
    /// </summary>
    /// <param name="s">The string to be converted.</param>
    /// <param name="nullValue">The value to return if the string is null or cannot be converted to a long. Default is 0.</param>
    /// <returns>
    ///     The long value converted from the string.
    ///     If the string is null, empty, or cannot be converted to a long, the method returns the provided nullValue.
    /// </returns>
    public static long ToInt64(this string s, long nullValue = 0) => string.IsNullOrEmpty(s) ? nullValue
                                                                     : long.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat,
                                                                                     out long _outInt) ? _outInt : nullValue;

    /// <summary>
    ///     Converts the given object to a long integer (Int64).
    ///     If the object is null or cannot be converted, the method will return the provided nullValue.
    /// </summary>
    /// <param name="o">The object to convert.</param>
    /// <param name="nullValue">The value to return if the object is null or cannot be converted. Default is 0.</param>
    /// <returns>
    ///     The long integer (Int64) representation of the object or nullValue if the object is null or cannot be
    ///     converted.
    /// </returns>
    public static long ToInt64(this object o, int nullValue = 0)
    {
        return o switch
               {
                   null => nullValue,
                   int _i => Convert.ToInt64(_i),
                   long _i => _i,
                   decimal => (long)Math.Round(decimal.TryParse(o.ToString(), out decimal _outDecimal) ? _outDecimal : nullValue),
                   double => (long)Math.Round(double.TryParse(o.ToString(), out double _outDouble) ? _outDouble : nullValue),
                   float _ => (long)Math.Round(float.TryParse(o.ToString(), out float _outFloat) ? _outFloat : nullValue),
                   _ => (long)Math.Round((double)(long.TryParse(o.ToString(), out long _outInt) ? _outInt : nullValue))
               };
    }

    /// <summary>
    ///     Converts the given string to a MarkupString.
    /// </summary>
    /// <param name="s">The string to be converted.</param>
    /// <returns>
    ///     A MarkupString where new line characters are replaced with HTML line break tags. If the input string is null,
    ///     empty, or consists only of white-space characters, an empty MarkupString is returned.
    /// </returns>
    public static MarkupString ToMarkupString(this string s)
    {
        if (s.NullOrWhiteSpace())
        {
            return (MarkupString)"";
        }

        s = s.Replace(Environment.NewLine, "<br/>");

        return (MarkupString)s;
    }
}