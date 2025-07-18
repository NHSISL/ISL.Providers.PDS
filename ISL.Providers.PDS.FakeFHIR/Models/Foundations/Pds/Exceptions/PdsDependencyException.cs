// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions
{
    public class PdsDependencyException : Xeption
    {
        public PdsDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
