

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

//using Mono.Linq.Expressions;
using Binbin.Linq;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using Lucene.Net.Documents;

namespace dip.Models.Domain
{
    [Table("FETexts")]// используется в oldDbContext.cs
    public class FEText
    {
        // [Index("PK_FeTexts_cons", IsClustered = true,IsUnique =true)]//,IsClustered =true
        //index должен быть PK_dbo.FeTexts, если менять то менять и в oldDbContext.cs

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int IDFE { get; set; }

        [Display(Name = "Название")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Название должно быть установлено")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Описание должно быть установлено")]
        public string Text { get; set; }

        [Display(Name = "Входное воздействие")]
        [DataType(DataType.MultilineText)]
        //[Required(ErrorMessage = "Входное воздействие должно быть установлено")]
        public string TextInp { get; set; }

        [Display(Name = "Выходное воздействие")]
        [DataType(DataType.MultilineText)]
        //[Required(ErrorMessage = "Выходное воздействие должно быть установлено")]
        public string TextOut { get; set; }

        [Display(Name = "Объект")]
        [DataType(DataType.MultilineText)]
        //[Required(ErrorMessage = "Объект должн быть установлен")]
        public string TextObj { get; set; }

        [Display(Name = "Применение")]
        [DataType(DataType.MultilineText)]
        //[Required(ErrorMessage = "Применение должно быть установлено")]
        public string TextApp { get; set; }

        [Display(Name = "Литература")]
        [DataType(DataType.MultilineText)]
        //[Required(ErrorMessage = "Литература должна быть установлено")]
        public string TextLit { get; set; }


        
        [ScaffoldColumn(false)]
        public bool NotApprove { get; set; }

        [ScaffoldColumn(false)]
        public bool Deleted { get; set; }


        [ScaffoldColumn(false)]
        public int CountInput { get; set; }//количество входов(дескрипторная часть)

        [ScaffoldColumn(false)]
        public bool ChangedObject { get; set; }//изменяется ли объект(дескрипторная часть)



        [NotMapped]
       // [ScaffoldColumn(false)]//TODO hz
        public bool? FavouritedCurrentUser { get; set; }//зафоловил ли текущий пользователь эту запись

        public string StateBeginId { get; set; }
        public StateObject StateBegin { get; set; }

        public string StateEndId { get; set; }
        public StateObject StateEnd { get; set; }


        public List<Image> Images { get; set; }

        //[NotMapped]
        //public List<ShowsFEImage> AllImages { get; set; }
        

        public List<FELatexFormula> LatexFormulas { get; set; }

        public List<ApplicationUser> FavouritedUser { get; set; }


