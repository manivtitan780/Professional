#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           EditContextCustomValidation.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          11-16-2022 19:31
// Last Updated On:     11-16-2022 19:34
// *****************************************/

#endregion

#region Using

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

#endregion

namespace Profsvc_AppTrack.Components.Code;

public static class EditContextCustomValidationExtensions
{
    public static IDisposable EnableCustomValidation(this EditContext editContext, bool doFieldValidation, bool clearMessageStore) =>
        new DataAnnotationsEventSubscriptions(editContext, doFieldValidation, clearMessageStore);

    private static void ClearCache(Type[] _) => OnClearCache?.Invoke();

    private sealed class DataAnnotationsEventSubscriptions : IDisposable
    {
        public DataAnnotationsEventSubscriptions(EditContext editContext, bool doFieldValidation, bool clearMessageStore)
        {
            _doFieldValidation = doFieldValidation;
            _clearMessageStore = clearMessageStore;
            _editContext = editContext ?? throw new ArgumentNullException(nameof(editContext));
            _messages = new(_editContext);

            if (doFieldValidation)
            {
                _editContext.OnFieldChanged += OnFieldChanged;
            }

            _editContext.OnValidationRequested += OnValidationRequested;

            if (MetadataUpdater.IsSupported)
            {
                OnClearCache += ClearCache;
            }
        }

        private static readonly ConcurrentDictionary<(Type ModelType, string FieldName), PropertyInfo> _propertyInfoCache = new();

        private readonly EditContext _editContext;
        private readonly ValidationMessageStore _messages;
        private readonly bool _clearMessageStore;
        private readonly bool _doFieldValidation;

        public void Dispose()
        {
            if (_clearMessageStore)
            {
                _messages.Clear();
            }

            if (_doFieldValidation)
            {
                _editContext.OnFieldChanged -= OnFieldChanged;
            }

            _editContext.OnValidationRequested -= OnValidationRequested;
            _editContext.NotifyValidationStateChanged();

            if (MetadataUpdater.IsSupported)
            {
                OnClearCache -= ClearCache;
            }
        }

        internal void ClearCache() => _propertyInfoCache.Clear();

        private void OnFieldChanged(object sender, FieldChangedEventArgs eventArgs)
        {
            FieldIdentifier _fieldIdentifier = eventArgs.FieldIdentifier;
            if (!TryGetValidatableProperty(_fieldIdentifier, out PropertyInfo _propertyInfo))
            {
                return;
            }

            object _propertyValue = _propertyInfo.GetValue(_fieldIdentifier.Model);
            ValidationContext _validationContext = new ValidationContext(_fieldIdentifier.Model)
                                                   {
                                                       MemberName = _propertyInfo.Name
                                                   };
            List<ValidationResult> results = new List<ValidationResult>();

            Validator.TryValidateProperty(_propertyValue, _validationContext, results);
            _messages.Clear(_fieldIdentifier);
            foreach (ValidationResult result in CollectionsMarshal.AsSpan(results))
            {
                _messages.Add(_fieldIdentifier, result.ErrorMessage!);
            }

            // We have to notify even if there were no messages before and are still no messages now,
            // because the "state" that changed might be the completion of some async validation task
            _editContext.NotifyValidationStateChanged();
        }

        private void OnValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
            ValidationContext _validationContext = new ValidationContext(_editContext.Model);
            List<ValidationResult> _validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(_editContext.Model, _validationContext, _validationResults, true);

            // Transfer results to the ValidationMessageStore
            _messages.Clear();
            foreach (ValidationResult _validationResult in _validationResults)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (_validationResult == null)
                {
                    continue;
                }

                bool _hasMemberNames = false;
                foreach (string _memberName in _validationResult.MemberNames)
                {
                    _hasMemberNames = true;
                    _messages.Add(_editContext.Field(_memberName), _validationResult.ErrorMessage!);
                }

                if (!_hasMemberNames)
                {
                    _messages.Add(new FieldIdentifier(_editContext.Model, string.Empty), _validationResult.ErrorMessage!);
                }
            }

            _editContext.NotifyValidationStateChanged();
        }

        private static bool TryGetValidatableProperty(in FieldIdentifier fieldIdentifier, [NotNullWhen(true)] out PropertyInfo propertyInfo)
        {
            (Type ModelType, string FieldName) _cacheKey = (ModelType: fieldIdentifier.Model.GetType(), fieldIdentifier.FieldName);
            if (!_propertyInfoCache.TryGetValue(_cacheKey, out propertyInfo))
            {
                // DataAnnotations only validates public properties, so that's all we'll look for
                // If we can't find it, cache 'null' so we don't have to try again next time
                propertyInfo = _cacheKey.ModelType.GetProperty(_cacheKey.FieldName);

                // No need to lock, because it doesn't matter if we write the same value twice
                _propertyInfoCache[_cacheKey] = propertyInfo;
            }

            return propertyInfo != null;
        }
    }

    private static event System.Action OnClearCache;
}