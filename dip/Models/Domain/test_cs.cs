﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using static dip.Models.Functions;


using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Web.Hosting;
using dip.Models.ViewModel;
//using Microsoft.SqlServer.Management.Common;




namespace dip.Models.Domain
{
    enum RolesProject { admin, subscriber, NotApproveUser, user };//vip
    //var a = (RolesProject)Enum.Parse(typeof(RolesProject), "", true);



    public class test
    {
        //[Index("PK_FeTexts_cons", IsClustered = true, IsUnique = true)]//,IsClustered =true
        public int Id { get; set; }

        public string levName { get; set; }
        public string levText { get; set; }
        public string levTextInp { get; set; }
        public string levTextOut { get; set; }
        public string levTextObj { get; set; }
        public string levTextApp { get; set; }
        public string levTextLit { get; set; }


        public test()
        {

        }
    }


   


    public class PhaseCharacteristicObject : AParentDb<PhaseCharacteristicObject>
    {
        // public string Id { get; set; }
        public string Name { get; set; }
        // public string Parent { get; set; }


        //[NotMapped]
        //public List<CharacteristicObject> Childs { get; set; }
        //[NotMapped]
        //public CharacteristicObject ParentItem { get; set; }




        public PhaseCharacteristicObject()
        {

        }



        public static List<PhaseCharacteristicObject> GetBase()
        {
            List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            using (var db = new ApplicationDbContext())
                res = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == "DESCOBJECT").ToList();
            return res;
        }


        public static string DeleteNotChildCheckbox(string strIds)
        {

            string res = "";
            var listId = strIds.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in listId)
            {
                var listItem = PhaseCharacteristicObject.GetChild(i);
                if (listItem.Count == 0)
                    res += i + " ";
                else
                {
                    bool needAdd = true;
                    foreach (var i2 in listItem)
                    {
                        if (listId.Contains(i2.Id))
                            needAdd = false;
                    }
                    if (needAdd)
                        res += i + " ";
                }


            }

