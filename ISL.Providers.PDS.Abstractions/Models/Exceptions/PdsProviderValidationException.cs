// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using Xeptions;

namespace ISL.Providers.PDS.Abstractions.Models.Exceptions
{
    public class PdsProviderValidationException : Xeption
    {
        public PdsProviderValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }

        public PdsProviderValidationException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
