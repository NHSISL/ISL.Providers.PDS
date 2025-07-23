// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using System;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal partial class PdsService
    {
        private delegate ValueTask<PatientBundle> ReturningPatientBundleFunction();
        private delegate ValueTask<Patient> ReturningPatientFunction();

        private async ValueTask<PatientBundle> TryCatch(
            ReturningPatientBundleFunction returningPatientBundleFunction)
        {
            try
            {
                return await returningPatientBundleFunction();
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentPdsException);
            }
            catch (Exception exception)
            {
                var failedServiceIdentificationResponseException =
                    new FailedServicePdsException(
                        message: "Failed pds service error occurred, please contact support.",
                        innerException: exception,
                        data: exception.Data);

                throw await CreateAndLogServiceExceptionAsync(failedServiceIdentificationResponseException);
            }
        }

        private async ValueTask<Patient> TryCatch(
            ReturningPatientFunction returningPatientFunction)
        {
            try
            {
                return await returningPatientFunction();
            }
            catch (InvalidArgumentPdsException invalidArgumentPdsException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentPdsException);
            }
            catch (NotFoundPdsPatientDetailsException notFoundPdsPatientDetailsException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundPdsPatientDetailsException);
            }
            catch (Exception exception)
            {
                var failedServiceIdentificationResponseException =
                    new FailedServicePdsException(
                        message: "Failed pds service error occurred, please contact support.",
                        innerException: exception,
                        data: exception.Data);

                throw await CreateAndLogServiceExceptionAsync(failedServiceIdentificationResponseException);
            }
        }

        private async ValueTask<PdsValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var accessAuditValidationException = new PdsValidationException(
                message: "Pds validation error occurred, please fix the errors and try again.",
                innerException: exception);

            return accessAuditValidationException;
        }

        private async ValueTask<PdsServiceException> CreateAndLogServiceExceptionAsync(
           Xeption exception)
        {
            var pdsServiceException = new PdsServiceException(
                message: "Pds service error occurred, please contact support.",
                innerException: exception);

            return pdsServiceException;
        }
    }
}
