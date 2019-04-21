using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models.ViewModel.PhysicV
{
    public class EditV
    {
        public FEText Obj { get; set; }
        public List<DescrSearchI> FormsInput { get; set; }
        public List<DescrSearchI> FormsOutput { get; set; }

        public DescrObjectI FormObjectBegin { get; set; }
        public int CountPhaseBegin { get; set; }

        public DescrObjectI FormObjectEnd { get; set; }
        public int CountPhaseEnd { get; set; }

        //public bool ChangedObject { get; set; }
        //public string StateIdStart { get; set; }
        //public string StateIdEnd { get; set; }


        public EditV()
        {
            Obj = null;
            FormsInput = null;
            FormsOutput = null;
            FormObjectBegin = new DescrObjectI();
            FormObjectEnd = new DescrObjectI();

            CountPhaseBegin = 0;
            CountPhaseEnd = 0;
        }



        public void Data(int? id)
        {
            List<FEAction> inp = null;
            List<FEAction> outp = null;
            FEAction.Get((int)id, ref inp, ref outp);
            List<FEObject> inpObj = null;
            List<FEObject> outpObj = null;
            FEObject.Get((int)id, ref inpObj, ref outpObj);

            this.FormsInput = inp.Select(x1 => {
                var rs = new DescrSearchI(x1);
                //rs.ListSelectedPros = Pro.GetAllIdsFor(rs.ListSelectedPros);
                //rs.ListSelectedVrem = Vrem.GetAllIdsFor(rs.ListSelectedVrem);
                //rs.ListSelectedSpec = Spec.GetAllIdsFor(rs.ListSelectedSpec);
                return rs;
            }).ToList();
            this.FormsOutput = outp.Select(x1 => {
                var rs = new DescrSearchI(x1);
                //rs.ListSelectedPros = Pro.GetAllIdsFor(rs.ListSelectedPros);
                //rs.ListSelectedVrem = Vrem.GetAllIdsFor(rs.ListSelectedVrem);
                //rs.ListSelectedSpec = Spec.GetAllIdsFor(rs.ListSelectedSpec);
                return rs;
            }).ToList();// new DescrSearchI(outp);

            if (inpObj != null)
            {
                var objTmp = inpObj.FirstOrDefault(x1 => x1.NumPhase == 1);
                if (objTmp != null)
                    this.FormObjectBegin.ListSelectedPhase1 = new DescrPhaseI(objTmp);//.Select(x1=>new DescrPhaseI(x1));
                objTmp = inpObj.FirstOrDefault(x1 => x1.NumPhase == 2);
                if (objTmp != null)
                    this.FormObjectBegin.ListSelectedPhase2 = new DescrPhaseI(objTmp);
                objTmp = inpObj.FirstOrDefault(x1 => x1.NumPhase == 3);
                if (objTmp != null)
                    this.FormObjectBegin.ListSelectedPhase3 = new DescrPhaseI(objTmp);
                this.CountPhaseBegin = this.FormObjectBegin.GetCountPhase();
            }

            if (outpObj != null)
            {
                var objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 1);
                if (objTmp != null)
                    this.FormObjectEnd.ListSelectedPhase1 = new DescrPhaseI(objTmp);
                objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 2);
                if (objTmp != null)
                    this.FormObjectEnd.ListSelectedPhase2 = new DescrPhaseI(objTmp);// outpObj.FirstOrDefault(x1 => x1.NumPhase == 2));
                objTmp = outpObj.FirstOrDefault(x1 => x1.NumPhase == 3);
                if (objTmp != null)
                    this.FormObjectEnd.ListSelectedPhase3 = new DescrPhaseI(objTmp);//outpObj.FirstOrDefault(x1 => x1.NumPhase == 3));
                this.CountPhaseEnd = this.FormObjectEnd.GetCountPhase();
            }



            this.Obj.LoadImage();
            this.Obj.LoadLatex();
        }

    }
}