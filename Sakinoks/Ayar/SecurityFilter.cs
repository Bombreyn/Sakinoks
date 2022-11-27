using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sakinoks.Models;

namespace Sakinoks.Ayar
{
    public class SecurityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            VortexContext db = new VortexContext();

            if (HttpContext.Current.Session["Admin"] == null)
                HttpContext.Current.Session["Admin"] = new Aktif();

            string controlAdi = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionAdi = filterContext.ActionDescriptor.ActionName;
            try
            {
                if (controlAdi == "Home" && actionAdi == "Urunler")
                {
                    if(Proje.Aktif.adminData == null)
                    {
                        filterContext.Result = new RedirectResult("/Home/Login");
                        return;
                    }

                    Proje.Aktif.adminData = db.Admin.Where(x => x.adminID == Proje.Aktif.adminData.adminID).FirstOrDefault();
                }

                if (controlAdi == "Home" && actionAdi == "UrunEkle")
                {
                    if (Proje.Aktif.adminData == null)
                    {
                        filterContext.Result = new RedirectResult("/Home/Login");
                        return;
                    }

                    Proje.Aktif.adminData = db.Admin.Where(x => x.adminID == Proje.Aktif.adminData.adminID).FirstOrDefault();
                }

                if (controlAdi == "Home" && actionAdi == "AdminDuzenle")
                {
                    if (Proje.Aktif.adminData == null)
                    {
                        filterContext.Result = new RedirectResult("/Home/Login");
                        return;
                    }

                    Proje.Aktif.adminData = db.Admin.Where(x => x.adminID == Proje.Aktif.adminData.adminID).FirstOrDefault();
                }

                if (controlAdi == "Home" && actionAdi == "Kategori")
                {
                    if (Proje.Aktif.adminData == null)
                    {
                        filterContext.Result = new RedirectResult("/Home/Login");
                        return;
                    }

                    Proje.Aktif.adminData = db.Admin.Where(x => x.adminID == Proje.Aktif.adminData.adminID).FirstOrDefault();
                }
                
                if (controlAdi == "Home" && actionAdi == "SiteAyar")
                {
                    if (Proje.Aktif.adminData == null)
                    {
                        filterContext.Result = new RedirectResult("/Home/Login");
                        return;
                    }

                    Proje.Aktif.adminData = db.Admin.Where(x => x.adminID == Proje.Aktif.adminData.adminID).FirstOrDefault();
                }


                if (controlAdi == "Home" && actionAdi == "KategoriEkle")
                {
                    if (Proje.Aktif.adminData == null)
                    {
                        filterContext.Result = new RedirectResult("/Home/Login");
                        return;
                    }

                    Proje.Aktif.adminData = db.Admin.Where(x => x.adminID == Proje.Aktif.adminData.adminID).FirstOrDefault();
                }

                base.OnActionExecuting(filterContext);
            }
            catch (Exception e)
            {
                filterContext.Result = new RedirectResult("/Home/Hata");
            }

        }
    }
}