using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR.Responses;
using RESTFulSense.Clients;
using System;
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
        public async ValueTask<string> GetNhsNumberAsync(string surname, string postcode, DateTimeOffset dateOfBirth)
        {
            string formattedSurname = surname.ToLower();
            string formattedDateOfBirth = dateOfBirth.ToString("yyyy-MM-dd");

            string route = $"Patient?family={formattedSurname}"
                + $"&address-postalcode={postcode}"
                + $"&birthdate=eq{formattedDateOfBirth}";

            string path = pdsFHIRConfiguration.ApiUrl.EndsWith("/")
                ? route
                : $"/{route}";

            var pdsFHIRResponse =
                await apiClient.GetContentAsync<PdsFHIRPatientSearchResponse>(path);

            string nhsNumber = "0000000000";
            PdsFHIREntry pdsFHIREntry = pdsFHIRResponse.Entries.FirstOrDefault();

            if (pdsFHIREntry != null)
            {
                nhsNumber = pdsFHIREntry.Resource.NhsNumber;
            }

            return nhsNumber;
        }

        public async ValueTask<PdsPatientDetails> GetPdsPatientDetailsAsync(string nhsNumber)
        {
            string route = $"Patient?{nhsNumber}";

            string path = pdsFHIRConfiguration.ApiUrl.EndsWith("/")
                ? route
                : $"/{route}";

            var pdsFHIRResponse =
                await apiClient.GetContentAsync<PdsFHIRPatientRetrieveResponse>(path);

            PdsPatientDetails pdsPatientDetails = new PdsPatientDetails();

            return pdsPatientDetails;
        }

        private HttpClient SetupHttpClient()
        {
            var httpClient = new HttpClient()
            {
                BaseAddress =
                    new Uri(uriString: pdsFHIRConfiguration.ApiUrl),
            };

            httpClient.DefaultRequestHeaders.Add("X-API-KEY", pdsFHIRConfiguration.ApiKey);

            return httpClient;
        }

        private IRESTFulApiFactoryClient SetupApiClient() =>
            new RESTFulApiFactoryClient(httpClient);
    }
}
