#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             GMailService
// File Name:           GMailFetch.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 18:50
// Last Updated On:     12-7-2023 15:36
// *****************************************/

#endregion

#region Using

using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

using MimeKit;

#endregion

namespace GMailService;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class GMailFetch
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="GMailFetch" /> class.
    /// </summary>
    /// <param name="serviceAccountUser">
    ///     The primary email address of the service account user. This is required to authenticate and authorize the
    ///     application.
    /// </param>
    /// <param name="jsonPath">
    ///     The path to the JSON file containing the private keys for the service account.
    /// </param>
    /// <param name="userEmails">
    ///     An array of user email addresses. The application will fetch emails for these users from Google Workspace Email.
    /// </param>
    /// <param name="query">
    ///     The query to execute against the mailbox. This is used to filter the emails that are fetched. The default value is
    ///     "label:inbox", which fetches all emails in the inbox.
    /// </param>
    /// <param name="maxResults">
    ///     The maximum number of results to fetch per page. The maximum value is 500, and the default value is 100.
    /// </param>
    public GMailFetch(string serviceAccountUser, string jsonPath, string[] userEmails, string query = "label:inbox", short maxResults = 100)
    {
        ServiceAccountUser = serviceAccountUser;
        JsonPath = jsonPath;
        UserEmails = userEmails;
        Query = query;
        MaxResults = maxResults > 500 ? (short)500 : maxResults;
    }

    /// <summary>
    ///     Gets or sets the path to the JSON file containing the private keys for the service account.
    /// </summary>
    /// <value>
    ///     The path to the JSON file.
    /// </value>
    /// <remarks>
    ///     This property is used during the initialization of the <see cref="GMailFetch" /> class.
    ///     The JSON file at this path should contain the private keys for the service account,
    ///     which are required for authenticating and authorizing the application.
    /// </remarks>
    public string JsonPath
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the maximum number of results to fetch per page.
    /// </summary>
    /// <value>
    ///     The maximum number of results per page.
    /// </value>
    /// <remarks>
    ///     This property is used during the fetching of emails. The maximum value is 500, and the default value is 100.
    ///     If a value greater than 500 is provided, it will be set to 500.
    /// </remarks>
    public short MaxResults
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the query to execute against the mailbox.
    /// </summary>
    /// <value>
    ///     The query string.
    /// </value>
    /// <remarks>
    ///     This property is used to filter the emails that are fetched from the mailbox.
    ///     The default value is "label:inbox", which fetches all emails in the inbox.
    /// </remarks>
    public string Query
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the primary email address of the service account user.
    /// </summary>
    /// <value>
    ///     The primary email address of the service account user.
    /// </value>
    /// <remarks>
    ///     This property is used during the initialization of the <see cref="GMailFetch" /> class.
    ///     The email address is required to authenticate and authorize the application.
    /// </remarks>
    public string ServiceAccountUser
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets the array of user email addresses.
    /// </summary>
    /// <value>
    ///     An array of user email addresses. The application fetches emails for these users from Google Workspace Email.
    /// </value>
    private string[] UserEmails
    {
        get;
    }

    /// <summary>
    ///     Retrieves the content of a specific email message for a given user.
    /// </summary>
    /// <param name="user">
    ///     The email address of the user whose email content is to be fetched.
    /// </param>
    /// <param name="emailID">
    ///     The unique identifier of the email message to be fetched.
    /// </param>
    /// <param name="fetchOnlyMetaData">
    ///     A boolean value indicating whether to fetch only the metadata of the email message.
    ///     If set to true, only the metadata is fetched. If set to false, the full email content is fetched.
    ///     The default value is false.
    /// </param>
    /// <param name="fileLocation">
    ///     The location where any attachments in the email message should be saved.
    ///     If this parameter is not provided or is an empty string, attachments are not saved.
    /// </param>
    /// <param name="saveAttachments">
    ///     A boolean value indicating whether to save any attachments in the email message.
    ///     If set to true, attachments are saved at the specified file location.
    ///     If set to false, attachments are not saved. The default value is false.
    /// </param>
    /// <returns>
    ///     A <see cref="MessageDetails" /> object containing the details of the fetched email message.
    ///     If the email message could not be fetched, returns null.
    /// </returns>
    public MessageDetails GetMailContent(string user, string emailID, bool fetchOnlyMetaData = false, string fileLocation = "", bool saveAttachments = false)
    {
        GoogleCredential _credential = GoogleCredential.FromFile(JsonPath)
                                                       .CreateScoped(GmailService.Scope.GmailReadonly)
                                                       .CreateWithUser(user);
        GmailService _service = new(new()
                                    {
                                        HttpClientInitializer = _credential,
                                        ApplicationName = "GMailService"
                                    });
        // Retrieve the email message with the specified ID
        UsersResource.MessagesResource.GetRequest _request = _service.Users.Messages.Get("me", emailID);
        _request.Format = fetchOnlyMetaData ? UsersResource.MessagesResource.GetRequest.FormatEnum.Metadata : UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

        Message _emailMessage = _request.Execute();
        MessageDetails _messageDetails = null;

        if (_emailMessage == null)
        {
            return null;
        }

        if (!fetchOnlyMetaData)
        {
            // Parse the email message using MimeKit
            MimeMessage _email = MimeMessage.Load(new MemoryStream(Convert.FromBase64String(_emailMessage.Payload.Body.Data)));

            // Get the email attachments, if any
            List<Attachment> _attachments = new();
            if (_emailMessage.Payload.Parts != null)
            {
                _attachments.AddRange(_emailMessage.Payload.Parts.Where(part => !string.IsNullOrEmpty(part.Filename))
                                                   .Select(part => new
                                                                   {
                                                                       part, data = Convert.FromBase64String(part.Body.Data)
                                                                   })
                                                   .Select(t => new Attachment(new MemoryStream(t.data), t.part.Filename)));
            }

            // Print the email information and attachments, if any
            string _attachmentsName = _attachments.Aggregate("", (current, attachment) => current + $", {attachment.Name}");

            if (!string.IsNullOrWhiteSpace(_attachmentsName) && _attachmentsName.StartsWith(","))
            {
                _attachmentsName = _attachmentsName[1..].Trim();
            }

            _messageDetails = new(emailID, _email.From.Mailboxes.FirstOrDefault()?.Address, _email.To.Mailboxes.FirstOrDefault()?.Address, _email.Subject,
                                  _email.HtmlBody ?? _email.TextBody, _attachmentsName, _email.Date.DateTime.ToShortDateString() + " " + _email.Date.DateTime.ToShortTimeString());
        }
        else
        {
            string _subject = _emailMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Subject")?.Value;
            string _from = _emailMessage.Payload.Headers.FirstOrDefault(h => h.Name == "From")?.Value;
            string _to = _emailMessage.Payload.Headers.FirstOrDefault(h => h.Name == "To")?.Value;
            string _date = _emailMessage.Payload.Headers.FirstOrDefault(h => h.Name == "Date")?.Value;

            _messageDetails = new(emailID, _from, _to, _subject, "", "", _date);
        }

        return _messageDetails;
    }

    /// <summary>
    ///     Retrieves a list of email messages for all users specified in the UserEmails property.
    /// </summary>
    /// <returns>
    ///     A list of <see cref="MessageDetails" /> objects, each representing an email message.
    ///     If the UserEmails property is empty or null, returns null.
    /// </returns>
    /// <remarks>
    ///     This method iterates over each user email address in the UserEmails property and fetches their email messages.
    ///     The fetched messages are then added to a list which is returned.
    ///     If no messages are fetched or if the UserEmails property is empty or null, the method returns null.
    /// </remarks>
    public List<MessageDetails> GetMessages()
    {
        List<MessageDetails> _messages = new();

        if (UserEmails is not {Length: > 0})
        {
            return null;
        }

        foreach (string _userEmail in UserEmails)
        {
            List<MessageDetails> _tempMessages = GetMessages(_userEmail);
            if (_tempMessages != null)
            {
                _messages.AddRange(_tempMessages);
            }
        }

        return _messages;
    }

    /// <summary>
    ///     Fetches a list of email messages for a given user.
    /// </summary>
    /// <param name="user">
    ///     The email address of the user whose email messages are to be fetched.
    /// </param>
    /// <returns>
    ///     A list of <see cref="MessageDetails" /> objects containing the details of the fetched email messages.
    ///     If no messages could be fetched, returns null.
    /// </returns>
    /// <remarks>
    ///     This method uses the Gmail API to fetch the email messages. It authenticates the application using the service
    ///     account's private keys,
    ///     which are loaded from the JSON file specified by the <see cref="JsonPath" /> property.
    ///     The method fetches the messages in pages, with the maximum number of results per page specified by the
    ///     <see cref="MaxResults" /> property.
    ///     The messages are filtered based on the query specified by the <see cref="Query" /> property.
    ///     For each fetched message, the method retrieves its details by calling the <see cref="GetMailContent" /> method with
    ///     the `fetchOnlyMetaData` parameter set to true.
    ///     If the application fails to authenticate or if an error occurs during the fetching of messages, the method returns
    ///     null.
    /// </remarks>
    public List<MessageDetails> GetMessages(string user)
    {
        GoogleCredential _credential = GoogleCredential.FromFile(JsonPath)
                                                       .CreateScoped(GmailService.Scope.GmailReadonly)
                                                       .CreateWithUser(user);

        List<Message> _returnMessages = new();

        if (_credential == null)
        {
            return null;
        }

        GmailService _service = new(new()
                                    {
                                        HttpClientInitializer = _credential,
                                        ApplicationName = "GMailService"
                                    });

        UsersResource.MessagesResource.ListRequest _request = _service.Users?.Messages?.List(user);
        if (_request == null)
        {
            return null;
        }

        _request.MaxResults = MaxResults;
        _request.Q = Query;
        List<Message> _messages = new();
        do
        {
            ListMessagesResponse _response = _request.Execute();
            if (_response is {Messages.Count: > 0})
            {
                _messages.AddRange(_response.Messages);
            }

            _request.PageToken = _response.NextPageToken;
        } while (_request.PageToken != null);

        if (_messages is {Count: 0})
        {
            return null;
        }

        _returnMessages.AddRange(_messages.Select(message => _service.Users?.Messages?.Get(user, message.Id).Execute()).Where(msg => msg != null));

        return _returnMessages.Select(returnMessage => GetMailContent(user, returnMessage.Id, true)).Where(detail => detail != null).ToList();
    }
}