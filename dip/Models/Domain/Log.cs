using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{

    /// <summary>
    /// класс для хранения логов
    /// </summary>
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


        public Log(string Action, string Controller, string PersonId, bool Success, Dictionary<string, string> param = null, string Info = null)
        {
            DateTime = DateTime.Now;
            this.Action = Action;
            this.Controller = Controller;
            this.Success = Success;
            this.Info = Info;

            this.PersonId = PersonId;
            this.LogParams = new List<LogParam>();
            this.Params_ = new Dictionary<string, string>();
            if (param != null)
                foreach (var i in param)
                    this.Params_.Add(i.Key, i.Value);
        }

        /// <summary>
        /// метод для установления параметров логов
        /// </summary>
        /// <param name="stateBegin">начальное состояние</param>
        /// <param name="stateEnd">конечное состояние</param>
        /// <param name="param">входные\выходные дескрипторы</param>
        /// <param name="paramobj">характеристики объекта</param>
        public void SetDescrParam(string stateBegin, string stateEnd, DescrSearchI[] param, DescrObjectI[] paramobj)
        {
            this.Params_.Add("stateBegin", stateBegin);
            this.Params_.Add("stateEnd", stateEnd);
            for (int i = 0; i < param.Length; ++i)
            {
                this.Params_.Add("ActionId" + i, param[i].ActionId);
                this.Params_.Add("ActionType" + i, param[i].ActionType);
                this.Params_.Add("FizVelId" + i, param[i].FizVelId);
                this.Params_.Add("ParametricFizVelId" + i, param[i].ParametricFizVelId);
                this.Params_.Add("ListSelectedPros" + i, param[i].ListSelectedPros);
                this.Params_.Add("ListSelectedSpec" + i, param[i].ListSelectedSpec);
                this.Params_.Add("ListSelectedVrem" + i, param[i].ListSelectedVrem);
            }
            for (int i = 0; i < paramobj.Length; ++i)
            {
                foreach (var i2 in paramobj[i])
                {
                    if (i2 == null)
                        break;
                    this.Params_.Add("phase" + i2.NumPhase + "_Begin" + i, i2.Begin.ToString());
                    this.Params_.Add("phase" + i2.NumPhase + "_PhaseState" + i, i2.PhaseState);
                    this.Params_.Add("phase" + i2.NumPhase + "_Composition" + i, i2.Composition);
                    this.Params_.Add("phase" + i2.NumPhase + "_MagneticStructure" + i, i2.MagneticStructure);
                    this.Params_.Add("phase" + i2.NumPhase + "_Conductivity" + i, i2.Conductivity);
                    this.Params_.Add("phase" + i2.NumPhase + "_MechanicalState" + i, i2.MechanicalState);
                    this.Params_.Add("phase" + i2.NumPhase + "_OpticalState" + i, i2.OpticalState);
                    this.Params_.Add("phase" + i2.NumPhase + "_Special" + i, i2.Special);
                }
            }
        }


        /// <summary>
        /// метод для добавления лога в бд
        /// </summary>
        /// <returns>список id параметров логов</returns>
        public List<int> AddLogDb()
        {
            List<int> res = new List<int>();
            using (var db = new ApplicationDbContext())
            {
                db.Logs.Add(this);
                db.SaveChanges();
                foreach (var i in this.Params_)
                {
                    var paramObj = new LogParam() { LogId = this.Id, Param = i.Value, Name = i.Key };
                    db.LogParams.Add(paramObj);
                    db.SaveChanges();
                    res.Add(paramObj.Id);
                }
            }
            return res;
        }
    }
}