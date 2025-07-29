// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using ISL.Providers.PDS.FHIR.Services.Foundations.Pds;
using Moq;
using System;
using Tynamix.ObjectFiller;
using ISL.Providers.PDS.Abstractions.Models;
using Hl7.Fhir.Model;
using System.Collections.Generic;
using ISL.Providers.PDS.FHIR.Mappers;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        private readonly Mock<IPdsFHIRBroker> pdsFHIRBrokerMock = new Mock<IPdsFHIRBroker>();
        private PdsFHIRConfigurations pdsFHIRConfigurations;
        private readonly IPdsService pdsService;

        public PdsServiceTests()
        {
            this.pdsFHIRBrokerMock = new Mock<IPdsFHIRBroker>();

            this.pdsFHIRConfigurations = new PdsFHIRConfigurations
            {
                ApiKey = GetRandomString(),
                ApiUrl = GetRandomString(),
                PatientLookupPath = GetRandomString(),
                PatientSearchPath = GetRandomString(),
                RequestId = GetRandomString()
            };

            this.pdsService = new PdsService(
                pdsFHIRBroker: pdsFHIRBrokerMock.Object,
                pdsFHIRConfigurations: pdsFHIRConfigurations);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetFamilySearchPathFromRandomString(string randomString) =>
            $"Patient?family={randomString}";

        private string GetPathFromRandomStringForNhsSearch(string randomString) =>
            $"{pdsFHIRConfigurations.PatientLookupPath}/{randomString}";

        private static string GenerateRandom10DigitNumber()
        {
            Random random = new Random();
            var randomNumber = random.Next(1000000000, 2000000000).ToString();

            return randomNumber;
        }

        private static Patient CreateRandomPatient(string surname)
        {
            var patient = new Patient();

            var nameFiller = new Filler<HumanName>();
            nameFiller.Setup()
                .OnProperty(n => n.Family).Use(surname)
                .OnProperty(n => n.Children).IgnoreIt()
                .OnProperty(n => n.Extension).IgnoreIt()
                .OnProperty(n => n.FamilyElement).IgnoreIt()
                .OnProperty(n => n.GivenElement).IgnoreIt()
                .OnProperty(n => n.NamedChildren).IgnoreIt()
                .OnProperty(n => n.Period).IgnoreIt()
                .OnProperty(n => n.PrefixElement).IgnoreIt()
                .OnProperty(n => n.SuffixElement).IgnoreIt()
                .OnProperty(n => n.TextElement).IgnoreIt()
                .OnProperty(n => n.UseElement).IgnoreIt();

            patient.Name = new List<HumanName> { nameFiller.Create() };
            patient.Gender = AdministrativeGender.Male;
            patient.BirthDate = GetRandomString();

            return patient;
        }

        private static Patient CreateRandomPatientWithNhsNumber(string nhsNumber)
        {
            var patient = new Patient();

            var nameFiller = new Filler<HumanName>();
            nameFiller.Setup()
                .OnProperty(n => n.Children).IgnoreIt()
                .OnProperty(n => n.Extension).IgnoreIt()
                .OnProperty(n => n.FamilyElement).IgnoreIt()
                .OnProperty(n => n.GivenElement).IgnoreIt()
                .OnProperty(n => n.NamedChildren).IgnoreIt()
                .OnProperty(n => n.Period).IgnoreIt()
                .OnProperty(n => n.PrefixElement).IgnoreIt()
                .OnProperty(n => n.SuffixElement).IgnoreIt()
                .OnProperty(n => n.TextElement).IgnoreIt()
                .OnProperty(n => n.UseElement).IgnoreIt();

            patient.Id = nhsNumber;
            patient.Name = new List<HumanName> { nameFiller.Create() };
            patient.Gender = AdministrativeGender.Male;
            patient.BirthDate = GetRandomString();

            return patient;
        }

        private Bundle CreateRandomBundle(string surname)
        {
            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Searchset,
                Total = 1,
                Timestamp = DateTimeOffset.UtcNow
            };

            Patient patient = CreateRandomPatient(surname);

            bundle.Entry = new List<Bundle.EntryComponent>{
                new Bundle.EntryComponent
                {
                    FullUrl = $"https://api.service.nhs.uk/personal-demographics/FHIR/R4/Patient/{patient.Id}",
                    Search = new Bundle.SearchComponent { Score = 1 },
                    Resource = patient
                }
            };
          

            return bundle;
        }

        private PatientBundle CreateRandomPatientBundle(Bundle bundle)
        {
            return PatientBundleMapper.FromBundle(bundle);
        }
    }
}
