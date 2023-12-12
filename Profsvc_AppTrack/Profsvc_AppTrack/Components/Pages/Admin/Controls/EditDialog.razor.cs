#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           EditDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          12-09-2022 15:57
// Last Updated On:     09-02-2023 16:08
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog component for editing administrative entities in the application.
/// </summary>
/// <remarks>
///     This class inherits from the ComponentBase class and contains parameters for various attributes such as ID,
///     Placeholder, and Readonly status.
///     It also contains a parameter for an AdminContext, which is an instance of the AdminList class.
///     The SetParametersAsync method is overridden to create a new EditContext based on the AdminContext.
/// </remarks>
public partial class EditDialog : ComponentBase
{
	private EditContext _context;

	/// <summary>
	///     Gets or sets the AdminContext which is an instance of the AdminList class.
	/// </summary>
	/// <value>
	///     The AdminContext is used to bind data to the EditDialog component,
	///     allowing the user to edit the properties of an administrative entity.
	/// </value>
	[Parameter]
	public AdminList AdminContext
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the collection of additional HTML attributes to apply to the SfTextBox component.
	/// </summary>
	/// <value>
	///     A Dictionary{string, object} that contains the HTML attributes and their values.
	///     The default attributes are "maxlength" set to "100" and "minlength" set to "2".
	/// </value>
	[Parameter]
	public Dictionary<string, object> Attributes
	{
		get;
		set;
	} = new()
		{
			{
				"maxlength", "100"
			},
			{
				"minlength", "2"
			}
		};

	/// <summary>
	///     Gets or sets the ID attribute of the SfTextBox component in the EditDialog.
	/// </summary>
	/// <value>
	///     A string that represents the unique identifier for the SfTextBox component.
	///     This ID is used as the target for the SfTooltip component.
	/// </value>
	[Parameter]
	public string ID
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets the placeholder text for the SfTextBox component in the EditDialog.
	/// </summary>
	/// <value>
	///     A string that represents the placeholder text displayed in the SfTextBox component when it is empty and not
	///     focused.
	/// </value>
	[Parameter]
	public string Placeholder
	{
		get;
		set;
	}

	/// <summary>
	///     Gets or sets a value indicating whether the SfTextBox component in the EditDialog is read-only.
	/// </summary>
	/// <value>
	///     A boolean that is true if the SfTextBox component is read-only; otherwise, false.
	///     When this property is set to true, the user cannot modify the text in the SfTextBox component.
	/// </value>
	[Parameter]
	public bool Readonly
	{
		get;
		set;
	}

	/// <summary>
	///     Asynchronously sets the parameters for the EditDialog component.
	/// </summary>
	/// <param name="parameters">The parameters to be set for the EditDialog component.</param>
	/// <returns>A Task that represents the asynchronous operation.</returns>
	/// <remarks>
	///     This method overrides the base SetParametersAsync method from the ComponentBase class.
	///     If the AdminContext is not null, it creates a new EditContext based on the AdminContext.
	/// </remarks>
	public override Task SetParametersAsync(ParameterView parameters)
	{
		if (_context != null && AdminContext != null)
		{
			_context = new(AdminContext);
		}

		return base.SetParametersAsync(parameters);
	}

	/// <summary>
	///     Handles the opening event of the tooltip.
	/// </summary>
	/// <param name="args">The arguments for the tooltip event.</param>
	/// <remarks>
	///     This method is triggered when the tooltip is about to open. If the tooltip does not contain any text,
	///     the opening of the tooltip is cancelled.
	/// </remarks>
	private void ToolTipOpen(TooltipEventArgs args)
	{
		_context?.Validate();
		args.Cancel = !args.HasText;
	}
}