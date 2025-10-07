// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnPatientLookupByNhsNumberAndLogItAsync()
        {
            // given
            var serviceException = new Exception();
            string someIdentifierString = GenerateValidNhsNumber();
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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnPatientLookupByNhsNumberAndLogItAsync()
        {
            // given
            string someIdentifierString = GenerateValidNhsNumber();
            string inputPath = GetPathFromRandomStringForNhsSearch(someIdentifierString);
            var serviceException = new Exception("Response status code does not indicate success: 404 (Not Found).");

            var patientNotFoundException =
                new PatientNotFoundException(
                    message: "Patient not found.");

            var expectedPdsValidationException =
                new PdsValidationException(
                    message: "PDS validation error occurred, please fix the errors and try again.",
                    innerException: patientNotFoundException);


            this.pdsFHIRBrokerMock.Setup(broker =>
                broker.GetNhsNumberAsync(inputPath))
                    .Throws(serviceException);

            // when
            ValueTask<Patient> lookupByDetailsTask =
                this.pdsService.PatientLookupByNhsNumberAsync(someIdentifierString);

            PdsValidationException actualPdsValidationException =
                await Assert.ThrowsAsync<PdsValidationException>(
                    testCode: lookupByDetailsTask.AsTask);

            // then
            actualPdsValidationException.Should().BeEquivalentTo(
                expectedPdsValidationException);

            this.pdsFHIRBrokerMock.Verify(broker =>
                broker.GetNhsNumberAsync(inputPath),
                    Times.Once());

            this.pdsFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
