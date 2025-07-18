// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FHIR.Services.Foundations.Pds;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using Xeptions;
using ISL.Providers.PDS.FHIR.Models.Providers.Exceptions;
using ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions;
using ISL.Providers.PDS.FHIR.Brokers.IdentifierBroker;
using ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker;

namespace ISL.Providers.PDS.FHIR.Providers
{
    public class PdsFHIRProvider : IPdsFHIRProvider
    {
        private IPdsService pdsService { get; set; }

        public PdsFHIRProvider()
        {
            IServiceProvider serviceProvider = RegisterServices();
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
        public async ValueTask<PdsResponse> PatientLookupByDetailsAsync(
            string surname,
            string postcode,
            DateTimeOffset dateOfBirth)
        {
            try
            {
                return await pdsService.PatientLookupByDetailsAsync(surname, postcode, dateOfBirth);
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
        public async ValueTask<PdsResponse> PatientLookupByNhsNumberAsync(string nhsNumber)
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
                message: "Pds FHIR provider validation error occurred, fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static PdsFHIRProviderDependencyValidationException CreateProviderDependencyValidationException(
            Xeption innerException)
        {
            return new PdsFHIRProviderDependencyValidationException(
                message: "Pds FHIR provider dependency validation error occurred, fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static PdsFHIRProviderDependencyException CreateProviderDependencyException(
            Xeption innerException)
        {
            return new PdsFHIRProviderDependencyException(
                message: "Pds FHIR provider dependency error occurred, contact support.",
                innerException);
        }

        private static PdsFHIRProviderServiceException CreateProviderServiceException(Xeption innerException)
        {
            return new PdsFHIRProviderServiceException(
                message: "Pds FHIR provider service error occurred, contact support.",
                innerException);
        }

        private void InitializeClients(IServiceProvider serviceProvider) =>
            this.pdsService = serviceProvider.GetRequiredService<IPdsService>();

        private static IServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IIdentifierBroker, IdentifierBroker>()
                .AddTransient<IPdsFHIRBroker, PdsFHIRBroker>()
                .AddTransient<IPdsService, PdsService>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
