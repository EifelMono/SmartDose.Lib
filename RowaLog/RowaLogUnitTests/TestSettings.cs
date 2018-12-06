using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

//As the Tests written here are more like integration tests they
//cannot run paralell -> operating in mostly the same directory 
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, DisableTestParallelization = true)]