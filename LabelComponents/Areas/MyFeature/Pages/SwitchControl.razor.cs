#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             LabelComponents
// File Name:           SwitchControl.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 18:50
// Last Updated On:     12-11-2023 21:8
// *****************************************/

#endregion

namespace LabelComponents.Areas.MyFeature.Pages;

/// <summary>
///     Represents a switch control component in the MyFeature area of the LabelComponents project.
///     This control is a toggle switch that can be turned on or off. It has customizable labels for both states.
/// </summary>
/// <remarks>
///     The SwitchControl component is used in various parts of the application, including the AdminListDialog and
///     AdvancedCandidateSearch.
///     It includes parameters for customization such as BindValue, BindValueChanged, ID, OffLabel, OnLabel, and
///     Placeholder.
/// </remarks>
public partial class SwitchControl
{
    private bool _value;

    /// <summary>
    ///     Gets or sets the current value of the SwitchControl.
    /// </summary>
    /// <value>
    ///     A boolean value representing the state of the SwitchControl. True indicates the switch is on, and false indicates
    ///     it is off.
    /// </value>
    /// <remarks>
    ///     When the value is set, it checks if the new value is different from the old one. If it is, it updates the value and
    ///     invokes the BindValueChanged event.
    /// </remarks>
    [Parameter]
    public bool BindValue
    {
        get => _value;
        set
        {
            if (_value == value)
            {
                return;
            }

            _value = value;
            BindValueChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    ///     Gets or sets the event callback that is invoked when the value of the SwitchControl changes.
    /// </summary>
    /// <value>
    ///     An EventCallback&lt;bool&gt; delegate that represents the method(s) that will handle the event.
    /// </value>
    /// <remarks>
    ///     This event is triggered when the BindValue property is set to a new value. The new value is passed as an argument
    ///     to the callback.
    /// </remarks>
    [Parameter]
    public EventCallback<bool> BindValueChanged
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the ID of the SwitchControl. This ID is used as the 'for' attribute value for the label and the
    ///     'Target' attribute value for the tooltip in the HTML markup.
    /// </summary>
    [Parameter]
    public string ID
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the label displayed when the SwitchControl is in the 'off' state.
    /// </summary>
    /// <value>
    ///     A string representing the label text. This text is displayed next to the switch when it is in the 'off' state.
    /// </value>
    /// <remarks>
    ///     This property allows for customization of the switch's 'off' label. If not set, the default label is used.
    /// </remarks>
    [Parameter]
    public string OffLabel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the label displayed when the SwitchControl is in the 'on' state.
    /// </summary>
    /// <value>
    ///     A string representing the label text. This text is displayed next to the switch when it is in the 'on' state.
    /// </value>
    /// <remarks>
    ///     This property allows for customization of the switch's 'on' label. If not set, the default label is used.
    /// </remarks>
    [Parameter]
    public string OnLabel
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the placeholder text for the SwitchControl component.
    ///     This property allows for setting a display text in the date input field when it is empty.
    /// </summary>
    [Parameter]
    public string Placeholder
    {
        get;
        set;
    }
}