// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using FluentAssertions;
using Task = System.Threading.Tasks.Task;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using System.Threading.Tasks;
using Moq;
using System;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnPatientLookupByNhsNumberAndLogItAsync()
        {
            // given
            var serviceException = new Exception();
            string someIdentifierString = GenerateRandom10DigitNumber();
            string inputPath = GetPathFromRandomStringForNhsSearch(someIdentifierString);

            var failedServicePdsException =
                new FailedPdsServiceException(
                    message: "Failed PDS service error occurred, please contact support.",
                    innerException: serviceException,
                    data: serviceException.Data);

            var expectedPdsServiceException =
                new PdsServiceException(
                    message: "PDS service error occurred, please contact support.",
                    innerException: failedServicePdsException);


            this.pdsFHIRBrokerMock.Setup(broker =>
                broker.GetNhsNumberAsync(inputPath))
                    .Throws(serviceException);

            // when
            ValueTask<Patient> lookupByDetailsTask =
                this.pdsService.PatientLookupByNhsNumberAsync(someIdentifierString);

            PdsServiceException actualPdsServiceException =
                await Assert.ThrowsAsync<PdsServiceException>(
                    testCode: lookupByDetailsTask.AsTask);

            // then
            actualPdsServiceException.Should().BeEquivalentTo(
                expectedPdsServiceException);

            this.pdsFHIRBrokerMock.Verify(broker =>
                broker.GetNhsNumberAsync(inputPath),
                    Times.Once());

            this.pdsFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
