// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using System;

namespace ISL.Providers.PDS.FHIR.Brokers.IdentifierBroker
{
    internal interface IIdentifierBroker
    {
        ValueTask<Guid> GetIdentifierAsync();
    }
}
