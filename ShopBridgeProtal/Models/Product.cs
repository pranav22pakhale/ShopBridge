using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopBridgeProtal.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Product Name is mandatory")]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Target Price; Max 18 digits")]
        public decimal  Price { get; set; }
        [Range(0, 1000000, ErrorMessage = "Invalid Target Quantity; Max 1000000 digits")]
        public int Qty { get; set; }
    }
}