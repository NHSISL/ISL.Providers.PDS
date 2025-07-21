// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using System;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.PDS.Abstractions
{
    public partial class PdsAbstractionProvider
    {
        private delegate ValueTask<PatientBundle> ReturningPdsRequestFunction();

        private async ValueTask<PatientBundle> TryCatch(
            ReturningPdsRequestFunction returningPdsRequestFunction)
        {
            try
            {
                return await returningPdsRequestFunction();
            }
            catch (Xeption ex) when (ex is IPdsProviderValidationException)
            {
                throw CreateValidationException(ex);
            }
            catch (Xeption ex) when (ex is IPdsProviderDependencyValidationException)
            {
                throw CreateValidationException(ex);
            }
            catch (Xeption ex) when (ex is IPdsProviderDependencyException)
            {
                throw CreateDependencyException(ex);
            }
            catch (Xeption ex) when (ex is IPdsProviderServiceException)
            {
                throw CreateServiceException(ex);
            }
            catch (Exception ex)
            {
                var uncatagorizedPdsProviderException =
                    new UncatagorizedPdsProviderException(
                        message: "Pds provider not properly implemented. Uncatagorized errors found, " +
                            "contact the pds provider owner for support.",
                        innerException: ex,
                        data: ex.Data);

                throw CreateUncatagorizedServiceException(uncatagorizedPdsProviderException);
            }
        }

        private PdsProviderValidationException CreateValidationException(Xeption exception)
        {
            var pdsProviderValidationException =
                new PdsProviderValidationException(
                    message: "Pds validation errors occurred, please try again.",
                    innerException: exception,
                    data: exception.Data);

            return pdsProviderValidationException;
        }

        private PdsProviderDependencyException CreateDependencyException(Xeption exception)
        {
            var pdsProviderDependencyException = new PdsProviderDependencyException(
                message: "Pds dependency error occurred, contact support.",
                innerException: exception,
                data: exception.Data);

            return pdsProviderDependencyException;
        }

        private PdsProviderServiceException CreateServiceException(Xeption exception)
        {
            var pdsProviderServiceException = new PdsProviderServiceException(
                message: "Pds service error occurred, contact support.",
                innerException: exception,
                data: exception.Data);

            return pdsProviderServiceException;
        }

        private PdsProviderServiceException CreateUncatagorizedServiceException(Exception exception)
        {
            var pdsProviderServiceException = new PdsProviderServiceException(
                message: "Uncatagorized pds service error occurred, contact support.",
                innerException: exception as Xeption,
                data: exception.Data);

            return pdsProviderServiceException;
        }
    }
}