            return res.Trim();

        }


        public static List<PhaseCharacteristicObject> GetChild(string id)
        {
            // Получаем список значений, соответствующий данной характеристике
            List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            using (var db = new ApplicationDbContext())
                res = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == id).ToList();
            return res;

        }


        public PhaseCharacteristicObject CloneWithOutRef()
        {
            return new PhaseCharacteristicObject()
            {

                Id = this.Id,
                Name = this.Name,
                Parent = this.Parent
            };


        }

        public static PhaseCharacteristicObject Get(string id)
        {
            PhaseCharacteristicObject res = null;
            if (!string.IsNullOrWhiteSpace(id))
                using (var db = new ApplicationDbContext())
                    res = db.PhaseCharacteristicObjects.FirstOrDefault(x1 => x1.Id == id);
            return res;
        }


        

        public override void ReLoadChild()
        {

            using (var db = new ApplicationDbContext())
                this.Childs = db.PhaseCharacteristicObjects.Where(x1 => x1.Parent == this.Id).ToList();
        }





        public override List<PhaseCharacteristicObject> GetParentsList(ApplicationDbContext db_ = null)
        {
            List<PhaseCharacteristicObject> res = new List<PhaseCharacteristicObject>();
            var db = db_ ?? new ApplicationDbContext();

            var par = db.PhaseCharacteristicObjects.FirstOrDefault(x1 => x1.Id == this.Parent);

            if (par != null)
            {
                if (par.Parent != "DESCOBJECT")
                    res.AddRange(par.GetParentsList(db));

                res.Add(par);
            }



            if (db_ == null)
                db.Dispose();

            return res;
        }


        public static string GetAllIdsFor(string str)
        {
            //из строки только детей формирует строку со всеми(дети+родители) id которые нужно выделить
            if (string.IsNullOrWhiteSpace(str))
                return "";

            List<PhaseCharacteristicObject> mainLst = new List<PhaseCharacteristicObject>();
            var strmass = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (strmass == null)
                return null;
            foreach (var i in strmass)
            {
                using (var db = new ApplicationDbContext())
                {
                    var pr = db.PhaseCharacteristicObjects.First(x1 => x1.Id == i);

                    var lstPr = pr.GetParentsList();
                    lstPr.Add(pr);
                    mainLst.AddRange(lstPr);
                }
            }
            return string.Join(" ", mainLst.Select(x1 => x1.Id).Distinct());

        }


     




    }

    public class DescrPhaseI : FEObject
    {

        public DescrPhaseI()
        {

        }
        public DescrPhaseI(FEObject a)
        {
            if (a != null)
            {
                Id = a.Id;
                Idfe = a.Idfe;
                Begin = a.Begin;
                NumPhase = a.NumPhase;

                PhaseState = a.PhaseState;
                Composition = a.Composition;
                MagneticStructure = a.MagneticStructure;
                Conductivity = a.Conductivity;
                MechanicalState = a.MechanicalState;
                OpticalState = a.OpticalState;
                Special = a.Special;
                //NumPhase = 1;
            }

        }

        public string GetListStr_()//TODO
        {
            string res = "";
            res += PhaseState + " " +
                Composition + " " +
                MagneticStructure + " " +
                Conductivity + " " +
                MechanicalState + " " +
                OpticalState + " " +
                Special + " ";


            return res;
        }

        public void DeleteNotChildCheckbox()
        {
            this.PhaseState = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.PhaseState);
            this.Composition = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.Composition);
            this.MagneticStructure = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.MagneticStructure);
            this.Conductivity = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.Conductivity);
            this.MechanicalState = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.MechanicalState);
            this.OpticalState = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.OpticalState);
            this.Special = PhaseCharacteristicObject.DeleteNotChildCheckbox(this.Special);

        }

        public static bool Validation(DescrPhaseI a)
        {
            if (a != null)
            {
                // a.DeleteNotChildCheckbox();
                //TODO валидация
                a.PhaseState = NullToEmpryStr(a?.PhaseState);
                a.Composition = NullToEmpryStr(a?.Composition);
                a.MagneticStructure = NullToEmpryStr(a?.MagneticStructure);
                a.Conductivity = NullToEmpryStr(a?.Conductivity);
                a.MechanicalState = NullToEmpryStr(a?.MechanicalState);
                a.OpticalState = NullToEmpryStr(a?.OpticalState);
                a.Special = NullToEmpryStr(a?.Special);



                a.SortIds();
            }

            return true;
        }


        public bool SortIds()//TODO
        {
            bool res = true;
            if (string.IsNullOrWhiteSpace(PhaseState))
                return false;
            // var gg = ids.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string resStr = string.Join(" ", PhaseState.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).
                     OrderBy(x1 => x1).Distinct().ToList());
            return res;
        }

    }


    public class DescrObjectI
    {
        public DescrPhaseI ListSelectedPhase1 { get; set; }
        public DescrPhaseI ListSelectedPhase2 { get; set; }
        public DescrPhaseI ListSelectedPhase3 { get; set; }

        public bool Begin { get; set; }//начальное или конечное состояние
                                       //public int NumPhase { get; set; }

        public bool Valide { get; private set; }//мб private
        public DescrObjectI()
        {
            Begin = true;
            ListSelectedPhase1 = null;
            ListSelectedPhase2 = null;
            ListSelectedPhase3 = null;
            Valide = false;
            //NumPhase = 1;
        }

        public int GetCountPhase()
        {
            int res = 0;
            if (this.ListSelectedPhase1 != null)
            {
                ++res;
                if (this.ListSelectedPhase2 != null)
                {
                    ++res;
                    if (this.ListSelectedPhase3 != null)
                    {
                        ++res;
                    }
                }

            }
            return res;
        }

        public List<DescrPhaseI> GetActualPhases()
        {
            List<DescrPhaseI> res = new List<DescrPhaseI>();
            if (this.ListSelectedPhase1 != null)
            {
                res.Add(this.ListSelectedPhase1);
                if (this.ListSelectedPhase2 != null)
                {
                    res.Add(this.ListSelectedPhase2);
                    if (this.ListSelectedPhase3 != null)
                    {
                        res.Add(this.ListSelectedPhase3);
                    }
                }

            }
            return res;
        }



        public List<string> GetList_()//TODO
        {
            List<string> res = new List<string>()
            {
                ListSelectedPhase1?.GetListStr_(),
                ListSelectedPhase2?.GetListStr_(),
                ListSelectedPhase3?.GetListStr_()
            };


            return res;
        }



        public void DeleteNotChildCheckbox()
        {

            ListSelectedPhase1?.DeleteNotChildCheckbox();
            ListSelectedPhase2?.DeleteNotChildCheckbox();
            ListSelectedPhase3?.DeleteNotChildCheckbox();


        }


        public static bool Validation(DescrObjectI a)
        {
            bool res = true;
            if (a != null)
            {
                if (a.GetCountPhase() == 0)
                    res = false;
                
                DescrPhaseI.Validation(a.ListSelectedPhase1);
                DescrPhaseI.Validation(a.ListSelectedPhase2);
                DescrPhaseI.Validation(a.ListSelectedPhase3);

                

            }
            //a.Valide = false;
            //if(!res)
                a.Valide = res;
            //a.Valide = true;
            return res;
        }


    }


    public class DescriptionFormWithData {

        public DescriptionForm Form { get; set; }
        public DescrSearchI FormData { get; set; }


        public DescriptionFormWithData()
        {

        }
    }

    //хранит только данные для заполнения в дескрипторной форме, саму форму не хранит
    public class DescrSearchI
    {

        public string ActionId { get; set; }
        public bool? Parametric { get; set; }
        public string ActionType { get; set; }
        public string FizVelId { get; set; }
        public string ParametricFizVelId { get; set; }
        public string ListSelectedPros { get; set; }
        public string ListSelectedSpec { get; set; }
        public string ListSelectedVrem { get; set; }

        public bool InputForm { get; set; }//вход\выход

        [ScaffoldColumn(false)]
        public bool Valide { get; private set; }//мб private

        public DescrSearchI()
        {
            Parametric = null;
            InputForm = true;
            Valide = false;
        }
      




        public bool? CheckParametric()
        {
            this.Parametric = AllAction.CheckParametric(this.ActionId);
            return this.Parametric;
        }

        public DescrSearchI(FEAction a)
        {

            this.ActionId = a.Name;
            this.ActionType = a.Type;
            this.FizVelId = a.FizVelId;
            this.ListSelectedPros = a.Pros;
            this.ListSelectedSpec = a.Spec;
            this.ListSelectedVrem = a.Vrem;
            this.ParametricFizVelId = a.FizVelSection;

        }



        public static bool IsNull(DescrSearchI a)
        {
            if (a == null)
                return true;
            if (string.IsNullOrWhiteSpace(a.ActionId) && string.IsNullOrWhiteSpace(a.ActionType) && string.IsNullOrWhiteSpace(a.FizVelId)
                && string.IsNullOrWhiteSpace(a.ParametricFizVelId) && string.IsNullOrWhiteSpace(a.ListSelectedPros)
                && string.IsNullOrWhiteSpace(a.ListSelectedSpec) && string.IsNullOrWhiteSpace(a.ListSelectedVrem))
                return true;


            return false;
        }


        public void DeleteNotChildCheckbox()
        {

            //pro
            this.ListSelectedPros = Pro.DeleteNotChildCheckbox(this.ListSelectedPros);



            //spec
            this.ListSelectedSpec = Spec.DeleteNotChildCheckbox(this.ListSelectedSpec);



            //vrem
            this.ListSelectedVrem = Vrem.DeleteNotChildCheckbox(this.ListSelectedVrem);


        }


        public static bool ValidationIfNeed(DescrSearchI a)
        {
            var res = a?.Valide ?? DescrSearchI.Validation(a);
            //if (res == null)
            //    res = DescrSearchIInput.Validation(a);
            return res;
        }

        public static bool Validation(DescrSearchI a)
        {
            bool res = true;
            if (a != null)
            {
                a.ActionId = NullToEmpryStr(a?.ActionId);
                if(string.IsNullOrWhiteSpace(a.ActionId))
                
                    res= false;
                
                a.ActionType = NullToEmpryStr(a?.ActionType);
                if (string.IsNullOrWhiteSpace(a.ActionType))
                    res =false;
                
                a.FizVelId = NullToEmpryStr(a?.FizVelId);
                if (string.IsNullOrWhiteSpace(a.FizVelId))
                    res = false;
                
                a.ParametricFizVelId = NullToEmpryStr(a?.ParametricFizVelId);
                a.ListSelectedPros = NullToEmpryStr(a?.ListSelectedPros);
                a.ListSelectedSpec = NullToEmpryStr(a?.ListSelectedSpec);
                a.ListSelectedVrem = NullToEmpryStr(a?.ListSelectedVrem);


                a.ListSelectedPros = Pro.SortIds(a?.ListSelectedPros);
               

                a.ListSelectedSpec = Spec.SortIds(a?.ListSelectedSpec);
               
                a.ListSelectedVrem = Vrem.SortIds(a?.ListSelectedVrem);
               
                //if(res)
                //a.Valide = true;
                //else
                //    a.Valide = false;
                a.Valide = res;
            }
            //a.Valide = false;
            return res;
        }






    }




    

    public class FullTextSearch_
    {


        public static bool Create()
        {
           


            //1-создать каталог
            //2- добавить индекс
            //3- апнуть индекс до семантического
            List<string> files = new List<string>() { "create_catalog", "create_index", "alter_index_semantic" };

            var connection = new SqlConnection();
            connection.ConnectionString = Constants.sql_0;
            connection.Open();
            foreach (var i in files)
            {
                string script = File.ReadAllText(HostingEnvironment.MapPath($"~/tsqlscripts/{i}.txt"));

                using (var command = new SqlCommand(script, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            connection.Close();
            return true;
        }
    }


    public class SaveDescription
    {
        //public DescrSearchI test { get; set; }
        public SaveDescriptionEntry[] MassAddActionId { get; set; }
        public SaveDescriptionEntry[] MassAddFizVels { get; set; }
        public SaveDescriptionEntry[] MassAddParamFizVels { get; set; }
        public SaveDescriptionEntry[] MassAddPros { get; set; }
        public SaveDescriptionEntry[] MassAddVrems { get; set; }
        public SaveDescriptionEntry[] MassAddSpecs { get; set; }

        public SaveDescriptionEntry[] MassEditActionId { get; set; }
        public SaveDescriptionEntry[] MassEditFizVels { get; set; }
        public SaveDescriptionEntry[] MassEditParamFizVels { get; set; }
        public SaveDescriptionEntry[] MassEditPros { get; set; }
        public SaveDescriptionEntry[] MassEditVrems { get; set; }
        public SaveDescriptionEntry[] MassEditSpecs { get; set; }

        public SaveDescriptionEntry[] MassDeletedActionId { get; set; }
        public SaveDescriptionEntry[] MassDeletedFizVels { get; set; }
        public SaveDescriptionEntry[] MassDeletedParamFizVels { get; set; }
        public SaveDescriptionEntry[] MassDeletedPros { get; set; }
        public SaveDescriptionEntry[] MassDeletedVrems { get; set; }
        public SaveDescriptionEntry[] MassDeletedSpecs { get; set; }


        public SaveDescription()
    {
       
    }
        public void SetNotNullArray()
        {
            MassAddActionId=MassAddActionId ?? new SaveDescriptionEntry[0];
            MassAddFizVels = MassAddFizVels ?? new SaveDescriptionEntry[0];
            MassAddParamFizVels = MassAddParamFizVels ?? new SaveDescriptionEntry[0];
            MassAddPros = MassAddPros ?? new SaveDescriptionEntry[0];
            MassAddVrems = MassAddVrems ?? new SaveDescriptionEntry[0];
            MassAddSpecs = MassAddSpecs ?? new SaveDescriptionEntry[0];

            MassEditActionId = MassEditActionId ?? new SaveDescriptionEntry[0];
            MassEditFizVels = MassEditFizVels ?? new SaveDescriptionEntry[0];
            MassEditParamFizVels = MassEditParamFizVels ?? new SaveDescriptionEntry[0];
            MassEditPros = MassEditPros ?? new SaveDescriptionEntry[0];
            MassEditVrems = MassEditVrems ?? new SaveDescriptionEntry[0];
            MassEditSpecs = MassEditSpecs ?? new SaveDescriptionEntry[0];

            MassDeletedActionId = MassDeletedActionId ?? new SaveDescriptionEntry[0];
            MassDeletedFizVels = MassDeletedFizVels ?? new SaveDescriptionEntry[0];
            MassDeletedParamFizVels = MassDeletedParamFizVels ?? new SaveDescriptionEntry[0];
            MassDeletedPros = MassDeletedPros ?? new SaveDescriptionEntry[0];
            MassDeletedVrems = MassDeletedVrems ?? new SaveDescriptionEntry[0];
            MassDeletedSpecs = MassDeletedSpecs ?? new SaveDescriptionEntry[0];
        }


        

            public void AddFizVels(string currentActionId,bool? currentActionParametric,ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            List<FizVel> fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentActionId + "_FIZVEL")).ToList();
            //if (currentActionParametric == false)
            //    fizvels=//выберет и не параметрические
            //else
            //    fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentActionId + "_FIZVEL_R")).ToList();
            int lastFizVel = 0;
            if (fizvels.Count > 0)
                if (currentActionParametric == false)
                {
                    lastFizVel = fizvels.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_FIZVEL_") }, StringSplitOptions.RemoveEmptyEntries)[0]));
                }
                else
                {
                    lastFizVel = fizvels.Max(x1 =>
                    {
                        int rs;
                        int.TryParse(x1.Id.Split(new string[] { (currentActionId + "_FIZVEL_R") }, StringSplitOptions.RemoveEmptyEntries)[0], out rs);
                        return rs;
                    });
                }

            foreach (var i in MassAddFizVels)
            {
                string fizVelId = "";
                if (currentActionParametric == false)
                    fizVelId = (currentActionId + "_FIZVEL_" + ++lastFizVel);
                else
                    fizVelId = (currentActionId + "_FIZVEL_R" + ++lastFizVel);


                db.FizVels.Add(new Models.Domain.FizVel()
                {
                    Name = i.Text,
                    Parent = currentActionId + "_FIZVEL",
                    Id = fizVelId
                });
                db.SaveChanges();
                foreach (var i2 in MassAddParamFizVels)
                {
                    if (i2.ParentId == i.Id)
                        i2.ParentId = fizVelId;
                }
            }
            if (db_ == null)
                db.Dispose();
        }

        public void EditFizVels(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditFizVels)
            {
                var fizVel = db.FizVels.FirstOrDefault(x1 => x1.Id == i.Id);
                if (fizVel != null)
                    fizVel.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }
            if (db_ == null)
                db.Dispose();
        }
        public void AddParamFizVels(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            if (MassAddParamFizVels.Length > 0)
            {


                string currentFizVels = MassAddParamFizVels[0].ParentId;

                var fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentFizVels + "_")).ToList();//("VOZ" + currentActionId + "_FIZVEL_R"+ currentFizVels)
                int lastFizVel = 0;
                if (fizvels.Count > 0)
                    lastFizVel = fizvels.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentFizVels + "_") }, StringSplitOptions.RemoveEmptyEntries)[0]));



                foreach (var i in MassAddParamFizVels)
                {
                    db.FizVels.Add(new Models.Domain.FizVel()
                    {
                        Name = i.Text,
                        Parent = currentFizVels,
                        Id = (currentFizVels + "_" + ++lastFizVel),
                        Parametric = true
                    });
                    db.SaveChanges();
                }
            }

            if (db_ == null)
                db.Dispose();
        }

        public void EditParamFizVels(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            foreach (var i in MassEditParamFizVels)
            {
                var fizVel = db.FizVels.FirstOrDefault(x1 => x1.Id == i.Id);
                if (fizVel != null)
                    fizVel.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }

            if (db_ == null)
                db.Dispose();
        }

        public void AddPro(string currentActionId,ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            int last = 0;
            var pros = db.Pros.Where(x1 => x1.Id.Contains(currentActionId + "_PROS")).ToList();
            if (pros.Count > 0)
                last = pros.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_PROS") }, StringSplitOptions.RemoveEmptyEntries)[0]));
            //List<JsonSaveDescription> done = new List<JsonSaveDescription>();

            int countLastIter = 0;
            List<SaveDescriptionEntry> massAddProsList = MassAddPros.ToList();
            while (massAddProsList.Count > 0)
            {

                if (countLastIter == massAddProsList.Count)
                    throw new Exception("непонятные PRO");
                countLastIter = massAddProsList.Count;
                for (int i = 0; i < massAddProsList.Count; ++i)
                {
                    //проверить можно ли добавить, и добавить и вынести в массив
                    if (!massAddProsList[i].ParentId.Contains("_NEW"))
                    {
                        var pro = new Models.Domain.Pro()
                        {
                            Name = massAddProsList[i].Text,
                            Parent = (massAddProsList[i].ParentId.Contains("_PROS") ? massAddProsList[i].ParentId : massAddProsList[i].ParentId + "_PROS"),
                            Id = (currentActionId + "_PROS" + ++last)//VOZ1_PROS1
                        };

                        db.Pros.Add(pro);
                        db.SaveChanges();
                        for (int i2 = 0; i2 < massAddProsList.Count; ++i2)
                        {
                            if (massAddProsList[i2].ParentId == massAddProsList[i].Id)
                            {
                                massAddProsList[i2].ParentId = pro.Id;
                            }
                        }
                        massAddProsList.Remove(massAddProsList[i--]);

                    }


                }
            }



            if (db_ == null)
                db.Dispose();
        }


        public void EditPro(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditPros)
            {
                var act = db.Pros.FirstOrDefault(x1 => x1.Id == i.Id);
                if (act != null)
                    act.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }
            if (db_ == null)
                db.Dispose();
        }


        public void AddSpec(string currentActionId,ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            int last = 0;
            var specs = db.Specs.Where(x1 => x1.Id.Contains(currentActionId + "_SPEC")).ToList();
            if (specs.Count > 0)
                last = specs.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_SPEC") }, StringSplitOptions.RemoveEmptyEntries)[0]));
            //List<JsonSaveDescription> done = new List<JsonSaveDescription>();

            int countLastIter = 0;
            List<SaveDescriptionEntry> massAddSpecsList = MassAddSpecs.ToList();
            while (massAddSpecsList.Count > 0)
            {

                if (countLastIter == massAddSpecsList.Count)
                    throw new Exception("непонятные Spec");
                countLastIter = massAddSpecsList.Count;
                for (int i = 0; i < massAddSpecsList.Count; ++i)
                {
                    //проверить можно ли добавить, и добавить и вынести в массив
                    if (!massAddSpecsList[i].ParentId.Contains("_NEW"))
                    {
                        var spec = new Models.Domain.Spec()
                        {
                            Name = massAddSpecsList[i].Text,
                            Parent = (massAddSpecsList[i].ParentId.Contains("_SPEC") ? massAddSpecsList[i].ParentId : massAddSpecsList[i].ParentId + "_SPEC"),

                            Id = (currentActionId + "_SPEC" + ++last)
                        };

                        db.Specs.Add(spec);
                        db.SaveChanges();
                        for (int i2 = 0; i2 < massAddSpecsList.Count; ++i2)
                        {
                            if (massAddSpecsList[i2].ParentId == massAddSpecsList[i].Id)
                            {
                                massAddSpecsList[i2].ParentId = spec.Id;
                            }
                        }
                        massAddSpecsList.Remove(massAddSpecsList[i--]);

                    }


                }
            }


            if (db_ == null)
                db.Dispose();
        }


        public void EditSpec(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditSpecs)
            {
                var act = db.Specs.FirstOrDefault(x1 => x1.Id == i.Id);
                if (act != null)
                    act.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }
            if (db_ == null)
                db.Dispose();
        }


        public void AddVrem(string currentActionId,ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();

            int last = 0;
            var vrems = db.Vrems.Where(x1 => x1.Id.Contains(currentActionId + "_VREM")).ToList();
            if (vrems.Count > 0)
                last = vrems.Max(x1 => int.Parse(x1.Id.Split(new string[] { (currentActionId + "_VREM") }, StringSplitOptions.RemoveEmptyEntries)[0]));
            //List<JsonSaveDescription> done = new List<JsonSaveDescription>();

            int countLastIter = 0;

            List<SaveDescriptionEntry> massAddVremsList = MassAddVrems.ToList();
            while (massAddVremsList.Count > 0)
            {

                if (countLastIter == massAddVremsList.Count)
                    throw new Exception("непонятные Vrem");
                countLastIter = massAddVremsList.Count;
                for (int i = 0; i < massAddVremsList.Count; ++i)
                {
                    //проверить можно ли добавить, и добавить и вынести в массив
                    if (!massAddVremsList[i].ParentId.Contains("_NEW"))
                    {
                        var vrem = new Models.Domain.Vrem()
                        {
                            Name = massAddVremsList[i].Text,
                            Parent = (massAddVremsList[i].ParentId.Contains("_VREM") ? massAddVremsList[i].ParentId : massAddVremsList[i].ParentId + "_VREM"),

                            Id = (currentActionId + "_VREM" + ++last)
                        };

                        db.Vrems.Add(vrem);
                        db.SaveChanges();
                        for (int i2 = 0; i2 < massAddVremsList.Count; ++i2)
                        {
                            if (massAddVremsList[i2].ParentId == massAddVremsList[i].Id)
                            {
                                massAddVremsList[i2].ParentId = vrem.Id;
                            }
                        }
                        massAddVremsList.Remove(massAddVremsList[i--]);

                    }


                }
            }


            if (db_ == null)
                db.Dispose();
        }


        public void EditVrem(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditVrems)
            {
                var act = db.Vrems.FirstOrDefault(x1 => x1.Id == i.Id);
                if (act != null)
                    act.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }
            if (db_ == null)
                db.Dispose();
        }



        public void EditActionId( ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditActionId)
            {
                var act = db.AllActions.FirstOrDefault(x1 => x1.Id == i.Id);
                if (act != null)
                    act.Name = i.Text;
                else
                    i.Id = null;


                db.SaveChanges();
            }
            if (db_ == null)
                db.Dispose();
        }

        public bool? AddActionId(int lastAllActionId, ref string currentActionId,ApplicationDbContext db_=null)
        {
            bool? currentActionParametric  = null;
            var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassAddActionId)//возможно вытащить из цикла тк сейчас нельзя добавить больше 1
            {
                var allAction = new Models.Domain.AllAction() { Name = i.Text, Parent = "ALLACTIONS", Parametric = i.Parametric, Id = ("VOZ" + ++lastAllActionId) };
                db.AllActions.Add(allAction);
                db.SaveChanges();
                //i.NewId = allAction.Id;
                currentActionId = allAction.Id;
                currentActionParametric = i.Parametric;
                //break;//TODO


                //надо обновить в pros... parentid там где =="VOZ0"? 
                foreach (var i2 in MassAddPros)
                {
                    i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                    i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                }

                foreach (var i2 in MassAddVrems)
                {
                    i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                    i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                }

                foreach (var i2 in MassAddSpecs)
                {
                    i2.ParentId = i2.ParentId.Replace("VOZ0", currentActionId);
                    i2.Id = i2.Id.Replace("VOZ0", currentActionId);
                }


            }
            if (db_ == null)
                db.Dispose();
            return currentActionParametric;
        }


        public void DeletePros(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            List<Pro> forDeleted = new List<Pro>();

            int start = 0;
            foreach (var i in MassDeletedPros)
            {
                var pro = db.Pros.FirstOrDefault(x1 => x1.Id == i.Id);
                if (pro == null)
                    continue;
                forDeleted.Add(pro);
                //var childs=Pro.GetChild(pro.Id);
                //    forDeleted.AddRange(childs);
                for (; start < forDeleted.Count; ++start)
                {
                    forDeleted.AddRange(Pro.GetChild(forDeleted[start].Id));
                }
            }
            db.Pros.RemoveRange(forDeleted);
            db.SaveChanges();
            if (db_ == null)
                db.Dispose();
        }

        public void DeleteVrem(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            List<Vrem> forDeleted = new List<Vrem>();

            int start = 0;
            foreach (var i in MassDeletedVrems)
            {
                var vrem = db.Vrems.FirstOrDefault(x1 => x1.Id == i.Id);
                if (vrem == null)
                    continue;
                forDeleted.Add(vrem);
                //var childs=Pro.GetChild(pro.Id);
                //    forDeleted.AddRange(childs);
                for (; start < forDeleted.Count; ++start)
                {
                    forDeleted.AddRange(Vrem.GetChild(forDeleted[start].Id));
                }
            }
            db.Vrems.RemoveRange(forDeleted);
            db.SaveChanges();
            if (db_ == null)
                db.Dispose();
        }


        public void DeleteSpec(ApplicationDbContext db_ = null)
        {
            var db = db_ ?? new ApplicationDbContext();
            List<Spec> forDeleted = new List<Spec>();

            int start = 0;
            foreach (var i in MassDeletedSpecs)
            {
                var spec = db.Specs.FirstOrDefault(x1 => x1.Id == i.Id);
                if (spec == null)
                    continue;
                forDeleted.Add(spec);
                //var childs=Pro.GetChild(pro.Id);
                //    forDeleted.AddRange(childs);
                for (; start < forDeleted.Count; ++start)
                {
                    forDeleted.AddRange(Spec.GetChild(forDeleted[start].Id));
                }
            }
            db.Specs.RemoveRange(forDeleted);
            db.SaveChanges();
            if (db_ == null)
                db.Dispose();
        }



    }
    public class SaveDescriptionEntry
    {
        public string Id { get; set; }
        //public string NewId { get; set; }
        public string ParentId { get; set; }
        //public int Type { get; set; }
        public string Text { get; set; }
        public bool Parametric { get; set; }
        //public int TypeAction { get; set; }


        public SaveDescriptionEntry()
        {
            //NewId = null;
        }
    }



}