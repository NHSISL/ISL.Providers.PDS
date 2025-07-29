// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
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

        public async ValueTask<Patient> GetNhsNumberAsync(string path)
        {
            string jsonResponse = await apiClient.GetContentStringAsync(path);
            var parser = new FhirJsonParser();

            Patient patient = parser.Parse<Patient>(jsonResponse);

            return patient;
        }

        public async ValueTask<Bundle> GetPdsPatientDetailsAsync(string path)
        {
            string jsonResponse = await apiClient.GetContentStringAsync(path);
            var parser = new FhirJsonParser();
            Bundle bundle = parser.Parse<Bundle>(jsonResponse);

            return bundle;
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
