// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Hl7.Fhir.Model;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FHIR.Brokers.PdsFHIRBroker
{
    internal interface IPdsFHIRBroker
    {
        ValueTask<Bundle> GetNhsNumberAsync(
            string givenName = null,
            string familyName = null,
            string gender = null,
            string postcode = null,
            string dateOfBirth = null,
            string dateOfDeath = null,
            string registeredGpPractice = null,
            string email = null,
            string phoneNumber = null);

        ValueTask<Patient> GetPdsPatientDetailsAsync(string nhsNumber);
    }
}
