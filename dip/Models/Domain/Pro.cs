﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Pro_
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }

        public ICollection<ActionPro> ActionPros;

        public Pro_()
        {

        }
    }
}