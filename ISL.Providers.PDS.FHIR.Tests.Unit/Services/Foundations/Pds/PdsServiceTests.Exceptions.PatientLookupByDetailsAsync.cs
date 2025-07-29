// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Task = System.Threading.Tasks.Task;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using System.Threading.Tasks;
using Moq;
using System;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FHIR.Services.Foundations.Pds;

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
    }
}
