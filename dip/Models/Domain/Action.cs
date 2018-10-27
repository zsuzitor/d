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
        public ICollection<ActionSpec> ActionSpecs { get; set; }
        public ICollection<ActionVrem> ActionVrems { get; set; }
        
       
       

        public Action()
        {

        }
    }
}
