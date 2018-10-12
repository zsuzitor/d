using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Action { get; set; }
        public bool Succes { get; set; }
        public string Info { get; set; }


        public string PersonId { get; set; }
        public ApplicationUser Person { get; set; }

        public Log()
        {
            DateTime = DateTime.Now;
            Action = null;
            Succes = false;
            Info = null;
            Person = null;
            PersonId = null;
        }
        public Log(string Action,bool Succes,string Info=null)
        {
            DateTime = DateTime.Now;
            this.Action = Action;
            this.Succes = Succes;
            this.Info = Info;
        }


        public bool AddLogDb()
        {
            using (var db=new ApplicationDbContext())
            {
                db.Logs.Add(new Log());


            }



                return true;
        }

    }
}