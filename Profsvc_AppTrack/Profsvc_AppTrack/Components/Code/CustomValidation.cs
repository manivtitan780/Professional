#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           CustomValidation.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          11-16-2022 19:43
// Last Updated On:     11-16-2022 19:43
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Code;

public class CustomValidation : ComponentBase, IDisposable
{
    private EditContext _originalEditContext;
    private IDisposable _subscriptions;

    [Parameter]
    public bool DoEditValidation { get; set; }

    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; }

    void IDisposable.Dispose()
    {
        _subscriptions?.Dispose();
        _subscriptions = null;

        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(DataAnnotationsValidator)} requires a cascading " +
                                                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(DataAnnotationsValidator)} " +
                                                "inside an EditForm.");
        }

        _subscriptions = CurrentEditContext.EnableCustomValidation(DoEditValidation, true);
        _originalEditContext = CurrentEditContext;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (CurrentEditContext != _originalEditContext)
        {
            // While we could support this, there's no known use case presently. Since InputBase doesn't support it,
            // it's more understandable to have the same restriction.
            throw new InvalidOperationException($"{GetType()} does not support changing the " +
                                                $"{nameof(EditContext)} dynamically.");
        }
    }
}