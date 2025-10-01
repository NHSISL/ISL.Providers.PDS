// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using ISL.Providers.PDS.FakeFHIR.Models;
using ISL.Providers.PDS.FakeFHIR.Services.Foundations;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsWithPatientsThatAreNotMarkedAsSensitiveAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputSearchString = randomString.DeepClone();

            List<PdsPatientDetails> randomPdsPatientDetails =
                CreateRandomPdsPatientDetails(inputSearchString).ToList();

            randomPdsPatientDetails.ForEach(details => details.IsSensitive = false);

            List<PdsPatientDetails> filteredPdsPatientDetails =
                GetFilteredPdsPatientDetails(randomPdsPatientDetails, inputSearchString);

            PatientBundle randomPatientBundle = CreatePatientBundleFromPatientDetails(filteredPdsPatientDetails);
            PatientBundle output = randomPatientBundle.DeepClone();
            PatientBundle expectedResponse = output.DeepClone();

            var pdsServiceMock = new Mock<PdsService>(this.fakeFHIRProviderConfiguration)
            {
                CallBase = true
            };

            pdsServiceMock.Setup(service =>
                service.GetFakePatientDetails())
                    .Returns(randomPdsPatientDetails);

            pdsServiceMock.Setup(service =>
                service.FilterPatientByDetails(
                    randomPdsPatientDetails,
                    inputSearchString,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null))
                        .Returns(filteredPdsPatientDetails);

            // when
            PatientBundle actualResponse = await pdsServiceMock.Object
                .PatientLookupByDetailsAsync(
                    inputSearchString,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);

            foreach (var patient in actualResponse.Patients)
            {
                patient.Meta.Security.First().Code.Should().Be("U");
                patient.Meta.Security.First().Display.Should().Be("unrestricted");
            }
        }

        [Fact]
        public async Task ShouldPatientLookupByDetailsWithPatientsThatAreMarkedAsSensitiveAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputSearchString = randomString.DeepClone();

            List<PdsPatientDetails> randomPdsPatientDetails =
                CreateRandomPdsPatientDetails(inputSearchString).ToList();

            randomPdsPatientDetails.ForEach(details => details.IsSensitive = true);

            List<PdsPatientDetails> filteredPdsPatientDetails =
                GetFilteredPdsPatientDetails(randomPdsPatientDetails, inputSearchString);

            PatientBundle randomPatientBundle = CreatePatientBundleFromPatientDetails(filteredPdsPatientDetails);
            PatientBundle output = randomPatientBundle.DeepClone();
            PatientBundle expectedResponse = output.DeepClone();

            var pdsServiceMock = new Mock<PdsService>(this.fakeFHIRProviderConfiguration)
            {
                CallBase = true
            };

            pdsServiceMock.Setup(service =>
                service.GetFakePatientDetails())
                    .Returns(randomPdsPatientDetails);

            pdsServiceMock.Setup(service =>
                service.FilterPatientByDetails(
                    randomPdsPatientDetails,
                    inputSearchString,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null))
                        .Returns(filteredPdsPatientDetails);

            // when
            PatientBundle actualResponse = await pdsServiceMock.Object
                .PatientLookupByDetailsAsync(
                    inputSearchString,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);

            foreach (var patient in actualResponse.Patients)
            {
                patient.Meta.Security.First().Code.Should().Be("R");
                patient.Meta.Security.First().Display.Should().Be("restricted");
            }
        }
    }
}
