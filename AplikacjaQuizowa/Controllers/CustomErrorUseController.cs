﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AplikacjaQuizowa.Controllers
{
    public class CustomErrorUseController : Controller
    {
        // GET: CustomErrorUse
        [HandleError]
        public ActionResult Index()
        {
            return View();
        }
    }
}