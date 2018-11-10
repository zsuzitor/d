using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool Succes { get; set; }
        public string Info { get; set; }


        public string PersonId { get; set; }
        public ApplicationUser Person { get; set; }


        public ICollection<LogParam> LogParams { get; set; }

        [NotMapped]
        public List<string> Params_ { get; set; }

        public Log()
        {
            DateTime = DateTime.Now;
            Action = null;
            Controller = null;
            Succes = false;
            Info = null;
            Person = null;
            PersonId = null;
            LogParams = new List<LogParam>();
        }
        public Log(string Action,string Controller, bool Succes,string Info=null,params string[] param)
        {
            DateTime = DateTime.Now;
            this.Action = Action;
            this.Controller = Controller;
            this.Succes = Succes;
            this.Info = Info;
            
            if (param.Length > 0)
                this.Params_.AddRange(param.ToList());
        }


        public bool AddLogDb()
        {
            using (var db=new ApplicationDbContext())
            {
                db.Logs.Add(this);
                db.SaveChanges();
                foreach(var i in this.Params_)
                {
                    db.LogParams.Add(new LogParam() {LogId=this.Id,Param=i });
                }
                db.SaveChanges();
            }



                return true;
        }

    }
}