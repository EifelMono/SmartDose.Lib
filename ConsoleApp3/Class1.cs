using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    // https://benohead.com/create-anonymous-types-at-runtime-in-c-sharp/


    // CompilerGeneratedAttribute
    // DebuggerDisplayAttribute
    public class Class1
    {
        public static Type Create()
        {
            AssemblyBuilder dynamicAssembly = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName("MyDynamicAssembly"), AssemblyBuilderAccess.Run);

            ModuleBuilder dynamicModule = dynamicAssembly.DefineDynamicModule("MyDynamicAssemblyModule");

            TypeBuilder dynamicType = dynamicModule.DefineType("System.MyDynamicType",
                    TypeAttributes.Sealed |
                    TypeAttributes.BeforeFieldInit|
                    TypeAttributes.Serializable);

            {
                Type[] ctorParams = new Type[] { };
                ConstructorInfo classCtorInfo = typeof(CompilerGeneratedAttribute).GetConstructor(ctorParams);
                CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(
                            classCtorInfo,
                           new object[] { });

                dynamicType.SetCustomAttribute(customAttributeBuilder);
            }
            {
                Type[] ctorParams = new Type[] { typeof(string) };
                ConstructorInfo classCtorInfo = typeof(DebuggerDisplayAttribute).GetConstructor(ctorParams);
                CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(
                classCtorInfo,
                           new object[] { "A={A} B={B}" });

                dynamicType.SetCustomAttribute(customAttributeBuilder);
            }

            AddProperty(dynamicType, "A", typeof(long));
            AddProperty(dynamicType, "B", typeof(string));
            var type = dynamicType.CreateType();
            // type.MakeGenericType(typeof(long), typeof(string));

            return type;
        }
        private static string GetPublicKeyTokenFromAssembly(Assembly assembly)
        {
            var bytes = assembly.GetName().GetPublicKeyToken();
            if (bytes == null || bytes.Length == 0)
                return "None";

            var publicKeyToken = string.Empty;
            for (int i = 0; i < bytes.GetLength(0); i++)
                publicKeyToken += string.Format("{0:x2}", bytes[i]);

            return publicKeyToken;
        }

        public static void AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;

            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            PropertyBuilder property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType,
                new[] { propertyType });

            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_value", getSetAttr, propertyType,
                Type.EmptyTypes);
            ILGenerator getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, field);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_value", getSetAttr, null, new[] { propertyType });
            ILGenerator setIl = setMethodBuilder.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, field);
            setIl.Emit(OpCodes.Ret);

            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);
        }
    }
}
