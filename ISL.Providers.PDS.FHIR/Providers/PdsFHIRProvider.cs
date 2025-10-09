// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker;
using ISL.Providers.PDS.FHIR.Models.Brokers.PdsFHIR;
using ISL.Providers.PDS.FHIR.Models.Providers.Exceptions;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using ISL.Providers.PDS.FHIR.Services.Foundations.Pds;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace ISL.Providers.PDS.FHIR.Providers
{
    public class PdsFHIRProvider : IPdsFHIRProvider
    {
        private IPdsService pdsService { get; set; }

        public PdsFHIRProvider(PdsFHIRConfigurations configurations)
        {
            IServiceProvider serviceProvider = RegisterServices(configurations);
            InitializeClients(serviceProvider);
        }

        /// <summary>
        /// Uses PDS FHIR API to obtain the NHS Number for a patient given their Surname, DOB and postcode
        /// </summary>
        /// <returns>
        /// A PDS response where the NHS Number has been replaced by the real NHS Number and additional details populated.
        /// If the PDS search could not happen due to search parameters being invalid, the Nhs Number will be
        /// replaced by 0000000000.
        /// </returns>
        /// <exception cref="PdsFHIRProviderValidationException" />
        /// <exception cref="PdsFHIRProviderDependencyValidationException" />
        /// <exception cref="PdsFHIRProviderDependencyException" />
        /// <exception cref="PdsFHIRProviderServiceException" />
        public async ValueTask<PatientBundle> PatientLookupByDetailsAsync(
            string givenName = null,
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
        /// Uses PDS FHIR API to obtain the name, DOB, Postcode and contact information
        /// for a patient given their NHS Number 
        /// </summary>
        /// <returns>
        /// A PDS response where the name, DOB, Postcode and contact information are populated.
        /// If the PDS search could not happen due to search parameters being invalid
        /// DOB, Postcode and contact information will be empty.
        /// </returns>
        /// <exception cref="PdsFHIRProviderValidationException" />
        /// <exception cref="PdsFHIRProviderDependencyValidationException" />
        /// <exception cref="PdsFHIRProviderDependencyException" />
        /// <exception cref="PdsFHIRProviderServiceException" />
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

        private static PdsFHIRProviderValidationException CreateProviderValidationException(
            Xeption innerException)
        {
            return new PdsFHIRProviderValidationException(
                message: innerException.Message,
                innerException,
                data: innerException.Data);
        }

        private static PdsFHIRProviderDependencyValidationException CreateProviderDependencyValidationException(
            Xeption innerException)
        {
            return new PdsFHIRProviderDependencyValidationException(
                message: innerException.Message,
                innerException,
                data: innerException.Data);
        }

        private static PdsFHIRProviderDependencyException CreateProviderDependencyException(
            Xeption innerException)
        {
            return new PdsFHIRProviderDependencyException(
                message: innerException.Message,
                innerException);
        }

        private static PdsFHIRProviderServiceException CreateProviderServiceException(Xeption innerException)
        {
            return new PdsFHIRProviderServiceException(
                message: innerException.Message,
                innerException);
        }

        private void InitializeClients(IServiceProvider serviceProvider) =>
            this.pdsService = serviceProvider.GetRequiredService<IPdsService>();

        private static IServiceProvider RegisterServices(PdsFHIRConfigurations configurations)
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IPdsFHIRBroker, PdsFHIRBroker>()
                .AddTransient<IPdsService, PdsService>()
                .AddSingleton(configurations);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
