using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MABEE_MVC.Controllers
{
    public class MabeeController : Controller
    {
        // GET: Mabee
        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return   View();
        }
    }
}