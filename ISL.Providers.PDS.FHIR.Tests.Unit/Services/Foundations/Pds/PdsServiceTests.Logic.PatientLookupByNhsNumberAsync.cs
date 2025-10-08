// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // Given
            string randomIdentifier = GenerateValidNhsNumber();
            string inputNhsNumber = randomIdentifier.DeepClone();
            string inputPath = GetPathFromRandomStringForNhsSearch(inputNhsNumber);

            Patient outputPatient = CreateRandomPatientWithNhsNumber(inputNhsNumber);
            Patient expectedPatient = outputPatient.DeepClone();

            this.pdsFHIRBrokerMock.Setup(broker =>
                broker.GetNhsNumberAsync(inputPath))
                .ReturnsAsync(outputPatient);

            // When
            Patient actualPatient = await pdsService
                .PatientLookupByNhsNumberAsync(inputNhsNumber);

            // Then
            actualPatient.Should().BeEquivalentTo(expectedPatient);

            this.pdsFHIRBrokerMock.Verify(broker =>
                broker.GetNhsNumberAsync(inputPath),
                        Times.Once);

            this.pdsFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
