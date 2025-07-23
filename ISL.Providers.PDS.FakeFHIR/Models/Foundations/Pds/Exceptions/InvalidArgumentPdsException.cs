// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions
{
    public class InvalidArgumentPdsException : Xeption
    {
        public InvalidArgumentPdsException(string message)
            : base(message)
        { }
    }
}
