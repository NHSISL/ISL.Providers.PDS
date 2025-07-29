// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using Hl7.Fhir.Model;
using FluentAssertions;
using Task = System.Threading.Tasks.Task;
using ISL.Providers.PDS.FHIR.Services.Foundations.Pds;

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
            string outputPath = GetFamilySearchPathFromRandomString(inputSurname);

            Bundle outputBundle = CreateRandomBundle(inputSurname);
            PatientBundle expectedPatientBundle = CreateRandomPatientBundle(outputBundle);

            this.pdsFHIRBrokerMock.Setup(broker =>
                broker.GetPdsPatientDetailsAsync(outputPath))
                .ReturnsAsync(outputBundle);

            var pdsServiceMock = new Mock<PdsService>(
                this.pdsFHIRBrokerMock.Object,
                this.pdsFHIRConfigurations)
                {
                    CallBase = true
                };

            pdsServiceMock.Setup(service =>
                service.GetPatientLookupByDetailsPath(
                    null,
                    inputSurname,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null))
                .Returns(outputPath);

            // When
            PatientBundle actualPatientBundle = await pdsServiceMock.Object
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

            pdsServiceMock.Verify(service =>
                service.GetPatientLookupByDetailsPath(
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

            this.pdsFHIRBrokerMock.Verify(broker =>
                broker.GetPdsPatientDetailsAsync(outputPath),
                        Times.Once);

            this.pdsFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
