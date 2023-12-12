#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             LabelComponents
// File Name:           DropDownControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     08-31-2023 15:08
// *****************************************/

#endregion

#region Using

using Syncfusion.Blazor.Data;

#endregion

namespace RazorComponents;

/// <summary>
///     Represents a customizable dropdown control.
/// </summary>
/// <typeparam name="TValue">The type of the value that this dropdown control represents.</typeparam>
/// <typeparam name="TItem">The type of the items that can be selected in this dropdown control.</typeparam>
/// <remarks>
///     This dropdown control provides several customization options such as allowing filtering of items, wrapping the
///     dropdown in a div tag, creating a tooltip, and more.
///     It also provides properties to set the CSS class, data source, event callback for value change, ID, placeholder
///     text, query object for data retrieval, and an option to show a clear button.
/// </remarks>
public partial class DropDownControl<TValue, TItem>
{
	private SfDropDownList<TValue, TItem> _drop;
	private TValue _value;

	/// <summary>
	///     Gets or sets a value indicating whether the DropDown allows filtering of its items.
	/// </summary>
	/// <value>
	///     <c>true</c> if the DropDown allows filtering; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, a filter bar is displayed in the DropDown dropdown that allows the user
	///     to filter items by typing text.
	///     The filter type is set to "Contains", meaning that any item that contains the typed text will be displayed in the
	///     filtered list.
	/// </remarks>
	[Parameter]
	public bool AllowFilter
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the DropDown is wrapped in a div tag.
	/// </summary>
	/// <value>
	///     <c>true</c> if the DropDown is wrapped in a div tag; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, the DropDown is wrapped in a div tag which provides additional styling
	///     and layout options.
	///     If set to <c>false</c>, the DropDown is rendered without the wrapping div tag.
	/// </remarks>
	[Parameter]
	public bool CreateDivTag
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether a tooltip should be created for the DropDownControl.
	/// </summary>
	/// <value>
	///     <c>true</c> if a tooltip should be created; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to true, a tooltip is created and displayed when the user hovers over the
	///     DropDownControl.
	///     The tooltip provides additional information or guidance to the user.
	/// </remarks>
	/// <summary>
	///     Gets or sets a value indicating whether a tooltip should be created for the DropDownControl.
	/// </summary>
	/// <value>
	///     <c>true</c> if a tooltip should be created; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to true, a tooltip is created and displayed when the user hovers over the
	///     DropDownControl.
	///     The tooltip provides additional information or guidance to the user.
	/// </remarks>
	[Parameter]
	public bool CreateTooltip
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the CSS class for the DropDownControl component. This property allows for custom styling of the
	///     component.
	/// </summary>
	[Parameter]
	public string CssClass
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the data source for the DropDown.
	/// </summary>
	/// <value>
	///     The collection of items that is used as the data source for the DropDown.
	/// </value>
	/// <remarks>
	///     This property is used to bind a collection of items to the DropDown.
	///     The items in the collection are displayed in the dropdown list of the DropDown,
	///     and the user can select an item from this list.
	/// </remarks>
	[Parameter]
	public IEnumerable<TItem> DataSource
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is invoked when the value of the DropDown changes.
	/// </summary>
	/// <value>
	///     The event callback for the DropDown value change event.
	/// </value>
	/// <remarks>
	///     This event callback is invoked when the user selects a different item in the DropDown.
	///     The callback receives a ChangeEventArgs object that contains the old and new values.
	/// </remarks>
	[Parameter]
	public EventCallback<ChangeEventArgs<TValue, TItem>> DropDownValueChange
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID of the DropDown control.
	/// </summary>
	/// <value>
	///     The ID of the DropDown control.
	/// </value>
	/// <remarks>
	///     This property is used to assign a unique identifier to the DropDown control.
	///     The ID can be used for referencing the control in JavaScript and CSS.
	/// </remarks>
	[Parameter]
	public string ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the placeholder text for the DropDown control.
	/// </summary>
	/// <value>
	///     The placeholder text for the DropDown control.
	/// </value>
	/// <remarks>
	///     This property is used to display a short hint in the DropDown's input field before the user enters a value.
	///     The placeholder text is displayed when the DropDown is empty and loses focus.
	/// </remarks>
	[Parameter]
	public string Placeholder
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the Query object that defines the parameters for data retrieval from the data source.
	/// </summary>
	/// <value>
	///     The Query object that defines the parameters for data retrieval.
	/// </value>
	/// <remarks>
	///     This property is used to specify the parameters for data retrieval from the data source.
	///     The Query object can include parameters such as sorting, filtering, and paging,
	///     which are applied when retrieving data from the data source.
	/// </remarks>
	[Parameter]
	public Query Query
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the DropDown displays a clear button.
	/// </summary>
	/// <value>
	///     <c>true</c> if the DropDown displays a clear button; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, a clear button is displayed in the DropDown.
	///     This button, when clicked, clears the current selection in the DropDown.
	/// </remarks>
	[Parameter]
	public bool ShowClearButton
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the property to be used as the display text in the DropDown.
	/// </summary>
	/// <value>
	///     The name of the property to be used as the display text.
	/// </value>
	/// <remarks>
	///     This property is used to specify which property of the items in the DropDown's data source should be used as the
	///     display text.
	///     The display text is shown in the DropDown's input field and in the dropdown list.
	/// </remarks>
	[Parameter]
	public string TextField
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that determines the validation message for the DropDownControl component.
	///     This property allows for setting a custom validation message based on the selected date.
	/// </summary>
	[Parameter]
	public Expression<Func<TValue>> ValidationMessage
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the selected value in the DropDown.
	/// </summary>
	/// <value>
	///     The selected value in the DropDown.
	/// </value>
	/// <remarks>
	///     This property is used to bind a value to the DropDown.
	///     The value corresponds to the selected item in the DropDown.
	///     If the selected value changes, the ValueChanged event is invoked.
	/// </remarks>
	[Parameter]
	public TValue Value
	{
		get => _value;
		set
		{
			if (EqualityComparer<TValue>.Default.Equals(value, _value))
			{
				return;
			}

			_value = value;
			ValueChanged.InvokeAsync(value);
		}
	}

