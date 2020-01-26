using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views
{
    public class ServerController : Controller
    {
        // GET: Server
        public ActionResult Index()
        {
            return View();
        }

        // GET: Server/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Server/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Skill()
        {
            return View();
        }

        // POST: Server/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Server/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Server/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Server/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Server/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}