#region Header

// /*****************************************
// Copyright:           Titan-Techs.
// Location:            Newtown, PA, USA
// Solution:            ProfSvc_AppTrack
// Project:             ProfSvc_Classes
// File Name:           TaxTermValidator.cs
// Created By:          Narendra Kumaran Kadhirvelu, Jolly Joseph Paily, DonBosco Paily, Mariappan Raja
// Created On:          07-07-2023 20:59
// Last Updated On:     07-07-2023 21:00
// *****************************************/

#endregion

#region Using

using FluentValidation;

using Microsoft.Extensions.Configuration;

using RestSharp;

#endregion

namespace ProfSvc_Classes.Validators;

public class TaxTermValidator : AbstractValidator<TaxTerm>
{
    public TaxTermValidator()
    {
        IConfigurationBuilder builder =
            new ConfigurationBuilder().AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

        IConfigurationRoot _configuration = builder.Build();

        ApiHost = _configuration.GetSection("APIHost").Value;

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Code).NotEmpty().WithMessage("Tax Term Code should not be empty.")
                            .Length(1).WithMessage("Tax Term Code should be exactly {MaxLength} character.")
                            .Must(CheckTaxTermCodeExists).WithMessage("Tax Term Code already exists. Enter another Tax Term Code.");

        RuleFor(x => x.TaxTermItem).NotEmpty().WithMessage("Tax Term should not be empty.")
                            .Length(2, 50).WithMessage("Tax Term should be between {MinLength} and {MaxLength} character.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Tax Term Description should not be empty.")
                            .Length(10, 500).WithMessage("Tax Term Description should be between {MinLength} and {MaxLength} character.")
                                   .Must((obj, taxTerm) => CheckTaxTermExists(obj.Code, taxTerm)).WithMessage("Tax Term already exists. Enter another Tax Term");
    }

    private string ApiHost
    {
        get;
    }

    public bool CheckTaxTermCodeExists(string taxTermCode)
    {
        RestClient _restClient = new(ApiHost ?? string.Empty);
        RestRequest _request = new("Admin/CheckTaxTermCode")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("id", taxTermCode);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return !_response;
    }

    public bool CheckTaxTermExists(string taxTermCode, string taxTerm)
    {
        RestClient _restClient = new(ApiHost ?? string.Empty);
        RestRequest _request = new("Admin/CheckTaxTer,")
                               {
                                   RequestFormat = DataFormat.Json
                               };
        _request.AddQueryParameter("code", taxTermCode);
        _request.AddQueryParameter("text", taxTerm);
        bool _response = _restClient.GetAsync<bool>(_request).Result;

        return !_response;
    }
}