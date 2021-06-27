using System;

namespace ShorkBot.Data
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string name)
        {
            Name = name;
        }
        
        public string Name { get; }
    }
}