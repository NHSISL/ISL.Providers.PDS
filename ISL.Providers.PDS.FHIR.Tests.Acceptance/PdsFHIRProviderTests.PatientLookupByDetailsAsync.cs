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
        [Fact]
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
                        .WithBodyAsJson(bundleResponse));


            // when
            PatientBundle actualResponse =
                await this.pdsFHIRProvider.PatientLookupByDetailsAsync(
                    familyName: inputSurname);

            var x = this.wireMockServer.LogEntries;

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
