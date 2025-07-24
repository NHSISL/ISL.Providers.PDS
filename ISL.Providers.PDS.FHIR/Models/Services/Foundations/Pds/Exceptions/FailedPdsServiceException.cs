// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.Providers.PDS.FHIR.Models.Services.Foundations.Pds.Exceptions
{
    public class FailedPdsServiceException : Xeption
    {
        public FailedPdsServiceException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
