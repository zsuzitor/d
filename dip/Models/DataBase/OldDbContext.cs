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
            connection.ConnectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SOFI.mdf;Integrated Security=True";
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
                var connection1 = new SqlConnection();
                //(LocalDb)\MSSQLLocalDB    SQLEXPRESS01
                // connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=|DataDirectory|\TechnicalFunctions.mdf;Integrated Security=True";
                //connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=C:\rub\d_bd\TechnicalFunctions.mdf;Integrated Security=True";
                //connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=|DataDirectory|\TechnicalFunctions.mdf;Integrated Security=True;User Instance=False";


                //connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=C:\rub\d_bd\1\TechnicalFunctions.mdf;Integrated Security=True;User Instance=False";
                connection1.ConnectionString = @"Data Source=.\SQLEXPRESS01;AttachDbFilename=C:\rub\d_bd\1\TechnicalFunctions.mdf;Integrated Security=True;User Instance=False";

                var command1 = new SqlCommand();
                command1.Connection = connection1;
                command1.CommandType = CommandType.Text;

                connection1.Open();




                //Limit

                try
                {
                    command1.CommandText = "select * from Limit";
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new TechnicalFunctions.Limit();


                                obj.Id = reader["Id"].ToString();
                                obj.Value = reader["Value"].ToString();
                                obj.Parent = reader["Parent"].ToString();
                                using (var db = new ApplicationDbContext())
                                {
                                    db.Limits.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                        }

                    }

                }
                catch (Exception e)
                {
                    throw e;
                }




                //OperandGroup

                try
                {
                    command1.CommandText = "select * from OperandGroup";
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new TechnicalFunctions.OperandGroup();


                                obj.Id = reader["Id"].ToString();
                                obj.Value = reader["Value"].ToString();
                                using (var db = new ApplicationDbContext())
                                {
                                    db.OperandGroups.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                        }

                    }

                }
                catch (Exception e)
                {
                    throw e;
                }




                //Operation

                try
                {
                    command1.CommandText = "select * from Operation";
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new TechnicalFunctions.Operation();


                                obj.Id = reader["Id"].ToString();
                                obj.Value = reader["Value"].ToString();
                                obj.Parent = reader["Parent"].ToString();
                                using (var db = new ApplicationDbContext())
                                {
                                    db.Operations.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                        }

                    }

                }
                catch (Exception e)
                {
                    throw e;
                }





                //Operand

                try
                {
                    command1.CommandText = "select * from Operand";
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new TechnicalFunctions.Operand();


                                obj.Id = reader["Id"].ToString();
                                obj.Value = reader["Value"].ToString();
                                obj.OperandGroupId = reader["OperandGroupId"].ToString();
                                using (var db = new ApplicationDbContext())
                                {
                                    db.Operands.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                        }

                    }

                }
                catch (Exception e)
                {
                    throw e;
                }




                //Index
                try
                {
                    command1.CommandText = "select * from [Index]";
                    using (SqlDataReader reader = command1.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new TechnicalFunctions.Index();


                                obj.Id = reader["Id"].ToString();
                                obj.OperationId = reader["OperationId"].ToString();
                                obj.OperandId = reader["OperandId"].ToString();
                                obj.LimitId = reader["LimitId"].ToString();
                                obj.EffectIds = reader["EffectIds"].ToString();
                                using (var db = new ApplicationDbContext())
                                {
                                    db.Indexs.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                        }

                    }

                }
                catch (Exception e)
                {
                    throw e;
                }














            }



            //try
            {
                connection.Open();





                //Pros


                try
                {
                    command.CommandText = "select * from Pros";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.Pro();


                                obj.Id = reader["id"].ToString();
                                obj.Name = reader["name"].ToString();
                                obj.Parent = reader["parent"].ToString();

                                using (var db = new ApplicationDbContext())
                                {
                                    db.Pros.Add(obj);
                                    db.SaveChanges();
                                }

                            }
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
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.ActionType();

                                obj.Id = reader["id"].ToString();
                                obj.Name = reader["name"].ToString();
                                if (string.IsNullOrWhiteSpace(reader["parent"].ToString()))
                                    obj.Parent = null;
                                else
                                    obj.Parent = reader["parent"].ToString();

                                using (var db = new ApplicationDbContext())
                                {
                                    db.ActionTypes.Add(obj);
                                    db.SaveChanges();
                                }

                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }




                //FizVels


                try
                {
                    command.CommandText = "select * from FizVels";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.FizVel();

                                obj.Id = reader["id"].ToString();

                                obj.Name = reader["name"].ToString();


                                if (string.IsNullOrWhiteSpace(reader["parent"].ToString()))
                                    obj.Parent = null;
                                else
                                    obj.Parent = reader["parent"].ToString();

                                using (var db = new ApplicationDbContext())
                                {
                                    db.FizVels.Add(obj);
                                    db.SaveChanges();
                                }

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }




                //AllActions

                try
                {
                    command.CommandText = "select * from AllActions";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.AllAction();

                                obj.Id = reader["id"].ToString();
                                obj.Name = reader["name"].ToString();
                                obj.Parent = reader["parent"].ToString();

                                using (var db = new ApplicationDbContext())
                                {
                                    db.AllActions.Add(obj);
                                    db.SaveChanges();
                                }

                            }
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

                    //костль для восстановления id, для того что бы начиналось с 134
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

                    //SqlDataReader reader = command.ExecuteReader();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.Action();
                                obj.Id = Convert.ToInt32(reader["id"].ToString());
                                obj.AllActionId = reader["actionId"].ToString();
                                obj.ActionType_Id = reader["actionType"].ToString();
                                obj.FizVelId = reader["fizVelId"].ToString();
                                using (var db = new ApplicationDbContext())
                                {
                                    db.Actions.Add(obj);
                                    db.SaveChanges();
                                }

                            }
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
                    //SqlDataReader reader = command.ExecuteReader()
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                
                                var ActionId = Convert.ToInt32(reader["actionId"].ToString());
                                var ProId = reader["prosId"].ToString();
                                using (var db = new ApplicationDbContext())
                                {
                                    var act=db.Actions.First(x1=>x1.Id==ActionId);
                                    act.Pros.Add(db.Pros.First(x1=>x1.Id==ProId));
                                    db.SaveChanges();
                                }

                            }

                        }
                    }
                    // SqlDataReader reader = command.ExecuteReader();


                }
                catch (Exception e)
                {
                    throw e;
                }
                //finally
                //{
                //    reader.Close();
                //}





                //Spec



                try
                {
                    command.CommandText = "select * from Spec";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.Spec();


                                obj.Id = reader["id"].ToString();
                                obj.Name = reader["name"].ToString();
                                obj.Parent = reader["parent"].ToString();

                                using (var db = new ApplicationDbContext())
                                {
                                    db.Specs.Add(obj);
                                    db.SaveChanges();
                                }
                            }
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

                    //SqlDataReader reader = command.ExecuteReader();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //var obj = new Domain.ActionSpec();

                                var ActionId = Convert.ToInt32(reader["actionId"].ToString());
                                var SpecId = reader["specId"].ToString();

                                using (var db = new ApplicationDbContext())
                                {
                                    var act = db.Actions.First(x1 => x1.Id == ActionId);
                                    act.Specs.Add(db.Specs.First(x1 => x1.Id == SpecId));
                                    db.SaveChanges();
                                }

                            }

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
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.Vrem();


                                obj.Id = reader["id"].ToString();
                                obj.Name = reader["name"].ToString();
                                obj.Parent = reader["parent"].ToString();


                                using (var db = new ApplicationDbContext())
                                {
                                    db.Vrems.Add(obj);
                                    db.SaveChanges();
                                }
                            }
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
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //var obj = new Domain.ActionVrem();

                                var ActionId = Convert.ToInt32(reader["actionId"].ToString());
                                var VremId = reader["vremId"].ToString();


                                using (var db = new ApplicationDbContext())
                                {
                                    var act = db.Actions.First(x1 => x1.Id == ActionId);
                                    act.Vrems.Add(db.Vrems.First(x1 => x1.Id == VremId));

                                    db.SaveChanges();
                                }

                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }





                

                //---- Chains





                //FeAction

                try
                {
                    command.CommandText = "select * from FeAction";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.FEAction();

                                obj.Id = Convert.ToInt32(reader["id"].ToString());
                                obj.Idfe = Convert.ToInt32(reader["idfe"].ToString());
                                obj.Input = Convert.ToInt32(reader["input"].ToString());
                                obj.Type = reader["type"].ToString();
                                obj.Name = reader["name"].ToString();
                                obj.FizVelId = reader["fizVelId"].ToString();
                                obj.FizVelSection = reader["fizVelSection"].ToString();
                                obj.FizVelChange = reader["fizVelChange"].ToString();
                                obj.FizVelLeftBorder = Convert.ToDouble(reader["fizVelLeftBorder"].ToString());
                                obj.FizVelRightBorder = Convert.ToDouble(reader["fizVelRightBorder"].ToString());
                                obj.Pros = reader["pros"].ToString();
                                obj.Spec = reader["spec"].ToString();
                                obj.Vrem = reader["vrem"].ToString();



                                using (var db = new ApplicationDbContext())
                                {
                                    db.FEActions.Add(obj);
                                    db.SaveChanges();
                                }

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }


                //FeIndex



                try
                {
                    command.CommandText = "select * from FeIndex";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.FEIndex();

                                obj.IDFE = Convert.ToInt32(reader["IDFE"].ToString());
                                obj.Index = reader["Index"].ToString();


                                using (var db = new ApplicationDbContext())
                                {
                                    db.FEIndexs.Add(obj);
                                    db.SaveChanges();
                                }

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }




                //FeObject




                try
                {
                    command.CommandText = "select * from FeObject";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        // SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.FEObject();

                                obj.Id = Convert.ToInt32(reader["id"].ToString());

                                obj.Idfe = Convert.ToInt32(reader["idfe"].ToString());
                                obj.Begin = Convert.ToInt32(reader["begin"].ToString());
                                obj.PhaseState = reader["phaseState"].ToString();
                                obj.Composition = reader["composition"].ToString();
                                obj.MagneticStructure = reader["magneticStructure"].ToString();
                                obj.Conductivity = reader["conductivity"].ToString();
                                obj.MechanicalState = reader["mechanicalState"].ToString();
                                obj.OpticalState = reader["opticalState"].ToString();
                                obj.Special = reader["special"].ToString();



                                using (var db = new ApplicationDbContext())
                                {
                                    db.FEObjects.Add(obj);
                                    db.SaveChanges();
                                }

                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }



                //FeText



                try
                {
                    command.CommandText = "select * from FeText";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.FEText();

                                obj.IDFE = Convert.ToInt32(reader["IDFE"].ToString());
                                obj.Name = reader["name"].ToString();
                                obj.Text = reader["text"].ToString();
                                obj.TextInp = reader["textInp"].ToString();
                                obj.TextOut = reader["textOut"].ToString();
                                obj.TextObj = reader["textObj"].ToString();
                                obj.TextApp = reader["textApp"].ToString();
                                obj.TextLit = reader["textLit"].ToString();



                                using (var db = new ApplicationDbContext())
                                {
                                    db.FEText.Add(obj);
                                    db.SaveChanges();
                                }

                            }
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
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.NewFEIndex();

                                obj.Id = Convert.ToInt32(reader["id"].ToString());
                                obj.Idfe = Convert.ToInt32(reader["idfe"].ToString());
                                obj.Input = reader["input"].ToString();
                                obj.Output = reader["output"].ToString();
                                obj.BeginObjectState = reader["beginObjectState"].ToString();
                                obj.EndObjectState = reader["endObjectState"].ToString();
                                obj.BeginPhase = reader["beginPhase"].ToString();
                                obj.EndPhase = reader["endPhase"].ToString();


                                using (var db = new ApplicationDbContext())
                                {
                                    db.NewFEIndexs.Add(obj);
                                    db.SaveChanges();
                                }

                            }
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
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.NeZakon();


                                obj.Id = Convert.ToInt32(reader["id"].ToString());
                                obj.FizVel1 = reader["FizVel1"].ToString();
                                obj.FizVel2 = reader["FizVel2"].ToString();


                                using (var db = new ApplicationDbContext())
                                {
                                    db.NeZakons.Add(obj);
                                    db.SaveChanges();
                                }

                            }
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
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.The();


                                obj.Id = reader["id"].ToString();
                                obj.Name = reader["name"].ToString();
                                obj.Parent = reader["parent"].ToString();



                                if (string.IsNullOrWhiteSpace(reader["compatible"].ToString()))
                                    obj.Compatible = null;
                                else
                                    obj.Compatible = reader["compatible"].ToString();


                                if (string.IsNullOrWhiteSpace(reader["path"].ToString()))
                                    obj.Path = null;
                                else
                                    obj.Path = reader["path"].ToString();


                                using (var db = new ApplicationDbContext())
                                {
                                    db.Thes.Add(obj);
                                    db.SaveChanges();
                                }
                            }
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
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        //SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var obj = new Domain.ThesChild();


                                obj.Id = Convert.ToInt32(reader["id"].ToString());
                                obj.NodeID = reader["nodeID"].ToString();
                                obj.ChildID = reader["childID"].ToString();
                                if (reader["order"].ToString() == null || reader["order"].ToString() == "")
                                    obj.Order = null;
                                else
                                    obj.Order = Convert.ToInt32(reader["order"].ToString());


                                using (var db = new ApplicationDbContext())
                                {
                                    db.ThesChilds.Add(obj);
                                    db.SaveChanges();
                                }
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    throw e;
                }




                //настройки для fullTextSearch sql server


               // using (var db = new ApplicationDbContext())
                {
                    ObjectContext context =
                (new ApplicationDbContext() as IObjectContextAdapter).ObjectContext;
                    ObjectQuery<DbDataRecord> Customers =
                context.CreateQuery<DbDataRecord>("create fulltext catalog DbaFeTextCatalog");

                    ObjectQuery<DbDataRecord> Customers1 =
                context.CreateQuery<DbDataRecord>(@"create fulltext index on [dbo.FeTexts](
                    Name language  1049,
                    Text language  1049,
                    TextInp language  1049,
                    TextOut language  1049,
                    TextObj language  1049,
                    TextApp language  1049,
                    TextLit language  1049
                    )
                    key index PK_dbo.FeTexts
                    on DbaFeTextCatalog
                    with change_tracking auto;");
                    context.SaveChanges();

                    //                    db.Database.ExecuteSqlCommand(@"create fulltext catalog DbaFeTextCatalog");
                    //db.Database.ExecuteSqlCommand(@"create fulltext index on [dbo.FeTexts](
                    //Name language  1049,
                    //Text language  1049,
                    //TextInp language  1049,
                    //TextOut language  1049,
                    //TextObj language  1049,
                    //TextApp language  1049,
                    //TextLit language  1049
                    //)
                    //key index PK_dbo.FeTexts
                    //on DbaFeTextCatalog
                    //with change_tracking auto;");
                }



