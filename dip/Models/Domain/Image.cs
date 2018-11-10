﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Image
    {
        public int Id { get; set; }

        public byte[] Data { get; set; }

        public int FeTextIDFE { get; set; }
        public FEText FeText { get; set; }

        public Image()
        {
            Data = null;
            FeTextIDFE = 0;
            FeText = null;
        }

    }
}