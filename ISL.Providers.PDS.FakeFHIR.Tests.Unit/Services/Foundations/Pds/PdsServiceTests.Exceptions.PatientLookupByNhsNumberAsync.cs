// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;

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

            identifierBrokerMock.Setup(broker =>
                broker.GetIdentifierAsync())
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<PdsResponse> lookupByDetailsTask =
                pdsService.PatientLookupByNhsNumberAsync(someIdentifierString);

            PdsServiceException actualPdsServiceException =
                await Assert.ThrowsAsync<PdsServiceException>(
                    testCode: lookupByDetailsTask.AsTask);

            // then
            actualPdsServiceException.Should().BeEquivalentTo(
                expectedPdsServiceException);

            identifierBrokerMock.Verify(service =>
                service.GetIdentifierAsync(),
                    Times.Once);

            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.fakeFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
