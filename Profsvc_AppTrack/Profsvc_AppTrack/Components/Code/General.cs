#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           General.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-22-2023 16:27
// Last Updated On:     12-22-2023 19:44
// *****************************************/

#endregion

#region Using

using System.Globalization;
using System.Runtime.CompilerServices;

using Profsvc_AppTrack.Components.Pages;
using Profsvc_AppTrack.Components.Pages.Admin;

using ILogger = Microsoft.Extensions.Logging.ILogger;

// ReSharper disable UnusedMember.Global

#endregion

namespace Profsvc_AppTrack.Components.Code;

/// <summary>
///     The 'General' class is an internal utility class in the 'ProfSvc_AppTrack.Code' namespace.
///     It provides a set of static methods that perform various operations such as calling cancel and save methods,
///     deserializing objects, executing save operations, formatting currency and percentages,
///     getting autocomplete data, reading data, hashing passwords, toggling posts, and saving lists and workflows.
/// </summary>
internal class General
{
    internal General(RedisService redisService) => Redis = redisService;

    private static Dictionary<string, object> _restResponse;

    private static RedisService Redis
    {
        get;
        set;
    }

    /// <summary>
    ///     Asynchronously executes the provided cancel method, hides the spinner and dialog, and enables the dialog buttons.
    ///     This method is designed to be used as a common cancellation routine for various dialogs in the application.
    /// </summary>
    /// <param name="args">The mouse event arguments associated with the cancel action.</param>
    /// <param name="spinner">The spinner control to be hidden when the cancel method completes.</param>
    /// <param name="footer">The dialog footer containing the buttons to be enabled when the cancel method completes.</param>
    /// <param name="dialog">The dialog to be hidden when the cancel method completes.</param>
    /// <param name="cancelMethod">The cancel method to be invoked asynchronously.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal static async Task CallCancelMethod(MouseEventArgs args, SfSpinner spinner, DialogFooter footer, SfDialog dialog, EventCallback<MouseEventArgs> cancelMethod)
    {
        await cancelMethod.InvokeAsync(args);
        footer.EnableButtons();
        await spinner.HideAsync();
        await dialog.HideAsync();
    }

    /// <summary>
    ///     Asynchronously executes the provided save method, shows the spinner, disables the dialog buttons, and then hides
    ///     the spinner and dialog, and enables the dialog buttons.
    ///     This method is designed to be used as a common save routine for various dialogs in the application.
    /// </summary>
    /// <param name="editContext">The edit context associated with the save action.</param>
    /// <param name="spinner">
    ///     The spinner control to be shown when the save method starts and hidden when the save method
    ///     completes.
    /// </param>
    /// <param name="footer">
    ///     The dialog footer containing the buttons to be disabled when the save method starts and enabled
    ///     when the save method completes.
    /// </param>
    /// <param name="dialog">The dialog to be hidden when the save method completes.</param>
    /// <param name="saveMethod">The save method to be invoked asynchronously.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal static async Task CallSaveMethod(EditContext editContext, SfSpinner spinner, DialogFooter footer, SfDialog dialog, EventCallback<EditContext> saveMethod)
    {
        if (!footer.ButtonsDisabled())
        {
            await spinner.ShowAsync();
            footer.DisableButtons();
            await saveMethod.InvokeAsync(editContext);
            footer.EnableButtons();
            await spinner.HideAsync();
            await dialog.HideAsync();
        }
    }

    /// <summary>
    ///     Deserializes a JSON string to an object of a specified type.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize to.</typeparam>
    /// <param name="array">The JSON string representing the object to be deserialized.</param>
    /// <returns>The deserialized object of type T.</returns>
    internal static T DeserializeObject<T>(object array) => JsonConvert.DeserializeObject<T>(array?.ToString() ?? string.Empty);

    /// <summary>
    ///     Executes the provided task within a semaphore lock. If the semaphore is currently locked, the method will return
    ///     immediately.
    ///     If an exception occurs during the execution of the task, it will be logged using the provided logger.
    /// </summary>
    /// <param name="semaphore">The semaphore used to control access to the task.</param>
    /// <param name="task">The task to be executed.</param>
    /// <param name="logger">The logger used to log any exceptions that occur during the execution of the task.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal static async Task ExecuteMethod(SemaphoreSlim semaphore, Func<Task> task, ILogger logger)
    {
        if (await semaphore.WaitAsync(TimeSpan.Zero))
        {
            try
            {
                await task();
            }
            catch (Exception _ex)
            {
                logger.LogError(_ex, $"Exception occurred at: [{DateTime.Today.ToString(CultureInfo.InvariantCulture)}]{Environment.NewLine}{new('-', 40)}{Environment.NewLine}");
            }
            finally
            {
                semaphore.Release();
            }
        }
    }

    /// <summary>
    ///     Processes the passed filter value and returns a formatted filter string.
    /// </summary>
    /// <param name="filterValue">The current filter value.</param>
    /// <param name="passedValue">The new value to be set as the filter.</param>
    /// <returns>
    ///     A string that represents the processed filter value. If the passed value is null, empty, or whitespace, or equals
    ///     to "null",
    ///     an empty string is returned. If the passed value starts or ends with a quotation mark, the quotation mark is
    ///     removed.
    /// </returns>
    /// <remarks>
    ///     This method is used in various parts of the application to process and format filter values before they are used to
    ///     filter data.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string FilterSet(string filterValue, string passedValue)
    {
        filterValue = !passedValue.NullOrWhiteSpace() && passedValue != "null" ? passedValue : "";

        if (filterValue.Length <= 0)
        {
            return "";
        }

        if (filterValue.StartsWith("\""))
        {
            filterValue = filterValue[1..];
        }

        if (filterValue.EndsWith("\""))
        {
            filterValue = filterValue[..^1];
        }

        return filterValue;
    }

