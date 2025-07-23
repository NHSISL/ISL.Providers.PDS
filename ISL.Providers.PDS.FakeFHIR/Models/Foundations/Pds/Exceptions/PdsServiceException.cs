// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions
{
    public class PdsServiceException : Xeption
    {
        public PdsServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
