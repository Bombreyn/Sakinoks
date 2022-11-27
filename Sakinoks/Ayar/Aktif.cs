using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sakinoks.Models;

namespace Sakinoks.Ayar
{
    public class Proje
    {
        public static Aktif Aktif
        {
            get
            {
                return (Aktif)HttpContext.Current.Session["Admin"];
            }
        }
    }
    public class Aktif
    {
        public Admin adminData { get; set; }
    }
}