using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lollipop.Models.Common
{
    public class Enums
    {
        public enum ExceptionGroup
        {
            [Description("SYSTEM")]
            System,
            [Description("VALIDATION")]
            Validation
        }
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
