#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             LabelComponents
// File Name:           MaskedTextBoxControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     08-31-2023 15:09
// *****************************************/

#endregion

namespace RazorComponents;

/// <summary>
///     Represents a masked text box control in the MyFeature area of the LabelComponents project.
///     This control allows user input to follow a certain format defined by a mask.
///     The class includes parameters for customization such as CssClass, ID, Mask, Placeholder, Readonly status,
///     ValidationMessage, Value, ValueChanged event, ValueExpression, and Width.
///     It is used in the EditActivityDialog and EditCandidateDialog of the ProfSvc_AppTrack project.
/// </summary>
public partial class MaskedTextBoxControl
{
	private string _value;

	/// <summary>
	///     Gets or sets a value indicating whether the MaskedTextBox is wrapped in a div tag.
	/// </summary>
	/// <value>
	///     <c>true</c> if the MaskedTextBox is wrapped in a div tag; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, the MaskedTextBox is wrapped in a div tag which provides additional
	///     styling
	///     and layout options.
	///     If set to <c>false</c>, the MaskedTextBox is rendered without the wrapping div tag.
	/// </remarks>
	[Parameter]
	public bool CreateDivTag
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether a tooltip should be created for the MaskedTextBoxControl.
	/// </summary>
	/// <value>
	///     <c>true</c> if a tooltip should be created; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to true, a tooltip is created and displayed when the user hovers over the
	///     MaskedTextBoxControl.
	///     The tooltip provides additional information or guidance to the user.
	/// </remarks>
	/// <summary>
	///     Gets or sets a value indicating whether a tooltip should be created for the MaskedTextBoxControl.
	/// </summary>
	/// <value>
	///     <c>true</c> if a tooltip should be created; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to true, a tooltip is created and displayed when the user hovers over the
	///     MaskedTextBoxControl.
	///     The tooltip provides additional information or guidance to the user.
	/// </remarks>
	[Parameter]
	public bool CreateTooltip
	{
		get;
		set;
	} = true;

	/// <summary>
	///     Gets or sets the CSS class for the MaskedTextBoxControl component. This property allows for custom styling of the
	///     component.
	/// </summary>
	[Parameter]
	public string CssClass
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the ID of the MaskedTextBox control.
	/// </summary>
	/// <value>
	///     The ID of the MaskedTextBox control.
	/// </value>
	/// <remarks>
	///     This property is used to assign a unique identifier to the MaskedTextBox control.
	///     The ID can be used for referencing the control in JavaScript and CSS.
	/// </remarks>
	[Parameter]
	public string ID
	{
		get;
		set;
	} = "maskedControl";

	/// <summary>
	///     Gets or sets the mask string for the MaskedTextBoxControl component. This property defines the input pattern
	///     that the user is forced to follow.
	/// </summary>
	/// <value>
	///     A string that represents the mask pattern.
	/// </value>
	/// <remarks>
	///     The mask is a string made up of characters from a predefined mask language. This mask language
	///     defines which characters are allowed in each position of the user input. For example, a mask of '00/00/0000'
	///     could be used to enter a date format.
	/// </remarks>
	[Parameter]
	public string Mask
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the placeholder text for the MaskedTextBox control.
	/// </summary>
	/// <value>
	///     The placeholder text for the MaskedTextBox control.
	/// </value>
	/// <remarks>
	///     This property is used to display a short hint in the MaskedTextBox's input field before the user enters a value.
	///     The placeholder text is displayed when the MaskedTextBox is empty and loses focus.
	/// </remarks>
	[Parameter]
	public string Placeholder
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the MaskedTextBoxControl is read-only.
	/// </summary>
	/// <value>
	///     <c>true</c> if the MaskedTextBoxControl is read-only; otherwise, <c>false</c>.
	/// </value>
	/// <remarks>
	///     When this property is set to <c>true</c>, the MaskedTextBoxControl will be read-only and user input will be
	///     disabled.
	///     If set to <c>false</c>, the MaskedTextBoxControl will be editable and user input will be enabled.
	/// </remarks>
	[Parameter]
	public bool Readonly
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that determines the validation message for the MaskedTextBoxControl component.
	///     This property allows for setting a custom validation message based on the selected date.
	/// </summary>
	[Parameter]
	public Expression<Func<string>> ValidationMessage
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the selected value in the MaskedTextBox.
	/// </summary>
	/// <value>
	///     The selected value in the MaskedTextBox.
	/// </value>
	/// <remarks>
	///     This property is used to bind a value to the MaskedTextBox.
	///     The value corresponds to the selected item in the MaskedTextBox.
	///     If the selected value changes, the ValueChanged event is invoked.
	/// </remarks>
	[Parameter]
	public string Value
	{
		get => _value;
		set
		{
			if (EqualityComparer<string>.Default.Equals(value, _value))
			{
				return;
			}

			_value = value;
			ValueChanged.InvokeAsync(value);
		}
	}

	/// <summary>
	///     Gets or sets the event callback that is invoked when the MaskedTextBox's selected value changes.
	/// </summary>
	/// <value>
	///     The event callback for the MaskedTextBox value change event.
	/// </value>
	/// <remarks>
	///     This event callback is invoked when the user selects a different item in the MaskedTextBox, causing the selected
	///     value
	///     to change.
	///     The callback receives the new selected value as an argument.
	/// </remarks>
	[Parameter]
	public EventCallback<string> ValueChanged
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the expression that identifies the value to be bound to the MaskedTextBox.
	/// </summary>
	/// <value>
	///     An expression that identifies the value to be bound to the MaskedTextBox.
	/// </value>
	/// <remarks>
	///     This property is used to bind a value to the MaskedTextBox. The expression is typically a lambda expression
	///     that identifies the property of a model object that provides the value. The MaskedTextBox's selected item
	///     will be used to update this value.
	/// </remarks>
	[Parameter]
	public Expression<Func<string>> ValueExpression
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the width of the MaskedTextBox control.
	/// </summary>
	/// <value>
	///     A string that represents the width of the MaskedTextBox control. The default value is "98%".
	/// </value>
	/// <remarks>
	///     This property is used to set the width of the MaskedTextBox control. The width can be set in percentages or pixels.
	///     For example, "50%" will make the MaskedTextBox control take up half of the available width, while "200px" will make
	///     the
	///     MaskedTextBox control 200 pixels wide.
	///     If this property is not set, the MaskedTextBox control will take up 98% of the available width by default.
	/// </remarks>
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