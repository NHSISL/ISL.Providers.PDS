// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Tests.Acceptance
{
    public partial class PdsServiceTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            string randomIdentifierString = GenerateRandom10DigitNumber();
            string nhsNumber = randomIdentifierString.DeepClone();
            DateTimeOffset dateOfBirth = new DateTimeOffset(new DateTime(2010, 10, 22));

            PdsResponse randomResponse = CreateRandomPdsResponse(
                dateOfBirth: dateOfBirth,
                nhsNumber: nhsNumber);

            PdsResponse expectedResponse = randomResponse.DeepClone();

            // when
            PdsResponse actualResponse =
                await this.fakeFHIRProvider.PatientLookupByNhsNumberAsync(nhsNumber);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse, options =>
                options.Excluding(r => r.ResponseId));
        }
    }
}