//# выполнение #sql скрипта 
                
//                var comps = db.Database.SqlQuery<Company>("SELECT * FROM Companies");
//                возвращает количество затронутых строк
//int numberOfRowInserted = db.Database.ExecuteSqlCommand("INSERT INTO Companies (Name) VALUES ('HTC')");
//                получить строку подключения
//db.Database.Connection.ConnectionString









                returnvalue = true;
            }
            //catch (Exception e)
            //{
            //    throw e;
            //}
            //finally
            //{
            //    connection.Close();
            //}
            return returnvalue;

        }


        //public struct tt
        //{
        //    public string Name { get; set; }// name var
        //    public int Type { get; set; }// 1-int  2-string
        //}
        //public void test(string TableNam,params tt[] mass)
        //{
        //    try
        //    {
        //        command.CommandText = "select * from "+ TableNam;

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                var obj = new Domain.ActionType();

        //                //obj.Id = reader["id"].ToString();
        //                //obj.Name = reader["name"].ToString();
        //                //obj.Parent = reader["parent"].ToString();
        //                for(int i = 0; i < mass.Count(); ++i)
        //                {

        //                }

        //                using (var db = new ApplicationDbContext())
        //                {
        //                    db.ActionTypes.Add(obj);
        //                    db.SaveChanges();
        //                }

        //            }

        //        }
        //    }
        //    catch { }
        //}

    }




}