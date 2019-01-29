using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PersonV
{
    public class PersonalRecordV
    {
        public string CurrenUserId { get; set; }
        public ApplicationUser User { get; set;}
        RolesProject[] RoleMax { get; set; }

        public PersonalRecordV()
        {
            CurrenUserId = null;
            User = null;
            RoleMax = null;
        }
    }
}