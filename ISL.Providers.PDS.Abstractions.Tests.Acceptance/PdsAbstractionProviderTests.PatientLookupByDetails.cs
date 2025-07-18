// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ISL.Providers.PDS.Abstractions.Tests.Acceptance
{
    public partial class PdsAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            string randomInputSurname = GetRandomString();
            string inputSurname = randomInputSurname.DeepClone();
            string randomInputPostcode = GetRandomString();
            string inputPostcode = randomInputSurname.DeepClone();
            DateTimeOffset randomInputDateOfBirth = GetRandomDateTimeOffset();
            DateTimeOffset inputDateOfBirth = randomInputDateOfBirth.DeepClone();
            PdsResponse randomOutputPdsResponse = CreateRandomPdsResponse();
            PdsResponse outputPdsResponse = randomOutputPdsResponse.DeepClone();
            PdsResponse expectedPdsResponse = outputPdsResponse.DeepClone();

            this.pdsProviderMock.Setup(provider =>
                provider.PatientLookupByDetailsAsync(inputSurname, inputPostcode, inputDateOfBirth))
                    .ReturnsAsync(outputPdsResponse);

            // when
            PdsResponse actualPdsResponse =
                await this.pdsAbstractionProvider.PatientLookupByDetailsAsync(inputSurname, inputPostcode, inputDateOfBirth);

            // then
            actualPdsResponse.Should().BeEquivalentTo(expectedPdsResponse);
        }
    }
}
