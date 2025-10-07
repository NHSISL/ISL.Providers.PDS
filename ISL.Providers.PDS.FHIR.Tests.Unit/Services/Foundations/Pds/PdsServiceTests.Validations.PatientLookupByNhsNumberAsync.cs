// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("1234")]
        [InlineData("0000000001")]
        public async Task ShouldThrowValidationExceptionOnPatientLookupByNhsNumberAsync(string invalidNhsNumber)
        {
            // given
            var invalidArgumentPdsException =
                new InvalidPdsArgumentException("Invalid PDS argument. Please correct the errors and try again.");

            invalidArgumentPdsException.AddData(
                key: "nhsNumber",
                values: "Text must be a valid NHS Number.");

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
