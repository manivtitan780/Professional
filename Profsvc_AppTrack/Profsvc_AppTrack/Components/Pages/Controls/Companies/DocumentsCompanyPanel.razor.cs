#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           DocumentsCompanyPanel.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-29-2023 18:59
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Pages.Controls.Common;

using ConfirmDialog = Profsvc_AppTrack.Components.Pages.Controls.Common.ConfirmDialog;
using ViewPDFDocument = Profsvc_AppTrack.Components.Pages.Controls.Common.ViewPDFDocument;

namespace Profsvc_AppTrack.Components.Pages.Controls.Companies;

/// <summary>
///     The DocumentsCompanyPanel class is a component used within the Companies page to manage company-related documents.
///     It provides functionality for deleting and downloading documents, and viewing them in PDF or Word format.
///     This component also includes a grid view for displaying document data, and supports row selection.
/// </summary>
/// <remarks>
///     The component includes parameters for handling document deletion and download events,
///     setting edit rights, specifying the model data, setting the row height, and specifying the user.
/// </remarks>
public partial class DocumentsCompanyPanel
{
    private string _internalFileName = "", _documentName = "", _documentLocation = "";

    private int _requisitionID;
    private int _selectedID;

    /// <summary>
    ///     Gets or sets the event callback that is triggered when a document is to be deleted.
    /// </summary>
    /// <value>
    ///     The event callback that takes an integer representing the document ID.
    /// </value>
    /// <remarks>
    ///     This event callback is used in the UI to handle the deletion of documents.
    ///     The integer parameter represents the ID of the document to be deleted.
    /// </remarks>
    [Parameter]
    public EventCallback<int> DeleteDocument
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ConfirmDialog component used for confirming user actions.
    /// </summary>
    /// <value>
    ///     The ConfirmDialog component.
    /// </value>
    /// <remarks>
    ///     This property is used to reference the ConfirmDialog component, which provides a dialog interface for confirming
    ///     various user actions.
    ///     The dialog can be used to confirm actions such as deletion, cancellation, and activity toggling.
    /// </remarks>
    private ConfirmDialog DialogConfirm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ViewPDFDocument component used for displaying PDF documents.
    /// </summary>
    /// <value>
    ///     The ViewPDFDocument component.
    /// </value>
    /// <remarks>
    ///     This component is used to display a PDF document in a dialog. It is triggered when a document is selected for
    ///     viewing in the DocumentsCompanyPanel.
    /// </remarks>
    private ViewPDFDocument DocumentViewPDF
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the instance of the ViewWordDocument component.
    /// </summary>
    /// <value>
    ///     The instance of the ViewWordDocument component.
    /// </value>
    /// <remarks>
    ///     This property is used to interact with the ViewWordDocument component, which provides functionality to view Word
    ///     documents.
    ///     It is used in various parts of the application where viewing of Word documents is required.
    /// </remarks>
    /// <explain>
    ///     This property is a private member of the DocumentsCompanyPanel class. It holds an instance of the ViewWordDocument
    ///     component, which is used to view Word documents. The ViewWordDocument component takes several parameters including
    ///     the location and name of the document, the ID and type of the entity associated with the document, and the internal
    ///     file name. The entity type is represented by the EntityType enumeration, which includes Candidate, Requisition,
    ///     Companies, Leads, and Benefits.
    /// </explain>
    private ViewWordDocument DocumentViewWord
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when a document is to be downloaded.
    /// </summary>
    /// <value>
    ///     The event callback that takes an integer representing the document ID.
    /// </value>
    /// <remarks>
    ///     This event callback is used in the UI to handle the downloading of documents.
    ///     The integer parameter represents the ID of the document to be downloaded.
    /// </remarks>
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
    ///     A boolean value. If true, the user has rights to edit the documents. If false, the user does not have these rights.
    /// </value>
    /// <remarks>
    ///     This property is used to control the visibility of the download and delete buttons in the UI.
    ///     If the user has edit rights and is the owner of the requisition, these buttons are displayed.
    /// </remarks>
    [Parameter]
    public bool EditRights
    {
        get;
        set;
    } = true;

