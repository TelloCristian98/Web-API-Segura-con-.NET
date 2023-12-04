using Azure;
using System.Net;
using System.Text.Json;
using WebAPI.Exceptions;
namespace WebAPI.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "aplication/json";
                //var responseModel = new Response<string>() { Succeded = false, Message = ex?.Message };
                switch (ex)
                {
                    case ApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;

                }
                var result = JsonSerializer.Serialize(response);
                await response.WriteAsync(result);
            }
        }
    }
}
