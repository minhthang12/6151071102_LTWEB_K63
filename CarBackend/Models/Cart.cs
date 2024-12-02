using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBackend.Models
{
    public class Cart
    {
        QLBanXeGanMayEntities data = new QLBanXeGanMayEntities();
        public int carID { get; set; }
        public string carName { get; set; }
        public string carImage { get; set; }
        public Double price { get; set; }
        public int quantity { get; set; }
        public Double total
        {
            get { return price * quantity; }
        }

        public Cart(int carId)
        {
            carID = carId;
            XEGANMAY book = data.XEGANMAYs.Single(b => b.MaXe == carId);
            carName = book.TenXe;
            carImage = book.Anhbia;
            price = double.Parse(book.Giaban.ToString());
            quantity = 1;
        }
    }
}