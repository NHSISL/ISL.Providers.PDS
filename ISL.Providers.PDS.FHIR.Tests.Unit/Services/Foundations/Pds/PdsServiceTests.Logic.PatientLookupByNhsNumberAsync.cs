// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using FluentAssertions;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // Given
            string randomIdentifier = GenerateRandom10DigitNumber();
            string inputNhsNumber = randomIdentifier.DeepClone();

            Patient outputPatient = CreateRandomPatientWithNhsNumber(inputNhsNumber);
            Patient expectedPatient = outputPatient.DeepClone();

            this.pdsFHIRBrokerMock.Setup(broker =>
                broker.GetPdsPatientDetailsAsync(inputNhsNumber))
                .ReturnsAsync(outputPatient);

            // When
            Patient actualPatient = await pdsService
                .PatientLookupByNhsNumberAsync(inputNhsNumber);

            // Then
            actualPatient.Should().BeEquivalentTo(expectedPatient);

            this.pdsFHIRBrokerMock.Verify(broker =>
                broker.GetPdsPatientDetailsAsync(inputNhsNumber),
                        Times.Once);

            this.pdsFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
