#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           AdvancedCandidateSearch.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-27-2023 21:2
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Candidates;

/// <summary>
///     Represents a component for advanced candidate search functionality.
/// </summary>
/// <remarks>
///     The AdvancedCandidateSearch class is a component that provides a user interface for performing advanced searches on
///     candidates.
///     It includes various parameters to customize the search criteria such as city, job options, eligibility, and state.
///     It also provides an autocomplete feature for city and zip code search, and event callbacks for save and cancel
///     actions.
/// </remarks>
public partial class AdvancedCandidateSearch
{
    /// <summary>
    ///     Gets or sets the autocomplete functionality for city and zip code search.
    /// </summary>
    /// <value>
    ///     The autocomplete functionality for city and zip code search.
    /// </value>
    /// <remarks>
    ///     This property is used to enable or disable the autocomplete feature for city and zip code fields in the advanced
    ///     candidate search form.
    /// </remarks>
    [Parameter]
    public object AutoCompleteCityZip
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the AutoCompleteButton control used in the advanced candidate search form.
    /// </summary>
    /// <value>
    ///     The AutoCompleteButton control used in the advanced candidate search form.
    /// </value>
    /// <remarks>
    ///     This property represents the AutoCompleteButton control that provides autocomplete functionality for input fields
    ///     in the advanced candidate search form.
    ///     The control includes various parameters to customize the behavior and appearance of the autocomplete feature.
    /// </remarks>
    public AutoCompleteButton AutoCompleteControl
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback for the cancel action in the advanced candidate search form.
    /// </summary>
    /// <value>
    ///     The event callback for the cancel action.
    /// </value>
    /// <remarks>
    ///     This property represents the event callback that is invoked when the cancel action is triggered in the advanced
    ///     candidate search form.
    ///     It is used to handle the cancellation of the search operation and perform any necessary cleanup.
    /// </remarks>
    [Parameter]
    public EventCallback<MouseEventArgs> Cancel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the dialog control used in the advanced candidate search form.
    /// </summary>
    /// <value>
    ///     The dialog control used in the advanced candidate search form.
    /// </value>
    /// <remarks>
    ///     This property represents the dialog control that is displayed when the advanced candidate search form is activated.
    ///     The dialog includes various elements for inputting search criteria and buttons for performing the search or
    ///     cancelling the operation.
    /// </remarks>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter control used in the advanced candidate search form.
    /// </summary>
    /// <value>
    ///     The DialogFooter control used in the advanced candidate search form.
    /// </value>
    /// <remarks>
    ///     This property represents the DialogFooter control that provides the footer section of the dialog in the advanced
    ///     candidate search form.
    ///     The control includes the Cancel and Save buttons for managing the dialog actions.
    /// </remarks>
    private DialogFooter DialogFooter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the EditForm used for managing the advanced search criteria.
    /// </summary>
    /// <value>
    ///     The EditForm used for managing the advanced search criteria.
    /// </value>
    private EditForm EditSearchForm
    {
        get;
        set;
    }

	/// <summary>
	///     Gets or sets the list of eligibility options for the advanced candidate search.
	/// </summary>
	/// <value>
	///     The list of eligibility options, represented as a list of <see cref="ProfSvc_Classes.IntValues" />.
	/// </value>
	/// <remarks>
	///     This property is used to populate the dropdown for eligibility options in the advanced candidate search. Each
	///     <see cref="ProfSvc_Classes.IntValues" /> in the list represents a distinct eligibility option.
	/// </remarks>
	[Parameter]
    public List<IntValues> EligibilityDropDown
    {
        get;
        set;
    } = [];

    /// <summary>
    ///     Gets or sets the list of job options for the dropdown in the advanced candidate search component.
    /// </summary>
    /// <value>
    ///     The list of job options. Each option is represented by an instance of the KeyValues class.
    /// </value>
    /// <remarks>
    ///     This property is used to populate the dropdown list with job options for the advanced candidate search.
    ///     The KeyValues class instances represent the individual job options.
    /// </remarks>
    [Parameter]
    public List<KeyValues> JobOptionsDropDown
    {
        get;
        set;
    } = [];

    /// <summary>
    ///     Gets or sets the model for the AdvancedCandidateSearch component.
    /// </summary>
    /// <value>
    ///     The model is of type CandidateSearch, and it holds the search parameters such as name, city, job options,
    ///     eligibility, and state.
    ///     It also includes flags for including admin and city/zip in the search.
    /// </value>
    [Parameter]
    public CandidateSearch Model
    {
        get;
        set;
    } = new();

