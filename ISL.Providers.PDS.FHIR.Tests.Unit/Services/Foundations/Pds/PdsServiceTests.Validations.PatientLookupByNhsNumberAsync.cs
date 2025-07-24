﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using FluentAssertions;
using Task = System.Threading.Tasks.Task;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
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
                new InvalidPdsArgumentException("Invalid PDS argument. Please correct the errors and try again.");

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
    }
}
