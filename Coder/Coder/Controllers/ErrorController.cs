using Coder.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Coder.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { StatusCode = (int)HttpStatusCode.Forbidden, Message = "Forbidden" });
        }

        public ActionResult BadRequest()
        {
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { StatusCode = (int)HttpStatusCode.BadRequest, Message = "Bad request" });
        }

        public ActionResult NotFound()
        {
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { StatusCode = (int)HttpStatusCode.NotFound, Message = "Not found" });
        }

        public ActionResult Unauthorized()
        {
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { StatusCode = (int)HttpStatusCode.Unauthorized, Message = "Unautorized" });
        }

        public ActionResult InternalServerError()
        {
            return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { StatusCode = (int)HttpStatusCode.InternalServerError, Message = "Internal Server Error" });
        }
    }
}