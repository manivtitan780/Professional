#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           VariableCommissionDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-23-2023 16:8
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing variable commissions in the Admin Controls of the ProfSvc_AppTrack application.
/// </summary>
/// <remarks>
///     The VariableCommissionDialog class is a partial class that provides a user interface for viewing and modifying
///     variable commissions.
///     It includes a SfDialog instance that controls the visibility and behavior of the dialog.
///     The dialog is initialized with the current variable commission data when the component is initialized.
/// </remarks>
public partial class VariableCommissionDialog
{
    /// <summary>
    ///     Gets or sets the SfDialog instance used in the variable commissions' dialog.
    /// </summary>
    /// <value>
    ///     The SfDialog instance.
    /// </value>
    /// <remarks>
    ///     This instance is used to control the visibility and behavior of the dialog.
    ///     It is bound to the dialog component in the Razor markup and can be used to programmatically show or hide the
    ///     dialog.
    /// </remarks>
    private SfDialog Dialog
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the DialogFooter instance associated with the VariableCommissionDialog.
    /// </summary>
    /// <value>
    ///     The DialogFooter instance.
    /// </value>
    /// <remarks>
    ///     The DialogFooter instance is used to manage the footer section of the dialog,
    ///     which includes the Cancel and Save buttons.
    /// </remarks>
    private DialogFooter DialogFooter
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the form used for editing variable commissions.
    /// </summary>
    /// <value>
    ///     The form used for editing variable commissions.
    /// </value>
    private EditForm EditVariableCommission
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the model representing the variable commissions for the dialog.
    /// </summary>
    /// <value>
    ///     The model representing the variable commissions.
    /// </value>
    /// <remarks>
    ///     This model is used to bind the variable commissions data in the dialog for editing purposes.
    /// </remarks>
    private VariableCommission Model
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the cloned model of the variable commissions for the dialog.
    /// </summary>
    /// <value>
    ///     The cloned model of the variable commissions.
    /// </value>
    /// <remarks>
    ///     This model is a copy of the original variable commissions data model. It is used to preserve the state of the
    ///     original model when making changes in the dialog. If the changes are cancelled, the original model is restored from
    ///     this clone.
    /// </remarks>
    private VariableCommission ModelClone
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the spinner control used in the variable commissions' dialog.
    /// </summary>
    /// <value>
    ///     The spinner control.
    /// </value>
    /// <remarks>
    ///     This control is used to display a loading indicator while the dialog is performing operations such as saving or
    ///     canceling.
    /// </remarks>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Cancels the changes made in the dialog and hides the dialog.
    /// </summary>
    /// <param name="arg">The mouse event arguments.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is triggered when the cancel button in the dialog is clicked. It restores the original model from the
    ///     cloned model and hides the dialog.
    /// </remarks>
    private Task CancelVariableCommission(MouseEventArgs arg)
    {
        Model = ModelClone.Copy();
        return Dialog.HideAsync();
    }

    /// <summary>
    ///     Asynchronously initializes the dialog with the current variable commission data.
    /// </summary>
    /// <remarks>
    ///     This method is called when the component is initialized. It retrieves the current variable commission data
    ///     from the server using the General.GetVariableCommission method and assigns it to the Model property.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    protected override async Task OnInitializedAsync() => Model = await General.GetVariableCommission();

    /// <summary>
    ///     Asynchronously opens the dialog for editing variable commissions.
    /// </summary>
    /// <param name="arg">The arguments for the event that triggers the opening of the dialog.</param>
    /// <remarks>
    ///     This method is triggered when the dialog is about to open. It validates the current edit context of the form and
    ///     creates a copy of the current model for editing.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private void OpenDialog(BeforeOpenEventArgs arg)
    {
        _ = EditVariableCommission.EditContext?.Validate();
        ModelClone = Model.Copy();
    }

    /// <summary>
    ///     Asynchronously saves the changes made to the variable commission data.
    /// </summary>
    /// <param name="editContext">The EditContext instance associated with the form being submitted.</param>
    /// <remarks>
    ///     This method is triggered when the Save button in the dialog footer is clicked.
    ///     It first checks if the buttons in the dialog footer are enabled. If they are, it shows a spinner, disables the
    ///     buttons,
    ///     and then calls the General.SaveVariableCommissionAsync method to save the changes made to the Model property.
    ///     After the changes are saved, it enables the buttons, hides the spinner, and closes the dialog.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    private async Task SaveVariableCommission(EditContext editContext)
    {
        if (!DialogFooter.ButtonsDisabled())
        {
            await Spinner.ShowAsync();
            DialogFooter.DisableButtons();
            await General.PostRest<int>("Admin/SaveVariableCommission", null, Model);

            DialogFooter.EnableButtons();
            await Spinner.HideAsync();
            await Dialog.HideAsync();
        }
    }

    /// <summary>
    ///     Asynchronously displays the VariableCommissionDialog.
    /// </summary>
    /// <remarks>
    ///     This method is used to display the VariableCommissionDialog by invoking the ShowAsync method of the SfDialog
    ///     component.
    ///     It is typically called when the user interacts with the associated UI element in the Header component.
    /// </remarks>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    public Task ShowDialog() => Dialog.ShowAsync();
}