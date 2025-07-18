// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Providers.FakeFHIR;
using System;
using Tynamix.ObjectFiller;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Acceptance
{
    public partial class PdsServiceTests
    {
        private readonly IFakeFHIRProvider fakeFHIRProvider;

        public PdsServiceTests()
        {
            this.fakeFHIRProvider = new FakeFHIRProvider();
        }

        private static string GenerateRandom10DigitNumber()
        {
            Random random = new Random();
            var randomNumber = random.Next(1000000000, 2000000000).ToString();

            return randomNumber;
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static PdsResponse CreateRandomPdsResponse(
            DateTimeOffset dateOfBirth,
            string nhsNumber = "9000000009",
            string surname = "Smith",
            string postcode = "LS1 6AE")
        {
            var response = new PdsResponse
            {
                ResponseId = Guid.NewGuid(),
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
    }
}