	/// <summary>
	///     Gets or sets the event callback that is invoked when the DropDown's selected value changes.
	/// </summary>
	/// <value>
	///     The event callback for the DropDown value change event.
	/// </value>
	/// <remarks>
	///     This event callback is invoked when the user selects a different item in the DropDown, causing the selected value
	///     to change.
	///     The callback receives the new selected value as an argument.
	/// </remarks>
	[Parameter]
	public EventCallback<TValue> ValueChanged
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that identifies the value to be bound to the DropDown.
	/// </summary>
	/// <value>
	///     An expression that identifies the value to be bound to the DropDown.
	/// </value>
	/// <remarks>
	///     This property is used to bind a value to the DropDown. The expression is typically a lambda expression
	///     that identifies the property of a model object that provides the value. The DropDown's selected item
	///     will be used to update this value.
	/// </remarks>
	[Parameter]
	public Expression<Func<TValue>> ValueExpression
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the field in the data source that provides the values for the DropDown.
	/// </summary>
	/// <value>
	///     The name of the field that provides the values for the DropDown.
	/// </value>
	/// <remarks>
	///     This property is used to bind a field in the data source to the value of the DropDown items.
	///     The value of this field is used as the value for each item in the DropDown.
	///     When an item is selected in the DropDown, the value of this field is used as the selected value.
	/// </remarks>
	[Parameter]
	public string ValueField
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the width of the DropDown control.
	/// </summary>
	/// <value>
	///     A string that represents the width of the DropDown control. The default value is "98%".
	/// </value>
	/// <remarks>
	///     This property is used to set the width of the DropDown control. The width can be set in percentages or pixels.
	///     For example, "50%" will make the DropDown control take up half of the available width, while "200px" will make the
	///     DropDown control 200 pixels wide.
	///     If this property is not set, the DropDown control will take up 98% of the available width by default.
	/// </remarks>
	[Parameter]
	public string Width
	{
		get;
		set;
	} = "98%";

	/// <summary>
	///     Asynchronously refreshes the data in the DropDown control.
	/// </summary>
	/// <remarks>
	///     This method is used to refresh the data in the DropDown control by re-fetching the data from the data source.
	///     It is useful when the data source has been updated and the changes need to be reflected in the DropDown.
	/// </remarks>
	public void Refresh()
	{
		_drop.RefreshDataAsync();
	}

	/// <summary>
	///     Handles the opening event of the tooltip in the DateControl component.
	///     This method cancels the opening of the tooltip if it does not contain any text.
	/// </summary>
	/// <param name="args">The arguments for the tooltip event, which include information about the tooltip's state and text.</param>
	private void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}