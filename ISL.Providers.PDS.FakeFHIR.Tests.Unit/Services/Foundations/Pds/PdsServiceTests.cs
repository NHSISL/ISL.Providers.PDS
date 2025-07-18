// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR;
using ISL.Providers.PDS.FakeFHIR.Brokers.Identifiers;
using KellermanSoftware.CompareNetObjects;
using Moq;
using System;
using Tynamix.ObjectFiller;
using ISL.Providers.PDS.FakeFHIR.Services.Foundations;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        private readonly Mock<IFakeFHIRBroker> fakeFHIRBrokerMock;
        private readonly Mock<IIdentifierBroker> identifierBrokerMock;
        private readonly IPdsService pdsService;
        private readonly ICompareLogic compareLogic;

        public PdsServiceTests()
        {
            this.fakeFHIRBrokerMock = new Mock<IFakeFHIRBroker>();
            this.identifierBrokerMock = new Mock<IIdentifierBroker>();
            this.compareLogic = new CompareLogic();
            this.pdsService = new PdsService(
                fakeFHIRBroker: fakeFHIRBrokerMock.Object,
                identifierBroker: this.identifierBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomString(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static string GenerateRandom10DigitNumber()
        {
            Random random = new Random();
            var randomNumber = random.Next(1000000000, 2000000000).ToString();

            return randomNumber;
        }

        private static PdsResponse CreateRandomPdsResponse(
            Guid responseId,
            DateTimeOffset dateOfBirth,
            string nhsNumber = "12345",
            string surname = "Smith",
            string postcode = "LS1 6AE")
        {
            var response = new PdsResponse
            {
                ResponseId = responseId,
                FirstName = "Jane",
                Surname = surname,
                Address = "1 Trevelyan Square, Boar Lane, City Centre,Leeds, West Yorkshire",
                EmailAddress = "jane.smith@example.com",
                PhoneNumber = "01632960587",
                Postcode = postcode,
                DateOfBirth = dateOfBirth,
                NhsNumber = nhsNumber
            };

            return response;
        }

        private static PdsPatientDetails CreateRandomPdsPatientDetails() =>
            CreatePdsPatientDetailsFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<PdsPatientDetails> CreatePdsPatientDetailsFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<PdsPatientDetails>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }

        private static PdsResponse CreateRandomPdsResponse(
            Guid responseId,
            string nhsNumber,
            PdsPatientDetails pdsPatientDetails)
        {
            var response = new PdsResponse
            {
                ResponseId = responseId,
                FirstName = pdsPatientDetails.FirstName,
                Surname = pdsPatientDetails.Surname,
                Address = pdsPatientDetails.Address,
                EmailAddress = pdsPatientDetails.EmailAddress,
                PhoneNumber = pdsPatientDetails.PhoneNumber,
                Postcode = pdsPatientDetails.Postcode,
                DateOfBirth = pdsPatientDetails.DateOfBirth,
                NhsNumber = nhsNumber
            };

            return response;
        }
    }
}
