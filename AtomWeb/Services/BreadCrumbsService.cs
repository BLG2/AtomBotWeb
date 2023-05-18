using AtomData.Services;
using AtomWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AtomWeb.Services
{
    public class BreadCrumbsService
    {

        public static List<BreadCrumb> AddBreadCrumbAsync(Controller controller, string? breadcrumbName = null)
        {
            var uri = controller.Request.GetEncodedPathAndQuery();
            var httpContext = controller.HttpContext;
            var controllerInfo = controller.ControllerContext.ActionDescriptor;
            var sessionBreadCrumb = httpContext.Session.GetString("_BreadCrumb") ?? null;
            var breadCrumb = sessionBreadCrumb != null ? JsonConvert.DeserializeObject<List<BreadCrumb>>(Xor.Decrypt(sessionBreadCrumb)) ?? new List<BreadCrumb>() : new List<BreadCrumb>();
            var newBreadCrumb = new BreadCrumb { Uri = uri, Name = !string.IsNullOrEmpty(breadcrumbName) ? breadcrumbName : controllerInfo.ControllerName };

            if (!string.IsNullOrEmpty(newBreadCrumb.Uri) && !string.IsNullOrEmpty(newBreadCrumb.Name))
            {
                var breadCrumbsToRemove = breadCrumb.Where(x => x.Uri == newBreadCrumb.Uri).ToList();
                foreach (BreadCrumb bc in breadCrumbsToRemove)
                    breadCrumb.Remove(bc);

                breadCrumb.Add(newBreadCrumb);
            }

            httpContext.Session.SetString("_BreadCrumb", Xor.Encrypt(JsonConvert.SerializeObject(breadCrumb)));
            return breadCrumb;
        }


        public static List<BreadCrumb> GetBreadCrumbsAsyc(Controller controller)
        {
            var httpContext = controller.HttpContext;
            var sessionBreadCrumb = httpContext.Session.GetString("_BreadCrumb") ?? null;
            var breadCrumb = sessionBreadCrumb != null ? JsonConvert.DeserializeObject<List<BreadCrumb>>(Xor.Decrypt(sessionBreadCrumb)) ?? new List<BreadCrumb>() : new List<BreadCrumb>();
            return breadCrumb;
        }





    }
}
