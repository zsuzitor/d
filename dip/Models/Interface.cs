using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    public class Interface
    {
       


    }

    public interface Item
    {
         string Id { get; set; }
         string Name { get; set; }
         string Parent { get; set; }
    }



}