    /// <summary>
    ///     Formats two decimal values into a string representation of US currency.
    /// </summary>
    /// <param name="first">The first decimal value to be formatted.</param>
    /// <param name="second">The second decimal value to be formatted.</param>
    /// <returns>A string that represents the two decimal values formatted as US currency, separated by a hyphen.</returns>
    internal static string FormatUSCurrency(decimal first, decimal second) => string.Format(new CultureInfo("en-US"), "{0:C} - {1:C}", first, second);

    /// <summary>
    ///     Formats a decimal value into a percentage string according to the US culture format.
    /// </summary>
    /// <param name="first">
    ///     The decimal value to be formatted.
    /// </param>
    /// <returns>
    ///     A string representing the input decimal value as a percentage, formatted according to the US culture.
    /// </returns>
    internal static string FormatUSPercent(decimal first) => string.Format(new CultureInfo("en-US"), "{0:P0}", first);

    /// <summary>
    ///     Asynchronously retrieves autocomplete data for a specific method and parameter.
    /// </summary>
    /// <param name="methodName">The name of the method for which autocomplete data is required.</param>
    /// <param name="parameterName">The name of the parameter for which autocomplete data is required.</param>
    /// <param name="dm">The DataManagerRequest object containing additional request parameters.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of autocomplete options
    ///     in the form of KeyValues objects if any matches are found, or an empty list if no matches are found.
    ///     If the DataManagerRequest object requires counts, the task result is a DataResult object containing the
    ///     autocomplete options and their count.
    /// </returns>
    /// <remarks>
    ///     This method makes an asynchronous request to the 'Admin/GetSearchDropDown' endpoint of the API,
    ///     passing the method name, parameter name, and filter value as query parameters.
    ///     The response is a list of strings which are then converted into KeyValues objects and returned.
    ///     If an exception occurs during the operation, an empty list or a DataResult object with a count of 0 is returned.
    /// </remarks>
    internal static async Task<object> GetAutocompleteAsync(string methodName, string parameterName, DataManagerRequest dm)
    {
        List<KeyValues> _dataSource = [];

        if (dm.Where is not {Count: > 0} || dm.Where[0].value.NullOrWhiteSpace())
        {
            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = 0
                                       } : _dataSource;
        }

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"methodName", methodName},
                                                         {"paramName", parameterName},
                                                         {"filter", dm.Where[0].value.ToString()}
                                                     };
            List<string> _response = await GetRest<List<string>>("Admin/GetSearchDropDown", _parameters);

            int _count = 0;
            if (_response == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = _dataSource,
                                               Count = _count
                                           } : _dataSource;
            }

            _count = _response.Count;
            _dataSource.AddRange(_response.Select(item => new KeyValues(item, item)));

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = _count
                                       } : _dataSource;
        }
        catch
        {
            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = 0
                                       } : _dataSource;
        }
    }

    /// <summary>
    ///     Asynchronously retrieves autocomplete data for zip codes based on the provided DataManagerRequest.
    ///     The method checks the DataManagerRequest for any filtering conditions and returns a list of zip codes that match
    ///     these conditions.
    ///     The method uses an in-memory cache to store and retrieve the zip codes.
    ///     If the DataManagerRequest requires counts, the method also returns the count of the returned zip codes.
    ///     If an exception occurs during the operation, the method returns an empty list.
    /// </summary>
    /// <param name="dm">The DataManagerRequest containing the filtering conditions for the zip codes.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of zip codes and their
    ///     count if required.
    /// </returns>
    internal static async Task<object> GetAutocompleteZipAsync(DataManagerRequest dm)
    {
        await Task.Yield();
        List<KeyValues> _dataSource = [];

        if (dm.Where is not {Count: > 0} || dm.Where[0].value.NullOrWhiteSpace())
        {
            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = 0
                                       } : _dataSource;
        }

        try
        {
            //IMemoryCache _memoryCache = Start.MemCache;
            List<Zip> _zips = null;
            while (_zips == null)
            {
                _zips = await Redis.GetAsync<List<Zip>>("Zips");
                //_memoryCache.TryGetValue("Zips", out _zips);
            }

            if (_zips.Count == 0)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = _dataSource,
                                               Count = 0
                                           } : _dataSource;
            }

            _dataSource.AddRange(_zips.Where(zip => zip.ZipCode.StartsWith(dm.Where[0].value.ToString() ?? string.Empty)).Select(zip => new KeyValues(zip.ZipCode, zip.ZipCode)));

            int _count = 0;
            _count = _dataSource.Count;

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = _count
                                       } : _dataSource;
        }
        catch
        {
            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = 0
                                       } : _dataSource;
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a list of candidates based on the provided search model and data manager request.
    /// </summary>
    /// <param name="searchModel">The search model containing the parameters for the candidate search.</param>
    /// <param name="dm">The data manager request object that contains the parameters for the data request.</param>
    /// <param name="optionalCandidateID">An optional candidate ID to further specify the data request. Default value is 0.</param>
    /// <param name="thenProceed">
    ///     A boolean value indicating whether to proceed with the operation after retrieving the
    ///     candidates. Default value is false.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The task result contains a list of candidates or a DataResult
    ///     object containing the list of candidates and the total count, depending on the 'RequiresCounts' property of the
    ///     data manager request.
    /// </returns>
    /// <remarks>
    ///     This method sends a GET request to the 'Candidates/GetGridCandidates' endpoint of the API host with the search
    ///     model and optional parameters as the request body.
    ///     It then deserializes the response into a list of candidates and returns the list or a DataResult object containing
    ///     the list and the total count, depending on the 'RequiresCounts' property of the data manager request.
    ///     If an error occurs during the operation, it returns a new list with a single candidate or a DataResult object
    ///     containing the list with a single candidate and a count of 1, depending on the 'RequiresCounts' property of the
    ///     data manager request.
    /// </remarks>
    internal static async Task<object> GetCandidateReadAdaptor(CandidateSearch searchModel, DataManagerRequest dm, int optionalCandidateID = 0, bool thenProceed = false)
    {
        List<Candidates> _dataSource = [];

        int _itemCount = searchModel.ItemCount;
        int _page = searchModel.Page;
        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"candidateID", optionalCandidateID.ToString()},
                                                         {"thenProceed", thenProceed.ToString()}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Candidates/GetGridCandidates", _parameters, searchModel);

            if (_restResponse == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = _dataSource,
                                               Count = 0 /*_count*/
                                           } : _dataSource;
            }

            _dataSource = JsonConvert.DeserializeObject<List<Candidates>>(_restResponse["Candidates"].ToString() ?? string.Empty);
            int _count = _restResponse["Count"].ToInt32();
            searchModel.Page = _restResponse["Page"].ToInt32();
            _page = searchModel.Page;
            Candidate.Count = _count;
            Candidate.PageCount = Math.Ceiling(_count / _itemCount.ToDecimal()).ToInt32();
            Candidate.StartRecord = ((_page - 1) * _itemCount + 1).ToInt32();
            Candidate.EndRecord = ((_page - 1) * _itemCount).ToInt32() + _dataSource.Count;

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = _count /*_count*/
                                       } : _dataSource;
        }
        catch
        {
            if (_dataSource == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = null,
                                               Count = 1
                                           } : null;
            }

            _dataSource.Add(new());

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = 1
                                       } : _dataSource;
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a list of companies based on the provided search model.
    /// </summary>
    /// <param name="searchModel">The search model containing the criteria for the company search.</param>
    /// <param name="user">The user performing the search.</param>
    /// <param name="dm">The DataManagerRequest object that contains additional request parameters.</param>
    /// <param name="getCompanyInformation">
    ///     Optional parameter that indicates whether to retrieve additional company
    ///     information. Default is false.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The task result contains a DataResult object if
    ///     dm.RequiresCounts is true,
    ///     which includes the list of companies and the total count of companies. If dm.RequiresCounts is false, the task
    ///     result contains
    ///     only the list of companies. If an error occurs during the operation, the task result contains a DataResult object
    ///     with a single
    ///     empty company in the Result property and a Count of 1, or a single empty company if dm.RequiresCounts is false.
    /// </returns>
    // ReSharper disable once UnusedParameter.Global
    internal static async Task<object> GetCompanyReadAdaptor(CompanySearch searchModel, string user, DataManagerRequest dm, bool getCompanyInformation = false)
    {
        List<Company> _dataSource = [];

        int _itemCount = searchModel.ItemCount;
        int _page = searchModel.Page;
        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"getCompanyInformation", getCompanyInformation.ToString()}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Company/GetGridCompanies", _parameters, searchModel);

            if (_restResponse == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = _dataSource,
                                               Count = 0 /*_count*/
                                           } : _dataSource;
            }

            _dataSource = JsonConvert.DeserializeObject<List<Company>>(_restResponse["Companies"].ToString() ?? string.Empty);
            int _count = _restResponse["Count"].ToInt32();
            Companies.Count = _count;
            Companies.PageCount = Math.Ceiling(_count / _itemCount.ToDecimal()).ToInt32();
            Companies.StartRecord = ((_page - 1) * _itemCount + 1).ToInt32();
            if (_dataSource == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = null,
                                               Count = 1
                                           } : null;
            }

            Companies.EndRecord = ((_page - 1) * _itemCount).ToInt32() + _dataSource.Count;
            Companies.CompaniesList = JsonConvert.DeserializeObject<List<Company>>(_restResponse["CompaniesList"].ToString() ?? string.Empty);
            Companies.CompanyContacts = JsonConvert.DeserializeObject<List<CompanyContact>>(_restResponse["Contacts"].ToString() ?? string.Empty);
            Companies.Skills = JsonConvert.DeserializeObject<List<IntValues>>(_restResponse["Skills"].ToString() ?? string.Empty);

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = _count /*_count*/
                                       } : _dataSource;
        }
        catch
        {
            if (_dataSource == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = null,
                                               Count = 1
                                           } : null;
            }

            _dataSource.Add(new());

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = 1
                                       } : _dataSource;
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a list of document types based on the provided filter and DataManagerRequest.
    /// </summary>
    /// <param name="filter">The filter string used to filter the document types.</param>
    /// <param name="dm">The DataManagerRequest object that contains the parameters for the read operation.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The value of the TResult parameter contains a list of document
    ///     types or a DataResult object that includes the list and its count, depending on the RequiresCounts property of the
    ///     DataManagerRequest.
    /// </returns>
    /// <remarks>
    ///     This method sends an asynchronous request to the "Admin/GetDocTypes" endpoint of the API host, passing the filter
    ///     as a query parameter. The response is expected to be a dictionary containing a list of document types and their
    ///     count. If the response is null or an exception occurs during the operation, the method handles these scenarios and
    ///     returns appropriate results.
    /// </remarks>
    internal static async Task<object> GetDocTypesAsync(string filter, DataManagerRequest dm)
    {
        List<DocumentType> _dataSource = [];

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"filter", filter}
                                                     };
            Dictionary<string, object> _response = await GetRest<Dictionary<string, object>>("Admin/GetDocTypes", _parameters);
            if (_response == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = _dataSource,
                                                                             Count = 0 /*_count*/
                                                                         } : _dataSource);
            }

            _dataSource = DeserializeObject<List<DocumentType>>(_response["DocTypes"]);
            int _count = _response["Count"].ToInt32();

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = _count
                                                                     } : _dataSource);
        }
        catch
        {
            if (_dataSource == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = null,
                                                                             Count = 1
                                                                         } : null);
            }

            _dataSource.Add(new());

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = 1
                                                                     } : _dataSource);
        }
    }

    /// <summary>
    ///     Retrieves the user ID from the provided login cookie.
    /// </summary>
    /// <param name="cooky">The login cookie containing the user's session information.</param>
    /// <returns>
    ///     The user ID in uppercase if it exists and is not null or whitespace; otherwise, returns "JOLLY".
    /// </returns>
    internal static string GetEmail(LoginCooky cooky) => cooky == null || cooky.Email.NullOrWhiteSpace() ? "info@titan-techs.com" : cooky.Email.ToLowerInvariant();

    /// <summary>
    ///     Asynchronously retrieves a list of leads based on the provided search model and data manager request.
    /// </summary>
    /// <param name="searchModel">The search model containing the search parameters for retrieving leads.</param>
    /// <param name="dm">The data manager request containing additional request parameters.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task result contains a list of leads if successful,
    ///     or a new instance of the LeadClass if an exception occurs during the operation.
    /// </returns>
    /// <remarks>
    ///     This method sends a GET request to the "Lead/GetGridLeads" endpoint with the search model as the request body.
    ///     The response is deserialized into a list of LeadClass instances and additional information such as count,
    ///     page count, start record, and end record are calculated and stored in the Leads object.
    ///     If the DataManagerRequest requires counts, a DataResult object containing the list of leads and the count is
    ///     returned.
    ///     If an exception occurs during the operation, a new instance of the LeadClass is added to the list and returned.
    /// </remarks>
    internal static async Task<object> GetLeadReadAdaptor(LeadSearch searchModel, DataManagerRequest dm)
    {
        List<LeadClass> _dataSource = [];

        int _itemCount = searchModel.ItemCount;
        int _page = searchModel.Page;
        try
        {
            _restResponse = await GetRest<Dictionary<string, object>>("Lead/GetGridLeads", null, searchModel);

            if (_restResponse == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = _dataSource,
                                               Count = 0 /*_count*/
                                           } : _dataSource;
            }

            _dataSource = JsonConvert.DeserializeObject<List<LeadClass>>(_restResponse["Leads"].ToString() ?? string.Empty);
            int _count = _restResponse["Count"].ToInt32();
            Leads.Count = _count;
            Leads.PageCount = Math.Ceiling(_count / _itemCount.ToDecimal()).ToInt32();
            Leads.StartRecord = ((_page - 1) * _itemCount + 1).ToInt32();
            Leads.EndRecord = ((_page - 1) * _itemCount).ToInt32() + _dataSource.Count;

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = _count /*_count*/
                                       } : _dataSource;
        }
        catch
        {
            if (_dataSource == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = null,
                                               Count = 1
                                           } : null;
            }

            _dataSource.Add(new());

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = 1
                                       } : _dataSource;
        }
    }

    /// <summary>
    ///     Asynchronously fetches a list of 'AdminList' objects from the server.
    /// </summary>
    /// <param name="methodName">The name of the method to be called on the server.</param>
    /// <param name="filter">The filter to be applied on the server-side data.</param>
    /// <param name="dm">The DataManagerRequest object that contains the parameters for the data request.</param>
    /// <param name="isString">A flag indicating whether the filter is a string. Default is true.</param>
    /// <param name="enableVirtualization">
    ///     A flag indicating whether the data virtualization is enabled. If set to true, the
    ///     data will be loaded on demand as the user scrolls through the list. Default is false.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task result contains the fetched data as an object.
    ///     If 'dm.RequiresCounts' is true, the result is a 'DataResult' object containing the fetched data and the count of
    ///     the data items.
    ///     Otherwise, the result is a list of 'AdminList' objects.
    /// </returns>
    /// <remarks>
    ///     This method uses the RestClient to send an asynchronous GET request to the server.
    ///     The 'methodName', 'filter', and 'isString' parameters are sent as query parameters in the request.
    ///     If the server response is null or an exception occurs during the request, the method returns a default value.
    /// </remarks>
    internal static async Task<object> GetReadAsync(string methodName, string filter, DataManagerRequest dm, bool isString = true, bool enableVirtualization = false)
    {
        List<AdminList> _dataSource = [];

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"methodName", methodName},
                                                         {"filter", HttpUtility.UrlEncode(filter)},
                                                         {"isString", isString.ToString()}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Admin/GetAdminList", _parameters); //, searchModel);
            if (_restResponse == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = _dataSource,
                                                                             Count = 0 /*_count*/
                                                                         } : _dataSource);
            }

            _dataSource = DeserializeObject<List<AdminList>>(_restResponse["GeneralItems"]);
            int _count = _restResponse["Count"].ToInt32();
            if (enableVirtualization && dm.Skip != 0)
            {
                _dataSource = DataOperations.PerformSkip(_dataSource, dm.Skip).ToList();
            }

            if (enableVirtualization && dm.Take != 0)
            {
                _dataSource = DataOperations.PerformTake(_dataSource, dm.Take).ToList(); // as List<AdminList>;
            }

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = _count
                                                                     } : _dataSource);
        }
        catch
        {
            if (_dataSource == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = null,
                                                                             Count = 1
                                                                         } : null);
            }

            _dataSource.Add(new());

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = 1
                                                                     } : _dataSource);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a list of requisitions based on the provided search model and data manager request.
    ///     This method also has the ability to fetch additional company information if required.
    /// </summary>
    /// <param name="searchModel">The model containing the search parameters for the requisitions.</param>
    /// <param name="dm">The data manager request object.</param>
    /// <param name="getInformation">
    ///     Optional parameter. If set to true, the method will fetch additional company information.
    ///     Default is false.
    /// </param>
    /// <param name="optionalRequisitionID">
    ///     Optional parameter. If provided, the method will fetch a specific requisition by
    ///     its ID. Default is 0.
    /// </param>
    /// <param name="thenProceed">
    ///     Optional parameter. If set to true, the method will continue processing even after
    ///     encountering an error. Default is false.
    /// </param>
    /// <param name="user">
    ///     A string value containing the logged-in user whose role should be a recruiter to fetch additional
    ///     information from the companies list.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of requisitions or a data result
    ///     object if counts are required.
    /// </returns>
    internal static async Task<object> GetRequisitionReadAdaptor(RequisitionSearch searchModel, DataManagerRequest dm, bool getInformation = false, int optionalRequisitionID = 0,
                                                                 bool thenProceed = false, string user = "")
    {
        List<Requisitions> _dataSource = [];

        int _itemCount = searchModel.ItemCount;
        int _page = searchModel.Page;
        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"getCompanyInformation", getInformation.ToString()},
                                                         {"requisitionID", optionalRequisitionID.ToString()},
                                                         {"thenProceed", thenProceed.ToString()},
                                                         {"user", user}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Requisition/GetGridRequisitions", _parameters, searchModel);

            if (_restResponse == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = _dataSource,
                                               Count = 0 /*_count*/
                                           } : _dataSource;
            }

            _dataSource = JsonConvert.DeserializeObject<List<Requisitions>>(_restResponse["Requisitions"].ToString() ?? string.Empty);
            int _count = _restResponse["Count"].ToInt32();
            searchModel.Page = _restResponse["Page"].ToInt32();
            _page = searchModel.Page;
            Requisition.Count = _count;
            Requisition.PageCount = Math.Ceiling(_count / _itemCount.ToDecimal()).ToInt32();
            Requisition.StartRecord = ((_page - 1) * _itemCount + 1).ToInt32();

            if (_dataSource == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = null,
                                               Count = 0 /*_count*/
                                           } : null;
            }

            Requisition.EndRecord = ((_page - 1) * _itemCount).ToInt32() + _dataSource.Count;

            if (!getInformation)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = _dataSource,
                                               Count = _count /*_count*/
                                           } : _dataSource;
            }

            Requisition.Companies = JsonConvert.DeserializeObject<List<Company>>(_restResponse["Companies"].ToString() ?? string.Empty);
            Requisition.CompanyContacts = JsonConvert.DeserializeObject<List<CompanyContact>>(_restResponse["Contacts"].ToString() ?? string.Empty);
            Requisition.Skills = JsonConvert.DeserializeObject<List<IntValues>>(_restResponse["Skills"].ToString() ?? string.Empty);
            Requisition.StatusList = JsonConvert.DeserializeObject<List<KeyValues>>(_restResponse["StatusCount"].ToString() ?? string.Empty);

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = _count /*_count*/
                                       } : _dataSource;
        }
        catch
        {
            if (_dataSource == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = null,
                                               Count = 1
                                           } : null;
            }

            _dataSource.Add(new());

            return dm.RequiresCounts ? new DataResult
                                       {
                                           Result = _dataSource,
                                           Count = 1
                                       } : _dataSource;
        }
    }

    /// <summary>
    ///     Sends a REST request to the specified endpoint and returns the response as an object of type T.
    /// </summary>
    /// <param name="endpoint">The endpoint to which the REST request is sent.</param>
    /// <param name="parameters">A dictionary of query parameters to be included in the REST request.</param>
    /// <param name="jsonBody">An optional JSON body to be included in the REST request.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is the response from the REST request as an
    ///     object of type T.
    /// </returns>
    /// <remarks>
    ///     This method uses the RestClient class to send the REST request. If a JSON body is provided, it is included in the
    ///     request.
    ///     If a dictionary of parameters is provided, they are added as query parameters to the request.
    ///     The method then sends the request and awaits the response. The response is returned as an object of type T.
    /// </remarks>
    private static async Task<T> GetRest<T>(string endpoint, Dictionary<string, string> parameters = null, object jsonBody = null)
    {
        using RestClient _client = new(Start.ApiHost);
        RestRequest _request = new(endpoint)
                               {
                                   RequestFormat = DataFormat.Json
                               };

        if (jsonBody != null)
        {
            _request.AddJsonBody(jsonBody);
        }

        if (parameters == null)
        {
            return await _client.GetAsync<T>(_request);
        }

        foreach (KeyValuePair<string, string> _parameter in parameters)
        {
            _request.AddQueryParameter(_parameter.Key, _parameter.Value);
        }

        return await _client.GetAsync<T>(_request);
    }

    /// <summary>
    ///     Asynchronously retrieves a list of roles from the server based on the provided filter and data manager request.
    /// </summary>
    /// <param name="filter">The filter string to be used in the server request for role data.</param>
    /// <param name="dm">The data manager request object containing additional parameters for the server request.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of roles or a data result
    ///     object with the list of roles and their count, depending on the requirements of the data manager request.
    /// </returns>
    /// <remarks>
    ///     This method sends a GET request to the "Admin/GetRoles" endpoint of the server with the provided filter as a query
    ///     parameter.
    ///     The server response is expected to be a dictionary containing a list of roles and their count.
    ///     If the server response is null, an empty list of roles or a data result object with an empty list of roles and a
    ///     count of 0 is returned, depending on the requirements of the data manager request.
    ///     If an exception occurs during the operation, a new role is added to the list of roles and a list of roles or a data
    ///     result object with the list of roles and a count of 1 is returned, depending on the requirements of the data
    ///     manager request.
    /// </remarks>
    internal static async Task<object> GetRoleDataAdaptorAsync(string filter, DataManagerRequest dm)
    {
        List<Role> _dataSource = [];

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"filter", HttpUtility.UrlEncode(filter)}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Admin/GetRoles", _parameters);

            if (_restResponse == null)
            {
                return dm.RequiresCounts ? new DataResult
                                           {
                                               Result = _dataSource,
                                               Count = 0 /*_count*/
                                           } : _dataSource;
            }

            _dataSource = JsonConvert.DeserializeObject<List<Role>>(_restResponse["Roles"].ToString() ?? string.Empty);
            int _count = _restResponse["Count"].ToInt32();

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = _count
                                                                     } : _dataSource);
        }
        catch
        {
            if (_dataSource == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = null,
                                                                             Count = 1
                                                                         } : null);
            }

            _dataSource.Add(new());

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = 1
                                                                     } : _dataSource);
        }
    }

    /// <summary>
    ///     Retrieves the user ID from the provided login cookie.
    /// </summary>
    /// <param name="cooky">The login cookie containing the user's session information.</param>
    /// <returns>
    ///     The user ID in uppercase if it exists and is not null or whitespace; otherwise, returns "JOLLY".
    /// </returns>
    internal static string GetRoleID(LoginCooky cooky) => cooky == null || cooky.RoleID.NullOrWhiteSpace() ? "RS" : cooky.RoleID.ToUpperInvariant();

    /// <summary>
    ///     Asynchronously retrieves a list of states from the server based on a provided filter and data manager request.
    /// </summary>
    /// <param name="filter">The filter string to be used in the server request.</param>
    /// <param name="dm">The data manager request containing additional parameters for the server request.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of states or a DataResult
    ///     object with the list of states and their count, depending on the RequiresCounts property of the data manager
    ///     request.
    /// </returns>
    /// <remarks>
    ///     This method sends a GET request to the "Admin/GetStates" endpoint of the API host with the provided filter as a
    ///     query parameter.
    ///     The response is expected to be a dictionary with "States" and "Count" keys.
    ///     The "States" key should contain a JSON string representing a list of State objects.
    ///     The "Count" key should contain the total count of State objects.
    ///     If the response is null or an exception occurs during the operation, the method returns a list with a single new
    ///     State object or a DataResult object with a single new State object and a count of 1, depending on the
    ///     RequiresCounts property of the data manager request.
    /// </remarks>
    internal static async Task<object> GetStateDataAdaptorAsync(string filter, DataManagerRequest dm)
    {
        List<State> _dataSource = [];

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"filter", HttpUtility.UrlEncode(filter)}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Admin/GetStates", _parameters);

            if (_restResponse == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = _dataSource,
                                                                             Count = 0 /*_count*/
                                                                         } : _dataSource);
            }

            _dataSource = DeserializeObject<List<State>>(_restResponse["States"]);
            int _count = _restResponse["Count"].ToInt32();

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = _count
                                                                     } : _dataSource);
        }
        catch
        {
            if (_dataSource == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = null,
                                                                             Count = 1
                                                                         } : null);
            }

            _dataSource.Add(new());

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = 1
                                                                     } : _dataSource);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a list of status codes from an API endpoint and formats the result according to the
    ///     provided DataManagerRequest.
    /// </summary>
    /// <param name="filter">The filter string to be used in the API request for filtering the status codes.</param>
    /// <param name="dm">The DataManagerRequest object that specifies the requirements for the returned data.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of status codes or a
    ///     DataResult object that includes the list of status codes and their count, depending on the requirements specified
    ///     in the DataManagerRequest.
    /// </returns>
    internal static async Task<object> GetStatusCodeReadAdaptorAsync(string filter, DataManagerRequest dm)
    {
        List<StatusCode> _dataSource = [];

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"filter", HttpUtility.UrlEncode(filter)}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Admin/GetStatusCodes", _parameters);

            if (_restResponse == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = _dataSource,
                                                                             Count = 0 /*_count*/
                                                                         } : _dataSource);
            }

            _dataSource = DeserializeObject<List<StatusCode>>(_restResponse["StatusCodes"]);
            int _count = _restResponse["Count"].ToInt32();

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = _count
                                                                     } : _dataSource);
        }
        catch
        {
            if (_dataSource == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = null,
                                                                             Count = 1
                                                                         } : null);
            }

            _dataSource.Add(new());

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = 1
                                                                     } : _dataSource);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a list of templates from the server using the provided filter and DataManagerRequest.
    /// </summary>
    /// <param name="dm">The DataManagerRequest object that contains the parameters for the server request.</param>
    /// <param name="filter">The filter string to be applied on the server side. Default is an empty string.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of templates if successful,
    ///     otherwise, it contains an empty list or a single default template in case of an exception.
    ///     If the DataManagerRequest requires counts, the task result is a DataResult object that includes both the list of
    ///     templates and the count.
    /// </returns>
    internal static async Task<object> GetTemplateReadAdaptor(DataManagerRequest dm, string filter = "") //string name, int page, int count)
    {
        List<Template> _dataSource = [];

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"filter", HttpUtility.UrlEncode(filter)}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Admin/GetTemplateList", _parameters);

            if (_restResponse == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = _dataSource,
                                                                             Count = 0 /*_count*/
                                                                         } : _dataSource);
            }

            _dataSource = DeserializeObject<List<Template>>(_restResponse["Templates"]);

            int _count = _restResponse["Count"].ToInt32();

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = _count
                                                                     } : _dataSource);
        }
        catch
        {
            if (_dataSource == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = null,
                                                                             Count = 1
                                                                         } : null);
            }

            _dataSource.Add(new());

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = 1
                                                                     } : _dataSource);
        }
    }

    /// <summary>
    ///     Retrieves the user ID from the provided login cookie.
    /// </summary>
    /// <param name="cooky">The login cookie containing the user's session information.</param>
    /// <returns>
    ///     The user ID in uppercase if it exists and is not null or whitespace; otherwise, returns "JOLLY".
    /// </returns>
    internal static string GetUserName(LoginCooky cooky) => cooky == null || cooky.UserID.NullOrWhiteSpace() ? "JOLLY" : cooky.UserID.ToUpperInvariant();

    /// <summary>
    ///     Asynchronously fetches a list of users based on the provided filter and DataManagerRequest.
    /// </summary>
    /// <param name="filter">A string used to filter the users to be fetched.</param>
    /// <param name="dm">The DataManagerRequest object that contains the parameters for the data request.</param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task result contains a list of users or a DataResult object
    ///     that includes the list of users and their count, depending on the RequiresCounts property of the
    ///     DataManagerRequest.
    /// </returns>
    /// <remarks>
    ///     This method sends a GET request to the "Admin/GetUserList" endpoint of the API, passing the filter as a query
    ///     parameter.
    ///     The response is expected to be a dictionary containing a list of users and their count. The users are deserialized
    ///     into
    ///     a list of User objects and the count is extracted. If the DataManagerRequest requires counts, a DataResult object
    ///     is
    ///     created with the list of users and their count; otherwise, the list of users is returned. If an exception occurs
    ///     during
    ///     the operation, the method returns a list with a single, default User object or a DataResult object with a count of
    ///     1,
    ///     depending on the RequiresCounts property of the DataManagerRequest.
    /// </remarks>
    internal static async Task<object> GetUserReadAsync(string filter, DataManagerRequest dm)
    {
        List<User> _dataSource = [];

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"filter", HttpUtility.UrlEncode(filter)}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Admin/GetUserList", _parameters);

            if (_restResponse == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = _dataSource,
                                                                             Count = 0 /*_count*/
                                                                         } : _dataSource);
            }

            _dataSource = DeserializeObject<List<User>>(_restResponse["Users"]);
            AppUsers.Roles = DeserializeObject<List<KeyValues>>(_restResponse["Roles"]);

            int _count = _restResponse["Count"].ToInt32();

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = _count
                                                                     } : _dataSource);
        }
        catch
        {
            if (_dataSource == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = null,
                                                                             Count = 1
                                                                         } : null);
            }

            _dataSource.Add(new());

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = 1
                                                                     } : _dataSource);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves the variable commission data from the server.
    /// </summary>
    /// <remarks>
    ///     This method sends a GET request to the "Admin/GetVariableCommission" endpoint of the server specified by the
    ///     ApiHost property of the Start class.
    ///     The response is expected to be a JSON object that can be deserialized into an instance of the VariableCommission
    ///     class.
    ///     If the request fails for any reason, or if the response is null, a new instance of the VariableCommission class is
    ///     created with default values and returned.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result is an instance of the VariableCommission class
    ///     containing the retrieved data.
    /// </returns>
    internal static async Task<VariableCommission> GetVariableCommission()
    {
        try
        {
            VariableCommission _response = await GetRest<VariableCommission>("Admin/GetVariableCommission");

            return _response ?? new(1, 1920, 24, 12, 3, 15);
        }
        catch
        {
            return new(1, 1920, 24, 12, 3, 15);
        }
    }

    /// <summary>
    ///     Asynchronously retrieves a list of workflows based on the provided filter and data manager request.
    /// </summary>
    /// <param name="filter">The filter string to be used in the workflow retrieval.</param>
    /// <param name="dm">The data manager request object containing additional parameters for the workflow retrieval.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of workflows or a data
    ///     result object with the list of workflows and their count, depending on the requirements of the data manager
    ///     request.
    /// </returns>
    internal static async Task<object> GetWorkflowAsync(string filter, DataManagerRequest dm)
    {
        List<AppWorkflow> _dataSource = [];

        try
        {
            Dictionary<string, string> _parameters = new()
                                                     {
                                                         {"filter", HttpUtility.UrlEncode(filter)}
                                                     };
            _restResponse = await GetRest<Dictionary<string, object>>("Admin/GetWorkflows", _parameters);

            if (_restResponse == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = _dataSource,
                                                                             Count = 0 /*_count*/
                                                                         } : _dataSource);
            }

            _dataSource = DeserializeObject<List<AppWorkflow>>(_restResponse["Workflows"]);
            Workflow.Roles = DeserializeObject<List<KeyValues>>(_restResponse["Roles"]);
            Workflow.Status = DeserializeObject<List<KeyValues>>(_restResponse["Status"]);
            int _count = _restResponse["Count"].ToInt32();

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = _count
                                                                     } : _dataSource);
        }
        catch
        {
            if (_dataSource == null)
            {
                return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                         {
                                                                             Result = null,
                                                                             Count = 1
                                                                         } : null);
            }

            _dataSource.Add(new());

            return await Task.FromResult<object>(dm.RequiresCounts ? new DataResult
                                                                     {
                                                                         Result = _dataSource,
                                                                         Count = 1
                                                                     } : _dataSource);
        }
    }

    /// <summary>
    ///     Computes the MD5 hash value for the specified string.
    /// </summary>
    /// <param name="inputText">The input string to compute the hash code for.</param>
    /// <returns>The computed hash code in the form of a byte array.</returns>
    /// <remarks>
    ///     This method takes a string as input and returns a byte array representing the MD5 hash of the input string. It is
    ///     used in the application for hashing passwords.
    /// </remarks>
    internal static byte[] Md5PasswordHash(string inputText) => MD5.Create().ComputeHash(new UTF8Encoding().GetBytes(inputText));

    /// <summary>
    ///     Sends a POST request to the specified endpoint with the provided parameters and JSON body.
    /// </summary>
    /// <param name="endpoint">The API endpoint to which the request is sent.</param>
    /// <param name="parameters">The parameters to be included in the request.</param>
    /// <param name="jsonBody">The JSON body to be included in the request. Default is null.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a dictionary with the response data.
    /// </returns>
    /// <remarks>
    ///     This method uses the RestClient to send a POST request to the API.
    ///     If a JSON body is provided, it is added to the request.
    ///     All key-value pairs in the parameters dictionary are added as query parameters to the request.
    /// </remarks>
    internal static Task<Dictionary<string, object>> PostRest(string endpoint, Dictionary<string, string> parameters, object jsonBody = null) =>
        PostRest<Dictionary<string, object>>(endpoint, parameters, jsonBody);

    /// <summary>
    ///     Sends a POST request to the specified endpoint with the provided parameters and JSON body.
    /// </summary>
    /// <param name="endpoint">The API endpoint to which the request is sent.</param>
    /// <param name="parameters">The parameters to be included in the request.</param>
    /// <param name="jsonBody">The JSON body to be included in the request. Default is null.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a dictionary with the response data.
    /// </returns>
    /// <remarks>
    ///     This method uses the RestClient to send a POST request to the API.
    ///     If a JSON body is provided, it is added to the request.
    ///     All key-value pairs in the parameters dictionary are added as query parameters to the request.
    /// </remarks>
    internal static async Task<T> PostRest<T>(string endpoint, Dictionary<string, string> parameters = null, object jsonBody = null)
    {
        using RestClient _client = new(Start.ApiHost);
        RestRequest _request = new(endpoint, Method.Post)
                               {
                                   RequestFormat = DataFormat.Json
                               };

        if (jsonBody != null)
        {
            _request.AddJsonBody(jsonBody);
        }

        if (parameters == null)
        {
            return await _client.PostAsync<T>(_request);
        }

        foreach (KeyValuePair<string, string> _parameter in parameters)
        {
            _request.AddQueryParameter(_parameter.Key, _parameter.Value);
        }

        return await _client.PostAsync<T>(_request);
    }

    /// <summary>
    ///     Asynchronously toggles the status of a given entity in the application.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="methodName">The name of the method to be invoked on the server side.</param>
    /// <param name="id">The identifier of the entity to be toggled.</param>
    /// <param name="userName">The username of the user performing the operation.</param>
    /// <param name="isString">A boolean value indicating whether the identifier is a string.</param>
    /// <param name="grid">The grid control that displays the entity.</param>
    /// <param name="isUser">A boolean value indicating whether the entity is a user. Default is false.</param>
    /// <param name="runtime">The JavaScript runtime. Default is null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    ///     This method sends a POST request to the 'Admin/ToggleAdminList' endpoint with the provided parameters.
    ///     After the request is completed, it refreshes the grid control and selects the row corresponding to the toggled
    ///     entity.
    ///     If a JavaScript runtime is provided, it scrolls the grid control to the selected row.
    /// </remarks>
    internal static async Task PostToggleAsync<T>(string methodName, object id, string userName, bool isString, SfGrid<T> grid, bool isUser = false, IJSRuntime runtime = null)
    {
        Dictionary<string, string> _parameters = new()
                                                 {
                                                     {"methodName", methodName},
                                                     {"id", id.ToString()},
                                                     {"username", userName},
                                                     {"idIsString", isString.ToString()},
                                                     {"isUser", isUser.ToString()}
                                                 };

        string _response = await PostRest<string>("Admin/ToggleAdminList", _parameters);

        await grid.Refresh();

        int _index = await grid.GetRowIndexByPrimaryKeyAsync(isString ? _response : _response.ToInt32());
        await grid.SelectRowAsync(_index);
        if (runtime != null)
        {
            await runtime.InvokeVoidAsync("scroll", _index);
        }
    }

    /// <summary>
    ///     Asynchronously saves the admin list to the database.
    /// </summary>
    /// <typeparam name="T">The type of the data in the admin list.</typeparam>
    /// <param name="methodName">The name of the method to be called for saving the admin list.</param>
    /// <param name="parameterName">The name of the parameter to be passed to the method.</param>
    /// <param name="containDescription">Indicates whether the admin list contains a description.</param>
    /// <param name="isString">Indicates whether the primary key is a string.</param>
    /// <param name="adminList">The admin list to be saved.</param>
    /// <param name="grid">The grid where the admin list is displayed.</param>
    /// <param name="mainAdminList">The main admin list. If provided, it will be updated with the saved admin list.</param>
    /// <param name="runtime">The JavaScript runtime. If provided, it will be used to scroll to the saved row in the grid.</param>
    /// <returns>A Task representing the asynchronous operation. The result of the Task is the response from the API call.</returns>
    /// <remarks>
    ///     This method performs the following steps:
    ///     - Sends a POST request to the "Admin/SaveAdminList" endpoint with the provided parameters and the admin list in the
    ///     request body.
    ///     - If a main admin list is provided, it updates the main admin list with the saved admin list.
    ///     - Refreshes the grid.
    ///     - Selects the saved row in the grid.
    ///     - If a JavaScript runtime is provided, it scrolls to the saved row in the grid.
    /// </remarks>
    internal static async Task SaveAdminListAsync<T>(string methodName, string parameterName, bool containDescription, bool isString, AdminList adminList, SfGrid<T> grid,
                                                     AdminList mainAdminList = null, IJSRuntime runtime = null)
    {
        Dictionary<string, string> _parameters = new()
                                                 {
                                                     {"methodName", methodName},
                                                     {"parameterName", parameterName},
                                                     {"containsDescription", containDescription.ToString()},
                                                     {"isString", isString.ToString()}
                                                 };

        string _response = await PostRest<string>("Admin/SaveAdminList", _parameters, adminList);

        if (mainAdminList != null)
        {
            mainAdminList = adminList.Copy();
        }

        await grid.Refresh();

        int _index = await grid.GetRowIndexByPrimaryKeyAsync(isString ? _response : _response.ToInt32());
        await grid.SelectRowAsync(_index);
        if (runtime != null)
        {
            await runtime.InvokeVoidAsync("scroll", _index);
        }

        await Task.FromResult(_response);
    }

    /// <summary>
    ///     Computes the SHA-512 hash of the input text.
    /// </summary>
    /// <param name="inputText">The text to be hashed.</param>
    /// <returns>A byte array representing the SHA-512 hash of the input text.</returns>
    public static byte[] SHA512PasswordHash(string inputText) => SHA512.Create().ComputeHash(new UTF8Encoding().GetBytes(inputText));
}