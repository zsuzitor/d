//using dip.Models;
using dip.Models;
using dip.Models.Domain;
using dip.Models.TechnicalFunctions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
//using PhysicalEffectsSearchEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    //[RequireHttps]
    public class DescriptionQueriesController : Controller
    {
        //private readonly TechnicalFunctionsEntities _TechnicalFunctionsDb = new TechnicalFunctionsEntities();
        //private readonly ApplicationDbContext db = new ApplicationDbContext();
        //private readonly ApplicationDbContext PhysicalEffectsDb = new ApplicationDbContext();


        private const string NotFoundFuncId = "FUNC_NOT_FOUND";
        private const string NotFoundErrorMessage = "Физических эффектов по такому запросy не найдено";

        // GET: DecriptionQueries/Create

        public ActionResult Create()
        {
            List<Operation> operationEntities = new List<Models.TechnicalFunctions.Operation>();
            List<Limit> limitEntities = new List<Models.TechnicalFunctions.Limit>();
            List<Operand> operandEntities = new List<Models.TechnicalFunctions.Operand>();
            List<OperandGroup> operandGroupEntities = new List<Models.TechnicalFunctions.OperandGroup>();

            SelectList allOperations;
            SelectList compatibleLimits;
            SelectList allOperandGroups;
            SelectList compatibleOperands;

            string operandGroupId;
            string operationId;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                operationEntities = db.Operations.OrderBy(operation => operation.Value).ToList();
                operandGroupEntities = db.OperandGroups.ToList();
                allOperandGroups = new SelectList(operandGroupEntities, "Id", "Value");
                operandGroupId = allOperandGroups.First().Value;
                operandEntities = db.Operands.Where(operand => operand.Id.Contains(operandGroupId))
                        .OrderBy(operand => operand.Value).ToList();
                allOperations = new SelectList(operationEntities, "Id", "Value");
                operationId = allOperations.First().Value;
                limitEntities = db.Limits.Where(limit => limit.Id.Contains(operationId)).OrderBy(limit => limit.Value).ToList();



            }
            compatibleLimits =
               new SelectList(
                   limitEntities, "Id",
                   "Value");



            compatibleOperands =
               new SelectList(
                   operandEntities, "Id", "Value");
            ViewBag.Operations = allOperations;
            ViewBag.Limits = compatibleLimits;
            ViewBag.OperandGroups = allOperandGroups;
            ViewBag.Operands = compatibleOperands;

            ViewBag.OperationId = operationId;
            ViewBag.OperandGroupId = operandGroupId;

            return View();
        }


        public ActionResult GetLimits(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var limitEntities = db.Limits.Where(limit => limit.Id.Contains(id)).OrderBy(limit => limit.Value);
                var compatibleLimits =
                    new SelectList(limitEntities, "Id",
                        "Value");

                ViewBag.Limits = compatibleLimits;
                ViewBag.OperationId = id;
            }
            return PartialView();
        }


        public ActionResult GetOperands(string id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var operandEntities = db.Operands.Where(operand => operand.Id.Contains(id)).OrderBy(operand => operand.Value).ToList();
                var compatibleOperands =
                    new SelectList(
                        operandEntities, "Id",
                        "Value");

                ViewBag.Operands = compatibleOperands;
                ViewBag.OperandGroupId = id;
            }
            return PartialView();
        }

        // POST: DecriptionQueries/Create

        [HttpPost]
        public ActionResult Create(string operationId, string operandId, string limitId)
        {
            List<Index> technicalFunction = new List<Index>();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {


                technicalFunction =
                   (from index in db.Indexs
                    where index.OperationId == operationId &&
                          index.OperandId == operandId &&
                          index.LimitId == limitId
                    select index).ToList();
            }
            string resId = NotFoundFuncId;
            if (technicalFunction.Count() != 0)
                resId= technicalFunction.First().Id ;

            return RedirectToAction("Results", new { id = resId });
        }


        public ActionResult Results(string id, int? page)
        {
            var pageNumber = (page ?? 1);
            const int pageSize = 15;
            List<FEText> allEffects = new List<FEText>();

            var notFoundErrorMessage = string.Empty;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                //allEffects = new List<FEText>();



                if (id == NotFoundFuncId)
                    notFoundErrorMessage = NotFoundErrorMessage;
                else
                {
                    var effectIds =
                        (from index in db.Indexs
                         where index.Id == id
                         select index.EffectIds).First().Split(' ');

                    foreach (var effectId in effectIds)
                    {
                        var effectIdAsInt = int.Parse(effectId);
                        var effects =
                            (from effect in db.FEText
                             where effect.IDFE == effectIdAsInt
                             select effect).ToList();

                        allEffects.AddRange(effects);
                    }
                }

            }

            ViewBag.TechnicalFunctionId = id;
            ViewBag.Query = string.Join(": ", "Найдено эффектов", allEffects.Count);
            ViewBag.Effects = allEffects;
            ViewBag.ErrorMessage = notFoundErrorMessage;

            return View(allEffects.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Details(int id)//, string technicalFunctionId
        {
            FEText effect;
            string check_id = ApplicationUser.GetUserId();
            

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
               

                 effect =
                    (from textEffect in db.FEText
                     where textEffect.IDFE == id
                     select textEffect).First();

                ViewBag.EffectName = effect.Name;
                ViewBag.TechnicalFunctionId = Request.Params.GetValues(0).First();

                db.Entry(effect).Collection(x1=>x1.Images).Load();
                //var g = db.Images.ToList();

            }

            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                            .GetUserManager<ApplicationUserManager>();
            IList<string> roles = userManager.GetRoles(check_id);

            if (roles.Contains("admin"))
                ViewBag.Admin = true;


            return View(effect);
            }

        
    }
    }
