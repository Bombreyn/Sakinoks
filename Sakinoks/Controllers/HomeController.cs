using System;
using System.Linq;
using System.Web.Mvc;
using Sakinoks.Models;
using System.Web.Security; //FormsAuthentication kullanmak için
using Sakinoks.Ayar;
using System.IO;



namespace Sakinoks.Controllers
{
    public class HomeController : Controller
    {
        VortexContext db = new VortexContext();

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewModel vw = new ViewModel()
                {
                    uruns = db.Urun.ToList(),
                    kategorilers = db.Kategoriler.ToList(),
                    siteAyars = db.siteAyar.ToList()
                };
                return View(vw);

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

        public ActionResult Login()
        {
            var siteisim = db.siteAyar.FirstOrDefault();
            Session["siteisim"] = siteisim.site_adi;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // kullanıcılardan veya yöneticilerden gelen isteklere (request) isteklerin doğruluğunu Tokenler aracılığıyla anlayıp ona göre cevap verir
        public ActionResult AdminLogin(Admin form)
        {
            ViewModel vw = new ViewModel();

            if (!ModelState.IsValid)  // box lar doğru validation şekilde doldurulmasını kontrol ediyor.
            {
                return View("Login", form);
            }


            using (VortexContext db = new VortexContext())
            {

                var AdminVarmi = db.Admin.FirstOrDefault(
                    x => x.adi_soyadi == form.adi_soyadi && x.sifre == form.sifre); //kullanıcı olup olmadığını kontrol ediyor.

                var siteisim = db.siteAyar.FirstOrDefault();
                if (AdminVarmi != null)
                {
                    FormsAuthentication.SetAuthCookie(AdminVarmi.adi_soyadi, false); //kullanıcı kayıtlı kalıncaksa true olacak. (cookie)
                    Session["siteisim"] = siteisim.site_adi;
                    Proje.Aktif.adminData = AdminVarmi;
                    return RedirectToAction("Urunler", "Home");
                }


                ViewBag.Hata = "kullanıcı adı veya şifre hatalı"; //hata mesajı
                return View("Login"); // kullanıcı yok ise geri login sayfasına yönlendiriyor.

            }
        } //login post işlemleri ve kontrol..

        [HttpGet]
        [Authorize]
        public ActionResult Urunler()
        {

            try
            {
                ViewModel vw = new ViewModel()
                {

                    uruns = db.Urun.ToList(),
                    kategorilers = db.Kategoriler.ToList(),
                    admins = db.Admin.ToList()

                };
                return View(vw);

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }



        }



        [HttpPost]
        [Authorize]
        public ActionResult UrunDuzenle(int urunID, string isim, string aciklama, string urunkodu, string kod, int kategori, Urun resim, bool aktif)
        {


            Urun urun = db.Urun.Where(x => x.urunID == urunID).FirstOrDefault();

            urun.isim = isim;
            urun.aciklama = aciklama;
            urun.urunkodu = urunkodu;
            urun.kod = kod;
            urun.kategoriID = kategori;
            urun.tarih = DateTime.Now;
            urun.aktif = aktif;
            urun.adminID = Proje.Aktif.adminData.adminID;

            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string yol = "~/Content/theme/img/urunler/" + dosyaadi;
                if (yol == "~/Content/theme/img/urunler/")
                {

                }
                else
                {
                    Request.Files[0].SaveAs(Server.MapPath(yol));
                    urun.resim = "/Content/theme/img/urunler/" + dosyaadi;
                }




            }



            db.SaveChanges();
            TempData["UrunGuncellendi"] = "Ürün Güncellendi";
            return RedirectToAction("Urunler");
        }

        [Authorize]
        public ActionResult UrunEkle()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewModel vw = new ViewModel();

            vw.kategorilers = db.Kategoriler.ToList();

