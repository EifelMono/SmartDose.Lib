using System.Reflection;
using System.Reflection.Emit;

namespace SmartDose.DynamicClasses
{
    internal static class AssemblyBuilderFactory
    {
        public static AssemblyBuilder DefineDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
        {
            var assemblyBuilder= AssemblyBuilder.DefineDynamicAssembly(name, access);
            return assemblyBuilder;
        }
    }
}
