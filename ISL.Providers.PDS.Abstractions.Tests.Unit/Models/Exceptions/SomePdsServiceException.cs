// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.PDS.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.PDS.Abstractions.Tests.Unit.Models.Exceptions
{
    public class SomePdsServiceException : Xeption, IPdsProviderServiceException
    {
        public SomePdsServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
