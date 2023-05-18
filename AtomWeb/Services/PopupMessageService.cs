using AtomData.Services;
using AtomWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AtomWeb.Services
{
    public class PopupMessageService
    {
        public static NotifyVM? GetPupupMessage(Controller controller)
        {
            var notify = new NotifyVM { NotifyMessage=""};
            var httpContext = controller.HttpContext;
            var controllerName = controller.ControllerContext.ActionDescriptor;
            var sessionNotify = httpContext.Session.GetString(controllerName.ControllerName) ?? null;
            if (!string.IsNullOrEmpty(sessionNotify)) notify = JsonConvert.DeserializeObject<NotifyVM>(Xor.Decrypt(sessionNotify));
            httpContext.Session.Remove(controllerName.ControllerName);
            return notify;
        }

        public static void SetPupupMessage(Controller controller, NotifyVM notify, string? controllerNameOptional = null)
        {
            var httpContext = controller.HttpContext;
            var controllerName = !string.IsNullOrEmpty(controllerNameOptional) ? controllerNameOptional : controller.ControllerContext.ActionDescriptor.ControllerName;
            httpContext.Session.SetString(controllerName, Xor.Encrypt(JsonConvert.SerializeObject(notify)));
        }
    }
}
