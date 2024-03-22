using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KhoaLuanSteam.Models;

namespace KhoaLuanSteam.Controllers
{
    public class CustomerController : ApiController
    {
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();
        [HttpGet]
        public HttpResponseMessage getAllCustomer()
        {
            List<Customer> list_customer = new List<Customer>();
            foreach (var item in db.KHACHHANGs)
            {
                Customer cus = new Customer();
                cus.MaKH = item.MaKH;
                cus.TenKH = item.TenKH;
                cus.DiaChi = item.DiaChi;
                cus.SDT = item.SDT;
                cus.Email = item.Email;
                cus.NgaySinh = item.NgaySinh;
                cus.GioiTinh = item.GioiTinh;
                cus.NgayTao = item.NgayTao;
                cus.TenDN = item.TenDN;
                cus.MatKhau = item.MatKhau;
                list_customer.Add(cus);
            }
            if (list_customer != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, list_customer);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        public HttpResponseMessage POST_CreateCustomer(Customer cus_Object)
        {
            List<Customer> list_customer = new List<Customer>();
            Customer cus = new Customer();
            //var Ketqua = db.KHACHHANGs.SingleOrDefault(x => x.MaKH == id);
            foreach (var item in db.KHACHHANGs)
            {
                if(cus_Object.TenDN == item.TenDN && cus_Object.MatKhau == item.MatKhau)
                {
                    cus.MaKH = item.MaKH;
                    cus.TenKH = item.TenKH;
                    cus.DiaChi = item.DiaChi;
                    cus.SDT = item.SDT;
                    cus.Email = item.Email;
                    cus.NgaySinh = item.NgaySinh;
                    cus.GioiTinh = item.GioiTinh;
                    cus.NgayTao = item.NgayTao;
                    cus.TenDN = item.TenDN;
                    cus.MatKhau = item.MatKhau;
                    list_customer.Add(cus);
                    break;
                }
                
            }

            if (list_customer != null && cus.MaKH >0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, cus);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
