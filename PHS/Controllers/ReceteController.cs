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
    public class ReceteController : Controller
    {
        private VeriTabani db = new VeriTabani();

        // GET: Recete
        public ActionResult Index()
        {
            if(Session["kullanici"] != null)
            {
                int _id = Convert.ToInt32(Session["kisiID"]);
                var recete = db.tbl_Recete.Where(x => x.kisi_id == _id).ToList();
                return View(recete);
            }
            else
            {
                return Redirect("/Home/Login");
            }
           
        }

        // GET: Recete/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["kullanici"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tbl_Recete tbl_Recete = db.tbl_Recete.Find(id);
                if (tbl_Recete == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    ViewBag.ilaclar = db.tbl_Ilac.Where(x => x.recete_id == id);
                    return View(tbl_Recete);
                }
               
            }
            else
            {
                return Redirect("/Home/Login");
            }
           
        }

        // GET: Recete/Create
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

        // POST: Recete/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "recete_id,kisi_id,recete_no,recete_tarih,recete_tur,recete_hekim")] tbl_Recete tbl_Recete)
        {
            if (Session["kullanici"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.tbl_Recete.Add(tbl_Recete);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Recete.kisi_id);
                return View(tbl_Recete);
            }
            else
            {
                return Redirect("/Home/Login");
            }         
        }


        // GET: Recete/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["kullanici"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tbl_Recete tbl_Recete = db.tbl_Recete.Find(id);
                if (tbl_Recete == null)
                {
                    return HttpNotFound();
                }
                ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Recete.kisi_id);
                return View(tbl_Recete);
            }
            else
            {
                return Redirect("/Home/Login");
            }
           
        }

        // POST: Recete/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "recete_id,kisi_id,recete_no,recete_tarih,recete_tur,recete_hekim")] tbl_Recete tbl_Recete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Recete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kisi_id = new SelectList(db.tbl_Kisiler, "kisi_id", "kisi_isimSoyisim", tbl_Recete.kisi_id);
            return View(tbl_Recete);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if(Session["kullanici"] != null)
            {
                int _id = id;
                tbl_Recete recete = db.tbl_Recete.Find(_id);
                db.tbl_Recete.Remove(recete);
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
