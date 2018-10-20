using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicalEffectsSearchEngine.Models;
using dip.Models.TechnicalFunctions;
using System.Data.Entity;

namespace dip.Controllers
{
    public class OperandsController : Controller
    {
        private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        private readonly PhysicalEffectsEntities _PhysicalEffectsDb = new PhysicalEffectsEntities();

        // GET: Operands
        public ActionResult Index()
        {
            var operandEntities = _TechnicalFunctionsDb.Operand;
            var operandGroupEntities = _TechnicalFunctionsDb.OperandGroup;
            var fizVelsEntities = _PhysicalEffectsDb.FizVels;

            var allOperandEntities =
                (from operand in operandEntities
                 select operand).ToList();

            operandEntities.RemoveRange(allOperandEntities);
            _TechnicalFunctionsDb.SaveChanges();

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
            _TechnicalFunctionsDb.SaveChanges();

            return View(_TechnicalFunctionsDb.Operand.Include(o => o.OperandGroup).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _TechnicalFunctionsDb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}