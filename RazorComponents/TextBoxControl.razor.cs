#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             LabelComponents
// File Name:           TextBoxControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          09-17-2022 20:01
// Last Updated On:     08-31-2023 19:03
// *****************************************/

#endregion

namespace RazorComponents;

/// <summary>
///     Represents a TextBoxControl component in the MyFeature area of the LabelComponents project.
///     This component is a part of the user interface and is used for accepting user input in a text format.
///     It provides various customization options such as setting the CSS class, height, ID, maximum and minimum length of
///     the input,
///     whether it's multiline or not, placeholder text, whether it's read-only or not, the number of rows,
///     the type of the input, tooltip, and whether to validate on input.
///     It also provides options for handling events such as when the component is created, when it loses focus, and when
///     it gains focus.
/// </summary>
public partial class TextBoxControl
{
    private string _value;

    /// <summary>
    ///     Gets or sets the instance of the Syncfusion TextBox component used in the TextBoxControl.
    ///     This instance is used to bind the component's properties and events to the TextBoxControl's properties and events.
    /// </summary>
    private SfTextBox Box
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the Created event. This event is triggered when the TextBoxControl component is created.
    ///     This can be used to perform any setup or initialization required for the component.
    /// </summary>
    [Parameter]
    public EventCallback<object> Created
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether to create a div tag for the TextBoxControl. If set to true, a div tag is
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
    ///     Gets or sets a value indicating whether to create a tooltip for the TextBoxControl. If set to true, a tooltip is
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
    ///     Gets or sets the CSS class for the TextBox control. This property allows for additional styling of the
    ///     TextBox control.
    /// </summary>
    [Parameter]
    public string CssClass
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the EditContext for the TextBoxControl component.
    ///     EditContext provides a context for tracking the validation state of a form and its inputs.
    ///     This context is used to interact with the form and its fields programmatically.
    /// </summary>
    [CascadingParameter]
    public EditContext EditContext
    {
        get;
        set;
    } = default!;

    /// <summary>
    ///     Gets or sets the height of the TextBoxControl component.
    ///     This property allows for setting a specific height for the TextBoxControl component.
    ///     The height is specified as a string and can take any valid CSS height value.
    ///     The default value is "inherit", which means the height of the TextBoxControl component will be the same as its
    ///     parent element's height.
    /// </summary>
    [Parameter]
    public string Height
    {
        get;
        set;
    } = "inherit";

    /// <summary>
    ///     Gets or sets the ID of the TextBoxControl. This ID is used as the 'for' attribute value for the label and the
    ///     'Target' attribute value for the tooltip in the HTML markup.
    /// </summary>
    [Parameter]
    public string ID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the maximum length of the input for the TextBoxControl component.
    ///     This property allows for limiting the number of characters that can be entered into the TextBoxControl.
    ///     The default value is 0, which means there is no limit on the length of the input.
    /// </summary>
    [Parameter]
    public int MaxLength
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the minimum length of the input for the TextBoxControl component.
    ///     This property allows for setting a lower limit on the number of characters that can be entered into the
    ///     TextBoxControl.
    ///     The default value is 0, which means there is no minimum length for the input.
    /// </summary>
    [Parameter]
    public int MinLength
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the TextBoxControl should accept multiple lines of input.
    ///     If this property is set to true, the TextBoxControl will allow the user to input text over multiple lines.
    ///     If it is set to false, the TextBoxControl will be a single-line input field.
    ///     The default value is false, which means the TextBoxControl is a single-line input field by default.
    /// </summary>
    [Parameter]
    public bool Multiline
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the TextBoxControl loses focus.
    ///     This event is triggered when the user clicks outside the TextBoxControl or moves the focus to another control.
    ///     The event handler receives an argument of type FocusOutEventArgs containing data related to this event.
    /// </summary>
    [Parameter]
    public EventCallback<FocusOutEventArgs> OnBlur
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the TextBoxControl gains focus.
    ///     The event handler receives an argument of type FocusInEventArgs, which contains data related to the focus event.
    /// </summary>
    [Parameter]
    public EventCallback<FocusInEventArgs> OnFocus
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the placeholder text for the TextBoxControl component.
    ///     This property allows for setting a display text in the date input field when it is empty.
    /// </summary>
    [Parameter]
    public string Placeholder
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the TextBox control is read-only.
    ///     If this property is set to true, the TextBox control will be in a read-only state and cannot be modified by the
    ///     user.
    ///     This property is bound to the Readonly attribute of the SfTextBox component in the Razor view.
    /// </summary>
    [Parameter]
    public bool Readonly
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the number of rows in the TextBoxControl component.
    ///     This property is applicable only when the Multiline property is set to true.
    ///     It allows for setting the visible number of lines in the TextBoxControl without needing to scroll.
    ///     The default value is 3.
    /// </summary>
    [Parameter]
    public byte Rows
    {
        get;
        set;
    } = 3;

