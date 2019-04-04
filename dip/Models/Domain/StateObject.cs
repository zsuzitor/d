using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class StateObject: ItemFormCheckbox<StateObject>//, ICloneable
    {
        //public string Id { get; set; }
        //public string Name { get; set; }
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


        /// <summary>
        /// возвращает список родителей и их родителей от корня до ребенка, где ребенок ближайший родитель this
        /// </summary>
        /// <param name="db_"></param>
        /// <returns></returns>
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

        //загружает список ближайших детей, возвращает список всех детей
        //public override List<StateObject> GetChildsList(ApplicationDbContext db_ = null)
        //{
        //    List<StateObject> res = new List<StateObject>();
        //    var db = db_ ?? new ApplicationDbContext();

        //    //var par = db.StateObjects.FirstOrDefault(x1 => x1.Id == this.Parent);
        //    this.ReLoadChild();
        //    res.AddRange(this.Childs);
        //    foreach (var i in this.Childs)
        //    {
        //        res.AddRange(i.GetChildsList(db));
        //    }
            
        //    if (db_ == null)
        //        db.Dispose();

        //    return res;
        //}


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