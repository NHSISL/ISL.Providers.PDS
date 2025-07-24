// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using Hl7.Fhir.Model;
using FluentAssertions;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // Given
            string randomString = GetRandomString();
            string inputSurname = randomString.DeepClone();

            Bundle outputBundle = CreateRandomBundle(inputSurname);
            PatientBundle expectedPatientBundle = CreateRandomPatientBundle(outputBundle);

            this.pdsFHIRBrokerMock.Setup(broker =>
                broker.GetNhsNumberAsync(
                    null,
                    inputSurname,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null))
                .ReturnsAsync(outputBundle);

            // When
            PatientBundle actualPatientBundle = await pdsService
                .PatientLookupByDetailsAsync(
                    null,
                    inputSurname,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            // Then
            actualPatientBundle.Should().BeEquivalentTo(expectedPatientBundle);

            this.pdsFHIRBrokerMock.Verify(broker =>
                broker.GetNhsNumberAsync(
                    null,
                    inputSurname,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null),
                        Times.Once);

            this.pdsFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