    /// <summary>
    ///     Gets or sets the type of the text box input. This property allows for customization of the input type for the text
    ///     box.
    ///     The input type can be set to various options such as text, password, email, etc., according to the InputType
    ///     enumeration.
    ///     The default value is InputType.Text, which means the text box will accept any form of text input.
    /// </summary>
    [Parameter]
    public InputType TextBoxType
    {
        get;
        set;
    } = InputType.Text;

    /// <summary>
    ///     Gets or sets the tooltip text for the TextBoxControl component.
    ///     This property allows for setting a descriptive text that appears when the user hovers over the TextBoxControl.
    /// </summary>
    [Parameter]
    public string Tooltip
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the TextBoxControl should validate the input as it is being entered.
    ///     If set to true, the input will be validated in real-time as the user types. If set to false, the validation will
    ///     occur after the input is fully entered.
    ///     The default value is false.
    /// </summary>
    [Parameter]
    public bool ValidateOnInput
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the expression that determines the validation message for the TextBoxControl component.
    ///     This property allows for setting a custom validation message based on the selected date.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> ValidationMessage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the selected date in the TextBoxControl component.
    ///     This property is bound to the Value property of the SfTextBox component in the Razor view.
    ///     When the value is set, it checks for equality with the current value. If the new value is different, it updates the
    ///     current value and invokes the ValueChanged event.
    /// </summary>
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
    ///     Gets or sets the ValueChanged event. This event is triggered when the TextBox value in the control is changed by
    ///     the
    ///     user.
    ///     The event provides the new TextBox value selected by the user.
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the expression that identifies the value to be used by the TextBox control.
    ///     This expression is used to bind the date control's value to a specified property.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> ValueExpression
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the width of the TextBoxControl component.
    ///     This property allows for setting a custom width for the component.
    ///     The width can be specified in any valid CSS unit (e.g., px, %, em).
    ///     The default value is "30%".
    /// </summary>
    [Parameter]
    public string Width
    {
        get;
        set;
    } = "98%";

    /// <summary>
    ///     Gets or sets the SfTooltip object for the TextBoxControl component.
    ///     This object is used to create a tooltip for the TextBoxControl when the CreateTooltip property is set to true.
    ///     The tooltip provides additional information or guidance to the user when they hover over the TextBoxControl.
    /// </summary>
    private SfTooltip ToolTipObj
    {
        get;
        set;
    }

    /// <summary>
	///     Handles the opening event of the tooltip in the TextBox component.
	///     This method cancels the opening of the tooltip if it does not contain any text.
	/// </summary>
	/// <param name="args">The arguments for the tooltip event, which include information about the tooltip's state and text.</param>
	private void ToolTipOpen(TooltipEventArgs args)
    {
        //ToolTipObj.CssClass = "sfTooltip";
        //args.Cancel = !(ToolTipObj.Content != null || ToolTipObj.ContentTemplate != null);
        args.Cancel = !args.HasText;
    }
}