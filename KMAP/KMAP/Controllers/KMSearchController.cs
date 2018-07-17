using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KMAP.Models;

namespace KMAP.Controllers
{
    public class KMSearchController : Controller
    {
        // GET: KMSearch
        public ActionResult Index()
        {
            return View();
        }

        // GET: KMAdvSearch
        public ActionResult AdvSearch(string advkeyword, string folderId, string userId)
        {
            try
            {
                AdvanceSearch adv = new AdvanceSearch(advkeyword, folderId, userId);
                string result = adv.GetResultTXT();
                ViewBag.ResultText = result;
                ViewBag.DatumClassList = adv.datumClassList;
            }
            catch(Exception ex)
            {
                string errormsg = ex.Message;
                ViewBag.ResultText = errormsg;
            }
            return View();
        }

        public ActionResult AdvSearchDocClass(string docclass, string docclassvalue, string advkeyword, string folderId, string userId)
        {
            try
            {
                AdvanceSearch adv = new AdvanceSearch(advkeyword, folderId, userId);
                adv.AdvanceSearchDocClass(docclass, docclassvalue, advkeyword, folderId, userId);
                string result = adv.GetResultDocClassTXT();
                ViewBag.ResultText = result;
                ViewBag.DatumClassList = adv.datumClassList;
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message;
                ViewBag.ResultText = errormsg;
            }
            return View();
        }

        // GET: KMSearch/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KMSearch/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KMSearch/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: KMSearch/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KMSearch/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: KMSearch/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KMSearch/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
