using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PHS.Models;

namespace PHS.Controllers
{
    public class KullaniciController : Controller
    {
        private VeriTabani db = new VeriTabani();



        public ActionResult Onayla()
        {
            if(RouteData.Values["id"] != null)
            {
                string kod = RouteData.Values["id"].ToString();
                try
                {
                    tbl_Kisiler kisi = db.tbl_Kisiler.Where(x => x.kisi_kod1 + x.kisi_kod2 == kod).FirstOrDefault();
                    kisi.isOnay = true;
                    db.SaveChanges();
                    ViewBag.success = 1;
                    ViewBag.OnayMesaj = "Aktivasyon Başarılı Lütfen giriş yapın..";
                }
                catch
                {
                    ViewBag.success = 0;
                    ViewBag.OnayMesaj = "Hata! geçersiz link..";
                }
                
               
            }
            return View();
        }

        public ActionResult SifreYenile()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SifreYenile(tbl_Kisiler _kisi)
        {
            if (ModelState.IsValid)
            {
                var kisi = db.tbl_Kisiler.Where(x => x.kisi_eposta == _kisi.kisi_eposta).FirstOrDefault();
                if(kisi != null)
                {
                    kisi.kisi_kod1 = DateTime.Now.Millisecond.ToString();
                    kisi.kisi_kod2 = Guid.NewGuid().ToString();
                    db.SaveChanges();

                    string NameSurname = kisi.kisi_isimSoyisim.ToString();
                    var body = new StringBuilder();
                    body.AppendLine("sayın " + NameSurname +" şifrenizi değiştirmek için aşağıdaki linle tıklayın..");
                    body.AppendLine("");
                    body.AppendLine("<a href="+"http://192.168.1.30/Kullanici/YeniSifre/" + kisi.kisi_kod1 + kisi.kisi_kod2 +">Şifremi Yenile</a>");
                    Mail.MailSender(body.ToString(), kisi.kisi_eposta, "Şifre Değiştirme Talebi");

                    string Kayitmesaj = kisi.kisi_eposta + " adresine mail gönderildi lütfen postanızı kontrol edin..";
                    ViewBag.GonderimMesaj = " adresine mail gönderildi lütfen postanızı kontrol edin..";
                    return View();
                }
                else
                {
                    ViewBag.GonderimMesaj = " Bu adres Kayıtlı değil";
                    return View();
                }

               
            }
            else
            {
                return View();
            }

        }

        public ActionResult YeniSifre()
        {
            if (RouteData.Values["id"] != null)
            {
                string kod = RouteData.Values["id"].ToString();
                
                try
                {
                    tbl_Kisiler kisi = db.tbl_Kisiler.Where(x => x.kisi_kod1 + x.kisi_kod2 == kod).FirstOrDefault();
                    Session["id"] = kisi.kisi_id;
                }
                catch
                {
                    ViewBag.HataMesaj = "bu link artık geçerli değil";
                }
                return View();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult YeniSifre(tbl_Kisiler _kisi)
        {
            if (ModelState.IsValid)
            {
                var id = Convert.ToInt32(Session["id"]);
                tbl_Kisiler kisi = db.tbl_Kisiler.Where(x => x.kisi_id == id).FirstOrDefault();
              
                try{
                    kisi.kisi_sifre = _kisi.kisi_sifre;
                    kisi.kisi_kod1 = DateTime.Now.Millisecond.ToString();
                    kisi.kisi_kod2 = Guid.NewGuid().ToString();
                    db.SaveChanges();
                    ViewBag.HataMesaj = "işlem başarılı";
                }
                catch
                {
                    ViewBag.HataMesaj = "Hata! bu link artık geçerli değil.";
                }
               
                Session.Clear();
                return View();
            }
            return View();
        }
        // Onay Maili Gönderme
        public ActionResult OnayMailGonder()
        {
            return View();
        }
        // Onay Maili Gönderme POST
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult OnayMailGonder(tbl_Kisiler _kisi)
        {
            if (ModelState.IsValid)
            {
                var kisi = db.tbl_Kisiler.Where(x => x.kisi_eposta == _kisi.kisi_eposta).FirstOrDefault();
                kisi.kisi_kod1 = DateTime.Now.Millisecond.ToString();
                kisi.kisi_kod2 = Guid.NewGuid().ToString();
                db.SaveChanges();

                var body = new StringBuilder();
                string NameSurname = kisi.kisi_isimSoyisim.ToString();
                body.AppendLine("sayın " + NameSurname + " E postanızı onaylamak için aşağıdaki linke tıkalyın..");
                body.AppendLine("");
                body.AppendLine("<a href=" + "http://192.168.1.30/Kullanici/Onayla/" + kisi.kisi_kod1 + kisi.kisi_kod2 + ">Onayla</a>");

                Mail.MailSender(body.ToString(), kisi.kisi_eposta, "Eposta Onay Talebi");

                ViewBag.mesaj = kisi.kisi_eposta + " adresine mail gönderildi lütfen postanızı kontrol edin..";
                ViewBag.GonderimMesaj = " adresine mail gönderildi lütfen postanızı kontrol edin..";
                return View();
            }
            else
            {
                return View();
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
