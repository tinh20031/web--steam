using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KhoaLuanSteam.Models;

namespace KhoaLuanSteam.Controllers
{
    public class OrderController : ApiController
    {
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();

        //[HttpGet]
        //[Route("{id}")]
        //[HttpGet]
        //[HttpGet("{id}")]


        //[HttpGet]
        //[Route("api/getIdOrder/{id}")]
        //[HttpGet("{id}")]
        //public HttpResponseMessage getIdOrder(int id)
        //{
        //    List<Order> list_order = new List<Order>();

        //    //var Ketqua = db.PHIEUDATHANGs.SingleOrDefault(x => x.MaKH == id);

        //    foreach (var item in db.PHIEUDATHANGs)
        //    {
        //        if (item.MaKH == id)
        //        {
        //            Order ord = new Order();
        //            ord.MaPhieuDH = item.MaPhieuDH;
        //            ord.MaKH = item.MaKH;
        //            ord.NgayDat = item.NgayDat;
        //            ord.Tong_SL_Dat = item.Tong_SL_Dat;
        //            ord.ThanhTien = item.ThanhTien;
        //            ord.TinhTrang = item.TinhTrang;
        //            ord.PhiShip = item.PhiShip;
        //            list_order.Add(ord);
        //        }
        //    }
        //    if (list_order != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, list_order);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //    }
        //}




        [HttpGet]
        public HttpResponseMessage getAllOrder()
        {
            List<Order> list_order = new List<Order>();
            foreach (var item in db.PHIEUDATHANGs)
            {
                Order ord = new Order();
                ord.MaPhieuDH = item.MaPhieuDH;
                ord.MaKH = item.MaKH;
                ord.NgayDat = item.NgayDat;
                ord.Tong_SL_Dat = item.Tong_SL_Dat;
                ord.ThanhTien = item.ThanhTien;
                ord.TinhTrang = item.TinhTrang;
                ord.PhiShip = item.PhiShip;
                list_order.Add(ord);
            }
            if (list_order != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, list_order);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }


        //public IHttpActionResult GetEmployeeById(int id)
        //{
        //    var employee = db.PHIEUDATHANGs.SingleOrDefault(e => e.MaKH == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(employee);
        //}

        //public ActionResult GetEmployeeById(int id)
        //{
        //    var employee = db.PHIEUDATHANGs.SingleOrDefault(e => e.MaKH == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    return Json(employee, JsonRequestBehavior.AllowGet);
        //}



    }
}
