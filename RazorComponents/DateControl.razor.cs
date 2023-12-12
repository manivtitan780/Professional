#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             LabelComponents
// File Name:           DateControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     08-31-2023 14:50
// *****************************************/

#endregion

namespace RazorComponents;

/// <summary>
///     Represents a date control component in the MyFeature area of the LabelComponents project.
///     This control provides a user interface for selecting and manipulating dates.
///     It includes parameters for customization such as CSS class, ID, maximum and minimum date,
///     placeholder text, read-only status, validation message, and width.
///     It also provides events for handling changes, opening, and closing of the date control.
/// </summary>
public partial class DateControl
{
	private DateTime _value;

	/// <summary>
	///     Gets or sets a value indicating whether a div tag should be created for the date control.
	///     If set to true, a div tag will be created. Default value is true.
	/// </summary>
	[Parameter]
	public bool CreateDivTag
	{
		get;
		set;
	} = true;

	/// <summary>
	///     Gets or sets a value indicating whether a tooltip should be created for the date control.
	///     If set to true, a tooltip will be created. Default value is true.
	/// </summary>
	[Parameter]
	public bool CreateTooltip
	{
		get;
		set;
	} = true;

	/// <summary>
	///     Gets or sets the CSS class for the DateControl component. This property allows for custom styling of the component.
	/// </summary>
	[Parameter]
	public string CssClass
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the SfDatePicker component of the DateControl.
	///     This component is used for date selection and manipulation within the DateControl.
	/// </summary>
	private SfDatePicker<DateTime> DateBox
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID for the DateControl component. This property allows for unique identification of the component.
	/// </summary>
	[Parameter]
	public string ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the maximum date that can be selected in the DateControl component.
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
	///     Gets or sets the minimum date that can be selected in the date control.
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
	///     Gets or sets the event callback that is triggered when the date value in the DateControl component changes.
	///     This event receives a ChangedEventArgs&lt;DateTime&gt; object which contains the old and new date values.
	/// </summary>
	[Parameter]
	public EventCallback<ChangedEventArgs<DateTime>> OnChange
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when the date control is closed.
	///     This event receives a <see cref="PopupObjectArgs" /> argument, which contains data related to the close event.
	/// </summary>
	[Parameter]
	public EventCallback<PopupObjectArgs> OnClose
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when the date control is opened.
	///     This event callback provides a <see cref="Syncfusion.Blazor.Calendars.PopupObjectArgs" /> object
	///     which contains information about the event.
	/// </summary>
	[Parameter]
	public EventCallback<PopupObjectArgs> OnOpen
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the placeholder text for the DateControl component.
	///     This property allows for setting a display text in the date input field when it is empty.
	/// </summary>
	[Parameter]
	public string Placeholder
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the date control is read-only.
	///     If this property is set to true, the date control will be in a read-only state and cannot be modified by the user.
	///     This property is bound to the Readonly attribute of the SfDatePicker component in the Razor view.
	/// </summary>
	[Parameter]
	public bool Readonly
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the date control should validate the input as the user types.
	///     If set to true, the control will validate the input immediately upon user input.
	///     If set to false, the control will validate the input only when the user finishes typing.
	/// </summary>
	[Parameter]
	public bool ValidateOnInput
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that determines the validation message for the DateControl component.
	///     This property allows for setting a custom validation message based on the selected date.
	/// </summary>
	[Parameter]
	public Expression<Func<DateTime>> ValidationMessage
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the selected date in the DateControl component.
	///     This property is bound to the Value property of the SfDatePicker component in the Razor view.
	///     When the value is set, it checks for equality with the current value. If the new value is different, it updates the
	///     current value and invokes the ValueChanged event.
	/// </summary>
	[Parameter]
	public DateTime Value
	{
		get => _value;
		set
		{
			if (EqualityComparer<DateTime>.Default.Equals(value, _value))
			{
				return;
			}

			_value = value;
			ValueChanged.InvokeAsync(value);
		}
	}

	/// <summary>
	///     Gets or sets the ValueChanged event. This event is triggered when the date value in the control is changed by the
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
	///     Gets or sets the expression that identifies the value to be used by the date control.
	///     This expression is used to bind the date control's value to a specified property.
	/// </summary>
	[Parameter]
	public Expression<Func<DateTime>> ValueExpression
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the width of the DateControl component.
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
	private void DTFocus(FocusEventArgs args)
	{
		DateBox.ShowPopupAsync();
	}

	/// <summary>
	///     Handles the opening event of the tooltip in the DateControl component.
	///     This method cancels the opening of the tooltip if it does not contain any text.
	/// </summary>
	/// <param name="args">The arguments for the tooltip event, which include information about the tooltip's state and text.</param>
	private static void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}