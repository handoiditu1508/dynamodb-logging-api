using Lollipop.Models.Common;

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
                error = new SimpleError
                {
                    Code = customException.Code,
                    Message = customException.Message,
                    Group = customException.Group
                };
            }
            else
            {
                var customException = CustomException.System.UnexpectedError();
                error = customException.ToSimpleError();
            }

            return error;
        }
    }
}
