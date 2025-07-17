// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Tynamix.ObjectFiller;

namespace ISL.Providers.PDS.Abstractions.Tests.Acceptance
{
    public partial class PdsAbstractionProviderTests
    {
        private readonly Mock<IPdsProvider> pdsProviderMock;
        private readonly IPdsAbstractionProvider pdsAbstractionProvider;
        private readonly IConfiguration configuration;

        public PdsAbstractionProviderTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();
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

        private static PdsResponse CreateRandomPdsResponse() =>
            CreateRandomPdsResponseFiller(dateTimeOffset: GetRandomDateTimeOffset()).Create();

        private static Filler<PdsResponse> CreateRandomPdsResponseFiller(DateTimeOffset dateTimeOffset)
        {
            var filler = new Filler<PdsResponse>();
            filler.Setup()
                .OnType<DateTimeOffset>().Use(dateTimeOffset);

            return filler;
        }
    }
}
