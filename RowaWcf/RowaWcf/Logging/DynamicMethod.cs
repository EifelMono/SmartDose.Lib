using System;
using System.IO;
using System.Reflection;

namespace Rowa.Lib.Wcf.Logging
{
    /// <summary>
    /// Class which handles the comfortable handling of methods from dynamically loaded assemblies.
    /// </summary>
    internal class DynamicMethod
    {
        #region Members

        /// <summary>
        /// The method information of the dynamic method itself.
        /// </summary>
        private readonly MethodInfo _method;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicMethod" /> class.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly to load the method from.</param>
        /// <param name="typeName">Name of the type which contains the method.</param>
        /// <param name="methodName">Name of the method itself.</param>
        /// <param name="parameterTypes">Optional list of parameter types to define the concrete overloaded version of a method.</param>
        public DynamicMethod(string assemblyName, string typeName, string methodName, params Type[] parameterTypes)
        {
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new ArgumentException(nameof(assemblyName));
            }

            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException(nameof(typeName));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentException(nameof(methodName));
            }

            try
            {
                var assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assemblyName);
                var assembly = Assembly.LoadFile(assemblyPath);
                var type = assembly.GetType(typeName);

                if (type != null)
                {
                    _method = type.GetMethod(methodName, parameterTypes);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Invokes the dynamic static method with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments to use when invoking the method.</param>
        /// <returns>
        /// The result of the invoked method.
        /// </returns>
        public object Invoke(params object[] arguments)
        {
            if (_method == null)
            {
                return null;
            }

            return _method.Invoke(null, arguments);
        }

        /// <summary>
        /// Invokes the dynamic instance method with the specified arguments on the specified object instance.
        /// </summary>
        /// <param name="objectInstance">The object instance to use when invoking the method.</param>
        /// <param name="args">The arguments to use when invoking the method.</param>
        /// <returns>The result of the invoked method.</returns>
        public object Invoke(object objectInstance, params object[] args)
        {
            if (_method == null)
            {
                return null;
            }

            return _method.Invoke(objectInstance, args);
        }
    }
}
