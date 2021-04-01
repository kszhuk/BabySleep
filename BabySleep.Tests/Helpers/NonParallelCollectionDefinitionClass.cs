using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BabySleep.Tests.Helpers
{
    [CollectionDefinition("Non-Parallel Collection", DisableParallelization = true)]
    public class NonParallelCollectionDefinitionClass
    {
    }
}
