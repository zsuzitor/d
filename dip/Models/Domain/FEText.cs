using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web;

namespace dip.Models.Domain
{
    [Table("FETexts")]// используется в oldDbContext.cs
    public class FEText
    {
        // [Index("PK_FeTexts_cons", IsClustered = true,IsUnique =true)]//,IsClustered =true
        //index должен быть PK_dbo.FeTexts, если менять то менять и в oldDbContext.cs
        [Key]
        public int IDFE { get; set; }
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextInp { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextOut { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextObj { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextApp { get; set; }
        [DataType(DataType.MultilineText)]
        public string TextLit { get; set; }


        public ICollection<Image> Images { get; set; }

        public FEText()
        {
            this.Images = new List<Image>();
        }

        public bool Equal (FEText a)
        {
            this.Name = a.Name;
            this.Text = a.Text;
            this.TextInp = a.TextInp;
            this.TextOut = a.TextOut;
            this.TextObj = a.TextObj;
            this.TextApp = a.TextApp;
            this.TextLit = a.TextLit;

            
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
            Text=Lucene_.ChangeForMap(Text);
            Name = Lucene_.ChangeForMap(Name);
            TextApp = Lucene_.ChangeForMap(TextApp);
            TextInp = Lucene_.ChangeForMap(TextInp);
            TextLit = Lucene_.ChangeForMap(TextLit);
            TextObj = Lucene_.ChangeForMap(TextObj);
            TextOut = Lucene_.ChangeForMap(TextOut);
            

        }


        public static List<string> GetPropTextSearch()
        {
            var res= new List<string>();
            var listNM=new List<string>() { "IDFE" };//исключаем


            PropertyInfo[] myPropertyInfo;
            Type myType = typeof(FEText);
            // Get the type and fields of FieldInfoClass.
            myPropertyInfo = myType.GetProperties();
            res = myPropertyInfo.Where(x1=>listNM.FirstOrDefault(x2=>x2== x1.Name)==null).Select(x1=>x1.Name).ToList();

            return res;
        }

        public static FEText Get(int? id){
            FEText res =null;
            if (id != null)//&&id>0
                using (var db = new ApplicationDbContext())
                    res = db.FEText.FirstOrDefault(x1 => x1.IDFE == id);
            return res;
            
        }

        public static int[] GetByDescr(DescrSearchIInput inp, DescrSearchIOut outp)
        {
            int[] list_id = null;
            if (DescrSearchIInput.Validation(inp) && DescrSearchIOut.Validation(outp))
            {
                //поиск
                //List<int> list_id = new List<int>();

                using (var db = new ApplicationDbContext())
                {
                    //находим все записи которые подходят по входным параметрам
                    var inp_query = db.FEActions.Where(x1 => x1.Input == 1 &&
                      x1.Type == inp.actionTypeI &&
                      x1.FizVelId == inp.FizVelIdI &&
                      x1.Pros == inp.listSelectedProsI &&
                      x1.Spec == inp.listSelectedSpecI &&
                      x1.Vrem == inp.listSelectedVremI);

                    //находим все записи которые подходят по выходным параметрам
                    var out_query = db.FEActions.Where(x1 => x1.Input == 0 &&
                    x1.Type == outp.actionTypeO &&
                    x1.FizVelId == outp.FizVelIdO &&
                    x1.Pros == outp.listSelectedProsO &&
                    x1.Spec == outp.listSelectedSpecO &&
                    x1.Vrem == outp.listSelectedVremO);

                    //записи которые подходят по всем параметрам
                    list_id = inp_query.Join(out_query, x1 => x1.Idfe, x2 => x2.Idfe, (x1, x2) => x1.Idfe).ToArray();
                    //ViewBag.listFeId = list_id;

                }
            }
            return list_id;
        }
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


        public static List<FEText> GetList(int[]id)
        {
            var res = new List<FEText>();
            if (id != null)
                using (var db = new ApplicationDbContext())

                    res = db.FEText.Join(id, x1 => x1.IDFE, x2 => x2, (x1, x2) => x1).ToList();
            return res;
        }


        public bool AddToDb(DescrSearchIInput inp , DescrSearchIOut outp )
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.FEText.Add(this);
                db.SaveChanges();


             

                db.FEActions.Add(new FEAction() {Idfe=this.IDFE,Input=1,Type=inp.actionTypeI, FizVelId= inp.FizVelIdI,
                    Pros = inp.listSelectedProsI, Spec= inp.listSelectedSpecI,
                    Vrem= inp.listSelectedVremI
                });

                db.FEActions.Add(new FEAction()
                {
                    Idfe = this.IDFE,
                    Input = 0,
                    Type = inp.actionTypeI,
                    FizVelId = inp.FizVelIdI,
                    Pros = inp.listSelectedProsI,
                    Spec = inp.listSelectedSpecI,
                    Vrem = inp.listSelectedVremI
                });
                db.SaveChanges();
            }
            return true;
        }

        public bool ChangeDb(FEText new_obj,List<int> deleteImg)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                this.Equal(new_obj);
                if (deleteImg != null && deleteImg.Count > 0)
                {
                    var imgs = db.Images.Where(x1 => x1.FeTextIDFE == this.IDFE).
                                            Join(deleteImg, x1 => x1.Id, x2 => x2, (x1, x2) => x1).ToList();//Where(x1 => x1.FeTextId == obj.IDFE);
                    db.Images.RemoveRange(imgs);
                    db.SaveChanges();
                }
            }
            return true;
        }
        public void AddImages(List<byte[]>imgs)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                foreach (var i in imgs)
                {
                    db.Images.Add(new Image() { Data = i, FeTextIDFE = this.IDFE });//FeTextId = oldObj.IDFE    //  FeText= oldObj
                }
                db.SaveChanges();
            }
        }


        

        }
}