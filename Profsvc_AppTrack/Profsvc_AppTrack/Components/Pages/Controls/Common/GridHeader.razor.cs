#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           GridHeader.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-08-2022 20:19
// Last Updated On:     09-26-2023 19:05
// *****************************************/

#endregion

using Profsvc_AppTrack.Components.Code;

namespace Profsvc_AppTrack.Components.Pages.Controls.Common;

/// <summary>
///     Represents a grid header component in the application. This component provides various functionalities such as
///     advanced search, pagination, and filtering.
///     It includes parameters for handling mouse events, managing autocomplete features, controlling display settings, and
///     managing grid data.
/// </summary>
public partial class GridHeader
{
    private static string _method, _parameterName;

    private readonly List<IntValues> _showRecords =
        new() { new(10, "10 rows"), new(25, "25 rows"), new(50, "50 rows"), new(75, "75 rows"), new(100, "100 rows") };

    /// <summary>
    ///     Gets or sets the value for the AutoCompleteButton in the GridHeader component. This value is bound to the
    ///     AutoCompleteButton's Value property and is updated whenever the ValueChanged event is triggered.
    /// </summary>
    [Parameter]
    public string ACBValue
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the callback event that will be triggered when the 'Add' button is clicked in the GridHeader
    ///     component.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> Add
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the callback event that will be triggered when the 'Advanced Search' button is clicked in the
    ///     GridHeader component.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> AdvancedSearch
    {
        get;
        set;
    }

    [Parameter]
    public bool AdvancedSearchDisabled
    {
        get;
        set;
    } = false;

    private AutoCompleteButton AutoCompleteControl
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the method name to be used for the autocomplete functionality in the GridHeader component.
    ///     This method is expected to provide suggestions for the autocomplete feature based on the input.
    /// </summary>
    [Parameter]
    public string AutocompleteMethod
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the parameter name to be used for the autocomplete functionality in the GridHeader component. <br />
    ///     This parameter is used in conjunction with the AutocompleteMethod to provide suggestions for the autocomplete
    ///     feature.
    /// </summary>
    [Parameter]
    public string AutocompleteParameterName
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the unique identifier for the GridHeader control. This identifier is used to distinguish this instance
    ///     of the GridHeader control from other instances in the application.
    /// </summary>
    [Parameter]
    public string ControlID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the current page number in the GridHeader component. This property is bound to the Value property of
    ///     the NumericControl component and is updated whenever the ValueChanged event is triggered.
    /// </summary>
    [Parameter]
    public int CurrentPage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the display style of the 'Add' button in the GridHeader component.
    ///     This property controls the visibility of the 'Add' button. The default value is "none", which means the 'Add'
    ///     button is not visible.
    /// </summary>
    [Parameter]
    public string DisplayAdd
    {
        get;
        set;
    } = "none";

    /// <summary>
    ///     Gets or sets the display style of the 'Submit' button in the GridHeader component.
    ///     This property controls the visibility of the 'Submit' button. The default value is "none", which means the 'Submit'
    ///     button is not visible.
    /// </summary>
    [Parameter]
    public string DisplaySubmit
    {
        get;
        set;
    } = "none";

    /// <summary>
    ///     Gets or sets the value that is bound to the dropdown control in the GridHeader component.
    ///     This value is updated whenever the ValueChanged event is triggered by the dropdown control.
    /// </summary>
    [Parameter]
    public int DropdownBindValue
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the name of the entity that the grid header is associated with. This property is used in the UI to
    ///     label various interactive elements related to the entity.
    /// </summary>
    [Parameter]
    public string Entity
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback for filtering the grid. This event is triggered when the filter criteria changes.
    ///     The event argument is of type <see cref="ChangeEventArgs{TValue, TKey}" /> where TValue is string representing the
    ///     filter criteria and TKey is a KeyValues object representing the filter keys.
    /// </summary>
    [Parameter]
    public EventCallback<ChangeEventArgs<string, KeyValues>> FilterGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the callback event that will be triggered when the 'First' button is clicked in the GridHeader
    ///     component.
    ///     This event is used for navigating to the first page of the grid.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> FirstClick
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the callback event that will be triggered when the 'Last' button is clicked in the GridHeader
    ///     component.
    ///     This event is used for navigating to the last page of the grid.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> LastClick
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the callback event that will be triggered when the 'Next' button is clicked in the GridHeader component.
    /// This event is used for navigating to the next page of the grid.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> NextClick
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the total number of pages in the grid. This property is used for pagination purposes.
    /// </summary>
    [Parameter]
    public int PageCount
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the page number changes in the grid header.
    /// </summary>
    /// <value>
    ///     The event callback for the page number change event.
    /// </value>
    [Parameter]
    public EventCallback<ChangeEventArgs> PageNumberChanged
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that will be triggered when the 'Previous' button is clicked in the GridHeader
    ///     component. This event is typically used for navigating to the previous page in a paginated grid.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> PreviousClick
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the callback event that will be triggered when the 'Refresh' button is clicked in the GridHeader
    ///     component. This event is used to refresh the grid data.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> RefreshGrid
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the callback event that will be triggered when the 'Submit' button is clicked in the GridHeader
    ///     component. This event is used to submit the selected options or data in the grid.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> Submit
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets an event callback that is triggered when the value of the grid header changes.
    ///     The event callback contains the change event arguments, which include the old and new values.
    /// </summary>
    [Parameter]
    public EventCallback<ChangeEventArgs<int, IntValues>> ValueChange
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the width of the GridHeader component. This property is used to control the visual width of the grid header.
    ///     The value should be a valid CSS width property value.
    /// </summary>
    [Parameter]
    public string Width
    {
        get;
        set;
    }

    /// <summary>
    ///     This method is called after the component has finished rendering. It is used to set the autocomplete method and 
    ///     parameter name for the GridHeader component.
    /// </summary>
    /// <param name="firstRender">
    ///     A boolean value that indicates whether this is the first time the component is being rendered. 
    ///     If true, this is the first render; otherwise, it is a subsequent render.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation.
    /// </returns>
    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        _method = AutocompleteMethod;
        _parameterName = AutocompleteParameterName;
        return base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    ///     The DropDownAdaptor is a specialized DataAdaptor used for handling data operations for a dropdown control in a grid header.
    ///     It overrides the ReadAsync method to provide custom data retrieval logic for the dropdown, using the General.GetAutocompleteAsync method.
    /// </summary>
    public class DropDownAdaptor : DataAdaptor
    {
        /// <summary>
        ///     Asynchronously reads data for a dropdown control in a grid header.
        /// </summary>
        /// <param name="dm">The DataManagerRequest object containing additional request parameters.</param>
        /// <param name="key">An optional key to further refine the data retrieval.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result contains a list of autocomplete options
        ///     in the form of KeyValues objects if any matches are found, or an empty list if no matches are found.
        ///     If the DataManagerRequest object requires counts, the task result is a DataResult object containing the
        ///     autocomplete options and their count.
        /// </returns>
        /// <remarks>
        ///     This method uses the General.GetAutocompleteAsync method to retrieve the data.
        /// </remarks>
        public override Task<object> ReadAsync(DataManagerRequest dm, string key = null) => General.GetAutocompleteAsync(_method, _parameterName, dm);
    }
}