            return View(vw);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UrunEkle(string isim, string aciklama, string urunkodu, string kod, int kategori, Urun resim, bool aktif)
        {

            ViewModel vw = new ViewModel();
            vw.kategorilers = db.Kategoriler.ToList();

            var etkin = db.Urun.Where(x => x.urunkodu == urunkodu || x.isim == isim).SingleOrDefault();
            if (etkin != null)
            {
                TempData["UrunMevcut"] = "UrunMevcut";
                return View(vw);
            }

            if (Proje.Aktif != null)
            {

                Urun u = new Urun()

                {
                    isim = isim,
                    aciklama = aciklama,
                    urunkodu = urunkodu,
                    kod = kod,
                    kategoriID = kategori,
                    aktif = aktif,
                    tarih = DateTime.Now,
                    adminID = Proje.Aktif.adminData.adminID

                };

                if (Request.Files.Count > 0)
                {
                    string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                    string yol = "~/Content/theme/img/urunler/" + dosyaadi;

                    if (yol == "~/Content/theme/img/urunler/")
                    {
                        TempData["ResimHata"] = "ResimHata";
                    }
                    else
                    {
                        Request.Files[0].SaveAs(Server.MapPath(yol));
                        u.resim = "/Content/theme/img/urunler/" + dosyaadi;
                        db.Urun.Add(u);
                        db.SaveChanges();
                        TempData["UrunEklendi"] = "Ürün Eklendi";
                        return View(vw);
                    }


                }


            }
            else
            {
                return RedirectToAction("Login");
            }


            return View(vw);

        }

        [Authorize]
        public ActionResult Kategori()
        {

            ViewModel vw = new ViewModel();

            vw.kategorilers = db.Kategoriler.ToList();

            return View(vw);
        }

        [Authorize]
        public ActionResult KategoriSil(int kategoriID)
        {
            using (VortexContext db = new VortexContext())
            {
                var silicnecekKategori = db.Kategoriler.Find(kategoriID);
                if (silicnecekKategori == null)
                {
                    return HttpNotFound();
                }
                db.Kategoriler.Remove(silicnecekKategori);
                db.SaveChanges();
                TempData["KategoriSilindi"] = "Kategori Silindi";
                return RedirectToAction("Kategori");

            }
        }

        [Authorize]
        public ActionResult KategoriEkle()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login");
            }

            ViewModel vw = new ViewModel();

            vw.kategorilers = db.Kategoriler.ToList();

