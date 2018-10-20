using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicalEffectsSearchEngine.Models;
using dip.Models.TechnicalFunctions;

namespace dip.Controllers
{
    public class OperandGroupsController : Controller
    {
        private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        private readonly PhysicalEffectsEntities _PhysicalEffectsDb = new PhysicalEffectsEntities();

        // GET: OperandGroups
        public ActionResult Index()
        {
            var operandGroupEntities = _TechnicalFunctionsDb.OperandGroup;
            var fizVelsTable = _PhysicalEffectsDb.FizVels;
            const string parentValue = "VOZ11_FIZVEL";

            var allOperandGroupEntities =
                (from operandGroup in operandGroupEntities
                 select operandGroup).ToList();

            operandGroupEntities.RemoveRange(allOperandGroupEntities);
            _TechnicalFunctionsDb.SaveChanges();

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
            _TechnicalFunctionsDb.SaveChanges();

            return View(_TechnicalFunctionsDb.OperandGroup.ToList());
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