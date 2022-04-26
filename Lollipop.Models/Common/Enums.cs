using System.ComponentModel;

namespace Lollipop.Models.Common
{
    public enum EnumExceptionGroup
    {
        [Description("SYSTEM")]
        System,
        [Description("VALIDATION")]
        Validation
    }

    public class NameValueAttribute : Attribute
    {
        internal NameValueAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
