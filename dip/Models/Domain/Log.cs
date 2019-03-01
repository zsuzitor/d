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
            Params_ = new List<string>();
        }
        public Log(string Action,string Controller,string PersonId, bool Succes,string Info=null,params string[] param)
        {
            DateTime = DateTime.Now;
            this.Action = Action;
            this.Controller = Controller;
            this.Succes = Succes;
            this.Info = Info;
            
            this.PersonId = PersonId;
            this.LogParams = new List<LogParam>();
            this.Params_ = new List<string>();
            if (param.Length > 0)
                this.Params_.AddRange(param.ToList());
        }


        public void SetDescrParam(DescrSearchI[] param)
        {
            //при добавлении добавлять в конец
            //this.Params_.Add(inp.ActionId);
            //this.Params_.Add(inp.ActionType);
            //this.Params_.Add(inp.FizVelId);
            //this.Params_.Add(inp.ParametricFizVelId);
            //this.Params_.Add(inp.ListSelectedPros);
            //this.Params_.Add(inp.ListSelectedSpec);
            //this.Params_.Add(inp.ListSelectedVrem);
            //this.Params_.Add(outp.ActionId);
            //this.Params_.Add(outp.ActionType);
            //this.Params_.Add(outp.FizVelId);
            //this.Params_.Add(outp.ParametricFizVelId);
            //this.Params_.Add(outp.ListSelectedPros);
            //this.Params_.Add(outp.ListSelectedSpec);
            //this.Params_.Add(outp.ListSelectedVrem);
            foreach(var i in param)
            {
                //TODO name проставлять
                this.Params_.Add(i.ActionId);
                this.Params_.Add(i.ActionType);
                this.Params_.Add(i.FizVelId);
                this.Params_.Add(i.ParametricFizVelId);
                this.Params_.Add(i.ListSelectedPros);
                this.Params_.Add(i.ListSelectedSpec);
                this.Params_.Add(i.ListSelectedVrem);
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
                    var paramObj = new LogParam() { LogId = this.Id, Param = i };
                    
                    db.LogParams.Add(paramObj);
                    
                                        db.SaveChanges();
                    res.Add(paramObj.Id);
                }

            }
            


            return res;
        }

    }
}