        public List<ListPhysics> Lists { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public FEText()
        {
            this.Images = new List<Image>();
            LatexFormulas = new List<FELatexFormula>();
            //AllImages = new List<ShowsFEImage>();
            FavouritedUser = new List<ApplicationUser>();
            NotApprove = true;
            FavouritedCurrentUser = null;

            CountInput = 1;
            ChangedObject = false;
            Deleted = false;
            Lists = new List<ListPhysics>();
            Users = new List<ApplicationUser>();
        }

        public bool Equal(FEText a)
        {
            this.Name = a.Name;
            this.Text = a.Text;
            this.TextInp = a.TextInp;
            this.TextOut = a.TextOut;
            this.TextObj = a.TextObj;
            this.TextApp = a.TextApp;
            this.TextLit = a.TextLit;
            this.FavouritedCurrentUser = a.FavouritedCurrentUser;

            this.Deleted = a.Deleted;
            this.CountInput = a.CountInput;
            this.ChangedObject = a.ChangedObject;
            this.NotApprove=a.NotApprove;
            this.StateBeginId=a.StateBeginId;
            this.StateEndId=a.StateEndId;
            

            return true;
        }

        public bool EqualWithId(FEText a)
        {
            this.Equal(a);
            this.IDFE = a.IDFE;

            return true;
        }

        public void ChangeForMap()
        {
            //TODO pattern?
            Text = Lucene_.ChangeForMap(Text);
            //Name = Lucene_.ChangeForMap(Name);
            //TextApp = Lucene_.ChangeForMap(TextApp);
            //TextInp = Lucene_.ChangeForMap(TextInp);
            //TextLit = Lucene_.ChangeForMap(TextLit);
            //TextObj = Lucene_.ChangeForMap(TextObj);
            //TextOut = Lucene_.ChangeForMap(TextOut);


        }


        public static List<string> GetPropTextSearch()//TODO
        {
            var res = new List<string>();
            //var listNM = new List<string>() { "IDFE", "CountInput", "ChangedObject", "NotApprove", "FavouritedCurrentUser", "Images", "FavouritedUser", "Deleted" };//исключаем
            //PropertyInfo[] myPropertyInfo;
            //Type myType = typeof(FEText);
            //// Get the type and fields of FieldInfoClass.
            //myPropertyInfo = myType.GetProperties();
            //res = myPropertyInfo.Where(x1 => listNM.FirstOrDefault(x2 => x2 == x1.Name) == null).Select(x1 => x1.Name).ToList();
            res.Add("Text");
            return res;
        }

        public Document GetDocumentForLucene()
        {
            var document = new Document();
            // document.Add(new NumericField("IDFE", Field.Store.YES, true).SetIntValue(obj.IDFE));
            document.Add(new Field("IDFE", this.IDFE.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field("Text", this.Text, Field.Store.YES, Field.Index.ANALYZED));
            return document;
        }


        public static FEText Get(int? id)
        {
            FEText res = null;
            if (id != null)//&&id>0
                using (var db = new ApplicationDbContext())
                    res = db.FEText.FirstOrDefault(x1 => x1.IDFE == id);
            return res;

        }

        public static FEText GetIfAccess(int? id, HttpContextBase HttpContext)
        {
            FEText res = null;
            if (id == null)
                return res;
            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            if (user == null)
                return res;
            List<int> accessList = user.CheckAccessPhys(new List<int>() { (int)id }, HttpContext);
            if (accessList.Contains((int)id))
                res = FEText.Get(id);
            return res;

        }


        //при более чем 2х входах и 1 выходах-(||forms>3) эту функцию необходимо будет изменить
        //[Authorize]
        public static int[] GetByDescr(string stateBegin, string stateEnd, DescrSearchI[] forms, DescrObjectI[]objects, HttpContextBase HttpContext)
        {
            int[] list_id = null;


            //поиск
            //List<int> list_id = new List<int>();
            foreach (var i in forms)
                i.DeleteNotChildCheckbox();
            foreach (var i in objects)
                i.DeleteNotChildCheckbox();
          

            List<FEAction> formsList = new List<FEAction>();
            List<FEObject> objectsList = new List<FEObject>();
            //System.Collections.Generic.List<System.Linq.IGrouping<int, dip.Models.Domain.FEAction>> checkInp = null;
            List<int> checkInp = new List<int>();

            using (var db = new ApplicationDbContext())
            {
                //IQueryable<dip.Models.Domain.FEAction> forms_query = db.FEActions;

                //
                //TODO временно
                //Mono.Linq.Expressions.
                var predicate = PredicateBuilder.False<FEAction>();
               


                foreach (var inp in forms)
                {
                    int beg = (inp.InputForm ? 1 : 0);

                    predicate = predicate.Or(x1 => x1.Input == beg &&
                 x1.Name == inp.ActionId &&
                   x1.Type == inp.ActionType &&
                   x1.FizVelId == inp.FizVelId &&
                   x1.Pros == inp.ListSelectedPros &&
                   x1.Spec == inp.ListSelectedSpec &&
                   x1.Vrem == inp.ListSelectedVrem &&
                   x1.FizVelSection == inp.ParametricFizVelId);


                }
                //formsList = forms_query.ToList();
                int formsLen = forms.Length;
                checkInp =db.FEActions.Where(predicate).GroupBy(x1 => x1.Idfe).Where(x1 => x1.Count() == formsLen).Select(x1=>x1.Key).ToList();
            }
            //.ToList() .ToArray()
            
            
            if (checkInp.Count < 1)
                return null;

            //если уже на этом этапе ничего не найдено дальше не искать
            List<int> checkObj = new List<int>();
            using (var db = new ApplicationDbContext())
            {
                var predicate = PredicateBuilder.False<FEObject>();
                //IQueryable<dip.Models.Domain.FEObject> objects_query = db.FEObjects;
                foreach (var obj in objects)
                {
                    int beg = (obj.Begin?1:0);

                    if (obj.ListSelectedPhase1 != null)//TODO вынести  все фазы в метод и в цикл по фазам??????
                        predicate = predicate.Or(x1 => x1.Begin == beg &&
                    x1.NumPhase == 1 &&
                    x1.Composition == obj.ListSelectedPhase1.Composition &&
                    x1.Conductivity == obj.ListSelectedPhase1.Conductivity &&
                    x1.MagneticStructure == obj.ListSelectedPhase1.MagneticStructure &&
                    x1.MechanicalState == obj.ListSelectedPhase1.MechanicalState &&
                    x1.OpticalState == obj.ListSelectedPhase1.OpticalState &&
                    x1.PhaseState == obj.ListSelectedPhase1.PhaseState &&
                    x1.Special == obj.ListSelectedPhase1.Special);



                    if (obj.ListSelectedPhase2 != null)
                        predicate = predicate.Or(x1 => x1.Begin == beg &&
                         x1.NumPhase == 2 &&
                        x1.Composition == obj.ListSelectedPhase2.Composition &&
                   x1.Conductivity == obj.ListSelectedPhase2.Conductivity &&
                   x1.MagneticStructure == obj.ListSelectedPhase2.MagneticStructure &&
                   x1.MechanicalState == obj.ListSelectedPhase2.MechanicalState &&
                   x1.OpticalState == obj.ListSelectedPhase2.OpticalState &&
                   x1.PhaseState == obj.ListSelectedPhase2.PhaseState &&
                   x1.Special == obj.ListSelectedPhase2.Special);


                    if (obj.ListSelectedPhase3 != null)
                        predicate = predicate.Or(x1 => x1.Begin == beg &&
                         x1.NumPhase == 3 &&
                        x1.Composition == obj.ListSelectedPhase3.Composition &&
                       x1.Conductivity == obj.ListSelectedPhase3.Conductivity &&
                       x1.MagneticStructure == obj.ListSelectedPhase3.MagneticStructure &&
                       x1.MechanicalState == obj.ListSelectedPhase3.MechanicalState &&
                       x1.OpticalState == obj.ListSelectedPhase3.OpticalState &&
                       x1.PhaseState == obj.ListSelectedPhase3.PhaseState &&
                       x1.Special == obj.ListSelectedPhase3.Special);



                }
                int AllCountPhase = 0;
                foreach (var i in objects)//TODO в метод #[]
                {
                    if (i.ListSelectedPhase1 != null)
                        AllCountPhase++;
                    if (i.ListSelectedPhase2 != null)
                        AllCountPhase++;
                    if (i.ListSelectedPhase3 != null)
                        AllCountPhase++;
                }

                 checkObj = db.FEObjects.Where(predicate).GroupBy(x1 => x1.Idfe).Where(x1 => x1.Count() == AllCountPhase).Select(x1 => x1.Key).ToList();
            }


            if (checkObj.Count == 0)
                return null;

            //x1.Key x2.Key

            //сравниваем состояния и результаты всех запросов
            list_id = FEText.GetList(null,checkObj.Join(checkInp, x1 => x1, x2 => x2, (x1, x2) => x1).ToArray())
                .Where(x1 => x1.StateBeginId == stateBegin && x1.StateEndId == stateEnd).Select(x1=>x1.IDFE).ToArray();



            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            if (user == null)
                return null;
            list_id=user.CheckAccessPhys(list_id.ToList(), HttpContext).ToArray();

            return list_id;
        }

        //алгоритм левинштайна
        //public static int[] GetByText(string text)
        //{
        //    using (var db = new ApplicationDbContext())
        //    {
        //        System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@searched_str", "Затухание");
        //        System.Data.SqlClient.SqlParameter param2 = new System.Data.SqlClient.SqlParameter("@max_lev", 5);
        //        var lst = db.Database.SqlQuery<test>("SELECT * FROM GetListLev (@searched_str,@max_lev)", param1, param2).ToList();


        //    }

        //    return null;
        //}


        public void LoadImage()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Set<FEText>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.Images).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Images).Load();
            }

        }

        public void LoadLatex()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Set<FEText>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.LatexFormulas).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.LatexFormulas).Load();
            }

        }

        public void AddByteToLatexImages()
        {
            this.LoadLatex();
            //this.LatexFormulas.AddRange(this.LatexFormulas.Select(x1=>Image.GetFromLatex(x1.Data)));
            foreach (var i in this.LatexFormulas)
                i.SetBytes();
        }


        


        public void LoadLists()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Set<FEText>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.Lists).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Lists).Load();
            }

        }


        public static List<int> GetListSimilar(int id, HttpContextBase HttpContext, int count = -1)
        {
            List<int> res = new List<int>();

            //этот запрос может возвращать дублирующиеся id тк сравниват не записи в целом а отдельно столбцы
            //            var quer=$@"select top({count})IDFE
            //from
            //       semanticsimilaritytable (FETexts, *, {id} ) data
            //            inner join dbo.FETexts
            //            txt on data.matched_document_key = txt.IDFE
            //order by data.score desc";

//            var quer = $@"
//select   [txt].IDFE
//from
//       semanticsimilaritytable (FETexts, *, {id}  ) as data
//            inner join dbo.FETexts as
//            txt on data.matched_document_key = [txt].IDFE
//order by [data].score desc
//";



            var quer = $@"
select   txt.IDFE
from
       semanticsimilaritytable (FETexts, *, {id}  ) as data
            inner join dbo.FETexts as
            txt on data.matched_document_key = txt.IDFE
order by data.score desc";



            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            if (user == null)
                return null;
            


            var dict = DataBase.DataBase.ExecuteQuery(quer, null, "IDFE");
            var accessList = user.CheckAccessPhys(dict.Select(x1 => Convert.ToInt32(x1["IDFE"])).Distinct().ToList(), HttpContext);
            if(count>=0)
            res.AddRange(accessList.Take(count));
            else
                res.AddRange(accessList);



            return res;
        }



        public static List<FEText> GetList(string userId,params int[] id)
        {
            var res = new List<FEText>();
            if (id != null&& id.Length>0)
                using (var db = new ApplicationDbContext())
                {
                    res = db.FEText.Join(id, x1 => x1.IDFE, x2 => x2, (x1, x2) => x1).ToList();
                    userId= userId?? ApplicationUser.GetUserId();
                    foreach(var i in res)
                    {
                        i.FavouritedCurrentUser = i.Favourited(userId);
                    }
                }
            
                    
            return res;
        }

        public static List<FEText> GetListIfAccess(HttpContextBase HttpContext,params int[] id)
        {
            var res = new List<FEText>();

            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            if (user == null)
                return res;
            List<int>accessList = user.CheckAccessPhys(id?.ToList(), HttpContext);

            res = FEText.GetList(user.Id, accessList.ToArray());
          
            return res;
        }




        public bool Validation()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Text) ||
                string.IsNullOrWhiteSpace(TextInp) ||
                string.IsNullOrWhiteSpace(TextOut) ||
                string.IsNullOrWhiteSpace(TextObj) ||
                string.IsNullOrWhiteSpace(TextApp) ||
                string.IsNullOrWhiteSpace(TextLit))
                return false;


            using (var db=new ApplicationDbContext())
            {
                var states = db.StateObjects.Where(x1 => x1.Id == this.StateBeginId || x1.Id == this.StateEndId).ToList();
                foreach (var i in states)
                    if (i.CountPhase == null)
                        return false;
            }


                return true;
        }

        public bool AddToDb(DescrSearchI[] forms, DescrObjectI[] objForms, List<byte[]> addImgs = null, string[] latexformulas = null)
        {
            
            bool commited = false;


            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.FEText.Add(this);
                        db.SaveChanges();

                        foreach (var i in forms)
                        {
                            var act = new FEAction() { Idfe = this.IDFE };//, Input = (i.InputForm ? 1 : 0)
                            act.SetFromInput(i);
                            db.FEActions.Add(act);
                            db.SaveChanges();
                        }
                        foreach (var i in objForms)
                        {
                            List<FEObject> objects = new List<FEObject>();
                            var phmass = i.GetActualPhases();//
                            foreach (var phit in phmass)
                            {
                                objects.Add(new FEObject(phit, this.IDFE, (i.Begin ? 1 : 0)));
                            }

                            db.FEObjects.AddRange(objects);
                            db.SaveChanges();
                        }


                        this.AddImages(addImgs, db);

                        db.SaveChanges();

                        if (latexformulas != null && latexformulas.Length > 0)
                        {


                            db.FELatexFormulas.AddRange(latexformulas.Select(x1 => new FELatexFormula() { Formula = x1, FeTextIDFE = this.IDFE }));
                            db.SaveChanges();
                        }


                        transaction.Commit();
                        commited = true;
                        //DescrObjectI[] objForms = ; //TODO-objForms
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            if (commited)
                Lucene_.BuildIndexSolo(this);
            return commited;
        }

        public bool ChangeDb(FEText newObj, List<int> deleteImg = null, List<byte[]> addImgs = null, List<DescrSearchI> forms = null, 
            List<DescrObjectI> objForms = null,ChangeLatex[] latexformulas=null)
        {
            bool commited = false;
            bool chengedText = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                      
                        db.Set<FEText>().Attach(this);
                        if (this.Text != newObj.Text)
                            chengedText = true;
                        this.Equal(newObj);
                        if (deleteImg != null && deleteImg.Count > 0)
                        {
                            var imgs = db.Images.Where(x1 => x1.FeTextIDFE == this.IDFE).
                                                    Join(deleteImg, x1 => x1.Id, x2 => x2, (x1, x2) => x1).ToList();//Where(x1 => x1.FeTextId == obj.IDFE);
                            db.Images.RemoveRange(imgs);
                            db.SaveChanges();
                        }
                        this.AddImages(addImgs, db);


                        var descrdb = db.FEActions.Where(x1 => x1.Idfe == this.IDFE);//TODO обработка ошибок(восстановление при неудаче)
                        db.FEActions.RemoveRange(descrdb);//без сохранения


                        foreach (var i in forms)
                        {
                            var act = new FEAction() { Idfe = this.IDFE };//,Input=(i.InputForm?1:0)
                            act.SetFromInput(i);
                            db.FEActions.Add(act);
                        }


                        db.SaveChanges();


                        var objdb = db.FEObjects.Where(x1 => x1.Idfe == this.IDFE);//TODO обработка ошибок(восстановление при неудаче)
                        db.FEObjects.RemoveRange(objdb);//без сохранения
                        foreach (var i in objForms)
                        {
                           
                            List<FEObject> objects = new List<FEObject>();
                            var phmass = i.GetActualPhases();//
                            foreach (var phit in phmass)
                            {
                                objects.Add(new FEObject(phit, this.IDFE, (i.Begin ? 1 : 0)));
                            }
                            db.FEObjects.AddRange(objects);
                            db.SaveChanges();
                        }
                        db.SaveChanges();


                        //latex
                        if (latexformulas != null&& latexformulas.Length>0)
                        {
                            //add
                            var ladd = latexformulas.Where(x1=>x1.Action==0);
                            
                            db.FELatexFormulas.AddRange(ladd.Select(x1 => new FELatexFormula() { Formula = x1.Text??"", FeTextIDFE = this.IDFE }));
                            db.SaveChanges();

                            //change
                            var lchange = latexformulas.Where(x1 => x1.Action == 1);
                            var lchangeid = lchange.Select(x1=>x1.Id);
                             var oldchange=db.FELatexFormulas.Where(x1 => lchangeid.FirstOrDefault(x2 => x2 == x1.Id) != 0 && x1.FeTextIDFE == this.IDFE).ToList();
                            foreach(var i in oldchange)
                            {
                                var tmp=lchange.FirstOrDefault(x1=>x1.Id==i.Id);
                                if (tmp != null&& i.Formula != tmp.Text)
                                    i.Formula = tmp.Text??"";
                            }
                            db.SaveChanges();

                            //delete
                            var ldel = latexformulas.Where(x1 => x1.Action == 2).Select(x1=>x1.Id).ToList();
                            var delst = db.FELatexFormulas.Where(x1 => x1.FeTextIDFE == this.IDFE&&ldel.Contains(x1.Id) );
                            db.FELatexFormulas.RemoveRange(delst);
                            db.SaveChanges();

                        }
                        
                        //if (latexformulasDel != null && latexformulasDel.Length > 0)
                        //{
                        //   // var lstdel = latexformulasDel;//.Select(x1=>x1)
                        //    var delst = db.FELatexFormulas.Where(x1=> latexformulasDel.Contains(x1.Id)&&x1.FeTextIDFE==this.IDFE);
                        //    db.FELatexFormulas.RemoveRange(delst);
                        //    db.SaveChanges();
                        //}
                        if(chengedText)
                            Lucene_.UpdateDocument(this.IDFE.ToString(), this);

                        transaction.Commit();
                        commited = true;
                        // DescrObjectI[] objForms = ; //TODO-objForms
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            return commited;
        }




        //не загружает картинки, только добавляет в бд
        public void AddImages(List<byte[]> imgs)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //db.Set<FEText>().Attach(this);
                //this.
                this.AddImages(imgs, db);
            }
        }
        public void AddImages(List<byte[]> imgs, ApplicationDbContext db)
        {
            foreach (var i in imgs)
            {
                db.Images.Add(new Image() { Data = i, FeTextIDFE = this.IDFE });//FeTextId = oldObj.IDFE    //  FeText= oldObj
            }
            db.SaveChanges();
        }


        //res-true- теперь добавлено , без проверки на null
        /// <summary>
        /// меняет(добавляет\удаляет) запись в избранном, если к ней есть доступ
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="HttpContext"></param>
        /// <returns></returns>
        public bool? ChangeFavourite(string personId, HttpContextBase HttpContext)
        {
            bool? res = null;

            ApplicationUser user = ApplicationUser.GetUser(personId);
            if (user == null)
                return res;
            List<int> accessList = user.CheckAccessPhys(new List<int>() { this.IDFE }, HttpContext);

            if (accessList.Contains(this.IDFE))
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    db.Set<FEText>().Attach(this);
                    var userFav = db.Entry(this).Collection(x1 => x1.FavouritedUser).Query().FirstOrDefault(x1 => x1.Id == personId);

                    if (userFav != null)
                    {
                        db.Entry(this).Collection(x1 => x1.FavouritedUser).Load();

                        this.FavouritedUser.Remove(userFav);
                        res = false;
                    }
                    else
                    {

                        db.Set<ApplicationUser>().Attach(user);
                        this.FavouritedUser.Add(user);
                        res = true;
                    }
                    db.SaveChanges();
                }



            return res;
        }
        public bool Favourited(string personId)
        {
            bool res = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                res=this.Favourited(personId,db);


            }
            
            return res;
        }
        /// <summary>
        /// проверка на то добавил ли пользователь в избранное этот ФЭ
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Favourited(string personId, ApplicationDbContext db)
        {
            bool res = false;

            db.Set<FEText>().Attach(this);
            var userFav = db.Entry(this).Collection(x1 => x1.FavouritedUser).Query().FirstOrDefault(x1 => x1.Id == personId);

            if (userFav != null)
            {
                res = true;
            }
            
            return res;
        }


        //public static FEText Get(int id)
        //{
        //    FEText res = null;
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        res=db.FEText.FirstOrDefault(x1=>x1.IDFE==id);
        //    }

        //    return res;
        //}

        public static FEText GetNextAccessPhysic(int id, HttpContextBase HttpContext)
        {
            //FEText res = null;
            ApplicationUser user = ApplicationUser.GetUser( ApplicationUser.GetUserId());
            return user.GetNextAccessPhysic(id, HttpContext);
            //IList<string> roles = HttpContext.GetOwinContext()
            //                             .GetUserManager<ApplicationUserManager>()?.GetRoles(user.Id);
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    //DbSet<FEText> collect = null;
            //if (roles.Contains(RolesProject.admin.ToString())|| roles.Contains(RolesProject.subscriber.ToString()))
            //    {
            //        //collect = db.FEText;
            //        res = db.FEText.FirstOrDefault(x1 => x1.IDFE > id);
            //    }

            //else if (roles.Contains(RolesProject.user.ToString())){
            //        db.Set<ApplicationUser>().Attach(user);
            //        user.LoadPhysics();
            //        collect = user.Physics;
            //    }


            //    res = db.FEText.FirstOrDefault(x1 => x1.IDFE > id);
            //    if(res==null)
            //        res = db.FEText.FirstOrDefault();
            //}

            //return res;
        }

        public static FEText GetPrevAccessPhysic(int id, HttpContextBase HttpContext)
        {
            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            return user.GetPrevAccessPhysic(id, HttpContext);
            //FEText res = null;
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    //res = db.FEText.LastOrDefault(x1 => x1.IDFE < id);
            //    res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault(x1 => x1.IDFE < id);
            //    if (res == null)
            //        res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault();

            //}

            //return res;
        }
        public static FEText GetFirstAccessPhysic( HttpContextBase HttpContext)
        {
            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            return user.GetFirstAccessPhysic( HttpContext);
            //FEText res = null;
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    //res = db.FEText.LastOrDefault(x1 => x1.IDFE < id);
            //    res = db.FEText.FirstOrDefault();

            //}

            //return res;
        }
        public static FEText GetLastAccessPhysic( HttpContextBase HttpContext)
        {
            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            return user.GetLastAccessPhysic(HttpContext);
            //FEText res = null;
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    //res = db.FEText.LastOrDefault(x1 => x1.IDFE < id);
            //    res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault();

            //}

            //return res;
        }

        public static void ReloadSemanticRecord()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var fe=db.FEText.FirstOrDefault(x1=>x1.IDFE==Constants.FEIDFORSEMANTICSEARCH);
                fe.Text = Constants.FeSemanticNullText;
                db.SaveChanges();

            }
        }

    }
}