    /// <summary>
    ///     Gets a list of proximity units used in the advanced candidate search.
    /// </summary>
    /// <value>
    ///     The list of proximity units represented as integer values.
    /// </value>
    /// <remarks>
    ///     The proximity units are used to specify the distance for location-based searches in the advanced candidate search.
    ///     Each unit in the list represents a specific distance value.
    /// </remarks>
    private List<IntValues> ProximityUnit
    {
        get;
        //set;
    } = [];

    /// <summary>
    ///     Gets a list of proximity values used for candidate search.
    /// </summary>
    /// <value>
    ///     The list of proximity values.
    /// </value>
    /// <remarks>
    ///     The ProximityValue property holds a list of IntValues that represent the proximity values used in the advanced
    ///     candidate search.
    ///     These values are used to determine the geographical closeness of the candidates to a certain location.
    /// </remarks>
    private List<IntValues> ProximityValue
    {
        get;
        //set;
    } = [];

    /// <summary>
    ///     Gets or sets the list of key-value pairs for the Relocate dropdown.
    /// </summary>
    /// <remarks>
    ///     This property is used to populate the Relocate dropdown in the advanced candidate search component.
    ///     Each KeyValues object in the list represents a single dropdown option, where the key is the option's value and the
    ///     value is the option's display text.
    /// </remarks>
    private List<KeyValues> RelocateDropDown
    {
        get;
        set;
    } = [];

    /// <summary>
    ///     Gets or sets the event callback for the save action in the advanced candidate search form.
    /// </summary>
    /// <value>
    ///     The event callback for the save action.
    /// </value>
    /// <remarks>
    ///     This property represents the event callback that is invoked when the save action is triggered in the advanced
    ///     candidate search form.
    ///     It is used to handle the saving of the search parameters and perform the search operation.
    /// </remarks>
    [Parameter]
    public EventCallback<EditContext> Save
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of security clearances for the dropdown menu in the advanced candidate search.
    /// </summary>
    /// <value>
    ///     The list of security clearances.
    /// </value>
    /// <remarks>
    ///     This property is used to populate the dropdown menu for selecting a security clearance level in the advanced
    ///     candidate search.
    ///     Each item in the list represents a different level of security clearance.
    /// </remarks>
    private List<KeyValues> SecurityClearanceDropDown
    {
        get;
        set;
    } = [];

    /// <summary>
    ///     Gets or sets the SfSpinner control used in the advanced candidate search form.
    /// </summary>
    /// <value>
    ///     The SfSpinner control used in the advanced candidate search form.
    /// </value>
    /// <remarks>
    ///     This property represents the SfSpinner control that provides a visual indication of the search operation's progress
    ///     in the advanced candidate search form.
    ///     The control is displayed when a search operation is in progress and hidden when the operation is complete.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the list of state values for the dropdown in the advanced candidate search.
    /// </summary>
    /// <value>
    ///     The list of state values.
    /// </value>
    /// <remarks>
    ///     This property is used to populate the state dropdown in the advanced candidate search.
    ///     Each item in the list represents a state that can be selected in the search criteria.
    /// </remarks>
    [Parameter]
    public List<IntValues> StateDropDown
    {
        get;
        set;
    } = [];

    /// <summary>
    ///     Cancels the ongoing search dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is triggered when the user clicks on the cancel button in the search dialog.
    ///     It calls the general cancel method, passing in the mouse event arguments, spinner, dialog footer, dialog, and
    ///     cancel callback.
    /// </remarks>
    private async Task CancelSearchDialog(MouseEventArgs args) => await General.CallCancelMethod(args, Spinner, DialogFooter, Dialog, Cancel);

