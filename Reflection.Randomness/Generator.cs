using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection.Randomness
{
    public class Generator<T> where T : new()
    {
        private readonly Dictionary<string, Func<Random, object>> Generators = new Dictionary<string, Func<Random, object>>();
        
        public Generator()
        {
            var properties = typeof(T).GetProperties().Where(p => p.GetCustomAttribute<FromDistributionAttribute>() != null).ToList();
            foreach(var item in properties)
            {
                var attribute = item.GetCustomAttribute<FromDistributionAttribute>();
                var distributionType = attribute.DistributionType;
                var parameters = attribute.Parameters;
                

                if (!typeof(IContinuousDistribution).IsAssignableFrom(distributionType)) 
                    throw new ArgumentException($"Invalid distribution type '{distributionType.Name}' for property '{item.Name}'");

                var paramsTypes = parameters.Select(p => p.GetType()).ToArray();
                var distributionConstructor = distributionType.GetConstructor(paramsTypes);

                if (distributionConstructor == null) 
                    throw new ArgumentException($"Invalid distribution parameters for property '{distributionType}'");
                
                var countConstructorParams = distributionConstructor.GetParameters().Length;
                if (parameters.Length != countConstructorParams)
                    throw new ArgumentException($"Invalid distribution parameters for property '{item.Name}' of type '{distributionType.Name}'");

                Generators[item.Name] = (rnd) => {
                    var distribution = (IContinuousDistribution)Activator.CreateInstance(distributionType, parameters);
                    return distribution.Generate(rnd);
                };
            }
        }

        public T Generate(Random rnd)
        {
            var item = new T();
            foreach (var Generator in Generators)
            {
                var property = typeof(T).GetProperty(Generator.Key);
                var value = Generator.Value.Invoke(rnd);
                property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
            }
            return item;
        }
    }

    public class FromDistributionAttribute : Attribute
    {
        public Type DistributionType { get; }
        public object[] Parameters { get; }

        public FromDistributionAttribute(Type distributionType, params object[] parameters)
        {
            DistributionType = distributionType;
            Parameters = parameters;
        }
    }
}