// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions
{
    public class NoMatchingPatientsException : Xeption
    {
        public NoMatchingPatientsException(string message)
            : base(message)
        { }
    }
}
