// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using ISL.Providers.PDS.Abstractions.Tests.Unit.Models.Exceptions;
using Moq;
using Xeptions;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.Abstractions.Tests.Unit
{
    public partial class PdsAbstractionTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionWhenTypeIPdsValidationExceptionOnLookupByNhsNumber()
        {
            // given
            var someException = new Xeption();

            var somePdsValidationException =
                new SomePdsValidationException(
                    message: "Some pds validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            PdsProviderValidationException expectedPdsValidationProviderException =
                new PdsProviderValidationException(
                    message: somePdsValidationException.Message,
                    innerException: somePdsValidationException);

            this.pdsMock.Setup(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()))
                    .ThrowsAsync(somePdsValidationException);

            // when
            ValueTask<Patient> patientLookupTask =
                this.pdsAbstractionProvider
                    .PatientLookupByNhsNumberAsync(It.IsAny<string>());

            PdsProviderValidationException actualPdsValidationProviderException =
                await Assert.ThrowsAsync<PdsProviderValidationException>(testCode: patientLookupTask.AsTask);

            // then
            actualPdsValidationProviderException.Should().BeEquivalentTo(
                expectedPdsValidationProviderException);

            this.pdsMock.Verify(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()),
                    Times.Once);

            this.pdsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionWhenTypeIPdsDependencyValidationExceptionOnLookupByNhsNumber()
        {
            // given
            var someException = new Xeption();

            var somePdsValidationException =
                new SomePdsDependencyValidationException(
                    message: "Some pds dependency validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            PdsProviderValidationException expectedPdsValidationProviderException =
                new PdsProviderValidationException(
                    message: somePdsValidationException.Message,
                    innerException: somePdsValidationException);

            this.pdsMock.Setup(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()))
                    .ThrowsAsync(somePdsValidationException);

            // when
            ValueTask<Patient> patientLookupTask =
                this.pdsAbstractionProvider
                    .PatientLookupByNhsNumberAsync(It.IsAny<string>());

            PdsProviderValidationException actualPdsValidationProviderException =
                await Assert.ThrowsAsync<PdsProviderValidationException>(testCode: patientLookupTask.AsTask);

            // then
            actualPdsValidationProviderException.Should().BeEquivalentTo(
                expectedPdsValidationProviderException);

            this.pdsMock.Verify(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()),
                    Times.Once);

            this.pdsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionWhenTypeIPdsDependencyExceptionOnLookupByNhsNumber()
        {
            // given
            var someException = new Xeption();

            var somePdsValidationException =
                new SomePdsDependencyException(
                    message: "Some pds dependency exception occurred",
                    innerException: someException);

            PdsProviderDependencyException expectedPdsDependencyProviderException =
                new PdsProviderDependencyException(
                    message: somePdsValidationException.Message,
                    innerException: somePdsValidationException);

            this.pdsMock.Setup(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()))
                    .ThrowsAsync(somePdsValidationException);

            // when
            ValueTask<Patient> patientLookupTask =
                this.pdsAbstractionProvider
                    .PatientLookupByNhsNumberAsync(It.IsAny<string>());

            PdsProviderDependencyException actualPdsDependencyProviderException =
                await Assert.ThrowsAsync<PdsProviderDependencyException>(testCode: patientLookupTask.AsTask);

            // then
            actualPdsDependencyProviderException.Should().BeEquivalentTo(
                expectedPdsDependencyProviderException);

            this.pdsMock.Verify(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()),
                    Times.Once);

            this.pdsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionWhenTypeIPdsServiceExceptionOnLookupByNhsNumber()
        {
            // given
            var someException = new Xeption();

            var somePdsValidationException =
                new SomePdsServiceException(
                    message: "Some pds service exception occurred",
                    innerException: someException);

            PdsProviderServiceException expectedPdsServiceProviderException =
                new PdsProviderServiceException(
                    message: somePdsValidationException.Message,
                    innerException: somePdsValidationException);

            this.pdsMock.Setup(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()))
                    .ThrowsAsync(somePdsValidationException);

            // when
            ValueTask<Patient> patientLookupTask =
                this.pdsAbstractionProvider
                    .PatientLookupByNhsNumberAsync(It.IsAny<string>());

            PdsProviderServiceException actualPdsServiceProviderException =
                await Assert.ThrowsAsync<PdsProviderServiceException>(testCode: patientLookupTask.AsTask);

            // then
            actualPdsServiceProviderException.Should().BeEquivalentTo(
                expectedPdsServiceProviderException);

            this.pdsMock.Verify(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()),
                    Times.Once);

            this.pdsMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowUncatagorizedServiceExceptionWhenTypeIsNotExpectedOnLookupByNhsNumber()
        {
            // given
            var someException = new Xeption();

            PdsProviderServiceException expectedPdsServiceProviderException =
                new PdsProviderServiceException(
                    message: "Pds provider not properly implemented. Uncatagorized errors found, " +
                        "contact the pds provider owner for support.",

                    innerException: someException);

            this.pdsMock.Setup(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()))
                    .ThrowsAsync(someException);

            // when
            ValueTask<Patient> patientLookupTask =
                this.pdsAbstractionProvider
                    .PatientLookupByNhsNumberAsync(It.IsAny<string>());

            PdsProviderServiceException actualPdsServiceProviderException =
                await Assert.ThrowsAsync<PdsProviderServiceException>(testCode: patientLookupTask.AsTask);

            // then
            actualPdsServiceProviderException.Should().BeEquivalentTo(
                expectedPdsServiceProviderException);

            this.pdsMock.Verify(provider =>
                provider.PatientLookupByNhsNumberAsync(It.IsAny<string>()),
                    Times.Once);

            this.pdsMock.VerifyNoOtherCalls();
        }
    }
}
