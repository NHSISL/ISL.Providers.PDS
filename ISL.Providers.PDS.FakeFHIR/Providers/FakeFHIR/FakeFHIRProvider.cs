// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using ISL.Providers.PDS.FakeFHIR.Models.Providers.Exceptions;
using ISL.Providers.PDS.FakeFHIR.Services.Foundations;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Providers.FakeFHIR
{
    public class FakeFHIRProvider : IFakeFHIRProvider
    {
        private IPdsService pdsService { get; set; }

        public FakeFHIRProvider(FakeFHIRProviderConfigurations fakeFHIRProviderConfigurations)
        {
            IServiceProvider serviceProvider = RegisterServices(fakeFHIRProviderConfigurations);
            InitializeClients(serviceProvider);
        }

        /// <summary>
        /// Uses PDS FHIR API to obtain the NHS Number for a patient given provided search parameters
        /// </summary>
        /// <returns>
        /// A PatientBundle object containing a list of matched patients
        /// </returns>
        /// <exception cref="FakeFHIRProviderValidationException" />
        /// <exception cref="FakeFHIRProviderDependencyValidationException" />
        /// <exception cref="FakeFHIRProviderDependencyException" />
        /// <exception cref="FakeFHIRProviderServiceException" />
        public async ValueTask<PatientBundle> PatientLookupByDetailsAsync(string givenName = null,
            string familyName = null,
            string gender = null,
            string address = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null)
        {
            try
            {
                return await pdsService.PatientLookupByDetailsAsync(
                    givenName,
                    familyName,
                    gender,
                    address,
                    dateOfBirth,
                    dateOfDeath,
                    registeredGpPractice,
                    email,
                    phoneNumber);
            }
            catch (PdsValidationException pdsValidationException)
            {
                throw CreateProviderValidationException(
                    pdsValidationException.InnerException as Xeption);
            }
            catch (PdsDependencyValidationException pdsDependencyValidationException)
            {
                throw CreateProviderDependencyValidationException(
                    pdsDependencyValidationException.InnerException as Xeption);
            }
            catch (PdsDependencyException pdsDependencyException)
            {
                throw CreateProviderDependencyException(
                    pdsDependencyException.InnerException as Xeption);
            }
            catch (PdsServiceException pdsServiceException)
            {
                throw CreateProviderServiceException(
                    pdsServiceException.InnerException as Xeption);
            }
        }

        /// <summary>
        /// Uses PDS FHIR API to obtain the patient details
        /// for a patient given their NHS Number 
        /// </summary>
        /// <returns>
        /// A Patient object containing information on the corresponding patient 
        /// </returns>
        /// <exception cref="FakeFHIRProviderValidationException" />
        /// <exception cref="FakeFHIRProviderDependencyValidationException" />
        /// <exception cref="FakeFHIRProviderDependencyException" />
        /// <exception cref="FakeFHIRProviderServiceException" />
        public async ValueTask<Patient> PatientLookupByNhsNumberAsync(string nhsNumber)
        {
            try
            {
                return await pdsService.PatientLookupByNhsNumberAsync(nhsNumber);
            }
            catch (PdsValidationException pdsValidationException)
            {
                throw CreateProviderValidationException(
                    pdsValidationException.InnerException as Xeption);
            }
            catch (PdsDependencyValidationException pdsDependencyValidationException)
            {
                throw CreateProviderDependencyValidationException(
                    pdsDependencyValidationException.InnerException as Xeption);
            }
            catch (PdsDependencyException pdsDependencyException)
            {
                throw CreateProviderDependencyException(
                    pdsDependencyException.InnerException as Xeption);
            }
            catch (PdsServiceException pdsServiceException)
            {
                throw CreateProviderServiceException(
                    pdsServiceException.InnerException as Xeption);
            }
        }

        private static FakeFHIRProviderValidationException CreateProviderValidationException(
            Xeption innerException)
        {
            return new FakeFHIRProviderValidationException(
                message: innerException.Message,
                innerException,
                data: innerException.Data);
        }

        private static FakeFHIRProviderDependencyValidationException CreateProviderDependencyValidationException(
            Xeption innerException)
        {
            return new FakeFHIRProviderDependencyValidationException(
                message: innerException.Message,
                innerException,
                data: innerException.Data);
        }

        private static FakeFHIRProviderDependencyException CreateProviderDependencyException(
            Xeption innerException)
        {
            return new FakeFHIRProviderDependencyException(
                message: innerException.Message,
                innerException);
        }

        private static FakeFHIRProviderServiceException CreateProviderServiceException(Xeption innerException)
        {
            return new FakeFHIRProviderServiceException(
                message: innerException.Message,
                innerException);
        }

        private void InitializeClients(IServiceProvider serviceProvider) =>
            this.pdsService = serviceProvider.GetRequiredService<IPdsService>();

        private static IServiceProvider RegisterServices(
            FakeFHIRProviderConfigurations fakeFHIRProviderConfigurations)
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IPdsService, PdsService>()
                .AddSingleton(fakeFHIRProviderConfigurations);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
