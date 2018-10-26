using dip.Models.Domain;
using dip.Models.TechnicalFunctions;
//using PhysicalEffectsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using dip.Models;

namespace dip.Controllers
{
    // [RequireHttps]
    public class IndexesController : Controller
    {
        //private readonly db _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        private const string ChangeOperationId = "CHANGE";
        private const string ConstOperationId = "CONST";
        private const string IncOperationId = "INC";
        private const string DecOperationId = "DEC";

        private const string DegLimitIdSuffix = "DEG";

        private const string Delimiter = "_";

        private const string NoLimitIdSuffix = "NO_LIMIT";

        private int _IndexCounter = 1;
        private const string IndexIdPrefix = "FUNC";

        private List<FEAction> GetActionsWithChange(string operandId, string fizVelChangeId)
        {
            var fEActionEntities = db.FEActions;

            return
                (from fEAction in fEActionEntities
                 where fEAction.Input == 0 &&
                       fEAction.FizVelId == operandId &&
                       fEAction.FizVelChange == fizVelChangeId
                 select fEAction).ToList();
        }

        private Operation GetOperationById(string id)
        {
            var operationEntities = db.Operations;

            return
                (from operation in operationEntities
                 where operation.Id == id
                 select operation).First();
        }

        private Limit GetLimitById(string id)
        {
            var limitEntities = db.Limits;

            return
                (from limit in limitEntities
                 where limit.Id == id
                 select limit).First();
        }

