#define debug

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace dip.Models.DataBase
{
    //public class OldDbContext : DbContext
    //{
    //    public OldDbContext()
    //    : base("OldDb")
    //{



    //}
    //    public DbSet<Domain.ActionPro> ActionPros { get; set; }
    //    public DbSet<Domain.Action> Actions { get; set; }
    //    public DbSet<ActionSpec> ActionSpecs { get; set; }
    //    public DbSet<ActionType> ActionTypes { get; set; }
    //    public DbSet<ActionVrem> ActionVrems { get; set; }
    //    public DbSet<AllAction> AllActions { get; set; }
    //    public DbSet<Chain> Chains { get; set; }
    //    public DbSet<FEAction> FEActions { get; set; }
    //    public DbSet<FEIndex> FEIndexs { get; set; }
    //    public DbSet<FEObject> FEObjects { get; set; }
    //    public DbSet<FizVel> FizVels { get; set; }
    //    public DbSet<NewFEIndex> NewFEIndexs { get; set; }
    //    public DbSet<NeZakon> NeZakons { get; set; }
    //    public DbSet<Pro> Pros { get; set; }
    //    public DbSet<ReverseChain> ReverseChains { get; set; }
    //    public DbSet<Spec> Specs { get; set; }
    //    public DbSet<TasksToSynthesy> TasksToSynthesys { get; set; }
    //    public DbSet<The> Thes { get; set; }
    //    public DbSet<ThesChild> ThesChilds { get; set; }
    //    public DbSet<Vrem> Vrems { get; set; }


    //    // public DbSet<ActionPro> dat { get; set; }




    //}



    class OldData
    {
        static SqlConnection connection;
        static SqlCommand command;

        static OldData()
        {
            connection = new SqlConnection();
            connection.ConnectionString = Constants.sql_1;
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
        } // constructor

        public static bool ReloadDataBase()
        {

            bool returnvalue = false;


            //выгрузка еще из 1 бд
            {


               // return true;
                //MSSQLSERVER
                //var connection1 = new SqlConnection();
                ////(LocalDb)\MSSQLLocalDB    SQLEXPRESS01
                //// connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=|DataDirectory|\TechnicalFunctions.mdf;Integrated Security=True";
                ////connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=C:\rub\d_bd\TechnicalFunctions.mdf;Integrated Security=True";
                ////connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=|DataDirectory|\TechnicalFunctions.mdf;Integrated Security=True;User Instance=False";


                ////connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=C:\rub\d_bd\1\TechnicalFunctions.mdf;Integrated Security=True;User Instance=False";
                //connection1.ConnectionString = Constants.sql_2;

                //var command1 = new SqlCommand();
                //command1.Connection = connection1;
                //command1.CommandType = CommandType.Text;
               
                //connection1.Open();




                //Limit

                //try
                //{
                //    command1.CommandText = "select * from Limit";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command1, "Id", "Value", "Parent");
                //    foreach (var i in ldr)
                //    {
                //        var obj = new TechnicalFunctions.Limit();
                //        obj.Id = i["Id"].ToString().Trim();
                //        obj.Value = i["Value"].ToString().Trim();
                //        obj.Parent = i["Parent"].ToString().Trim();
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.Limits.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                       
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}




                //OperandGroup

                //try
                //{

                //    command1.CommandText = "select * from OperandGroup";
                    
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command1, "Id", "Value");
                //    foreach (var i in ldr)
                //    {
                //        var obj = new TechnicalFunctions.OperandGroup();
                //        obj.Id = i["Id"].ToString().Trim();
                //        obj.Value = i["Value"].ToString().Trim();
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.OperandGroups.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                    

                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}




                //Operation

                //try
                //{
                //    command1.CommandText = "select * from Operation";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command1, "Id", "Value", "Parent");
                //    foreach (var i in ldr)
                //    {
                //        var obj = new TechnicalFunctions.Operation();
                //        obj.Id = i["Id"].ToString().Trim();
                //        obj.Value = i["Value"].ToString().Trim();
                //        obj.Parent = i["Parent"].ToString().Trim();
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.Operations.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                    
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}





                //Operand

                //try
                //{
                //    command1.CommandText = "select * from Operand";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command1, "Id", "Value", "OperandGroupId");
                //    foreach (var i in ldr)
                //    {
                //        var obj = new TechnicalFunctions.Operand();
                //        obj.Id = i["Id"].ToString().Trim();
                //        obj.Value = i["Value"].ToString().Trim();
                //        obj.OperandGroupId = i["OperandGroupId"].ToString().Trim();
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.Operands.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                    
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}




                //Index
                //try
                //{
                //    command1.CommandText = "select * from [Index]";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command1, "Id", "OperationId", "OperandId", "LimitId", "EffectIds");
                //    foreach (var i in ldr)
                //    {
                //        var obj = new TechnicalFunctions.Index();
                //        obj.Id = i["Id"].ToString().Trim();
                //        obj.OperationId = i["OperationId"].ToString().Trim();
                //        obj.OperandId = i["OperandId"].ToString().Trim();
                //        obj.LimitId = i["LimitId"].ToString().Trim();
                //        obj.EffectIds = i["EffectIds"].ToString().Trim();
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.Indexs.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                   
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}


                //connection1.Close();
                //command1.Dispose();
                

            }


            


            //try
            {
                connection.Open();





                //Pros


                try
                {
                    command.CommandText = "select * from Pros";

                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.Pro();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        obj.Parent = i["parent"].ToString().Trim();
                        using (var db = new ApplicationDbContext())
                        {
                            db.Pros.Add(obj);
                            db.SaveChanges();
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }




                //ActionTypes



                try
                {
                    command.CommandText = "select * from ActionTypes";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.ActionType();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(i["parent"].ToString().Trim()))
                            obj.Parent = null;
                        else
                            obj.Parent = i["parent"].ToString().Trim();
                        using (var db = new ApplicationDbContext())
                        {
                            db.ActionTypes.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }




                //FizVels


                //try
                {
                    command.CommandText = "select * from FizVels";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.FizVel();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(i["parent"].ToString().Trim()))
                            obj.Parent = null;
                        else
                            obj.Parent = i["parent"].ToString().Trim();
                        if (obj.Parent?.Contains("_FIZVEL_R")==true)
                            obj.Parametric = true; 
                        using (var db = new ApplicationDbContext())
                        {
                            db.FizVels.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                //catch (Exception e)
                //{
                //    throw e;
                //}




                //AllActions

                try
                {
                    command.CommandText = "select * from AllActions";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.AllAction();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(i["parent"].ToString().Trim()))
                            obj.Parent = null;
                        else
                            obj.Parent = i["parent"].ToString().Trim();
                        if (obj.Id == "VOZ11")
                            obj.Parametric = true;
                        using (var db = new ApplicationDbContext())
                        {
                            db.AllActions.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }





                //Actions
               // List<dynamic> ActionsList = new List<dynamic>();
                //try
                //{


                //    //костль для восстановления id, для того что бы начиналось с 134, можно при создании таблицы указывать начальный id
                //    for (var i = 0; i < 133; ++i)
                //    {
                //        using (var db = new ApplicationDbContext())
                //        {
                //            var obj = new Domain.Action();
                //            db.Actions.Add(obj);
                //            db.SaveChanges();
                //            db.Actions.Remove(obj);
                //            db.SaveChanges();
                //        }
                //    }



                //    command.CommandText = "select * from Actions";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "actionId", "actionType", "fizVelId");
                //    foreach (var i in ldr)
                //    {
                //        //ActionsList.Add(new {
                //        //    Id = Convert.ToInt32(i["id"].ToString().Trim()),
                //        //    AllActionId = i["actionId"].ToString().Trim(),
                //        //    ActionType_Id = i["actionType"].ToString().Trim(),
                //        //    FizVelId = i["fizVelId"].ToString().Trim(),

                //        //});
                //        var obj = new Domain.Action();
                //        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                //        obj.AllActionId = i["actionId"].ToString().Trim();
                //        obj.ActionType_Id = i["actionType"].ToString().Trim();
                //        obj.FizVelId = i["fizVelId"].ToString().Trim();
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.Actions.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                    
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}




                //ActionPros

                //try
                //{
                //    //костль для восстановления id, для того что бы начиналось с 207, по идеи это не нужно, 
                //    //for (var i = 0; i < 207; ++i)
                //    //{
                //    //    using (var db = new ApplicationDbContext())
                //    //    {
                //    //        var obj = new Domain.ActionPro();
                //    //        db.ActionPros.Add(obj);
                //    //        db.SaveChanges();
                //    //        db.ActionPros.Remove(obj);
                //    //        db.SaveChanges();
                //    //    }
                //    //}



                //    //после action, pro
                //    command.CommandText = "select * from ActionPros";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "actionId", "prosId");
                //    foreach (var i in ldr)
                //    {
                //        var ActionId = Convert.ToInt32(i["actionId"].ToString().Trim());
                //        var ProId = i["prosId"].ToString().Trim();
                //        using (var db = new ApplicationDbContext())
                //        {
                //            var act = db.Actions.First(x1 => x1.Id == ActionId);
                //            act.Pros.Add(db.Pros.First(x1 => x1.Id == ProId));
                //            db.SaveChanges();
                //        }
                //    }
                    

                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}
              



                //Spec



                try
                {
                    command.CommandText = "select * from Spec";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.Spec();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        obj.Parent = i["parent"].ToString().Trim();

                        using (var db = new ApplicationDbContext())
                        {
                            db.Specs.Add(obj);
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }



                //ActionSpec


                //try
                //{
                //    //костль для восстановления id, для того что бы начиналось с 212
                //    //for (var i = 0; i < 211; ++i)
                //    //{
                //    //    using (var db = new ApplicationDbContext())
                //    //    {
                //    //        var obj = new Domain.ActionSpec();
                //    //        db.ActionSpecs.Add(obj);
                //    //        db.SaveChanges();
                //    //        db.ActionSpecs.Remove(obj);
                //    //        db.SaveChanges();
                //    //    }
                //    //}


                //    command.CommandText = "select * from ActionSpec";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "actionId", "specId");
                //    foreach (var i in ldr)
                //    {
                //        var ActionId = Convert.ToInt32(i["actionId"].ToString().Trim());
                //        var SpecId = i["specId"].ToString().Trim();

                //        using (var db = new ApplicationDbContext())
                //        {
                //            var act = db.Actions.First(x1 => x1.Id == ActionId);
                //            act.Specs.Add(db.Specs.First(x1 => x1.Id == SpecId));
                //            db.SaveChanges();
                //        }
                //    }

                    
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}





                //Vrem



                try
                {
                    command.CommandText = "select * from Vrem";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.Vrem();
                        
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        obj.Parent = i["parent"].ToString().Trim();
                        
                        using (var db = new ApplicationDbContext())
                        {
                            db.Vrems.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }







                //ActionVrem



                //try
                //{
                //    //костль для восстановления id, для того что бы начиналось с 212
                //    //for(var i = 0; i < 211; ++i)
                //    //{
                //    //    using (var db = new ApplicationDbContext())
                //    //    {
                //    //        var obj = new Domain.ActionVrem();
                //    //        db.ActionVrems.Add(obj);
                //    //        db.SaveChanges();
                //    //        db.ActionVrems.Remove(obj);
                //    //        db.SaveChanges();
                //    //    }
                //    //}
                //    command.CommandText = "select * from ActionVrem";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "actionId", "vremId");
                //    foreach (var i in ldr)
                //    {
                //        var ActionId = Convert.ToInt32(i["actionId"].ToString().Trim());
                //        var VremId = i["vremId"].ToString().Trim();
                //        using (var db = new ApplicationDbContext())
                //        {
                //            var act = db.Actions.First(x1 => x1.Id == ActionId);
                //            act.Vrems.Add(db.Vrems.First(x1 => x1.Id == VremId));

                //            db.SaveChanges();
                //        }
                //    }
                    
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}





                

                //---- Chains





                //FeAction

                //try
                {
                    command.CommandText = "select * from FeAction";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "idfe", "input", "type", "name", "fizVelId", 
                        "fizVelSection", "fizVelChange", "fizVelLeftBorder", "fizVelRightBorder", "pros", "spec", "vrem");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.FEAction();

                        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                        obj.Idfe = Convert.ToInt32(i["idfe"].ToString().Trim());
                        obj.Input = Convert.ToInt32(i["input"].ToString().Trim());
                        obj.Type = i["type"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();

                        //TODO возможно так правильно, надо потестить
                        obj.FizVelId = i["fizVelId"].ToString().Trim();
                        obj.FizVelSection = i["fizVelSection"].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(obj.FizVelSection))
                        {
                            string tmp = obj.FizVelId;
                            obj.FizVelId = obj.FizVelSection;
                            obj.FizVelSection = tmp;
                        }
                            //
                        //    obj.FizVelChange = i["fizVelChange"].ToString().Trim();
                        //obj.FizVelLeftBorder = Convert.ToDouble(i["fizVelLeftBorder"].ToString().Trim());
                        //obj.FizVelRightBorder = Convert.ToDouble(i["fizVelRightBorder"].ToString().Trim());

                        //
                        {
                            //TODO
                            obj.Pros = i["pros"].ToString().Trim();
                            obj.Pros = Pro.SortIds(obj.Pros);
                            //string prs = "";
                            //string prsRes = "";
                            //if (!string.IsNullOrWhiteSpace(obj.Pros))
                            //{
                            //    foreach (var pr in obj.Pros.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                            //    {
                            //        prs = pr + " " + Pro.GetParents(pr);
                            //        prsRes += prs + " ";
                            //    }

                            //}


                            //obj.Pros = Pro.SortIds(prsRes);
                        }
                        //
                        {
                            //TODO
                            obj.Spec = i["spec"].ToString().Trim();
                            obj.Spec = Spec.SortIds(obj.Spec);
                            //string spc = "";
                            //string spcRes = "";
                            //if (!string.IsNullOrWhiteSpace(obj.Spec))
                            //{
                            //    foreach (var sp in obj.Spec.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                            //    {
                            //        spc = sp + " " + Spec.GetParents(sp);
                            //        spcRes += spc + " ";
                            //    }

                            //}
                            //obj.Spec = Spec.SortIds(spcRes);
                        }
                        //
                       
                        {
                            obj.Vrem = i["vrem"].ToString().Trim();
                            obj.Vrem = Vrem.SortIds(obj.Vrem);
                            //string vrm = "";
                            //string vrmRes = "";
                            //if (!string.IsNullOrWhiteSpace(obj.Vrem))
                            //{
                            //    foreach (var vr in obj.Vrem.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                            //    {
                            //        vrm = vr + " " + Vrem.GetParents(vr);
                            //        vrmRes += vrm + " ";
                            //    }

                            //}
                            //obj.Vrem = Vrem.SortIds(vrmRes);
                        }

                        //
                        using (var db = new ApplicationDbContext())
                        {
                            db.FEActions.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                    
                }


                //catch (Exception e)
                //{
                //    throw e;
                //}


                //FeIndex


                List<dynamic> FeIndexList = new List<dynamic>();
                try
                {
                    command.CommandText = "select * from FeIndex";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "IDFE", "Index");
                    foreach (var i in ldr)
                    {
                        FeIndexList.Add(new {
                            IDFE= Convert.ToInt32(i["IDFE"].ToString().Trim()),
                            Index= i["Index"].ToString().Trim().Replace("\u0002\u0003\u0004", "\n")
                        });


                        //var obj = new Domain.FEIndex();

                        //obj.IDFE = Convert.ToInt32(i["IDFE"].ToString().Trim());
                        //obj.Index = i["Index"].ToString().Trim();

                        //using (var db = new ApplicationDbContext())
                        //{
                        //    db.FEIndexs.Add(obj);
                        //    db.SaveChanges();
                        //}
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }






                using (var db = new ApplicationDbContext())
                {
                    db.StateObjects.Add(new StateObject()
                    {
                        Id = "MONOFAZ",
                        Name = "Однофазное",
                        Parent = "STRUCTOBJECT",//ALLSTATE
                        CountPhase = 1
                    });
                    db.StateObjects.Add(new StateObject()
                    {
                        Id = "POLYFAZ",
                        Name = "Многофазное",
                        Parent = "STRUCTOBJECT"

                    });
                    db.SaveChanges();
                }
                //state phase
                LoadStateObject("MONOFAZ");
                LoadStateObject("POLYFAZ");

                LoadCharacteristicObject(Constants.FeObjectBaseCharacteristic);










                //FeObject



                //выгрузка из существующей бд не все записи заносятся

                //try
                //{
                //    command.CommandText = "select * from FeObject";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "idfe", "begin", "phaseState", "composition", "magneticStructure",
                //        "conductivity", "mechanicalState", "opticalState", "special");

                //    int lastIdfe = -1;
                //    int lastBegin = -1;
                //    int NumPhase = 1;
                //    foreach (var i in ldr)
                //    {
                //        var obj = new Domain.FEObject();

                //        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());

                //        obj.Idfe = Convert.ToInt32(i["idfe"].ToString().Trim());
                //        obj.Begin = Convert.ToInt32(i["begin"].ToString().Trim());

                //        //if (lastIdfe==-1)
                //        //{
                //        //    lastIdfe = obj.Idfe;
                //        //    lastBegin = obj.Begin;
                //        //}
                //        //else
                //        {
                //            if (lastIdfe == obj.Idfe && lastBegin == obj.Begin)
                //            {
                //                NumPhase++;
                //            }
                //            else
                //            {
                //                NumPhase = 1;
                //            }
                //            lastIdfe = obj.Idfe;
                //            lastBegin = obj.Begin;
                //        }
                //        obj.NumPhase = NumPhase;

                //        obj.PhaseState = i["phaseState"].ToString().Trim();
                //        obj.Composition = i["composition"].ToString().Trim();
                //        obj.MagneticStructure = i["magneticStructure"].ToString().Trim();
                //        obj.Conductivity = i["conductivity"].ToString().Trim();
                //        obj.MechanicalState = i["mechanicalState"].ToString().Trim();
                //        obj.OpticalState = i["opticalState"].ToString().Trim();
                //        {
                //            string[] tmpSpecial = i["special"].ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                //            if (tmpSpecial != null)
                //                foreach (var i2 in tmpSpecial)
                //                {
                //                    if (i2[0] == 'C')
                //                        obj.Special += " ";
                //                    obj.Special += i2;
                //                }
                //        }

                //        //obj.Special = i["special"].ToString().Trim();

                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.FEObjects.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }

                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}




                //new

                using (var db = new ApplicationDbContext())
                {
                    
                    //var test = db.FEIndexs.ToList();
                    foreach (var i in FeIndexList)
                    {
                       
                        //i.Index = i.Index.Replace("\u0002\u0003\u0004", "\n");
                        var indexMass = ((string[])i.Index.Split(new string[] { "\u0000", "\u0001", "\u0002", "\u0003", "\u0004" }, StringSplitOptions.RemoveEmptyEntries)).ToList();
                        //var indexMass = indexMass1.ToList();
                        for (int i2 = 0; i2 < indexMass.Count; ++i2)
                        {
                            if (indexMass[i2] == "2" || indexMass[i2] == "3" || indexMass[i2].IndexOf("2\n") == 0 || indexMass[i2].IndexOf("3\n") == 0)//g[i2][0] == '2'|| g[i2][0] == '3'
                            {
                                //характеристики начального состояния объекта

                                if (indexMass[i2].Contains('\n'))
                                {
                                    var th = indexMass[i2].Split('\n');
                                    indexMass[i2] = th[0];
                                    indexMass.Insert(i2 + 1, th[1]);

                                }
                                FEObject obj = new FEObject()
                                {
                                    NumPhase = 1,
                                    Begin = indexMass[i2][0] == '2' ? 1 : 0,
                                    Idfe = i.IDFE

                                };
                           
                                i2++;

                                FeObjectParseStep(ref i2, indexMass, obj, 1, i,db);
                            }
                           
                        }
                       
                    }
                    
                }


                //using (var db = new ApplicationDbContext())
                //{
                //    db.StateObjects.Add(new StateObject()
                //    {
                //        Id = "MONOFAZ",
                //        Name = "Однофазное",
                //        Parent = "STRUCTOBJECT",//ALLSTATE
                //        CountPhase = 1
                //    });
                //    db.StateObjects.Add(new StateObject()
                //    {
                //        Id = "POLYFAZ",
                //        Name = "Многофазное",
                //        Parent = "STRUCTOBJECT"

                //    });
                //    db.SaveChanges();
                //}
                ////state phase
                //LoadStateObject("MONOFAZ");
                //LoadStateObject("POLYFAZ");

                //LoadCharacteristicObject(Constants.FeObjectBaseCharacteristic);



                //NewFeIndex


                List<dynamic> NewFeIndexList = new List<dynamic>();
                try
                {
                    command.CommandText = "select * from NewFeIndex";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "idfe", "input", "output", "beginObjectState", "endObjectState",
                       "beginPhase", "endPhase");
                    foreach (var i in ldr)
                    {
                        NewFeIndexList.Add(new {
                            BeginPhase= i["beginPhase"].ToString().Trim(),
                            EndPhase= i["endPhase"].ToString().Trim(),
                            Idfe= Convert.ToInt32(i["idfe"].ToString().Trim())
                        });
                        



                        //var obj = new Domain.NewFEIndex();

                        //obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                        //obj.Idfe = Convert.ToInt32(i["idfe"].ToString().Trim());
                        //obj.Input = i["input"].ToString().Trim();
                        //obj.Output = i["output"].ToString().Trim();
                        //obj.BeginObjectState = i["beginObjectState"].ToString().Trim();
                        //obj.EndObjectState = i["endObjectState"].ToString().Trim();
                        //obj.BeginPhase = i["beginPhase"].ToString().Trim();
                        //obj.EndPhase = i["endPhase"].ToString().Trim();

                        //using (var db = new ApplicationDbContext())
                        //{
                        //    db.NewFEIndexs.Add(obj);
                        //    db.SaveChanges();
                        //}
                    }


                }
                catch (Exception e)
                {
                    throw e;
                }






                //FeText



               // try
                {
                    command.CommandText = "select * from FeText";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "IDFE", "name", "text", "textInp", "textOut", "textObj",
                        "textApp", "textLit");
                    var listFetext=new List<FEText>();
                    using (var db = new ApplicationDbContext())
                    {
                        foreach (var i in ldr)
                    {
                        var obj = new Domain.FEText();

                        obj.IDFE = Convert.ToInt32(i["IDFE"].ToString().Trim());
                       
                        obj.Name = i["name"].ToString().Trim();
                        obj.Text = i["text"].ToString().Trim();
                        obj.TextInp = i["textInp"].ToString().Trim();
                        obj.TextOut = i["textOut"].ToString().Trim();
                        obj.TextObj = i["textObj"].ToString().Trim();
                        obj.TextApp = i["textApp"].ToString().Trim();
                        obj.TextLit = i["textLit"].ToString().Trim();
                        obj.NotApprove = false;
                        
                        

                            //db.FEText.Add(obj);
                            //db.SaveChanges();
                            listFetext.Add(obj);
                            var tmpinp=db.FEActions.Where(x1 => x1.Idfe == obj.IDFE).ToList();
                            switch (tmpinp.Count)
                            {
                                case 0://вообще нет добавить 1 вход 1 выход
                                    { 
                                    obj.CountInput = 1;
                                    FEAction feactinp = new FEAction() { Idfe = obj.IDFE, Input = 1, Name = "VOZ1", Type = "NO_ACTIONS", FizVelId = "NO_FIZVEL" };
                                    db.FEActions.Add(feactinp);
                                    FEAction feactoutp = new FEAction() { Idfe = obj.IDFE, Input = 0, Name = "VOZ1", Type = "NO_ACTIONS", FizVelId = "NO_FIZVEL" };
                                    db.FEActions.Add(feactoutp);
                                    db.SaveChanges();
                                    break;
                                    }
                                case 1://что то есть добавить то чего нет
                                    obj.CountInput = 1;
                                    if (tmpinp.FirstOrDefault(x1 => x1.Input == 1) != null)
                                    {
                                        FEAction feactoutp = new FEAction() { Idfe = obj.IDFE, Input = 0, Name = "VOZ1", Type = "NO_ACTIONS", FizVelId = "NO_FIZVEL" };
                                        db.FEActions.Add(feactoutp);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        FEAction feactoutp = new FEAction() { Idfe = obj.IDFE, Input = 1, Name = "VOZ1", Type = "NO_ACTIONS", FizVelId = "NO_FIZVEL" };
                                        db.FEActions.Add(feactoutp);
                                        db.SaveChanges();
                                    }
                                    break;
                                case 2://1 вход и 1 выход
                                    obj.CountInput = 1;
                                    break;
                                case 3://2 вход и 1 выход
                                    obj.CountInput = 2;
                                    break;
                            }
                           


                            //if (obj.StateBeginId== "MONOFAZ")
                            //{
                            //    if (obj.StateEndId == "MONOFAZ")
                            //    {
                            //        if (tmpobj.Count != 2)
                            //            throw;
                            //    }
                            //    else
                            //    {
                            //        if (tmpobj.Count != 1)
                            //            throw;
                            //    }
                                
                            //}
                            //switch (tmpobj.Count)
                            //{
                            //    case 0:
                            //        break;


                            //}

                            }
                        
                    //}
                    //восстанавливаем бывшие id
                    listFetext= listFetext.OrderBy(x1 => x1.IDFE).ToList();
                    //using (var db = new ApplicationDbContext())
                    //{
                        foreach (var i in listFetext)
                        {
                            int tmpId = i.IDFE;
                            //try
                            //{
                            db.FEText.Add(i);
                            db.SaveChanges();
                            //}
                            //catch
                            //{
                            //    var asd = 10;
                            //}


                            while (tmpId != i.IDFE)
                            {
                                db.FEText.Remove(i);
                                db.SaveChanges();
                                db.FEText.Add(i);
                                db.SaveChanges();
                            }
                            //if (i.IDFE == 1)
                            //{
                            //    i.StateBeginId = "MONOFAZ";
                            //}
                            //else if(i.IDFE==1282){
                            //    i.StateBeginId = "MONOFAZ";
                            //    i.StateEndId = "MONOFAZ";
                            //}
                            if (string.IsNullOrWhiteSpace(i.StateBeginId))
                            {
                                i.StateBeginId = "MONOFAZ";
                            }
                            if (i.IDFE == 1282)
                            {
                                    i.StateEndId = "MONOFAZ";
                                }
                                if (i.IDFE == Constants.FEIDFORSEMANTICSEARCH)
                            {
                                db.SaveChanges();
                                continue;
                            }

                            var tmpphase = db.FEObjects.Where(x1 => x1.Idfe == i.IDFE && x1.Begin == 0).Count();
                            if (tmpphase != 0)
                                i.ChangedObject = true;

                            var tmpNewIndex = NewFeIndexList.FirstOrDefault(x1 => x1.Idfe == i.IDFE);
                            if (tmpNewIndex != null)
                            {
                                i.StateBeginId = tmpNewIndex.BeginPhase;// db.StateObjects.First(x1=>x1.Id== tmpNewIndex.BeginPhase) ;
                                if (!string.IsNullOrEmpty(tmpNewIndex.EndPhase))
                                    i.StateEndId = tmpNewIndex.EndPhase;// db.StateObjects.First(x1 => x1.Id == tmpNewIndex.EndPhase);
                            }



                            var tmpobj = db.FEObjects.Where(x1 => x1.Idfe == i.IDFE).ToList();
                            int allcount = 0;
                            {
#if debug
                                if (i.IDFE == 801|| i.IDFE == 809)
                                {
                                    var g = 10;
                                }
#endif                   
                                switch (i.StateBeginId)
                            {
                                case "MONOFAZ":
                                    allcount++;
                                    break;
                                case "2CONFAZ1":
                                    allcount += 2;
                                    break;
                                case "2MIXFAZ":
                                    allcount += 2;
                                    break;
                                case "2CONFAZ2":
                                    allcount += 2;
                                    break;
                                case "3CONFAZ1":
                                    allcount += 3;
                                    break;
                                case "3MIXFAZ":
                                    allcount += 3;
                                    break;
                            }
                            var beginobj = tmpobj.Where(x1 => x1.Begin == 1).ToList();
                            int difbeginobj = allcount - beginobj.Count;
                            if (difbeginobj > 0)
                            {
                                List<int> free = new List<int>();
                                if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 1) == null)
                                    free.Add(1);
                                if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 2) == null)
                                    free.Add(2);
                                if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 3) == null)
                                    free.Add(3);

                                for (var it = 0; it < difbeginobj; ++it)
                                {
                                    FEObject obj = new FEObject() { Idfe = i.IDFE, Begin = 1, NumPhase = free[it] };
                                    db.FEObjects.Add(obj);
                                    db.SaveChanges();
                                }

                            }
