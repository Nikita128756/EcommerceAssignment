﻿using EcommerceDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApp.Models
{
    public class ProductViewModel
    {       
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }      
        public List<FeatureViewModel> AttributeDetails { get; set; }       
    }
}