using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
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





        public DescrPhaseI this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return ListSelectedPhase1;
                        break;
                    case 1:
                        return ListSelectedPhase2;
                        break;
                    case 2:
                        return ListSelectedPhase3;
                        break;
                    default:
                        return null;
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        ListSelectedPhase1 = value;
                        break;
                    case 1:
                        ListSelectedPhase2 = value;
                        break;
                    case 2:
                        ListSelectedPhase3 = value;
                        break;

                }
            }
        }

        public IEnumerator<DescrPhaseI> GetEnumerator()
        {

            yield return ListSelectedPhase1;
            yield return ListSelectedPhase2;
            yield return ListSelectedPhase3;
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


        //удаляет прямых родителей если и родитель и ребенок есть в строке
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
}