using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShopBridgeDataAccess;


namespace ShopBridge.Controllers
{
    public class ProductController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage  GetAllProductsDetails()
        {
            try
            {
                using (ShopBridgeEntities entities = new ShopBridgeEntities())
                {
                    var products = entities.Products.ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, products);
                }
            }
            catch(Exception ex)
            {
                var message = string.Format("ERROR: Error occured while processing request:{0}", ex);
                HttpError httpError = new HttpError(message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
            }
        }

        // GET api/<controller>/5
        public HttpResponseMessage GetProductDetail(int id)
        {
            try
            {
                using (ShopBridgeEntities entities = new ShopBridgeEntities())
                {
                    var product = entities.Products.FirstOrDefault(e => e.ProductID == id);
                    return Request.CreateResponse(HttpStatusCode.OK, product);
                }
            }
            catch(Exception ex)
            {
                var message = string.Format("ERROR: Error occured while processing request:{0}", ex);
                HttpError httpError = new HttpError(message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
            }
        }

        // POST api/<controller>
        public HttpResponseMessage  PostNewProduct([FromBody] Product new_product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = "Model is not valid";
                    HttpError httpError = new HttpError(message);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
                }
                using (ShopBridgeEntities entities = new ShopBridgeEntities())
                {
                    var isProductexists = entities.Products.Where(s => s.ProductName == new_product.ProductName)
                                                            .FirstOrDefault<Product>();
                    if (isProductexists == null)
                    {
                        var message = NonNegativeCheck(new_product);
                        if (message != "")
                        {
                            HttpError httpError = new HttpError(message);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
                        }
                        entities.Products.Add(new Product()
                        {
                            ProductName = new_product.ProductName,
                            ProductDescription = new_product.ProductDescription,
                            Price = new_product.Price,
                            Qty = new_product.Qty
                        });
                        entities.SaveChanges();
                    }
                    else
                    {
                        var message = "Product already exists";
                        HttpError httpError = new HttpError(message);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, new_product.ProductName);

            }
            catch(Exception ex)
            {
                var message = string.Format("ERROR: Error occured while processing request:{0}", ex);
                HttpError httpError = new HttpError(message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
            }

        }

        // PUT api/<controller>
        public HttpResponseMessage PutProduct([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = "Model is not valid";
                    HttpError httpError = new HttpError(message);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
                }
                using (ShopBridgeEntities entities = new ShopBridgeEntities())
                {
                    var existingProduct = entities.Products.Where(s => s.ProductName == product.ProductName)
                                                            .FirstOrDefault<Product>();

                    if (existingProduct != null)
                    {
                        var message = NonNegativeCheck(product);
                        if (message != "")
                        {
                            HttpError httpError = new HttpError(message);
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
                        }
                        existingProduct.ProductName = product.ProductName;
                        existingProduct.ProductDescription = product.ProductDescription;
                        existingProduct.Price = product.Price;
                        existingProduct.Qty = product.Qty;

                        entities.SaveChanges();
                    }
                    else
                    {
                        var message = "Product is not available";
                        HttpError httpError = new HttpError(message);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, product.ProductName);
            }
            catch(Exception ex)
            {
                var message = string.Format("ERROR: Error occured while processing request:{0}", ex);
                HttpError httpError = new HttpError(message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
            }
        }

        // DELETE api/<controller>/"ACD"
        public HttpResponseMessage  DeleteProduct(string product_name)
        {
            try
            {
                using (ShopBridgeEntities entities = new ShopBridgeEntities())
                {
                    var Product = entities.Products.Where(s => s.ProductName == product_name)
                                                            .FirstOrDefault<Product>();

                    if (Product != null)
                    {
                        entities.Entry(Product).State = System.Data.Entity.EntityState.Deleted;

                        entities.SaveChanges();
                    }
                    else
                    {
                        var message = "You cannot perform delete operation,because it is not exists in the inventory";
                        HttpError httpError = new HttpError(message);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, product_name);
            }
            catch(Exception ex)
            {
                var message = string.Format("ERROR: Error occured while processing request:{0}", ex);
                HttpError httpError = new HttpError(message);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, httpError);
            }
        }
        [NonAction]
        public string  NonNegativeCheck(Product p)
        {
            if (p.Qty < 0 )
            {
                return "Qty cannot be negative";                           
            }
            else if (p.Price < 0)
            {
                return "Price cannot be negative";                 
            }
            return "";
        }
    }
}