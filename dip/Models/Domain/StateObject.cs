﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class StateObject: AParentDb<StateObject>//, ICloneable
    {
        //public string Id { get; set; }
        public string Name { get; set; }
        // public string Parent { get; set; }

        public int? CountPhase { get; set; }

        //[NotMapped]
        //public List<StateObject> Childs { get; set; }
        //[NotMapped]
        //public StateObject ParentItem { get; set; }

        public List<FEText> FeTextBegin { get; set; }
        public List<FEText> FeTextEnd { get; set; }


        public StateObject()
        {
            CountPhase = null;
        }

        //public StateObject(StateObject obj)
        //{
        //    this.
        //}

        public  string LoadPartialTree(List<StateObject> States, out int countPhase)
        {
            string res = "";
            countPhase = 1;
            var stateList = this.GetParentsList();
            stateList.Add(this);
            foreach (var i in stateList)
                res += i.Id + " ";
            //using (var db = new ApplicationDbContext())
            //db.StateObjects.Where(x1=>x1.Parent== "STRUCTOBJECT").ToList();
            foreach (var i in States)
                if (i.Id == stateList[0].Id)//res.StateStart = stateList[0];
                {
                    i.LoadPartialTree(stateList);
                    countPhase = (int)i.CountPhase;
                    //Characteristics.SetFirstLvlStates(i.CountPhase, basePhase);
                    break;
                }

            return res;
        }




        public static List<StateObject> GetBase()
        {
            List<StateObject> res = new List<StateObject>();
            using (var db = new ApplicationDbContext())
                            res= db.StateObjects.Where(x1 => x1.Parent == "STRUCTOBJECT").ToList();
            return res; 
        }


        public StateObject CloneWithOutRef()
        {
            return new StateObject()
            {
                CountPhase=this.CountPhase,
                Id=this.Id,
                Name= this.Name,
                Parent= this.Parent
            };


        }
        public static List<StateObject> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<StateObject> res = new List<StateObject>();
            using (var db = new ApplicationDbContext())
                res = db.StateObjects.Where(x1 => x1.Parent == id).ToList();
            return res;

        }

        public static StateObject Get(string id)
        {
            StateObject res = null;
            if(!string.IsNullOrWhiteSpace(id))
            using (var db = new ApplicationDbContext())
                res=db.StateObjects.FirstOrDefault(x1=>x1.Id==id);
            return res;
        }


        //public override void LoadChild()
        //{
        //    if (this.Childs.Count < 1)
        //        this.ReLoadChild();
        //}

        public override void ReLoadChild()
        {

            using (var db = new ApplicationDbContext())
                this.Childs = db.StateObjects.Where(x1 => x1.Parent == this.Id).ToList();
        }



        public override List<StateObject> GetParentsList(ApplicationDbContext db_ = null)
        {
            List<StateObject> res = new List<StateObject>();
            var db = db_ ?? new ApplicationDbContext();

            var par = db.StateObjects.FirstOrDefault(x1 => x1.Id == this.Parent);

            if (par != null)
            {
                if (par.Parent!= "STRUCTOBJECT")
                    res.AddRange(par.GetParentsList(db));

                res.Add(par);
            }



            if (db_ == null)
                db.Dispose();

            return res;
        }



        //мб вынести в класс
        //public override bool LoadPartialTree(List<StateObject> list)
        //{
        //    this.LoadChild();
        //    if (list == null || list.Count < 1)
        //        return false;
        //    //this.LoadChild();
        //    foreach (var i in this.Childs)
        //    {
        //        if (list.FirstOrDefault(x1 => x1.Id == i.Id) != null) //if (list.Contains(i))
        //            i.LoadPartialTree(list);
        //    }
           

        //    return true;
        //}
    }
}