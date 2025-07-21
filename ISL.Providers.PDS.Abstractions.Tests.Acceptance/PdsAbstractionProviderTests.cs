// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using Moq;
using System;
using System.Collections.Generic;
using Tynamix.ObjectFiller;

namespace ISL.Providers.PDS.Abstractions.Tests.Acceptance
{
    public partial class PdsAbstractionProviderTests
    {
        private readonly Mock<IPdsProvider> pdsProviderMock;
        private readonly IPdsAbstractionProvider pdsAbstractionProvider;

        public PdsAbstractionProviderTests()
        {
            pdsProviderMock = new Mock<IPdsProvider>();

            this.pdsAbstractionProvider =
                new PdsAbstractionProvider(pdsProviderMock.Object);
        }

        private static string GetRandomString()
        {
            int length = GetRandomNumber();

            return new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static List<Patient> CreateRandomPatients()
        {
            List<Patient> patients = new List<Patient>();
            Patient patient = CreateRandomPatientFiller();
            patients.Add(patient);

            return patients;
        }

        private static Patient CreateRandomPatientFiller()
        {
            var patient = new Patient();
            HumanName humanName = new HumanName
            {
                Family = "Smith",
                Given = ["John"]
            };

            patient.Name = new List<HumanName> { humanName };
            patient.Gender = AdministrativeGender.Male;
            patient.BirthDate = "1980-05-10";

            Address address = new Address
            {
                City = "Leeds",
                Line = ["123 Example Street"],
                PostalCode = "ABC 123",
                Country = "United Kingdom"
            };

            patient.Address = new List<Address> { address };

            return patient;
        }

        private static List<Bundle.LinkComponent> CreateRandomLinkComponents()
        {
            List<Bundle.LinkComponent> linkComponents = new List<Bundle.LinkComponent>();

            Bundle.LinkComponent linkComponent = new Bundle.LinkComponent
            {
                ElementId = GetRandomString(),
                Relation = GetRandomString(),
                Url = GetRandomString()
            };

            linkComponents.Add(linkComponent);

            return linkComponents;
        }

        private static PatientBundle CreateRandomPatientBundle() =>
            CreateRandomPatientBundleFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<PatientBundle> CreateRandomPatientBundleFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<PatientBundle>();
            filler.Setup()
                
                .OnProperty(patientBundle => patientBundle.Patients)
                    .Use(CreateRandomPatients())

                .OnProperty(patientBundle => patientBundle.Links)
                    .Use(CreateRandomLinkComponents());

            return filler;
        }
    }
}
