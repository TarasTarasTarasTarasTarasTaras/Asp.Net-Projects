using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public static class ProductCategoryVal
    {
        public static void Validate(ProductCategoryModel model)
        {
            if (String.IsNullOrEmpty(model.CategoryName))
                throw new MarketException("The product category name can not be empty");
        }
    }
}
