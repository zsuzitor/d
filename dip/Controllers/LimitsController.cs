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
    // [RequireHttps]
    public class LimitsController : Controller
    {
        //private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        //private readonly ApplicationDbContext db = new ApplicationDbContext();

        private const string ConstOperationId = "CONST";
        private const string ChangeOperationId = "CHANGE";
        private const string IncOperationId = "INC";
        private const string DecOperationId = "DEC";

        private const string DegLimitIdSuffix = "DEG";
        private const string LineLimitIdSuffix = "LINE";
        private const string VipLimitIdSuffix = "VIP";
        private const string VogLimitIdSuffix = "VOG";

        private const string IncOperationParent = "MONO";
        private const string DefaultOperationParent = "NO_PARENT";

        private const string Delimiter = "_";

        private const string NoLimitIdSuffix = "NO_LIMIT";
        private const string NoLimitValue = "Без ограничений";

        private const string DefaultLimitParent = "NO_PARENT";

        private static string CombineLimitProperty(string thesProperty, string operationId)
        {
            return string.Join(Delimiter, thesProperty, operationId);
        }

        private void IndexLimits(ref List<Limit> limitEntities, string operationId, string parent)
        {
            List<The> selectedTheses = new List<The>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                 selectedTheses =
                (from thes in db.Thes
                 where thes.Parent == parent
                 select thes).ToList();
            }
            foreach (var thes in selectedTheses)
                {
                    var limitEntity = new Limit();
                    switch (operationId)
                    {
                        case ChangeOperationId:
                            limitEntity.Value = thes.Name;

                            if (thes.Id.Contains(IncOperationId) &&
                                (thes.Id.Contains(DegLimitIdSuffix) || thes.Id.Contains(LineLimitIdSuffix) ||
                                 thes.Id.Contains(VipLimitIdSuffix) || thes.Id.Contains(VogLimitIdSuffix)))
                            {
                                limitEntity.Id = CombineLimitProperty(operationId, thes.Id.Substring(IncOperationId.Length));
                                limitEntity.Parent = CombineLimitProperty(operationId, IncOperationParent);
                                limitEntities.Add(limitEntity);
                            }
                            else if (thes.Id != ConstOperationId && thes.Id != ChangeOperationId &&
                                     thes.Id != IncOperationId && !thes.Id.Contains(DecOperationId))
                            {
                                limitEntity.Id = CombineLimitProperty(operationId, thes.Id);
                                limitEntity.Parent =
                                    parent == ChangeOperationId
                                    ? DefaultOperationParent
                                    : CombineLimitProperty(operationId, parent);
                                limitEntities.Add(limitEntity);
                            }
                            break;

                        case IncOperationId:
                            if ((thes.Id.Contains(IncOperationId) &&
                                 (thes.Id.Contains(DegLimitIdSuffix) || thes.Id.Contains(LineLimitIdSuffix) ||
                                  thes.Id.Contains(VipLimitIdSuffix) || thes.Id.Contains(VogLimitIdSuffix))))
                            {
                                limitEntity.Value = thes.Name;
                                limitEntity.Id = CombineLimitProperty(operationId, thes.Id.Substring(IncOperationId.Length));
                                limitEntity.Parent = CombineLimitProperty(operationId, IncOperationParent);
                                limitEntities.Add(limitEntity);
                            }
                            break;

                        case DecOperationId:
                            if ((thes.Id.Contains(DecOperationId) &&
                                 (thes.Id.Contains(DegLimitIdSuffix) || thes.Id.Contains(LineLimitIdSuffix) ||
                                  thes.Id.Contains(VipLimitIdSuffix) || thes.Id.Contains(VogLimitIdSuffix))))
                            {
                                limitEntity.Value = thes.Name;
                                limitEntity.Id = CombineLimitProperty(operationId, thes.Id.Substring(DecOperationId.Length));
                                limitEntity.Parent = CombineLimitProperty(operationId, IncOperationParent);
                                limitEntities.Add(limitEntity);
                            }
                            break;
                    }

                    IndexLimits(ref limitEntities, operationId, thes.Id);
                }
            
        }

        // GET: Limits
        public ActionResult Index()
        {
            //var limitEntities = db.Limits;
            //var operationEntities = db.Operations;

            //var allLimitEntities =
            //    (from limit in db.Limits
            //     select limit).ToList();
           

                const string baseParent = "CHARACTER";
                var newLimitEntities = new List<Limit>();
            List<Operation> operations_list = new List<Operation>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Limits.RemoveRange(db.Limits.ToList());
                db.SaveChanges();

                operations_list = db.Operations.ToList();
            }
                foreach (var operation in operations_list)
                {
                    var noLimitEntity = new Limit
                    {
                        Id = CombineLimitProperty(operation.Id, NoLimitIdSuffix),
                        Value = NoLimitValue
                    };

                    if (operation.Id == IncOperationId || operation.Id == DecOperationId)
                        noLimitEntity.Parent = CombineLimitProperty(ChangeOperationId, NoLimitIdSuffix);
                    else
                        noLimitEntity.Parent = DefaultLimitParent;

                    newLimitEntities.Add(noLimitEntity);
                    IndexLimits(ref newLimitEntities, operation.Id, baseParent);
                }
            List<Limit> res = new List<Limit>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                
                db.Limits.AddRange(newLimitEntities);
                db.SaveChanges();
                res = db.Limits.ToList();
            }
            return View(res);
        }

        
    }
}