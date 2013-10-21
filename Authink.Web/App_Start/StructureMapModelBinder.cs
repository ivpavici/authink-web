using System;
using System.Diagnostics;
using System.Web.Mvc;
using StructureMap;

namespace Authink.Web.App_Start
{
    public class StructureMapModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            try
            {
                return ObjectFactory.GetInstance(modelType);
            }
            catch (StructureMapException)
            {
                Debug.WriteLine(ObjectFactory.WhatDoIHave());
                throw;
            }
        }
    }
}