using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrulySkilled.Web.Controllers
{
    public class TicTacToeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
