using dip.Models;
using dip.Models.CustomException;
using dip.Models.Domain;
using dip.Models.ViewModel.ListPhysicsV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dip.Controllers
{
    [Authorize(Roles = "admin")]
    [RequireHttps]
    public class ListPhysicsController : Controller
    {
        // GET: ListPhysics
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Отрисовывает страницу для присвоения\удаления у пользователей списков фэ
        /// </summary>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListUserAct()
        {
            //ListActV res = new ListActV();
            //res.Lists = ListPhysics.GetAll();

            return View();
        }

        /// <summary>
        /// Отрисовывает страницу для создания\редактирования списков ФЭ
        /// </summary>
        /// <param name="currentListId"> Выбранный id списка</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult ListAct(int? currentListId = null)
        {
            ListActV res = new ListActV() { CurrentListId = currentListId };
            res.Lists = ListPhysics.GetAll();

            return View(res);
        }

        /// <summary>
        /// post-создание нового списка ФЭ
        /// </summary>
        /// <param name="name">имя нового списка</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult CreateList(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 5)//TODO в метод валидации
                return new HttpStatusCodeResult(404);//TODO сообщение об ошибке
            ListPhysics res = ListPhysics.Create(name);

            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает список всех ФЭ включенных в список
        /// </summary>
        /// <param name="id">id списка</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult LoadPhysInList(int id)
        {
            ListPhysics res = ListPhysics.LoadPhysics(id);

            return PartialView(res);
        }

        /// <summary>
        /// Отрисовывает пользователей которым выдан список
        /// </summary>
        /// <param name="id">id списка</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult LoadUsersForList(int id)
        {
            ListPhysics res = ListPhysics.LoadUsers(id);

            return PartialView(res);
        }

        /// <summary>
        /// post- редактирование списка ФЭ
        /// </summary>
        /// <param name="id">id списка</param>
        /// <param name="name">Имя списка</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult EditList(int id, string name)
        {
            ListPhysics res = ListPhysics.Edit(id, name);

            return PartialView(res);
        }

        /// <summary>
        /// post-удаление списка
        /// </summary>
        /// <param name="id">id списка</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult DeleteList(int id)
        {
            ListPhysics res = ListPhysics.Delete(id);

            return PartialView(res);
        }

        /// <summary>
        /// post-добавить ФЭ к списку ФЭ
        /// </summary>
        /// <param name="idphys">id ФЭ</param>
        /// <param name="idlist">id списка ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult AddToList(int idphys, int idlist)
        {
            ListPhysics res = ListPhysics.AddPhys(idphys, idlist);
            if (res == null)
                // return Content("-что то пошло не так(возможно запись была добавлена ранее)", "text/html");
                return new HttpStatusCodeResult(230);//, "что то пошло не так(возможно запись была добавлена ранее)");

            return PartialView(res?.Physics.FirstOrDefault(x1 => x1.IDFE == idphys));
        }
        /// <summary>
        /// post-удаление ФЭ из списка ФЭ
        /// </summary>
        /// <param name="idphys">id ФЭ</param>
        /// <param name="idlist">id списка ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult DeleteFromList(int idphys, int idlist)
        {

            ListPhysics res = ListPhysics.DeletePhys(idphys, idlist);

            return PartialView(res);
        }


        /// <summary>
        /// post-присвоить пользователю список ФЭ
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <param name="idlist">id списка ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult AssignListToUser(string iduser, int idlist)
        {
            bool? hadList;
            var res = ApplicationUser.AddList(iduser, idlist, out hadList);

            if (hadList == true)//TODO
                res = null;
            if (res == null)
                return new EmptyResult();
            return PartialView(res);
        }

        /// <summary>
        /// post-удалить у пользователя список ФЭ
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <param name="idlist">id списка ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult RemoveListFromUser(string iduser, int idlist)
        {
            ApplicationUser.RemoveList(iduser, idlist);


            return PartialView();
        }

        /// <summary>
        /// post-добавить пользователю ФЭ
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <param name="idphys">id ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult AssignPhysicToUser(string iduser, int idphys)
        {
            bool? had;
            var res = ApplicationUser.AddPhysics(iduser, idphys, out had);
            if (had != false)//TODO //had == true|| had == null
                res = null;
            if (res == null)
                return new EmptyResult();
            return PartialView(res);
        }

        /// <summary>
        /// post-удалить у пользователя ФЭ
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <param name="idphys">id ФЭ</param>
        /// <returns>результат действия ActionResult</returns>
        [HttpPost]
        public ActionResult RemovePhysicFromUser(string iduser, int idphys)
        {

            ApplicationUser.RemovePhysics(iduser, idphys);

            return PartialView();
        }


        /// <summary>
        /// Отрисовывает списки присвоенные пользователю
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult AssignsUsersList(string iduser)
        {
            var user = ApplicationUser.GetUser(iduser);
            user?.LoadListPhysics();

            return PartialView(user?.ListPhysics);
        }

        /// <summary>
        /// Отрисовывает ФЭ присвоенные пользователю
        /// </summary>
        /// <param name="iduser">id пользователя</param>
        /// <returns>результат действия ActionResult</returns>
        public ActionResult AssignsUsersPhysics(string iduser)
        {
            var user = ApplicationUser.GetUser(iduser);
            user?.LoadPhysics();

            return PartialView(user?.Physics);
        }

    }
}