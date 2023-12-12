﻿#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           DownloadsPanel.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-05-2023 14:56
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Pages.Controls.Common;

using ConfirmDialog = Profsvc_AppTrack.Components.Pages.Controls.Common.ConfirmDialog;
using ViewPDFDocument = Profsvc_AppTrack.Components.Pages.Controls.Common.ViewPDFDocument;

namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     Represents a panel for managing candidate documents in the application.
/// </summary>
/// <remarks>
///     This class provides functionalities for downloading and deleting documents associated with a candidate.
///     It also allows the viewing of a candidate's resume in various formats such as PDF, DOC, DOCX, and RTF.
///     The panel's appearance and behavior can be customized through various parameters, such as `EditRights` for editing
///     permissions,
///     `RowHeight` for the height of each row in the panel, and `Model` for the list of documents to be managed.
/// </remarks>
public partial class DownloadsPanel
{
    private int _candidateID, _selectedID;
    private string _internalFileName = "", _documentName = "", _documentLocation = "";

    /// <summary>
    ///     Gets or sets the event callback that is triggered when a document is to be deleted.
    /// </summary>
    /// <value>
    ///     The event callback that takes an integer as a parameter, which represents the ID of the document to be deleted.
    /// </value>
    [Parameter]
    public EventCallback<int> DeleteDocument
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ConfirmDialog instance used for confirming document deletion operations.
    /// </summary>
    /// <value>
    ///     The ConfirmDialog instance.
    /// </value>
    /// <remarks>
    ///     This dialog is shown when a user attempts to delete a document. It provides the user with a final chance to confirm
    ///     or cancel the deletion.
    /// </remarks>
    private ConfirmDialog DialogConfirm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the ViewPDFDocument component used for displaying PDF documents.
    /// </summary>
    /// <value>
    ///     The instance of the ViewPDFDocument component.
    /// </value>
    /// <remarks>
    ///     This property is used to manage the display of PDF documents within the DownloadsPanel.
    ///     It is utilized in methods such as `ShowResume` and `ViewDocumentDialog` to show the dialog of the PDF document
    ///     viewer.
    /// </remarks>
    private ViewPDFDocument DocumentViewPDF
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ViewWordDocument component used for displaying Word documents.
    /// </summary>
    /// <value>
    ///     The ViewWordDocument component.
    /// </value>
    private ViewWordDocument DocumentViewWord
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when a document is to be downloaded.
    /// </summary>
    /// <value>
    ///     The event callback that takes an integer as a parameter, which represents the ID of the document to be downloaded.
    /// </value>
    [Parameter]
    public EventCallback<int> DownloadDocument
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the user has rights to edit the documents.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the user can edit the documents; otherwise, <c>false</c>. The default value is <c>true</c>.
    /// </value>
    [Parameter]
    public bool EditRights
    {
        get;
        set;
    } = true;

    /// <summary>
    ///     Gets or sets the Syncfusion Blazor Grid component for managing candidate documents.
    /// </summary>
    /// <value>
    ///     The Syncfusion Blazor Grid component that displays the list of candidate documents and provides functionalities
    ///     such as downloading, deleting, and viewing the documents.
    /// </value>
    private SfGrid<CandidateDocument> GridDownload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of candidate documents to be managed in the panel.
    /// </summary>
    /// <value>
    ///     The list of <see cref="CandidateDocument" /> objects representing the documents associated with a candidate.
    /// </value>
    [Parameter]
    public List<CandidateDocument> Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the height of each row in the panel.
    /// </summary>
    /// <value>
    ///     The height of each row in pixels. The default value is 38.
    /// </value>
    [Parameter]
    public int RowHeight
    {
        get;
        set;
    } = 38;

    /// <summary>
    ///     Gets or sets the selected row in the panel.
    /// </summary>
    /// <value>
    ///     The <see cref="CandidateDocument" /> object representing the document associated with the selected row in the
    ///     panel.
    /// </value>
    public CandidateDocument SelectedRow
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the Syncfusion spinner control used in the DownloadsPanel.
    /// </summary>
    /// <value>
    ///     The Syncfusion spinner control.
    /// </value>
    /// <remarks>
    ///     The spinner is used to indicate a loading state while the panel is processing tasks such as downloading or deleting
    ///     documents.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Initiates the process of deleting a document.
    /// </summary>
    /// <param name="id">The ID of the document to be deleted.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sets the ID of the document to be deleted, selects the corresponding row in the grid,
    ///     and shows a confirmation dialog to the user.
    /// </remarks>
    private async Task DeleteDocumentMethod(int id)
    {
        _selectedID = id;
        int _index = await GridDownload.GetRowIndexByPrimaryKeyAsync(id);
        await GridDownload.SelectRowAsync(_index);
        await DialogConfirm.ShowDialog();
    }

