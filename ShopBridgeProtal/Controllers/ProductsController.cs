using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ShopBridgeProtal.Models;
using ShopBridge;

namespace ShopBridgeProtal.Controllers
{ 
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            IEnumerable<Product> products = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60464/api/"); //change web api url to run 
                var responseTask = client.GetAsync("Product");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Product>>();
                    readTask.Wait();
                    products = readTask.Result;
                }
                else
                {
                    products = Enumerable.Empty<Product>();
                    ModelState.AddModelError(string.Empty, "Server Error please contact administrator");
                }
                return View(products);
            }
        }

        public ActionResult AddNewProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewProduct(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60464/api/Product");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Product>("product", product);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(product);
        }

        public ActionResult Edit(int id)
        {
            Product product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60464/api/");
                //HTTP GET
                var responseTask = client.GetAsync("Product?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Product>();
                    readTask.Wait();

                    product = readTask.Result;
                }
            }
            return View(product);
        }


        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60464/api/product");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<Product>("product", product);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }
        
        public ActionResult Delete(string productname)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:60464/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("product?=" + productname);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

    }
}