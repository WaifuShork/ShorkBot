using System;

namespace ShorkBot.Data
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute
    {
        public ModuleAttribute(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}