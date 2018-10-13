using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.Models;
namespace Ecommerce.Models
{
    public class ProductEdit
    {
      public  Product Product { get; set; }
      public  List<Image> Images { get; set; }
      public  List<ProductColor> ProductColors { get; set; }
      public  List<ProductSize> ProductSizes { get; set; }


    }
}