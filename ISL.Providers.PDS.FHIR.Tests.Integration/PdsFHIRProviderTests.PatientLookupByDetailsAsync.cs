// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Task = System.Threading.Tasks.Task;

namespace ISL.Providers.PDS.FHIR.Tests.Integration
{
    public partial class PdsFHIRProviderTests
    {
        [Fact]
        public async Task ShouldPatientLookupByDetailsAsync()
        {
            // given
            string inputSurname = "Jones26th";
            string inputPostcode = "EN3 7DG";
            string inputDateOfBirth = "1997-01-02";

            // when
            await this.pdsFHIRProvider.PatientLookupByDetailsAsync(
                familyName: inputSurname,
                postcode: inputPostcode,
                dateOfBirth: inputDateOfBirth);
        }
    }
}
