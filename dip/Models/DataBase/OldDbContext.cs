using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        public OldData()
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
            try
            {
                connection.Open();



                //ActionPros

                try
                {
                    command.CommandText = "select * from ActionPros";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new ActionPro();
                            obj.ActionId = Convert.ToInt32(reader["actionId"].ToString());
                            obj.ProId = reader["prosId"].ToString();
                            using (var db=new ApplicationDbContext())
                            {
                                db.ActionPros.Add(obj);
                                db.SaveChanges();
                            }

                        }
                        
                    }
                }
                catch { }



                //Actions

                try
                {
                    command.CommandText = "select * from Actions";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.Action();
                            obj.Id = Convert.ToInt32(reader["id"].ToString());
                            obj.actionId = reader["actionId"].ToString();
                            obj.actionType = reader["actionType"].ToString();
                            obj.fizVelId = reader["fizVelld"].ToString();
                            using (var db = new ApplicationDbContext())
                            {
                                db.Actions.Add(obj);
                                db.SaveChanges();
                            }

                        }

                    }
                }
                catch { }




                //ActionSpec


                try
                {
                    command.CommandText = "select * from ActionSpec";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.ActionSpec();
                            
                            obj.ActionId = Convert.ToInt32(reader["actionId"].ToString());
                            obj.SpecId = reader["specId"].ToString();
                            
                            using (var db = new ApplicationDbContext())
                            {
                                db.ActionSpecs.Add(obj);
                                db.SaveChanges();
                            }

                        }

                    }
                }
                catch { }


                //ActionTypes



                try
                {
                    command.CommandText = "select * from ActionTypes";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.ActionType();

                            obj.Id =reader["id"].ToString();
                            obj.Name = reader["name"].ToString();
                            obj.Parent = reader["parent"].ToString();

                            using (var db = new ApplicationDbContext())
                            {
                                db.ActionTypes.Add(obj);
                                db.SaveChanges();
                            }

                        }

                    }
                }
                catch { }








                //ActionVrem



                try
                {
                    command.CommandText = "select * from ActionVrem";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.ActionVrem();

                            obj.ActionId =Convert.ToInt32( reader["actionId"].ToString());
                            obj.VremId = reader["vremId"].ToString();
                            

                            using (var db = new ApplicationDbContext())
                            {
                                db.ActionVrems.Add(obj);
                                db.SaveChanges();
                            }

                        }

                    }
                }
                catch { }





                //AllActions

                try
                {
                    command.CommandText = "select * from AllActions";

                    SqlDataReader reader = command.ExecuteReader();
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
                catch { }

                //---- Chains





                //FeAction

                try
                {
                    command.CommandText = "select * from FeAction";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.FEAction();

                            obj.Id = Convert.ToInt32( reader["id"].ToString());
                            obj.Idfe = Convert.ToInt32(reader["idfe"].ToString());
                            obj.Input = Convert.ToInt32(reader["input"].ToString());
                            obj.Type = reader["type"].ToString();
                            obj.Name = reader["name"].ToString();
                            obj.FizVelId = reader["fizVelld"].ToString();
                            obj.FizVelSection = reader["fizVelSection"].ToString();
                            obj.FizVelSection = reader["fizVelChange"].ToString();
                            obj.FizVelChange = reader["fizVelLeftBorder"].ToString();
                            obj.FizVelLeftBorder = Convert.ToInt32(reader["fizVelRightBorder"].ToString());
                            obj.FizVelRightBorder = Convert.ToInt32(reader["pros"].ToString());
                            obj.Id = Convert.ToInt32(reader["spec"].ToString());
                            obj.Id = Convert.ToInt32(reader["vrem"].ToString());
                            


                            using (var db = new ApplicationDbContext())
                            {
                                db.FEActions.Add(obj);
                                db.SaveChanges();
                            }

                        }

                    }
                }
                catch { }


                //FeIndex



                try
                {
                    command.CommandText = "select * from FeIndex";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.FEIndex();

                            obj.IDFE = Convert.ToInt32( reader["IDFE"].ToString());
                            obj.Index = reader["Index"].ToString();
                           

                            using (var db = new ApplicationDbContext())
                            {
                                db.FEIndexs.Add(obj);
                                db.SaveChanges();
                            }

                        }

                    }
                }
                catch { }




                //FeObject




                try
                {
                    command.CommandText = "select * from FeObject";

                    SqlDataReader reader = command.ExecuteReader();
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
                catch { }



                //FeText



                try
                {
                    command.CommandText = "select * from FeText";

                    SqlDataReader reader = command.ExecuteReader();
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
                catch { }




                //FizVels


                try
                {
                    command.CommandText = "select * from FizVels";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.FizVel();

                            obj.Id = reader["id"].ToString();

                            obj.Name = reader["name"].ToString();
                            obj.Parent = reader["parent"].ToString();
                           

                            
                            using (var db = new ApplicationDbContext())
                            {
                                db.FizVels.Add(obj);
                                db.SaveChanges();
                            }

                        }

                    }
                }
                catch { }



                //NewFeIndex



                try
                {
                    command.CommandText = "select * from NewFeIndex";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.NewFEIndex();

                            obj.Id =Convert.ToInt32( reader["id"].ToString());
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
                catch { }



                //NeZakon



                try
                {
                    command.CommandText = "select * from NeZakon";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.NeZakon();

                            
                            obj.Id = Convert.ToInt32( reader["id"].ToString());
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
                catch { }




                //Pros


                try
                {
                    command.CommandText = "select * from Pros";

                    SqlDataReader reader = command.ExecuteReader();
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
                catch { }



                //------ReverseChains




                //Spec



                try
                {
                    command.CommandText = "select * from Spec";

                    SqlDataReader reader = command.ExecuteReader();
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
                catch { }




                //-----TasksToSynthesys





                //Thes


                try
                {
                    command.CommandText = "select * from Thes";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.The();


                            obj.Id = reader["id"].ToString();
                            obj.Name = reader["name"].ToString();
                            obj.Parent = reader["parent"].ToString();
                            obj.Compatible = reader["compatible"].ToString();
                            obj.Path = reader["path"].ToString();

                            using (var db = new ApplicationDbContext())
                            {
                                db.Thes.Add(obj);
                                db.SaveChanges();
                            }

                        }

                    }
                }
                catch { }





                //ThesChild



                try
                {
                    command.CommandText = "select * from ThesChild";

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var obj = new Domain.ThesChild();


                            obj.Id = Convert.ToInt32( reader["id"].ToString());
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
                catch { }





                //Vrem



                try
                {
                    command.CommandText = "select * from Vrem";

                    SqlDataReader reader = command.ExecuteReader();
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
                catch { }



                returnvalue = true;
            }
            catch
            { }
            finally
            {
                connection.Close();
            }
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