// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnPatientLookupByDetailsAsync(string invalidString)
        {
            // given
            var invalidArgumentPdsException =
                new InvalidArgumentPdsException("Invalid Pds argument. Please correct the errors and try again.");

            invalidArgumentPdsException.AddData(
                key: "searchParams",
                values: "Text is required");

            var expectedPdsValidationException =
                new PdsValidationException(
                    message: "Pds validation error occurred, please fix the errors and try again.",
                    innerException: invalidArgumentPdsException);

            // when
            ValueTask<PatientBundle> patientLookupByDetailsAction =
                pdsService.PatientLookupByDetailsAsync(invalidString);

            PdsValidationException actualException =
                await Assert.ThrowsAsync<PdsValidationException>(patientLookupByDetailsAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedPdsValidationException);

            this.fakeFHIRBrokerMock.Verify(broker =>
                broker.GetNhsNumberAsync(invalidString),
                    Times.Never);

            this.fakeFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
