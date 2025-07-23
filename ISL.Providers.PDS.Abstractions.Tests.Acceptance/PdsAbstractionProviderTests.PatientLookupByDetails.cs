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
            string randomInputSurname = GetRandomString();
            string inputSurname = randomInputSurname.DeepClone();
            PatientBundle randomOutputPatientBundle = CreateRandomPatientBundle();
            PatientBundle outputPatientBundle = randomOutputPatientBundle.DeepClone();
            PatientBundle expectedPatientBundle = outputPatientBundle.DeepClone();

            this.pdsProviderMock.Setup(provider =>
                provider.PatientLookupByDetailsAsync(
                    null, 
                    inputSurname, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null, 
                    null))
                        .ReturnsAsync(outputPatientBundle);

            // when
            PatientBundle actualPatientBundle =
                await this.pdsAbstractionProvider.PatientLookupByDetailsAsync(
                    null,
                    inputSurname,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            // then
            actualPatientBundle.Should().BeEquivalentTo(expectedPatientBundle);
        }
    }
}
