using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models.Process
{
    public class HomeProcess
    {
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = null;
        
        //constructor :  khởi tạo đối tượng
        public HomeProcess()
        {
            db = new QL_THIETBISTEAMEntities1();
        }
        /// <summary>
        /// hàm lưu phản hồi từ khách hàng vào db
        /// </summary>
        /// <param name="entity">LienHe</param>
        /// <returns>int</returns>
        public int InsertContact(LIENHE entity)
        {
            db.LIENHEs.Add(entity);
            db.SaveChanges();

            return entity.MaLH;
        }



    }
}