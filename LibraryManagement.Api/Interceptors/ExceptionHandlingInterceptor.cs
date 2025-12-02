using FluentValidation;
using Grpc.Core;
using Grpc.Core.Interceptors;
using LibraryManagement.Shared.Exceptions;
using Serilog;

namespace LibraryManagement.Api.Interceptors;

public class ExceptionHandlingInterceptor : Interceptor
{
    private readonly Serilog.ILogger _logger;
    public ExceptionHandlingInterceptor()
    {
        _logger = Log.Logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var methodName = context.Method;
        try
        {
            _logger.Information("Starting gRPC call: {Method}", methodName);

            var response = await continuation(request, context);

            _logger.Information("Completed gRPC call: {Method}", methodName);

            return response;
        }
        catch (RpcException rpcExc)
        {
            _logger.Warning("RpcException in {Method}: {StatusCode} - {Message}",
                methodName, rpcExc.StatusCode, rpcExc.Message);
            throw;
        }
        catch (Exception exc)
        {
            _logger.Error(exc, "Unhandled exception in gRPC call: {Method}", methodName);

            throw new RpcException(MapExceptionToGrpcStatus(exc));
        }
    }

    private static Status MapExceptionToGrpcStatus(Exception exception)
    {
        return exception switch
        {
            ValidationException => new Status(StatusCode.InvalidArgument, exception.Message),
            NotFoundException => new Status(StatusCode.NotFound, exception.Message),
            NotAvailableException => new Status(StatusCode.Unavailable, exception.Message),
            NotImplementedException => new Status(StatusCode.Unimplemented, exception.Message),
            _ => new Status(StatusCode.Unknown, $"Unknown error: {exception.Message}")
        };
    }
}
