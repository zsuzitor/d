using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.AdminV
{
    public class ChangeRoleV
    {
        public List<string> Roles { get; set; }

        public ChangeRoleV()
        {
            Roles = new List<string>();
        }
    }
}