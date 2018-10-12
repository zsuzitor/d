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
        public string actionId { get; set; }
        public string actionType { get; set; }
        public string fizVelId { get; set; }




        public ICollection<ActionPro> ActionPros { get; set; }
        public ICollection<ActionSpec> ActionSpecs { get; set; }
        public ICollection<ActionVrem> ActionVrems { get; set; }
        public ICollection<ActionType> ActionType1 { get; set; }
        public ICollection<AllAction> AllAction { get; set; }
        public ICollection<FizVel> FizVel { get; set; }

        public Action()
        {

        }
    }
}
