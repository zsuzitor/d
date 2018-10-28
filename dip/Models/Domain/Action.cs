using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace dip.Models.Domain
{
    [Table("Actions")]
    public class Action
    {
        [Key]
        public int Id { get; set; }


        public string AllActionId { get; set; }
        public AllAction AllAction { get; set; }

        public string ActionType_Id { get; set; }
        public ActionType ActionType_ { get; set; }

        public string FizVelId { get; set; }
        public FizVel FizVel { get; set; }


        //судя по сгенеренным классам это поле к actionId
        //public AllAction AllAction { get; set; }



        public ICollection<ActionPro> ActionPros { get; set; }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Action_ActionSpec",  ThisKey = "Id", OtherKey = "ActionId")]
        public ICollection<ActionSpec> ActionSpecs { get; set; }

        [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Action_ActionVrem",  ThisKey = "Id", OtherKey = "ActionId")]
        public ICollection<ActionVrem> ActionVrems { get; set; }
        
       
       

        public Action()
        {

        }
    }
}
