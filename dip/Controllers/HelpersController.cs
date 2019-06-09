using dip.Models;
using dip.Models.Domain;
using dip.Models.ViewModel.HelpersV;
using dip.Models.ViewModel.PhysicV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{

    //контроллер заменяющий хелперы тк они ломаются
    [ChildActionOnly]
    [RequireHttps]
    public class HelpersController : Controller
    {
        // GET: Helpers
        public ActionResult Index()
        {
            return PartialView();
        }


        /// <summary>
        /// Отрисовывает часть дескрипторной формы
        /// </summary>
        /// <param name="a">Форма для дескрипторного поиска</param>
        /// <param name="type">Тип формы</param>
        /// <param name="param">Данные для заполнения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult PartFormDescrSearch(dip.Models.ViewModel.DescriptionForm a, string type = "", DescrSearchI param = null)
        {
            PartFormDescrSearchV res = new PartFormDescrSearchV();
            res.Form = a;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }


        //вся часть формы
        /// <summary>
        /// Полностью отрисовывает часть формы которая отвечает за состояния объекта
        /// </summary>
        /// <param name="a">Состояния</param>
        /// <param name="type">Тип</param>
        /// <param name="param">Данные для заполнения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult PartFormStateObject(List<StateObject> a, string type = "", string param = null)
        {
            //PartFormStateObjectV res = new PartFormStateObjectV();
            FormStateObjectV res = new FormStateObjectV();
            res.States = a;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }

        //только список
        /// <summary>
        /// Отрисовывает список состояний объекта
        /// </summary>
        /// <param name="a">Состояния</param>
        /// <param name="type">Тип</param>
        /// <param name="param">Данные для заполнения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListStateObject(List<StateObject> a, string type = "", string param = "")
        {
            FormStateObjectV res = new FormStateObjectV();
            res.States = a;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает 3 фазы для состояния объекта
        /// </summary>
        /// <param name="a">Фазы с данными для отображения</param>
        /// <param name="type">Тип</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult PartFormCharacteristicObject(CharacteristicObject a, string type = "")//, string param = null
        {
            PartFormCharacteristicObjectV res = new PartFormCharacteristicObjectV();
            res.Characteristic = a;
            res.Type = type;
            //res.Param = param;
            return PartialView(res);
        }



        /// <summary>
        /// Отрисовывает одну фазу
        /// </summary>
        /// <param name="a">Список пунктов в фазе</param>
        /// <param name="type">Тип</param>
        /// <param name="param">Данные для выделения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListPhaseObject(List<PhaseCharacteristicObject> a, string type = "", string param = "")
        {
            ListPhaseObjectV res = new ListPhaseObjectV();
            res.Phases = a;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }






        /// <summary>
        /// Отрисовывает часть дескрипторной формы(FizVels)
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="fizVelId">Список FizVels</param>
        /// <param name="param">Данные для выделения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormFizVels(string type, List<FizVel> fizVelId, DescrSearchI param = null)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List = fizVelId;
            res.Type = type;
            res.Param = param;

            return PartialView(res);
        }
        /// <summary>
        /// Отрисовывает часть дескрипторной формы(FizVels) для редактирования
        /// </summary>
        /// <param name="fizVelId">Список FizVels</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormFizVelsEdit(List<FizVel> fizVelId)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List = fizVelId;

            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает часть дескрипторной формы( параметрические FizVels)
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="parametricFizVelId">Список параметрических FizVels</param>
        /// <param name="param">Данные для выделения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormParamFizVels(string type, List<FizVel> parametricFizVelId, DescrSearchI param = null)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List = parametricFizVelId;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }
        /// <summary>
        /// Отрисовывает часть дескрипторной формы( параметрические FizVels) для редактирования
        /// </summary>
        /// <param name="parametricFizVelId">Список параметрических FizVels</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormParamFizVelsEdit(List<FizVel> parametricFizVelId)
        {
            DescrFormListDataV<FizVel> res = new DescrFormListDataV<FizVel>();
            res.List = parametricFizVelId;
            return PartialView(res);
        }
        /// <summary>
        /// Отрисовывает часть дескрипторной формы( Vrem) 
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="vrems">Список Vrem</param>
        /// <param name="param">Данные для выделения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormVrem(string type, List<Vrem> vrems, DescrSearchI param = null)
        {
            DescrFormListDataV<Vrem> res = new DescrFormListDataV<Vrem>();
            res.List = vrems;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает часть дескрипторной формы( Spec) 
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="specs">Список Spec</param>
        /// <param name="param">Данные для выделения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormSpec(string type, List<Spec> specs, DescrSearchI param = null)
        {
            DescrFormListDataV<Spec> res = new DescrFormListDataV<Spec>();
            res.List = specs;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает часть дескрипторной формы( Pros) 
        /// </summary>
        /// <param name="type">Тип</param>
        /// <param name="pros">Список Pros</param>
        /// <param name="param">Данные для выделения</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormPros(string type, List<Pro> pros, DescrSearchI param = null)
        {
            DescrFormListDataV<Pro> res = new DescrFormListDataV<Pro>();
            res.List = pros;
            res.Type = type;
            res.Param = param;
            return PartialView(res);
        }
        /// <summary>
        /// Отрисовывает часть дескрипторной формы( Pros)  для редактирования
        /// </summary>
        /// <param name="pros">Список Pros</param>
        /// <param name="parentId">id родительской записи</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormProsEdit(List<Pro> pros, string parentId)
        {
            DescrFormListDataV<Pro> res = new DescrFormListDataV<Pro>();
            res.List = pros;
            res.ParentId = parentId;


            return PartialView(res);
        }
        /// <summary>
        /// Отрисовывает часть дескрипторной формы( Spec)  для редактирования
        /// </summary>
        /// <param name="specs">Список Spec</param>
        /// <param name="parentId">id родительской записи</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormSpecEdit(List<Spec> specs, string parentId)
        {
            DescrFormListDataV<Spec> res = new DescrFormListDataV<Spec>();
            res.List = specs;
            res.ParentId = parentId;

            return PartialView(res);
        }
        /// <summary>
        ///  Отрисовывает часть дескрипторной формы( Vrem)  для редактирования
        /// </summary>
        /// <param name="vrems">Список Vrem</param>
        /// <param name="parentId">id родительской записи</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult DescrFormVremEdit(List<Vrem> vrems, string parentId)
        {
            DescrFormListDataV<Vrem> res = new DescrFormListDataV<Vrem>();
            res.List = vrems;
            res.ParentId = parentId;

            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает часть дескрипторной формы( состояние объекта)  для редактирования
        /// </summary>
        /// <param name="list">Список состояний</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListStateObjectEdit(List<StateObject> list)
        {

            return PartialView(list);
        }
        /// <summary>
        /// Отрисовывает часть дескрипторной формы( 1 фаза)  для редактирования
        /// </summary>
        /// <param name="list">Список итемов </param>
        /// <param name="parentId">id родительской записи</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListPhaseObjectEdit(List<PhaseCharacteristicObject> list, string parentId)
        {
            DescrFormListDataV<PhaseCharacteristicObject> res = new DescrFormListDataV<PhaseCharacteristicObject>();
            res.List = list;
            res.ParentId = parentId;
            return PartialView(res);
        }




        /// <summary>
        /// Отрисовывает изображение которое может открываться
        /// </summary>
        /// <param name="a">Изображение</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ImageLink(IShowsImage a)
        {
            ImageV res = new ImageV();
            res.Image = a;


            return PartialView(res);
        }
        /// <summary>
        /// Отрисовывает изображение
        /// </summary>
        /// <param name="a">Изображение</param>
        /// <param name="show_empty_img">нужно ли отобразить замену изображения если a==null </param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult Image(IShowsImage a, bool show_empty_img)
        {
            ImageV res = new ImageV();
            res.Image = a;
            res.ShowEmptyImage = show_empty_img;

            return PartialView(res);
        }



        /// <summary>
        /// Отрисовывает кнопку для добавления\удаления из избранного
        /// </summary>
        /// <param name="favourite">Какую именно кнопку отрисовывать</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult Favourite(bool? favourite)
        {
            return PartialView(favourite);
        }


        /// <summary>
        /// Отрисовывает список физических эффектов
        /// </summary>
        /// <param name="model"> Данные для отображения списка</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListFeText(ListFeTextV model)
        {

            return PartialView(model);
        }


        /// <summary>
        /// Отрисовывает один item(список физ эффектов) с радио для выбора
        /// </summary>
        /// <param name="item">Данные для отображения</param>
        /// <param name="select">Необходимо ли проставить radio</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListPhysItem(ListPhysics item, bool select = false)
        {
            ListPhysItemV res = new ListPhysItemV() { Select = select, ListPhysic = item };


            return PartialView(res);
        }


        /// <summary>
        /// Отрисовывает все физ эффектов которые содержатся в списке
        /// </summary>
        /// <param name="item">Список</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListPhysInItem(ListPhysics item)//,int idlist
        {
            ListPhysInItemV res = new ListPhysInItemV() { Item = item };//,Idlist=idlist 


            return PartialView(res);
        }
        /// <summary>
        /// Отрисовывает один элемент из "списка физ эффектов"
        /// </summary>
        /// <param name="item"></param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult OnePhysInListsItem(FEText item)//, int idlist
        {
            OnePhysInListsItemV res = new OnePhysInListsItemV() { Item = item };//, Idlist = idlist

            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает блок для "перелистывания ФЭ"
        /// </summary>
        /// <param name="idfe"></param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult MovementPhysicsBlock(int idfe)//,string action
        {

            return PartialView(idfe);
        }


        /// <summary>
        /// Отрисовывает список пользователей, имя+id
        /// </summary>
        /// <param name="list">Список пользователей</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListUsersShortData(List<ApplicationUser> list)//,string action
        {

            return PartialView(list);
        }

        /// <summary>
        /// Отрисовывает список пользователей, имя+id +кнопка
        /// </summary>
        /// <param name="list">Список пользователей</param>
        /// <param name="butText">Текст для кнопки</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListUsersShortDataWithButton(List<ApplicationUser> list, string butText)//,string action
        {
            ListUsersShortDataWithButtonV res = new ListUsersShortDataWithButtonV() { Users = list, ButText = butText };

            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает поля ФЭ для формы редактирования
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult FeTextInput(FEText obj)
        {
            return PartialView(obj);
        }

        /// <summary>
        /// TODO comm
        /// </summary>
        /// <param name="phasedata"></param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ObjectTextOnePhase(List<List<string>> phasedata)//, int idlist
        {

            return PartialView(phasedata);
        }

    }
}