#if debug
                                if (difbeginobj < 0)
                            {
                                var error = 10;
                            }
#endif
                            }
                            allcount = 0;
                            {


                                switch (i.StateEndId)
                                {
                                    case "MONOFAZ":
                                        allcount++;
                                        break;
                                    case "2CONFAZ1":
                                        allcount += 2;
                                        break;
                                    case "2MIXFAZ":
                                        allcount += 2;
                                        break;
                                    case "2CONFAZ2":
                                        allcount += 2;
                                        break;
                                    case "3CONFAZ1":
                                        allcount += 3;
                                        break;
                                    case "3MIXFAZ":
                                        allcount += 3;
                                        break;
                                }
                                var beginobj = tmpobj.Where(x1 => x1.Begin == 0).ToList();
                                int difbeginobj = allcount - beginobj.Count;
                                if (difbeginobj > 0)
                                {
                                    List<int> free = new List<int>();
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 1) == null)
                                        free.Add(1);
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 2) == null)
                                        free.Add(2);
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 3) == null)
                                        free.Add(3);

                                    for (var it = 0; it < difbeginobj; ++it)
                                    {
                                        FEObject obj = new FEObject() { Idfe = i.IDFE, Begin = 0, NumPhase = free[it] };
                                        db.FEObjects.Add(obj);
                                        db.SaveChanges();
                                    }

                                }
