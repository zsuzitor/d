using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    public class FEAction
    {
        public int Id { get; set; }
        public int Idfe { get; set; }
        public int Input { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        
        public string FizVelSection { get; set; }
        //public string FizVelChange { get; set; }
        //public double FizVelLeftBorder { get; set; }
       // public double FizVelRightBorder { get; set; }
        public string Pros { get; set; }
        public string Spec { get; set; }
        public string Vrem { get; set; }

        public string FizVelId { get; set; }
        //? не сгенерировалось
        public FizVel FizVel { get; set; }
        public FEAction()
        {
            Type = "";
            Name = "";
            FizVelSection = "";
           // FizVelChange = "";
            Pros = "";
            Spec = "";
            Vrem = "";
            FizVelId = null;//тк прописано внешним ключом

        }

        public static void Get(int FETextId,ref List<FEAction> inp, ref List<FEAction> outp)
        {
            List<FEAction> lst = null;
            using (var db = new ApplicationDbContext())
            {
                 lst= db.FEActions.Where(x1=>x1.Idfe==FETextId).ToList();
            }
            inp = lst.Where(x1=>x1.Input==1).ToList();
            outp = lst.Where(x1 => x1.Input == 0).ToList();
        }

        public void SetFromInput(DescrSearchI a)
        {
            Name = a?.ActionId;
            Type = a?.ActionType;
            FizVelId = string.IsNullOrWhiteSpace(a?.FizVelId)?null: a?.FizVelId;
            FizVelSection = a?.ParametricFizVelId;
            Pros = a?.ListSelectedPros;
            Spec = a?.ListSelectedSpec;
            Vrem = a?.ListSelectedVrem;
            if(a?.InputForm!=null)
            Input = (a.InputForm? 1 : 0);


        }

    }
}