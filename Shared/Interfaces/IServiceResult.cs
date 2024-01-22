using Shared.Enums;

namespace Shared.Responses
{
    public interface IServiceResult
    {
        object Result { get; set; }
        ResultStatus Status { get; set; }
    }
}