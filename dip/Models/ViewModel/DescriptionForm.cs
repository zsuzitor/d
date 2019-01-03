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

        //возвращает форму(данные) для отображения
        public static DescriptionForm GetFormObject(string actionId,string fizVelId)
        {
            DescriptionForm res = new DescriptionForm();


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
                var prosList = db.Pros.Where(pros => pros.Parent == actionId + "_PROS").ToList();
                //var listSelectedPros = GetListSelectedItem(prosList);

                // Получаем список специальных характеристик для выбранного воздействия
                var specList = db.Specs.Where(spec => spec.Parent == actionId + "_SPEC").ToList();
                // var listSelectedSpec = GetListSelectedItem(specList);

                // Получаем список временных характеристик для выбранного воздействия
                var vremList = db.Vrems.Where(vrem => vrem.Parent == actionId + "_VREM").ToList();
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