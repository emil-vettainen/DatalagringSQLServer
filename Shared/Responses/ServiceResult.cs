using Shared.Enums;
using Shared.Interfaces;

namespace Shared.Responses;

public class ServiceResult : IServiceResult
{
    public object Result { get; set; } = null!;

    public ResultStatus Status { get; set; }



}
