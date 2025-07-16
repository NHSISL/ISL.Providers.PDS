// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using Xeptions;

namespace ISL.Providers.PDS.Abstractions.Models.Exceptions
{
    public class PdsProviderServiceException : Xeption
    {
        public PdsProviderServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }

        public PdsProviderServiceException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