            return View(vw);
        }

        [Authorize]
        [HttpPost]
        public ActionResult KategoriEkle(string adi, string etiket, bool aktif)
        {

            ViewModel vw = new ViewModel();
            vw.kategorilers = db.Kategoriler.ToList();

            var etkin = db.Kategoriler.Where(x => x.adi == adi || x.etiket == etiket).SingleOrDefault();
            if (etkin != null)
            {
                TempData["KategoriMevcut"] = "KategoriMevcut";
                return View(vw);
            }

            if (Proje.Aktif != null)
            {

                Kategoriler k = new Kategoriler()

                {
                    adi = adi,
                    etiket = etiket,
                    aktif = aktif,

                };


                db.Kategoriler.Add(k);
                db.SaveChanges();
                TempData["KategoriEklendi"] = "Kategori Eklendi";
                return View(vw);




            }
            else
            {
                return RedirectToAction("Login");
            }



        }

        public ActionResult KategoriDuzenle(int kategoriID, string adi, string etiket, bool aktif)
        {
            Kategoriler kategori = db.Kategoriler.Where(x => x.kategoriID == kategoriID).FirstOrDefault();

            kategori.adi = adi;
            kategori.etiket = etiket;
            kategori.aktif = aktif;

            db.SaveChanges();
            TempData["KategoriGuncellendi"] = "Kategori Guncellendi";
            return RedirectToAction("Kategori");

        }

        [Authorize]
        public ActionResult Sil(int urunID)
        {
            using (VortexContext db = new VortexContext())
            {
                var silicnecekurun = db.Urun.Find(urunID);
                if (silicnecekurun == null)
                {
                    return HttpNotFound();
                }
                db.Urun.Remove(silicnecekurun);
                db.SaveChanges();
                TempData["UrunSilindi"] = "Urun Silindi";
                return RedirectToAction("Urunler");

            }
        }

        public ActionResult Logout()
        {
            Session.Abandon(); // seansı yok eder.
            FormsAuthentication.SignOut(); //cookie yi yok eder.
            return RedirectToAction("Login");
        }

        public ActionResult Hata()
        {
            try
            {
                siteAyar siteAyar = new siteAyar();
               
                return View(siteAyar);

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

        [HttpPost]
        public ActionResult AdminEkle(string adi_soyadi, string sifre)
        {
            ViewModel vw = new ViewModel();

            var yon = db.Admin.Where(x => x.adi_soyadi == adi_soyadi).SingleOrDefault();
            if (yon != null)
            {
                TempData["AdminEklenmedi"] = "AdminEklenmedi";
            }
            else
            {
                Admin a = new Admin()
                {
                    adi_soyadi = adi_soyadi,
                    sifre = sifre

                };
                db.Admin.Add(a);
                db.SaveChanges();
                TempData["AdminEklendi"] = "AdminEklendi";
                return RedirectToAction("AdminDuzenle");
            }

            return RedirectToAction("AdminDuzenle");

        }

        [Authorize]
        public ActionResult AdminDuzenle()
        {
            ViewModel vw = new ViewModel();

            vw.admins = db.Admin.ToList();
            return View(vw);
        }

        public ActionResult AdminSil(int adminID)
        {
            using (VortexContext db = new VortexContext())
            {
                var silinecekadmin = db.Admin.Find(adminID);
                if (silinecekadmin == null)
                {
                    return HttpNotFound();
                }
                db.Admin.Remove(silinecekadmin);
                db.Admin.Remove(silinecekadmin);
                db.SaveChanges();
                TempData["AdminSilindi"] = "Yonetici Silindi";
                return RedirectToAction("AdminDuzenle");
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult siteduzenle(int siteayarID, string site_adi, siteAyar slider, string hakkinda_baslik, string hakkinda_yazi, string adres,string harita, string mail, string telefon1, string telefon2, string telefon3, string _abstract, string description, string keywords)
        {

            siteAyar ayar = db.siteAyar.Where(x => x.siteayarID == siteayarID).FirstOrDefault();

            ayar.site_adi = site_adi;
            ayar.hakkinda_baslik = hakkinda_baslik;
            ayar.hakkinda_yazi = hakkinda_yazi;
            ayar.adres = adres;
            ayar.mail = mail;
            ayar.harita = harita;
            ayar.telefon1 = telefon1;
            ayar.telefon2 = telefon2;
            ayar.telefon3 = telefon3;
            ayar._abstract = _abstract;
            ayar.description = description;
            ayar.keywords = keywords;


            if (Request.Files.Count > 0)
            {
                string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                string yol = "~/Content/images/" + dosyaadi;
                if (yol == "~/Content/images/")
                {

                }
                else
                {
                    Request.Files[0].SaveAs(Server.MapPath(yol));
                    ayar.slider = "/Content/images/" + dosyaadi;
                }




            }



            db.SaveChanges();
            TempData["Kaydedildi"] = "Kaydedildi";
            return RedirectToAction("SiteAyar");
        }

        public ActionResult SiteAyar()
        {
            try
            {
                ViewModel vw = new ViewModel()
                {

                    uruns = db.Urun.ToList(),
                    kategorilers = db.Kategoriler.ToList(),
                    admins = db.Admin.ToList(),
                    siteAyars = db.siteAyar.ToList()

                };
                return View(vw);

            }
            catch (Exception)
            {
                ViewBag.Message = "Bilinmeyen bir hata meydana geldi";
                return View();
            }
        }

    }
}