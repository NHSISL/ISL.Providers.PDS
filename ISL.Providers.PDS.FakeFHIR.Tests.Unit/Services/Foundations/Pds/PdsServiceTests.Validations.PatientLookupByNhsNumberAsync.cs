// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FakeFHIR.Models;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using ISL.Providers.PDS.FakeFHIR.Services.Foundations;
using Moq;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("1234")]
        public async Task ShouldThrowValidationExceptionOnPatientLookupByNhsNumberAsync(string invalidNhsNumber)
        {
            // given
            var invalidArgumentPdsException =
                new InvalidArgumentPdsException("Invalid PDS argument. Please correct the errors and try again.");

            invalidArgumentPdsException.AddData(
                key: "nhsNumber",
                values: "Text must be exactly 10 digits.");

            var expectedPdsValidationException =
                new PdsValidationException(
                    message: "PDS validation error occurred, please fix the errors and try again.",
                    innerException: invalidArgumentPdsException);

            // when
            ValueTask<Patient> patientLookupByNhsNumberAction =
                pdsService.PatientLookupByNhsNumberAsync(invalidNhsNumber);

            PdsValidationException actualException =
                await Assert.ThrowsAsync<PdsValidationException>(patientLookupByNhsNumberAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedPdsValidationException);
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnPatientLookupByNhsNumberIfPatientIsNotFoundAndLogItAsync()
        {
            //given
            string randomInputNhsNumber = GenerateRandom10DigitNumber();
            List<PdsPatientDetails> noPdsPatientDetails = new List<PdsPatientDetails>();

            var notFoundPdsPatientDetailsException =
                new NotFoundPdsPatientDetailsException(
                    $"Couldn't find PDS patient with nhsNumber: {randomInputNhsNumber}.");

            var expectedPdsValidationException =
                new PdsValidationException(
                    message: "PDS validation error occurred, please fix the errors and try again.",
                    innerException: notFoundPdsPatientDetailsException);

            var pdsServiceMock = new Mock<PdsService>(this.fakeFHIRProviderConfiguration)
            {
                CallBase = true
            };

            pdsServiceMock.Setup(service =>
                service.GetFakePatientDetails())
                    .Returns(noPdsPatientDetails);

            //when
            ValueTask<Patient> patientLookupByNhsNumberTask =
                pdsServiceMock.Object.PatientLookupByNhsNumberAsync(randomInputNhsNumber);

            PdsValidationException actualPdsValidationException =
                await Assert.ThrowsAsync<PdsValidationException>(
                    patientLookupByNhsNumberTask.AsTask);

            //then
            actualPdsValidationException.Should().BeEquivalentTo(expectedPdsValidationException);
        }
    }
}
