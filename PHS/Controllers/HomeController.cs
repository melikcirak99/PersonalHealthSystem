using PHS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PHS.Controllers
{
    public class HomeController : Controller
    {
        VeriTabani db = new VeriTabani();
        // GET: Home
        public ActionResult Index()
        {
            if(Session["kullanici"] != null)
            {
                var _eposta = Session["kullanici"].ToString();
                try
                {
                    tbl_Kisiler _kisi = db.tbl_Kisiler.Where(x => x.kisi_eposta == _eposta).FirstOrDefault();
                    if (_kisi.isOnay == true)
                    {
                        ViewModel viewModel = new ViewModel();
                        viewModel.Kisiler = db.tbl_Kisiler.Where(x => x.kisi_eposta == _eposta).ToList();
                        viewModel.Randevular = db.tbl_Randevular.Where(x => x.tbl_Kisiler.kisi_eposta == _eposta).ToList();
                        return View(viewModel);
                    }
                    else
                    {
                        return RedirectToAction("Login", new { hata = " hesabınız onaylı değil lütfen onaylayın.. " });
                    }
                }
                catch
                {
                    return RedirectToAction("Login");
                }
            }

            else
            {
                return RedirectToAction("Login");
            }
          
        }


        public ActionResult Cikis()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login(string hata, string KayitMsg)
        {
            ViewBag.hata = hata;
            ViewBag.Kayitmesaj = KayitMsg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tbl_Kisiler _kullanici)
        {
            if (ModelState.IsValid)
            {
                var kayit = db.tbl_Kisiler.Where(x => x.kisi_eposta == _kullanici.kisi_eposta && x.kisi_sifre == _kullanici.kisi_sifre);
                if (kayit.Count() > 0)
                {
                    //giriş başarılı
                    Session["kullanici"] = kayit.FirstOrDefault().kisi_eposta;
                    Session["kisiID"] = kayit.FirstOrDefault().kisi_id.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    //giriş başarısız
                    string hataMesaji = "Kullanıcı adı yada Şifre hatalı";
                    return RedirectToAction("Login", new { hata = hataMesaji });
                }

            }
            return View();
        }

        // GET: Kullanici/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Kullanici/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "kisi_id,kisi_isimSoyisim,kisi_telefon,kisi_eposta,kisi_sifre,kisi_adresSatiri1,kisi_adresSatiri2,kisi_postaKodu,kisi_il,kisi_acilTelNo,kisi_boy,kisi_kilo,kisi_dogumTarihi,kisi_cinsiyet,kisi_isAktiv,kisi_isHamile,kisi_meslek,kişi_hastaliklari,isOnay,kisi_kod1,kisi_kod2")] tbl_Kisiler tbl_Kisiler)
        {
            if (ModelState.IsValid)
            {
                if (db.tbl_Kisiler.Where(x => x.kisi_eposta == tbl_Kisiler.kisi_eposta).Any())
                {
                    ViewBag.Kayitmesaj = "Bu eposta ile daha önce kayıt yapılmış.. Lütfen başka bir tane deneyin";
                    return View("Register");
                }
                else
                {
                    tbl_Kisiler.kisi_kod1 = DateTime.Now.Millisecond.ToString();
                    tbl_Kisiler.kisi_kod2 = Guid.NewGuid().ToString();
                    tbl_Kisiler.isOnay = false;
                    db.tbl_Kisiler.Add(tbl_Kisiler);
                    db.SaveChanges();

                    var body = new StringBuilder();
                    body.AppendLine("hesabınızı etkinleştirmek için aşağıdaki linke tıklayın.");
                    body.AppendLine("");
                    body.AppendLine("<a href=" + "http://192.168.1.30/Kullanici/Onayla/"+ tbl_Kisiler.kisi_kod1 + tbl_Kisiler.kisi_kod2 + ">Tıklayın<a/>");                 
                    Mail.MailSender(body.ToString(), tbl_Kisiler.kisi_eposta, "E posta Onay Talebi");
                    string Kayitmesaj = tbl_Kisiler.kisi_eposta + " adresine bir doğrulama maili gönderildi lütfen postanızı kontrol edin..";
                    return RedirectToAction("Login", new { KayitMsg = Kayitmesaj });
                }
            }
            return View(tbl_Kisiler);
        }

        [HttpGet]
        public ActionResult Profil()
        {
            if(Session["kullanici"] != null)
            {
                int _id = Convert.ToInt32(Session["kisiId"]);
                tbl_Kisiler kisi = db.tbl_Kisiler.Find(_id);
                if (kisi == null)
                {
                    return HttpNotFound();
                }
                return View(kisi);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profil([Bind(Include = "kisi_id,kisi_isimSoyisim,kisi_telefon,kisi_eposta,kisi_sifre,kisi_adresSatiri1,kisi_adresSatiri2,kisi_postaKodu,kisi_il,kisi_acilTelNo,kisi_boy,kisi_kilo,kisi_dogumTarihi,kisi_cinsiyet,kisi_isAktiv,kisi_isHamile,kisi_meslek,kişi_hastaliklari,isOnay,kisi_kod1,kisi_kod2")] tbl_Kisiler tbl_Kisiler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Kisiler).State = EntityState.Modified;
                int id = tbl_Kisiler.kisi_id;
                var kisi = db.tbl_Kisiler.Find(id);
                kisi.isOnay = true;
                db.SaveChanges();
                ViewBag.message = new List<string> { "1", "kayıt başarılı" };
                return View(tbl_Kisiler);
            }
            else
            {
                ViewBag.message = new List<string> { "0", "bir hata oluştu" };
                return View(tbl_Kisiler);
            }
        }
    }
}