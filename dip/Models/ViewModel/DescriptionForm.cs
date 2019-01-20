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

            List<Pro> prosI = null;
            List<Spec> specI = null;
            List<Vrem> vremI = null;

            List<Pro> prosO = null;
            List<Spec> specO = null;
            List<Vrem> vremO = null;


            using (var db = new ApplicationDbContext())
            {
                //TODO вынести по классам в методы типо "gettext"
                res["NameI"] = db.AllActions.First(x1 => x1.Id == inp.Name).Name;
                res["TypeI"] = db.ActionTypes.First(x1 => x1.Id == inp.Type).Name;
                res["FizVelIdI"] = db.FizVels.First(x1 => x1.Id == inp.FizVelId).Name;
                res["FizVelparamI"] = db.FizVels.FirstOrDefault(x1 => x1.Id == inp.FizVelSection)?.Name;
                //
                prosI = db.Pros.Where(x1 => inp_pros.Contains(x1.Id)).ToList();
                specI = db.Specs.Where(x1 => inp_spec.Contains(x1.Id)).ToList();
                vremI = db.Vrems.Where(x1 => inp_vrem.Contains(x1.Id)).ToList();
                prosO = db.Pros.Where(x1 => outp_pros.Contains(x1.Id)).ToList();
                specO = db.Specs.Where(x1 => outp_spec.Contains(x1.Id)).ToList();
                vremO = db.Vrems.Where(x1 => outp_vrem.Contains(x1.Id)).ToList();

                //




                res["NameO"] = db.AllActions.First(x1 => x1.Id == outp.Name).Name;
                res["TypeO"] = db.ActionTypes.First(x1 => x1.Id == outp.Type).Name;
                res["FizVelIdO"] = db.FizVels.First(x1 => x1.Id == outp.FizVelId).Name;
                res["FizVelparamO"] = db.FizVels.FirstOrDefault(x1 => x1.Id == outp.FizVelSection)?.Name;

            }


            res["ProsI"] = string.Join(" ", Item<Pro>.GetQueueParentString(Item<Pro>.GetQueueParent(prosI)));// ;
            res["SpecsI"] = string.Join(" ", Item<Spec>.GetQueueParentString(Item<Spec>.GetQueueParent(specI)));
            res["VremsI"] = string.Join(" ", Item<Vrem>.GetQueueParentString(Item<Vrem>.GetQueueParent(vremI)));


            res["ProsO"] = string.Join(" ", Item<Pro>.GetQueueParentString(Item<Pro>.GetQueueParent(prosO)));
            res["SpecsO"] = string.Join(" ", Item<Spec>.GetQueueParentString(Item<Spec>.GetQueueParent(specO)));
            res["VremsO"] = string.Join(" ", Item<Vrem>.GetQueueParentString(Item<Vrem>.GetQueueParent(vremO)));

            return res;
        }


        //возвращает форму() для отображения
        public static DescriptionForm GetFormObject(string actionId,string fizVelId,string prosIds="", string specIds="", string vremIds="")
        {
            DescriptionForm res = new DescriptionForm();
            
            string[] prosIdList = prosIds?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)??new string[0];
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
                if (prosIdList.Count()>0)
                {
                    prosList = db.Pros.Where(x1 => prosIdList.Contains(x1.Id)).ToList();
                    var treeProBase = Item<Pro>.GetQueueParent(prosList);
                    foreach(var i in treeProBase)
                    {
                        if (!i[0].LoadPartialTree(i))
                            throw new Exception("TODO ошибка");
                    }
                }
                else
                
                    prosList = db.Pros.Where(x1 => x1.Parent == actionId + "_PROS").ToList();                    

                // Получаем список специальных характеристик для выбранного воздействия
                if ( specIdList.Count() > 0)
                {
                    specList = db.Specs.Where(x1 => specIdList.Contains(x1.Id)).ToList();
                    var treeSpecBase = Item<Spec>.GetQueueParent(specList);
                    foreach (var i in treeSpecBase)
                    {
                        if (!i[0].LoadPartialTree(i))
                            throw new Exception("TODO ошибка");
                    }
                }
                else
                
                    specList = db.Specs.Where(x1 => x1.Parent == actionId + "_SPEC").ToList();

                
                // Получаем список временных характеристик для выбранного воздействия
                if ( vremIdList.Count() > 0)
                {
                    vremList = db.Vrems.Where(x1 => vremIdList.Contains(x1.Id)).ToList();
                    var treeVremBase = Item<Vrem>.GetQueueParent(vremList);
                    foreach (var i in treeVremBase)
                    {
                        if (!i[0].LoadPartialTree(i))
                            throw new Exception("TODO ошибка");
                    }
                }
                else
                
                    vremList = db.Vrems.Where(x1 => x1.Parent == actionId + "_VREM").ToList();

                

                //vremList = db.Vrems.Where(x1 => vremIdList.Contains(x1.Id)).ToList();
                //var treeVremBase = Item<Vrem>.GetQueueParent(vremList);




                
                vremList = db.Vrems.Where(vrem => vrem.Parent == actionId + "_VREM").ToList();
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