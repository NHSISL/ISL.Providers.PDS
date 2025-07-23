// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Mappers;
using ISL.Providers.PDS.FakeFHIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal partial class PdsService : IPdsService
    {
        private readonly FakeFHIRProviderConfigurations fakeFHIRProviderConfiguration;

        public PdsService(
            FakeFHIRProviderConfigurations fakeFHIRProviderConfiguration)
        { 
            this.fakeFHIRProviderConfiguration = fakeFHIRProviderConfiguration;
        }

        public ValueTask<PatientBundle> PatientLookupByDetailsAsync(string givenName = null,
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
                List<PdsPatientDetails> fakePatientDetails = GetFakePatientDetails();

                List<PdsPatientDetails> filteredPatients = FilterPatientByDetails(
                    fakePatientDetails,
                    givenName,
                    familyName,
                    gender,
                    postcode,
                    dateOfBirth,
                    dateOfDeath, registeredGpPractice,
                    email,
                    phoneNumber);

                var patients = new List<Patient>();

                foreach (var filteredPatient in filteredPatients)
                {
                    Patient patient = PatientMapper.FromPdsPatientDetails(filteredPatient);
                    patients.Add(patient);
                }

                Bundle bundle = BundleMapper.FromListOfPatients(patients);
                PatientBundle patientBundle = PatientBundleMapper.FromBundle(bundle);

                return patientBundle;
            });

        public ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber) =>
            TryCatch(async () =>
            {
                ValidatePatientLookupByNhsNumberArguments(nhsNumber);

                List<PdsPatientDetails> fakePatientDetails = GetFakePatientDetails();

                PdsPatientDetails pdsPatientDetails = 
                    fakePatientDetails.Where(patient => patient.NhsNumber == nhsNumber).FirstOrDefault();

                ValidatePdsPatientDetails(pdsPatientDetails, nhsNumber);

                Patient patient = PatientMapper.FromPdsPatientDetails(pdsPatientDetails);

                return patient;
            });

        virtual internal List<PdsPatientDetails> GetFakePatientDetails()
        {
            List<PdsPatientDetails> fakePatients = new List<PdsPatientDetails>();

            foreach (FakeFHIRProviderPatientDetails patientDetails in fakeFHIRProviderConfiguration.FakePatients)
            {
                PdsPatientDetails pdsPatientDetails = new PdsPatientDetails
                {
                    Title = patientDetails.Title,
                    GivenName = patientDetails.GivenName,
                    Surname = patientDetails.Surname,
                    Gender = patientDetails.Gender,
                    PhoneNumber = patientDetails.PhoneNumber,
                    EmailAddress = patientDetails.EmailAddress,
                    Address = patientDetails.Address,
                    NhsNumber = patientDetails.NhsNumber,
                    DateOfBirth = DateTimeOffset.Parse(patientDetails.DateOfBirth),
                    DateOfDeath = DateTimeOffset.Parse(patientDetails.DateOfDeath),
                    RegisteredGpPractice = patientDetails.RegisteredGpPractice
                };

                fakePatients.Add(pdsPatientDetails);
            }

            return fakePatients;
        }

        virtual internal List<PdsPatientDetails> FilterPatientByDetails(
            List<PdsPatientDetails> patientDetails,
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
            var patients = patientDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(givenName))
            {
                patients = patients.Where(patient => patient.GivenName.Contains(givenName));
            }

            if (!string.IsNullOrWhiteSpace(familyName))
            {
                patients = patients.Where(patient => patient.Surname.Contains(familyName));
            }

            if (!string.IsNullOrWhiteSpace(postcode))
            {
                patients = patients.Where(patient => patient.Address.Contains(postcode));
            }

            if (!string.IsNullOrWhiteSpace(dateOfBirth))
            {
                patients = patients.Where(patient => patient.DateOfBirth.Equals(DateTimeOffset.Parse(dateOfBirth)));
            }

            if (!string.IsNullOrWhiteSpace(dateOfDeath))
            {
                patients = patients.Where(patient => patient.DateOfDeath.Equals(DateTimeOffset.Parse(dateOfDeath)));
            }

            if (!string.IsNullOrWhiteSpace(registeredGpPractice))
            {
                patients = patients.Where(patient => patient.RegisteredGpPractice.Contains(registeredGpPractice));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                patients = patients.Where(patient => patient.EmailAddress.Contains(email));
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                patients = patients.Where(patient => patient.PhoneNumber.Contains(phoneNumber));
            }

            return patients.ToList();
        }
    }
}
