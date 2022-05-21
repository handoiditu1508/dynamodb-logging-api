using Lollipop.Models.Common;

namespace Lollipop.Helpers.Extensions
{
    public static class ExceptionExtension
    {
        public static SimpleError ToSimpleError(this Exception exception)
        {
            SimpleError error;
            CustomException customException;

            if (exception.GetType() == typeof(CustomException))
            {
                customException = (CustomException)exception;
            }
            else customException = CustomException.System.UnexpectedError();

            error = new SimpleError
            {
                Code = customException.Code,
                Message = customException.Message,
                Group = customException.Group
            };

            return error;
        }
    }
}