    /// <summary>
    ///     Gets or sets the SfGrid control for displaying and interacting with the list of RequisitionDocuments.
    /// </summary>
    /// <value>
    ///     The SfGrid control of type RequisitionDocuments.
    /// </value>
    /// <remarks>
    ///     This property is used to display the list of RequisitionDocuments in a grid format.
    ///     It also provides functionality for selecting a row, which is used in methods such as DeleteDocumentMethod.
    /// </remarks>
    public SfGrid<RequisitionDocuments> GridDownload
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of RequisitionDocuments associated with the company.
    /// </summary>
    /// <value>
    ///     The list of RequisitionDocuments.
    /// </value>
    /// <remarks>
    ///     This property is used to populate the grid view with the company's documents.
    ///     Each RequisitionDocuments object in the list represents a document related to the company.
    /// </remarks>
    [Parameter]
    public List<RequisitionDocuments> Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the height of the rows in the grid view for displaying document data.
    /// </summary>
    /// <value>
    ///     A double value representing the height of the rows in pixels. The default value is 38.
    /// </value>
    /// <remarks>
    ///     This property is used to control the height of the rows in the grid view.
    ///     It can be adjusted to accommodate the amount of data in each row and the display preferences of the user.
    /// </remarks>
    [Parameter]
    public double RowHeight
    {
        get;
        set;
    } = 38;

    /// <summary>
    ///     Gets or sets the currently selected row in the grid view of company-related documents.
    /// </summary>
    /// <value>
    ///     The RequisitionDocuments object representing the selected document in the grid view.
    /// </value>
    /// <remarks>
    ///     This property is used to track the document selected by the user in the grid view.
    ///     It is used in various operations such as downloading or deleting a document.
    /// </remarks>
    public RequisitionDocuments SelectedRow
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the SfSpinner control used in the DocumentsCompanyPanel component.
    /// </summary>
    /// <value>
    ///     The SfSpinner control from Syncfusion Blazor library.
    /// </value>
    /// <remarks>
    ///     This control is used to display a loading spinner while asynchronous operations are being performed,
    ///     such as when the ViewDocumentDialog method is retrieving document details.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the user who is currently interacting with the DocumentsCompanyPanel.
    /// </summary>
    /// <value>
    ///     A string representing the username of the current user.
    /// </value>
    /// <remarks>
    ///     This property is used to determine the user's permissions for editing documents.
    ///     If the user is the owner of the document, they are granted edit rights.
    /// </remarks>
    [Parameter]
    public string User
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously deletes a document with the specified ID.
    /// </summary>
    /// <param name="id">
    ///     The ID of the document to be deleted.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method sets the _selectedID field to the provided ID, gets the row index of the document in the GridDownload
    ///     control,
    ///     selects the row, and then shows the DialogConfirm component to confirm the deletion of the document.
    /// </remarks>
    private async Task DeleteDocumentMethod(int id)
    {
        _selectedID = id;
        int _index = await GridDownload.GetRowIndexByPrimaryKeyAsync(id);
        await GridDownload.SelectRowAsync(_index);
        await DialogConfirm.ShowDialog();
    }

    /// <summary>
    ///     Initiates the download of a document.
    /// </summary>
    /// <param name="id">
    ///     The ID of the document to be downloaded.
    /// </param>
    /// <remarks>
    ///     This method is called when a user clicks on the download button in the UI.
    ///     It first gets the index of the row in the grid view that corresponds to the document ID.
    ///     Then, it selects that row in the grid view.
    ///     Finally, it invokes the DownloadDocument event callback, passing in the document ID.
    /// </remarks>
    private async Task DownloadDocumentDialog(int id)
    {
        int _index = await GridDownload.GetRowIndexByPrimaryKeyAsync(id);
        await GridDownload.SelectRowAsync(_index);
        await DownloadDocument.InvokeAsync(id);
    }

    /// <summary>
    ///     Handles the row selection event in the grid view of company-related documents.
    /// </summary>
    /// <param name="row">
    ///     The selected row in the grid view, represented as an instance of
    ///     <see cref="RowSelectEventArgs{RequisitionDocuments}" />.
    /// </param>
    /// <remarks>
    ///     This method is triggered when a row in the grid view is selected by the user.
    ///     If the selected row is not null, the method sets the `SelectedRow` property to the data of the selected row.
    /// </remarks>
    private void RowSelected(RowSelectEventArgs<RequisitionDocuments> row)
    {
        if (row != null)
        {
            SelectedRow = row.Data;
        }
    }

    /// <summary>
    ///     Asynchronously displays a dialog to view a document.
    /// </summary>
    /// <param name="documentID">
    ///     The ID of the document to be viewed.
    /// </param>
    /// <remarks>
    ///     This method retrieves the details of the document with the specified ID from the API,
    ///     and then displays a dialog to view the document. The document can be viewed in PDF or Word format,
    ///     depending on the document's file type. A loading spinner is displayed while the document details are being
    ///     retrieved.
    /// </remarks>
    private async Task ViewDocumentDialog(int documentID)
    {
        await Task.Yield();
        await Spinner.ShowAsync();
        RestClient _restClient = new($"{Start.ApiHost}");
        RestRequest _request = new("Company/DownloadFile")
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
                _requisitionID = _restResponse.EntityID;
                _documentName = _restResponse.DocumentName;
                _documentLocation = _restResponse.DocumentLocation;
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