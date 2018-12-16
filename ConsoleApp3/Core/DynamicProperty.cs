using System;

namespace SmartDose.DynamicClasses
{
    public class DynamicProperty
    {
        public DynamicProperty(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public Type Type { get; }
    }
}
