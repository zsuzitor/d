using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using PhysicalEffectsSearchEngine.Models;
using dip.Models.TechnicalFunctions;
using dip.Models;

namespace dip.Controllers
{
    //TODO чужой контроллер, не используется
    // [RequireHttps]
    [Authorize(Roles = "close")]//такой роли нет, закрываем контроллер
    public class OperandGroupsController : Controller
    {
        //private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        //private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: OperandGroups
        public ActionResult Index()
        {
            List<OperandGroup> res = new List<OperandGroup>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
               
                
                const string parentValue = "VOZ11_FIZVEL";

                //var allOperandGroupEntities =
                //    (from operandGroup in db.OperandGroups
                //     select operandGroup).ToList();

                db.OperandGroups.RemoveRange(db.OperandGroups.ToList());
                db.SaveChanges();

                var selectedFizVelses =
                    (from fizVel in db.FizVels
                     where fizVel.Parent == parentValue
                     select fizVel).ToList();

                var newOperandGroupEntities = new List<OperandGroup>();
                foreach (var fizVel in selectedFizVelses)
                {
                    var operandGroupEntity = new OperandGroup
                    {
                        Id = fizVel.Id,
                        Value = fizVel.Name
                    };
                    newOperandGroupEntities.Add(operandGroupEntity);
                }

                db.OperandGroups.AddRange(newOperandGroupEntities);
                db.SaveChanges();
                res = db.OperandGroups.ToList();
            }
            return View(res);
        }
       
    }
}