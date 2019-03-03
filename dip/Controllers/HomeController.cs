using dip.Models.DataBase;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

//
using System.Configuration;
using System.Data.SqlClient;
using dip.Models.Domain;
using dip.Models;
using dip.Models.ViewModel.HomeV;

namespace dip.Controllers
{
    public class HomeController : Controller
    {

        ////////List<FEObject> res = new List<FEObject>();
        public ActionResult Index()
        {
        ////////    using (var db = new ApplicationDbContext())
        ////////    {
        ////////        //db.Database.ExecuteSqlCommand(@"EXEC initFullTextSearch;");
        ////////        //db.SaveChanges();
        ////////       var test= db.FEIndexs.ToList();
        ////////        foreach(var i in test)
        ////////        {
        ////////            //if (i.IDFE == 33)
        ////////            //{
        ////////            //    int df = 3;
        ////////            //}
        ////////            i.Index= i.Index.Replace("\u0002\u0003\u0004","\n");
        ////////            var g = i.Index.Split(new string[] { "\u0000", "\u0001", "\u0002", "\u0003", "\u0004" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        ////////            if (i.IDFE == 838)//30)
        ////////            {
        ////////                var sdf = 1;
        ////////            }
        ////////            for (int i2 = 0; i2 < g.Count; ++i2)
        ////////            {
        ////////                if (g[i2] == "2"|| g[i2] == "3"|| g[i2].IndexOf("2\n")==0 || g[i2].IndexOf("3\n") == 0)//g[i2][0] == '2'|| g[i2][0] == '3'
        ////////                {
        ////////                    //характеристики начального состояния объекта

        ////////                    if (g[i2].Contains('\n'))
        ////////                    {
        ////////                        var th = g[i2].Split('\n');
        ////////                        g[i2] = th[0];
        ////////                        g.Insert(i2 + 1, th[1]);
                                
        ////////                    }
        ////////                    FEObject obj = new FEObject()
        ////////                    {
        ////////                        NumPhase = 1,
        ////////                        Begin = g[i2][0] == '2'?1:0,
        ////////                        Idfe=i.IDFE
                                
        ////////                    };
        ////////                    //obj.NumPhase = 1;
        ////////                    //obj.Begin = 1;
        ////////                    //if(g[i2][0] == '3')
        ////////                    //    obj.Begin = 2;
        ////////                    i2++;
        ////////                    //char last;
        ////////                    tttt(ref i2, g, obj, 1,i);
        ////////                }
        ////////                //if (g[i2][0] == '3')
        ////////                //{
        ////////                //    //характеристики конечного состояния объекта
        ////////                //    FEObject obj = new FEObject();
        ////////                //    obj.Begin = 0;
        ////////                //    i2++;
        ////////                //    for (; i2 < g.Length && (g[i2][0] != '3' && g[i2][0] != '4' && g[i2][0] != '5'); ++i2)
        ////////                //    {
        ////////                //        switch (g[i2][0])
        ////////                //        {
        ////////                //            case 'F':
        ////////                //                obj.PhaseState += g[i2] + " ";
        ////////                //                break;
        ////////                //            case 'X':
        ////////                //                obj.Composition += g[i2] + " ";
        ////////                //                break;
        ////////                //            case 'M':
        ////////                //                obj.MagneticStructure += g[i2] + " ";
        ////////                //                break;
        ////////                //            case 'E':
        ////////                //                obj.Conductivity += g[i2] + " ";
        ////////                //                break;
        ////////                //            case 'D':
        ////////                //                obj.MechanicalState += g[i2] + " ";
        ////////                //                break;
        ////////                //            case 'O':
        ////////                //                obj.OpticalState += g[i2] + " ";
        ////////                //                break;
        ////////                //            case 'C':
        ////////                //                obj.Special += g[i2] + " ";
        ////////                //                break;


        ////////                //        }
        ////////                //    }
        ////////                //}
        ////////            }
        ////////            if (i.IDFE == 1)//30)
        ////////            {
        ////////                var sdf = 1;
        ////////            }
        ////////        }
                
        ////////        //char asd = '\u0000';
        ////////    }

        ////////    //string Kappa = true.ToString();
        ////////    var sdgg = res.Where(x1=>x1.Composition==""&&
        ////////    x1.Conductivity == "" &&
        ////////    x1.MagneticStructure == "" &&
        ////////    x1.MechanicalState== "" &&
        ////////    x1.OpticalState== "" &&
        ////////    x1.Special== "" &&
        ////////    x1.PhaseState== "" ).ToList();

        ////////    var gggg = res.Where(x1=>x1.Idfe==838).ToList();
            return View();
        }

