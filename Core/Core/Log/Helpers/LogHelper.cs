using MassTransit;

namespace Core.Log.Helpers;

public static class LogHelper
{
    public static Exception ToException(this ExceptionInfo[] exceptionInfos)
    {
        var exceptionInfo = exceptionInfos.FirstOrDefault();
        if (exceptionInfo is null)
            return new Exception();

        return new Exception(exceptionInfo.Message);
    }
}