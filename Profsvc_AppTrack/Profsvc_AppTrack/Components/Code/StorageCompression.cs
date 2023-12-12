#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_AppTrack
// File Name:           StorageCompression.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily
// Created On:          09-17-2022 20:01
// Last Updated On:     09-17-2022 21:05
// *****************************************/

#endregion

namespace Profsvc_AppTrack.Components.Code;

/// <summary>
/// </summary>
public class StorageCompression
{
    /// <summary>
    /// </summary>
    /// <param name="storage"></param>
    public StorageCompression(ProtectedLocalStorage storage) => LocalStorage = storage;

    private ProtectedLocalStorage LocalStorage
    {
        get;
    }

    /// <summary>
    /// </summary>
    /// <param name="cookieName"></param>
    /// <returns></returns>
    public async Task<bool> Delete(string cookieName)
    {
        try
        {
            await LocalStorage.DeleteAsync(cookieName);

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="cookieName"></param>
    /// <returns></returns>
    public async Task<LoginCooky> Get(string cookieName)
    {
        try
        {
            ProtectedBrowserStorageResult<LoginCooky> _resultValue = await LocalStorage.GetAsync<LoginCooky>(cookieName);
            //if (_resultValue.Value == null)
            //{
            //    _cooky = new();
            //       await LocalStorage.SetAsync("GridVal", _cooky);
            //}
            //else
            //{
            LoginCooky _loginCooky = _resultValue.Value ?? new();
            //}

            return _loginCooky;
        }
        catch
        {
            return new();
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public async Task<CandidateGrid> GetCandidateGrid()
    {
        try
        {
            await Task.Yield();
            //ProtectedBrowserStorageResult<CandidateGrid> _resultValue = await LocalStorage.GetAsync<CandidateGrid>("CandidateGrid");

            //CandidateGrid _candidateGrid;
            //if (_resultValue.Value == null)
            //{
            //    _candidateGrid = new();
            //    try
            //    {
            //        await LocalStorage.SetAsync("CandidateGrid", _candidateGrid);
            //    }
            //    catch
            //    {
            //    }
            //}
            //else
            //{
            //_candidateGrid = _resultValue.Value ?? new();
            //}

            return null; //_candidateGrid;
        }
        catch
        {
            return null;
            //return new();
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="cookieName"></param>
    /// <param name="cookieValue"></param>
    /// <returns></returns>
    public async Task<int> Set(string cookieName, LoginCooky cookieValue)
    {
        try
        {
            await LocalStorage.SetAsync(cookieName, cookieValue);
        }
        catch (Exception)
        {
            return 0;
        }

        return 0;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public async Task<int> SetCandidateGrid()
    {
        try
        {
            await Task.Yield();
            //await LocalStorage.SetAsync("CandidateGrid", Candidate.CandidateGridPersistValues);
        }
        catch (Exception)
        {
            return 0;
        }

        return 0;
    }
}