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

                try
                {

                    //костль для восстановления id, для того что бы начиналось с 134, можно при создании таблицы указывать начальный id
                    for (var i = 0; i < 133; ++i)
                    {
                        using (var db = new ApplicationDbContext())
                        {
                            var obj = new Domain.Action();
                            db.Actions.Add(obj);
                            db.SaveChanges();
                            db.Actions.Remove(obj);
                            db.SaveChanges();
                        }
                    }



                    command.CommandText = "select * from Actions";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "actionId", "actionType", "fizVelId");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.Action();
                        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                        obj.AllActionId = i["actionId"].ToString().Trim();
                        obj.ActionType_Id = i["actionType"].ToString().Trim();
                        obj.FizVelId = i["fizVelId"].ToString().Trim();
                        using (var db = new ApplicationDbContext())
                        {
                            db.Actions.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }




                //ActionPros

                try
                {
                    //костль для восстановления id, для того что бы начиналось с 207, по идеи это не нужно, 
                    //for (var i = 0; i < 207; ++i)
                    //{
                    //    using (var db = new ApplicationDbContext())
                    //    {
                    //        var obj = new Domain.ActionPro();
                    //        db.ActionPros.Add(obj);
                    //        db.SaveChanges();
                    //        db.ActionPros.Remove(obj);
                    //        db.SaveChanges();
                    //    }
                    //}



                    //после action, pro
                    command.CommandText = "select * from ActionPros";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "actionId", "prosId");
                    foreach (var i in ldr)
                    {
                        var ActionId = Convert.ToInt32(i["actionId"].ToString().Trim());
                        var ProId = i["prosId"].ToString().Trim();
                        using (var db = new ApplicationDbContext())
                        {
                            var act = db.Actions.First(x1 => x1.Id == ActionId);
                            act.Pros.Add(db.Pros.First(x1 => x1.Id == ProId));
                            db.SaveChanges();
                        }
                    }
                    

                }
                catch (Exception e)
                {
                    throw e;
                }
              



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


                try
                {
                    //костль для восстановления id, для того что бы начиналось с 212
                    //for (var i = 0; i < 211; ++i)
                    //{
                    //    using (var db = new ApplicationDbContext())
                    //    {
                    //        var obj = new Domain.ActionSpec();
                    //        db.ActionSpecs.Add(obj);
                    //        db.SaveChanges();
                    //        db.ActionSpecs.Remove(obj);
                    //        db.SaveChanges();
                    //    }
                    //}


                    command.CommandText = "select * from ActionSpec";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "actionId", "specId");
                    foreach (var i in ldr)
                    {
                        var ActionId = Convert.ToInt32(i["actionId"].ToString().Trim());
                        var SpecId = i["specId"].ToString().Trim();

                        using (var db = new ApplicationDbContext())
                        {
                            var act = db.Actions.First(x1 => x1.Id == ActionId);
                            act.Specs.Add(db.Specs.First(x1 => x1.Id == SpecId));
                            db.SaveChanges();
                        }
                    }

                    
                }
                catch (Exception e)
                {
                    throw e;
                }





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



                try
                {
                    //костль для восстановления id, для того что бы начиналось с 212
                    //for(var i = 0; i < 211; ++i)
                    //{
                    //    using (var db = new ApplicationDbContext())
                    //    {
                    //        var obj = new Domain.ActionVrem();
                    //        db.ActionVrems.Add(obj);
                    //        db.SaveChanges();
                    //        db.ActionVrems.Remove(obj);
                    //        db.SaveChanges();
                    //    }
                    //}
                    command.CommandText = "select * from ActionVrem";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "actionId", "vremId");
                    foreach (var i in ldr)
                    {
                        var ActionId = Convert.ToInt32(i["actionId"].ToString().Trim());
                        var VremId = i["vremId"].ToString().Trim();
                        using (var db = new ApplicationDbContext())
                        {
                            var act = db.Actions.First(x1 => x1.Id == ActionId);
                            act.Vrems.Add(db.Vrems.First(x1 => x1.Id == VremId));

                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }





                

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
                            obj.FizVelChange = i["fizVelChange"].ToString().Trim();
                        obj.FizVelLeftBorder = Convert.ToDouble(i["fizVelLeftBorder"].ToString().Trim());
                        obj.FizVelRightBorder = Convert.ToDouble(i["fizVelRightBorder"].ToString().Trim());

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



                try
                {
                    command.CommandText = "select * from FeIndex";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "IDFE", "Index");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.FEIndex();

                        obj.IDFE = Convert.ToInt32(i["IDFE"].ToString().Trim());
                        obj.Index = i["Index"].ToString().Trim();
                        
                        using (var db = new ApplicationDbContext())
                        {
                            db.FEIndexs.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }




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
                //            if(lastIdfe== obj.Idfe&& lastBegin== obj.Begin)
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
                //            string[] tmpSpecial = i["special"].ToString().Split(new string[] {" " },StringSplitOptions.RemoveEmptyEntries);
                //            if(tmpSpecial!=null)
                //            foreach(var i2 in tmpSpecial)
                //            {
                //                    if (i2[0] == 'C')
                //                        obj.Special += " ";
                //                    obj.Special += i2;
                //            }
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



                //FeText



                try
                {
                    command.CommandText = "select * from FeText";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "IDFE", "name", "text", "textInp", "textOut", "textObj",
                        "textApp", "textLit");
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
                        
                        using (var db = new ApplicationDbContext())
                        {
                            db.FEText.Add(obj);
                            db.SaveChanges();
                        }
                        
                    }

                    
                }
                catch (Exception e)
                {
                    throw e;
                }




                



                //NewFeIndex



                try
                {
                    command.CommandText = "select * from NewFeIndex";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "idfe", "input", "output", "beginObjectState", "endObjectState",
                       "beginPhase", "endPhase");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.NewFEIndex();

                        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                        obj.Idfe = Convert.ToInt32(i["idfe"].ToString().Trim());
                        obj.Input = i["input"].ToString().Trim();
                        obj.Output = i["output"].ToString().Trim();
                        obj.BeginObjectState = i["beginObjectState"].ToString().Trim();
                        obj.EndObjectState = i["endObjectState"].ToString().Trim();
                        obj.BeginPhase = i["beginPhase"].ToString().Trim();
                        obj.EndPhase = i["endPhase"].ToString().Trim();
                        
                        using (var db = new ApplicationDbContext())
                        {
                            db.NewFEIndexs.Add(obj);
                            db.SaveChanges();
                        }
                    }

                    
                }
                catch (Exception e)
                {
                    throw e;
                }



                //NeZakon



                try
                {
                    command.CommandText = "select * from NeZakon";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "FizVel1", "FizVel2");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.NeZakon();
                        
                        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                        obj.FizVel1 = i["FizVel1"].ToString().Trim();
                        obj.FizVel2 = i["FizVel2"].ToString().Trim();
                        
                        using (var db = new ApplicationDbContext())
                        {
                            db.NeZakons.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }



                



                //------ReverseChains




                




                //-----TasksToSynthesys





                //Thes


                try
                {
                    command.CommandText = "select * from Thes";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent", "compatible", "path");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.The();
                        
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        obj.Parent = i["parent"].ToString().Trim();
                        
                        if (string.IsNullOrWhiteSpace(i["compatible"].ToString().Trim()))
                            obj.Compatible = null;
                        else
                            obj.Compatible = i["compatible"].ToString().Trim();
                        
                        if (string.IsNullOrWhiteSpace(i["path"].ToString().Trim()))
                            obj.Path = null;
                        else
                            obj.Path = i["path"].ToString().Trim();
                        
                        using (var db = new ApplicationDbContext())
                        {
                            db.Thes.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }





                //ThesChild



                try
                {
                    command.CommandText = "select * from ThesChild";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "nodeID", "childID", "order");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.ThesChild();
                        
                        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                        obj.NodeID = i["nodeID"].ToString().Trim();
                        obj.ChildID = i["childID"].ToString().Trim();
                        if (i["order"].ToString().Trim() == null || i["order"].ToString().Trim() == "")
                            obj.Order = null;
                        else
                            obj.Order = Convert.ToInt32(i["order"].ToString().Trim());
                        
                        using (var db = new ApplicationDbContext())
                        {
                            db.ThesChilds.Add(obj);
                            db.SaveChanges();
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    throw e;
                }








                //thes theschild 2

                using (var db=new ApplicationDbContext())
                {
                    db.StateObjects.Add(new StateObject()
                    {
                        Id="MONOFAZ",
                        Name= "Однофазное",
                        Parent= "STRUCTOBJECT",//ALLSTATE
                        CountPhase=1
                    });
                    db.StateObjects.Add(new StateObject()
                    {
                        Id = "POLYFAZ",
                        Name = "Многофазное",
                        Parent = "STRUCTOBJECT"

                    });
                    db.SaveChanges();
                }
                LoadStateObject("MONOFAZ");
                LoadStateObject("POLYFAZ");

                LoadCharacteristicObject("DESCOBJECT");
               







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
                    if (obj.Id == "3CONFAZ1")
                        obj.CountPhase = 3;
                    if(obj.Id == "2CONFAZ1" || obj.Id == "2CONFAZ2")
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



    }
    
    
}