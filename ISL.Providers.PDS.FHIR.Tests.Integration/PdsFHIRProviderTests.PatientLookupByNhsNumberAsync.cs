// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Integration
{
    public partial class PdsFHIRProviderTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            string randomString = "9449304424";
            string inputNhsNumber = randomString.DeepClone();
            Patient patientResponse = CreateRandomPatientWithNhsNumber(inputNhsNumber);
            Patient expectedResponse = patientResponse.DeepClone();

            // when
            Patient actualResponse =
                await this.pdsFHIRProvider.PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
