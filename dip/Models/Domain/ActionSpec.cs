//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Web;

//namespace dip.Models.Domain
//{
//    public class ActionSpec
//    {
//        [Key]
//        public int Id { get; set; }
//        //[ForeignKey("Action")]
//        //[global::System.Data.Linq.Mapping.ColumnAttribute( DbType = "Int NOT NULL", IsPrimaryKey = true)]

//        public int ActionId { get; set; }

//       // [global::System.Data.Linq.Mapping.AssociationAttribute(Name = "Action_ActionSpec", ThisKey = "ActionId", OtherKey = "Id", IsForeignKey = true, DeleteOnNull = true, DeleteRule = "CASCADE")]
//        public Action Action { get; set; }

//        public string SpecId { get; set; }
//        public Spec Spec { get; set; }


//        public ActionSpec()
//        {

//        }

//    }
//}