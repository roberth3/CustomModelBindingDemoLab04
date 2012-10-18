using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomModelBindingDemo.Infrastructure
{
    public class DateOfBirthModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof(DateTime?)
                && propertyDescriptor.Name == "DateOfBirth")
            {
                var request = controllerContext.HttpContext.Request;

                var dob = String.Format("{0}/{1}/{2}",
                                            request["DOBday"],
                                            request["DOBmonth"],
                                            request["DOByear"]);

                DateTime dateOfBirth;
                if (DateTime.TryParse(dob, out dateOfBirth))
                {
                    SetProperty(controllerContext, bindingContext, propertyDescriptor, dateOfBirth);
                    return;
                }
                else
                {
                    bindingContext.ModelState.AddModelError("Expiry",
                           "Date was not recognised");
                    return;
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}