#if debug
                                if (difbeginobj < 0)
                                {
                                    var error = 10;
                                }
#endif
                            }


                            //if (allcount != tmpobj.Count)
                            //{
                            //    var error = 10;
                            //}



                            db.SaveChanges();
                        }

                        //id этой записи должно быть == Models.Constants.FEIDFORSEMANTICSEARCH
                        var objsemantic = new Domain.FEText()
                        {
                            Deleted = true,
                            Name = "Временная запись для семантического поиска",
                            Text = Models.Constants.FeSemanticNullText,
                            TextInp = "",
                            TextOut = "",
                            TextObj = "",
                            TextApp = "",
                            TextLit = "",
                            CountInput = 0
                        };
                    db.FEText.Add(objsemantic);
                    db.SaveChanges();
                    }
                }
                //catch (Exception e)
                //{
                //    throw e;
                //}




                



               



                //NeZakon



                //try
                //{
                //    command.CommandText = "select * from NeZakon";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "FizVel1", "FizVel2");
                //    foreach (var i in ldr)
                //    {
                //        var obj = new Domain.NeZakon();
                        
                //        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                //        obj.FizVel1 = i["FizVel1"].ToString().Trim();
                //        obj.FizVel2 = i["FizVel2"].ToString().Trim();
                        
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.NeZakons.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                    
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}



                



                //------ReverseChains




                




                //-----TasksToSynthesys





                //Thes


                //try
                //{
                //    command.CommandText = "select * from Thes";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent", "compatible", "path");
                //    foreach (var i in ldr)
                //    {
                //        var obj = new Domain.The();
                        
                //        obj.Id = i["id"].ToString().Trim();
                //        obj.Name = i["name"].ToString().Trim();
                //        obj.Parent = i["parent"].ToString().Trim();
                        
                //        if (string.IsNullOrWhiteSpace(i["compatible"].ToString().Trim()))
                //            obj.Compatible = null;
                //        else
                //            obj.Compatible = i["compatible"].ToString().Trim();
                        
                //        if (string.IsNullOrWhiteSpace(i["path"].ToString().Trim()))
                //            obj.Path = null;
                //        else
                //            obj.Path = i["path"].ToString().Trim();
                        
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.Thes.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                    
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}





                //ThesChild



                //try
                //{
                //    command.CommandText = "select * from ThesChild";
                //    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "nodeID", "childID", "order");
                //    foreach (var i in ldr)
                //    {
                //        var obj = new Domain.ThesChild();
                        
                //        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                //        obj.NodeID = i["nodeID"].ToString().Trim();
                //        obj.ChildID = i["childID"].ToString().Trim();
                //        if (i["order"].ToString().Trim() == null || i["order"].ToString().Trim() == "")
                //            obj.Order = null;
                //        else
                //            obj.Order = Convert.ToInt32(i["order"].ToString().Trim());
                        
                //        using (var db = new ApplicationDbContext())
                //        {
                //            db.ThesChilds.Add(obj);
                //            db.SaveChanges();
                //        }
                //    }
                    
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}

                //using (var db = new ApplicationDbContext())
                //{
                //    FixOldFeRecord(db);
                //}




                //thes theschild 2

                //using (var db=new ApplicationDbContext())
                //{
                //    db.StateObjects.Add(new StateObject()
                //    {
                //        Id="MONOFAZ",
                //        Name= "Однофазное",
                //        Parent= "STRUCTOBJECT",//ALLSTATE
                //        CountPhase=1
                //    });
                //    db.StateObjects.Add(new StateObject()
                //    {
                //        Id = "POLYFAZ",
                //        Name = "Многофазное",
                //        Parent = "STRUCTOBJECT"

                //    });
                //    db.SaveChanges();
                //}
                //LoadStateObject("MONOFAZ");
                //LoadStateObject("POLYFAZ");

                //LoadCharacteristicObject(Constants.FeObjectBaseCharacteristic);








                returnvalue = true;




                















                connection.Close();
                command.Dispose();



            }
 
            return returnvalue;

        }
    
        
        
        
        
        public static void LoadStateObject(string id)
        {
            try
            {
                command.CommandText = $"select * from Thes where parent ='{id}'";
                var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                foreach (var i in ldr)
                {
                    var obj = new Domain.StateObject();

                    obj.Id = i["id"].ToString().Trim();
                    obj.Name = i["name"].ToString().Trim();
                    obj.Parent = i["parent"].ToString().Trim();
                    if (obj.Id == "3CONFAZ1"|| obj.Id == "3MIXFAZ")
                        obj.CountPhase = 3;
                    if(obj.Id == "2CONFAZ1" || obj.Id == "2CONFAZ2"|| obj.Id == "2MIXFAZ")
                        obj.CountPhase = 2;

                    using (var db = new ApplicationDbContext())
                    {
                        db.StateObjects.Add(obj);
                        db.SaveChanges();
                    }
                    //if (!string.IsNullOrWhiteSpace(obj.Parent))
                        LoadStateObject(obj.Id);
                }

            }
            catch (Exception e)
            {
                //connection.Open();
                //LoadState(id);
                throw e;
            }
        }

        public static void LoadCharacteristicObject(string id)
        {
            try
            {
                command.CommandText = $"select * from Thes where parent = '{id}'";
                var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                foreach (var i in ldr)
                {
                    var obj = new Domain.PhaseCharacteristicObject();

                    obj.Id = i["id"].ToString().Trim();
                    obj.Name = i["name"].ToString().Trim();
                    obj.Parent = i["parent"].ToString().Trim();


                    using (var db = new ApplicationDbContext())
                    {
                        db.PhaseCharacteristicObjects.Add(obj);
                        db.SaveChanges();
                    }
                    //if (!string.IsNullOrWhiteSpace(obj.Parent))
                        LoadCharacteristicObject(obj.Id);
                }

            }
            catch (Exception e)
            {
                //connection.Open();
                //LoadState(id);
                throw e;
            }
        }

        static void FeObjectParseStep(ref int i2, List<string> indexMass, FEObject obj, int numPhase, dynamic index, ApplicationDbContext db)
        {
            
            for (; i2 < indexMass.Count && (indexMass[i2].Length == 0 || (indexMass[i2][0] != '4' && indexMass[i2][0] != '5')); ++i2)//g[i2][0] != '3' && 
            {
                
                bool slN = false;
                if (indexMass[i2].Contains("\n"))
                {
                    //переход на след фазу или переход на выход

                    var th = indexMass[i2].Split('\n');
                    indexMass[i2] = th[0];
                    if ((i2 + 1) >= indexMass.Count)
                        indexMass.Add(th[1]);
                    else
                        indexMass.Insert(i2 + 1, th[1]);
                    slN = true;

                }
                if (indexMass[i2].Length != 0)
                    if (indexMass[i2] == "3" || indexMass[i2].IndexOf("3\n") == 0) //if (g[i2] == "2" || g[i2] == "3" || g[i2].IndexOf("2\n") == 0 || g[i2].IndexOf("3\n") == 0)//g[i2][0] == '2'|| g[i2][0] == '3'
                    {
                        //переход на выходные характеристики
                        FEObject objNext = new FEObject() { NumPhase = 1, Idfe = index.IDFE, Begin = 0 };
                        i2++;
                        FeObjectParseStep(ref i2, indexMass, objNext, objNext.NumPhase, index, db);
                    }
                    else if (slN)//if (g[i2].Contains("\n"))
                    {
                        //переход на след фазу

                       
                        if (i2 < indexMass.Count)
                            FeObjectParseAddValueInObj(indexMass[i2], obj);
                        
                        FEObject objNext = new FEObject() { NumPhase = ++numPhase, Idfe = index.IDFE, Begin = obj.Begin };
                        i2++;
                        FeObjectParseStep(ref i2, indexMass, objNext, numPhase, index, db);
                    }
               
                if (i2 < indexMass.Count)

                    FeObjectParseAddValueInObj(indexMass[i2], obj);
               
            }
            
            obj.Composition = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.DeleteNotChildCheckbox(obj.Composition.Trim()));
            obj.Conductivity = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.DeleteNotChildCheckbox(obj.Conductivity.Trim()));
            obj.MagneticStructure = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.DeleteNotChildCheckbox(obj.MagneticStructure.Trim())); 
            obj.MechanicalState = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.DeleteNotChildCheckbox(obj.MechanicalState.Trim())); 
            obj.OpticalState = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.DeleteNotChildCheckbox(obj.OpticalState.Trim())); 
            obj.PhaseState = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.DeleteNotChildCheckbox(obj.PhaseState.Trim())); 
            obj.Special = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.DeleteNotChildCheckbox(obj.Special.Trim()));

            

            db.FEObjects.Add(obj);
            db.SaveChanges();
           
        }

        static void FeObjectParseAddValueInObj(string g, FEObject obj)
        {
           
            if (g.Length > 0)
                switch (g[0])
                {
                    case 'F':
                        obj.PhaseState += g + " ";
                        break;
                    case 'X':
                        obj.Composition += g + " ";
                        break;
                    case 'M':
                        obj.MagneticStructure += g + " ";
                        break;
                    case 'E':
                        obj.Conductivity += g + " ";
                        break;
                    case 'D':
                        obj.MechanicalState += g + " ";
                        break;
                    case 'O':
                        obj.OpticalState += g + " ";
                        break;
                    case 'C':
                        obj.Special += g + " ";
                        break;


                }
        }


        public static void Fix2OldFeRecord(ApplicationDbContext db)
        {
            // проверить состояние и количество фаз
            //проверить дескрипторы
            //проверить объект

            command.CommandText = @"";
            //#TODO



        }





        public static void FixOldFeRecord(ApplicationDbContext db)
        {

            //            command.CommandText = @"select FEObjects_.IDFE as idfe1,FEActions_.IDFE as idfe2 from (select FETexts.IDFE from dbo.FETexts
            //left join dbo.FEObjects
            //on FETexts.IDFE = FEObjects.Idfe
            //where FEObjects.Id is null) as FEObjects_
            //full join (select FETexts.IDFE from dbo.FETexts where FETexts.IDFE not in (select Idfe  from dbo.FEActions)) as FEActions_
            //on FEObjects_.IDFE = FEActions_.IDFE";
            command.CommandText = @"select FEObject_.IDFE as idfe1,FEAction_.IDFE as idfe2 from (select FEText.IDFE from dbo.FEText
                left join dbo.FEObject
                on FEText.IDFE = FEObject.Idfe
                where FEObject.Id is null) as FEObject_
                full join (select FEText.IDFE from dbo.FEText where FEText.IDFE not in (select Idfe  from dbo.FEAction)) as FEAction_
                on FEObject_.IDFE = FEAction_.IDFE";

            var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "idfe1", "idfe2");
            foreach (var i in ldr)
            {
                string idfe1 = i["idfe1"].ToString().Trim();
                string idfe2 = i["idfe2"].ToString().Trim();
                //TODO поменять на базовые значние, 1282 не должен заходить в 1 условие
#if debug
                if (idfe1=="1282"|| idfe2 == "1282")
                {
                    var g = 10;
                }
#endif
                if (!string.IsNullOrWhiteSpace(idfe1))
                {
                    int intid = int.Parse(idfe1);
                    if (intid == Constants.FEIDFORSEMANTICSEARCH)
                        continue;
                    //установить начальное состояние, добавить дескрипторы и объект
                    FEText fe = db.FEText.First(x1=>x1.IDFE== intid);
                    fe.StateBeginId = "MONOFAZ";
                    db.SaveChanges();
                    FEAction feactinp = new FEAction() { Idfe= fe.IDFE, Input=1 };
                    db.FEActions.Add(feactinp);
                    FEAction feactoutp = new FEAction() { Idfe = fe.IDFE, Input = 0 };
                    db.FEActions.Add(feactoutp);
                    db.SaveChanges();
                    FEObject obj = new FEObject() { Idfe = fe.IDFE, Begin=1, NumPhase=1 };
                    db.FEObjects.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    int intid = int.Parse(idfe1);
                    FEText fe = db.FEText.First(x1 => x1.IDFE == intid);
                    fe.StateBeginId = "2CONFAZ1";
                    db.SaveChanges();
                    FEAction feactinp = new FEAction() { Idfe = fe.IDFE, Input = 1 };
                    db.FEActions.Add(feactinp);
                    FEAction feactoutp = new FEAction() { Idfe = fe.IDFE, Input = 0 };
                    db.FEActions.Add(feactoutp);
                    db.SaveChanges();
                }
            }


            }


    }
    
    
}