        ////////void tttt(ref int i2,List<string>g, FEObject obj,int numPhase,FEIndex index)
        ////////{
        ////////    if (index.IDFE == 838)//30)
        ////////    {
        ////////        var sdf = 1;
        ////////    }
        ////////    for (; i2 < g.Count&&(g[i2].Length == 0||(g[i2][0] != '4' && g[i2][0] != '5')); ++i2)//g[i2][0] != '3' && 
        ////////    {
        ////////        //if (g[i2].Length ==0)
        ////////        //    continue;
        ////////        bool slN = false;
        ////////        if (g[i2].Contains("\n"))
        ////////        {
        ////////            //переход на след фазу или переход на выход

        ////////            var th = g[i2].Split('\n');
        ////////            g[i2] = th[0];
        ////////            if ((i2 + 1) >= g.Count)
        ////////                g.Add(th[1]);
        ////////            else
        ////////            g.Insert(i2 + 1, th[1]);
        ////////            slN = true;

        ////////        }
        ////////        if(g[i2].Length!=0)
        ////////            if (g[i2] == "3" || g[i2].IndexOf("3\n") == 0) //if (g[i2] == "2" || g[i2] == "3" || g[i2].IndexOf("2\n") == 0 || g[i2].IndexOf("3\n") == 0)//g[i2][0] == '2'|| g[i2][0] == '3'
        ////////                {
        ////////            //переход на выходные характеристики
        ////////            FEObject objNext = new FEObject() { NumPhase = 1, Idfe = index.IDFE, Begin = 0 };
        ////////            i2++;
        ////////            tttt(ref i2, g, objNext, objNext.NumPhase, index);
        ////////        }
        ////////        else if(slN)//if (g[i2].Contains("\n"))
        ////////        {
        ////////            //переход на след фазу

        ////////            //var th = g[i2].Split('\n');
        ////////            //g[i2] = th[0];
        ////////            //g.Insert(i2+1, th[1]);
        ////////            //// vvvv(g[i2], obj);
        ////////            ////try
        ////////            ////{
        ////////            //if (i2 < g.Count && g[i2].Length < 1)
        ////////            //{
        ////////            //    var asd = 10;
        ////////            //}
        ////////            if (i2 < g.Count)
        ////////                vvvv(g[i2], obj);
        ////////            //}
        ////////            //catch
        ////////            //{
        ////////            //    var ggg = 1;
        ////////            //}
        ////////            FEObject objNext = new FEObject() {NumPhase=++numPhase,Idfe=index.IDFE,Begin=obj.Begin };
        ////////            i2++;
        ////////            tttt(ref i2,g, objNext, numPhase, index);
        ////////        }
        ////////        //if (i2 < g.Count && g[i2].Length < 1)
        ////////        //{
        ////////        //    var asd = 10;
        ////////        //}
        ////////        if (i2 < g.Count)
        ////////        //try
        ////////        //{
        ////////            vvvv(g[i2], obj);
        ////////        //}
        ////////        //catch
        ////////        //{
        ////////        //    var ggg = 1;
        ////////        //}
               
