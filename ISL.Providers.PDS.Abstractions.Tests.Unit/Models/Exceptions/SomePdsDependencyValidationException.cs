// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using System.Collections;
using Xeptions;

namespace ISL.Providers.PDS.Abstractions.Tests.Unit.Models.Exceptions
{
    public class SomePdsDependencyValidationException : Xeption, IPdsProviderDependencyValidationException
    {
        public SomePdsDependencyValidationException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