    /// <summary>
    ///     Asynchronously initializes the AdvancedCandidateSearch component.
    /// </summary>
    /// <remarks>
    ///     This method is called when the component is first initialized. It populates the ProximityValue and ProximityUnit
    ///     lists
    ///     with predefined values, and sets up the dropdown options for the SecurityClearanceDropDown and RelocateDropDown.
    ///     The method is asynchronous and returns a Task.
    /// </remarks>
    protected override async Task OnInitializedAsync()
    {
        await Task.Yield();
        ProximityValue.Add(new(1));
        ProximityValue.Add(new(5));
        ProximityValue.Add(new(10));
        ProximityValue.Add(new(20));
        ProximityValue.Add(new(25));
        ProximityValue.Add(new(30));
        ProximityValue.Add(new(40));
        ProximityValue.Add(new(50));
        ProximityValue.Add(new(60));
        ProximityValue.Add(new(70));
        ProximityValue.Add(new(80));
        ProximityValue.Add(new(90));
        ProximityValue.Add(new(100));
        ProximityValue.Add(new(125));
        ProximityValue.Add(new(150));
        ProximityValue.Add(new(175));
        ProximityValue.Add(new(200));
        ProximityValue.Add(new(250));
        ProximityValue.Add(new(300));
        ProximityValue.Add(new(400));
        ProximityValue.Add(new(500));
        ProximityValue.Add(new(600));
        ProximityValue.Add(new(700));
        ProximityValue.Add(new(800));
        ProximityValue.Add(new(900));
        ProximityValue.Add(new(1000));

        ProximityUnit.Add(new(1, "miles"));
        ProximityUnit.Add(new(2, "kilometers"));

        //StateDropDown.Add(new(0, "All"));

        //JobOptionsDropDown.Add(new("0", "All"));

        SecurityClearanceDropDown.Add(new("%", "All"));
        SecurityClearanceDropDown.Add(new("0", "No"));
        SecurityClearanceDropDown.Add(new("1", "Yes"));

        RelocateDropDown.Add(new("%", "All"));
        RelocateDropDown.Add(new("0", "No"));
        RelocateDropDown.Add(new("1", "Yes"));
    }

    /// <summary>
    ///     Opens the advanced candidate search dialog.
    /// </summary>
    /// <remarks>
    ///     This method is responsible for opening the advanced candidate search dialog.
    ///     Before the dialog is opened, it validates the current context of the EditSearchForm.
    /// </remarks>
    private void OpenDialog() => EditSearchForm.EditContext?.Validate();

    /// <summary>
    ///     Asynchronously executes the advanced candidate search operation.
    /// </summary>
    /// <param name="editContext">The edit context associated with the advanced candidate search form.</param>
    /// <remarks>
    ///     This method is responsible for executing the advanced candidate search operation. It uses the provided edit context
    ///     to validate the form and initiate the search operation. The search operation is performed by calling the
    ///     `General.CallSaveMethod` with the appropriate parameters.
    /// </remarks>
    private async Task SearchCandidateDialog(EditContext editContext) => await General.CallSaveMethod(editContext, Spinner, DialogFooter, Dialog, Save);

    /// <summary>
    ///     Initiates the display of the advanced candidate search dialog.
    /// </summary>
    /// <returns>
    ///     A Task that represents the asynchronous operation of showing the dialog.
    /// </returns>
    /// <remarks>
    ///     This method is used to asynchronously display the advanced candidate search dialog to the user.
    ///     It triggers the ShowAsync method of the SfDialog control, which is part of the advanced candidate search form.
    /// </remarks>
    internal async Task ShowDialog() => await Dialog.ShowAsync();

    /// <summary>
    ///     Represents a data adaptor for city and zip code autocomplete functionality in candidate search.
    /// </summary>
    /// <remarks>
    ///     The CandidateCityZipAdaptor class is a specialized data adaptor that provides asynchronous data retrieval for
    ///     city and zip code autocomplete feature in the advanced candidate search component. It uses the "GetCityZipList"
    ///     method for fetching the autocomplete data.
    /// </remarks>
    public class CandidateCityZipAdaptor : DataAdaptor
    {
        /// <summary>
        ///     Asynchronously reads data for the city and zip code autocomplete feature in the advanced candidate search
        ///     component.
        /// </summary>
        /// <param name="dm">The DataManagerRequest object containing additional request parameters.</param>
        /// <param name="key">
        ///     An optional key to further refine the data retrieval. This parameter is not used in the current
        ///     implementation.
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains a list of autocomplete options
        ///     in the form of KeyValues objects if any matches are found, or an empty list if no matches are found.
        ///     If the DataManagerRequest object requires counts, the task result is a DataResult object containing the
        ///     autocomplete options and their count.
        /// </returns>
        /// <remarks>
        ///     This method makes an asynchronous request to the 'GetCityZipList' method of the 'General' class,
        ///     passing the DataManagerRequest object as a parameter.
        ///     The response is a list of strings which are then converted into KeyValues objects and returned.
        ///     If an exception occurs during the operation, an empty list or a DataResult object with a count of 0 is returned.
        /// </remarks>
        public override Task<object> ReadAsync(DataManagerRequest dm, string key = null) => General.GetAutocompleteAsync("GetCityZipList", "@CityZip", dm);
    }
}