        private List<Index> CombineQueries(Operand operand)
        {
            const string constId = "CONST";
            const string incId = "INC";
            const string decId = "DEC";
            const string incDegId = "INCDEG";
            const string decDegId = "DECDEG";

            var constOperation = GetOperationById(ConstOperationId);
            var changeOperation = GetOperationById(ChangeOperationId);
            var incOperation = GetOperationById(IncOperationId);
            var decOperation = GetOperationById(DecOperationId);

            var constNoLimit = GetLimitById(string.Join(Delimiter, ConstOperationId, NoLimitIdSuffix));
            var changeNoLimit = GetLimitById(string.Join(Delimiter, ChangeOperationId, NoLimitIdSuffix));
            var incNoLimit = GetLimitById(string.Join(Delimiter, IncOperationId, NoLimitIdSuffix));
            var decNoLimit = GetLimitById(string.Join(Delimiter, DecOperationId, NoLimitIdSuffix));

            var changeDegLimit = GetLimitById(string.Join(Delimiter, ChangeOperationId, DegLimitIdSuffix));
            var incDegLimit = GetLimitById(string.Join(Delimiter, IncOperationId, DegLimitIdSuffix));
            var decDegLimit = GetLimitById(string.Join(Delimiter, DecOperationId, DegLimitIdSuffix));

            var constActions = GetActionsWithChange(operand.Id, constId);
            var incActions = GetActionsWithChange(operand.Id, incId);
            var decActions = GetActionsWithChange(operand.Id, decId);
            var incDegActions = GetActionsWithChange(operand.Id, incDegId);
            var decDegActions = GetActionsWithChange(operand.Id, decDegId);

            var indexes = new List<Index>();

            // CONST FIZVEL CONST_NO_LIMIT
            if (constActions.Count != 0)
            {
                var constQuery = new Index
                {
                    Id = string.Join(Delimiter, IndexIdPrefix, _IndexCounter),
                    Operation = constOperation,
                    Operand = operand,
                    Limit = constNoLimit
                };

                var effectIdsConst =
                    (from action in constActions
                     select action.Idfe).ToList();

                constQuery.EffectIds = string.Join(" ", effectIdsConst);
                indexes.Add(constQuery);
                ++_IndexCounter;
            }

            // CHANGE FIZVEL CHANGE_NO_LIMIT
            if (decActions.Count != 0 || incActions.Count != 0 ||
                incDegActions.Count != 0 || decDegActions.Count != 0)
            {
                var changeQuery = new Index
                {
                    Id = string.Join(Delimiter, IndexIdPrefix, _IndexCounter),
                    Operation = changeOperation,
                    Operand = operand,
                    Limit = changeNoLimit
                };

                var effectIdsDec =
                    (from action in decActions
                     select action.Idfe).ToList();

                var effectIdsInc =
                    (from action in incActions
                     select action.Idfe).ToList();

                var effectIdsDecDeg =
                    (from action in decDegActions
                     select action.Idfe).ToList();

                var effectIdsIncDeg =
                    (from action in incDegActions
                     select action.Idfe).ToList();

                var allEffectsIds = new List<int>();
                allEffectsIds.AddRange(effectIdsDec);
                allEffectsIds.AddRange(effectIdsInc);
                allEffectsIds.AddRange(effectIdsDecDeg);
                allEffectsIds.AddRange(effectIdsIncDeg);

                changeQuery.EffectIds = string.Join(" ", allEffectsIds);
                indexes.Add(changeQuery);
                ++_IndexCounter;
            }

            // CHANGE FIZVEL CHANGE_DEG
            if (incDegActions.Count != 0 || decDegActions.Count != 0)
            {
                var changeQuery = new Index
                {
                    Id = string.Join(Delimiter, IndexIdPrefix, _IndexCounter),
                    Operation = changeOperation,
                    Operand = operand,
                    Limit = changeDegLimit
                };

                var effectIdsDecDeg =
                    (from action in decDegActions
                     select action.Idfe).ToList();

                var effectIdsIncDeg =
                    (from action in incDegActions
                     select action.Idfe).ToList();

                var allEffectsIds = new List<int>();
                allEffectsIds.AddRange(effectIdsDecDeg);
                allEffectsIds.AddRange(effectIdsIncDeg);

                changeQuery.EffectIds = string.Join(" ", allEffectsIds);
                indexes.Add(changeQuery);
                ++_IndexCounter;
            }

            // DEC FIZVEL DEC_NO_LIMIT
            if (decActions.Count != 0 || decDegActions.Count != 0)
            {
                var decQuery = new Index
                {
                    Id = string.Join(Delimiter, IndexIdPrefix, _IndexCounter),
                    Operation = decOperation,
                    Operand = operand,
                    Limit = decNoLimit
                };

                var effectIdsDec =
                    (from action in decActions
                     select action.Idfe).ToList();

                var effectIdsDecDeg =
                    (from action in decDegActions
                     select action.Idfe).ToList();

                var allEffectsIds = new List<int>();
                allEffectsIds.AddRange(effectIdsDec);
                allEffectsIds.AddRange(effectIdsDecDeg);

                decQuery.EffectIds = string.Join(" ", allEffectsIds);
                indexes.Add(decQuery);
                ++_IndexCounter;
            }

            // DEC FIZVEL DEC_DEG
            if (decDegActions.Count != 0)
            {
                var decQuery = new Index
                {
                    Id = string.Join(Delimiter, IndexIdPrefix, _IndexCounter),
                    Operation = decOperation,
                    Operand = operand,
                    Limit = decDegLimit
                };

                var effectIdsDecDeg =
                    (from action in decDegActions
                     select action.Idfe).ToList();

                decQuery.EffectIds = string.Join(" ", effectIdsDecDeg);
                indexes.Add(decQuery);
                ++_IndexCounter;
            }

            // INC FIZVEL INC_NO_LINIT
            if (incActions.Count != 0 || incDegActions.Count != 0)
            {
                var incQuery = new Index
                {
                    Id = string.Join(Delimiter, IndexIdPrefix, _IndexCounter),
                    Operation = incOperation,
                    Operand = operand,
                    Limit = incNoLimit
                };

                var effectIdsInc =
                    (from action in incActions
                     select action.Idfe).ToList();

                var effectIdsIncDeg =
                    (from action in decDegActions
                     select action.Idfe).ToList();

                var allEffectsIds = new List<int>();
                allEffectsIds.AddRange(effectIdsInc);
                allEffectsIds.AddRange(effectIdsIncDeg);

                incQuery.EffectIds = string.Join(" ", allEffectsIds);
                indexes.Add(incQuery);
                ++_IndexCounter;
            }

            // INC FIZVEL INC_DEG
            if (incDegActions.Count != 0)
            {
                var incQuery = new Index()
                {
                    Id = string.Join(Delimiter, IndexIdPrefix, _IndexCounter),
                    Operation = incOperation,
                    Operand = operand,
                    Limit = incDegLimit
                };

                var effectIdsIncDeg =
                    (from action in decDegActions
                     select action.Idfe).ToList();

                incQuery.EffectIds = string.Join(" ", effectIdsIncDeg);
                indexes.Add(incQuery);
                ++_IndexCounter;
            }

            return indexes;
        }

        // GET: FuncIndexes
       
        public ActionResult Index()
        {
            var indexEntities = db.Indexs;
            var operandEntities = db.Operands;

            var allIndexEntities =
                (from index in indexEntities
                 select index).ToList();

            indexEntities.RemoveRange(allIndexEntities);
            db.SaveChanges();

            var newIndexEntities = new List<Index>();

            foreach (var operand in operandEntities)
                newIndexEntities.AddRange(CombineQueries(operand));

            indexEntities.AddRange(newIndexEntities);
            db.SaveChanges();

            return View(db.Indexs.Include(f => f.Limit).Include(f => f.Operand).Include(f => f.Operation).ToList());
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