

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


        //TODO раскомментить
        [ScaffoldColumn(false)]
        public bool NotApprove { get; set; }


        [ScaffoldColumn(false)]
        public int CountInput { get; set; }

        [ScaffoldColumn(false)]
        public bool ChangedObject { get; set; }



        [NotMapped]
       // [ScaffoldColumn(false)]//TODO hz
        public bool? FavouritedCurrentUser { get; set; }//зафоловил ли текущий пользователь эту запись

        public string StateBeginId { get; set; }
        public StateObject StateBegin { get; set; }

        public string StateEndId { get; set; }
        public StateObject StateEnd { get; set; }


        public ICollection<Image> Images { get; set; }

        public ICollection<ApplicationUser> FavouritedUser { get; set; }


        public List<ListPhysics> Lists { get; set; }
        public List<ApplicationUser> Users { get; set; }

        public FEText()
        {
            this.Images = new List<Image>();
            FavouritedUser = new List<ApplicationUser>();
            NotApprove = true;
            FavouritedCurrentUser = null;

            CountInput = 1;
            ChangedObject = false;
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

            this.CountInput = a.CountInput;
            this.ChangedObject = a.ChangedObject;

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
            Name = Lucene_.ChangeForMap(Name);
            TextApp = Lucene_.ChangeForMap(TextApp);
            TextInp = Lucene_.ChangeForMap(TextInp);
            TextLit = Lucene_.ChangeForMap(TextLit);
            TextObj = Lucene_.ChangeForMap(TextObj);
            TextOut = Lucene_.ChangeForMap(TextOut);


        }


        public static List<string> GetPropTextSearch()
        {
            var res = new List<string>();
            var listNM = new List<string>() { "IDFE", "CountInput", "ChangedObject", "NotApprove", "FavouritedCurrentUser", "Images", "FavouritedUser" };//исключаем


            PropertyInfo[] myPropertyInfo;
            Type myType = typeof(FEText);
            // Get the type and fields of FieldInfoClass.
            myPropertyInfo = myType.GetProperties();
            res = myPropertyInfo.Where(x1 => listNM.FirstOrDefault(x2 => x2 == x1.Name) == null).Select(x1 => x1.Name).ToList();

            return res;
        }

        public static FEText Get(int? id)
        {
            FEText res = null;
            if (id != null)//&&id>0
                using (var db = new ApplicationDbContext())
                    res = db.FEText.FirstOrDefault(x1 => x1.IDFE == id);
            return res;

        }

        //при более чем 2х входах и 1 выходах-(||forms>3) эту функцию необходимо будет изменить
        public static int[] GetByDescr(string stateBegin, string stateEnd, DescrSearchI[] forms, DescrObjectI[]objects)
        {
            int[] list_id = null;


            //if (DescrSearchI.Validation(inp) && DescrSearchI.Validation(outp))
            //{
            //поиск
            //List<int> list_id = new List<int>();
            foreach (var i in forms)
                i.DeleteNotChildCheckbox();
            foreach (var i in objects)
                i.DeleteNotChildCheckbox();
            //inp.DeleteNotChildCheckbox();
            //outp.DeleteNotChildCheckbox();

            List<FEAction> formsList = new List<FEAction>();
            List<FEObject> objectsList = new List<FEObject>();
            //System.Collections.Generic.List<System.Linq.IGrouping<int, dip.Models.Domain.FEAction>> checkInp = null;
            List<int> checkInp = new List<int>();

            using (var db = new ApplicationDbContext())
            {
                //IQueryable<dip.Models.Domain.FEAction> forms_query = db.FEActions;


                //
                //{
                //    var t_ = forms[0];
                //    var y_ = forms[1];
                //    var g_= forms_query.Where(x1 => 
                //x1.Name == t_.ActionId &&
                //  x1.Type == t_.ActionType &&
                //  x1.FizVelId == t_.FizVelId &&
                //  x1.Pros == t_.ListSelectedPros &&
                //  x1.Spec == t_.ListSelectedSpec &&
                //  x1.Vrem == t_.ListSelectedVrem &&
                //  x1.FizVelSection == t_.ParametricFizVelId).ToList();

                //    var g_1 = forms_query.Where(x1 =>
                // x1.Name == y_.ActionId &&
                //   x1.Type == y_.ActionType &&
                //   x1.FizVelId == y_.FizVelId &&
                //   x1.Pros == y_.ListSelectedPros &&
                //   x1.Spec == y_.ListSelectedSpec &&
                //   x1.Vrem == y_.ListSelectedVrem &&
                //   x1.FizVelSection == y_.ParametricFizVelId).ToList();

                //    var g_2 = forms_query.Where(x1 =>
                //x1.Name == y_.ActionId &&
                //  x1.Type == y_.ActionType &&
                //  x1.FizVelId == y_.FizVelId &&
                //  x1.Pros == y_.ListSelectedPros &&
                //  x1.Spec == y_.ListSelectedSpec &&
                //  x1.Vrem == y_.ListSelectedVrem &&
                //  x1.FizVelSection == y_.ParametricFizVelId).Where(x1 =>
                //x1.Name == t_.ActionId &&
                //  x1.Type == t_.ActionType &&
                //  x1.FizVelId == t_.FizVelId &&
                //  x1.Pros == t_.ListSelectedPros &&
                //  x1.Spec == t_.ListSelectedSpec &&
                //  x1.Vrem == t_.ListSelectedVrem &&
                //  x1.FizVelSection == t_.ParametricFizVelId).ToList();

                //    var g__ = 10;
                //}



                //
                //TODO временно
                //Mono.Linq.Expressions.
                var predicate = PredicateBuilder.False<FEAction>();
               


                foreach (var inp in forms)
                {
                    int beg = (inp.InputForm ? 1 : 0);
                    //    forms_query = forms_query.Where(x1 => x1.Input == beg &&
                    //x1.Name == inp.ActionId &&
                    //  x1.Type == inp.ActionType &&
                    //  x1.FizVelId == inp.FizVelId &&
                    //  x1.Pros == inp.ListSelectedPros &&
                    //  x1.Spec == inp.ListSelectedSpec &&
                    //  x1.Vrem == inp.ListSelectedVrem &&
                    //  x1.FizVelSection == inp.ParametricFizVelId);
                    predicate = predicate.Or(x1 => x1.Input == beg &&
                 x1.Name == inp.ActionId &&
                   x1.Type == inp.ActionType &&
                   x1.FizVelId == inp.FizVelId &&
                   x1.Pros == inp.ListSelectedPros &&
                   x1.Spec == inp.ListSelectedSpec &&
                   x1.Vrem == inp.ListSelectedVrem &&
                   x1.FizVelSection == inp.ParametricFizVelId);

                 //   formsList.AddRange(db.FEActions.Where(x1 => x1.Input == beg &&
                 //x1.Name == inp.ActionId &&
                 //  x1.Type == inp.ActionType &&
                 //  x1.FizVelId == inp.FizVelId &&
                 //  x1.Pros == inp.ListSelectedPros &&
                 //  x1.Spec == inp.ListSelectedSpec &&
                 //  x1.Vrem == inp.ListSelectedVrem &&
                 //  x1.FizVelSection == inp.ParametricFizVelId).ToList());
                    //InputForm
                }
                //formsList = forms_query.ToList();
                int formsLen = forms.Length;
                checkInp =db.FEActions.Where(predicate).GroupBy(x1 => x1.Idfe).Where(x1 => x1.Count() == formsLen).Select(x1=>x1.Key).ToList();
            }
            //.ToList() .ToArray()
            
            //checkInp=formsList.GroupBy(x1 => x1.Idfe).Where(x1=>x1.Count()== forms.Length).ToList();
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
                    //objectsList.AddRange(db.FEObjects.Where(x1 => x1.Begin == beg &&
                    // x1.NumPhase == 1 &&
                    // x1.Composition == obj.ListSelectedPhase1.Composition &&
                    // x1.Conductivity == obj.ListSelectedPhase1.Conductivity &&
                    // x1.MagneticStructure == obj.ListSelectedPhase1.MagneticStructure &&
                    // x1.MechanicalState == obj.ListSelectedPhase1.MechanicalState &&
                    // x1.OpticalState == obj.ListSelectedPhase1.OpticalState &&
                    // x1.PhaseState == obj.ListSelectedPhase1.PhaseState &&
                    // x1.Special == obj.ListSelectedPhase1.Special).ToList());


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
                    //     objectsList.AddRange(db.FEObjects.Where(x1 => x1.Begin == beg &&
                    //      x1.NumPhase == 2 &&
                    //     x1.Composition == obj.ListSelectedPhase2.Composition &&
                    //x1.Conductivity == obj.ListSelectedPhase2.Conductivity &&
                    //x1.MagneticStructure == obj.ListSelectedPhase2.MagneticStructure &&
                    //x1.MechanicalState == obj.ListSelectedPhase2.MechanicalState &&
                    //x1.OpticalState == obj.ListSelectedPhase2.OpticalState &&
                    //x1.PhaseState == obj.ListSelectedPhase2.PhaseState &&
                    //x1.Special == obj.ListSelectedPhase2.Special).ToList());

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
                       //objectsList.AddRange(db.FEObjects.Where(x1 => x1.Begin == beg &&
                       //  x1.NumPhase == 3 &&
                       // x1.Composition == obj.ListSelectedPhase3.Composition &&
                       //x1.Conductivity == obj.ListSelectedPhase3.Conductivity &&
                       //x1.MagneticStructure == obj.ListSelectedPhase3.MagneticStructure &&
                       //x1.MechanicalState == obj.ListSelectedPhase3.MechanicalState &&
                       //x1.OpticalState == obj.ListSelectedPhase3.OpticalState &&
                       //x1.PhaseState == obj.ListSelectedPhase3.PhaseState &&
                       //x1.Special == obj.ListSelectedPhase3.Special).ToList());


                    //objects_query = objects_query.Where(x1 => x1.Begin == beg &&
                    //x1.NumPhase == 1 &&
                    //x1.Composition == obj.ListSelectedPhase1.Composition &&
                    //x1.Conductivity == obj.ListSelectedPhase1.Conductivity &&
                    //x1.MagneticStructure == obj.ListSelectedPhase1.MagneticStructure &&
                    //x1.MechanicalState == obj.ListSelectedPhase1.MechanicalState &&
                    //x1.OpticalState == obj.ListSelectedPhase1.OpticalState &&
                    //x1.PhaseState == obj.ListSelectedPhase1.PhaseState &&
                    //x1.Special == obj.ListSelectedPhase1.Special);

                    // //2 phase
                    // if (obj.ListSelectedPhase2 != null)
                    //     objects_query = objects_query.Where(x1 => x1.Begin == beg &&
                    //      x1.NumPhase == 2 &&
                    //     x1.Composition == obj.ListSelectedPhase2.Composition &&
                    //x1.Conductivity == obj.ListSelectedPhase2.Conductivity &&
                    //x1.MagneticStructure == obj.ListSelectedPhase2.MagneticStructure &&
                    //x1.MechanicalState == obj.ListSelectedPhase2.MechanicalState &&
                    //x1.OpticalState == obj.ListSelectedPhase2.OpticalState &&
                    //x1.PhaseState == obj.ListSelectedPhase2.PhaseState &&
                    //x1.Special == obj.ListSelectedPhase2.Special);

                    // //3phase
                    // if(obj.ListSelectedPhase3!=null)
                    // objects_query = objects_query.Where(x1 => x1.Begin == beg &&
                    //  x1.NumPhase == 3 &&
                    // x1.Composition == obj.ListSelectedPhase3.Composition &&
                    //x1.Conductivity == obj.ListSelectedPhase3.Conductivity &&
                    //x1.MagneticStructure == obj.ListSelectedPhase3.MagneticStructure &&
                    //x1.MechanicalState == obj.ListSelectedPhase3.MechanicalState &&
                    //x1.OpticalState == obj.ListSelectedPhase3.OpticalState &&
                    //x1.PhaseState == obj.ListSelectedPhase3.PhaseState &&
                    //x1.Special == obj.ListSelectedPhase3.Special);



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

                //objectsList = objects_query.ToList();
                //db.FEActions.Where(predicate).GroupBy(x1 => x1.Idfe).Where(x1 => x1.Count() == formsLen).Select(x1 => x1.Key).ToList();
                 checkObj = db.FEObjects.Where(predicate).GroupBy(x1 => x1.Idfe).Where(x1 => x1.Count() == AllCountPhase).Select(x1 => x1.Key).ToList();
            }

            
            //var checkObj = objectsList.GroupBy(x1=>x1.Idfe).Where(x1=>x1.Count()== AllCountPhase).ToList();
            //formsList.Where(x1=>x1.Input==1).Join(formsList.Where(x1 => x1.Input == 0),inp1=> inp1.Idfe, inp0 => inp0.Idfe,(inp1, inp0)=> inp1.Idfe);
            //objectsList;
            if (checkObj.Count == 0)
                return null;

            //x1.Key x2.Key

            //сравниваем состояния и результаты всех запросов
            list_id = FEText.GetList(checkObj.Join(checkInp, x1 => x1, x2 => x2, (x1, x2) => x1).ToArray())
                .Where(x1 => x1.StateBeginId == stateBegin && x1.StateEndId == stateEnd).Select(x1=>x1.IDFE).ToArray();
            //foreach (var i in list_id)
               // FEText.GetList(list_id).Where(x1=>x1.StateBeginId== stateBegin&&x1.StateEndId== stateEnd).ToArray();


            //using (var db = new ApplicationDbContext())
            //{
                //TODO оптимизация? разница только в  x1.Input == 1\0
                //находим все записи которые подходят по входным параметрам
                //var inp_query = db.FEActions.Where(x1 => x1.Input == 1 &&
                //x1.Name == inp.ActionId &&
                //  x1.Type == inp.ActionType &&
                //  x1.FizVelId == inp.FizVelId &&
                //  x1.Pros == inp.ListSelectedPros &&
                //  x1.Spec == inp.ListSelectedSpec &&
                //  x1.Vrem == inp.ListSelectedVrem &&
                //  x1.FizVelSection == inp.ParametricFizVelId);

                ////находим все записи которые подходят по выходным параметрам
                //var out_query = db.FEActions.Where(x1 => x1.Input == 0 &&
                // x1.Name == outp.ActionId &&
                //x1.Type == outp.ActionType &&
                //x1.FizVelId == outp.FizVelId &&
                //x1.Pros == outp.ListSelectedPros &&
                //x1.Spec == outp.ListSelectedSpec &&
                //x1.Vrem == outp.ListSelectedVrem &&
                //x1.FizVelSection == outp.ParametricFizVelId);

                //записи которые подходят по всем параметрам
                //list_id = inp_query.Join(out_query, x1 => x1.Idfe, x2 => x2.Idfe, (x1, x2) => x1.Idfe).ToArray();
                
            //}
            //}




            //////if (DescrSearchI.Validation(inp) && DescrSearchI.Validation(outp))
            //////{
            //////поиск
            //////List<int> list_id = new List<int>();
            ////foreach(var i in forms)
            ////    i.DeleteNotChildCheckbox();
            //////inp.DeleteNotChildCheckbox();
            //////outp.DeleteNotChildCheckbox();
            ////using (var db = new ApplicationDbContext())
            ////{
            ////    //TODO оптимизация? разница только в  x1.Input == 1\0
            ////    //находим все записи которые подходят по входным параметрам
            ////    var inp_query = db.FEActions.Where(x1 => x1.Input == 1 &&
            ////    x1.Name == inp.ActionId &&
            ////      x1.Type == inp.ActionType &&
            ////      x1.FizVelId == inp.FizVelId &&
            ////      x1.Pros == inp.ListSelectedPros &&
            ////      x1.Spec == inp.ListSelectedSpec &&
            ////      x1.Vrem == inp.ListSelectedVrem &&
            ////      x1.FizVelSection == inp.ParametricFizVelId);

            ////    //находим все записи которые подходят по выходным параметрам
            ////    var out_query = db.FEActions.Where(x1 => x1.Input == 0 &&
            ////     x1.Name == outp.ActionId &&
            ////    x1.Type == outp.ActionType &&
            ////    x1.FizVelId == outp.FizVelId &&
            ////    x1.Pros == outp.ListSelectedPros &&
            ////    x1.Spec == outp.ListSelectedSpec &&
            ////    x1.Vrem == outp.ListSelectedVrem &&
            ////    x1.FizVelSection == outp.ParametricFizVelId);

            ////    //записи которые подходят по всем параметрам
            ////    list_id = inp_query.Join(out_query, x1 => x1.Idfe, x2 => x2.Idfe, (x1, x2) => x1.Idfe).ToArray();
            ////    //ViewBag.listFeId = list_id;

            ////}
            //////}
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


        public static List<int> GetListSimilar(int id, int count = 5)
        {
            List<int> res = new List<int>();

            //этот запрос может возвращать дублирующиеся id тк сравниват не записи в целом а отдельно столбцы
            //            var quer=$@"select top({count})IDFE
            //from
            //       semanticsimilaritytable (FETexts, *, {id} ) data
            //            inner join dbo.FETexts
            //            txt on data.matched_document_key = txt.IDFE
            //order by data.score desc";

            var quer = $@"
select   [txt].IDFE
from
       semanticsimilaritytable (FETexts, *, {id}  ) as data
            inner join dbo.FETexts as
            txt on data.matched_document_key = [txt].IDFE
order by [data].score desc
";






            //var connection1 = new SqlConnection();
            //connection1.ConnectionString = Constants.sql_0;
            //var command1 = new SqlCommand();
            //command1.Connection = connection1;
            //command1.CommandType = CommandType.Text;

            //connection1.Open();
            //command1.CommandText = quer;
            //using (SqlDataReader reader = command1.ExecuteReader())
            //{

            //    //SqlDataReader reader = command.ExecuteReader();
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {


            //            res.Add(Convert.ToInt32( reader["IDFE"]));

            //        }
            //    }

            //}



            var dict = DataBase.DataBase.ExecuteQuery(quer, null, "IDFE");
            foreach (var i in dict)
            {
                if (res.Count == count)
                    break;
                int idRec = Convert.ToInt32(i["IDFE"]);
                if (!res.Contains(idRec))
                    res.Add(idRec);
            }



            return res;
        }
        //public  List<int> GetListSimilar(int count = 5)
        //{

        //    return FEText.GetListSimilarS(this.IDFE,count);
        //}


        public static List<FEText> GetList(params int[] id)
        {
            var res = new List<FEText>();
            if (id != null&& id.Length>0)
                using (var db = new ApplicationDbContext())
                {
                    res = db.FEText.Join(id, x1 => x1.IDFE, x2 => x2, (x1, x2) => x1).ToList();
                    string check_id = ApplicationUser.GetUserId();
                    foreach(var i in res)
                    {
                        i.FavouritedCurrentUser = i.Favourited(check_id);
                    }
                }
            
                    
            return res;
        }

        //public void GetDescrFrom(DescrSearchIInput inp = null, DescrSearchIOut outp = null)
        //{
        //    List<FEAction> lst = null;
        //    using (var db = new ApplicationDbContext())
        //    {
        //        lst = db.FEActions.Where(x1 => x1.Idfe == this.IDFE).ToList();
        //    }
        //    inp = new DescrSearchIInput(lst.First(x1 => x1.Input == 1));
        //    outp?

        //}

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

            return true;
        }

        public bool AddToDb( DescrSearchI[] forms, DescrObjectI[] objForms , List<byte[]> addImgs = null)
        {
            //if (!this.Validation())
            //    return false;



            using (ApplicationDbContext db = new ApplicationDbContext())
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
                    List<FEObject> objects = new List<FEObject>()
                    {
                        
                        new FEObject(i.ListSelectedPhase1, this.IDFE,(i.Begin?1:0)),
                        new FEObject(i.ListSelectedPhase2, this.IDFE,(i.Begin?1:0)),
                        new FEObject(i.ListSelectedPhase3, this.IDFE,(i.Begin?1:0))
                };
                    db.FEObjects.AddRange(objects);
                    db.SaveChanges();
                }

                //FEAction inpa = new FEAction()
                //{
                //    Idfe = this.IDFE,
                //    Input = 1,

                //};
                //inpa.SetFromInput(inp);
                //db.FEActions.Add(inpa);

                //FEAction outpa = new FEAction()
                //{
                //    Idfe = this.IDFE,
                //    Input = 0,

                //};
                //outpa.SetFromInput(outp);
                //db.FEActions.Add(outpa);
                this.AddImages(addImgs, db);

                db.SaveChanges();

                //DescrObjectI[] objForms = ; //TODO-objForms


            }
            Lucene_.BuildIndexSolo(this);
            return true;
        }

        public bool ChangeDb(FEText new_obj, List<int> deleteImg = null, List<byte[]> addImgs = null, List<DescrSearchI> forms = null, List<DescrObjectI> objForms=null)
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<FEText>().Attach(this);
                this.Equal(new_obj);
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
                
                //var inpdb = descrdb.Where(x1 => x1.Input == 1).ToList();
                //var outpdb = descrdb.Where(x1 => x1.Input == 0).ToList();
                //if (inpdb == null || outpdb == null)
                //    return false;
                //foreach(var i in inpdb)//TODO
                //{
                //    var newobj = forms.FirstOrDefault(x1 => x1.InputForm);
                //    if (newobj != null)
                //    {
                //        forms.Remove(newobj);
                //        //forms.FirstOrDefault(x1=>x1.id);
                //        i.SetFromInput(newobj);
                //    }
                    
                //}
                foreach(var i in forms)
                {
                    var act=new FEAction() {Idfe=this.IDFE };//,Input=(i.InputForm?1:0)
                    act.SetFromInput(i);
                    db.FEActions.Add(act);
                }
                //foreach (var i in outpdb)
                //{
                //    var newobj = forms.FirstOrDefault(x1 => !x1.InputForm);
                //    if (newobj != null)
                //    {
                //        forms.Remove(newobj);
                //        //forms.FirstOrDefault(x1=>x1.id);
                //        i.SetFromInput(newobj);
                //    }

                //}
                //inpdb.SetFromInput(inp);
                //outpdb.SetFromInput(outp);

                db.SaveChanges();


                var objdb = db.FEObjects.Where(x1 => x1.Idfe == this.IDFE);//TODO обработка ошибок(восстановление при неудаче)
                db.FEObjects.RemoveRange(objdb);//без сохранения
                foreach (var i in objForms)
                {
                    List<FEObject> objects = new List<FEObject>()
                    {
                        new FEObject(i.ListSelectedPhase1, this.IDFE,(i.Begin?1:0)),
                        new FEObject(i.ListSelectedPhase2, this.IDFE,(i.Begin?1:0)),
                        new FEObject(i.ListSelectedPhase3, this.IDFE,(i.Begin?1:0))
                };
                    db.FEObjects.AddRange(objects);
                    db.SaveChanges();
                }
                db.SaveChanges();

                // DescrObjectI[] objForms = ; //TODO-objForms
            }
            return true;
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
        public bool ChangeFavourite(string personId)
        {
            bool res = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Set<FEText>().Attach(this);
                var userFav = db.Entry(this).Collection(x1 => x1.FavouritedUser).Query().FirstOrDefault(x1 => x1.Id == personId);
                
                if (userFav != null)
                {
                    db.Entry(this).Collection(x1 => x1.FavouritedUser).Load();

                    this.FavouritedUser.Remove(userFav);
                }
                else
                {
                    var user = ApplicationUser.GetUser(personId);
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


        public static FEText Get(int id)
        {
            FEText res = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                res=db.FEText.FirstOrDefault(x1=>x1.IDFE==id);
            }

            return res;
        }

        public static FEText GetNext(int id)
        {
            FEText res = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                res = db.FEText.FirstOrDefault(x1 => x1.IDFE > id);
                if(res==null)
                    res = db.FEText.FirstOrDefault();
            }

            return res;
        }

        public static FEText GetPrev(int id)
        {
            FEText res = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //res = db.FEText.LastOrDefault(x1 => x1.IDFE < id);
                res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault(x1 => x1.IDFE < id);
                if (res == null)
                    res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault();

            }

            return res;
        }
        public static FEText GetFirst()
        {
            FEText res = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //res = db.FEText.LastOrDefault(x1 => x1.IDFE < id);
                res = db.FEText.FirstOrDefault();

            }

            return res;
        }
        public static FEText GetLast()
        {
            FEText res = null;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //res = db.FEText.LastOrDefault(x1 => x1.IDFE < id);
                res = db.FEText.OrderByDescending(x1 => x1.IDFE).FirstOrDefault();

            }

            return res;
        }

    }
}