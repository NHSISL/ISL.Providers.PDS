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
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            string randomString = GetRandomString();
            string randomInputNhsNumber = GenerateRandom10DigitNumber();
            string inputNhsNumber = randomInputNhsNumber.DeepClone();

            List<PdsPatientDetails> randomPdsPatientDetails =
                CreateRandomPdsPatientDetails(randomString).ToList();

            PdsPatientDetails nhsMatchingPatient = CreateRandomPdsPatientDetailsWithMatchingNhsNumber(inputNhsNumber);

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
        }
    }
}
