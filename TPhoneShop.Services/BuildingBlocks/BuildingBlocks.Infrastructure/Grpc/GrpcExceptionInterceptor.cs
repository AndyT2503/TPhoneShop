using BuildingBlocks.Application.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Net;

namespace BuildingBlocks.Infrastructure.Grpc
{
    public class GrpcExceptionInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
                                                                                      ServerCallContext context,
                                                                                      UnaryServerMethod<TRequest, TResponse> continuation
        )
        {
            try
            {
                return await continuation(request, context);
            }
            catch (AppException ex)
            {
                throw new RpcException(new Status(ToGrpcStatus(ex.StatusCode), ex.Message));
            }
            catch (Exception)
            {
                throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."));
            }
        }

        private static StatusCode ToGrpcStatus(HttpStatusCode statusCode)
        {
            return statusCode switch
            {
                HttpStatusCode.NotFound => StatusCode.NotFound,
                HttpStatusCode.BadRequest => StatusCode.InvalidArgument,
                HttpStatusCode.Unauthorized => StatusCode.Unauthenticated,
                HttpStatusCode.Forbidden => StatusCode.PermissionDenied,
                _ => StatusCode.Internal
            };
        }
    }
}
