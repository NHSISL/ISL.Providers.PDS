// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Integration
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
            
            // when
            PatientBundle actualResponse =
                await this.pdsFHIRProvider.PatientLookupByDetailsAsync(
                    familyName: inputSurname);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
