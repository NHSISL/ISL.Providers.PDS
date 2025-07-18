// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using System;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Services.Foundations
{
    internal partial class PdsService
    {
        private delegate ValueTask<PdsResponse> ReturningPdsResponseFunction();

        private async ValueTask<PdsResponse> TryCatch(
            ReturningPdsResponseFunction returningPdsResponseFunction)
        {
            try
            {
                return await returningPdsResponseFunction();
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