        ////////        //switch (g[i2][0])
        ////////        //{
        ////////        //    case 'F':
        ////////        //        obj.PhaseState += g[i2] + " ";
        ////////        //        break;
        ////////        //    case 'X':
        ////////        //        obj.Composition += g[i2] + " ";
        ////////        //        break;
        ////////        //    case 'M':
        ////////        //        obj.MagneticStructure += g[i2] + " ";
        ////////        //        break;
        ////////        //    case 'E':
        ////////        //        obj.Conductivity += g[i2] + " ";
        ////////        //        break;
        ////////        //    case 'D':
        ////////        //        obj.MechanicalState += g[i2] + " ";
        ////////        //        break;
        ////////        //    case 'O':
        ////////        //        obj.OpticalState += g[i2] + " ";
        ////////        //        break;
        ////////        //    case 'C':
        ////////        //        obj.Special += g[i2] + " ";
        ////////        //        break;


        ////////        //}
        ////////        //last = g[i2][0];
        ////////    }
        ////////    obj.Composition=obj.Composition.Trim();
        ////////    obj.Conductivity = obj.Conductivity.Trim();
        ////////    obj.MagneticStructure = obj.MagneticStructure.Trim();
        ////////    obj.MechanicalState = obj.MechanicalState.Trim();
        ////////    obj.OpticalState= obj.OpticalState.Trim();
        ////////    obj.PhaseState= obj.PhaseState.Trim();
        ////////    obj.Special = obj.Special.Trim();



        ////////    res.Add(obj);
        ////////    //if (res.Count == 136)
        ////////    //{
        ////////    //    var h = 10;
        ////////    //}
        ////////    //if(g[i2][0] == '3')
        ////////    //{
        ////////    //    //переход на выходные характеристики
        ////////    //    FEObject objNext = new FEObject() { NumPhase = ++numPhase, Idfe = index.IDFE, Begin = 0};
        ////////    //    i2++;
        ////////    //    tttt(ref i2,g, objNext,1,index);
        ////////    //}
        ////////    //if (res.Count > 130)
        ////////    //{
        ////////    //    var h = 10;
        ////////    //}
        ////////    //if (res.Count > 1100)
        ////////    //{
        ////////    //    var h = 10;
        ////////    //}
        ////////}

        ////////void vvvv(string g, FEObject obj)
        ////////{
        ////////    //if (g.Length == 0)
        ////////    //    throw new Exception();
        ////////    if(g.Length>0)
        ////////    switch (g[0])
        ////////    {
        ////////        case 'F':
        ////////            obj.PhaseState += g + " ";
        ////////            break;
        ////////        case 'X':
        ////////            obj.Composition += g + " ";
        ////////            break;
        ////////        case 'M':
        ////////            obj.MagneticStructure += g + " ";
        ////////            break;
        ////////        case 'E':
        ////////            obj.Conductivity += g + " ";
        ////////            break;
        ////////        case 'D':
        ////////            obj.MechanicalState += g + " ";
        ////////            break;
        ////////        case 'O':
        ////////            obj.OpticalState += g + " ";
        ////////            break;
        ////////        case 'C':
        ////////            obj.Special += g + " ";
        ////////            break;


        ////////    }
        ////////}



        public ActionResult About()
        {
            return View();
        }

        public ActionResult MainHeader()//string textSearchStr, string textSearchType
        {
             

            MainHeaderV res = new MainHeaderV();
            res.SearchList= new List<string>() { "lucene", "fullTextSearchF", "fullTextSearchCf", "fullTextSearchCl" };

            //ViewBag.textSearchStr = textSearchStr;
            //ViewBag.textSearchType = ViewBag;
            return PartialView(res);
        }

        public ActionResult MainFooter()
        {

            return PartialView();
        }

        public ActionResult DecriptSearch()
        {
            //TODO
            return View();
        }


        public ActionResult TextSearch()
        {
            //TODO
            return View();
        }







        [Authorize(Roles = "admin")]
        public ActionResult ReloadDataBase()
        {
            //OldData.ReloadDataBase();

            

            return View();
        }




        //TODO old method for check view
        public ActionResult CreateInput()
        {

            //ViewBag.vrem   --- mass
            //ViewBag.spec   --- mass
            //ViewBag.pros   --- mass

            //ViewBag.parametricFizVelId   ---mass


            ///ViewBag.currentAction  --str
            ///ViewBag.currentActionId  --- int

            return View();
        }

    }
}