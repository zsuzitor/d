using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using static dip.Models.Functions;


using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Web.Hosting;
using dip.Models.ViewModel;
using System.Text.RegularExpressions;
using System.Data.Linq.SqlClient;
using Binbin.Linq;
//using Microsoft.SqlServer.Management.Common;




namespace dip.Models.Domain
{
    enum RolesProject { NotApproveUser, user,subscriber, admin };//vip
    //var a = (RolesProject)Enum.Parse(typeof(RolesProject), "", true);
    

   

   
}