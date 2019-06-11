/*файл класса для отображения входных и выходных дескрипторов
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Models.ViewModel
{

    /// <summary>
    /// класс для отображения входных\выходных дескрипторов
    /// </summary>
    public class DescriptionForm
    {
        public List<Domain.AllAction> ActionId { get; set; }
        public List<Domain.ActionType> ActionType { get; set; }
        public List<Domain.FizVel> FizVelId { get; set; }
        public List<Domain.FizVel> ParametricFizVelId { get; set; }
        public List<Pro> Pros { get; set; }
        public List<Spec> Specs { get; set; }
        public List<Vrem> Vrems { get; set; }
        public string CurrentAction { get; set; }
        public string CurrentActionId { get; set; }



        public DescriptionForm()
        {
            ActionId = new List<AllAction>();
            ActionType = new List<Domain.ActionType>();
            FizVelId = new List<FizVel>();
            ParametricFizVelId = new List<FizVel>();
            Pros = new List<Pro>();
            Specs = new List<Spec>();
            Vrems = new List<Vrem>();

        }

        /// <summary>
        /// метод возвращает только данные для отображения(список полей и чему равны) (текстовое представление, то что видно при просмотре дескрипторов ФЭ)
        /// </summary>
        /// <param name="idfe">id ФЭ</param>
        /// <returns>словарь с данными</returns>
        public static Dictionary<string, string> GetFormShow(int idfe)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            List<FEAction> inp = null;
            List<FEAction> outp = null;
            FEAction.Get(idfe, ref inp, ref outp);

            int iter = 0;
            foreach (var i2 in inp)
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
                    res["NameI" + iter] = db.AllActions.First(x1 => x1.Id == i2.Name).Name;
                    res["TypeI" + iter] = db.ActionTypes.First(x1 => x1.Id == i2.Type).Name;
                    res["FizVelIdI" + iter] = db.FizVels.First(x1 => x1.Id == i2.FizVelId).Name;
                    res["FizVelparamI" + iter] = db.FizVels.FirstOrDefault(x1 => x1.Id == i2.FizVelSection)?.Name;


                    List<Pro> prs = new List<Pro>();
                    if (inpPros.Length > 0)
                        prs = db.Pros.Where(x1 => inpPros.Contains(x1.Id)).ToList();
                    prosI = Pro.GetQueueParent(prs);

                    List<Spec> specs = new List<Spec>();
                    if (inpSpec.Length > 0)
                        specs = db.Specs.Where(x1 => inpSpec.Contains(x1.Id)).ToList();
                    specI = Spec.GetQueueParent(specs);

                    List<Vrem> vrems = new List<Vrem>();
                    if (inpVrem.Length > 0)
                        vrems = db.Vrems.Where(x1 => inpVrem.Contains(x1.Id)).ToList();
                    vremI = Vrem.GetQueueParent(vrems);

                }
                res["ProsI" + iter] = string.Join(" ", Pro.GetQueueParentString(prosI));
                res["SpecsI" + iter] = string.Join(" ", Spec.GetQueueParentString(specI));
                res["VremsI" + iter] = string.Join(" ", Vrem.GetQueueParentString(vremI));
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

                }


                res["ProsO" + iter] = string.Join(" ", Pro.GetQueueParentString(prosO));
                res["SpecsO" + iter] = string.Join(" ", Spec.GetQueueParentString(specO));
                res["VremsO" + iter] = string.Join(" ", Vrem.GetQueueParentString(vremO));

            }

            return res;
        }


        /// <summary>
        /// метод возвращает объект формы для отображения
        /// </summary>
        /// <param name="actionId">id воздействия</param>
        /// <param name="fizVelId">id физической величины</param>
        /// <param name="prosIds">id пространственных характеристик</param>
        /// <param name="specIds">id специальных характеристик</param>
        /// <param name="vremIds">id временных характеристик</param>
        /// <returns></returns>
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
                var listOfActions = db.AllActions.OrderBy(action => action.Id).ToList();

                if (string.IsNullOrWhiteSpace(actionId))
                    actionId = listOfActions.FirstOrDefault()?.Id;

                if (string.IsNullOrWhiteSpace(actionId))
                    return res;
                var actionType = db.ActionTypes.OrderByDescending(type => type.Name).ToList();//, "id", "name", "Не выбрано")


                List<FizVel> listOfFizVels;

                listOfFizVels = db.FizVels.Where(fizVel => (fizVel.Parent == actionId + "_FIZVEL") ||
                                                                            (fizVel.Id == "NO_FIZVEL"))
                                                       .OrderBy(fizVel => fizVel.Id).ToList();

                if (string.IsNullOrWhiteSpace(fizVelId))
                    fizVelId = listOfFizVels.First().Id;

                var listOfParametricFizVels = db.FizVels.Where(parametricFizVel => (parametricFizVel.Parent == fizVelId))
                                                                     .OrderBy(parametricFizVel => parametricFizVel.Id).ToList();

                prosList = db.Pros.Where(x1 => x1.Parent == actionId + "_PROS").ToList();
                if (prosIdList.Length > 0)
                {
                    var allPros = db.Pros.Where(x1 => prosIdList.Contains(x1.Id)).ToList();
                    foreach (var p in prosList)
                    {
                        if (allPros.FirstOrDefault(x1 => x1.Parent == p.Id) != null)
                            p.LoadPartialTree(allPros);
                    }

                }


                specList = db.Specs.Where(x1 => x1.Parent == actionId + "_SPEC").ToList();
                if (specIdList.Length > 0)
                {
                    var allSpec = db.Specs.Where(x1 => specIdList.Contains(x1.Id)).ToList();
                    foreach (var s in specList)
                    {
                        if (allSpec.FirstOrDefault(x1 => x1.Parent == s.Id) != null)
                            s.LoadPartialTree(allSpec);
                    }
                }


                vremList = db.Vrems.Where(x1 => x1.Parent == actionId + "_VREM").ToList();
                if (vremIdList.Length > 0)
                {
                    var allVrem = db.Vrems.Where(x1 => vremIdList.Contains(x1.Id)).ToList();
                    foreach (var v in vremList)
                    {
                        if (allVrem.FirstOrDefault(x1 => x1.Parent == v.Id) != null)
                            v.LoadPartialTree(allVrem);
                    }
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