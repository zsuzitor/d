/*
Контроллер, отвечающий за формирование нового дескрипторного описания ФЭ.
Вайнгольц Илья Игоревич(WeiLTS) © 2016
E-mail: ilyavayngolts @gmail.com
*/

using parse_feindex.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace synthesys_module.Controllers
{
    public class NewFEIndexesController : Controller
    {
        private sofiEntities db = new sofiEntities();

        /// <summary>
        /// извлечение характеристик ФЭ из дескрипторного описания
        /// </summary>
        /// <param name="feIndex"> дескрипторное описание ФЭ </param> 
        private void GetPhysicalEffect(FEIndex feIndex)
        {
            // Создаем класс ФЭ
            var effect = new NewFEIndex();
            effect.idfe = feIndex.IDFE;
            effect.beginObjectState = "";
            effect.beginPhase = "";
            effect.endObjectState = "";
            effect.endPhase = "";
            effect.input = "";
            effect.output = "";

            var pattern = @"[0-5]\u0002.*?\u0001"; // маска для выделения частей описания ФЭ

            try
            {
                // Распарсим описание частей ФЭ
                var items = Regex.Matches(feIndex.Index, pattern);

                // Распарсим каждую часть в отдельности
                foreach (var item in items)
                {
                    // Получим индекс части описания ФЭ
                    var index = int.Parse(item.ToString()[0].ToString());
                    switch (index)
                    {
                        case 0: // вход
                            // Извлекаем из общего описания входов каждый по отдельности
                            var inputs = (item.ToString().Substring(2)).Split('\u0002').ToList();

                            // Для каждого входа извлекаем характеристики воздействия
                            foreach (var input in inputs)
                            {
                                var inputId = GetAction(input, feIndex.IDFE, 1);
                                effect.input += inputId.ToString() + " ";
                            }
                            break;

                        case 1: // выход
                            // Извлекаем характеристики выходного воздействия
                            var outputId = GetAction(item.ToString(), feIndex.IDFE, 0);
                            if (outputId != -1) // воздействие не указано                             
                                effect.output = outputId.ToString();
                            break;

                        case 2: // начальное состояние объекта                   
                            // Получаем объекты для каждой фазы в отдельности
                            var objects = (item.ToString().Substring(2)).Split('\u0002').ToList();

                            // Для каждой фазы извлекаем описание объекта
                            foreach (var obj in objects)
                            {
                                var beginObjId = GetObject(obj, feIndex.IDFE, 1);
                                effect.beginObjectState += beginObjId.ToString() + " ";
                            }
                            break;

                        case 3: // конечное состояние объекта
                            // Получаем объекты для каждой фазы в отдельности
                            var endObjects = (item.ToString().Substring(2)).Split('\u0002').ToList();

                            // Для каждой фазы извлекаем описание объекта
                            foreach (var obj in endObjects)
                            {
                                var objId = GetObject(item.ToString(), feIndex.IDFE, 0);
                                effect.endObjectState += objId.ToString() + " ";
                            }
                            break;

                        case 4: // начальная фаза
                            // Извлекаем описание фазы
                            effect.beginPhase = GetPhase(item.ToString());
                            break;

                        case 5: // конечная фаза
                            // Извлекаем описание фазы
                            effect.endPhase = GetPhase(item.ToString());
                            break;
                    }
                }

                if (!(effect.beginObjectState == "" && effect.beginPhase == "" && effect.endObjectState == "" &&
                    effect.endPhase == "" && effect.input == "" && effect.output == "")) // хотя б одно поле заполнено
                {
                    if (effect.output != "") // указано выходное воздействие
                    {
                        // Добавляем эффект в БД
                        db.NewFEIndex.Add(effect);
                        db.SaveChanges();
                    }
                }
                 
            }

            catch { }
        }

        /// <summary>
        /// извлечение характеристик воздействия из дескрипторного описания
        /// </summary>
        /// <param name="description"> описание </param>
        /// <param name="idfe"> идентификатор ФЭ </param>
        /// <param name="isInput"> признак того, что воздействие является входным </param>
        /// <returns> идентификатор воздействия </returns> 
        private int GetAction(string description, int idfe, int isInput)
        {
            // Создаем класс воздействия
            var action = new FEAction();
            action.input = isInput;
            action.idfe = idfe;
            action.fizVelChange = "";
            action.fizVelId = "";
            action.fizVelLeftBorder = -0.000000001;
            action.fizVelRightBorder = -0.000000001;
            action.fizVelSection = "";
            action.name = "";
            action.pros = "";
            action.spec = "";
            action.type = "";
            action.vrem = "";

            var patternType = @"[A-Z]{3}_ACTIONS"; // маска для выделения типа воздействия
            var patternName = @"VOZ\d{1,2}"; // маска для выделения дескриптора воздействия
            var patternSpatial = @"VOZ\d{1,2}_PROS\d{1,2}"; // маска для выделения дескрипторов
                                                            // пространственных характеристик
            var patternTemporary = @"VOZ\d{1,2}_VREM\d{1,2}"; // маска для выделения дескрипторов
                                                              // временных характеристик
            var patternSpecial = @"VOZ\d{1,2}_SPEC\d{1,2}"; // маска для выделения типа дескрипторов
                                                            // специальных характеристик

            try
            {
                // Извлекаем тип воздействия
                action.type = Regex.Matches(description, patternType)[0].ToString();
            }
            catch
            { }

            try
            {
                // Извлекаем дескриптор воздействия
                action.name = Regex.Matches(description, patternName)[0].ToString();
            }
            catch
            { }

            try
            {
                // Извлекаем пространственные характеристики
                var pros = Regex.Matches(description, patternSpatial);
                var listOfPros = new List<string>();

                foreach (var item in pros)
                    listOfPros.Add(item.ToString());
                listOfPros = listOfPros.Distinct().ToList();

                foreach (var item in listOfPros)
                    action.pros += item + " ";
            }
            catch { }

            try
            {
                // Извлекаем временные характеристики
                var vrem = Regex.Matches(description, patternTemporary);
                var listOfVrem = new List<string>();

                foreach (var item in vrem)
                    listOfVrem.Add(item.ToString());
                listOfVrem = listOfVrem.Distinct().ToList();

                foreach (var item in listOfVrem)
                    action.vrem += item + " ";
            }
            catch { }

            try
            {
                // Извлекаем специальные характеристики
                var spec = Regex.Matches(description, patternSpecial);
                var listOfSpec = new List<string>();

                foreach (var item in spec)
                    listOfSpec.Add(item.ToString());
                listOfSpec = listOfSpec.Distinct().ToList();

                foreach (var item in listOfSpec)
                    action.spec += item + " ";
            }
            catch { }
       
            var patternFizVelName = @"VOZ\d{1,2}_FIZVEL_\d{1,2}|VOZ\d{1,2}_FIZVEL_R\d{1,2}_\d{1,2}"; // маска для выделения дескриптора
                                                                                                     // физической величины
            var patternFizVelSection = @"VOZ\d{1,2}_FIZVEL_R\d{1,2}"; // маска для выделения раздела физики
            var patternChange = @"CONST|DECDEG|INCDEG|DEC|INC"; // маска для выделения характера изменения

            try
            {
                // Извлекаем дескриптор физической величины
                action.fizVelId = Regex.Matches(description, patternFizVelName)[0].ToString();
            }
            catch
            { }

            try
            {
                // Извлекаем раздел физики
                action.fizVelSection = Regex.Matches(description, patternFizVelSection)[0].ToString();
            }
            catch
            {}

            try
            {
                // Извлекаем характер изменения
                action.fizVelChange = Regex.Matches(description, patternChange)[0].ToString();
            }
            catch
            { }

            if (!(action.fizVelChange == "" && action.fizVelId == "" && action.fizVelSection == "" && action.name == "" &&
                action.pros == "" && action.spec == "" && action.type == "" && action.vrem == "")) // хотя б одно поле заполнено
            {
                // Добавляем воздействие в БД
                db.FEAction.Add(action);
                db.SaveChanges();

                return db.FEAction.ToList().Last().id;
            }
            else
                return -1;         
        }

        /// <summary>
        /// извлечение характеристик объекта из дескрипторного описания
        /// </summary>
        /// <param name="description"> описание </param>
        /// <param name="idfe"> идентификатор ФЭ </param>
        /// <param name="isBegin"> признак того, что состояние является начальным </param>
        /// <returns> идентификатор состояния объекта </returns>
        private int GetObject(string description, int idfe, int isBegin)
        {
            // Создаем класс объекта
            var obj = new FEObject();
            obj.idfe = idfe;
            obj.begin = isBegin;
            obj.magneticStructure = "";
            obj.mechanicalState = "";
            obj.opticalState = "";
            obj.phaseState = "";
            obj.special = "";
            obj.composition = "";
            obj.conductivity = "";

            var patternPhaseState = @"F\d{1,2}"; // маска для выделения фазового состояния
            var patternComposition = @"X\d{1,2}"; // маска для выделения химического состава
            var patternMagneticStructure = @"M\d{1,2}"; // маска для выделения магнитной структуры
            var patternConductivity = @"E\d{1,2}"; // маска для выделения электрической проводимости
            var patternMechanicalState = @"D\d{1,2}"; // маска для выделения механического состояния
            var patternOpticalState = @"O\d{1,2}"; // маска для выделения механического состояния
            var patternSpecial = @"C\d{1,2}"; // маска для выделения специальных характеристик

            try
            {
                // Извлекаем фазовые состояния
                var phaseState = Regex.Matches(description, patternPhaseState);
                var listOfPhaseState = new List<string>();

                foreach (var item in phaseState)
                    listOfPhaseState.Add(item.ToString());

                foreach (var item in listOfPhaseState)
                    obj.phaseState += item + " ";
            }
            catch { }

            try
            {
                // Извлекаем химический состав
                var composition = Regex.Matches(description, patternComposition);
                var listOfComposition = new List<string>();

                foreach (var item in composition)
                    listOfComposition.Add(item.ToString());

                foreach (var item in listOfComposition)
                    obj.composition += item + " ";
            }
            catch { }

            try
            {
                // Извлекаем магнитную структуру
                var magneticStructure = Regex.Matches(description, patternMagneticStructure);
                var listOfMagneticStructure = new List<string>();

                foreach (var item in magneticStructure)
                    listOfMagneticStructure.Add(item.ToString());

                foreach (var item in listOfMagneticStructure)
                    obj.magneticStructure += item + " ";
            }
            catch { }

            try
            {
                // Извлекаем электрическую проводимость
                var conductivity = Regex.Matches(description, patternConductivity);
                var listOfConductivity = new List<string>();

                foreach (var item in conductivity)
                    listOfConductivity.Add(item.ToString());

                foreach (var item in listOfConductivity)
                    obj.conductivity += item + " ";
            }
            catch { }

            try
            {
                // Извлекаем механические состояния
                var mechanicalState = Regex.Matches(description, patternMechanicalState);
                var listOfMechanicalState = new List<string>();

                foreach (var item in mechanicalState)
                    listOfMechanicalState.Add(item.ToString());

                foreach (var item in listOfMechanicalState)
                    obj.mechanicalState += item + " ";

            }
            catch { }

            try
            {
                // Извлекаем оптические состояния
                var opticalState = Regex.Matches(description, patternOpticalState);
                var listOfOpticalState = new List<string>();

                foreach (var item in opticalState)
                    listOfOpticalState.Add(item.ToString());

                foreach (var item in listOfOpticalState)
                    obj.opticalState += item + " ";
            }
            catch { }

            try
            {
                // Извлекаем специальные характеристики
                var special = Regex.Matches(description, patternSpecial)[0].ToString();

                var listOfSpecial = new List<string>();

                foreach (var item in special)
                    listOfSpecial.Add(item.ToString());

                foreach (var item in listOfSpecial)
                    obj.special += item + " ";
            }
            catch
            { }

            if (!(obj.magneticStructure == "" && obj.mechanicalState == "" && obj.opticalState == "" && obj.phaseState == "" &&
                obj.special == "" && obj.composition == "" && obj.conductivity == "")) // хотя б одно поле заполнено
            {
                // Добавляем объект в БД
                db.FEObject.Add(obj);
                db.SaveChanges();

                return db.FEObject.ToList().Last().id;
            }
            else
                return -1;
        }

        /// <summary>
        /// извлечение значения фазы из дескрипторного описания
        /// </summary>
        /// <param name="description"></param>
        /// <returns> фаза объекта </returns>
        private string GetPhase(string description)
        {
            var pattern = @"2CONFAZ2|2MIXFAZ|3CONFAZ1|3MIXFAZ|MONOFAZ"; // маска для выделения дескриптора фазы
            var phase = "";

            try
            {
                // Извлекаем значение фазы
                phase = Regex.Matches(description, pattern)[0].ToString();
            }
            catch { }

            return phase;
        }

        /// <summary>
        /// GET-метод заполнения таблиц БД новым дескрипторным описанием
        /// </summary>
        /// <returns> результат действия ActionResult </returns>
        public ActionResult Fill()
        {
            // Получаем список ФЭ из БД
            var listOfFEIndex = db.FEIndex.ToList();

            // Подбираем ФЭ, которые можно подать на начало цепочки
            foreach (var feIndex in listOfFEIndex)
            {
                // Парсим описание ФЭ
                GetPhysicalEffect(feIndex);
            }

            // Получаем идентификаторы старых описаний эффектов
            var effectsIds = (from effect in listOfFEIndex.AsParallel()
                              select effect.IDFE).ToList();

            // Получаем идентификаторы новых описаний эффектов
            var baseEffectsIds = (from effect in db.NewFEIndex.AsParallel()
                                  select effect.idfe).ToList();

            // Получаем идентификаторы эффектов, не попавшие в новые таблицы
            var notBaseEffectsIds = (from effectId in effectsIds.AsParallel()
                                  where (!baseEffectsIds.Contains(effectId))
                                  select effectId).ToList();

            // Удаляем воздействия, соответствующие не попавшим в БД эффектам
            foreach (var feIndexId in notBaseEffectsIds)
            {
                // Получаем список воздействий
                var actions = (from action in db.FEAction.AsParallel()
                               where (action.idfe == feIndexId)
                               select action).ToList();

                // Удаляем воздействия
                foreach (var action in actions)
                    db.FEAction.Remove(action);
            }

            // Сохраняем изменения
            db.SaveChanges();

            // Переадресация на страницу просмотра таблицы NewFEIndex
            return RedirectToAction("Index");
        }

        /// <summary>
        /// очистка таблиц
        /// </summary>
        /// <returns> результат действия ActionResult </returns>
        public ActionResult Clear()
        {
            // Обходим таблицу эффектов и удаляем каждый элемент
            foreach (var newFEIndex in db.NewFEIndex)
                db.NewFEIndex.Remove(newFEIndex);

            // Обходим таблицу воздействий и удаляем каждый элемент
            foreach (var feAction in db.FEAction)
                db.FEAction.Remove(feAction);

            // Обходим таблицу объектов и удаляем каждый элемент
            foreach (var feObject in db.FEObject)
                db.FEObject.Remove(feObject);

            // Сохраняем изменения
            db.SaveChanges();

            // Переадресация на страницу просмотра таблицы NewFEIndex
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET-метод просмотра записей таблицы NewFEIndex
        /// </summary>
        /// <returns> результат действия ActionResult </returns>
        public ActionResult Index()
        {
            return View(db.NewFEIndex.ToList());
        }

        /// <summary>
        /// деструктор
        /// </summary>
        /// <param name="disposing"></param>
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