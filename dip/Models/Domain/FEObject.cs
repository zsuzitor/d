using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    //1 фаза объекта, данные
    public class FEObject
    {

        public int Id { get; set; }
        public int Idfe { get; set; }
        public int Begin { get; set; }
        public int NumPhase { get; set; }

        public string PhaseState { get; set; }
        public string Composition { get; set; }
        public string MagneticStructure { get; set; }
        public string Conductivity { get; set; }
        public string MechanicalState { get; set; }
        public string OpticalState { get; set; }
        public string Special { get; set; }

        public FEObject()
        {
            NumPhase = 1;

            PhaseState = "";
            Composition = "";
            MagneticStructure = "";
            Conductivity = "";
            MechanicalState = "";
            OpticalState = "";
            Special = "";

        }
        public FEObject(DescrPhaseI a,  int idfe, int begin)//string str, int idfe, int begin)
        {
            this.Idfe = idfe;
            this.Begin = begin;
            //string[] massStr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (var i in massStr)
            //    switch (i[0])
            //    {
            //        case 'F':
            //            PhaseState += i + " ";
            //            break;
            //        case 'X':
            //            Composition += i + " ";
            //            break;
            //        case 'M':
            //            MagneticStructure += i + " ";
            //            break;
            //        case 'E':
            //            Conductivity += i + " ";
            //            break;
            //        case 'D':
            //            MechanicalState += i + " ";
            //            break;
            //        case 'O':
            //            OpticalState += i + " ";
            //            break;
            //        case 'C':
            //            Special += i + " ";
            //            break;
            //    }
            
            PhaseState = PhaseCharacteristicObject.DeleteNotChildCheckbox(a.PhaseState);
            Composition = PhaseCharacteristicObject.DeleteNotChildCheckbox(a.Composition);
            MagneticStructure = PhaseCharacteristicObject.DeleteNotChildCheckbox(a.MagneticStructure);
            Conductivity = PhaseCharacteristicObject.DeleteNotChildCheckbox(a.Conductivity);
            MechanicalState = PhaseCharacteristicObject.DeleteNotChildCheckbox(a.MechanicalState);
            OpticalState = PhaseCharacteristicObject.DeleteNotChildCheckbox(a.OpticalState);
            Special = PhaseCharacteristicObject.DeleteNotChildCheckbox(a.Special);

        }

        public FEObject(DescrPhaseI a):this(a,a.Idfe,a.Begin)
        {
            
        }

        public static bool Get(int id, ref List<FEObject> inp, ref List<FEObject> outp)
        {
            using (var db=new ApplicationDbContext())
            {
                var lst=db.FEObjects.Where(x1=>x1.Idfe==id).ToList();
                inp = lst.Where(x1=>x1.Begin==1).ToList();
                outp = lst.Where(x1 => x1.Begin == 0).ToList();
            }



                return true;
        }

    } 
}