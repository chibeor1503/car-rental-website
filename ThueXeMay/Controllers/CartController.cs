//Đức Tài :v
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeMay.Models;

namespace ThueXeMay.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        RENT_MOTOREntities myObj = new RENT_MOTOREntities();

        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);
        }

        public ActionResult AddCart(int id)
        {
            if (Session["giohang"] == null)
            {
                Session["giohang"] = new List<CartItem>();
            }

            List<CartItem> giohang = Session["giohang"] as List<CartItem>;

            if (giohang.FirstOrDefault(m => m.id_xe == id) == null) // ko co sp nay trong gio hang
            {
                bike sp = myObj.bikes.Find(id);

                CartItem newItem = new CartItem()
                {
                    id_xe = id,
                    name = sp.name,
                    SoLuong = 1,
                    image = sp.image,
                    price = (int)sp.price,

                };  // Tạo ra 1 CartItem mới

                giohang.Add(newItem);  // Thêm CartItem vào giỏ 
            }
            else
            {
                // Nếu sản phẩm khách chọn đã có trong giỏ hàng thì không thêm vào giỏ nữa mà tăng số lượng lên.
                CartItem cardItem = giohang.FirstOrDefault(m => m.id_xe == id);
                cardItem.SoLuong++;
            }
            return Redirect(url: Request.UrlReferrer.ToString());
            // Action này sẽ chuyển hướng về trang chi tiết sp khi khách hàng đặt vào giỏ thành công. Bạn có thể chuyển về chính trang khách hàng vừa đứng bằng lệnh return Redirect(Request.UrlReferrer.ToString()); nếu muốn.
            //return RedirectToAction("Details", "Cars", new { idx = id });
        }
        public RedirectToRouteResult SuaSoLuong(int SanPhamID, int soluongmoi)
        {
            // tìm carditem muon sua
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem itemSua = giohang.FirstOrDefault(m => m.id_xe == SanPhamID);
            if (itemSua != null)
            {
                itemSua.SoLuong = soluongmoi;
            }
            return RedirectToAction("Index");

        }
        public RedirectToRouteResult XoaKhoiGio(int SanPhamID)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            CartItem itemXoa = giohang.FirstOrDefault(m => m.id_xe == SanPhamID);
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(string inputUsername, string inputPhone, string inputEmail, string inputNote, int? id)
        {
            //try
            //{
            List<CartItem> carts = (List<CartItem>)Session["giohang"];
            rent rent = new rent()
            {
                name = inputUsername,
                phone = inputPhone,
                mail = inputEmail,
                note = inputNote,
                id_user = id,
                date = DateTime.Now,

            };
            myObj.rents.Add(rent);
            myObj.SaveChanges();

            foreach (CartItem item in carts)
            {
                rentDetail rentDetail = new rentDetail()
                {
                    id_rent = rent.id_rent,
                    id_bike = item.id_xe,
                    amount = item.SoLuong,
                };
                myObj.rentDetails.Add(rentDetail);
                myObj.SaveChanges();
            }

            Session.Remove("giohang");
            return View("RentSuccess");
            //} catch
            //{
            //    return View("Error");
            //}
        }
        //public ActionResult Checkout(FormCollection frm)
        //{
        //    //try
        //    //{
        //        List<CartItem> carts = (List<CartItem>)Session["giohang"];
        //        rent rent = new rent()
        //        {
        //            name = frm["inputUsername"],
        //            phone = frm["inputPhone"],
        //            mail = frm["inputEmail"],
        //            note = frm["inputNote"],
        //            date = DateTime.Now,
        //        };
        //        myObj.rents.Add(rent);
        //        myObj.SaveChanges();

        //        foreach (CartItem item in carts)
        //        {
        //            rentDetail rentDetail = new rentDetail()
        //            {
        //                id_rent = rent.id_rent,
        //                id_bike = item.id_xe,
        //                amount = item.SoLuong,
        //            };
        //            myObj.rentDetails.Add(rentDetail);
        //            myObj.SaveChanges();
        //        }

        //        Session.Remove("giohang");
        //        return View("RentSuccess");
        //    //} catch
        //    //{
        //    //    return View("Error");
        //    //}
        //}
    }
}