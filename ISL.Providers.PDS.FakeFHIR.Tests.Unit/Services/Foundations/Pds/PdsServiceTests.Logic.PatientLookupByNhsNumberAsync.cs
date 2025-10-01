// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FakeFHIR.Models;
using ISL.Providers.PDS.FakeFHIR.Services.Foundations;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Services.Foundations.Pds
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberWithPatientsThatAreNotMarkedAsSensitiveAsync()
        {
            // given
            string randomString = GetRandomString();
            string randomInputNhsNumber = GenerateRandom10DigitNumber();
            string inputNhsNumber = randomInputNhsNumber.DeepClone();

            List<PdsPatientDetails> randomPdsPatientDetails =
                CreateRandomPdsPatientDetails(randomString).ToList();

            randomPdsPatientDetails.ForEach(details => details.IsSensitive = false);
            PdsPatientDetails nhsMatchingPatient = CreateRandomPdsPatientDetailsWithMatchingNhsNumber(inputNhsNumber);
            nhsMatchingPatient.IsSensitive = false;

            List<PdsPatientDetails> outputPdsPatientDetails = randomPdsPatientDetails.DeepClone();
            outputPdsPatientDetails.Add(nhsMatchingPatient);

            Patient randomPatient = CreateMatchingPatient(nhsMatchingPatient);
            Patient outputPatient = randomPatient.DeepClone();
            Patient expectedResponse = outputPatient.DeepClone();

            var pdsServiceMock = new Mock<PdsService>(this.fakeFHIRProviderConfiguration)
            {
                CallBase = true
            };

            pdsServiceMock.Setup(service =>
                service.GetFakePatientDetails())
                    .Returns(outputPdsPatientDetails);

            // when
            Patient actualResponse = await pdsServiceMock.Object
                .PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
            actualResponse.Meta.Security.First().Code.Should().Be("U");
            actualResponse.Meta.Security.First().Display.Should().Be("unrestricted");
        }

        [Fact]
        public async Task ShouldPatientLookupByNhsNumberWithPatientsThatAreMarkedAsSensitiveAsync()
        {
            // given
            string randomString = GetRandomString();
            string randomInputNhsNumber = GenerateRandom10DigitNumber();
            string inputNhsNumber = randomInputNhsNumber.DeepClone();

            List<PdsPatientDetails> randomPdsPatientDetails =
                CreateRandomPdsPatientDetails(randomString).ToList();

            randomPdsPatientDetails.ForEach(details => details.IsSensitive = true);
            PdsPatientDetails nhsMatchingPatient = CreateRandomPdsPatientDetailsWithMatchingNhsNumber(inputNhsNumber);
            nhsMatchingPatient.IsSensitive = true;

            List<PdsPatientDetails> outputPdsPatientDetails = randomPdsPatientDetails.DeepClone();
            outputPdsPatientDetails.Add(nhsMatchingPatient);

            Patient randomPatient = CreateMatchingPatient(nhsMatchingPatient);
            Patient outputPatient = randomPatient.DeepClone();
            Patient expectedResponse = outputPatient.DeepClone();

            var pdsServiceMock = new Mock<PdsService>(this.fakeFHIRProviderConfiguration)
            {
                CallBase = true
            };

            pdsServiceMock.Setup(service =>
                    service.GetFakePatientDetails())
                .Returns(outputPdsPatientDetails);

            // when
            Patient actualResponse = await pdsServiceMock.Object
                .PatientLookupByNhsNumberAsync(inputNhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
            actualResponse.Meta.Security.First().Code.Should().Be("R");
            actualResponse.Meta.Security.First().Display.Should().Be("restricted");
        }
    }
}
