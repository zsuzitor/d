/*файл класса предназначенного для хранения характеристик объекта(3 фазы)
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/


using dip.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    /// <summary>
    /// характеристики объекта(только вход или только выход), все 3 фазы
    /// </summary>
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
        /// выстраивает древо из списка переданных элементов и заносит в this.PhaseN
        /// </summary>
        /// <param name="Characteristics">id записей которые должны войти в итоговое древо</param>
        public void LoadTreePhasesForChilds(List<string> Characteristics)//
        {
            for (var charac = 0; charac < Characteristics.Count; ++charac)
            {
                var prosIdList = Characteristics[charac]?.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                List<PhaseCharacteristicObject> prosList = new List<PhaseCharacteristicObject>();


                if (Characteristics == null)
                    break;
                var allPros = new List<PhaseCharacteristicObject>();
                if (Characteristics.Count > 0)
                {
                    using (var db = new ApplicationDbContext())//TODO using in this controller
                    {
                        allPros = db.PhaseCharacteristicObjects.Where(x1 => prosIdList.Contains(x1.Id)).ToList();
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
                    if (prosList != null)
                        foreach (var p in prosList)
                        {
                            if (allPros.FirstOrDefault(x1 => x1.Id[0] == p.Id[0]) != null)
                                p.LoadPartialTree(allPros);
                        }
                }
            }
        }


        /// <summary>
        /// загружает в фазы(this.PhaseN) копию! списка фаз(только 1 уровень(родители без родителей))
        /// </summary>
        /// <param name="countPhase">количество фаз</param>
        /// <param name="basePhase">список фаз для копирования. если список пустой, попытается  получить из бд список базовых фаз(верхний уровень)</param>
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
                    this.Phase2 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                    goto case 1;
                case 3:
                    this.Phase3 = basePhase.Select(x1 => x1.CloneWithOutRef()).ToList();
                    goto case 2;

            }
        }

    }
}