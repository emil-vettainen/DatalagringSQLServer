using Shared.Enums;

namespace Shared.Interfaces
{
    public interface IServiceResult
    {
        object Result { get; set; }
        ResultStatus Status { get; set; }
    }
}