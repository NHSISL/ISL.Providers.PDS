// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Acceptance
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            string randomIdentifierString = GenerateRandom10DigitNumber();
            string nhsNumber = randomIdentifierString.DeepClone();

            Patient randomPatient = CreateRandomPatient();

            Patient expectedResponse = randomPatient.DeepClone();

            // when
            Patient actualResponse =
                await this.fakeFHIRProvider.PatientLookupByNhsNumberAsync(nhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
