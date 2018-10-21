//using dip.Models;
using dip.Models;
using dip.Models.Domain;
using PagedList;
//using PhysicalEffectsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    public class DescriptionQueriesController : Controller
    {
        //private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        //private readonly ApplicationDbContext PhysicalEffectsDb = new ApplicationDbContext();
        

        private const string NotFoundFuncId = "FUNC_NOT_FOUND";
        private const string NotFoundErrorMessage = "Физических эффектов по такому запросy не найдено";

        // GET: DecriptionQueries/Create
        [RequireHttps]
        public ActionResult Create()
        {
            
            var operationEntities = db.Operations;
            var operandGroupEntities = db.OperandGroups;
            var operandEntities = db.Operands;
            var limitEntities = db.Limits;

            var allOperations = new SelectList(operationEntities.OrderBy(operation => operation.Value), "Id", "Value");
            var operationId = allOperations.First().Value;

            var compatibleLimits =
                new SelectList(
                    limitEntities.Where(limit => limit.Id.Contains(operationId)).OrderBy(limit => limit.Value), "Id",
                    "Value");

            var allOperandGroups = new SelectList(operandGroupEntities, "Id", "Value");
            var operandGroupId = allOperandGroups.First().Value;

            var compatibleOperands =
                new SelectList(
                    operandEntities.Where(operand => operand.Id.Contains(operandGroupId))
                        .OrderBy(operand => operand.Value), "Id", "Value");

            ViewBag.Operations = allOperations;
            ViewBag.Limits = compatibleLimits;
            ViewBag.OperandGroups = allOperandGroups;
            ViewBag.Operands = compatibleOperands;

            ViewBag.OperationId = operationId;
            ViewBag.OperandGroupId = operandGroupId;

            return View();
        }

        [RequireHttps]
        public ActionResult GetLimits(string id)
        {
            var limitEntities = db.Limits;
            var compatibleLimits =
                new SelectList(limitEntities.Where(limit => limit.Id.Contains(id)).OrderBy(limit => limit.Value), "Id",
                    "Value");

            ViewBag.Limits = compatibleLimits;
            ViewBag.OperationId = id;

            return PartialView();
        }

        [RequireHttps]
        public ActionResult GetOperands(string id)
        {
            var operandEntities = db.Operands;
            var compatibleOperands =
                new SelectList(
                    operandEntities.Where(operand => operand.Id.Contains(id)).OrderBy(operand => operand.Value), "Id",
                    "Value");

            ViewBag.Operands = compatibleOperands;
            ViewBag.OperandGroupId = id;

            return PartialView();
        }

        // POST: DecriptionQueries/Create
        [RequireHttps]
        [HttpPost]
        public ActionResult Create(string operationId, string operandId, string limitId)
        {
            var indexEntities = db.Indexs;

            var technicalFunction =
                (from index in indexEntities
                 where index.OperationId == operationId &&
                       index.OperandId == operandId &&
                       index.LimitId == limitId
                 select index);

            if (technicalFunction.Count() != 0)
                return RedirectToAction("Results", new { id = technicalFunction.First().Id });

            return RedirectToAction("Results", new { id = NotFoundFuncId });
        }

        [RequireHttps]
        public ActionResult Results(string id, int? page)
        {
            var indexEntities = db.Indexs;
            var textEffectEntities = db.FEText;

            var allEffects = new List<FEText>();

            var notFoundErrorMessage = string.Empty;

            if (id == NotFoundFuncId)
                notFoundErrorMessage = NotFoundErrorMessage;
            else
            {
                var effectIds =
                    (from index in indexEntities
                     where index.Id == id
                     select index.EffectIds).First().Split(' ');

                foreach (var effectId in effectIds)
                {
                    var effectIdAsInt = int.Parse(effectId);
                    var effects =
                        (from effect in textEffectEntities
                         where effect.IDFE == effectIdAsInt
                         select effect).ToList();

                    allEffects.AddRange(effects);
                }
            }

            const int pageSize = 15;
            var pageNumber = (page ?? 1);

            ViewBag.TechnicalFunctionId = id;
            ViewBag.Query = string.Join(": ", "Найдено эффектов", allEffects.Count);
            ViewBag.Effects = allEffects;
            ViewBag.ErrorMessage = notFoundErrorMessage;

            return View(allEffects.ToPagedList(pageNumber, pageSize));
        }

        [RequireHttps]
        public ActionResult Details(int id, string technicalFunctionId)
        {
            var textEffectEntities = db.FEText;

            var effect =
                (from textEffect in textEffectEntities
                 where textEffect.IDFE == id
                 select textEffect).First();

            ViewBag.EffectName = effect.Name;
            ViewBag.TechnicalFunctionId = Request.Params.GetValues(0).First();

            return View(effect);
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