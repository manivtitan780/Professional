#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Extensions.To.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          01-26-2022 19:30
// Last Updated On:     02-12-2022 19:38
// *****************************************/

#endregion

namespace ProfSvc_AppTrack.Code;

public static partial class Extensions
{
    /// <summary>
    ///     Checks for Null value and returns a replacement value if null or the actual Boolean value for the Column.
    /// </summary>
    /// <param name="read"> SqlDataReader. </param>
    /// <param name="index"> Zero-based column ordinal. </param>
    /// <param name="nullReplaceValue"> Value to be used in case of null value. </param>
    /// <returns> Boolean </returns>
    public static bool GetNBoolean(this SqlDataReader read, int index, bool nullReplaceValue = false) => read.IsDBNull(index) ? nullReplaceValue : read.GetBoolean(index);

    /// <summary>
    ///     Checks for Null value and returns a replacement value if null or the actual int value for the Reader.
    /// </summary>
    /// <param name="read"> SqlDataReader. </param>
    /// <param name="index"> Zero-based column ordinal. </param>
    /// <param name="nullReplaceValue"> Value to be used in case of null value. </param>
    /// <returns> Decimal </returns>
    public static decimal GetNDecimal(this SqlDataReader read, int index, decimal nullReplaceValue = 0) => read.IsDBNull(index) ? nullReplaceValue : read.GetDecimal(index);

    /// <summary>
    ///     Checks for Null value and returns a replacement value if null or the actual int value for the Reader.
    /// </summary>
    /// <param name="read"> SqlDataReader. </param>
    /// <param name="index"> Zero-based column ordinal. </param>
    /// <param name="nullReplaceValue"> Value to be used in case of null value. </param>
    /// <returns> Int32 </returns>
    public static int GetNInt32(this SqlDataReader read, int index, int nullReplaceValue = 0) => read.IsDBNull(index) ? nullReplaceValue : read.GetInt32(index);

    /// <summary>
    ///     Checks for Null value and returns a replacement value if null or the actual string value for the Reader.
    /// </summary>
    /// <param name="read"> SqlDataReader. </param>
    /// <param name="index"> Zero-based column ordinal. </param>
    /// <param name="nullReplaceValue"> Value to be used in case of null value. </param>
    /// <param name="checkEmptyString"> Should empty string s be replace with null replace value? </param>
    /// <returns> String </returns>
    public static string GetNString(this SqlDataReader read, int index, string nullReplaceValue = "", bool checkEmptyString = false) =>
        checkEmptyString ? read.IsDBNull(index) || read.GetString(index) == string.Empty ? nullReplaceValue : read.GetString(index).Trim() :
        read.IsDBNull(index) ? nullReplaceValue : read.GetString(index).Trim();

    /// <summary>
    ///     Returns a Boolean value of the System.String.
    /// </summary>
    /// <param name="s"> String which needs to be converted. </param>
    /// <returns> An Boolean value of the string provided </returns>
    public static bool ToBoolean(this string s) => !string.IsNullOrEmpty(s) && bool.TryParse(s, out bool _outDate) && _outDate;

    /// <summary>
    ///     Returns a Boolean value of the System.Object.
    /// </summary>
    /// <param name="o"> Object which needs to be converted. </param>
    /// <returns> An Boolean value of the Object provided </returns>
    public static bool ToBoolean(this object o)
    {
        if (o is bool _b)
        {
            return _b;
        }

        return o != null && bool.TryParse(o.ToString(), out bool _outDate) && _outDate;
    }

    /// <summary>
    ///     Returns a String value of the represented boolean value.
    /// </summary>
    /// <param name="b"> Boolean which needs to be converted. </param>
    /// <returns> Converted String </returns>
    public static string ToBooleanString(this bool b) => b ? "true" : "false";

    /// <summary>
    ///     Returns a byte converted value of the System.String.
    /// </summary>
    /// <param name="s"> String which needs to be converted. </param>
    /// <param name="nullValue"> Value to be used if Conversion fails. Defaults to 0. </param>
    /// <returns> A Byte value of the string provided </returns>
    public static byte ToByte(this string s, byte nullValue = 0) => string.IsNullOrEmpty(s) ? nullValue : byte.TryParse(s, out byte _outInt) ? _outInt : nullValue;

    /// <summary>
    ///     Returns a DateTime converted value of the System.Object.
    /// </summary>
    /// <param name="o"> Object which needs to be converted. </param>
    /// <returns> An DateTime value of the object provided </returns>
    public static DateTime ToDateTime(this object o)
    {
        if (o is DateTime _time)
        {
            return _time;
        }

        return o == null ? DateTime.MinValue :
               DateTime.TryParse(o.ToString(), out DateTime _outDate) ? _outDate : DateTime.MinValue;
    }

    /// <summary>
    ///     Returns a DateTime converted value of the System.String.
    /// </summary>
    /// <param name="s"> String which needs to be converted. </param>
    /// <returns> An DateTime value of the string provided </returns>
    public static DateTime ToDateTime(this string s) => string.IsNullOrEmpty(s) ? DateTime.MinValue : DateTime.TryParse(s, out DateTime _outDate) ? _outDate : DateTime.MinValue;

    /// <summary>
    ///     Returns an integer converted value of the System.Object.
    /// </summary>
    /// <param name="o"> Object which needs to be converted. </param>
    /// <param name="nullValue"> Value to be used if Conversion fails. Defaults to 0. </param>
    /// <returns> An Integer value of the object provided </returns>
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
    ///     Returns an integer converted value of the System.Object.
    /// </summary>
    /// <param name="o"> Object which needs to be converted. </param>
    /// <param name="nullValue"> Value to be used if Conversion fails. Defaults to 0. </param>
    /// <returns> An Integer value of the object provided </returns>
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
    ///     Returns an integer converted value of the System.Object.
    /// </summary>
    /// <param name="o"> Object which needs to be converted. </param>
    /// <param name="nullValue"> Value to be used if Conversion fails. Defaults to 0. </param>
    /// <returns> An Integer value of the object provided </returns>
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
    ///     Returns an integer converted value of the System.String.
    /// </summary>
    /// <param name="s"> String which needs to be converted. </param>
    /// <param name="nullValue"> Value to be used if Conversion fails. Defaults to 0. </param>
    /// <returns> An Integer value of the string provided </returns>
    public static int ToInt32(this string s, int nullValue = 0) => string.IsNullOrEmpty(s) ? nullValue
                                                                   : int.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat,
                                                                                  out int _outInt) ? _outInt : nullValue;

    /// <summary>
    ///     Returns an long converted value of the System.String.
    /// </summary>
    /// <param name="s"> String which needs to be converted. </param>
    /// <param name="nullValue"> Value to be used if Conversion fails. Defaults to 0. </param>
    /// <returns> An long value of the string provided </returns>
    public static long ToInt64(this string s, long nullValue = 0) => string.IsNullOrEmpty(s) ? nullValue
                                                                     : long.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat,
                                                                                     out long _outInt) ? _outInt : nullValue;

    /// <summary>
    ///     Returns an integer converted value of the System.Object.
    /// </summary>
    /// <param name="o"> Object which needs to be converted. </param>
    /// <param name="nullValue"> Value to be used if Conversion fails. Defaults to 0. </param>
    /// <returns> An Integer value of the object provided </returns>
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
                   _ => long.TryParse(o.ToString(), out long _outInt) ? _outInt : nullValue
               };
    }

    /// <summary>
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
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