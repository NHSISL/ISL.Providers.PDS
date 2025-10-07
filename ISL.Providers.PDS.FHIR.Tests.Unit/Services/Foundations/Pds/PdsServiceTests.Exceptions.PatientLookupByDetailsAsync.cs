// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using ISL.Providers.PDS.FHIR.Services.Foundations.Pds;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnPatientLookupByDetailsAndLogItAsync()
        {
            // given
            var serviceException = new Exception();
            string someString = GetRandomString();

            var failedServicePdsException =
                new FailedPdsServiceException(
                    message: "Failed PDS service error occurred, please contact support.",
                    innerException: serviceException,
                    data: serviceException.Data);

            var expectedPdsServiceException =
                new PdsServiceException(
                    message: "PDS service error occurred, please contact support.",
                    innerException: failedServicePdsException);

            var pdsServiceMock = new Mock<PdsService>(
                this.pdsFHIRBrokerMock.Object,
                this.pdsFHIRConfigurations)
            {
                CallBase = true
            };

            pdsServiceMock.Setup(service =>
                service.GetPatientLookupByDetailsPath(
                    null,
                    someString,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null))
                .Throws(serviceException);

            // when
            ValueTask<PatientBundle> lookupByDetailsTask =
                pdsServiceMock.Object.PatientLookupByDetailsAsync(
                    null,
                    someString,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            PdsServiceException actualPdsServiceException =
                await Assert.ThrowsAsync<PdsServiceException>(
                    testCode: lookupByDetailsTask.AsTask);

            // then
            actualPdsServiceException.Should().BeEquivalentTo(
                expectedPdsServiceException);

            pdsServiceMock.Verify(service =>
                service.GetPatientLookupByDetailsPath(
                    null,
                    someString,
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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnPatientLookupByDetailsAndLogItAsyncWhenPatientNotFound()
        {
            // given
            string someString = GetRandomString();

            var noMatchingPatientsException =
                new NoMatchingPatientsException(
                    message: "No patients found matching the search criteria.");

            var expectedPdsValidationException =
                new PdsValidationException(
                    message: "PDS validation error occurred, please fix the errors and try again.",
                    innerException: noMatchingPatientsException);

            var pdsServiceMock = new Mock<PdsService>(
                this.pdsFHIRBrokerMock.Object,
                this.pdsFHIRConfigurations)
            {
                CallBase = true
            };

            pdsServiceMock.Setup(service =>
                service.GetPatientLookupByDetailsPath(
                    null,
                    someString,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null))
                .Throws(noMatchingPatientsException);

            // when
            ValueTask<PatientBundle> lookupByDetailsTask =
                pdsServiceMock.Object.PatientLookupByDetailsAsync(
                    null,
                    someString,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            PdsValidationException actualPdsValidationException =
                await Assert.ThrowsAsync<PdsValidationException>(
                    testCode: lookupByDetailsTask.AsTask);

            // then
            actualPdsValidationException.Should().BeEquivalentTo(
                expectedPdsValidationException);

            pdsServiceMock.Verify(service =>
                service.GetPatientLookupByDetailsPath(
                    null,
                    someString,
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
