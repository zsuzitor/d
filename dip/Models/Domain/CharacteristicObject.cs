using dip.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    //характеристики объекта(только вход или только выход), все 3 фазы
    public class CharacteristicObject
    {
        public List<PhaseCharacteristicObject> Phase1 { get; set; }
        public string ParamPhase1 { get; set; }
        public List<PhaseCharacteristicObject> Phase2 { get; set; }
        public string ParamPhase2 { get; set; }

        public List<PhaseCharacteristicObject> Phase3 { get; set; }
        public string ParamPhase3 { get; set; }



        public CharacteristicObject()
        {

        }

        /// <summary>
        /// выстраивает древо из списка переданных элементов
        /// </summary>
        /// <param name="Characteristics"></param>
        /// <param name="allidslist">список элементов которые нужно выделить</param>
        public void LoadTreePhasesForChilds(List<string> Characteristics, List<string> allidslist)
        {
            for (var charac = 0; charac < Characteristics.Count; ++charac)
            {
                //var prosIdList = CharacteristicStart[charac]?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                List<PhaseCharacteristicObject> prosList = new List<PhaseCharacteristicObject>();
                List<List<PhaseCharacteristicObject>> treeProBase = null;
                var allids = PhaseCharacteristicObject.GetAllIdsFor(Characteristics[charac]);
                allidslist.Add(allids);
                if (allids == null)
                    break;
                var prosIdList = allids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (prosIdList.Length > 0)
                {
                    using (var db = new ApplicationDbContext())//TODO using in this controller
                    {
                        //var prosList = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == "DESCOBJECT").ToList();

                        var allPros = db.PhaseCharacteristicObjects.Where(x1 => prosIdList.Contains(x1.Id)).ToList();
                        treeProBase = PhaseCharacteristicObject.GetQueueParent(allPros);


                        switch (charac)
                        {
                            case 0:
                                prosList = this.Phase1;
                                break;


                            case 1:

                                prosList = this.Phase2;
                                break;

                            case 2:

                                prosList = this.Phase3;
                                break;

                        }



                    }
                    foreach (var p in prosList)
                    {
                        foreach (var i in treeProBase)
                        {
                            if (p.Id == i[0].Id)
                                if (!p.LoadPartialTree(i))
                                    throw new Exception("TODO ошибка");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// загружает в фазы копию! списка фаз(только 1 уровень(родители без родителей))
        /// </summary>
        /// <param name="countPhase"></param>
        /// <param name="basePhase">если список пустой, попытается его получить из бд</param>
        public void SetFirstLvlStates(int? countPhase, List<PhaseCharacteristicObject> basePhase)
        {
            if (basePhase == null || basePhase.Count == 0)
                basePhase = PhaseCharacteristicObject.GetBase();
            switch (countPhase)//TODO в метод
            {
                case 1:
                    this.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                    break;


                case 2:
                    //res.CharacteristicsStart.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList(); 
                    this.Phase2 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                    goto case 1;
                //break;

                case 3:
                    //res.CharacteristicsStart.Phase1 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList(); 
                    //res.CharacteristicsStart.Phase2 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList(); 
                    this.Phase3 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                    goto case 2;
                    //break;

            }
        }

    }
}