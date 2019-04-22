using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Models.ViewModel
{
    public class DescriptionForm
    {
        // public string Postfix { get; set; }
        public List<Domain.AllAction> ActionId { get; set; }
        public List<Domain.ActionType> ActionType { get; set; }
        public List<Domain.FizVel> FizVelId { get; set; }
        public List<Domain.FizVel> ParametricFizVelId { get; set; }
        public List<Pro> Pros { get; set; }
        public List<Spec> Specs { get; set; }
        public List<Vrem> Vrems { get; set; }
        public string CurrentAction { get; set; }
        public string CurrentActionId { get; set; }
       

        //public Domain.Action Action { get; set; }


        public DescriptionForm()
        {
            ActionId = new List<AllAction>();
            ActionType = new List<Domain.ActionType>();
            FizVelId = new List<FizVel>();
            ParametricFizVelId = new List<FizVel>();
            Pros = new List<Pro>();
            Specs = new List<Spec>();
            Vrems = new List<Vrem>();

            //ActionParametricIds ="";
        }

        //возвращает только данные для отображения(список полей и чему равны) (текстовое представление)
        public static Dictionary<string, string> GetFormShow(int idfe)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            List<FEAction> inp = null;
            List<FEAction > outp = null;
            FEAction.Get(idfe, ref inp, ref outp);

            int iter = 0;
            foreach(var i2 in inp)
            {
                ++iter;
                string[] inpPros = i2.Pros.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] inpSpec = i2.Spec.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] inpVrem = i2.Vrem.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                List<List<Pro>> prosI = new List<List<Pro>>();
                List<List<Spec>> specI = new List<List<Spec>>();
                List<List<Vrem>> vremI = new List<List<Vrem>>();
                using (var db = new ApplicationDbContext())
                {
                    //TODO вынести по классам в методы типо "gettext"
                    res["NameI"+ iter] = db.AllActions.First(x1 => x1.Id == i2.Name).Name;
                    res["TypeI" + iter] = db.ActionTypes.First(x1 => x1.Id == i2.Type).Name;
                    res["FizVelIdI" + iter] = db.FizVels.First(x1 => x1.Id == i2.FizVelId).Name;
                    res["FizVelparamI" + iter] = db.FizVels.FirstOrDefault(x1 => x1.Id == i2.FizVelSection)?.Name;


                    //TODO тут везде цикл в цикле
                    //TODO сейчас очень не оптимизированно (на каждой итерации циклов загружаются по сути 1 и теже данные тк родитель у многих общий)
                    var prs = db.Pros.Where(x1=> inpPros.Contains(x1.Id)).ToList();
                    prosI=Pro.GetQueueParent(prs);
                    //foreach (var i in inpPros)
                    //{
                    //    Pro pr = db.Pros.First(x1 => x1.Id == i);
                    //    var list = pr.GetParentsList(db);
                    //    list.Add(pr);
                    //    prosI.Add(list);
                    //    //prosI.Add(string.Join("->",(( Pro.GetParents(i, db) + " " + i).Split(new string[]{" " },StringSplitOptions.RemoveEmptyEntries))));

                    //}
                    var specs = db.Specs.Where(x1 => inpSpec.Contains(x1.Id)).ToList();
                    specI = Spec.GetQueueParent(specs);
                    //foreach (var i in inpSpec)
                    //{
                    //    Spec sp = db.Specs.First(x1 => x1.Id == i);
                    //    var list = sp.GetParentsList(db);
                    //    list.Add(sp);
                    //    specI.Add(list);
                    //    //specI.Add(string.Join("->", (( Spec.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                    //}

                    var vrems = db.Vrems.Where(x1 => inpVrem.Contains(x1.Id)).ToList();
                    vremI = Vrem.GetQueueParent(vrems);
                    //foreach (var i in inpVrem)
                    //{
                    //    Vrem pr = db.Vrems.First(x1 => x1.Id == i);
                    //    var list = pr.GetParentsList(db);
                    //    list.Add(pr);
                    //    vremI.Add(list);
                    //    //vremI.Add(string.Join("->", (( Vrem.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                    //}
                }
                res["ProsI" + iter] = string.Join(" ", Pro.GetQueueParentString(prosI));// string.Join(" ", prosI);// ;
                res["SpecsI" + iter] = string.Join(" ", Spec.GetQueueParentString(specI)); //string.Join(" ", specI); string.Join(" ", Spec.GetQueueParentString(Spec.GetQueueParent(specI)));
                res["VremsI" + iter] = string.Join(" ", Vrem.GetQueueParentString(vremI));// string.Join(" ", vremI); //string.Join(" ", Vrem.GetQueueParentString(Vrem.GetQueueParent(vremI)));
            }
             iter = 0;
            foreach (var i2 in outp)
            {
                ++iter;
                string[] outpPros = i2.Pros.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] outpSpec = i2.Spec.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string[] outpVrem = i2.Vrem.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);



                List<List<Pro>> prosO = new List<List<Pro>>();
                List<List<Spec>> specO = new List<List<Spec>>();
                List<List<Vrem>> vremO = new List<List<Vrem>>();


                using (var db = new ApplicationDbContext())
                {
                    res["NameO" + iter] = db.AllActions.First(x1 => x1.Id == i2.Name).Name;
                    res["TypeO" + iter] = db.ActionTypes.First(x1 => x1.Id == i2.Type).Name;
                    res["FizVelIdO" + iter] = db.FizVels.First(x1 => x1.Id == i2.FizVelId).Name;
                    res["FizVelparamO" + iter] = db.FizVels.FirstOrDefault(x1 => x1.Id == i2.FizVelSection)?.Name;



                    var prs = db.Pros.Where(x1 => outpPros.Contains(x1.Id)).ToList();
                    prosO = Pro.GetQueueParent(prs);

                    var specs = db.Specs.Where(x1 => outpSpec.Contains(x1.Id)).ToList();
                    specO = Spec.GetQueueParent(specs);

                    var vrems = db.Vrems.Where(x1 => outpVrem.Contains(x1.Id)).ToList();
                    vremO = Vrem.GetQueueParent(vrems);



                    //TODO тут везде цикл в цикле
                    //foreach (var i in outp_pros)
                    //{
                    //    Pro pr = db.Pros.First(x1 => x1.Id == i);
                    //    var list = pr.GetParentsList(db);
                    //    list.Add(pr);
                    //    prosO.Add(list);
                    //    //prosO.Add(string.Join("->", ((Pro.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                    //}
                    //foreach (var i in outp_spec)
                    //{
                    //    Spec sp = db.Specs.First(x1 => x1.Id == i);
                    //    var list = sp.GetParentsList(db);
                    //    list.Add(sp);
                    //    specO.Add(list);
                    //    //specO.Add(string.Join("->", ((Spec.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                    //}
                    //foreach (var i in outp_vrem)
                    //{
                    //    Vrem pr = db.Vrems.First(x1 => x1.Id == i);
                    //    var list = pr.GetParentsList(db);
                    //    list.Add(pr);
                    //    vremO.Add(list);
                    //    //vremO.Add(string.Join("->", ((Vrem.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                    //}

                    //




                    

                }





                res["ProsO" + iter] = string.Join(" ", Pro.GetQueueParentString(prosO));// string.Join(" ", prosO);// string.Join(" ", Pro.GetQueueParentString(Pro.GetQueueParent(prosO)));
                res["SpecsO" + iter] = string.Join(" ", Spec.GetQueueParentString(specO)); //string.Join(" ", specO);// string.Join(" ", Spec.GetQueueParentString(Spec.GetQueueParent(specO)));
                res["VremsO" + iter] = string.Join(" ", Vrem.GetQueueParentString(vremO));// string.Join(" ", vremO); //string.Join(" ", Vrem.GetQueueParentString(Vrem.GetQueueParent(vremO)));

            }



            return res;
        }


        //возвращает форму() для отображения
        public static DescriptionForm GetFormObject(string actionId, string fizVelId, string prosIds = "", string specIds = "", string vremIds = "")
        {
            DescriptionForm res = new DescriptionForm();

            string[] prosIdList = prosIds?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            string[] specIdList = specIds?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            string[] vremIdList = vremIds?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];



            List<Pro> prosList = null;
            List<Spec> specList = null;
            List<Vrem> vremList = null;




            using (var db = new ApplicationDbContext())
            {
                // Получаем список всех воздействий 
                var listOfActions = db.AllActions.OrderBy(action => action.Id).ToList();

                if (string.IsNullOrWhiteSpace(actionId))
                    actionId = listOfActions.FirstOrDefault()?.Id;
                //res.ActionParametricIds = string.Join(" ",listOfActions.Where(x1 => x1.Parametric).Select(x1 => x1.Id).ToList());

                if (string.IsNullOrWhiteSpace(actionId))
                    return res;
                // Получаем список типов воздействий     
                var actionType = db.ActionTypes.OrderByDescending(type => type.Name).ToList();//, "id", "name", "Не выбрано")

                // Получаем список физических величин для выбранного воздействия
                List<FizVel> listOfFizVels;
                //if (!string.IsNullOrWhiteSpace(actionId))
                    listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == actionId + "_FIZVEL") ||
                                                                                (fizVel.Id == "NO_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();
                //else
                //    listOfFizVels = new List<FizVel>();

                // Выбираем  из списка раздел физики
                if (string.IsNullOrWhiteSpace(fizVelId))
                    fizVelId = listOfFizVels.First().Id;

                // Получаем список физических величин для параметрических воздействий
                var listOfParametricFizVels = db.FizVels.Where(parametricFizVel => (parametricFizVel.Parent == fizVelId))
                                                                     .OrderBy(parametricFizVel => parametricFizVel.Id).ToList();


                //TODO че по оптимизации?
                // Получаем список пространственных характеристик для выбранного воздействия
                prosList = db.Pros.Where(x1 => x1.Parent == actionId + "_PROS").ToList();
                if (prosIdList.Length > 0)
                {
                    var allPros = db.Pros.Where(x1 => prosIdList.Contains(x1.Id)).ToList();
                   // var treeProBase = Pro.GetQueueParent(allPros);
                    foreach (var p in prosList)
                    {
                        if (allPros.FirstOrDefault(x1 => x1.Parent == p.Id) != null)
                            p.LoadPartialTree(allPros);
                        //foreach (var i in treeProBase)
                        //{
                        //    if (p.Id == i[0].Id)
                        //        if (!p.LoadPartialTree(i))
                        //            throw new Exception("TODO ошибка");
                        //}
                    }

                    // prosList = allPros.Where(x1 => x1.Parent.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries).Length == 1).ToList();
                }



                //TODO че по оптимизации?
                // Получаем список специальных характеристик для выбранного воздействия
                specList = db.Specs.Where(x1 => x1.Parent == actionId + "_SPEC").ToList();
                if (specIdList.Length > 0)
                {
                    var allSpec = db.Specs.Where(x1 => specIdList.Contains(x1.Id)).ToList();
                    //var treeSpecBase = Spec.GetQueueParent(allSpec);
                    foreach (var s in specList)
                    {
                        if (allSpec.FirstOrDefault(x1 => x1.Parent == s.Id) != null)
                            s.LoadPartialTree(allSpec);
                        //foreach (var i in treeSpecBase)
                        //{
                        //    if (!i[0].LoadPartialTree(i))
                        //        throw new Exception("TODO ошибка");
                        //}
                    }
                    //specList = allSpec.Where(x1=>x1.Parent.Split(new string[] {"SPEC" },StringSplitOptions.RemoveEmptyEntries).Length==1).ToList();
                }




                //TODO че по оптимизации?
                // Получаем список временных характеристик для выбранного воздействия
                vremList = db.Vrems.Where(x1 => x1.Parent == actionId + "_VREM").ToList();
                if (vremIdList.Length > 0)
                {
                    var allVrem = db.Vrems.Where(x1 => vremIdList.Contains(x1.Id)).ToList();
                    //var treeVremBase = Vrem.GetQueueParent(allVrem);
                    foreach (var v in vremList)
                    {
                        if(allVrem.FirstOrDefault(x1=>x1.Parent==v.Id)!=null)
                        v.LoadPartialTree(allVrem);
                        //foreach (var i in treeVremBase)
                        //{
                        //    if (!i[0].LoadPartialTree(i))
                        //        throw new Exception("TODO ошибка");
                        //}
                    }
                    //vremList = allVrem.Where(x1 => x1.Parent.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries).Length == 1).ToList();
                }



                // Готовим данные для отправки в представление
                res.ActionId = listOfActions;
                res.ActionType = actionType;
                res.FizVelId = listOfFizVels;
                res.ParametricFizVelId = listOfParametricFizVels;
                res.Pros = prosList;// listSelectedPros;
                res.Specs = specList;//listSelectedSpec;
                res.Vrems = vremList;// listSelectedVrem;
                res.CurrentAction = actionId;
                res.CurrentActionId = "-1";
            }
            return res;
        }

        /// <summary>
        /// возвращает список всех параметрических воздействий загруженных в эту форму, одной строкой
        /// </summary>
        public string GetAllParametricAction()
        {
            return string.Join(" ", this.ActionId.Where(x1 => x1.Parametric).Select(x1 => x1.Id).ToList());
        }

    }
}