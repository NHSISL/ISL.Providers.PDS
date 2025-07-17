// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.FakeFHIR.Models;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.PDS.FakeFHIR.Brokers.FakeFHIR
{
    internal class FakeFHIRBroker : IFakeFHIRBroker
    {
        private string NhsNumber {  get; set; }
        private PdsPatientDetails PatientDetails { get; set; }

        public FakeFHIRBroker()
        {
            this.NhsNumber = "9000000009";
            this.PatientDetails = new PdsPatientDetails
            {
                FirstName = "Jane",
                Surname = "Smith",
                Address = "1 Trevelyan Square, Boar Lane, City Centre,Leeds, West Yorkshire",
                EmailAddress = "jane.smith@example.com",
                PhoneNumber = "01632960587",
                Postcode = "LS1 6AE",
                DateOfBirth = new DateTimeOffset(new DateTime(2010, 10, 22))
            };
        }

        public async ValueTask<string> GetNhsNumberAsync(
            string surname, 
            string postcode, 
            DateTimeOffset dateOfBirth) =>
                this.NhsNumber;

        public async ValueTask<PdsPatientDetails> GetPdsPatientDetailsAsync(string nhsNumber) =>
            this.PatientDetails;
    }
}
