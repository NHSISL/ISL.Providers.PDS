// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker;
using ISL.Providers.PDS.FHIR.Mappers;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using ISL.Providers.PDS.FHIR.Services.Foundations.Pds;
using Moq;
using Tynamix.ObjectFiller;

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

        private static string GenerateValidNhsNumber()
        {
            int total = 10;
            string formattedNhsNumber = string.Empty;

            while (total == 10)
            {
                var randomNumber = new LongRange(100000000, 999999999);
                formattedNhsNumber = randomNumber.GetValue().ToString();
                int[] multiplers = new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int currentNumber;
                int currentSum = 0;
                int currentMultipler;
                string currentString;
                int remainder;

                for (int i = 0; i <= 8; i++)
                {
                    currentString = formattedNhsNumber.Substring(i, 1);

                    currentNumber = Convert.ToInt16(currentString);
                    currentMultipler = multiplers[i];
                    currentSum = currentSum + (currentNumber * currentMultipler);
                }

                remainder = currentSum % 11;
                total = 11 - remainder;

                if (total.Equals(11))
                {
                    total = 0;
                }

                if (total != 10)
                {
                    break;
                }
            }

            string checkNumber = total.ToString();

            return $"{formattedNhsNumber}{checkNumber}";
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
