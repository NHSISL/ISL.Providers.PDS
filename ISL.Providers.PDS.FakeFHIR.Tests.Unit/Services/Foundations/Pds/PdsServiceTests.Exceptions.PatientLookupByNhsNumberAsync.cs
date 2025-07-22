// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnPatientLookupByNhsNumberAndLogItAsync()
        {
            // given
            var serviceException = new Exception();
            string someIdentifierString = GenerateRandom10DigitNumber();

            var failedServicePdsException =
                new FailedServicePdsException(
                    message: "Failed pds service error occurred, please contact support.",
                    innerException: serviceException,
                    data: serviceException.Data);

            var expectedPdsServiceException =
                new PdsServiceException(
                    message: "Pds service error occurred, please contact support.",
                    innerException: failedServicePdsException);

            fakeFHIRBrokerMock.Setup(broker =>
                broker.GetPdsPatientDetailsAsync(someIdentifierString))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Patient> lookupByDetailsTask =
                pdsService.PatientLookupByNhsNumberAsync(someIdentifierString);

            PdsServiceException actualPdsServiceException =
                await Assert.ThrowsAsync<PdsServiceException>(
                    testCode: lookupByDetailsTask.AsTask);

            // then
            actualPdsServiceException.Should().BeEquivalentTo(
                expectedPdsServiceException);

            fakeFHIRBrokerMock.Verify(service =>
                service.GetPdsPatientDetailsAsync(someIdentifierString),
                    Times.Once);

            this.fakeFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
