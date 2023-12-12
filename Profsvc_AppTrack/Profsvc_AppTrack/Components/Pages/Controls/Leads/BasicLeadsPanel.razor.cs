#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           BasicLeadsPanel.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          01-26-2023 19:18
// Last Updated On:     09-30-2023 14:57
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Controls.Leads;

/// <summary>
///     Represents a panel in the user interface for displaying and interacting with lead details.
///     This panel is part of the Leads controls and is used to manage basic information about a lead.
/// </summary>
/// <remarks>
///     The panel uses a model of type <see cref="LeadDetails" /> to bind and manipulate the lead data.
/// </remarks>
public partial class BasicLeadsPanel
{
    /// <summary>
    ///     Gets or sets the model representing the details of a lead.
    /// </summary>
    /// <value>
    ///     The model is of type <see cref="LeadDetails" />, which contains properties for various lead details such as company
    ///     name, contact name, email address, phone number, etc.
    ///     These details are displayed and can be interacted with in the BasicLeadsPanel.
    /// </value>
    [Parameter]
    public LeadDetails Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Generates a formatted address string from the lead details.
    /// </summary>
    /// <returns>
    ///     A <see cref="MarkupString" /> that represents the formatted address of the lead.
    ///     The address includes the street, city, state, and zip code, separated by commas and line breaks.
    ///     If any part of the address is not provided, it is omitted from the returned string.
    /// </returns>
    /// <remarks>
    ///     The method uses the <see cref="ProfSvc_Classes.LeadDetails" /> model to access the address details.
    /// </remarks>
    private MarkupString ShowAddress()
    {
        string _address = "";
        if (!Model.Street.NullOrWhiteSpace())
        {
            _address += ", " + Model.Street;
        }

        if (!Model.City.NullOrWhiteSpace())
        {
            _address += ", " + Model.City;
        }

        if (!Model.StateName.NullOrWhiteSpace())
        {
            _address += "<br/>" + Model.StateName;
        }

        if (!Model.ZipCode.NullOrWhiteSpace())
        {
            _address += ", " + Model.ZipCode;
        }

        if (_address.StartsWith(", "))
        {
            _address = _address[2..];
        }

        if (_address.StartsWith("<br/>"))
        {
            _address = _address[5..];
        }

        return _address.ToMarkupString();
    }
}