﻿using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
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
        //public SaveDescriptionEntry[] MassDeletedParamFizVels { get; set; }
        public SaveDescriptionEntry[] MassDeletedPros { get; set; }
        public SaveDescriptionEntry[] MassDeletedVrems { get; set; }
        public SaveDescriptionEntry[] MassDeletedSpecs { get; set; }


        public SaveDescription()
        {

        }
        public void SetNotNullArray()
        {
            MassAddActionId = MassAddActionId ?? new SaveDescriptionEntry[0];
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
            // MassDeletedParamFizVels = MassDeletedParamFizVels ?? new SaveDescriptionEntry[0];
            MassDeletedPros = MassDeletedPros ?? new SaveDescriptionEntry[0];
            MassDeletedVrems = MassDeletedVrems ?? new SaveDescriptionEntry[0];
            MassDeletedSpecs = MassDeletedSpecs ?? new SaveDescriptionEntry[0];
        }


        public void AddFizVels(string currentActionId, bool? currentActionParametric, ApplicationDbContext db)
        {
            //var db = db_ ?? new ApplicationDbContext();
            List<FizVel> fizvels = db.FizVels.Where(x1 => x1.Id.Contains(currentActionId + "_FIZVEL")).ToList();

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
            //if (db_ == null)
            //db.Dispose();
        }

        public void EditFizVels(ApplicationDbContext db)
        {
            //var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditFizVels)
            {
                var fizVel = db.FizVels.FirstOrDefault(x1 => x1.Id == i.Id);
                if (fizVel != null)
                    fizVel.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }
            //if (db_ == null)
            //db.Dispose();
        }

        public void AddParamFizVels(ApplicationDbContext db)
        {
            //var db = db_ ?? new ApplicationDbContext();

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

        }

        public void EditParamFizVels(ApplicationDbContext db)
        {
            //var db = db_ ?? new ApplicationDbContext();

            foreach (var i in MassEditParamFizVels)
            {
                var fizVel = db.FizVels.FirstOrDefault(x1 => x1.Id == i.Id);
                if (fizVel != null)
                    fizVel.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }

            //if (db_ == null)
            //db.Dispose();
        }

        public void AddPro(string currentActionId, ApplicationDbContext db)
        {
            //var db = db_ ?? new ApplicationDbContext();

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

        }

        public void EditPro(ApplicationDbContext db)
        {
            //var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditPros)
            {
                var act = db.Pros.FirstOrDefault(x1 => x1.Id == i.Id);
                if (act != null)
                    act.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }
            //if (db_ == null)
            //db.Dispose();
        }

        public void AddSpec(string currentActionId, ApplicationDbContext db)
        {
            // var db = db_ ?? new ApplicationDbContext();

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

        }


        public void EditSpec(ApplicationDbContext db)//db_ = null
        {
            //var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditSpecs)
            {
                var act = db.Specs.FirstOrDefault(x1 => x1.Id == i.Id);
                if (act != null)
                    act.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }
            //if (db_ == null)
            //db.Dispose();
        }


        public void AddVrem(string currentActionId, ApplicationDbContext db)//db_ = null
        {
            //var db = db_ ?? new ApplicationDbContext();

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


            //if (db_ == null)
            // db.Dispose();
        }


        public void EditVrem(ApplicationDbContext db)//db_ = null
        {
            //var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditVrems)
            {
                var act = db.Vrems.FirstOrDefault(x1 => x1.Id == i.Id);
                if (act != null)
                    act.Name = i.Text;
                else
                    i.Id = null;

                db.SaveChanges();
            }
            //if (db_ == null)
            //db.Dispose();
        }



        public void EditActionId(ApplicationDbContext db)//db_ = null
        {
            //var db = db_ ?? new ApplicationDbContext();
            foreach (var i in MassEditActionId)
            {
                var act = db.AllActions.FirstOrDefault(x1 => x1.Id == i.Id);
                if (act != null)
                    act.Name = i.Text;
                else
                    i.Id = null;


                db.SaveChanges();
            }
            //if (db_ == null)
            //db.Dispose();
        }

        public bool? AddActionId(int lastAllActionId, ref string currentActionId, ApplicationDbContext db)//db_ = null
        {
            bool? currentActionParametric = null;
            //var db = db_ ?? new ApplicationDbContext();
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
            //if (db_ == null)
            // db.Dispose();
            return currentActionParametric;
        }


        //проверяет есть ли фэ которые используют что то из списка(не грузит детей и тд) и если хотя бы 1 итем блокируется не удаляет ничего
        public List<int> DeleteFizVels(ApplicationDbContext db)//db_ = null
        {
            if (this.MassDeletedFizVels.Length == 0)
                return new List<int>();
            List<string> fizstr = this.MassDeletedFizVels.Select(x1 => x1.Id).ToList();
            var list = db.FizVels.Where(x1 => fizstr.FirstOrDefault(x2 => x2 == x1.Id || x2 == x1.Parent) != null).ToList();
            return FizVel.TryDelete(db, list);


        }
        public List<int> DeleteActionId(ApplicationDbContext db)//db_ = null
        {
            //TODO проверям можно ли удалить проверять actionid и все что удалится связанное
            List<int> blockFe = new List<int>();
            List<string> actionstr = this.MassDeletedActionId.Select(x1 => x1.Id).ToList();
            var actionIdList = db.AllActions.Where(x1 => actionstr.Contains(x1.Id)).ToList();
            return AllAction.TryDeleteWithChilds(db, actionIdList);

        }





        public List<int> DeleteVrem(ApplicationDbContext db)//db_ = null
        {
            //TODO проверям можно ли удалить проверять actionid и все что удалится связанное
            //List<int> blockFe = new List<int>();
            List<string> MassDeletedVremsS = MassDeletedVrems.Select(x1 => x1.Id).ToList();
            var childcol = db.Vrems.Where(x1 => MassDeletedVremsS.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList();


            return Vrem.TryDeleteWithChilds(db, childcol);
        }






        public List<int> DeleteSpec(ApplicationDbContext db)//db_ = null
        {
            //TODO проверям можно ли удалить проверять actionid и все что удалится связанное
            //List<int> blockFe = new List<int>();
            List<string> MassDeletedSpecsS = MassDeletedSpecs.Select(x1 => x1.Id).ToList();
            var childcol = db.Specs.Where(x1 => MassDeletedSpecsS.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList();
            return Spec.TryDeleteWithChilds(db, childcol);

        }




        public List<int> DeletePros(ApplicationDbContext db)//db_ = null
        {
            //TODO проверям можно ли удалить проверять actionid и все что удалится связанное
            //List<int> blockFe = new List<int>();
            List<string> MassDeletedProsS = MassDeletedPros.Select(x1 => x1.Id).ToList();
            var childcol = db.Pros.Where(x1 => MassDeletedProsS.FirstOrDefault(x2 => x2 == x1.Id) != null).ToList();
            return Pro.TryDeleteWithChilds(db, childcol);

        }

    }
}