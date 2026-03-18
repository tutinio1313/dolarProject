using System.Net;
using backend.Helper;
using Microsoft.AspNetCore.Http;

namespace backend.Middleware;

public class RequestAuthenticationMiddleware(RequestDelegate requestDelegate, ITokenHelper tokenHelper)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if(context.Request.Cookies.Count > 0)
        {
            bool hasValidCookie = false;
            foreach(KeyValuePair<string,string> cookie in context.Request.Cookies)
            {
                if(Equals(cookie.Value, null) || string.IsNullOrEmpty(cookie.Value))
                {
                    continue;
                }

                bool result = await tokenHelper.ValidateTokenAsync(cookie.Value);
                if(result)
                {
                    hasValidCookie = true;
                    break;
                }
            }

            if(hasValidCookie)
            {
                await requestDelegate(context);
            }
            else
            {
                context = await setUnauthorized(context);
                return;
            }
        }
        else
        {
            context = await setUnauthorized(context);
            return;
        }
    }

    private async Task<HttpContext> setUnauthorized(HttpContext context)
    {
            context.Response.Clear();
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            context.Response.ContentType = "text/`plain";
            await context.Response.WriteAsJsonAsync("The user is unauthorized.");

            return context;
    }
}