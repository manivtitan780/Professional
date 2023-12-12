#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           Extensions.Add.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          01-26-2022 19:30
// Last Updated On:     02-12-2022 19:39
// *****************************************/

#endregion

#region Using

using System.Data;

#endregion

namespace ProfSvc_AppTrack.Code;

/// <summary>
/// </summary>
public static partial class Extensions
{
    /// <summary>
    ///     Generates a SQLParameter of type Binary.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="size"> Size in bytes (length) of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type Binary </returns>
    public static void AddBinaryParameter(this SqlCommand t, string name, int size, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.Binary, size)
                                                                                                                                           {
                                                                                                                                               Value = value,
                                                                                                                                               Direction = output ? ParameterDirection.InputOutput
                                                                                                                                                               : ParameterDirection.Input
                                                                                                                                           });

    /// <summary>
    ///     Generates a SQLParameter of type Bit.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type Bit </returns>
    public static void AddBitParameter(this SqlCommand t, string name, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.Bit)
                                                                                                                              {
                                                                                                                                  Value = value,
                                                                                                                                  Direction =
                                                                                                                                      output
                                                                                                                                          ? ParameterDirection
                                                                                                                                             .InputOutput
                                                                                                                                          : ParameterDirection
                                                                                                                                             .Input
                                                                                                                              });

    /// <summary>
    ///     Generates a SQLParameter of type Char.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="size"> Size in bytes (length) of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="isNType"> Whether the parameter should be nvarchar or varchar. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type Char </returns>
    public static void AddCharParameter(this SqlCommand t, string name, int size, object value, bool isNType = true,
                                        bool output = false) => t.Parameters.Add(new(name, isNType ? SqlDbType.NChar : SqlDbType.Char, size)
                                                                                 {
                                                                                     Value = value,
                                                                                     Direction = output ? ParameterDirection.InputOutput
                                                                                                     : ParameterDirection.Input
                                                                                 });

    /// <summary>
    ///     Generates a SQLParameter of type Date.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction be InputOutput or Input. </param>
    /// <returns> SQLParameter of type Date </returns>
    public static void AddDateParameter(this SqlCommand t, string name, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.Date)
                                                                                                                               {
                                                                                                                                   Value = value,
                                                                                                                                   Direction =
                                                                                                                                       output
                                                                                                                                           ? ParameterDirection
                                                                                                                                              .InputOutput
                                                                                                                                           : ParameterDirection
                                                                                                                                              .Input
                                                                                                                               });

    /// <summary>
    ///     Generates a SQLParameter of type DateTime.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type DateTime </returns>
    public static void AddDateTimeParameter(this SqlCommand t, string name, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.DateTime)
                                                                                                                                   {
                                                                                                                                       Value = value,
                                                                                                                                       Direction =
                                                                                                                                           output
                                                                                                                                               ? ParameterDirection
                                                                                                                                                  .InputOutput
                                                                                                                                               : ParameterDirection
                                                                                                                                                  .Input
                                                                                                                                   });

    /// <summary>
    ///     Generates a SQLParameter of type Decimal.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="precision"> Gets or sets the number of digits used to represent the Value property. </param>
    /// <param name="scale"> Gets or sets the number of decimal places to Value property is resolved. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type DateTime </returns>
    public static void AddDecimalParameter(this SqlCommand t, string name, byte precision, byte scale, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.Decimal)
                                                                                                                                                              {
                                                                                                                                                                  Value = value,
                                                                                                                                                                  Direction =
                                                                                                                                                                      output ? ParameterDirection
                                                                                                                                                                             .InputOutput
                                                                                                                                                                          : ParameterDirection.Input,
                                                                                                                                                                  Precision = precision,
                                                                                                                                                                  Scale = scale
                                                                                                                                                              });

    /// <summary>
    ///     Generates a SQLParameter of type Int.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type Int </returns>
    public static SqlParameter AddIntParameter(this SqlCommand t, string name, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.Int)
                                                                                                                                      {
                                                                                                                                          Value = value,
                                                                                                                                          Direction =
                                                                                                                                              output
                                                                                                                                                  ? ParameterDirection
                                                                                                                                                     .InputOutput
                                                                                                                                                  : ParameterDirection
                                                                                                                                                     .Input
                                                                                                                                      });

    /// <summary>
    ///     Generates a SQLParameter of type SmallDateTime.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type SmallDateTime </returns>
    public static void AddSmallDateTimeParameter(this SqlCommand t, string name, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.SmallDateTime)
                                                                                                                                        {
                                                                                                                                            Value = value,
                                                                                                                                            Direction = output ? ParameterDirection.InputOutput
                                                                                                                                                            : ParameterDirection.Input
                                                                                                                                        });

    /// <summary>
    ///     Generates a SQLParameter of type SmallInt.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type SmallInt </returns>
    public static void AddSmallIntParameter(this SqlCommand t, string name, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.SmallInt)
                                                                                                                                   {
                                                                                                                                       Value = value,
                                                                                                                                       Direction =
                                                                                                                                           output
                                                                                                                                               ? ParameterDirection
                                                                                                                                                  .InputOutput
                                                                                                                                               : ParameterDirection
                                                                                                                                                  .Input
                                                                                                                                   });

    /// <summary>
    ///     Generates a SQLParameter of type Tinyint.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type Tinyint </returns>
    public static void AddTinyIntParameter(this SqlCommand t, string name, object value, bool output = false) => t.Parameters.Add(new(name, SqlDbType.TinyInt)
                                                                                                                                  {
                                                                                                                                      Value = value,
                                                                                                                                      Direction =
                                                                                                                                          output
                                                                                                                                              ? ParameterDirection
                                                                                                                                                 .InputOutput
                                                                                                                                              : ParameterDirection
                                                                                                                                                 .Input
                                                                                                                                  });

    /// <summary>
    ///     Generates a SQLParameter of type UniqueIdentifier.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type UniqueIdentifier </returns>
    public static void AddUniqueIdentifierParameter(this SqlCommand t, string name, object value,
                                                    bool output = false) => t.Parameters.Add(new(name, SqlDbType.UniqueIdentifier)
                                                                                             {
                                                                                                 Value = value,
                                                                                                 Direction = output ? ParameterDirection.InputOutput
                                                                                                                 : ParameterDirection.Input
                                                                                             });

    /// <summary>
    ///     Generates a SQLParameter of type Varchar.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="size"> Size in bytes (length) of the parameter. Enter -1 for MAX parameter </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <param name="isNType"> Whether the parameter should be nvarchar or varchar. </param>
    /// <param name="output"> Should the Parameter Direction b InputOutput or Input. </param>
    /// <returns> SQLParameter of type Varchar </returns>
    public static void AddVarcharParameter(this SqlCommand t, string name, int size, object value,
                                           bool isNType = true, bool output = false) => t.Parameters.Add(new(name, isNType ? SqlDbType.NVarChar : SqlDbType.VarChar, size)
                                                                                                         {
                                                                                                             Value = value,
                                                                                                             Direction = output ? ParameterDirection.InputOutput : ParameterDirection.Input
                                                                                                         });

    /// <summary>
    ///     Generates a SQLParameter of type Xml.
    /// </summary>
    /// <param name="t"> SqlParameterCollection </param>
    /// <param name="name"> The name of the parameter. </param>
    /// <param name="value"> The value to be assigned to the parameter. </param>
    /// <returns> SQLParameter of type Xml </returns>
    public static SqlParameter AddXmlParameter(this SqlCommand t, string name, object value) => t.Parameters.Add(new(name, SqlDbType.Xml)
                                                                                                                 {
                                                                                                                     Value = value
                                                                                                                 });
}