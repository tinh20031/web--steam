using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models
{
    //public class Address
    //{
    //}
    public class Province
    {
        public string name { get; set; }
        public int code { get; set; }
        public string codename { get; set; }
        public string division_type { get; set; }
        public int phone_code { get; set; }
        public List<District> districts { get; set; }
    }

    public class District
    {
        public string name { get; set; }
        public int code { get; set; }
        public string codename { get; set; }
        public string division_type { get; set; }
        public string short_codename { get; set; }
        public List<Ward> wards { get; set; }
    }

    public class Ward
    {
        public string name { get; set; }
        public int code { get; set; }
        public string codename { get; set; }
        public string division_type { get; set; }
        public string short_codename { get; set; }
    }

}