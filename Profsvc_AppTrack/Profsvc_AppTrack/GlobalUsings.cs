#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            Profsvc_AppTrack
// Project:             Profsvc_AppTrack
// File Name:           GlobalUsings.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          11-23-2023 19:47
// Last Updated On:     12-05-2023 16:09
// *****************************************/

#endregion

#region Using

#region System

global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Data;
global using System.Linq;
global using System.Linq.Expressions;
global using System.Net.Http;
global using System.Security.Cryptography;
global using System.Text;
global using System.Threading.Tasks;
global using System.Web;
global using JsonSerializer = System.Text.Json.JsonSerializer;

#endregion

#region Blazored

global using Blazored.LocalStorage;
global using Blazored.SessionStorage;

#endregion

#region Microsoft

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.Data.SqlClient;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Primitives;
global using Microsoft.JSInterop;

#endregion

#region NewtonSoft

global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;

#endregion

#region ProfSvc

global using Extensions;
global using LabelComponents.Areas.MyFeature.Pages;
global using Profsvc_AppTrack;
global using Profsvc_AppTrack.Components;
global using Profsvc_AppTrack.Components.Code;
global using Profsvc_AppTrack.Components.Pages.Admin.Controls;
global using Profsvc_AppTrack.Components.Pages.Controls.Candidates;
global using Profsvc_AppTrack.Components.Pages.Controls.Requisitions;
global using ProfSvc_Classes;
global using ProfSvc_Classes.Validation;
global using WorkflowDialog = Profsvc_AppTrack.Components.Pages.Admin.Controls.WorkflowDialog;

#endregion

#region RestSharp

global using RestSharp;

#endregion

#region Serilog

global using Serilog;

#endregion

#region Syncfusion

global using Syncfusion.Blazor;
global using Syncfusion.Blazor.Buttons;
global using Syncfusion.Blazor.Calendars;
global using Syncfusion.Blazor.Data;
global using Syncfusion.Blazor.DocumentEditor;
global using Syncfusion.Blazor.DropDowns;
global using Syncfusion.Blazor.Grids;
global using Syncfusion.Blazor.Inputs;
global using Syncfusion.Blazor.Navigations;
global using Syncfusion.Blazor.Popups;
global using Syncfusion.Blazor.RichTextEditor;
global using Syncfusion.Blazor.SfPdfViewer;
global using Syncfusion.Blazor.Spinner;
global using Syncfusion.Blazor.SplitButtons;
global using Syncfusion.Licensing;
global using Action = Syncfusion.Blazor.Grids.Action; 
global using ActionCompleteEventArgs = Syncfusion.Blazor.Inputs.ActionCompleteEventArgs;
global using BeforeOpenEventArgs = Syncfusion.Blazor.Popups.BeforeOpenEventArgs;
global using ChangeEventArgs = Microsoft.AspNetCore.Components.ChangeEventArgs;
global using ClickEventArgs = Syncfusion.Blazor.DocumentEditor.ClickEventArgs;
global using FailureEventArgs = Syncfusion.Blazor.Inputs.FailureEventArgs;
global using FocusEventArgs = Syncfusion.Blazor.RichTextEditor.FocusEventArgs;

#endregion

#endregion