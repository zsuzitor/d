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
        public bool Success { get; set; }
        public string Info { get; set; }


        public string PersonId { get; set; }
        public ApplicationUser Person { get; set; }


        public List<LogParam> LogParams { get; set; }

        [NotMapped]
        public Dictionary<string, string> Params_ { get; set; }

        public Log()
        {
            DateTime = DateTime.Now;
            Action = null;
            Controller = null;
            Success = false;
            Info = null;
            Person = null;
            PersonId = null;
            LogParams = new List<LogParam>();
            Params_ = new Dictionary<string, string>();
        }
        public Log(string Action,string Controller,string PersonId, bool Success, Dictionary<string, string> param=null, string Info=null)
        {
            DateTime = DateTime.Now;
            this.Action = Action;
            this.Controller = Controller;
            this.Success = Success;
            this.Info = Info;
            
            this.PersonId = PersonId;
            this.LogParams = new List<LogParam>();
            this.Params_ = new Dictionary<string, string>();
            if(param!=null)
            foreach(var i in param)
                this.Params_.Add(i.Key,i.Value);
        }


        public void SetDescrParam(DescrSearchI[] param)
        {
          
            foreach(var i in param)
            {
                //TODO name проставлять
                this.Params_.Add("ActionId", i.ActionId);
                this.Params_.Add("ActionType", i.ActionType);
                this.Params_.Add("FizVelId", i.FizVelId);
                this.Params_.Add("ParametricFizVelId", i.ParametricFizVelId);
                this.Params_.Add("ListSelectedPros", i.ListSelectedPros);
                this.Params_.Add("ListSelectedSpec", i.ListSelectedSpec);
                this.Params_.Add("ListSelectedVrem", i.ListSelectedVrem);
            }



        }



        public List<int> AddLogDb()
        {
            List<int> res = new List<int>();
            using (var db=new ApplicationDbContext())
            {
                db.Logs.Add(this);
                db.SaveChanges();
                foreach(var i in this.Params_)
                {
                    var paramObj = new LogParam() { LogId = this.Id, Param = i.Value,Name=i.Key };
                    
                    db.LogParams.Add(paramObj);
                    
                                        db.SaveChanges();
                    res.Add(paramObj.Id);
                }

            }
            


            return res;
        }

    }
}