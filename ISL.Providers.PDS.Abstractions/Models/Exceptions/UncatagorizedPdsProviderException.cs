// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.Providers.PDS.Abstractions.Models.Exceptions
{
    public class UncatagorizedPdsProviderException : Xeption
    {
        public UncatagorizedPdsProviderException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public UncatagorizedPdsProviderException(
            string message,
            Exception innerException,
            IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
