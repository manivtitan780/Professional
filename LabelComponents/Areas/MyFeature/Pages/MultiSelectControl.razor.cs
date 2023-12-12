#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             LabelComponents
// File Name:           MultiSelectControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 18:50
// Last Updated On:     12-11-2023 21:7
// *****************************************/

#endregion

namespace LabelComponents.Areas.MyFeature.Pages;

/// <summary>
///     Represents a multi-select control for selecting items of type <typeparamref name="TItem" /> with values of type
///     <typeparamref name="TValue" />.
/// </summary>
/// <typeparam name="TItem">The type of items that can be selected in the control.</typeparam>
/// <typeparam name="TValue">The type of the value that represents the selected items in the control.</typeparam>
/// <remarks>
///     This control supports various customization options such as enabling/disabling tooltips, customizing the
///     placeholder text, and defining a custom item template.
///     It also provides support for validation and event callbacks for value changes.
/// </remarks>
public partial class MultiSelectControl<TItem, TValue>
{
    private TValue _value;

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
    } = true;

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
    ///     Gets or sets a value indicating whether the drop-down icon is displayed in the multi-select control.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the drop-down icon is displayed; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     The drop-down icon is used to indicate that the control has a drop-down list of items that can be selected.
    ///     If this property is set to <c>false</c>, the drop-down icon will not be displayed, but the drop-down list will
    ///     still be accessible.
    /// </remarks>
    [Parameter]
    public bool DropDownIcon
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the placeholder text for the filter bar in the multi-select control.
    /// </summary>
    /// <value>
    ///     The placeholder text for the filter bar.
    /// </value>
    /// <remarks>
    ///     This property is used to display a short hint in the filter bar's input field before the user enters a value.
    ///     The placeholder text is displayed when the filter bar is empty and loses focus.
    /// </remarks>
    [Parameter]
    public string FilterBarPlaceholder
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
    ///     Gets or sets the template for displaying each item in the multi-select control.
    /// </summary>
    /// <value>
    ///     A <see cref="RenderFragment" /> that specifies the HTML markup and bindings for each item.
    /// </value>
    /// <remarks>
    ///     This property is used to customize the way each item is displayed in the multi-select control.
    ///     The template should contain HTML markup and bindings that are appropriate for the type of items in the control.
    ///     If this property is not set, a default template will be used.
    /// </remarks>
    [Parameter]
    public RenderFragment ItemTemplate
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the key field for the items in the multi-select control.
    /// </summary>
    /// <value>
    ///     The key field for the items.
    /// </value>
    /// <remarks>
    ///     This property is used to specify the property of the items that should be used as the key.
    ///     The key is used to uniquely identify each item in the control.
    ///     If this property is not set, a default key field will be used.
    /// </remarks>
    [Parameter]
    public string KeyField
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the visual mode of the multi-select control.
    /// </summary>
    /// <value>
    ///     The visual mode of the control. The default value is <see cref="VisualMode.Box" />.
    /// </value>
    /// <remarks>
    ///     The visual mode determines how the selected items are displayed in the control.
    ///     For example, in 'Box' mode, each selected item is displayed in its own box.
    /// </remarks>
    [Parameter]
    public VisualMode Mode
    {
        get;
        set;
    } = VisualMode.Box;

    /// <summary>
    ///     Gets or sets a value indicating whether the multi-select control should be displayed in multiline mode.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the control should be displayed in multiline mode; otherwise, <c>false</c>. Default value is
    ///     <c>false</c>.
    /// </value>
    /// <remarks>
    ///     When this property is set to <c>true</c>, the control will be displayed in a larger area, allowing for better
    ///     visibility of selected items.
    ///     When set to <c>false</c>, the control will be displayed in a smaller area, suitable for single line display.
    /// </remarks>
    [Parameter]
    public bool Multiline
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
    ///     Gets or sets the Type of the items in the multi-select control.
    /// </summary>
    /// <value>
    ///     The Type of the items.
    /// </value>
    /// <remarks>
    ///     This property is used to specify the Type of the items that are being selected in the control.
    ///     The Type information can be used for various purposes such as validation, formatting, or custom rendering.
    /// </remarks>
    [Parameter]
    public Type TypeItem
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the type of the value that represents the selected items in the multi-select control.
    /// </summary>
    /// <value>
    ///     The type of the value that represents the selected items.
    /// </value>
    /// <remarks>
    ///     This property is used to specify the type of the value that represents the selected items in the control.
    ///     The type is used to ensure that the selected items are of the correct type.
    ///     If this property is not set, a default type will be used.
    /// </remarks>
    [Parameter]
    public Type TypeValue
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets a value indicating whether to use a custom template for the MultiSelectControl.
    /// </summary>
    /// <value>
    ///     <c>true</c> if a custom template should be used; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    ///     When this property is set to <c>true</c>, the MultiSelectControl will use a custom template for rendering its
    ///     items.
    ///     If set to <c>false</c>, the MultiSelectControl will use its default rendering.
    /// </remarks>
    [Parameter]
    public bool UseCustomTemplate
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the expression that determines the validation message for the DropDownControl component.
    ///     This property allows for setting a custom validation message based on the selected date.
    /// </summary>
    [Parameter]
    public Expression<Func<string>> ValidationMessage
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
    ///     Gets or sets the template for the value representation in the multi-select control.
    /// </summary>
    /// <value>
    ///     The template for the value representation.
    /// </value>
    /// <remarks>
    ///     This property is used to customize the way selected values are displayed in the control.
    ///     The template is a RenderFragment, which means it can contain any valid Razor markup.
    ///     If this property is not set, the control will use a default template for value representation.
    /// </remarks>
    [Parameter]
    public RenderFragment ValueTemplate
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
    ///     Handles the opening event of the tooltip in the DateControl component.
    ///     This method cancels the opening of the tooltip if it does not contain any text.
    /// </summary>
    /// <param name="args">The arguments for the tooltip event, which include information about the tooltip's state and text.</param>
    private static void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}