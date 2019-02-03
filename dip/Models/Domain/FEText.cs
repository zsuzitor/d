﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

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



        [NotMapped]
        public bool? FavouritedCurrentUser { get; set; }//зафоловил ли текущий пользователь эту запись


        public ICollection<Image> Images { get; set; }

        public ICollection<ApplicationUser> FavouritedUser { get; set; }



        public FEText()
        {
            this.Images = new List<Image>();
            FavouritedUser = new List<ApplicationUser>();
            NotApprove = true;
            FavouritedCurrentUser = null;
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
            var listNM = new List<string>() { "IDFE" };//исключаем


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

        public static int[] GetByDescr(DescrSearchI inp, DescrSearchI outp)
        {
            int[] list_id = null;
            //if (DescrSearchI.Validation(inp) && DescrSearchI.Validation(outp))
            //{
            //поиск
            //List<int> list_id = new List<int>();
            inp.DeleteNotChildCheckbox();
            outp.DeleteNotChildCheckbox();
            using (var db = new ApplicationDbContext())
            {
                //TODO оптимизация? разница только в  x1.Input == 1\0
                //находим все записи которые подходят по входным параметрам
                var inp_query = db.FEActions.Where(x1 => x1.Input == 1 &&
                x1.Name == inp.ActionId &&
                  x1.Type == inp.ActionType &&
                  x1.FizVelId == inp.FizVelId &&
                  x1.Pros == inp.ListSelectedPros &&
                  x1.Spec == inp.ListSelectedSpec &&
                  x1.Vrem == inp.ListSelectedVrem &&
                  x1.FizVelSection == inp.ParametricFizVelId);

                //находим все записи которые подходят по выходным параметрам
                var out_query = db.FEActions.Where(x1 => x1.Input == 0 &&
                 x1.Name == outp.ActionId &&
                x1.Type == outp.ActionType &&
                x1.FizVelId == outp.FizVelId &&
                x1.Pros == outp.ListSelectedPros &&
                x1.Spec == outp.ListSelectedSpec &&
                x1.Vrem == outp.ListSelectedVrem &&
                x1.FizVelSection == outp.ParametricFizVelId);

                //записи которые подходят по всем параметрам
                list_id = inp_query.Join(out_query, x1 => x1.Idfe, x2 => x2.Idfe, (x1, x2) => x1.Idfe).ToArray();
                //ViewBag.listFeId = list_id;

            }
            //}
            return list_id;
        }

        //алгоритм левинштайна
        public static int[] GetByText(string text)
        {
            using (var db = new ApplicationDbContext())
            {
                System.Data.SqlClient.SqlParameter param1 = new System.Data.SqlClient.SqlParameter("@searched_str", "Затухание");
                System.Data.SqlClient.SqlParameter param2 = new System.Data.SqlClient.SqlParameter("@max_lev", 5);
                var lst = db.Database.SqlQuery<test>("SELECT * FROM GetListLev (@searched_str,@max_lev)", param1, param2).ToList();


            }

            return null;
        }


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
            if (id != null)
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

        public void GetDescrFrom(DescrSearchIInput inp = null, DescrSearchIOut outp = null)
        {
            List<FEAction> lst = null;
            using (var db = new ApplicationDbContext())
            {
                lst = db.FEActions.Where(x1 => x1.Idfe == this.IDFE).ToList();
            }
            inp = new DescrSearchIInput(lst.First(x1 => x1.Input == 1));


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

            return true;
        }

        public bool AddToDb(DescrSearchI inp, DescrSearchI outp, List<byte[]> addImgs = null)
        {
            //if (!this.Validation())
            //    return false;



            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.FEText.Add(this);
                db.SaveChanges();



                FEAction inpa = new FEAction()
                {
                    Idfe = this.IDFE,
                    Input = 1,

                };
                inpa.SetFromInput(inp);
                db.FEActions.Add(inpa);

                FEAction outpa = new FEAction()
                {
                    Idfe = this.IDFE,
                    Input = 0,

                };
                outpa.SetFromInput(outp);
                db.FEActions.Add(outpa);
                this.AddImages(addImgs, db);

                db.SaveChanges();
            }
            Lucene_.BuildIndexSolo(this);
            return true;
        }

        public bool ChangeDb(FEText new_obj, List<int> deleteImg = null, List<byte[]> addImgs = null, DescrSearchI inp = null, DescrSearchI outp = null)
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


                var descrdb = db.FEActions.Where(x1 => x1.Idfe == this.IDFE);//&&x1.Input==1
                var inpdb = descrdb.FirstOrDefault(x1 => x1.Input == 1);
                var outpdb = descrdb.FirstOrDefault(x1 => x1.Input == 0);
                if (inpdb == null || outpdb == null)
                    return false;
                inpdb.SetFromInput(inp);
                outpdb.SetFromInput(outp);

                db.SaveChanges();
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
    }
}