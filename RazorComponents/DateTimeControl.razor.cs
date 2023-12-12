#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             LabelComponents
// File Name:           DateTimeControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     08-31-2023 14:59
// *****************************************/

#endregion

namespace RazorComponents;

/// <summary>
///     Represents a reusable UI component for date and time input. This component provides various customization options
///     such as CSS styling, tooltips, and validation.
/// </summary>
public partial class DateTimeControl
{
	private DateTime _value;

	/// <summary>
	///     Gets or sets a value indicating whether to create a div tag for the DateTimeControl. If set to true, a div tag is
	///     created; otherwise, no div tag is created.
	/// </summary>
	/// <value>
	///     True if a div tag should be created; otherwise, false. The default value is true.
	/// </value>
	[Parameter]
	public bool CreateDivTag
	{
		get;
		set;
	} = true;

	/// <summary>
	///     Gets or sets a value indicating whether to create a tooltip for the DateTimeControl. If set to true, a tooltip is
	///     created; otherwise, no tooltip is created.
	/// </summary>
	/// <value>
	///     True if a tooltip should be created; otherwise, false. The default value is true.
	/// </value>
	[Parameter]
	public bool CreateTooltip
	{
		get;
		set;
	} = true;

	/// <summary>
	///     Gets or sets the CSS class for the DateTimePicker control. This property allows for additional styling of the
	///     DateTimePicker control.
	/// </summary>
	[Parameter]
	public string CssClass
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the instance of the Syncfusion DateTimePicker component used in the DateTimeControl.
	///     This component provides a user-friendly interface for date and time selection.
	/// </summary>
	private SfDateTimePicker<DateTime> DateTimeBox
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID of the DateTimeControl. This ID is used as the 'for' attribute value for the label and the
	///     'Target' attribute value for the tooltip in the HTML markup.
	/// </summary>
	[Parameter]
	public string ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the maximum date that can be selected in the DateTimeControl component.
	///     This property allows for setting an upper limit on the date range.
	///     The default value is DateTime.MaxValue, which represents the latest date and time supported by the DateTime type.
	/// </summary>
	[Parameter]
	public DateTime Max
	{
		get;
		set;
	} = DateTime.MaxValue;

	/// <summary>
	///     Gets or sets the minimum date that can be selected in the DateTime control.
	///     This is used to limit the date range for user selection.
	///     By default, this is set to the minimum value of DateTime.
	/// </summary>
	[Parameter]
	public DateTime Min
	{
		get;
		set;
	} = DateTime.MinValue;

	/// <summary>
	///     Gets or sets the placeholder text for the DateTimeControl component.
	///     This property allows for setting a display text in the date input field when it is empty.
	/// </summary>
	[Parameter]
	public string Placeholder
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the DateTime control is read-only.
	///     If this property is set to true, the DateTime control will be in a read-only state and cannot be modified by the
	///     user.
	///     This property is bound to the Readonly attribute of the SfDateTimePicker component in the Razor view.
	/// </summary>
	[Parameter]
	public bool Readonly
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that determines the validation message for the DateTimeControl component.
	///     This property allows for setting a custom validation message based on the selected date.
	/// </summary>
	[Parameter]
	public Expression<Func<string>> ValidationMessage
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the selected date in the DateTimeControl component.
	///     This property is bound to the Value property of the SfDateTimePicker component in the Razor view.
	///     When the value is set, it checks for equality with the current value. If the new value is different, it updates the
	///     current value and invokes the ValueChanged event.
	/// </summary>
	[Parameter]
	public DateTime Value
	{
		get => _value;
		set
		{
			if (EqualityComparer<DateTime?>.Default.Equals(value, _value))
			{
				return;
			}

			_value = value;
			ValueChanged.InvokeAsync(value);
		}
	}

	/// <summary>
	///     Gets or sets the ValueChanged event. This event is triggered when the DateTime value in the control is changed by
	///     the
	///     user.
	///     The event provides the new DateTime value selected by the user.
	/// </summary>
	[Parameter]
	public EventCallback<DateTime> ValueChanged
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that identifies the value to be used by the DateTime control.
	///     This expression is used to bind the date control's value to a specified property.
	/// </summary>
	[Parameter]
	public Expression<Func<DateTime>> ValueExpression
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the width of the DateTimeControl component.
	///     This property allows for setting a custom width for the component.
	///     The width can be specified in any valid CSS unit (e.g., px, %, em).
	///     The default value is "30%".
	/// </summary>
	[Parameter]
	public string Width
	{
		get;
		set;
	} = "30%";

	/// <summary>
	///     Handles the focus event of the date picker control.
	///     When the date picker control receives focus, this method triggers the display of the date picker popup.
	/// </summary>
	/// <param name="args">The event arguments associated with the focus event.</param>
	private async Task DTFocus(FocusEventArgs args)
	{
		await Task.Delay(1);
		await DateTimeBox.ShowDatePopupAsync();
	}

	/// <summary>
	///     Handles the opening event of the tooltip in the DateTimeControl component.
	///     This method cancels the opening of the tooltip if it does not contain any text.
	/// </summary>
	/// <param name="args">The arguments for the tooltip event, which include information about the tooltip's state and text.</param>
	private static void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}