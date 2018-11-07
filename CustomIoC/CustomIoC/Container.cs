using CustomIoC.Attributes;
using CustomIoC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomIoC
{
    public class Container
    {
        private readonly Dictionary<Type, Type> typesDictionary;

        public Container()
        {
            typesDictionary = new Dictionary<Type, Type>();
        }

        // Explicit way of adding dependencies
        public void AddType(Type type)
        {
            typesDictionary.Add(type, type);
        }

        public void AddType(Type baseType, Type type)
        {
            typesDictionary.Add(baseType, type);
        }

        // Read dependencies from assembly by attributes
        public void AddAssembly(Assembly assembly)
        {
            var listOfTypes = assembly.GetTypes();
            foreach (var type in listOfTypes)
            {
                var typeImportConstrAttr = type.GetCustomAttribute<ImportConstructorAttribute>();
                var typeImportPropAttr = type.GetProperties().Where(x => x.GetCustomAttribute<ImportAttribute>() != null);

                if (typeImportConstrAttr != null || typeImportPropAttr.Count() > 0)
                {
                    typesDictionary.Add(type, type);
                }

                var typeExportAttr = type.GetCustomAttributes<ExportAttribute>();
                foreach(var exportAttr in typeExportAttr)
                {
                    if(exportAttr.Type != null)
                    {
                        typesDictionary.Add(exportAttr.Type, type);
                    }
                    else
                    {
                        typesDictionary.Add(type, type);
                    }
                }
            }
        }

        // Get an instance of a class that was previously registered with all dependencies
        public object CreateInstance(Type type)
        {
            return CreateInstanceWithDependencies(type);
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstanceWithDependencies(typeof(T));
        }

        #region Private

        private object CreateInstanceWithDependencies(Type type)
        {
            if (!typesDictionary.ContainsKey(type))
            {
                throw new CustomIoCExpection($"CreateInstance method thrown an exception. Type {type.Name} is not registered.");
            }

            var typeToGetInstance = typesDictionary[type];
            var constrOfType = typeToGetInstance.GetConstructors();
            if(constrOfType.Count() == 0)
            {
                throw new CustomIoCExpection($"CreateInstance method thrown an exception. Type {type.Name} doesn't have a public constructor.");
            }

            var currectConstr = constrOfType.First(); 
            var instance = ResolveConstructor(typeToGetInstance, currectConstr);
            if (type.GetCustomAttribute<ImportConstructorAttribute>() != null)
            {
                return instance;
            }

            ResolveProperties(type, instance);
            return instance;
        }

        private void ResolveProperties(Type type, object instance)
        {
            var propertiesToResolve = type.GetProperties().Where(x => x.GetCustomAttribute<ImportAttribute>() != null);
            foreach (var property in propertiesToResolve)
            {
                var resolvedProp = CreateInstanceWithDependencies(property.PropertyType);
                property.SetValue(instance, resolvedProp);
            }
        }

        private object ResolveConstructor(Type type, ConstructorInfo constrInfo)
        {
            var currectConstrParams = constrInfo.GetParameters();
            var resolvedParams = new List<object>();
            Array.ForEach(currectConstrParams, x => resolvedParams.Add(CreateInstanceWithDependencies(x.ParameterType)));
            var instance = Activator.CreateInstance(type, resolvedParams.ToArray());
            return instance;
        }

        #endregion
    }
}
