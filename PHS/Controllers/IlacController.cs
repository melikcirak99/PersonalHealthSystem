using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PHS.Models;

namespace PHS.Controllers
{
    public class IlacController : Controller
    {
        private VeriTabani db = new VeriTabani();

        // GET: Ilac
        public ActionResult Index()
        {
            var tbl_Ilac = db.tbl_Ilac.Include(t => t.tbl_Recete);
            return View(tbl_Ilac.ToList());
        }

        // GET: Ilac/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Ilac tbl_Ilac = db.tbl_Ilac.Find(id);
            if (tbl_Ilac == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Ilac);
        }

        // GET: Ilac/Create
        public ActionResult Create()
        {
            ViewBag.recete_id = new SelectList(db.tbl_Recete, "recete_id", "recete_id");
            return View();
        }

        // POST: Ilac/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ilac_id,recete_id,ilan_name,ilac_doz")] tbl_Ilac tbl_Ilac)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Ilac.Add(tbl_Ilac);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.recete_id = new SelectList(db.tbl_Recete, "recete_id", "recete_id", tbl_Ilac.recete_id);
            return View(tbl_Ilac);
        }

        // GET: Ilac/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Ilac tbl_Ilac = db.tbl_Ilac.Find(id);
            if (tbl_Ilac == null)
            {
                return HttpNotFound();
            }
            ViewBag.recete_id = new SelectList(db.tbl_Recete, "recete_id", "recete_id", tbl_Ilac.recete_id);
            return View(tbl_Ilac);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ilac_id,recete_id,ilan_name,ilac_doz")] tbl_Ilac tbl_Ilac)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Ilac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.recete_id = new SelectList(db.tbl_Recete, "recete_id", "recete_id", tbl_Ilac.recete_id);
            return View(tbl_Ilac);
        }

        // GET: Ilac/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Ilac tbl_Ilac = db.tbl_Ilac.Find(id);
            if (tbl_Ilac == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Ilac);
        }

        // POST: Ilac/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Ilac tbl_Ilac = db.tbl_Ilac.Find(id);
            db.tbl_Ilac.Remove(tbl_Ilac);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
