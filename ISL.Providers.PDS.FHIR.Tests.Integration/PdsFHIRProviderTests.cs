// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using ISL.Providers.PDS.FHIR.Providers;
using Microsoft.Extensions.Configuration;

namespace ISL.Providers.PDS.FHIR.Tests.Integration
{
    public partial class PdsFHIRProviderTests
    {
        private readonly IPdsFHIRProvider pdsFHIRProvider;
        private readonly PdsFHIRConfigurations pdsFHIRConfigurations;
        private readonly IConfiguration configuration;

        public PdsFHIRProviderTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();

            this.pdsFHIRConfigurations = configuration
                .GetSection("pdsFHIRConfigurations").Get<PdsFHIRConfigurations>();

            this.pdsFHIRProvider = new PdsFHIRProvider(pdsFHIRConfigurations);
        }

    }
}
