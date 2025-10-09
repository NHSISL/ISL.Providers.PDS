// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.PDS.Abstractions
{
    public partial class PdsAbstractionProvider
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
            catch (Xeption exception) when (exception is IPdsProviderValidationException)
            {
                throw CreateValidationException(exception);
            }
            catch (Xeption exception) when (exception is IPdsProviderDependencyValidationException)
            {
                throw CreateValidationException(exception);
            }
            catch (Xeption exception) when (exception is IPdsProviderDependencyException)
            {
                throw CreateDependencyException(exception);
            }
            catch (Xeption exception) when (exception is IPdsProviderServiceException)
            {
                throw CreateServiceException(exception);
            }
            catch (Exception exception)
            {
                throw CreateUncatagorizedServiceException(exception);
            }
        }

        private async ValueTask<Patient> TryCatch(
            ReturningPatientFunction returningPatientFunction)
        {
            try
            {
                return await returningPatientFunction();
            }
            catch (Xeption exception) when (exception is IPdsProviderValidationException)
            {
                throw CreateValidationException(exception);
            }
            catch (Xeption exception) when (exception is IPdsProviderDependencyValidationException)
            {
                throw CreateValidationException(exception);
            }
            catch (Xeption exception) when (exception is IPdsProviderDependencyException)
            {
                throw CreateDependencyException(exception);
            }
            catch (Xeption exception) when (exception is IPdsProviderServiceException)
            {
                throw CreateServiceException(exception);
            }
            catch (Exception exception)
            {
                throw CreateUncatagorizedServiceException(exception);
            }
        }

        private PdsProviderValidationException CreateValidationException(Xeption exception)
        {
            var pdsProviderValidationException =
                new PdsProviderValidationException(
                    message: exception.Message,
                    innerException: exception,
                    data: exception.Data);

            return pdsProviderValidationException;
        }

        private PdsProviderDependencyException CreateDependencyException(Xeption exception)
        {
            var pdsProviderDependencyException = new PdsProviderDependencyException(
                message: exception.Message,
                innerException: exception,
                data: exception.Data);

            return pdsProviderDependencyException;
        }

        private PdsProviderServiceException CreateServiceException(Xeption exception)
        {
            var pdsProviderServiceException = new PdsProviderServiceException(
                message: exception.Message,
                innerException: exception,
                data: exception.Data);

            return pdsProviderServiceException;
        }

        private PdsProviderServiceException CreateUncatagorizedServiceException(Exception exception)
        {
            var pdsProviderServiceException = new PdsProviderServiceException(
                message: "Pds provider not properly implemented. Uncatagorized errors found, " +
                    "contact the pds provider owner for support.",

                innerException: exception as Xeption,
                data: exception.Data);

            return pdsProviderServiceException;
        }
    }
}
