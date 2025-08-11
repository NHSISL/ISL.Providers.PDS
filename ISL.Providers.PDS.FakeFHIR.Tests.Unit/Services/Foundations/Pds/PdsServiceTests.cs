// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using KellermanSoftware.CompareNetObjects;
using System;
using Tynamix.ObjectFiller;
using ISL.Providers.PDS.FakeFHIR.Services.Foundations;
using ISL.Providers.PDS.Abstractions.Models;
using Hl7.Fhir.Model;
using System.Collections.Generic;
using ISL.Providers.PDS.FakeFHIR.Mappers;
using ISL.Providers.PDS.FakeFHIR.Models;
using System.Linq;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        private readonly IPdsService pdsService;
        private readonly FakeFHIRProviderConfigurations fakeFHIRProviderConfiguration;
        private readonly ICompareLogic compareLogic;

        public PdsServiceTests()
        {
            this.fakeFHIRProviderConfiguration = GetRandomConfigurations();
            this.compareLogic = new CompareLogic();
            this.pdsService = new PdsService(this.fakeFHIRProviderConfiguration);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GenerateRandom10DigitNumber()
        {
            Random random = new Random();
            var randomNumber = random.Next(1000000000, 2000000000).ToString();

            return randomNumber;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static FakeFHIRProviderConfigurations GetRandomConfigurations() =>
            CreateConfigurationsFiller().Create();

        private static Filler<FakeFHIRProviderConfigurations> CreateConfigurationsFiller()
        {
            var filler = new Filler<FakeFHIRProviderConfigurations>();
            filler.Setup();

            return filler;
        }

        private static IQueryable<PdsPatientDetails> CreateRandomPdsPatientDetails(string inputSearchString)
        {
            return CreatePdsPatientDetailsFiller(inputSearchString)
                .Create(count: GetRandomNumber())
                    .AsQueryable();
        }

        private static PdsPatientDetails CreateRandomPdsPatientDetailsWithMatchingNhsNumber(string nhsNumber)
        {
            return CreatePdsPatientDetailsWithNhsNumberFiller(nhsNumber)
                .Create();
        }

        private static Filler<PdsPatientDetails> CreatePdsPatientDetailsFiller(string givenName)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<PdsPatientDetails>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(pdsPatientDetails => pdsPatientDetails.GivenNames).Use(new List<string> { givenName });

            return filler;
        }

        private static Filler<PdsPatientDetails> CreatePdsPatientDetailsWithNhsNumberFiller(string nhsNumber)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            var filler = new Filler<PdsPatientDetails>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset)
                .OnProperty(pdsPatientDetails => pdsPatientDetails.NhsNumber).Use(nhsNumber);

            return filler;
        }

        private static List<PdsPatientDetails> GetFilteredPdsPatientDetails(
            List<PdsPatientDetails> patientDetails, 
            string givenName)
        {
            return patientDetails.Where(patientDetails => patientDetails.GivenNames.Contains(givenName)).ToList();
        }

        private static PatientBundle CreatePatientBundleFromPatientDetails(List<PdsPatientDetails> patientDetailsList)
        {
            List<Patient> patients = new List<Patient>();

            foreach (PdsPatientDetails patientDetails in patientDetailsList)
            {
                Patient patient = PatientMapper.FromPdsPatientDetails(patientDetails);
                patients.Add(patient);
            }

            Bundle bundle = BundleMapper.FromListOfPatients(patients);
            PatientBundle patientBundle = PatientBundleMapper.FromBundle(bundle);

            return patientBundle;
        }

        private static Patient CreateMatchingPatient(PdsPatientDetails pdsPatientDetails)
        {
            Patient patient = PatientMapper.FromPdsPatientDetails(pdsPatientDetails);

            return patient;
        }
    }
}
