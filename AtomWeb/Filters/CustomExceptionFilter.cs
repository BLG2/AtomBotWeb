using AtomWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AtomWeb.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _environment;
        public CustomExceptionFilter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public override void OnException(ExceptionContext context)
        {
            var ViewDataModel = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) {
                { "Title", "Error"},
                { "Message", context.Exception.Message }
            };
            context.Result = new ViewResult { ViewName = "CustomError" , ViewData = ViewDataModel };
       }


    }
}
