#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           PreferencesDialog.razor.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:53
// Last Updated On:     12-14-2023 16:3
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Pages.Admin.Controls;

/// <summary>
///     Represents a dialog for managing user preferences in the Admin Controls of the ProfSvc_AppTrack application.
/// </summary>
/// <remarks>
///     The PreferencesDialog class is a partial class that contains methods for initializing the dialog and displaying it.
///     It includes a method for initializing the dialog with user preferences from the memory cache, and a method for
///     displaying the dialog asynchronously.
/// </remarks>
/// <summary>
///     Represents a dialog for managing user preferences in the Admin Controls of the ProfSvc_AppTrack application.
/// </summary>
/// <remarks>
///     The PreferencesDialog class is a partial class that contains methods for initializing the dialog and displaying it.
///     It includes a method for initializing the dialog with user preferences from the memory cache, and a method for
///     displaying the dialog asynchronously.
/// </remarks>
public partial class PreferencesDialog
{
    /// <summary>
    ///     Represents a list of page size options for a dropdown menu in the Preferences Dialog.
    ///     Each option is represented by a ByteValues object, where the byte value indicates the number of rows to be
    ///     displayed on a page,
    ///     and the corresponding string provides a user-friendly description of the option.
    /// </summary>
    private readonly List<ByteValues> _pageSizeDropItems = [new(10, "10 rows"), new(25, "25 rows"), new(50, "50 rows"), new(75, "75 rows"), new(100, "100 rows")];

    /// <summary>
    ///     Gets or sets the SfDialog component used in the PreferencesDialog.
    /// </summary>
    /// <value>
    ///     The SfDialog component that provides the user interface for managing roles.
    /// </value>
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
    ///     Gets or sets the form used for editing preferences in the PreferencesDialog.
    /// </summary>
    /// <value>
    ///     The form used for editing preferences.
    /// </value>
    private EditForm EditPreferenceForm
    {
        get;
        set;
    }

    /// <summary>
    ///     Gets or sets the preferences model for the PreferencesDialog.
    /// </summary>
    /// <value>
    ///     The Preferences model represents the role that is currently being edited in the dialog.
    /// </value>
    private Preferences Model
    {
        set;
        get;
    } = new();

    /// <summary>
    ///     Gets or sets the cloned model of the preferences for the dialog.
    /// </summary>
    /// <value>
    ///     The cloned model of the preferences.
    /// </value>
    /// <remarks>
    ///     This model is a copy of the original preferences data model. It is used to preserve the state of the
    ///     original model when making changes in the dialog. If the changes are cancelled, the original model is restored from
    ///     this clone.
    /// </remarks>
    private Preferences ModelClone
    {
        set;
        get;
    } = new();

    /// <summary>
    ///     Gets or sets the SfSpinner component used in the PreferencesDialog.
    /// </summary>
    /// <value>
    ///     The SfSpinner component that provides a visual indication of processing when the user performs actions such as
    ///     saving or canceling.
    /// </value>
    private SfSpinner Spinner
    {
        get;
        set;
    }

    /// <summary>
    ///     Cancels the changes made in the dialog and hides the dialog.
    /// </summary>
    /// <param name="args">The mouse event arguments.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    ///     This method is triggered when the cancel button in the dialog is clicked. It restores the original model from the
    ///     cloned model and hides the dialog.
    /// </remarks>
    private Task CancelPreferences(MouseEventArgs args)
    {
        Model = ModelClone.Copy();
        return Dialog.HideAsync();
    }

    [Inject]
    private RedisService Redis
    {
        get;
        set;
    }

    /// <summary>
    ///     Opens the dialog for editing preferences.
    /// </summary>
    /// <param name="args">The arguments for the BeforeOpen event.</param>
    /// <remarks>
    ///     This method is invoked before the dialog is opened. It ensures that the form is validated before the dialog is
    ///     displayed.
    /// </remarks>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task OpenDialog(BeforeOpenEventArgs args)
    {
        await Task.Yield();
        Preferences _preferences = await Redis.GetAsync<Preferences>("Preferences");
        if (_preferences != null)
        {
            Model = _preferences;
        }
        _ = EditPreferenceForm.EditContext?.Validate();
        ModelClone = Model.Copy();
    }

    /// <summary>
    ///     Asynchronously saves the changes made to the preferences data.
    /// </summary>
    /// <param name="editContext">The EditContext instance associated with the form being submitted.</param>
    /// <remarks>
    ///     This method is triggered when the Save button in the dialog footer is clicked.
    ///     It first checks if the buttons in the dialog footer are enabled. If they are, it shows a spinner, disables the
    ///     buttons,
    ///     and then calls the General.SavePreferencesAsync method to save the changes made to the Model property.
    ///     After the changes are saved, it enables the buttons, hides the spinner, and closes the dialog.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    private async Task SavePreferences(EditContext editContext)
    {
        if (!DialogFooter.ButtonsDisabled())
        {
            await Spinner.ShowAsync();
            DialogFooter.DisableButtons();
            await General.PostRest<int>("Admin/SavePreferences", null, Model);
            await Redis.RefreshAsync("Preferences", Model);
            await Task.Yield();
            DialogFooter.EnableButtons();
            await Spinner.HideAsync();
            await Dialog.HideAsync();
        }
    }

    /// <summary>
    ///     Asynchronously displays the PreferencesDialog.
    /// </summary>
    /// <remarks>
    ///     This method is used to display the PreferencesDialog by invoking the ShowAsync method of the SfDialog component.
    ///     It is typically called when the user interacts with the associated UI element in the Header component.
    /// </remarks>
    /// <returns>A Task that represents the asynchronous operation.</returns>
    public Task ShowDialog() => Dialog.ShowAsync();

    /// <summary>
    ///     Handles the opening of a tooltip.
    /// </summary>
    /// <param name="args">The arguments associated with the tooltip event.</param>
    /// <remarks>
    ///     This method cancels the opening of the tooltip if it does not contain any text.
    /// </remarks>
    private static void ToolTipOpen(TooltipEventArgs args) => args.Cancel = !args.HasText;
}