// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using Microsoft.IdentityModel.Tokens;

namespace ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker
{
    internal class PdsFHIRBroker : IPdsFHIRBroker
    {
        private readonly PdsFHIRConfigurations pdsFHIRConfiguration;
        private readonly HttpClient httpClient;
        private string accessToken = string.Empty;
        private DateTimeOffset tokenExpiry = DateTimeOffset.MinValue;

        public PdsFHIRBroker(PdsFHIRConfigurations pdsFHIRConfiguration)
        {
            this.pdsFHIRConfiguration = pdsFHIRConfiguration;
            httpClient = SetupHttpClient();
        }

        public async ValueTask<Patient> GetNhsNumberAsync(string path)
        {
            string requestUri = $"{pdsFHIRConfiguration.ApiUrl}{path}";
            string accessToken = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            request.Headers.Add("X-Request-ID", pdsFHIRConfiguration.RequestId);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var parser = new FhirJsonParser();
            Patient patient = parser.Parse<Patient>(jsonResponse);

            return patient;
        }

        public async ValueTask<Bundle> GetPdsPatientDetailsAsync(string path)
        {
            string requestUri = $"{pdsFHIRConfiguration.ApiUrl}{path}";
            string accessToken = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            request.Headers.Add("X-Request-ID", pdsFHIRConfiguration.RequestId);
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string jsonResponse = await response.Content.ReadAsStringAsync();
            var parser = new FhirJsonParser();
            Bundle bundle = parser.Parse<Bundle>(jsonResponse);

            return bundle;
        }

        private HttpClient SetupHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/fhir+json");

            return httpClient;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(accessToken) && DateTimeOffset.UtcNow < tokenExpiry)
            {
                return accessToken;
            }

            var assertion = CreateClientAssertion();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),

                new KeyValuePair<string, string>("client_assertion_type",
                    "urn:ietf:params:oauth:client-assertion-type:jwt-bearer"),

                new KeyValuePair<string, string>("client_assertion", assertion)
            });

            var response = await httpClient.PostAsync(pdsFHIRConfiguration.AuthorisationUrl, content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);

            accessToken = doc.RootElement.GetProperty("access_token").GetString();
            var expiresIn = int.Parse(doc.RootElement.GetProperty("expires_in").GetString());
            tokenExpiry = DateTimeOffset.UtcNow.AddSeconds(expiresIn - 30);

            return accessToken;
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
    }
}
