using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WebNorthwindOrders.Models;


namespace WebNorthwindOrders.Controllers
{
    public class DefaultController : Controller
    {
        private NORTHWNDEntities db = new NORTHWNDEntities();
        database_Access_Layer.db dbLayer = new database_Access_Layer.db();
     
        public ActionResult Index()
        {
           
            try
            {
                return View(db.CustOrdersOrders(Session["UserId"].ToString(), "").ToList());
            }
            catch (Exception ex)
            {             
                return View("Error");
            }

        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {

            return View(db.CustOrdersOrders(Session["UserId"].ToString(), fc["txtSearch"]).ToList());

          
        }


        public ActionResult Login(string Lan)
        {


            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "English", Value = "en-US" });
            items.Add(new SelectListItem { Text = "Chinese", Value = "zh-TW" });

            ViewBag.Language = items;
         
            string LanguageAbbrevation = "";
            if (Lan == null)
            {
              
                LanguageAbbrevation = "en-US";

            }
            else {
                LanguageAbbrevation = Lan;
            }
            
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);
            return View();

          
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            int res = dbLayer.Login(fc["txtUserId"], fc["txtPassword"]);
            if (res == 1)
            {
               
                Session["UserId"] = fc["txtUserId"];

                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Login account or password is not correct...";
                return View();
            }
            
        }

        public ActionResult Change(String LanguageAbbrevation)
        {
            
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);

                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = LanguageAbbrevation;
                Response.Cookies.Add(cookie);
                return View();
         
            
        }

        public ActionResult Edit(int id)
        {
            Order EditOrder= db.OrderByOrderID(id).First();
          
            return View(EditOrder);
            
        }

        [HttpPost]
        public ActionResult Edit(FormCollection fc)
        {
           
            DateTime dtOrderDate = Convert.ToDateTime(fc["OrderDate"]);
            DateTime dtRequiredDate = Convert.ToDateTime(fc["RequiredDate"]);
            DateTime dtShippedDate = Convert.ToDateTime(fc["ShippedDate"]);
          
            try
            {
                db.UpdateOrder(Session["UserId"].ToString(), fc["txtOrderId"], dtOrderDate.ToLocalTime(), dtRequiredDate.ToLocalTime(), dtShippedDate.ToLocalTime(), fc["txtShipCity"], fc["txtShipAddress"]);
              
            }
            catch (Exception ex)
            {

                TempData["ErrMsg"] = ex.InnerException.Message;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Order DeleteOrder = db.OrderByOrderID(id).First();

            return View(DeleteOrder);

        }

        [HttpPost]
        public ActionResult Delete(FormCollection fc)
        {
          
            try
            {
                db.DeleteOrder(Session["UserId"].ToString(), fc["txtOrderId"]);
               
            }
            catch (Exception ex)
            {
               
                TempData["ErrMsg"] = ex.InnerException.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(FormCollection fc)
        {
          
            
            DateTime dtOrderDate = Convert.ToDateTime(fc["txtOrderDate"]);
            DateTime dtRequiredDate = Convert.ToDateTime(fc["txtRequiredDate"]);
            DateTime dtShippedDate = Convert.ToDateTime(fc["txtShippedDate"]);
            
          try
           {
                db.CreateOrder(Session["UserId"].ToString(), fc["txtOrderId"], fc["txtCustomerID"], fc["txtShipCity"], fc["txtShipAddress"], dtOrderDate.ToLocalTime(), dtRequiredDate.ToLocalTime(), dtShippedDate.ToLocalTime());
              
            }
            catch (Exception ex) {
                
                TempData["ErrMsg"] = ex.InnerException.Message;
            }
                return RedirectToAction("Index");
        }

        public ActionResult Create()
        {

            return View();
        }

    }
}