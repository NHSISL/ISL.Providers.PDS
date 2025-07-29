// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using RESTFulSense.Clients;

namespace ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker
{
    internal class PdsFHIRBroker : IPdsFHIRBroker
    {
        private readonly PdsFHIRConfigurations pdsFHIRConfiguration;
        private readonly IRESTFulApiFactoryClient apiClient;
        private readonly HttpClient httpClient;

        public PdsFHIRBroker(PdsFHIRConfigurations pdsFHIRConfiguration)
        {
            this.pdsFHIRConfiguration = pdsFHIRConfiguration;
            httpClient = SetupHttpClient();
            apiClient = SetupApiClient();
        }

        public async ValueTask<Bundle> GetNhsNumberAsync(string path)
        {
            var bundle = await apiClient.GetContentAsync<Bundle>(path);

            return bundle;
        }

        public async ValueTask<Patient> GetPdsPatientDetailsAsync(string path)
        {
            var patient = await apiClient.GetContentAsync<Patient>(path);

            return patient;
        }

        private HttpClient SetupHttpClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(uriString: pdsFHIRConfiguration.ApiUrl),
            };

            httpClient.DefaultRequestHeaders.Add("X-Request-ID", pdsFHIRConfiguration.RequestId);

            return httpClient;
        }

        private IRESTFulApiFactoryClient SetupApiClient() =>
            new RESTFulApiFactoryClient(httpClient);
    }
}
