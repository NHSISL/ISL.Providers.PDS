// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Mappers;
using ISL.Providers.PDS.FakeFHIR.Models;
using ISL.Providers.PDS.FakeFHIR.Providers.FakeFHIR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Acceptance
{
    public partial class PdsServiceTests
    {
        private readonly IFakeFHIRProvider fakeFHIRProvider;
        private readonly IConfiguration configuration;

        public PdsServiceTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();

            FakeFHIRProviderConfigurations fakeFHIRProviderConfigurations = configuration
                .GetSection("fakeFHIRProviderConfigurations")
                    .Get<FakeFHIRProviderConfigurations>();

            this.fakeFHIRProvider = new FakeFHIRProvider(fakeFHIRProviderConfigurations);
        }

        private static PdsPatientDetails GetPdsPatientDetailsFromFakeFHIRPatient(
            FakeFHIRProviderPatientDetails patientDetails)
        {
            PdsPatientDetails pdsPatientDetails = new PdsPatientDetails
            {
                Title = patientDetails.Title,
                GivenNames = patientDetails.GivenNames,
                Surname = patientDetails.Surname,
                Gender = patientDetails.Gender,
                PhoneNumber = patientDetails.PhoneNumber,
                EmailAddress = patientDetails.Email,
                Address = patientDetails.Address,
                NhsNumber = patientDetails.NhsNumber,
                DateOfBirth = patientDetails.DateOfBirth,
                DateOfDeath = patientDetails.DateOfDeath,
                RegisteredGpPractice = patientDetails.RegisteredGpPractice
            };

            return pdsPatientDetails;
        }

        private static PatientBundle CreatePatientBundle(
            List<FakeFHIRProviderPatientDetails> patientDetailsList,
            string surname)
        {
            var patientDetails = patientDetailsList
                .Where(patientDetails => patientDetails.Surname.Contains(surname)).FirstOrDefault();

            PdsPatientDetails pdsPatientDetails = GetPdsPatientDetailsFromFakeFHIRPatient(patientDetails);
            Patient patient = PatientMapper.FromPdsPatientDetails(pdsPatientDetails);
            Bundle bundle = BundleMapper.FromListOfPatients([patient]);
            PatientBundle patientBundle = PatientBundleMapper.FromBundle(bundle);

            return patientBundle;
        }

        private static Patient CreatePatient(List<FakeFHIRProviderPatientDetails> patientDetailsList, string nhsNumber)
        {
            var patientDetails = patientDetailsList
                .Where(patientDetails => patientDetails.NhsNumber.Contains(nhsNumber)).FirstOrDefault();

            PdsPatientDetails pdsPatientDetails = GetPdsPatientDetailsFromFakeFHIRPatient(patientDetails);
            Patient patient = PatientMapper.FromPdsPatientDetails(pdsPatientDetails);

            return patient;
        }
    }
}
