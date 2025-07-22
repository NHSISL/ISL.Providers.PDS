// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Mappers;
using ISL.Providers.PDS.FakeFHIR.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal partial class PdsService : IPdsService
    {
        private readonly FakeFHIRProviderConfigurations fakeFHIRProviderConfiguration;
        private List<PdsPatientDetails> FakePatientDetails = new List<PdsPatientDetails>();

        public PdsService(
            FakeFHIRProviderConfigurations fakeFHIRProviderConfiguration)
        { 
            this.fakeFHIRProviderConfiguration = fakeFHIRProviderConfiguration;
            this.FakePatientDetails = GetFakePatientDetails();
        }

        public ValueTask<PatientBundle> PatientLookupByDetailsAsync(string givenName = null,
            string familyName = null,
            string gender = null,
            string postCode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null) =>
            TryCatch(async () =>
            {
                ValidatePatientLookupByDetailsArguments(givenName);

                Bundle bundle = new Bundle();
                PatientBundle patientBundle = PatientBundleMapper.FromBundle(bundle);

                return patientBundle;
            });

        public ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber) =>
            TryCatch(async () =>
            {
                ValidatePatientLookupByNhsNumberArguments(nhsNumber);

                Patient patient = new Patient();

                return patient;
            });

        private List<PdsPatientDetails> GetFakePatientDetails()
        {
            List<PdsPatientDetails> fakePatients = new List<PdsPatientDetails>();

            foreach (FakeFHIRProviderPatientDetails patientDetails in fakeFHIRProviderConfiguration.FakePatients)
            {
                PdsPatientDetails pdsPatientDetails = new PdsPatientDetails
                {
                    Title = patientDetails.Title,
                    GivenNames = patientDetails.GivenNames,
                    Surname = patientDetails.Surname,
                    PhoneNumber = patientDetails.PhoneNumber,
                    EmailAddress = patientDetails.EmailAddress,
                    Address = patientDetails.Address,
                    Postcode = patientDetails.Postcode,
                    NhsNumber = patientDetails.NhsNumber,
                    DateOfBirth = DateTimeOffset.Parse(patientDetails.DateOfBirth)
                };

                fakePatients.Add(pdsPatientDetails);
            }

            return fakePatients;
        }
    }
}
