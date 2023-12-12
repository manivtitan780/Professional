#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             LabelComponents
// File Name:           AutoCompleteButton.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          03-11-2023 20:10
// Last Updated On:     11-04-2023 21:00
// *****************************************/

#endregion

namespace RazorComponents;

/// <summary>
///     Represents a button with autocomplete functionality.
/// </summary>
/// <remarks>
///     The AutoCompleteButton class is a component that provides a user interface for input with autocomplete suggestions.
///     It includes various parameters to customize the behavior and appearance of the autocomplete feature.
///     The parameters include options to control the event callbacks, tooltip creation, state persistence,
///     input length restrictions, placeholder text, and positioning of the autocomplete component.
/// </remarks>
public partial class AutoCompleteButton
{
    private Random _random = new();
    private string _value;

    private SfAutoComplete<string, KeyValues> Acb { get; set; }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback ButtonClick
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the AutoComplete component is created.
    /// </summary>
    /// <remarks>
    ///     The Created property is an event callback that gets triggered when the AutoComplete component is created. The
    ///     Created event callback can be used to perform any custom actions or initializations when the AutoComplete component
    ///     is created.
    /// </remarks>
    [Parameter]
    public EventCallback<object> Created
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether a tooltip should be created for the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     <c>true</c> if a tooltip should be created; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     When this property is set to true, a tooltip is created and displayed when the user hovers over the
    ///     AutoCompleteButton.
    ///     The tooltip provides additional information or guidance to the user.
    /// </remarks>
    [Parameter]
    public bool CreateTooltip
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the state of the AutoCompleteButton should be persisted.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the state of the AutoCompleteButton should be persisted; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     When this property is set to true, the state of the AutoCompleteButton (such as the text entered by the user) is
    ///     saved and restored across page reloads.
    ///     This can be useful in scenarios where the user might accidentally navigate away from the page and lose their
    ///     entered data.
    /// </remarks>
    [Parameter]
    public bool EnablePersistence
    {
        get;
        set;
    } = false;

    /// <summary>
    ///     Gets or sets the ID of the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     A string that represents the ID of the AutoCompleteButton.
    /// </value>
    /// <remarks>
    ///     This property is used to uniquely identify the AutoCompleteButton in the DOM. It can be used for styling or
    ///     scripting purposes.
    /// </remarks>
    [Parameter]
    public string ID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the maximum length of the input allowed in the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     The maximum number of characters allowed in the AutoCompleteButton. The default value is 50.
    /// </value>
    /// <remarks>
    ///     This property sets the "maxlength" attribute of the AutoComplete component's input field.
    ///     If the user tries to enter more characters than the specified maximum length, the additional input is ignored.
    /// </remarks>
    [Parameter]
    public int MaxLength
    {
        get;
        set;
    } = 50;

    /// <summary>
    ///     Gets or sets the minimum length of text that the user must enter before the autocomplete suggestions are displayed.
    /// </summary>
    /// <value>
    ///     The minimum length of text that the user must enter before the autocomplete suggestions are displayed. The default
    ///     value is 3.
    /// </value>
    /// <remarks>
    ///     This property is used to control when the autocomplete suggestions are displayed. If the length of the entered text
    ///     is less than the MinLength, the autocomplete suggestions will not be displayed.
    /// </remarks>
    [Parameter]
    public int MinLength
    {
        get;
        set;
    } = 3;

    /// <summary>
    ///     Gets or sets a value indicating whether the model value is set for the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the model value is set; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     This property is used to determine if a model value is set for the AutoCompleteButton.
    ///     It can be used to control the behavior of the AutoCompleteButton based on whether a model value is present.
    /// </remarks>
    [Parameter]
    public bool ModelValue
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the placeholder text for the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     A string that represents the placeholder text of the AutoCompleteButton. The default value is "Select a ...".
    /// </value>
    /// <remarks>
    ///     This property is used to display a short hint in the input field before the user enters a value.
    ///     It helps the user to understand the expected input.
    /// </remarks>
    [Parameter]
    public string PlaceholderText
    {
        get;
        set;
    } = "Select a ...";

