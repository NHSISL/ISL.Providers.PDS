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
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            string randomString = GetRandomString();
            string surname = randomString.DeepClone();
            string postcode = randomString.DeepClone();
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset dateOfBirth = randomDateTimeOffset.DeepClone();

            PdsResponse randomResponse = CreateRandomPdsResponse(
                dateOfBirth: dateOfBirth, 
                surname: surname,
                postcode: postcode);

            PdsResponse expectedResponse = randomResponse.DeepClone();

            // when
            PdsResponse actualResponse =
                await this.fakeFHIRProvider.PatientLookupByDetailsAsync(surname, postcode, dateOfBirth);

            // then
            actualResponse.Should().BeEquivalentTo(expectedResponse, options =>
                options.Excluding(r => r.ResponseId));
        }
    }
}
