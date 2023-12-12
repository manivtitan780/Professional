#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             GMailService
// File Name:           MessageDetails.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          04-07-2023 21:14
// Last Updated On:     08-19-2023 20:20
// *****************************************/

#endregion

namespace GMailService;

public class MessageDetails
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageDetails" /> class and clears all data.
    /// </summary>
    public MessageDetails()
    {
        ClearData();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MessageDetails" /> class with the specified parameters.
    /// </summary>
    /// <param name="messageID">The unique identifier of the message.</param>
    /// <param name="from">The sender of the message.</param>
    /// <param name="to">The recipient of the message.</param>
    /// <param name="subject">The subject of the message.</param>
    /// <param name="body">The body of the message.</param>
    /// <param name="attachmentNames">The names of any attachments in the message.</param>
    /// <param name="date">The date the message was sent.</param>
    public MessageDetails(string messageID, string from, string to, string subject, string body, string attachmentNames, string date)
    {
        MessageID = messageID;
        From = from;
        To = to;
        Subject = subject;
        Body = body;
        AttachmentNames = attachmentNames;
        Date = date;
    }

    /// <summary>
    ///     Gets or sets the names of any attachments associated with the message.
    /// </summary>
    public string AttachmentNames
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the body content of the message.
    /// </summary>
    public string Body
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the date the message was sent.
    /// </summary>
    public string Date
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the sender of the message.
    /// </summary>
    public string From
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the unique identifier of the message.
    /// </summary>
    public string MessageID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the subject line of the message.
    /// </summary>
    public string Subject
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the recipient of the message.
    /// </summary>
    public string To
    {
        get;
        set;
    }

    /// <summary>
    ///     Clears all the data in the current instance of the <see cref="MessageDetails" /> class.
    /// </summary>
    public void ClearData()
    {
        MessageID = "";
        From = "";
        To = "";
        Subject = "";
        Body = "";
        AttachmentNames = "";
    }

    /// <summary>
    ///     Creates a copy of the current instance of the <see cref="MessageDetails" /> class.
    /// </summary>
    /// <returns>A new instance of the <see cref="MessageDetails" /> class with the same values as the current instance.</returns>
    public MessageDetails Copy() => MemberwiseClone() as MessageDetails;
}