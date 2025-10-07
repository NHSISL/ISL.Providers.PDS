// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using Hl7.Fhir.Model;
using ISL.Providers.PDS.FakeFHIR.Mappers;
using ISL.Providers.PDS.FakeFHIR.Models;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Unit.Mappers
{
    public partial class PatientMapperTests
    {
        [Fact]
        public void ShouldMapPatientFromPdsPatientDetails()
        {
            // given
            PdsPatientDetails randomPdsPatientDetails = CreateRandomPdsPatientDetails();
            PdsPatientDetails inputPdsPatientDetails = randomPdsPatientDetails.DeepClone();
            Patient expectedResponse = GetFhirPatient(inputPdsPatientDetails);

            // when
            Patient actualResponse = PatientMapper.FromPdsPatientDetails(inputPdsPatientDetails);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