    /// <summary>
    ///     Initiates the download process for a specific document.
    /// </summary>
    /// <param name="id">The ID of the document to be downloaded.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sets the selected document ID, gets the row index of the document in the grid by its primary key,
    ///     selects the row in the grid, and invokes the DownloadDocument event callback.
    /// </remarks>
    private async Task DownloadDocumentDialog(int id)
    {
        _selectedID = id;
        int _index = await GridDownload.GetRowIndexByPrimaryKeyAsync(id);
        await GridDownload.SelectRowAsync(_index);
        await DownloadDocument.InvokeAsync(id);
    }

    /// <summary>
    ///     Handles the event when a row is selected in the panel.
    /// </summary>
    /// <param name="companyDocument">
    ///     The <see cref="RowSelectEventArgs{CandidateDocument}" /> instance containing the event data.
    ///     This includes the selected document associated with the selected row.
    /// </param>
    /// <remarks>
    ///     When a row is selected, this method updates the `SelectedRow` property with the document associated with the
    ///     selected row.
    ///     If no row is selected (i.e., `companyDocument` is null), the `SelectedRow` property is not updated.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<CandidateDocument> companyDocument)
    {
        if (companyDocument != null)
        {
            SelectedRow = companyDocument.Data;
        }
    }

    /// <summary>
    ///     Displays the resume of a candidate in the appropriate format based on the document's extension.
    /// </summary>
    /// <param name="documentLocation">The location of the document to be displayed.</param>
    /// <param name="candidateID">The ID of the candidate whose resume is to be displayed.</param>
    /// <param name="documentName">The name of the document to be displayed.</param>
    /// <param name="internalFileName">The internal file name of the document to be displayed.</param>
    /// <returns>A Task representing the asynchronous operation of displaying the resume.</returns>
    /// <remarks>
    ///     This method checks the extension of the document and displays it in the appropriate format.
    ///     It supports PDF, DOC, DOCX, and RTF formats. If the document is a PDF, it is displayed using the DocumentViewPDF
    ///     dialog.
    ///     Otherwise, it is displayed using the DocumentViewWord dialog.
    /// </remarks>
    public async Task ShowResume(string documentLocation, int candidateID, string documentName, string internalFileName)
    {
        if (documentLocation.EndsWith(".pdf") || documentLocation.EndsWith(".doc") || documentLocation.EndsWith(".docx") || documentLocation.EndsWith(".rtf"))
        {
            _candidateID = candidateID;
            _documentName = documentName;
            _documentLocation = documentLocation;
            _internalFileName = internalFileName;
            //await DocumentViewPDF.ShowDialog();
            if (documentLocation.EndsWith(".pdf"))
            {
                await DocumentViewPDF.ShowDialog();
            }
            else
            {
                await DocumentViewWord.ShowDialog();
            }
        }
    }

    /// <summary>
    ///     Asynchronously displays a dialog for viewing a document.
    /// </summary>
    /// <param name="documentID">
    ///     The ID of the document to be viewed.
    /// </param>
    /// <remarks>
    ///     This method retrieves the document details from the server using the provided document ID.
    ///     If the document is in PDF, DOC, DOCX, or RTF format, it opens the corresponding dialog to view the document.
    ///     The method also manages the visibility of a loading spinner during the process.
    /// </remarks>
    private async Task ViewDocumentDialog(int documentID)
    {
        await Task.Yield();
        await Spinner.ShowAsync();
        RestClient _restClient = new($"{Start.ApiHost}");
        RestRequest _request = new("Candidates/DownloadFile")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("documentID", documentID);

        DocumentDetails _restResponse = await _restClient.GetAsync<DocumentDetails>(_request);

        if (_restResponse != null)
        {
            string _location = _restResponse.DocumentLocation;
            if (_location.EndsWith(".pdf") || _location.EndsWith(".doc") || _location.EndsWith(".docx") || _location.EndsWith(".rtf"))
            {
                _candidateID = _restResponse.EntityID;
                _documentName = _restResponse.DocumentName;
                _documentLocation = _location;
                _internalFileName = _restResponse.InternalFileName;
                //await DocumentViewPDF.ShowDialog();
                if (_location.EndsWith(".pdf"))
                {
                    await DocumentViewPDF.ShowDialog();
                }
                else
                {
                    await DocumentViewWord.ShowDialog();
                }
            }
        }

        await Task.Yield();
        await Spinner.HideAsync();
    }
}