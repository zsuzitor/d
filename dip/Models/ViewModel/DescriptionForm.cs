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
        public List<Domain.AllAction> actionId { get; set; }
        public List<Domain.ActionType> actionType { get; set; }
        public List<Domain.FizVel> fizVelId { get; set; }
        public List<Domain.FizVel> parametricFizVelId { get; set; }
        public List<Pro> pros { get; set; }
        public List<Spec> spec { get; set; }
        public List<Vrem> vrem { get; set; }
        public string currentAction { get; set; }
        public string currentActionId { get; set; }

        //public Domain.Action Action { get; set; }


        public DescriptionForm()
        {

        }

        //возвращает только данные для отображения(список полей и чему равны)
        public static Dictionary<string, string> GetFormShow(int idfe)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            FEAction inp = null;
            FEAction outp = null;
            FEAction.Get(idfe, ref inp, ref outp);

            string[] inp_pros = inp.Pros.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] inp_spec = inp.Spec.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] inp_vrem = inp.Vrem.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            string[] outp_pros = outp.Pros.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] outp_spec = outp.Spec.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] outp_vrem = outp.Vrem.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            List<List<Pro>> prosI = new List<List<Pro>>() ; 
            List<List<Spec>> specI = new List<List<Spec>>();
            List<List<Vrem>> vremI = new List<List<Vrem>>();

            List<List<Pro>> prosO = new List<List<Pro>>();
            List<List<Spec>> specO = new List<List<Spec>>();
            List<List<Vrem>> vremO = new List<List<Vrem>>();


            using (var db = new ApplicationDbContext())
            {
                //TODO вынести по классам в методы типо "gettext"
                res["NameI"] = db.AllActions.First(x1 => x1.Id == inp.Name).Name;
                res["TypeI"] = db.ActionTypes.First(x1 => x1.Id == inp.Type).Name;
                res["FizVelIdI"] = db.FizVels.First(x1 => x1.Id == inp.FizVelId).Name;
                res["FizVelparamI"] = db.FizVels.FirstOrDefault(x1 => x1.Id == inp.FizVelSection)?.Name;
                //
                //prosI = db.Pros.Where(x1 => inp_pros.Contains(x1.Id)).ToList();
                //specI = db.Specs.Where(x1 => inp_spec.Contains(x1.Id)).ToList();
                //vremI = db.Vrems.Where(x1 => inp_vrem.Contains(x1.Id)).ToList();
                //prosO = db.Pros.Where(x1 => outp_pros.Contains(x1.Id)).ToList();
                //specO = db.Specs.Where(x1 => outp_spec.Contains(x1.Id)).ToList();
                //vremO = db.Vrems.Where(x1 => outp_vrem.Contains(x1.Id)).ToList();


                //TODO сейчас очень не оптимизированно (на каждой итерации циклов загружаются по сути 1 и теже данные тк родитель у многих общий)
                foreach(var i in inp_pros)
                {
                    Pro pr = db.Pros.First(x1 => x1.Id == i);
                  var list=  pr.GetParentsList(db);
                    list.Add(pr);
                    prosI.Add(list);
                    //prosI.Add(string.Join("->",(( Pro.GetParents(i, db) + " " + i).Split(new string[]{" " },StringSplitOptions.RemoveEmptyEntries))));

                }
                foreach (var i in inp_spec)
                {
                    Spec sp = db.Specs.First(x1 => x1.Id == i);
                    var list = sp.GetParentsList(db);
                    list.Add(sp);
                    specI.Add(list);
                    //specI.Add(string.Join("->", (( Spec.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                }
                foreach (var i in inp_vrem)
                {
                    Vrem pr = db.Vrems.First(x1 => x1.Id == i);
                    var list = pr.GetParentsList(db);
                    list.Add(pr);
                    vremI.Add(list);
                    //vremI.Add(string.Join("->", (( Vrem.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                }
                foreach (var i in outp_pros)
                {
                    Pro pr = db.Pros.First(x1 => x1.Id == i);
                    var list = pr.GetParentsList(db);
                    list.Add(pr);
                    prosO.Add(list);
                    //prosO.Add(string.Join("->", ((Pro.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                }
                foreach (var i in outp_spec)
                {
                    Spec sp = db.Specs.First(x1 => x1.Id == i);
                    var list = sp.GetParentsList(db);
                    list.Add(sp);
                    specO.Add(list);
                    //specO.Add(string.Join("->", ((Spec.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                }
                foreach (var i in outp_vrem)
                {
                    Vrem pr = db.Vrems.First(x1 => x1.Id == i);
                    var list = pr.GetParentsList(db);
                    list.Add(pr);
                    vremO.Add(list);
                    //vremO.Add(string.Join("->", ((Vrem.GetParents(i, db) + " " + i).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))));
                }

                //




                res["NameO"] = db.AllActions.First(x1 => x1.Id == outp.Name).Name;
                res["TypeO"] = db.ActionTypes.First(x1 => x1.Id == outp.Type).Name;
                res["FizVelIdO"] = db.FizVels.First(x1 => x1.Id == outp.FizVelId).Name;
                res["FizVelparamO"] = db.FizVels.FirstOrDefault(x1 => x1.Id == outp.FizVelSection)?.Name;

            }


            res["ProsI"] = string.Join(" ", Pro.GetQueueParentString(prosI));// string.Join(" ", prosI);// ;
            res["SpecsI"] = string.Join(" ", Spec.GetQueueParentString(specI)); //string.Join(" ", specI); string.Join(" ", Spec.GetQueueParentString(Spec.GetQueueParent(specI)));
            res["VremsI"] = string.Join(" ", Vrem.GetQueueParentString(vremI));// string.Join(" ", vremI); //string.Join(" ", Vrem.GetQueueParentString(Vrem.GetQueueParent(vremI)));


            res["ProsO"] = string.Join(" ", Pro.GetQueueParentString(prosO));// string.Join(" ", prosO);// string.Join(" ", Pro.GetQueueParentString(Pro.GetQueueParent(prosO)));
            res["SpecsO"] = string.Join(" ", Spec.GetQueueParentString(specO)); //string.Join(" ", specO);// string.Join(" ", Spec.GetQueueParentString(Spec.GetQueueParent(specO)));
            res["VremsO"] = string.Join(" ", Vrem.GetQueueParentString(vremO));// string.Join(" ", vremO); //string.Join(" ", Vrem.GetQueueParentString(Vrem.GetQueueParent(vremO)));

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
                    actionId = listOfActions.First().Id;



                // Получаем список типов воздействий     
                var actionType = db.ActionTypes.OrderByDescending(type => type.Name).ToList();//, "id", "name", "Не выбрано")

                // Получаем список физических величин для выбранного воздействия
                var listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == actionId + "_FIZVEL") ||
                                                                              (fizVel.Id == "NO_FIZVEL"))
                                                           .OrderBy(fizVel => fizVel.Id).ToList();

                // Выбираем  из списка раздел физики
                if (string.IsNullOrWhiteSpace(fizVelId))
                    fizVelId = listOfFizVels.First().Id;

                // Получаем список физических величин для параметрических воздействий
                var listOfParametricFizVels = db.FizVels.Where(parametricFizVel => (parametricFizVel.Parent == fizVelId))
                                                                     .OrderBy(parametricFizVel => parametricFizVel.Id).ToList();

                // Получаем список пространственных характеристик для выбранного воздействия
                prosList = db.Pros.Where(x1 => x1.Parent == actionId + "_PROS").ToList();

                {
                    var allPros = db.Pros.Where(x1 => prosIdList.Contains(x1.Id)).ToList();
                    var treeProBase = Item<Pro>.GetQueueParent(allPros);
                    foreach (var p in prosList)
                    {
                        foreach (var i in treeProBase)
                        {
                            if (p.Id == i[0].Id)
                                if (!p.LoadPartialTree(i))
                                    throw new Exception("TODO ошибка");
                        }
                    }

                    // prosList = allPros.Where(x1 => x1.Parent.Split(new string[] { "PROS" }, StringSplitOptions.RemoveEmptyEntries).Length == 1).ToList();
                }




                // Получаем список специальных характеристик для выбранного воздействия
                specList = db.Specs.Where(x1 => x1.Parent == actionId + "_SPEC").ToList();

                {
                    var allSpec = db.Specs.Where(x1 => specIdList.Contains(x1.Id)).ToList();
                    var treeSpecBase = Item<Spec>.GetQueueParent(allSpec);
                    foreach (var s in specList)
                    {
                        foreach (var i in treeSpecBase)
                        {
                            if (!i[0].LoadPartialTree(i))
                                throw new Exception("TODO ошибка");
                        }
                    }
                    //specList = allSpec.Where(x1=>x1.Parent.Split(new string[] {"SPEC" },StringSplitOptions.RemoveEmptyEntries).Length==1).ToList();
                }





                // Получаем список временных характеристик для выбранного воздействия
                vremList = db.Vrems.Where(x1 => x1.Parent == actionId + "_VREM").ToList();

                {
                    var allVrem = db.Vrems.Where(x1 => vremIdList.Contains(x1.Id)).ToList();
                    var treeVremBase = Item<Vrem>.GetQueueParent(allVrem);
                    foreach (var v in vremList)
                    {

                        foreach (var i in treeVremBase)
                        {
                            if (!i[0].LoadPartialTree(i))
                                throw new Exception("TODO ошибка");
                        }
                    }
                    //vremList = allVrem.Where(x1 => x1.Parent.Split(new string[] { "VREM" }, StringSplitOptions.RemoveEmptyEntries).Length == 1).ToList();
                }






                //vremList = db.Vrems.Where(x1 => vremIdList.Contains(x1.Id)).ToList();
                //var treeVremBase = Item<Vrem>.GetQueueParent(vremList);





                // vremList = db.Vrems.Where(vrem => vrem.Parent == actionId + "_VREM").ToList();
                //var listSelectedVrem = GetListSelectedItem(vremList);

                // Готовим данные для отправки в представление
                res.actionId = listOfActions;
                res.actionType = actionType;
                res.fizVelId = listOfFizVels;
                res.parametricFizVelId = listOfParametricFizVels;
                res.pros = prosList;// listSelectedPros;
                res.spec = specList;//listSelectedSpec;
                res.vrem = vremList;// listSelectedVrem;
                res.currentAction = actionId;
                res.currentActionId = "-1";
            }
            return res;
        }

    }
}