using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public static class ProductVal
    {
        public static void Validate(ProductModel model)
        {
            if (model is null)
                throw new MarketException("Customer model can not be null");

            if (String.IsNullOrEmpty(model.ProductName))
                throw new MarketException("The product name can not be empty");

            if (model.Price < 0)
                throw new MarketException("Price can not be negative");
        }
    }
}
