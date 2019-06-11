/*файл класса модели БД предназначенного для хранения записи ФЭ, добавлены методы для взаимодействия с сущностью
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

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
using System.Linq.Expressions;

namespace dip.Models.Domain
{

    /// <summary>
    /// запись ФЭ
    /// </summary>
    [Table("FETexts")]// используется в oldDbContext.cs
    public class FEText
    {
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
        // [ScaffoldColumn(false)]
        public bool? FavouritedCurrentUser { get; set; }//зафоловил ли текущий пользователь эту запись

        public string StateBeginId { get; set; }
        public StateObject StateBegin { get; set; }

        public string StateEndId { get; set; }
        public StateObject StateEnd { get; set; }


        public List<Image> Images { get; set; }
        public List<FELatexFormula> LatexFormulas { get; set; }
        public List<ApplicationUser> FavouritedUser { get; set; }
        public List<ListPhysics> Lists { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public FEText()
        {
            this.Images = new List<Image>();
            LatexFormulas = new List<FELatexFormula>();
            FavouritedUser = new List<ApplicationUser>();
            NotApprove = true;
            FavouritedCurrentUser = null;

            CountInput = 1;
            ChangedObject = false;
            Deleted = false;
            Lists = new List<ListPhysics>();
            Users = new List<ApplicationUser>();
        }
        /// <summary>
        /// Метод эквивалент "=" для datatype, но не включает id
        /// </summary>
        /// <param name="a">объект с новыми свойствами</param>
        /// <returns>флаг успеха</returns>
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
            this.NotApprove = a.NotApprove;
            this.StateBeginId = a.StateBeginId;
            this.StateEndId = a.StateEndId;

            return true;
        }

        /// <summary>
        /// Метод эквивалент "=" для datatype,  включает id
        /// </summary>
        /// <param name="a">объект с новыми свойствами</param>
        /// <returns>флаг равенства</returns>
        public bool EqualWithId(FEText a)
        {
            this.Equal(a);
            this.IDFE = a.IDFE;

            return true;
        }


        /// <summary>
        /// Метод для подготовки объекта к использованию с lucene 
        /// </summary>
        public void ChangeForMap()
        {
            Text = Lucene_.ChangeForMap(Text);
        }


        /// <summary>
        /// Метод возвращает список свойст по которым нужно искать при полнотекстовом поиске lucene
        /// </summary>
        /// <returns>список свойств</returns>
        public static List<string> GetPropTextSearch()//TODO
        {
            var res = new List<string>();

            res.Add("Text");
            return res;
        }

        /// <summary>
        /// получение Document для lucene - для добавления в индекс
        /// </summary>
        /// <returns>объект документа</returns>
        public Document GetDocumentForLucene()
        {
            var document = new Document();
            document.Add(new Field("IDFE", this.IDFE.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            document.Add(new Field("Text", this.Text, Field.Store.YES, Field.Index.ANALYZED));
            return document;
        }

        /// <summary>
        /// получение записи по id
        /// </summary>
        /// <param name="id">id записи</param>
        /// <returns>запись FEText найденная по id</returns>
        public static FEText Get(int? id)
        {
            FEText res = null;
            if (id != null)
                using (var db = new ApplicationDbContext())
                    res = db.FEText.FirstOrDefault(x1 => x1.IDFE == id);
            return res;

        }

        /// <summary>
        /// Метод для получения записи фэ если у текущего пользователю есть к нему доступ
        /// </summary>
        /// <param name="id">id фэ</param>
        /// <param name="HttpContext">контекст http</param>
        /// <returns>запись ФЭ</returns>
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



        /// <summary>
        /// Метод для получения записей ФЭ по дескрипторному описанию
        /// //при более чем 2х входах и 1 выходах-(||forms>3) эту функцию необходимо будет изменить
        /// </summary>
        /// <param name="stateBegin">начальное состояние</param>
        /// <param name="stateEnd">конечное состояние</param>
        /// <param name="forms">входные\выходные дескрипторы</param>
        /// <param name="objects">характеристики объекта</param>
        /// <param name="lastId">id последней записи которое нужно пропустить</param>
        /// <param name="HttpContext"></param>
        /// <returns>массив id фэ</returns>
        public static int[] GetByDescr(string stateBegin, string stateEnd, DescrSearchI[] forms, DescrObjectI[] objects, int lastId, HttpContextBase HttpContext)
        {
            int[] list_id = null;
            bool changedObject = objects.Length == 2 ? true : false;
            foreach (var i in forms)
                i.DeleteNotChildCheckbox();
            foreach (var i in objects)
                i.DeleteNotChildCheckbox();


            List<FEAction> formsList = new List<FEAction>();
            List<FEObject> objectsList = new List<FEObject>();
            List<int> checkInp = new List<int>();

            using (var db = new ApplicationDbContext())
            {
                var predicate = PredicateBuilder.False<FEAction>();


                //actionType NO_ACTIONS
                //fizvel NO_FIZVEL
                foreach (var inp in forms)
                {
                    var predicateIns = PredicateBuilder.True<FEAction>();
                    int beg = (inp.InputForm ? 1 : 0);

                    predicateIns = predicateIns.And(x1 => x1.Input == beg);
                    predicateIns = predicateIns.And(x1 => x1.Name == inp.ActionId);

                    if (!string.IsNullOrWhiteSpace(inp.ActionType) && inp.ActionType != "NO_ACTIONS")
                        predicateIns = predicateIns.And(x1 => x1.Type == inp.ActionType);

                    if (!string.IsNullOrWhiteSpace(inp.FizVelId) && inp.FizVelId != "NO_FIZVEL")
                        predicateIns = predicateIns.And(x1 => x1.FizVelId == inp.FizVelId);

                    if (!string.IsNullOrWhiteSpace(inp.ParametricFizVelId) && inp.ParametricFizVelId != "NO_FIZVEL")
                        predicateIns = predicateIns.And(x1 => x1.FizVelSection == inp.ParametricFizVelId);

                    if (!string.IsNullOrWhiteSpace(inp.ListSelectedPros))
                    {
                        string[] tmp = inp.ListSelectedPros.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        var predicateInscheCkbox = PredicateBuilder.True<FEAction>();
                        foreach (var i in tmp)
                        {
                            //predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Pros.Contains(i));
                            predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Pros == i || x1.Pros.StartsWith(i + " ")
                                        || x1.Pros.EndsWith(" " + i) || x1.Pros.Contains(" " + i + " "));

                        }
                        predicateIns = predicateIns.And(predicateInscheCkbox);
                    }

                    if (!string.IsNullOrWhiteSpace(inp.ListSelectedSpec))
                    {
                        string[] tmp = inp.ListSelectedSpec.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        var predicateInscheCkbox = PredicateBuilder.True<FEAction>();
                        foreach (var i in tmp)
                        {
                            //predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Spec.Contains(i));
                            predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Spec == i || x1.Spec.StartsWith(i + " ")
                                        || x1.Spec.EndsWith(" " + i) || x1.Spec.Contains(" " + i + " "));
                        }
                        predicateIns = predicateIns.And(predicateInscheCkbox);
                    }
                    if (!string.IsNullOrWhiteSpace(inp.ListSelectedVrem))
                    {
                        string[] tmp = inp.ListSelectedVrem.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        var predicateInscheCkbox = PredicateBuilder.True<FEAction>();
                        foreach (var i in tmp)
                        {
                            // predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Vrem.Contains(i));
                            predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Vrem == i || x1.Vrem.StartsWith(i + " ")
                                         || x1.Vrem.EndsWith(" " + i) || x1.Vrem.Contains(" " + i + " "));
                        }
                        predicateIns = predicateIns.And(predicateInscheCkbox);
                    }
                    predicate = predicate.Or(predicateIns);
                }
                int formsLen = forms.Length;
                checkInp = db.FEActions.Where(predicate).GroupBy(x1 => x1.Idfe).Where(x1 => x1.Count() >= formsLen).Select(x1 => x1.Key).ToList();
            }

            //если уже на этом этапе ничего не найдено дальше не искать
            if (checkInp.Count < 1)
                return null;


            List<int> checkObj = new List<int>();
            using (var db = new ApplicationDbContext())
            {
                {
                    var predicate = PredicateBuilder.False<FEObject>();
                    int AllCountPhase = 0;
                    foreach (var obj in objects)
                    {
                        int beg = (obj.Begin ? 1 : 0);
                        int numph = 1;

                        foreach (var i in obj)
                        {
                            if (i != null)
                            {
                                //копируем значения в локальные переменные тк предикат(делегат который передаем) захватит их по ссылке
                                int numph_ = numph;
                                int beg_ = beg;
                                AllCountPhase++;
                                var predicateIns = PredicateBuilder.True<FEObject>();
                                predicateIns = predicateIns.And(x1 => x1.Begin == beg_);
                                predicateIns = predicateIns.And(x1 => x1.NumPhase == numph_);

                                //поиск не через contains для того что бы не находился фэ у которого QWER12 при поиске QWER1

                                if (!string.IsNullOrWhiteSpace(i.Composition))
                                {
                                    string[] tmp = i.Composition.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    var predicateInscheCkbox = PredicateBuilder.True<FEObject>();
                                    foreach (var i2 in tmp)
                                        predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Composition == i2 || x1.Composition.StartsWith(i2 + " ")
                                        || x1.Composition.EndsWith(" " + i2) || x1.Composition.Contains(" " + i2 + " "));
                                    predicateIns = predicateIns.And(predicateInscheCkbox);
                                }
                                if (!string.IsNullOrWhiteSpace(i.Conductivity))
                                {
                                    string[] tmp = i.Conductivity.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    var predicateInscheCkbox = PredicateBuilder.True<FEObject>();
                                    foreach (var i2 in tmp)
                                        predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Conductivity == i2 || x1.Conductivity.StartsWith(i2 + " ") ||
                                         x1.Conductivity.EndsWith(" " + i2) || x1.Conductivity.Contains(" " + i2 + " "));
                                    predicateIns = predicateIns.And(predicateInscheCkbox);
                                }
                                if (!string.IsNullOrWhiteSpace(i.MagneticStructure))
                                {
                                    string[] tmp = i.MagneticStructure.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    var predicateInscheCkbox = PredicateBuilder.True<FEObject>();
                                    foreach (var i2 in tmp)
                                        predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.MagneticStructure == i2 || x1.MagneticStructure.StartsWith(i2 + " ") ||
                                         x1.MagneticStructure.EndsWith(" " + i2) || x1.MagneticStructure.Contains(" " + i2 + " "));
                                    predicateIns = predicateIns.And(predicateInscheCkbox);
                                }
                                if (!string.IsNullOrWhiteSpace(i.MechanicalState))
                                {
                                    string[] tmp = i.MechanicalState.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    var predicateInscheCkbox = PredicateBuilder.True<FEObject>();
                                    foreach (var i2 in tmp)
                                        predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.MechanicalState == i2 || x1.MechanicalState.StartsWith(i2 + " ") ||
                                        x1.MechanicalState.EndsWith(" " + i2) || x1.MechanicalState.Contains(" " + i2 + " "));
                                    predicateIns = predicateIns.And(predicateInscheCkbox);
                                }
                                if (!string.IsNullOrWhiteSpace(i.OpticalState))
                                {
                                    string[] tmp = i.OpticalState.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    var predicateInscheCkbox = PredicateBuilder.True<FEObject>();
                                    foreach (var i2 in tmp)
                                        predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.OpticalState == i2 || x1.OpticalState.StartsWith(i2 + " ") ||
                                        x1.OpticalState.EndsWith(" " + i2) || x1.OpticalState.Contains(" " + i2 + " "));
                                    predicateIns = predicateIns.And(predicateInscheCkbox);
                                }
                                if (!string.IsNullOrWhiteSpace(i.PhaseState))
                                {
                                    string[] tmp = i.PhaseState.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    var predicateInscheCkbox = PredicateBuilder.True<FEObject>();
                                    foreach (var i2 in tmp)
                                        predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.PhaseState == i2 || x1.PhaseState.StartsWith(i2 + " ") ||
                                        x1.PhaseState.EndsWith(" " + i2) || x1.PhaseState.Contains(" " + i2 + " "));
                                    predicateIns = predicateIns.And(predicateInscheCkbox);
                                }
                                if (!string.IsNullOrWhiteSpace(i.Special))
                                {
                                    string[] tmp = i.Special.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    var predicateInscheCkbox = PredicateBuilder.True<FEObject>();
                                    foreach (var i2 in tmp)
                                        predicateInscheCkbox = predicateInscheCkbox.And(x1 => x1.Special == i2 || x1.Special.StartsWith(i2 + " ") ||
                                        x1.Special.EndsWith(" " + i2) || x1.Special.Contains(" " + i2 + " "));
                                    predicateIns = predicateIns.And(predicateInscheCkbox);
                                }
                                predicate = predicate.Or(predicateIns);
                                numph++;
                            }
                        }
                    }
                    if (AllCountPhase == 0)
                        checkObj = db.FEObjects.Select(x1 => x1.Idfe).Distinct().ToList();

                    else
                        checkObj = db.FEObjects.Where(predicate).GroupBy(x1 => x1.Idfe).Where(x1 => x1.Count() >= AllCountPhase).Select(x1 => x1.Key).ToList();
                }
            }


            if (checkObj.Count == 0)
                return null;

            //сравниваем состояния и результаты всех запросов
            list_id = FEText.GetList(null, checkObj.Join(checkInp, x1 => x1, x2 => x2, (x1, x2) => x1).ToArray())
                .Where(x1 => (string.IsNullOrWhiteSpace(stateBegin) ? (changedObject ? !string.IsNullOrWhiteSpace(x1.StateBeginId) : true) : x1.StateBeginId == stateBegin) &&
                (string.IsNullOrWhiteSpace(stateEnd) ? (changedObject ? !string.IsNullOrWhiteSpace(x1.StateEndId) : true) : x1.StateEndId == stateEnd)).Select(x1 => x1.IDFE).ToArray();


            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            if (user == null)
                return null;
            list_id = user.CheckAccessPhys(list_id.ToList(), HttpContext).
                    SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToArray();

            return list_id;
        }


        /// <summary>
        /// Метод для загрузки изображений
        /// </summary>
        public void LoadImage()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Set<FEText>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.Images).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Images).Load();
            }

        }

        /// <summary>
        /// Метод для загрузки latex
        /// </summary>
        public void LoadLatex()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Set<FEText>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.LatexFormulas).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.LatexFormulas).Load();
            }

        }

        /// <summary>
        /// Метод для загрузки latex и преобразования их к байтам
        /// </summary>
        public void AddByteToLatexImages()
        {
            this.LoadLatex();
            foreach (var i in this.LatexFormulas)
                i.SetBytes();
        }


        /// <summary>
        /// Метод для загрузки списков к которым принадлежит ФЭ
        /// </summary>
        public void LoadLists()
        {
            using (var db = new ApplicationDbContext())
            {
                db.Set<FEText>().Attach(this);
                if (!db.Entry(this).Collection(x1 => x1.Lists).IsLoaded)
                    db.Entry(this).Collection(x1 => x1.Lists).Load();
            }

        }


        /// <summary>
        /// Метод для получения семантически схожих записей
        /// </summary>
        /// <param name="id">id записи фэ для которой ищем похожие</param>
        /// <param name="HttpContext"></param>
        /// <param name="count">количество схожих</param>
        /// <returns>список схожих записей</returns>
        public static List<int> GetListSimilar(int id, HttpContextBase HttpContext, int count = -1)
        {
            List<int> res = new List<int>();



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
            if (count >= 0)
                res.AddRange(accessList.Take(count));
            else
                res.AddRange(accessList);

            return res;
        }


        /// <summary>
        /// Метод для получения списка ФЭ(с проставленным флагом FavouritedCurrentUser) из массива id фэ,
        /// нет проверок на то может ли текущий пользователь получить все фэ из списка
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <param name="id">массив id фэ которые нужно получить</param>
        /// <returns>список записей FEText найденных по id</returns>
        public static List<FEText> GetList(string userId, params int[] id)
        {
            var res = new List<FEText>();
            if (id != null && id.Length > 0)
                using (var db = new ApplicationDbContext())
                {
                    res = db.FEText.Join(id, x1 => x1.IDFE, x2 => x2, (x1, x2) => x1).ToList();
                    userId = userId ?? ApplicationUser.GetUserId();
                    foreach (var i in res)
                    {
                        i.FavouritedCurrentUser = i.Favourited(userId);
                    }
                }
            return res;
        }

        /// <summary>
        /// Метод для получения списка ФЭ(с проставленным флагом FavouritedCurrentUser) из массива id фэ,
        /// проверока на то может ли текущий пользователь получить все фэ из списка
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="id">массив id фэ которые нужно получить</param>
        /// <returns>список выданных записей FEText найденных по id</returns>
        public static List<FEText> GetListIfAccess(HttpContextBase HttpContext, params int[] id)
        {
            var res = new List<FEText>();

            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            if (user == null)
                return res;
            List<int> accessList = user.CheckAccessPhys(id?.ToList(), HttpContext);
            res = FEText.GetList(user.Id, accessList.ToArray());
            return res;
        }



        /// <summary>
        /// Метод валидации
        /// </summary>
        /// <returns>флаг успеха</returns>
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


            using (var db = new ApplicationDbContext())
            {
                var states = db.StateObjects.Where(x1 => x1.Id == this.StateBeginId || x1.Id == this.StateEndId).ToList();
                foreach (var i in states)
                    if (i.CountPhase == null)
                        return false;

                if (this.ChangedObject && states.Count < 2)
                    return false;
                if (states.Count == 0)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// добавление новой записи ФЭ
        /// </summary>
        /// <param name="forms">входные\выходные дескрипторы</param>
        /// <param name="objForms">начальные\конечные характеристики объекта</param>
        /// <param name="addImgs">байты для изображений</param>
        /// <param name="latexformulas">latex формулы</param>
        /// <returns>флаг успеха</returns>
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

        /// <summary>
        /// метод для изменения существующей записи фэ
        /// </summary>
        /// <param name="newObj">новые данные для записи фэ</param>
        /// <param name="deleteImg">изображения которые необходимо удалить</param>
        /// <param name="addImgs">изображения которые необходимо добавить</param>
        /// <param name="forms">входные\выходные дескрипторы</param>
        /// <param name="objForms">начальные\конечные характеристики объекта</param>
        /// <param name="latexformulas">latex формулы</param>
        /// <returns>флаг успеха</returns>
        public bool ChangeDb(FEText newObj, List<int> deleteImg = null, List<byte[]> addImgs = null, List<DescrSearchI> forms = null,
            List<DescrObjectI> objForms = null, ChangeLatex[] latexformulas = null)
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
                                                    Join(deleteImg, x1 => x1.Id, x2 => x2, (x1, x2) => x1).ToList();
                            db.Images.RemoveRange(imgs);
                            db.SaveChanges();
                        }
                        this.AddImages(addImgs, db);


                        var descrdb = db.FEActions.Where(x1 => x1.Idfe == this.IDFE);
                        db.FEActions.RemoveRange(descrdb);//без сохранения

                        foreach (var i in forms)
                        {
                            var act = new FEAction() { Idfe = this.IDFE };
                            act.SetFromInput(i);
                            db.FEActions.Add(act);
                        }

                        db.SaveChanges();

                        var objdb = db.FEObjects.Where(x1 => x1.Idfe == this.IDFE);
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
                        if (latexformulas != null && latexformulas.Length > 0)
                        {
                            //add
                            var ladd = latexformulas.Where(x1 => x1.Action == 0);
                            db.FELatexFormulas.AddRange(ladd.Select(x1 => new FELatexFormula() { Formula = x1.Text ?? "", FeTextIDFE = this.IDFE }));
                            db.SaveChanges();

                            //change
                            var lchange = latexformulas.Where(x1 => x1.Action == 1);
                            var lchangeid = lchange.Select(x1 => x1.Id);
                            var oldchange = db.FELatexFormulas.Where(x1 => lchangeid.FirstOrDefault(x2 => x2 == x1.Id) != 0 && x1.FeTextIDFE == this.IDFE).ToList();
                            foreach (var i in oldchange)
                            {
                                var tmp = lchange.FirstOrDefault(x1 => x1.Id == i.Id);
                                if (tmp != null && i.Formula != tmp.Text)
                                    i.Formula = tmp.Text ?? "";
                            }
                            db.SaveChanges();

                            //delete
                            var ldel = latexformulas.Where(x1 => x1.Action == 2).Select(x1 => x1.Id).ToList();
                            var delst = db.FELatexFormulas.Where(x1 => x1.FeTextIDFE == this.IDFE && ldel.Contains(x1.Id));
                            db.FELatexFormulas.RemoveRange(delst);
                            db.SaveChanges();

                        }

                        if (chengedText)
                            Lucene_.UpdateDocument(this.IDFE.ToString(), this);

                        transaction.Commit();
                        commited = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            return commited;
        }




        /// <summary>
        /// Добавляет новые изображения в бд,не загружает картинки, только добавляет в бд
        /// </summary>
        /// <param name="imgs">изображения</param>
        public void AddImages(List<byte[]> imgs)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //db.Set<FEText>().Attach(this);
                //this.
                this.AddImages(imgs, db);
            }
        }

        /// <summary>
        ///  Добавляет новые изображения в бд,не загружает картинки, только добавляет в бд
        /// </summary>
        /// <param name="imgs">изображения</param>
        /// <param name="db">контекст бд</param>
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
        /// <param name="personId">id пользователя</param>
        /// <param name="HttpContext"></param>
        /// <returns>true- если теперь зафоловлена пользователем</returns>
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

        /// <summary>
        /// проверка на то добавил ли пользователь в избранное этот ФЭ
        /// </summary>
        /// <param name="personId">id пользователя</param>
        /// <returns>true-если зафоловлена пользователем</returns>
        public bool Favourited(string personId)
        {
            bool res = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                res = this.Favourited(personId, db);
            }
            return res;
        }

        /// <summary>
        /// проверка на то добавил ли пользователь в избранное этот ФЭ
        /// </summary>
        /// <param name="personId">id пользователя</param>
        /// <param name="db">контекст бд</param>
        /// <returns>true-если зафоловлена пользователем</returns>
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


        /// <summary>
        /// метод для получения следующей разрешенной записи ФЭ
        /// </summary>
        /// <param name="id">id текущей записи</param>
        /// <param name="HttpContext"></param>
        /// <returns>следующая разрешенная запись</returns>
        public static FEText GetNextAccessPhysic(int id, HttpContextBase HttpContext)
        {
            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            return user.GetNextAccessPhysic(id, HttpContext);

        }

        /// <summary>
        /// метод для получения предыдущей разрешенной записи ФЭ
        /// </summary>
        /// <param name="id">id текущей записи</param>
        /// <param name="HttpContext"></param>
        /// <returns>предыдущая разрешенная запись</returns>
        public static FEText GetPrevAccessPhysic(int id, HttpContextBase HttpContext)
        {
            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            return user.GetPrevAccessPhysic(id, HttpContext);
        }

        /// <summary>
        /// метод для получения первой разрешенной записи ФЭ
        /// </summary>
        /// <param name="HttpContext"> контекст http</param>
        /// <returns>первая разрешенная запись</returns>
        public static FEText GetFirstAccessPhysic(HttpContextBase HttpContext)
        {
            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            return user.GetFirstAccessPhysic(HttpContext);
        }

        /// <summary>
        /// метод для получения последней разрешенной записи ФЭ
        /// </summary>
        /// <param name="HttpContext"> контекст http</param>
        /// <returns>последняя разрешенная запись</returns>
        public static FEText GetLastAccessPhysic(HttpContextBase HttpContext)
        {
            ApplicationUser user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            return user.GetLastAccessPhysic(HttpContext);
        }

        /// <summary>
        /// Метод для обнуления семантической записи
        /// </summary>
        public static void ReloadSemanticRecord()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var fe = db.FEText.FirstOrDefault(x1 => x1.IDFE == Constants.FEIDFORSEMANTICSEARCH);
                fe.Text = Constants.FeSemanticNullText;
                db.SaveChanges();

            }
        }

    }
}