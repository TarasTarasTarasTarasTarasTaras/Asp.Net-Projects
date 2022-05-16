using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Validation
{
    public static class CustomerVal
    {
        public static void Validate(CustomerModel model)
        {
            if (model is null)
                throw new MarketException("Customer model can not be null");

            if (String.IsNullOrEmpty(model.Name))
                throw new MarketException("The name can not be empty");

            if (String.IsNullOrEmpty(model.Surname))
                throw new MarketException("The surname can not be empty");

            if (model.BirthDate.Year < 1900 || model.BirthDate > DateTime.Now)
                throw new MarketException($"The year should be between 1900 and {DateTime.Now.Year}");

            if (model.DiscountValue < 0)
                throw new MarketException("The discount value can not be negative");
        }
    }
}
