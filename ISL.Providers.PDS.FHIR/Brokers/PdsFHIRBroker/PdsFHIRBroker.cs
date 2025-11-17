// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using Microsoft.IdentityModel.Tokens;
using RESTFulSense.Clients;

namespace ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker
{
    internal class PdsFHIRBroker : IPdsFHIRBroker
    {
        private readonly PdsFHIRConfigurations pdsFHIRConfiguration;
        private readonly SemaphoreSlim tokenGate = new(1, 1);
        private IRESTFulApiFactoryClient? apiClient = null;
        private string accessToken = string.Empty;
        private DateTimeOffset tokenExpiry = DateTimeOffset.MinValue;
        private bool disposed;

        public PdsFHIRBroker(PdsFHIRConfigurations pdsFHIRConfiguration)
        {
            this.pdsFHIRConfiguration = pdsFHIRConfiguration;
            apiClient = null;
        }

        public async ValueTask<Patient> GetNhsNumberAsync(string path)
        {
            await EnsureAccessTokenAsync();
            string requestUri = $"{pdsFHIRConfiguration.ApiUrl}{path}";
            string jsonResponse = await GetAsync<string>(requestUri);
            var parser = new FhirJsonParser();
            Patient patient = parser.Parse<Patient>(jsonResponse);

            return patient;
        }

        public async ValueTask<Bundle> GetPdsPatientDetailsAsync(string path)
        {
            string requestUri = $"{pdsFHIRConfiguration.ApiUrl}{path}";
            string jsonResponse = await GetAsync<string>(requestUri);
            var parser = new FhirJsonParser();
            Bundle bundle = parser.Parse<Bundle>(jsonResponse);

            return bundle;
        }

        private async ValueTask<T> GetAsync<T>(string relativeUrl)
        {
            if (apiClient is null || DateTimeOffset.UtcNow >= tokenExpiry)
            {
                await SetupApiClient();
            }

            if (apiClient is null)
            {
                throw new InvalidOperationException("Failed to setup API client");
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)await this.apiClient.GetContentStringAsync(relativeUrl);
            }
            else
            {
                return await this.apiClient.GetContentAsync<T>(relativeUrl);
            }
        }

        private async ValueTask EnsureAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(this.accessToken)
                && DateTimeOffset.UtcNow < this.tokenExpiry)
            {
                return;
            }

            await this.tokenGate.WaitAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                if (string.IsNullOrEmpty(this.accessToken)
                    || DateTimeOffset.UtcNow >= this.tokenExpiry)
                {
                    await SetupApiClient();
                }
            }
            finally
            {
                this.tokenGate.Release();
            }
        }

        private async ValueTask GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var assertion = CreateClientAssertion();

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_assertion_type",
                        "urn:ietf:params:oauth:client-assertion-type:jwt-bearer"),
                    new KeyValuePair<string, string>("client_assertion", assertion)
                });

                var response = await httpClient.PostAsync(
                    pdsFHIRConfiguration.AuthorisationUrl,
                    content,
                    cancellationToken);

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                accessToken = doc.RootElement.GetProperty("access_token").GetString();
                var expiresIn = int.Parse(doc.RootElement.GetProperty("expires_in").GetString());
                tokenExpiry = DateTimeOffset.UtcNow.AddSeconds(expiresIn - 30);
            }
        }

        private RsaSecurityKey LoadPrivateKey()
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(pdsFHIRConfiguration.JwtSigningKey.ToCharArray());
            return new RsaSecurityKey(rsa) { KeyId = pdsFHIRConfiguration.KeyId };
        }

        private string CreateClientAssertion()
        {
            var securityKey = LoadPrivateKey();
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);
            var header = new JwtHeader(signingCredentials);
            var now = DateTimeOffset.UtcNow;
            var expiry = now.AddMinutes(5);

            var payload = new JwtPayload
            {
                { "iss", pdsFHIRConfiguration.ApiKey },
                { "sub", pdsFHIRConfiguration.ApiKey },
                { "aud", pdsFHIRConfiguration.AuthorisationUrl },
                { "jti", Guid.NewGuid().ToString() },
                { "exp", expiry.ToUnixTimeSeconds() },
                { "iat", now.ToUnixTimeSeconds() }
            };

            var token = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }

        private async ValueTask SetupApiClient()
        {
            await GetAccessTokenAsync();

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/fhir+json");
            httpClient.DefaultRequestHeaders.Add("X-Request-ID", pdsFHIRConfiguration.RequestId);

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    scheme: "Bearer",
                    parameter: this.accessToken ?? "");

            this.apiClient = new RESTFulApiFactoryClient(httpClient);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            tokenGate.Dispose();
        }
    }
}
