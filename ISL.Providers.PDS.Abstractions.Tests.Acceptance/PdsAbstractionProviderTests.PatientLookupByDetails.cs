// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.PDS.Abstractions.Models;
using Moq;
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
            PdsRequest randomInputPdsRequest = CreateRandomPdsRequest();
            PdsRequest inputPdsRequest = randomInputPdsRequest.DeepClone();
            PdsRequest randomOutputPdsRequest = CreateRandomPdsRequest();
            PdsRequest outputPdsRequest = randomOutputPdsRequest.DeepClone();
            PdsRequest expectedPdsRequest = outputPdsRequest.DeepClone();

            this.pdsProviderMock.Setup(provider =>
                provider.PatientLookupByDetailsAsync(inputPdsRequest))
                    .ReturnsAsync(outputPdsRequest);

            // when
            PdsRequest actualPdsRequest =
                await this.pdsAbstractionProvider.PatientLookupByDetailsAsync(inputPdsRequest);

            // then
            actualPdsRequest.Should().BeEquivalentTo(expectedPdsRequest);
        }
    }
}
