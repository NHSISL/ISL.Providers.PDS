﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using ISL.Providers.PDS.FakeFHIR.Services.Foundations;
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
                    message: "Failed PDS service error occurred, please contact support.",
                    innerException: serviceException,
                    data: serviceException.Data);

            var expectedPdsServiceException =
                new PdsServiceException(
                    message: "PDS service error occurred, please contact support.",
                    innerException: failedServicePdsException);

            var pdsServiceMock = new Mock<PdsService>(this.fakeFHIRProviderConfiguration)
            {
                CallBase = true
            };

            pdsServiceMock.Setup(service =>
                service.GetFakePatientDetails())
                    .Throws(serviceException);

            // when
            ValueTask<Patient> lookupByDetailsTask =
                pdsServiceMock.Object.PatientLookupByNhsNumberAsync(someIdentifierString);

            PdsServiceException actualPdsServiceException =
                await Assert.ThrowsAsync<PdsServiceException>(
                    testCode: lookupByDetailsTask.AsTask);

            // then
            actualPdsServiceException.Should().BeEquivalentTo(
                expectedPdsServiceException);
        }
    }
}
