﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker;
using ISL.Providers.PDS.FHIR.Mappers;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;

namespace ISL.Providers.PDS.FHIR.Services.Foundations.Pds
{
    internal partial class PdsService : IPdsService
    {
        private readonly IPdsFHIRBroker pdsFHIRBroker;
        private readonly PdsFHIRConfigurations pdsFHIRConfigurations;

        public PdsService(IPdsFHIRBroker pdsFHIRBroker, PdsFHIRConfigurations pdsFHIRConfigurations)
        {
            this.pdsFHIRBroker = pdsFHIRBroker;
            this.pdsFHIRConfigurations = pdsFHIRConfigurations;
        }

        public ValueTask<PatientBundle> PatientLookupByDetailsAsync(
            string givenName = null,
            string familyName = null,
            string gender = null,
            string postcode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null) =>
            TryCatch(async () =>
            {
                string searchPath = GetPatientLookupByDetailsPath(
                    givenName,
                    familyName,
                    gender,
                    postcode,
                    dateOfBirth,
                    dateOfDeath,
                    registeredGpPractice,
                    email,
                    phoneNumber);

                Bundle bundle = await pdsFHIRBroker.GetPdsPatientDetailsAsync(searchPath);
                PatientBundle patientBundle = PatientBundleMapper.FromBundle(bundle);

                return patientBundle;
            });

        public ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber) =>
        TryCatch(async () =>
        {
            ValidatePatientLookupByNhsNumberArguments(nhsNumber);
            string path = $"{this.pdsFHIRConfigurations.PatientLookupPath}/{nhsNumber}";
            Patient patient = await pdsFHIRBroker.GetNhsNumberAsync(path);

            return patient;
        });

        virtual internal string GetPatientLookupByDetailsPath(
            string givenName = null,
            string familyName = null,
            string gender = null,
            string postcode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(givenName))
            {
                var splitGivenNames = string.Join("&",
                    givenName
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(name => $"given={name}"));

                queryParams.Add(splitGivenNames);
            }

            if (!string.IsNullOrEmpty(familyName))
            {
                queryParams.Add($"family={familyName}");
            }

            if (!string.IsNullOrEmpty(gender))
            {
                queryParams.Add($"gender={gender}");
            }

            if (!string.IsNullOrEmpty(postcode))
            {
                queryParams.Add($"address-postalcode={postcode}");
            }

            if (!string.IsNullOrEmpty(dateOfBirth))
            {
                queryParams.Add($"birthdate=eq{dateOfBirth}");
            }

            if (!string.IsNullOrEmpty(dateOfDeath))
            {
                queryParams.Add($"death-date=eq{dateOfDeath}");
            }

            if (!string.IsNullOrEmpty(registeredGpPractice))
            {
                queryParams.Add($"general-practitioner={registeredGpPractice}");
            }

            if (!string.IsNullOrEmpty(email))
            {
                queryParams.Add($"email={email}");
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                queryParams.Add($"phone={phoneNumber}");
            }

            string queryString = string.Join("&", queryParams);
            string path = $"{pdsFHIRConfigurations.PatientSearchPath}?{queryString}";

            return path;
        }
    }
}
