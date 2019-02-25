//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
////using PhysicalEffectsSearchEngine.Models;
//using dip.Models.TechnicalFunctions;
//using System.Data.Entity;
//using dip.Models;

//namespace dip.Controllers
//{
//    //TODO чужой контроллер, не используется
//    // [RequireHttps]
//    [Authorize(Roles = "close")]//такой роли нет, закрываем контроллер
//    public class OperandsController : Controller
//    {
//        //private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
//        //private readonly ApplicationDbContext db = new ApplicationDbContext();

//        // GET: Operands
//        public ActionResult Index()
//        {

//            //var operandGroupEntities = ;
//            //var fizVelsEntities = ;

//            //var allOperandEntities =
//            //    (from operand in operandEntities
//            //     select operand).ToList();
//            List<Operand> res = new List<Operand>();
//            using (ApplicationDbContext db = new ApplicationDbContext())
//            {
//                db.Operands.RemoveRange(db.Operands.ToList());
//                db.SaveChanges();

//                var newOperandEntities = new List<Operand>();
//                foreach (var operandGroup in db.OperandGroups.ToList())
//                {
//                    var selectedFizVelses =
//                        (from fizVel in db.FizVels
//                         where fizVel.Parent == operandGroup.Id
//                         select fizVel).ToList();

//                    foreach (var fizVel in selectedFizVelses)
//                    {
//                        var operandEntity = new Operand
//                        {
//                            Id = fizVel.Id,
//                            Value = fizVel.Name,
//                            OperandGroup = operandGroup
//                        };
//                        newOperandEntities.Add(operandEntity);
//                    }
//                }

//                db.Operands.AddRange(newOperandEntities);
//                db.SaveChanges();
//                res = db.Operands.Include(o => o.OperandGroup).ToList();
//            }
//            return View(res);
//        }

        
//    }
//}