// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;

namespace ISL.Providers.PDS.Abstractions.Models
{
    public class PdsRequest
    {
        public Guid RequestId { get; set; }
        public string NhsNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Postcode { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }
}
