using AtomData.Models;
using AtomData.Services;
using AtomWeb.Filters;
using AtomWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace AtomWeb.Services
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CookieService
    {
        public static DiscordLoginTokenModel? GetSignedInAccesToken(HttpContext httpContext)
        {
            try
            {
                if (httpContext != null && httpContext.Request.Cookies["SignedInAs"] != null)
                {
                    var cookie = httpContext.Request.Cookies["SignedInAs"];
                    var decrypted = !string.IsNullOrEmpty(cookie) ? Xor.Decrypt(cookie) : "";
                    if (!string.IsNullOrEmpty(decrypted))
                    {
                        var deSerialized = JsonConvert.DeserializeObject<DiscordLoginTokenModel>(decrypted);
                        return deSerialized;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                httpContext.Response.Cookies.Delete("SignedInAs");
                throw new Exception("Somthing went whrong getting accesToken.");
            }
        }

        public static void SetSignedInAccesToken(HttpContext httpContext, DiscordLoginTokenModel token)
        {
            try
            {
                if (httpContext != null)
                {
                    var serialized = JsonConvert.SerializeObject(token);
                    var encrypted = Xor.Encrypt(serialized);
                    httpContext.Response.Cookies.Append("SignedInAs", encrypted, new CookieOptions { Expires = DateTime.Now.AddHours(3) });
                }

            }
            catch (Exception)
            {
                httpContext.Response.Cookies.Delete("SignedInAs");
                throw new Exception("Somthing went whrong saving accesToken.");
            }
        }


        /**/
        public static string? GetTheme(HttpContext? httpContext)
        {
            try
            {
                if (httpContext != null && httpContext.Request.Cookies["Theme"] != null)
                {
                    var cookie = httpContext.Request.Cookies["Theme"];
                    if (!string.IsNullOrEmpty(cookie))
                    {
                        return cookie;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                httpContext.Response.Cookies.Delete("Theme");
            }
            return null;
        }

        public static void SetTheme(HttpContext httpContext, string theme)
        {
            try
            {
                if (httpContext != null)
                    httpContext.Response.Cookies.Append("Theme", theme, new CookieOptions { Expires = DateTime.Now.AddDays(365) });
            }
            catch (Exception)
            {
                httpContext.Response.Cookies.Delete("Theme");
            }
        }


    }
}
