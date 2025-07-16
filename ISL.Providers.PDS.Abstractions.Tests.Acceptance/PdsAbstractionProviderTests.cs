// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using Microsoft.Extensions.Configuration;
using Moq;
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

        private static PdsRequest CreateRandomPdsRequest() =>
            CreateRandomPdsRequestFiller().Create();

        private static Filler<PdsRequest> CreateRandomPdsRequestFiller()
        {
            var filler = new Filler<PdsRequest>();
            filler.Setup();

            return filler;
        }
    }
}
