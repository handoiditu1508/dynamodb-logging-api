using Lollipop.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lollipop.Helpers.Extensions
{
    public static class CustomExceptionExtension
    {
        public static SimpleError ToSimpleError(this CustomException exception)
        {
            return new SimpleError
            {
                Code = exception.Code,
                Group = exception.Group,
                Message = exception.Message
            };
        }
    }
}
