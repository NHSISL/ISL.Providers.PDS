// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Acceptance
{
    public partial class PdsFHIRProviderTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            string randomString = GenerateRandom10DigitNumber();
            string inputNhsNumber = randomString.DeepClone();
            
            Patient patientResponse = CreateRandomPatientWithNhsNumber(inputNhsNumber);
            Patient expectedResponse = patientResponse.DeepClone();
            var path = $"/Patient/{inputNhsNumber}";
            var fhirSerializer = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string patientResponseString = fhirSerializer.SerializeToString(patientResponse);

            this.wireMockServer
                .Given(
                    Request.Create()
                        .WithPath(path)
                        .UsingGet()
                        .WithHeader("X-REQUEST-ID", this.pdsFHIRConfigurations.RequestId))
                .RespondWith(
                    Response.Create()
                        .WithSuccess()
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(patientResponseString));

            // when
            Patient actualResponse =
                await this.pdsFHIRProvider.PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
