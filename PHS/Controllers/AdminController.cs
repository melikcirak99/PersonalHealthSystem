using PHS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PHS.Controllers
{
    public class AdminController : Controller
    {
        VeriTabani db = new VeriTabani();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["admin"] != null)
            {
                var model = db.tbl_Kisiler.ToList();
                ViewBag.randevular = db.tbl_Randevular.ToList();
                ViewBag.hastaliklar = db.tbl_Hastaliklar.ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("AdminGiris");
            }
        }

        public ActionResult AdminGiris()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminGiris(tbl_admin admin)
        {
            if (ModelState.IsValid)
            {
                var adminSorgu = db.tbl_admin.Where(x => x.password == admin.password && x.username == admin.username).FirstOrDefault();
                if (adminSorgu != null)
                {
                    Session["admin"] = adminSorgu.id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.mesaj = "Kullanıcı adı ya da Şifre yanlış";
                    return View();
                }

            }
            else
            {
                ViewBag.mesaj = "Geçersiz giriş denemesi";
                return View();
            }

        }

        public ActionResult Profil(int id)
        {
            if (Session["admin"] != null)
            {
                tbl_admin sorgu = db.tbl_admin.Where(x => x.id == id).FirstOrDefault();
                if (sorgu == null)
                {
                    ViewBag.error = "hata oluştu";
                    return View();
                }
                else
                {
                    return View(sorgu);
                }
            }
            else
            {
                return RedirectToAction("AdminGiris");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profil([Bind(Include = "id,username,password")] tbl_admin tbl_admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }

        }

        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Kisiler tbl_Kisiler = db.tbl_Kisiler.Find(id);
            if (tbl_Kisiler == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Kisiler);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(tbl_Kisiler tbl_Kisiler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Kisiler).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.mesaj = "1";
                return View();
            }
            else
            {
                ViewBag.mesaj = "0";
                return View(tbl_Kisiler);
            }

        }

        // GET: Kullanici/Details/5
        public ActionResult UserDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Kisiler tbl_Kisiler = db.tbl_Kisiler.Find(id);
            if (tbl_Kisiler == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Kisiler);
        }

        public ActionResult DeleteUser(int? id)
        {
            tbl_Kisiler tbl_Kisiler = db.tbl_Kisiler.Find(id);
            db.tbl_Kisiler.Remove(tbl_Kisiler);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult FindUser(string search)
        {
            if (ModelState.IsValid)
            {
                var _kisi = db.tbl_Kisiler.Where(x => x.kisi_eposta.Contains(search)
                || x.kisi_isimSoyisim.Contains(search)
                || x.kisi_il.Contains(search)).ToList();

                return View("Index", _kisi);
            }
            else
            {
                ViewBag.hatamesaj = "hata";
                return View("Index");
            }


        }
        public ActionResult AdminCikis()
        {
            Session.Clear();
            return RedirectToAction("AdminGiris");
        }

    }
}