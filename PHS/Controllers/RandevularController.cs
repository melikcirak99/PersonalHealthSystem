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
    public class RandevularController : Controller
    {
        private VeriTabani db = new VeriTabani();

        // GET: Randevular
        public ActionResult Index()
        {
            var tbl_Randevular = db.tbl_Randevular.Include(t => t.tbl_Kisiler);
            return View(tbl_Randevular.ToList());
        }

        // GET: Randevular/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Randevular tbl_Randevular = db.tbl_Randevular.Find(id);
            if (tbl_Randevular == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Randevular);
        }

        // GET: Randevular/Create
        public ActionResult Create()
        {
            ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim");
            return View();
        }

        // POST: Randevular/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "randevu_id,randevu_hastahane,randevu_doktor,randevu_klinik,randevu_tarih,kisi_id")] tbl_Randevular tbl_Randevular)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Randevular.Add(tbl_Randevular);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Randevular.kisi_id);
            return View(tbl_Randevular);
        }

        // GET: Randevular/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Randevular tbl_Randevular = db.tbl_Randevular.Find(id);
            if (tbl_Randevular == null)
            {
                return HttpNotFound();
            }
            ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Randevular.kisi_id);
            return View(tbl_Randevular);
        }

        // POST: Randevular/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "randevu_id,randevu_hastahane,randevu_doktor,randevu_klinik,randevu_tarih,kisi_id")] tbl_Randevular tbl_Randevular)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Randevular).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.message = "Başarı ile Kaydedildi";
                return View();
            }
            ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Randevular.kisi_id);
            return View(tbl_Randevular);
        }

        // GET: Randevular/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Randevular tbl_Randevular = db.tbl_Randevular.Find(id);
            if (tbl_Randevular == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Randevular);
        }

        // POST: Randevular/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Randevular tbl_Randevular = db.tbl_Randevular.Find(id);
            db.tbl_Randevular.Remove(tbl_Randevular);
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
