/*файл класса предназначенного для выгрузки данных из старых БД, форматирования данных и загрузки данных в актуальную БД
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/


//#define debug

using dip.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace dip.Models.DataBase
{

    /// <summary>
    /// Класс для выгрузки данных из старых бд и  перенос их  в текущую бд
    /// </summary>
    class OldData
    {
        static SqlConnection connection;
        static SqlCommand command;

        static OldData()
        {
            connection = new SqlConnection();
            connection.ConnectionString = Constants.sql_1;
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
        } // constructor


        /// <summary>
        /// Общий Метод для выгрузки данных из старых бд и  перенос их  в текущую бд
        /// </summary>
        /// <returns>флаг успеха</returns>
        public static bool ReloadDataBase()
        {

            bool returnvalue = false;

            {
                connection.Open();

                //Pros
                try
                {
                    command.CommandText = "select * from Pros";

                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.Pro();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        obj.Parent = i["parent"].ToString().Trim();
                        using (var db = new ApplicationDbContext())
                        {
                            db.Pros.Add(obj);
                            db.SaveChanges();
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }


                //ActionTypes
                try
                {
                    command.CommandText = "select * from ActionTypes";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.ActionType();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(i["parent"].ToString().Trim()))
                            obj.Parent = null;
                        else
                            obj.Parent = i["parent"].ToString().Trim();
                        using (var db = new ApplicationDbContext())
                        {
                            db.ActionTypes.Add(obj);
                            db.SaveChanges();
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }


                //FizVels
                {
                    command.CommandText = "select * from FizVels";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.FizVel();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(i["parent"].ToString().Trim()))
                            obj.Parent = null;
                        else
                            obj.Parent = i["parent"].ToString().Trim();
                        if (obj.Parent?.Contains("_FIZVEL_R") == true)
                            obj.Parametric = true;
                        using (var db = new ApplicationDbContext())
                        {
                            db.FizVels.Add(obj);
                            db.SaveChanges();
                        }
                    }

                }




                //AllActions
                try
                {
                    command.CommandText = "select * from AllActions";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.AllAction();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        if (string.IsNullOrWhiteSpace(i["parent"].ToString().Trim()))
                            obj.Parent = null;
                        else
                            obj.Parent = i["parent"].ToString().Trim();
                        if (obj.Id == "VOZ11")
                            obj.Parametric = true;
                        using (var db = new ApplicationDbContext())
                        {
                            db.AllActions.Add(obj);
                            db.SaveChanges();
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }





                //Spec
                try
                {
                    command.CommandText = "select * from Spec";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.Spec();
                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        obj.Parent = i["parent"].ToString().Trim();

                        using (var db = new ApplicationDbContext())
                        {
                            db.Specs.Add(obj);
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }





                //Vrem
                try
                {
                    command.CommandText = "select * from Vrem";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.Vrem();

                        obj.Id = i["id"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        obj.Parent = i["parent"].ToString().Trim();

                        using (var db = new ApplicationDbContext())
                        {
                            db.Vrems.Add(obj);
                            db.SaveChanges();
                        }
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }





                //FeAction
                {
                    command.CommandText = "select * from FeAction";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "idfe", "input", "type", "name", "fizVelId",
                        "fizVelSection", "fizVelChange", "fizVelLeftBorder", "fizVelRightBorder", "pros", "spec", "vrem");
                    foreach (var i in ldr)
                    {
                        var obj = new Domain.FEAction();

                        obj.Id = Convert.ToInt32(i["id"].ToString().Trim());
                        obj.Idfe = Convert.ToInt32(i["idfe"].ToString().Trim());
                        obj.Input = Convert.ToInt32(i["input"].ToString().Trim());
                        obj.Type = i["type"].ToString().Trim();
                        obj.Name = i["name"].ToString().Trim();
                        obj.FizVelId = i["fizVelId"].ToString().Trim();
                        obj.FizVelSection = i["fizVelSection"].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(obj.FizVelSection))
                        {
                            string tmp = obj.FizVelId;
                            obj.FizVelId = obj.FizVelSection;
                            obj.FizVelSection = tmp;
                        }
                        obj.Pros = i["pros"].ToString().Trim();

                        obj.Spec = i["spec"].ToString().Trim();

                        obj.Vrem = i["vrem"].ToString().Trim();


                        //
                        using (var db = new ApplicationDbContext())
                        {
                            obj.Pros = Pro.SortIds(Pro.GetParentListForIds(obj.Pros, db));
                            obj.Spec = Spec.SortIds(Spec.GetParentListForIds(obj.Spec, db));
                            obj.Vrem = Vrem.SortIds(Vrem.GetParentListForIds(obj.Vrem, db));

                            db.FEActions.Add(obj);
                            db.SaveChanges();
                        }
                    }
                }


                //FeIndex
                List<dynamic> FeIndexList = new List<dynamic>();
                try
                {
                    command.CommandText = "select * from FeIndex";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "IDFE", "Index");
                    foreach (var i in ldr)
                    {
                        FeIndexList.Add(new
                        {
                            IDFE = Convert.ToInt32(i["IDFE"].ToString().Trim()),
                            Index = i["Index"].ToString().Trim().Replace("\u0002\u0003\u0004", "\n")
                        });

                    }

                }
                catch (Exception e)
                {
                    throw e;
                }



                //States
                using (var db = new ApplicationDbContext())
                {
                    db.StateObjects.Add(new StateObject()
                    {
                        Id = "MONOFAZ",
                        Name = "Однофазное",
                        Parent = "STRUCTOBJECT",//ALLSTATE
                        CountPhase = 1
                    });
                    db.StateObjects.Add(new StateObject()
                    {
                        Id = "POLYFAZ",
                        Name = "Многофазное",
                        Parent = "STRUCTOBJECT"

                    });
                    db.SaveChanges();
                }
                //state phase
                LoadStateObject("MONOFAZ");
                LoadStateObject("POLYFAZ");

                LoadCharacteristicObject(Constants.FeObjectBaseCharacteristic);




                //FeObject
                using (var db = new ApplicationDbContext())
                {
                    foreach (var i in FeIndexList)
                    {
                        var indexMass = ((string[])i.Index.Split(new string[] { "\u0000", "\u0001", "\u0002", "\u0003", "\u0004" }, StringSplitOptions.RemoveEmptyEntries)).ToList();
                        for (int i2 = 0; i2 < indexMass.Count; ++i2)
                        {
                            if (indexMass[i2] == "2" || indexMass[i2] == "3" || indexMass[i2].IndexOf("2\n") == 0 || indexMass[i2].IndexOf("3\n") == 0)
                            {
                                //характеристики начального состояния объекта

                                if (indexMass[i2].Contains('\n'))
                                {
                                    var th = indexMass[i2].Split('\n');
                                    indexMass[i2] = th[0];
                                    indexMass.Insert(i2 + 1, th[1]);

                                }
                                FEObject obj = new FEObject()
                                {
                                    NumPhase = 1,
                                    Begin = indexMass[i2][0] == '2' ? 1 : 0,
                                    Idfe = i.IDFE

                                };
                                i2++;
                                FeObjectParseStep(ref i2, indexMass, obj, 1, i, db);
                            }
                        }
                    }
                }




                //NewFeIndex


                List<dynamic> NewFeIndexList = new List<dynamic>();
                try
                {
                    command.CommandText = "select * from NewFeIndex";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "idfe", "input", "output", "beginObjectState", "endObjectState",
                       "beginPhase", "endPhase");
                    foreach (var i in ldr)
                    {
                        NewFeIndexList.Add(new
                        {
                            BeginPhase = i["beginPhase"].ToString().Trim(),
                            EndPhase = i["endPhase"].ToString().Trim(),
                            Idfe = Convert.ToInt32(i["idfe"].ToString().Trim())
                        });
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }





                //FeText
                {
                    command.CommandText = "select * from FeText";
                    var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "IDFE", "name", "text", "textInp", "textOut", "textObj",
                        "textApp", "textLit");
                    var listFetext = new List<FEText>();
                    using (var db = new ApplicationDbContext())
                    {
                        foreach (var i in ldr)
                        {
                            var obj = new Domain.FEText();

                            obj.IDFE = Convert.ToInt32(i["IDFE"].ToString().Trim());

                            obj.Name = i["name"].ToString().Trim();
                            obj.Text = i["text"].ToString().Trim();
                            obj.TextInp = i["textInp"].ToString().Trim();
                            obj.TextOut = i["textOut"].ToString().Trim();
                            obj.TextObj = i["textObj"].ToString().Trim();
                            obj.TextApp = i["textApp"].ToString().Trim();
                            obj.TextLit = i["textLit"].ToString().Trim();
                            obj.NotApprove = false;


                            listFetext.Add(obj);
                            var tmpinp = db.FEActions.Where(x1 => x1.Idfe == obj.IDFE).ToList();
                            switch (tmpinp.Count)
                            {
                                case 0://вообще нет добавить 1 вход 1 выход
                                    {
                                        obj.CountInput = 1;
                                        FEAction feactinp = new FEAction() { Idfe = obj.IDFE, Input = 1, Name = "VOZ1", Type = "NO_ACTIONS", FizVelId = "NO_FIZVEL" };
                                        db.FEActions.Add(feactinp);
                                        FEAction feactoutp = new FEAction() { Idfe = obj.IDFE, Input = 0, Name = "VOZ1", Type = "NO_ACTIONS", FizVelId = "NO_FIZVEL" };
                                        db.FEActions.Add(feactoutp);
                                        db.SaveChanges();
                                        break;
                                    }
                                case 1://что то есть добавить то чего нет
                                    obj.CountInput = 1;
                                    if (tmpinp.FirstOrDefault(x1 => x1.Input == 1) != null)
                                    {
                                        FEAction feactoutp = new FEAction() { Idfe = obj.IDFE, Input = 0, Name = "VOZ1", Type = "NO_ACTIONS", FizVelId = "NO_FIZVEL" };
                                        db.FEActions.Add(feactoutp);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        FEAction feactoutp = new FEAction() { Idfe = obj.IDFE, Input = 1, Name = "VOZ1", Type = "NO_ACTIONS", FizVelId = "NO_FIZVEL" };
                                        db.FEActions.Add(feactoutp);
                                        db.SaveChanges();
                                    }
                                    break;
                                case 2://1 вход и 1 выход
                                    obj.CountInput = 1;
                                    break;
                                case 3://2 вход и 1 выход
                                    obj.CountInput = 2;
                                    break;
                            }
                        }

                        //восстанавливаем бывшие id
                        listFetext = listFetext.OrderBy(x1 => x1.IDFE).ToList();
                        foreach (var i in listFetext)
                        {
                            int tmpId = i.IDFE;
                            db.FEText.Add(i);
                            db.SaveChanges();


                            while (tmpId != i.IDFE)
                            {
                                db.FEText.Remove(i);
                                db.SaveChanges();
                                db.FEText.Add(i);
                                db.SaveChanges();
                            }

                            if (string.IsNullOrWhiteSpace(i.StateBeginId))
                            {
                                i.StateBeginId = "MONOFAZ";
                            }
                            if (i.IDFE == 1282)
                            {
                                i.StateEndId = "MONOFAZ";
                            }
                            if (i.IDFE == Constants.FEIDFORSEMANTICSEARCH)
                            {
                                db.SaveChanges();
                                continue;
                            }

                            var tmpphase = db.FEObjects.Where(x1 => x1.Idfe == i.IDFE && x1.Begin == 0).Count();
                            if (tmpphase != 0)
                                i.ChangedObject = true;

                            var tmpNewIndex = NewFeIndexList.FirstOrDefault(x1 => x1.Idfe == i.IDFE);
                            if (tmpNewIndex != null)
                            {
                                i.StateBeginId = tmpNewIndex.BeginPhase;
                                if (!string.IsNullOrEmpty(tmpNewIndex.EndPhase))
                                    i.StateEndId = tmpNewIndex.EndPhase;
                            }



                            var tmpobj = db.FEObjects.Where(x1 => x1.Idfe == i.IDFE).ToList();
                            int allcount = 0;
                            {
#if debug
                                if (i.IDFE == 801 || i.IDFE == 809)
                                {
                                    var g = 10;
                                }
#endif                   
                                switch (i.StateBeginId)
                                {
                                    case "MONOFAZ":
                                        allcount++;
                                        break;
                                    case "2CONFAZ1":
                                        allcount += 2;
                                        break;
                                    case "2MIXFAZ":
                                        allcount += 2;
                                        break;
                                    case "2CONFAZ2":
                                        allcount += 2;
                                        break;
                                    case "3CONFAZ1":
                                        allcount += 3;
                                        break;
                                    case "3MIXFAZ":
                                        allcount += 3;
                                        break;
                                }
                                var beginobj = tmpobj.Where(x1 => x1.Begin == 1).ToList();
                                int difbeginobj = allcount - beginobj.Count;
                                if (difbeginobj > 0)
                                {
                                    List<int> free = new List<int>();
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 1) == null)
                                        free.Add(1);
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 2) == null)
                                        free.Add(2);
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 3) == null)
                                        free.Add(3);

                                    for (var it = 0; it < difbeginobj; ++it)
                                    {
                                        FEObject obj = new FEObject() { Idfe = i.IDFE, Begin = 1, NumPhase = free[it] };
                                        db.FEObjects.Add(obj);
                                        db.SaveChanges();
                                    }

                                }
#if debug
                                if (difbeginobj < 0)
                                {
                                    var error = 10;
                                }
#endif
                            }
                            allcount = 0;

                            {
                                switch (i.StateEndId)
                                {
                                    case "MONOFAZ":
                                        allcount++;
                                        break;
                                    case "2CONFAZ1":
                                        allcount += 2;
                                        break;
                                    case "2MIXFAZ":
                                        allcount += 2;
                                        break;
                                    case "2CONFAZ2":
                                        allcount += 2;
                                        break;
                                    case "3CONFAZ1":
                                        allcount += 3;
                                        break;
                                    case "3MIXFAZ":
                                        allcount += 3;
                                        break;
                                }
                                var beginobj = tmpobj.Where(x1 => x1.Begin == 0).ToList();
                                int difbeginobj = allcount - beginobj.Count;
                                if (difbeginobj > 0)
                                {
                                    List<int> free = new List<int>();
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 1) == null)
                                        free.Add(1);
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 2) == null)
                                        free.Add(2);
                                    if (beginobj.FirstOrDefault(x1 => x1.NumPhase == 3) == null)
                                        free.Add(3);

                                    for (var it = 0; it < difbeginobj; ++it)
                                    {
                                        FEObject obj = new FEObject() { Idfe = i.IDFE, Begin = 0, NumPhase = free[it] };
                                        db.FEObjects.Add(obj);
                                        db.SaveChanges();
                                    }

                                }
#if debug
                                if (difbeginobj < 0)
                                {
                                    var error = 10;
                                }
#endif
                            }

                            db.SaveChanges();
                        }

                        //id этой записи должно быть == Models.Constants.FEIDFORSEMANTICSEARCH
                        var objsemantic = new Domain.FEText()
                        {
                            Deleted = true,
                            Name = "Временная запись для семантического поиска",
                            Text = Models.Constants.FeSemanticNullText,
                            TextInp = "",
                            TextOut = "",
                            TextObj = "",
                            TextApp = "",
                            TextLit = "",
                            CountInput = 0
                        };
                        db.FEText.Add(objsemantic);
                        db.SaveChanges();
                    }
                }

                returnvalue = true;

                connection.Close();
                command.Dispose();

            }

            return returnvalue;

        }




        /// <summary>
        /// Метод для загрузки состояний объекта
        /// </summary>
        /// <param name="id">id состояния для которого нужно найти детей</param>
        public static void LoadStateObject(string id)
        {
            try
            {
                command.CommandText = $"select * from Thes where parent ='{id}'";
                var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                foreach (var i in ldr)
                {
                    var obj = new Domain.StateObject();

                    obj.Id = i["id"].ToString().Trim();
                    obj.Name = i["name"].ToString().Trim();
                    obj.Parent = i["parent"].ToString().Trim();
                    if (obj.Id == "3CONFAZ1" || obj.Id == "3MIXFAZ")
                        obj.CountPhase = 3;
                    if (obj.Id == "2CONFAZ1" || obj.Id == "2CONFAZ2" || obj.Id == "2MIXFAZ")
                        obj.CountPhase = 2;

                    using (var db = new ApplicationDbContext())
                    {
                        db.StateObjects.Add(obj);
                        db.SaveChanges();
                    }
                    LoadStateObject(obj.Id);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Метод для загрузки характеристик объекта
        /// </summary>
        /// <param name="id">id характеристики для которого нужно найти детей</param>
        public static void LoadCharacteristicObject(string id)
        {
            try
            {
                command.CommandText = $"select * from Thes where parent = '{id}'";
                var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "id", "name", "parent");
                foreach (var i in ldr)
                {
                    var obj = new Domain.PhaseCharacteristicObject();

                    obj.Id = i["id"].ToString().Trim();
                    obj.Name = i["name"].ToString().Trim();
                    obj.Parent = i["parent"].ToString().Trim();

                    using (var db = new ApplicationDbContext())
                    {
                        db.PhaseCharacteristicObjects.Add(obj);
                        db.SaveChanges();
                    }
                    LoadCharacteristicObject(obj.Id);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Метод для парсинга записи feobject
        /// </summary>
        /// <param name="i2">Индекс</param>
        /// <param name="indexMass">Массив строк для парсинга</param>
        /// <param name="obj"> Запись для парсинга</param>
        /// <param name="numPhase">Номер фазы</param>
        /// <param name="index"></param>
        /// <param name="db">Контекст</param>
        static void FeObjectParseStep(ref int i2, List<string> indexMass, FEObject obj, int numPhase, dynamic index, ApplicationDbContext db)
        {
            for (; i2 < indexMass.Count && (indexMass[i2].Length == 0 || (indexMass[i2][0] != '4' && indexMass[i2][0] != '5')); ++i2)//g[i2][0] != '3' && 
            {
                bool slN = false;
                if (indexMass[i2].Contains("\n"))
                {
                    //переход на след фазу или переход на выход

                    var th = indexMass[i2].Split('\n');
                    indexMass[i2] = th[0];
                    if ((i2 + 1) >= indexMass.Count)
                        indexMass.Add(th[1]);
                    else
                        indexMass.Insert(i2 + 1, th[1]);
                    slN = true;

                }
                if (indexMass[i2].Length != 0)
                    if (indexMass[i2] == "3" || indexMass[i2].IndexOf("3\n") == 0)
                    {
                        //переход на выходные характеристики
                        FEObject objNext = new FEObject() { NumPhase = 1, Idfe = index.IDFE, Begin = 0 };
                        i2++;
                        FeObjectParseStep(ref i2, indexMass, objNext, objNext.NumPhase, index, db);
                    }
                    else if (slN)
                    {
                        //переход на след фазу
                        if (i2 < indexMass.Count)
                            FeObjectParseAddValueInObj(indexMass[i2], obj);

                        FEObject objNext = new FEObject() { NumPhase = ++numPhase, Idfe = index.IDFE, Begin = obj.Begin };
                        i2++;
                        FeObjectParseStep(ref i2, indexMass, objNext, numPhase, index, db);
                    }

                if (i2 < indexMass.Count)

                    FeObjectParseAddValueInObj(indexMass[i2], obj);

            }



            obj.Composition = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.GetParentListForIds(obj.Composition.Trim(), db));
            obj.Conductivity = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.GetParentListForIds(obj.Conductivity.Trim(), db));
            obj.MagneticStructure = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.GetParentListForIds(obj.MagneticStructure.Trim(), db));
            obj.MechanicalState = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.GetParentListForIds(obj.MechanicalState.Trim(), db));
            obj.OpticalState = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.GetParentListForIds(obj.OpticalState.Trim(), db));
            obj.PhaseState = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.GetParentListForIds(obj.PhaseState.Trim(), db));
            obj.Special = PhaseCharacteristicObject.SortIds(PhaseCharacteristicObject.GetParentListForIds(obj.Special.Trim(), db));



            db.FEObjects.Add(obj);
            db.SaveChanges();

        }



        /// <summary>
        /// Добавление данных в нужный массив
        /// </summary>
        /// <param name="g"></param>
        /// <param name="obj"></param>
        static void FeObjectParseAddValueInObj(string g, FEObject obj)
        {

            if (g.Length > 0)
                switch (g[0])
                {
                    case 'F':
                        obj.PhaseState += g + " ";
                        break;
                    case 'X':
                        obj.Composition += g + " ";
                        break;
                    case 'M':
                        obj.MagneticStructure += g + " ";
                        break;
                    case 'E':
                        obj.Conductivity += g + " ";
                        break;
                    case 'D':
                        obj.MechanicalState += g + " ";
                        break;
                    case 'O':
                        obj.OpticalState += g + " ";
                        break;
                    case 'C':
                        obj.Special += g + " ";
                        break;


                }
        }




        /// <summary>
        /// Метод для добавления базовых дескрипторов для записей у которых их нет
        /// </summary>
        /// <param name="db">контекст бд</param>
        public static void FixOldFeRecord(ApplicationDbContext db)
        {
            command.CommandText = @"select FEObject_.IDFE as idfe1,FEAction_.IDFE as idfe2 from (select FEText.IDFE from dbo.FEText
                left join dbo.FEObject
                on FEText.IDFE = FEObject.Idfe
                where FEObject.Id is null) as FEObject_
                full join (select FEText.IDFE from dbo.FEText where FEText.IDFE not in (select Idfe  from dbo.FEAction)) as FEAction_
                on FEObject_.IDFE = FEAction_.IDFE";

            var ldr = Models.DataBase.DataBase.ExecuteQuery(null, command, "idfe1", "idfe2");
            foreach (var i in ldr)
            {
                string idfe1 = i["idfe1"].ToString().Trim();
                string idfe2 = i["idfe2"].ToString().Trim();
                //TODO поменять на базовые значние, 1282 не должен заходить в 1 условие
#if debug
                if (idfe1 == "1282" || idfe2 == "1282")
                {
                    var g = 10;
                }
#endif
                if (!string.IsNullOrWhiteSpace(idfe1))
                {
                    int intid = int.Parse(idfe1);
                    if (intid == Constants.FEIDFORSEMANTICSEARCH)
                        continue;
                    //установить начальное состояние, добавить дескрипторы и объект
                    FEText fe = db.FEText.First(x1 => x1.IDFE == intid);
                    fe.StateBeginId = "MONOFAZ";
                    db.SaveChanges();
                    FEAction feactinp = new FEAction() { Idfe = fe.IDFE, Input = 1 };
                    db.FEActions.Add(feactinp);
                    FEAction feactoutp = new FEAction() { Idfe = fe.IDFE, Input = 0 };
                    db.FEActions.Add(feactoutp);
                    db.SaveChanges();
                    FEObject obj = new FEObject() { Idfe = fe.IDFE, Begin = 1, NumPhase = 1 };
                    db.FEObjects.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    int intid = int.Parse(idfe1);
                    FEText fe = db.FEText.First(x1 => x1.IDFE == intid);
                    fe.StateBeginId = "2CONFAZ1";
                    db.SaveChanges();
                    FEAction feactinp = new FEAction() { Idfe = fe.IDFE, Input = 1 };
                    db.FEActions.Add(feactinp);
                    FEAction feactoutp = new FEAction() { Idfe = fe.IDFE, Input = 0 };
                    db.FEActions.Add(feactoutp);
                    db.SaveChanges();
                }
            }
        }
    }
}