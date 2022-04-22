using Lollipop.Models.Common;

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
