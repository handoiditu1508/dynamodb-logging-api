using Lollipop.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lollipop.Helpers.Extensions
{
    public static class ExceptionExtension
    {
        public static SimpleError ToSimpleError(this Exception exception)
        {
            SimpleError error;

            if (exception.GetType() == typeof(CustomException))
            {
                var customException = (CustomException)exception;
                error = customException.ToSimpleError();
            }
            else
            {
                var customException = CustomException.System.UnexpectedError;
                error = customException.ToSimpleError();
            }

            return error;
        }
    }
}
