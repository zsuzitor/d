using dip.Models.TechnicalFunctions;
using PhysicalEffectsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    public class OperationsController : Controller
    {
        private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        private readonly PhysicalEffectsEntities _PhysicalEffectsDb = new PhysicalEffectsEntities();

        private const string ConstOperationId = "CONST";
        private const string ChangeOperationId = "CHANGE";
        private const string IncOperationId = "INC";
        private const string DecOperationId = "DEC";

        private const string DefaultOperationParent = "NO_PARENT";

        // GET: Operations
        public ActionResult Index()
        {
            var operationEntities = _TechnicalFunctionsDb.Operation;
            var thesEntities = _PhysicalEffectsDb.Thes;

            var allOperationEntities =
                (from operation in operationEntities
                 select operation).ToList();

            operationEntities.RemoveRange(allOperationEntities);
            _TechnicalFunctionsDb.SaveChanges();

            var selectedTheses =
                (from thes in thesEntities
                 where thes.Id == ConstOperationId || thes.Id == ChangeOperationId ||
                       thes.Id == IncOperationId || thes.Id == DecOperationId
                 select thes).ToList();

            const string constOperationValue = "Стабилизация";
            var newOperationEntities = new List<Operation>();

            foreach (var thes in selectedTheses)
            {
                var operationEntity = new Operation
                {
                    Id = thes.Id,
                    Value =
                        thes.Id == ConstOperationId
                            ? constOperationValue
                            : thes.Name
                };

                if (thes.Id == ConstOperationId || thes.Id == ChangeOperationId)
                {
                    operationEntity.Parent = DefaultOperationParent;
                }
                else if (thes.Id == IncOperationId || thes.Id == DecOperationId)
                {
                    operationEntity.Parent = ChangeOperationId;
                }
                else
                {
                    operationEntity.Parent = thes.Parent;
                }

                newOperationEntities.Add(operationEntity);
            }

            operationEntities.AddRange(newOperationEntities);
            _TechnicalFunctionsDb.SaveChanges();

            return View(_TechnicalFunctionsDb.Operation.ToList());
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