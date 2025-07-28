// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using RESTFulSense.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
        public async ValueTask<Bundle> GetNhsNumberAsync(
            string givenName = null,
            string familyName = null,
            string gender = null,
            string postcode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null)
        {
            var queryParams = new List<string>();

            string queryString = string.Join("&", queryParams);

            if (!string.IsNullOrEmpty(givenName)) {
                var splitGivenNames = string.Join("&",
                    givenName
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(name => $"given={name}"));

                queryParams.Add(splitGivenNames);
            }

            if (!string.IsNullOrEmpty(familyName))
            {
                queryParams.Add($"family={familyName}");
            }

            if (!string.IsNullOrEmpty(gender))
            {
                queryParams.Add($"gender={gender}");
            }

            if (!string.IsNullOrEmpty(postcode))
            {
                queryParams.Add($"address-postalcode={postcode}");
            }

            if (!string.IsNullOrEmpty(dateOfBirth))
            {
                queryParams.Add($"birthdate=eq{dateOfBirth}");
            }

            if (!string.IsNullOrEmpty(dateOfDeath))
            {
                queryParams.Add($"death-date=eq{dateOfDeath}");
            }

            if (!string.IsNullOrEmpty(registeredGpPractice))
            {
                queryParams.Add($"general-practitioner={registeredGpPractice}");
            }

            if (!string.IsNullOrEmpty(email))
            {
                queryParams.Add($"email={email}");
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                queryParams.Add($"phone={phoneNumber}");
            }

            string route = $"Patient?{queryString}";

            string path = pdsFHIRConfiguration.ApiUrl.EndsWith("/")
                ? route
                : $"/{route}";

            var bundle =
                await apiClient.GetContentAsync<Bundle>(path);

            return bundle;
        }

        public async ValueTask<Patient> GetPdsPatientDetailsAsync(string nhsNumber)
        {
            string route = $"Patient?{nhsNumber}";

            string path = pdsFHIRConfiguration.ApiUrl.EndsWith("/")
                ? route
                : $"/{route}";

            var patient =
                await apiClient.GetContentAsync<Patient>(path);

            return patient;
        }

        private HttpClient SetupHttpClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress =
                    new Uri(uriString: pdsFHIRConfiguration.ApiUrl),
            };

            // THIS VALUE IS HARD-CODED FOR NOW FOR TESTING BUT WILL NEED TO BE CHANGED
            httpClient.DefaultRequestHeaders.Add("X-Request-ID", "a44681a7-7fb4-41ad-b13a-b481da392232");

            return httpClient;
        }

        private IRESTFulApiFactoryClient SetupApiClient() =>
            new RESTFulApiFactoryClient(httpClient);
    }
}
