using MassTransit;

namespace Core.Log.Helpers;

public static class LogHelper
{
    public static Exception ToException(this ExceptionInfo[] exceptionInfos)
    {
        var exceptionInfo = exceptionInfos.FirstOrDefault();
        return exceptionInfo is null ? new Exception() : new Exception(exceptionInfo.Message);
    }
}