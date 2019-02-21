using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Models.Domain
{

    //---
    [Table("Actions")]
    public class Action
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }


        public string AllActionId { get; set; }
        public AllAction AllAction { get; set; }

        public string ActionType_Id { get; set; }
        public ActionType ActionType_ { get; set; }

        public string FizVelId { get; set; }
        public FizVel FizVel { get; set; }


        //судя по сгенеренным классам это поле к actionId
        //public AllAction AllAction { get; set; }



        public ICollection<Pro> Pros { get; set; }

       // [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Action_ActionSpec",  ThisKey = "Id", OtherKey = "ActionId")]
        public ICollection<Spec> Specs { get; set; }

        //[global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Action_ActionVrem",  ThisKey = "Id", OtherKey = "ActionId")]
        public ICollection<Vrem> Vrems { get; set; }
        
       
       

        public Action()
        {
            Pros = new List<Pro>();
            Specs = new List<Spec>();
            Vrems = new List<Vrem>();
        }



        public static Action GetAction(int id)
        {
            Action res=null;
            if (id > 0)
                using (var db = new ApplicationDbContext())
                    res = db.Actions.FirstOrDefault(x1 => x1.Id == id);
            return res;
        }

    }
}
