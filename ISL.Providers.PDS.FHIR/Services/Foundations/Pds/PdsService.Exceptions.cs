// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using System;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.PDS.FHIR.Services.Foundations.Pds
{
    internal partial class PdsService
    {
        private delegate ValueTask<PatientBundle> ReturningPatientBundleFunction();
        private delegate ValueTask<Patient> ReturningPatientFunction();

        private async ValueTask<PatientBundle> TryCatch(ReturningPatientBundleFunction returningPatientBundleFunction)
        {
            try
            {
                return await returningPatientBundleFunction();
            }
            catch (InvalidPdsArgumentException invalidArgumentPdsException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentPdsException);
            }
            catch (Exception exception)
            {
                var failedPdsServiceException =
                    new FailedPdsServiceException(
                        message: "Failed PDS service error occurred, please contact support.",
                        innerException: exception,
                        data: exception.Data);

                throw await CreateAndLogServiceExceptionAsync(failedPdsServiceException);
            }
        }

        private async ValueTask<Patient> TryCatch(ReturningPatientFunction returningPatientFunction)
        {
            try
            {
                return await returningPatientFunction();
            }
            catch (InvalidPdsArgumentException invalidArgumentPdsException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentPdsException);
            }
            catch (Exception exception)
            {
                var failedPdsServiceException =
                    new FailedPdsServiceException(
                        message: "Failed PDS service error occurred, please contact support.",
                        innerException: exception,
                        data: exception.Data);

                throw await CreateAndLogServiceExceptionAsync(failedPdsServiceException);
            }
        }

        private async ValueTask<PdsValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var pdsValidationException = new PdsValidationException(
                message: "PDS validation error occurred, please fix the errors and try again.",
                innerException: exception);

            return pdsValidationException;
        }

        private async ValueTask<PdsServiceException> CreateAndLogServiceExceptionAsync(Xeption exception)
        {
            var pdsServiceException = new PdsServiceException(
                message: "PDS service error occurred, please contact support.",
                innerException: exception);

            return pdsServiceException;
        }
    }
}
