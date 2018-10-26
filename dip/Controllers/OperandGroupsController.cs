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
    // [RequireHttps]
    public class OperandGroupsController : Controller
    {
        //private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: OperandGroups
        public ActionResult Index()
        {
            var operandGroupEntities = db.OperandGroups;
            var fizVelsTable = db.FizVels;
            const string parentValue = "VOZ11_FIZVEL";

            var allOperandGroupEntities =
                (from operandGroup in operandGroupEntities
                 select operandGroup).ToList();

            operandGroupEntities.RemoveRange(allOperandGroupEntities);
            db.SaveChanges();

            var selectedFizVelses =
                (from fizVel in fizVelsTable
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

            operandGroupEntities.AddRange(newOperandGroupEntities);
            db.SaveChanges();

            return View(db.OperandGroups.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}