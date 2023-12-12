#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             LabelComponents
// File Name:           ComboBoxControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     08-30-2023 21:32
// *****************************************/

#endregion

#region Using

using Syncfusion.Blazor.Data;

#endregion

namespace RazorComponents;

/// <summary>
///     Represents a custom ComboBox control that allows for selection and filtering of items.
/// </summary>
/// <typeparam name="TValue">The type of the value that ComboBox is bound to.</typeparam>
/// <typeparam name="TItem">The type of the items in the ComboBox's data source.</typeparam>
/// <remarks>
///     The ComboBoxControl provides a user interface for selecting an item from a list.
///     It includes features such as filtering, custom item rendering, and binding to a value.
/// </remarks>
public partial class ComboBoxControl<TValue, TItem>
{
	private SfComboBox<TValue, TItem> _combo;
	private TValue _value;

	/// <summary>
	///     Gets or sets a value indicating whether the ComboBox allows filtering of its items.
	/// </summary>
	/// <value>
	///     <c>true</c> if the ComboBox allows filtering; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, a filter bar is displayed in the ComboBox dropdown that allows the user
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
	///     Gets or sets the event callback that is invoked when the value of the ComboBox changes.
	/// </summary>
	/// <value>
	///     The event callback for the ComboBox value change event.
	/// </value>
	/// <remarks>
	///     This event callback is invoked when the user selects a different item in the ComboBox.
	///     The callback receives a ChangeEventArgs object that contains the old and new values.
	/// </remarks>
	[Parameter]
	public EventCallback<ChangeEventArgs<TValue, TItem>> ComboValueChange
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the ComboBox is wrapped in a div tag.
	/// </summary>
	/// <value>
	///     <c>true</c> if the ComboBox is wrapped in a div tag; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, the ComboBox is wrapped in a div tag which provides additional styling
	///     and layout options.
	///     If set to <c>false</c>, the ComboBox is rendered without the wrapping div tag.
	/// </remarks>
	[Parameter]
	public bool CreateDivTag
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the data source for the ComboBox.
	/// </summary>
	/// <value>
	///     The collection of items that is used as the data source for the ComboBox.
	/// </value>
	/// <remarks>
	///     This property is used to bind a collection of items to the ComboBox.
	///     The items in the collection are displayed in the dropdown list of the ComboBox,
	///     and the user can select an item from this list.
	/// </remarks>
	[Parameter]
	public IEnumerable<TItem> DataSource
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID of the ComboBox control.
	/// </summary>
	/// <value>
	///     The ID of the ComboBox control.
	/// </value>
	/// <remarks>
	///     This property is used to assign a unique identifier to the ComboBox control.
	///     The ID can be used for referencing the control in JavaScript and CSS.
	/// </remarks>
	[Parameter]
	public string ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the placeholder text for the ComboBox control.
	/// </summary>
	/// <value>
	///     The placeholder text for the ComboBox control.
	/// </value>
	/// <remarks>
	///     This property is used to display a short hint in the ComboBox's input field before the user enters a value.
	///     The placeholder text is displayed when the ComboBox is empty and loses focus.
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
	///     Gets or sets a value indicating whether the ComboBox displays a clear button.
	/// </summary>
	/// <value>
	///     <c>true</c> if the ComboBox displays a clear button; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, a clear button is displayed in the ComboBox.
	///     This button, when clicked, clears the current selection in the ComboBox.
	/// </remarks>
	[Parameter]
	public bool ShowClearButton
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the name of the property to be used as the display text in the ComboBox.
	/// </summary>
	/// <value>
	///     The name of the property to be used as the display text.
	/// </value>
	/// <remarks>
	///     This property is used to specify which property of the items in the ComboBox's data source should be used as the
	///     display text.
	///     The display text is shown in the ComboBox's input field and in the dropdown list.
	/// </remarks>
	[Parameter]
	public string TextField
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the selected value in the ComboBox.
	/// </summary>
	/// <value>
	///     The selected value in the ComboBox.
	/// </value>
	/// <remarks>
	///     This property is used to bind a value to the ComboBox.
	///     The value corresponds to the selected item in the ComboBox.
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
	///     Gets or sets the event callback that is invoked when the ComboBox's selected value changes.
	/// </summary>
	/// <value>
	///     The event callback for the ComboBox value change event.
	/// </value>
	/// <remarks>
	///     This event callback is invoked when the user selects a different item in the ComboBox, causing the selected value
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
	///     Gets or sets the expression that identifies the value to be bound to the ComboBox.
	/// </summary>
	/// <value>
	///     An expression that identifies the value to be bound to the ComboBox.
	/// </value>
	/// <remarks>
	///     This property is used to bind a value to the ComboBox. The expression is typically a lambda expression
	///     that identifies the property of a model object that provides the value. The ComboBox's selected item
	///     will be used to update this value.
	/// </remarks>
	[Parameter]
	public Expression<Func<TValue>> ValueExpression
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the field in the data source that provides the values for the ComboBox.
	/// </summary>
	/// <value>
	///     The name of the field that provides the values for the ComboBox.
	/// </value>
	/// <remarks>
	///     This property is used to bind a field in the data source to the value of the ComboBox items.
	///     The value of this field is used as the value for each item in the ComboBox.
	///     When an item is selected in the ComboBox, the value of this field is used as the selected value.
	/// </remarks>
	[Parameter]
	public string ValueField
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the width of the ComboBox control.
	/// </summary>
	/// <value>
	///     A string that represents the width of the ComboBox control. The default value is "98%".
	/// </value>
	/// <remarks>
	///     This property is used to set the width of the ComboBox control. The width can be set in percentages or pixels.
	///     For example, "50%" will make the ComboBox control take up half of the available width, while "200px" will make the
	///     ComboBox control 200 pixels wide.
	///     If this property is not set, the ComboBox control will take up 98% of the available width by default.
	/// </remarks>
	[Parameter]
	public string Width
	{
		get;
		set;
	} = "98%";

	/// <summary>
	///     Asynchronously refreshes the data in the ComboBox control.
	/// </summary>
	/// <remarks>
	///     This method is used to refresh the data in the ComboBox control by re-fetching the data from the data source.
	///     It is useful when the data source has been updated and the changes need to be reflected in the ComboBox.
	/// </remarks>
	public async Task Refresh()
	{
		await _combo.RefreshDataAsync();
	}
}