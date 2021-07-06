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
    public class HastaliklarController : Controller
    {
        private VeriTabani db = new VeriTabani();

        // GET: Hastaliklar
        public ActionResult Index()
        {
            if(Session["kullanici"] != null)
            {
                int _id = Convert.ToInt32(Session["kisiId"]);
                var tbl_Hastaliklar = db.tbl_Hastaliklar.Where(t => t.tbl_Kisiler.kisi_id == _id).ToList();
                return View(tbl_Hastaliklar);
            }
            else
            {
                return Redirect("/Home/Login");
            }
        }

        // GET: Hastaliklar/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["kullanici"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tbl_Hastaliklar tbl_Hastaliklar = db.tbl_Hastaliklar.Find(id);
                if (tbl_Hastaliklar == null)
                {
                    return HttpNotFound();
                }
                return View(tbl_Hastaliklar);
            }
            else
            {
                return Redirect("/Home/Login");
            }
        }

        // GET: Hastaliklar/Create
        public ActionResult Create()
        {
            if (Session["kullanici"] != null)
            {
                ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim");
                return View();
            }
            else
            {
                return Redirect("/Home/Login");
            }
           
        }

        // POST: Hastaliklar/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "hastalik_id,kisi_id,hastalik_tani,hastalik_tarih,hastalik_klinik,hastalik_hekim")] tbl_Hastaliklar tbl_Hastaliklar)
        {
                if (ModelState.IsValid)
                {
                    db.tbl_Hastaliklar.Add(tbl_Hastaliklar);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Hastaliklar.kisi_id);
                return View(tbl_Hastaliklar);
            
        }

        // GET: Hastaliklar/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["kullanici"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tbl_Hastaliklar tbl_Hastaliklar = db.tbl_Hastaliklar.Find(id);
                if (tbl_Hastaliklar == null)
                {
                    return HttpNotFound();
                }
                ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Hastaliklar.kisi_id);
                return View(tbl_Hastaliklar);
            }
            else
            {
                return Redirect("/Home/Login");
            }

           
        }

        // POST: Hastaliklar/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "hastalik_id,kisi_id,hastalik_tani,hastalik_tarih,hastalik_klinik,hastalik_hekim")] tbl_Hastaliklar tbl_Hastaliklar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Hastaliklar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Hastaliklar.kisi_id);
            return View(tbl_Hastaliklar);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["kullanici"] != null)
            {
                int _id = id;
                tbl_Hastaliklar hastalik = db.tbl_Hastaliklar.Find(_id);
                db.tbl_Hastaliklar.Remove(hastalik);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect("/Home/Login");
            }
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
