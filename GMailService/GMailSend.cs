#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             GMailService
// File Name:           GMailSend.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 18:50
// Last Updated On:     12-7-2023 15:36
// *****************************************/

#endregion

#region Using

using System.Text;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

using MimeKit;

#endregion

namespace GMailService;

public class GMailSend
{
    /// <summary>
    ///     Sends an email using the Gmail API.
    /// </summary>
    /// <param name="jsonPath">The path to the JSON file containing the service account credentials.</param>
    /// <param name="user">The email address of the sender.</param>
    /// <param name="cc">A dictionary containing the names and email addresses of the CC recipients.</param>
    /// <param name="recipients">A dictionary containing the names and email addresses of the recipients.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body of the email. HTML is supported.</param>
    /// <param name="attachments">A list of file paths to be attached to the email.</param>
    /// <returns>Returns true if the email was sent successfully.</returns>
    /// <remarks>
    ///     This method uses the Gmail API to send an email. It first loads the service account credentials from a JSON file
    ///     specified by the jsonPath parameter. It then creates a Gmail API client and a MIME message, which is populated with
    ///     the provided sender, recipients, CC recipients, subject, body, and attachments. The MIME message is then converted
    ///     to a Gmail message and sent. If the email is sent successfully, the method returns true
    /// </remarks>
    public static bool SendEmail(string jsonPath, string user, Dictionary<string, string> cc, Dictionary<string, string> recipients, string subject, string body, List<string> attachments)
    {
        // Load the service account credentials file.
        GoogleCredential _credential;

        using (FileStream _stream = new(jsonPath, FileMode.Open, FileAccess.Read))
        {
            _credential = GoogleCredential.FromStream(_stream)
                                         .CreateScoped(GmailService.Scope.GmailCompose, GmailService.Scope.GmailSend)
                                         .CreateWithUser(user);
        }

        // Create the Gmail API client.
        GmailService _service = new(new()
                                   {
                                       HttpClientInitializer = _credential,
                                       ApplicationName = "GMailService.GMailSend"
                                   });

        // Create the MIME message.
        MimeMessage _message = new();
        _message.From.Add(new MailboxAddress("", user));
        foreach (KeyValuePair<string, string> _recipient in recipients)
        {
            _message.To.Add(new MailboxAddress(_recipient.Key, _recipient.Value));
        }

        foreach (KeyValuePair<string, string> _ccSingle in cc)
        {
            _message.Cc.Add(new MailboxAddress(_ccSingle.Key, _ccSingle.Value));
        }

        _message.Subject = subject;

        BodyBuilder _builder = new()
                               {
                                   HtmlBody = body
                               };

        if (attachments is {Count: > 0})
        {
            foreach (MimePart _attachment2 in attachments.Where(File.Exists)
                                                        .Select(attachment =>
                                                                {
                                                                    MimePart _part = new();
                                                                    using FileStream _fs = new(attachment, FileMode.Open, FileAccess.Read, FileShare.None, 4096);
                                                                    MemoryStream _memoryStream = new();
                                                                    _fs.CopyTo(_memoryStream);
                                                                    _part.Content = new MimeContent(_memoryStream);
                                                                    _part.ContentDisposition = new(ContentDisposition.Attachment);
                                                                    _part.ContentTransferEncoding = ContentEncoding.Base64;
                                                                    _part.FileName = Path.GetFileName(attachment);
                                                                    _fs.Close();
                                                                    _fs.Dispose();

                                                                    return _part;
                                                                }))
            {
                _builder.Attachments.Add(_attachment2);
            }
        }

        _message.Body = _builder.ToMessageBody();

        // Convert the MIME message to a Gmail message.
        Message _gmailMessage = new()
                                {
                                    Raw = Convert.ToBase64String(Encoding.UTF8.GetBytes(_message.ToString() ?? string.Empty))
                                };

        // Send the email.
        UsersResource.MessagesResource.SendRequest _request = _service.Users.Messages.Send(_gmailMessage, user);
        _request.Execute();

        return true;
    }
}