    /// <summary>
    ///     Gets or sets the reference object for the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     An object that represents the reference of the AutoCompleteButton.
    /// </value>
    /// <remarks>
    ///     This property is used to hold a reference to the AutoCompleteButton.
    ///     It can be used to access the AutoCompleteButton directly from other parts of the application.
    /// </remarks>
    [Parameter]
    public object Ref
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the AutoComplete component should be positioned to the left.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the AutoComplete component should be positioned to the left; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     When this property is set to true, the AutoComplete component is positioned to the left.
    ///     This can be useful in scenarios where the layout or design requires the component to be aligned to the left.
    /// </remarks>
    [Parameter]
    public bool SetLeft
    {
        get;
        set;
    } = true;

    /// <summary>
    ///     Gets or sets the Type instance associated with the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     An instance of Type that represents the type of the AutoCompleteButton.
    /// </value>
    /// <remarks>
    ///     This property is used to hold a Type instance associated with the AutoCompleteButton.
    ///     It can be used to perform type-specific operations or behaviors on the AutoCompleteButton.
    /// </remarks>
    [Parameter]
    public Type TypeInstance
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the validation message expression for the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     An expression that represents the validation message of the AutoCompleteButton.
    /// </value>
    /// <remarks>
    ///     This property is used to display a validation message when the user's input does not meet certain criteria.
    ///     The validation message is determined by the expression provided to this property.
    /// </remarks>
    [Parameter]
    public Expression<Func<string>> ValidationMessage
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the current value of the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     A string that represents the current value of the AutoCompleteButton.
    /// </value>
    /// <remarks>
    ///     This property is used to hold the current value of the AutoCompleteButton.
    ///     It can be used to get or set the value directly from other parts of the application.
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
    ///     Gets or sets the event callback that is triggered when the value of the AutoCompleteButton changes.
    /// </summary>
    /// <value>
    ///     An event callback that includes the ChangeEventArgs with the new value and the associated KeyValues.
    /// </value>
    /// <remarks>
    ///     This property is used to handle the change event of the AutoCompleteButton.
    ///     It provides the new value and the associated KeyValues to the event handler.
    /// </remarks>
    [Parameter]
    public EventCallback<ChangeEventArgs<string, KeyValues>> ValueChange
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the event callback that is triggered when the value of the AutoCompleteButton changes.
    /// </summary>
    /// <remarks>
    ///     The ValueChanged property is an event callback that gets triggered when the value of the AutoCompleteButton
    ///     changes.
    ///     This event callback can be used to perform any custom actions when the value changes, such as validation or state
    ///     updates.
    /// </remarks>
    [Parameter]
    public EventCallback<string> ValueChanged
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the expression that identifies the value to be auto-completed.
    /// </summary>
    /// <value>
    ///     An expression that identifies the value to be auto-completed.
    /// </value>
    /// <remarks>
    ///     This property is used to bind the input field of the AutoCompleteButton to a specific value.
    ///     The expression should return a string that represents the current value of the input field.
    ///     When the user selects an item from the auto-complete list, the value of the input field is updated to match the
    ///     selected item.
    /// </remarks>
    [Parameter]
    public Expression<Func<string>> ValueExpression
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the width of the AutoCompleteButton.
    /// </summary>
    /// <value>
    ///     A string representing the width of the AutoCompleteButton. The default value is "200px".
    /// </value>
    /// <remarks>
    ///     The width property is used to set the width of the AutoCompleteButton. The value should be a valid CSS width
    ///     property value.
    /// </remarks>
    [Parameter]
    public string Width
    {
        get;
        set;
    } = "200px";

    /// <summary>
    ///     Handles the opening event of the tooltip.
    /// </summary>
    /// <param name="args">The arguments for the tooltip event.</param>
    /// <remarks>
    ///     This method is triggered when the tooltip is about to open. If the tooltip does not contain any text,
    ///     the opening of the tooltip is cancelled.
    /// </remarks>
    private void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}