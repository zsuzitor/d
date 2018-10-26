using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using PhysicalEffectsSearchEngine.Models;
using dip.Models.TechnicalFunctions;
using System.Data.Entity;
using dip.Models;

namespace dip.Controllers
{
    // [RequireHttps]
    public class OperandsController : Controller
    {
        //private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Operands
        public ActionResult Index()
        {
            var operandEntities = db.Operands;
            var operandGroupEntities = db.OperandGroups;
            var fizVelsEntities = db.FizVels;

            var allOperandEntities =
                (from operand in operandEntities
                 select operand).ToList();

            operandEntities.RemoveRange(allOperandEntities);
            db.SaveChanges();

            var newOperandEntities = new List<Operand>();
            foreach (var operandGroup in operandGroupEntities)
            {
                var selectedFizVelses =
                    (from fizVel in fizVelsEntities
                     where fizVel.Parent == operandGroup.Id
                     select fizVel).ToList();

                foreach (var fizVel in selectedFizVelses)
                {
                    var operandEntity = new Operand
                    {
                        Id = fizVel.Id,
                        Value = fizVel.Name,
                        OperandGroup = operandGroup
                    };
                    newOperandEntities.Add(operandEntity);
                }
            }

            operandEntities.AddRange(newOperandEntities);
            db.SaveChanges();

            return View(db.Operands.Include(o => o.OperandGroup).ToList());
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