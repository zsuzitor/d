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
        SqlConnection connection;
        SqlCommand command;

        public OldData()
        {
            connection = new SqlConnection();
            connection.ConnectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SOFI.mdf;Integrated Security=True";
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
        } // constructor

        public bool GetUsersData(ref string lastname, ref string firstname, ref string age)
        {
            bool returnvalue = false;
            try
            {
                command.CommandText = "select * from ActionPros";
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        lastname = reader["actionId"].ToString();
                        firstname = reader["prosId"].ToString();

                    }
                }
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

    }




}