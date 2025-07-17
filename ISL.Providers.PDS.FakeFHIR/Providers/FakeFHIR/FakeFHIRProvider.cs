// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System;
using Xeptions;
using ISL.Providers.PDS.FakeFHIR.Services.Foundations;
using ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions;
using ISL.Providers.PDS.FakeFHIR.Models.Providers.Exceptions;

namespace ISL.Providers.PDS.FakeFHIR.Providers.FakeFHIR
{
    public class FakeFHIRProvider : IFakeFHIRProvider
    {
        private IPdsService pdsService { get; set; }

        public FakeFHIRProvider()
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
        /// <exception cref="FakeFHIRProviderValidationException" />
        /// <exception cref="FakeFHIRProviderDependencyValidationException" />
        /// <exception cref="FakeFHIRProviderDependencyException" />
        /// <exception cref="FakeFHIRProviderServiceException" />
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
        /// <exception cref="FakeFHIRProviderValidationException" />
        /// <exception cref="FakeFHIRProviderDependencyValidationException" />
        /// <exception cref="FakeFHIRProviderDependencyException" />
        /// <exception cref="FakeFHIRProviderServiceException" />
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

        private static FakeFHIRProviderValidationException CreateProviderValidationException(
            Xeption innerException)
        {
            return new FakeFHIRProviderValidationException(
                message: "Fake FHIR provider validation error occurred, fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static FakeFHIRProviderDependencyValidationException CreateProviderDependencyValidationException(
            Xeption innerException)
        {
            return new FakeFHIRProviderDependencyValidationException(
                message: "Fake FHIR provider dependency validation error occurred, fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static FakeFHIRProviderDependencyException CreateProviderDependencyException(
            Xeption innerException)
        {
            return new FakeFHIRProviderDependencyException(
                message: "Fake FHIR provider dependency error occurred, contact support.",
                innerException);
        }

        private static FakeFHIRProviderServiceException CreateProviderServiceException(Xeption innerException)
        {
            return new FakeFHIRProviderServiceException(
                message: "Fake FHIR provider service error occurred, contact support.",
                innerException);
        }

        private void InitializeClients(IServiceProvider serviceProvider) =>
            this.pdsService = serviceProvider.GetRequiredService<IPdsService>();

        private static IServiceProvider RegisterServices()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IFakeFHIRBroker, FakeFHIRBroker>()
                .AddTransient<IPdsService, PdsService>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
