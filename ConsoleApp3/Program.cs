using System;
using System.Linq;
using SmartDose.DynamicClasses;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {

            var o = JObject.FromObject(new
            {
                A = (long)1234,
                B = "HALLO WELT"
            });
            var ot = o.GetType();



            var method = typeof(Program).GetMethod("Call");
            //{
            //    var genericMethod = method.MakeGenericMethod(typeof(int));
            //    genericMethod.Invoke(null, new object[] { 1, 1 });
            //}

            Type anoType = null;
            {
                var anoObjectA = new
                {
                    B = (long)123,
                    A = "Hello"
                };
                var anoTypeA = anoObjectA.GetType();

                var anoObject = new
                {
                    A = (long)123,
                    B = "Hello"
                };
                anoType = anoObject.GetType();

                foreach (var attr in anoType.GetCustomAttributes())
                    Console.WriteLine(attr.GetType().Name);
                {
                    var genericMethod = method.MakeGenericMethod(anoType);
                    genericMethod.Invoke(null, new object[] { anoObject, anoObject });
                }

                var anoTypes = anoObject.GetType().Assembly.GetTypes();

                var anoObjectAsJson = JsonConvert.SerializeObject(anoObject);
                var anoObjectNew = JsonConvert.DeserializeObject(anoObjectAsJson, anoType);

                var anoTypeAsJson = JsonConvert.SerializeObject(anoType);
                var anoTypeNew = JsonConvert.DeserializeObject<Type>(anoTypeAsJson);
            }

            Type dynType = null;
            {
                var linqList = new List<int>() { 1 };
                var linqObject = linqList.Select(l => new { A = (long)1, B = "11" }).FirstOrDefault();
                dynType = linqObject.GetType();

                foreach (var attr in dynType.GetCustomAttributes())
                    Console.WriteLine(attr.GetType().Name);

                var dynObject = Activator.CreateInstance(dynType, new object[] { 1, "a" });
                foreach (var p in dynObject.GetType().GetProperties())
                    Console.WriteLine(p.Name);
                {
                    var genericMethod = method.MakeGenericMethod(dynType);
                    // Bug Call<T> T is null
                    genericMethod.Invoke(null, new object[] { dynObject, dynObject });
                }
                var dynObjectAsJson = JsonConvert.SerializeObject(dynObject);
                var dynObjectNew = JsonConvert.DeserializeObject(dynObjectAsJson, dynType);
                var dynTypeAsJson = JsonConvert.SerializeObject(dynType);
                var dynTypeNew = JsonConvert.DeserializeObject<Type>(dynTypeAsJson);
                {
                    var genericMethod = method.MakeGenericMethod(dynTypeNew);
                    // Bug Call<T> T is null
                    genericMethod.Invoke(null, new object[] { dynObject, dynObject });
                }
            }

            dynType = null;
            {
                var options = ScriptOptions.Default.AddReferences(typeof(Program).Assembly);

                var a = CSharpScript.EvaluateAsync("new {A=(long)0, B=\"test\"}");
                var b = CSharpScript.EvaluateAsync("new {A=(long)0, B=\"test\"}");
                var c = CSharpScript.EvaluateAsync("new {A=(long)0, B=\"test\"}");
                var d = CSharpScript.EvaluateAsync("new {A=(long)0, B=\"test\"}");
                var e = CSharpScript.EvaluateAsync("new {A=(long)0, B=\"test\"}");
                a.Wait();
                dynType = a.Result.GetType();

                foreach (var attr in dynType.GetCustomAttributes())
                    Console.WriteLine(attr.GetType().Name);

                var dynObject = Activator.CreateInstance(dynType, new object[] { 1, "a" });
                foreach (var p in dynObject.GetType().GetProperties())
                    Console.WriteLine(p.Name);
                {
                    var genericMethod = method.MakeGenericMethod(dynType);
                    // Bug Call<T> T is null
                    genericMethod.Invoke(null, new object[] { dynObject, dynObject });
                }
                var dynObjectAsJson = JsonConvert.SerializeObject(dynObject);
                var dynObjectNew = JsonConvert.DeserializeObject(dynObjectAsJson, dynType);
                var dynTypeAsJson = JsonConvert.SerializeObject(dynType);
                var dynTypeNew = JsonConvert.DeserializeObject<Type>(dynTypeAsJson);
                {
                    var genericMethod = method.MakeGenericMethod(dynTypeNew);
                    // Bug Call<T> T is null
                    genericMethod.Invoke(null, new object[] { dynObject, dynObject });
                }
            }

            dynType = null;
            {
                DynamicProperty[] props = { new DynamicProperty("A", typeof(long)), new DynamicProperty("B", typeof(string)) };
                dynType = DynamicClassFactory.CreateType(props);
                foreach (var attr in dynType.GetCustomAttributes())
                    Console.WriteLine(attr.GetType().Name);

                var dynObject = Activator.CreateInstance(dynType, new object[] { 456, "World" });

                var dynTypes = dynObject.GetType().Assembly.GetTypes();
                //dynType.GetProperty("A").SetValue(dynObject, 456);
                //dynType.GetProperty("B").SetValue(dynObject, "World");
                {
                    var genericMethod = method.MakeGenericMethod(dynType);
                    // Bug Call<T> T is null
                    genericMethod.Invoke(null, new object[] { dynObject, dynObject });
                }

                var dynObjectAsJson = JsonConvert.SerializeObject(dynObject);
                var dynObjectNew = JsonConvert.DeserializeObject(dynObjectAsJson, dynType);
                var dynTypeAsJson = JsonConvert.SerializeObject(dynType);
                var dynTypeNewA = JsonConvert.DeserializeObject(dynTypeAsJson);
                var dynTypeNewB = JsonConvert.DeserializeObject(dynTypeAsJson, typeof(Type));
                var dynTypeNew = JsonConvert.DeserializeObject<Type>(dynTypeAsJson);
                {
                    var genericMethod = method.MakeGenericMethod(dynTypeNew);
                    // Bug Call<T> T is null
                    genericMethod.Invoke(null, new object[] { dynObject, dynObject });
                }
            }

            var asms = AppDomain.CurrentDomain.GetAssemblies().ToList();

            Console.ReadLine();
        }

        public static void Call<T>(T value, object objectValue)
        {
            if (typeof(T) == null)
                Console.WriteLine("NULL");
            else
                Console.WriteLine(typeof(T));
            var x = (T)objectValue;
        }
    }

}
