// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.Providers.PDS.FakeFHIR.Models.Foundations.Pds.Exceptions
{
    public class FailedServicePdsException : Xeption
    {
        public FailedServicePdsException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
