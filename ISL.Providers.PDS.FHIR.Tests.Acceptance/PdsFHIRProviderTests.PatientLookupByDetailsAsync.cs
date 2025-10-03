// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Acceptance
{
    public partial class PdsFHIRProviderTests
    {
        [Fact(Skip = "To fix when jwt creation moved to client")]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputSurname = randomString.DeepClone();
            Bundle bundleResponse = CreateRandomBundle(inputSurname);
            PatientBundle randomResponse = CreateRandomPatientBundle(bundleResponse);
            PatientBundle response = randomResponse;
            PatientBundle expectedResponse = response.DeepClone();
            var path = $"/Patient";
            var fhirSerializer = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string bundleResponseString = fhirSerializer.SerializeToString(bundleResponse);

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .WithParam("family", inputSurname)
                        .UsingGet()
                        .WithHeader("X-REQUEST-ID", this.pdsFHIRConfigurations.RequestId))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(bundleResponseString));

            // when
            PatientBundle actualResponse =
                await this.pdsFHIRProvider.PatientLookupByDetailsAsync(
                    familyName: inputSurname);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
