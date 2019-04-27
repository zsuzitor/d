using Binbin.Linq;
using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace dip.Models
{

    /// <summary>
    /// класс для сохранения дескрипторов объекта
    /// </summary>
    public class SaveDescriptionObject
    {
        public SaveDescriptionObjectEntry[] MassAddCharacteristic { get; set; }
        public SaveDescriptionObjectEntry[] MassEditCharacteristic { get; set; }
        public SaveDescriptionObjectEntry[] MassEditState { get; set; }
        public SaveDescriptionObjectEntry[] MassDeleteCharacteristic { get; set; }

        public SaveDescriptionObject()
        {

        }

        /// <summary>
        /// метод для сохранения
        /// </summary>
        /// <param name="commited"></param>
        /// <returns></returns>
        public List<int> Save(out bool commited)
        {
            commited = false;
            List<int> blockFe = new List<int>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            using (var transaction = db.Database.BeginTransaction())
                try
                {
                    this.AddCharacteristic(db);
                    this.EditCharacteristic(db);
                    this.EditState(db);
                    blockFe.AddRange(this.DeleteCharacteristic(db));
                    transaction.Commit();
                    commited = true;
                }
                catch
                {
                    transaction.Rollback();
                }
            return blockFe;
        }

        /// <summary>
        /// метод для установления пустых массивов если свойства равны null
        /// </summary>
        public void SetNotNullArray()
        {
            MassAddCharacteristic = MassAddCharacteristic ?? new SaveDescriptionObjectEntry[0];
            MassEditCharacteristic = MassEditCharacteristic ?? new SaveDescriptionObjectEntry[0];
            MassEditState = MassEditState ?? new SaveDescriptionObjectEntry[0];
            MassDeleteCharacteristic = MassDeleteCharacteristic ?? new SaveDescriptionObjectEntry[0];
        }

        /// <summary>
        /// метод для получения детей характеристик
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<SaveDescriptionObjectEntry> AllChildsCharacObj(List<SaveDescriptionObjectEntry> mass, string id)
        {
            List<SaveDescriptionObjectEntry> res = new List<SaveDescriptionObjectEntry>();
            var items = mass.Where(x1 => x1.ParentId == id).ToList();
            res.AddRange(items);
            foreach (var i in items)
            {
                res.AddRange(SaveDescriptionObject.AllChildsCharacObj(mass, i.Id));
            }
            return res;
        }


        /// <summary>
        /// метод для добавления характеристик
        /// </summary>
        /// <param name="db"></param>
        public void AddCharacteristic(ApplicationDbContext db)
        {
            Dictionary<string, List<SaveDescriptionObjectEntry>> MainTree = new Dictionary<string, List<SaveDescriptionObjectEntry>>();
            List<SaveDescriptionObjectEntry> MassAddCharacteristicList = this.MassAddCharacteristic.ToList();
            foreach (var i in this.MassAddCharacteristic)
                if (!i.ParentId.Contains("NEW"))
                {
                    if (!MainTree.ContainsKey(i.ParentId))
                        MainTree[i.ParentId] = SaveDescriptionObject.AllChildsCharacObj(MassAddCharacteristicList, i.ParentId);
                }
            var AllcharDb = db.PhaseCharacteristicObjects.ToList();
            string letterPart = "";
            string groupName = "letter";
            Regex letterPartR = new Regex(@"^(?<" + groupName + @">\D+)\d*$");
            foreach (var i in MainTree)
            {
                if (i.Value.Count > 0)
                {
                    var match = letterPartR.Match(i.Key);
                    letterPart = match.Groups[groupName].Value;

                    Regex letterPartN = new Regex(@"^" + letterPart + @"\d*$");
                    int last = AllcharDb.Where(x1 =>
                    {
                        return letterPartN.IsMatch(x1.Id);
                    }).Max(x1 =>
                       {
                           string[] tmpstr = x1.Id.Split(new string[] { letterPart }, StringSplitOptions.RemoveEmptyEntries);
                           if (tmpstr.Length > 0)
                               return int.Parse(tmpstr[0]);
                           else
                               return 0;
                       });
                    foreach (var i2 in i.Value)//TODO не уверен что не возникнет ошибки, тк они не сортируются специально, сотировка задается в функции AllChildsCharacObj
                    {
                        PhaseCharacteristicObject ob = new PhaseCharacteristicObject()
                        {
                            Id = letterPart + ++last,// i2.Id.Split(new string[] {"NEW" },StringSplitOptions.RemoveEmptyEntries)[0],
                            Parent = i2.ParentId,
                            Name = i2.Text
                        };
                        db.PhaseCharacteristicObjects.Add(ob);
                        db.SaveChanges();
                        AllcharDb.Add(ob);
                        foreach (var i3 in MainTree[i.Key])
                        {
                            if (i3.ParentId == i2.Id)
                                i3.ParentId = ob.Id;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// метод для редактирования характеристик
        /// </summary>
        /// <param name="db"></param>
        public void EditCharacteristic(ApplicationDbContext db)
        {
            foreach (var i in this.MassEditCharacteristic)
            {
                var objCharac = db.PhaseCharacteristicObjects.FirstOrDefault(x1 => x1.Id == i.Id);
                if (objCharac != null)
                {
                    objCharac.Name = i.Text;
                    db.SaveChanges();
                }
            }
        }


        /// <summary>
        /// метод для редактирования состояний
        /// </summary>
        /// <param name="db"></param>
        public void EditState(ApplicationDbContext db)
        {
            //var db = db_ ?? new ApplicationDbContext();

            foreach (var i in this.MassEditState)
            {
                var objCharac = db.StateObjects.FirstOrDefault(x1 => x1.Id == i.Id);
                if (objCharac != null)
                {
                    objCharac.Name = i.Text;
                    db.SaveChanges();
                }
            }
            //if (db_ == null)
            //    db.Dispose();
        }


        /// <summary>
        /// метод для удаления характеристик
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public List<int> DeleteCharacteristic(ApplicationDbContext db)
        {
            List<PhaseCharacteristicObject> fordel = new List<PhaseCharacteristicObject>();
            List<string> MassDeleteCharacteristicStr = MassDeleteCharacteristic.Select(x1 => x1.Id).ToList();
            var childcol = db.PhaseCharacteristicObjects.Where(x1 => MassDeleteCharacteristicStr.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList();
            if (childcol.Count == 0)
                return new List<int>();
            foreach (var i in childcol)
            {
                fordel.Add(i);
                fordel.AddRange(i.GetChildsList(db));
            }
            //формировать регулярку по списку удаляемых объектов
            var predicate = PredicateBuilder.False<FEObject>();
            foreach (var i in fordel)
            {
                predicate = predicate.Or(x1 => x1.Composition == i.Id || x1.Composition.StartsWith(i.Id + " ") ||
                x1.Composition.EndsWith(" " + i.Id) || x1.Composition.Contains(" " + i.Id + " ") ||
                x1.Conductivity == i.Id || x1.Conductivity.StartsWith(i.Id + " ") ||
                x1.Conductivity.EndsWith(" " + i.Id) || x1.Conductivity.Contains(" " + i.Id + " ") ||
                x1.MagneticStructure == i.Id || x1.MagneticStructure.StartsWith(i.Id + " ") ||
                x1.MagneticStructure.EndsWith(" " + i.Id) || x1.MagneticStructure.Contains(" " + i.Id + " ") ||
                x1.MechanicalState == i.Id || x1.MechanicalState.StartsWith(i.Id + " ") ||
                x1.MechanicalState.EndsWith(" " + i.Id) || x1.MechanicalState.Contains(" " + i.Id + " ") ||
                x1.OpticalState == i.Id || x1.OpticalState.StartsWith(i.Id + " ") ||
                x1.OpticalState.EndsWith(" " + i.Id) || x1.OpticalState.Contains(" " + i.Id + " ") ||
                x1.PhaseState == i.Id || x1.PhaseState.StartsWith(i.Id + " ") ||
                x1.PhaseState.EndsWith(" " + i.Id) || x1.PhaseState.Contains(" " + i.Id + " ") ||
                x1.Special == i.Id || x1.Special.StartsWith(i.Id + " ") ||
                x1.Special.EndsWith(" " + i.Id) || x1.Special.Contains(" " + i.Id + " "));
            }

            var blocked = db.FEObjects.Where(predicate).Select(x1 => x1.Idfe).ToList();
            if (blocked.Count > 0)
                return blocked;

            AParentDb<PhaseCharacteristicObject>.DeleteFromDbFromListOnly(db, db.PhaseCharacteristicObjects, fordel.Select(x1 => x1.Id));
            return blocked;
        }
    }
}