using Lollipop.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lollipop.Models.Common.Enums;

namespace Lollipop.Helpers
{
    public class CustomException : Exception
    {
        public string Group { get; set; }

        public string Code { get; set; }

        public CustomException(ExceptionGroup group, string code, string message) : base(message)
        {
            Group = group.GetDescription();
            Code = code;
        }

        public static class System
        {
            public static readonly CustomException UnexpectedError = new(ExceptionGroup.System, "SYSTEM_001", "Unexpected error.");
        }

        public static class Validation
        {
            public static CustomException PropertyIsNullOrEmpty(string propertyName) => new CustomException(ExceptionGroup.Validation, "VALIDATION_001", $"Property {propertyName} is null or empty.");
        }
    }
}
