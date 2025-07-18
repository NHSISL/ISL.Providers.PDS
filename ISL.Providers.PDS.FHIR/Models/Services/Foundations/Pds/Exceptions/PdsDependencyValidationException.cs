// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions
{
    public class PdsDependencyValidationException : Xeption
    {
        public PdsDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
