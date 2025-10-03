// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Force.DeepCloner;
using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Integration
{
    public partial class PdsFHIRProviderTests
    {
        [Fact]
        public async Task ShouldPatientLookupByNhsNumberAsync()
        {
            // given
            string randomString = "9449304424";
            string inputNhsNumber = randomString.DeepClone();

            // when
            await this.pdsFHIRProvider.PatientLookupByNhsNumberAsync(inputNhsNumber);
        }
    }
}
