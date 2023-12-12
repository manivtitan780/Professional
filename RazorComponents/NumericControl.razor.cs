#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             LabelComponents
// File Name:           NumericControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     08-31-2023 16:01
// *****************************************/

#endregion

namespace RazorComponents;

/// <summary>
///     Represents a NumericControl component that allows users to input numeric values within a specified range.
/// </summary>
/// <typeparam name="TValue">The type of the value that the NumericControl component represents.</typeparam>
/// <remarks>
///     The NumericControl component provides several parameters for customization, including:
///     - The ability to create a div tag and a tooltip for the NumericControl.
///     - The ability to set the CSS class, currency, number of decimals, and format for the NumericControl.
///     - The ability to set the ID, maximum and minimum values, and placeholder text for the NumericControl.
///     - The ability to make the NumericControl read-only and to show a spin button.
///     - The ability to set the step value, validation message, and width of the NumericControl.
/// </remarks>
public partial class NumericControl<TValue>
{
	private TValue _value;

	/// <summary>
	///     Gets or sets a value indicating whether to create a div tag for the NumericControl. If set to true, a div tag is
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
	///     Gets or sets a value indicating whether to create a tooltip for the NumericControl. If set to true, a tooltip is
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
	///     Gets or sets the CSS class for the NumericPicker control. This property allows for additional styling of the
	///     NumericPicker control.
	/// </summary>
	[Parameter]
	public string CssClass
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the currency for the NumericControl component.
	/// </summary>
	/// <value>
	///     A string that represents the currency for the NumericControl. The default value is "USD".
	/// </value>
	[Parameter]
	public string Currency
	{
		get;
		set;
	} = "USD";

	/// <summary>
	///     Gets or sets the number of decimal places for the NumericControl component.
	/// </summary>
	/// <value>
	///     An integer that represents the number of decimal places. The default value is 0.
	/// </value>
	[Parameter]
	public int? Decimals
	{
		get;
		set;
	} = 0;

	/// <summary>
	///     Gets or sets the format for the NumericControl component.
	/// </summary>
	/// <value>
	///     A string that represents the format for the NumericControl. The default value is "n", which stands for numeric
	///     format.
	/// </value>
	[Parameter]
	public string Format
	{
		get;
		set;
	} = "n";

	/// <summary>
	///     Gets or sets the ID of the NumericControl. This ID is used as the 'for' attribute value for the label and the
	///     'Target' attribute value for the tooltip in the HTML markup.
	/// </summary>
	[Parameter]
	public string ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the maximum date that can be selected in the NumericControl component.
	///     This property allows for setting an upper limit on the date range.
	///     The default value is Numeric.MaxValue, which represents the latest date and time supported by the Numeric type.
	/// </summary>
	[Parameter]
	public TValue Max
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the minimum date that can be selected in the Numeric control.
	///     This is used to limit the date range for user selection.
	///     By default, this is set to the minimum value of Numeric.
	/// </summary>
	[Parameter]
	public TValue Min
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the event callback that is triggered when the value of the NumericControl component changes.
	/// </summary>
	/// <value>
	///     An EventCallback&lt;ChangeEventArgs&gt; that represents the method to be called when the NumericControl value
	///     changes.
	/// </value>
	[Parameter]
	public EventCallback<ChangeEventArgs> OnChanged
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the placeholder text for the NumericControl component.
	///     This property allows for setting a display text in the date input field when it is empty.
	/// </summary>
	[Parameter]
	public string Placeholder
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the Numeric control is read-only.
	///     If this property is set to true, the Numeric control will be in a read-only state and cannot be modified by the
	///     user.
	///     This property is bound to the Readonly attribute of the SfNumericPicker component in the Razor view.
	/// </summary>
	[Parameter]
	public bool Readonly
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether to display a spin button for the NumericControl. If set to true, a spin
	///     button is
	///     displayed; otherwise, no spin button is displayed.
	/// </summary>
	/// <value>
	///     True if a spin button should be displayed; otherwise, false. The default value is false.
	/// </value>
	[Parameter]
	public bool ShowSpinButton
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the step value for the NumericControl. The step value determines the increments for each step when the
	///     spin button is used.
	/// </summary>
	/// <value>
	///     The step value for the NumericControl. The type of this value is determined by the TValue type parameter of the
	///     NumericControl.
	/// </value>
	[Parameter]
	public TValue Step
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that determines the validation message for the NumericControl component.
	///     This property allows for setting a custom validation message based on the selected date.
	/// </summary>
	[Parameter]
	public Expression<Func<TValue>> ValidationMessage
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the selected date in the NumericControl component.
	///     This property is bound to the Value property of the SfNumericPicker component in the Razor view.
	///     When the value is set, it checks for equality with the current value. If the new value is different, it updates the
	///     current value and invokes the ValueChanged event.
	/// </summary>
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
	///     Gets or sets the ValueChanged event. This event is triggered when the Numeric value in the control is changed by
	///     the
	///     user.
	///     The event provides the new Numeric value selected by the user.
	/// </summary>
	[Parameter]
	public EventCallback<TValue> ValueChanged
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that identifies the value to be used by the Numeric control.
	///     This expression is used to bind the date control's value to a specified property.
	/// </summary>
	[Parameter]
	public Expression<Func<TValue>> ValueExpression
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the width of the NumericControl component.
	///     This property allows for setting a custom width for the component.
	///     The width can be specified in any valid CSS unit (e.g., px, %, em).
	///     The default value is "30%".
	/// </summary>
	[Parameter]
	public string Width
	{
		get;
		set;
	} = "100%";

	/// <summary>
	///     Handles the opening event of the tooltip in the DateControl component.
	///     This method cancels the opening of the tooltip if it does not contain any text.
	/// </summary>
	/// <param name="args">The arguments for the tooltip event, which include information about the tooltip's state and text.</param>
	private static void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}