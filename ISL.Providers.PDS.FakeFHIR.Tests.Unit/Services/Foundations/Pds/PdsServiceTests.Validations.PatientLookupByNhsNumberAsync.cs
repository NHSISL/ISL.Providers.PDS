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
        [InlineData("1234")]
        public async Task ShouldThrowValidationExceptionOnPatientLookupByNhsNumberAsync(string invalidPath)
        {
            // given
            var invalidArgumentPdsException =
                new InvalidArgumentPdsException("Invalid Pds argument(s). Please correct the errors and try again.");

            invalidArgumentPdsException.AddData(
                key: "nhsNumber",
                values: "Text must be exactly 10 digits.");

            var expectedPdsValidationException =
                new PdsValidationException(
                    message: "Pds validation error occurred, please fix the errors and try again.",
                    innerException: invalidArgumentPdsException);

            // when
            ValueTask<PdsResponse> patientLookupByNhsNumberAction =
                pdsService.PatientLookupByNhsNumberAsync(invalidPath);

            PdsValidationException actualException =
                await Assert.ThrowsAsync<PdsValidationException>(patientLookupByNhsNumberAction.AsTask);

            // then
            actualException.Should().BeEquivalentTo(expectedPdsValidationException);

            this.identifierBrokerMock.Verify(broker =>
                broker.GetIdentifierAsync(),
                    Times.Never);

            this.identifierBrokerMock.VerifyNoOtherCalls();
            this.fakeFHIRBrokerMock.VerifyNoOtherCalls();
        }
    }
}
