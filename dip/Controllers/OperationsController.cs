using dip.Models;
using dip.Models.Domain;
using dip.Models.TechnicalFunctions;
//using PhysicalEffectsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    //TODO чужой контроллер, не используется
    // [RequireHttps]
    [Authorize(Roles = "close")]//такой роли нет, закрываем контроллер
    public class OperationsController : Controller
    {
        //private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        //private readonly ApplicationDbContext db = new ApplicationDbContext();

        private const string ConstOperationId = "CONST";
        private const string ChangeOperationId = "CHANGE";
        private const string IncOperationId = "INC";
        private const string DecOperationId = "DEC";

        private const string DefaultOperationParent = "NO_PARENT";

        // GET: Operations
        public ActionResult Index()
        {
            //var operationEntities = ;
            //var thesEntities = ;

            //var allOperationEntities =
            //    (from operation in operationEntities
            //     select operation).ToList();
            List<The> selectedTheses = new List<The>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Operations.RemoveRange(db.Operations.ToList());
                db.SaveChanges();

                 selectedTheses =
                    (from thes in db.Thes
                     where thes.Id == ConstOperationId || thes.Id == ChangeOperationId ||
                           thes.Id == IncOperationId || thes.Id == DecOperationId
                     select thes).ToList();
            }
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
            List<Operation> res = new List<Operation>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Operations.AddRange(newOperationEntities);
                db.SaveChanges();
                res = db.Operations.ToList();
            }
            return View(res);
        }

        
    }
}