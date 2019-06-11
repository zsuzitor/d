/*файл скриптов для изменения и просмотра списков
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

;;


var ListActMainObject = {
    ListId: null
};

//событие загрузки страницы
$(document).ready(function () {
    $('#ListAct_ExistList_id input:radio:checked').each(function () {
        ListActMainObject.ListId = this.id.split('ListPhysExistItem_')[1];
    });
    if (ListActMainObject.ListId) 
        LoadOneListData(ListActMainObject.ListId);
    
});

//метод для добавления нового списка
function AddNewList() {
    var formData = { name: document.getElementById('NewListName_id').value };
    if (formData.name.length < 5) {
        alert('Введите название больше 5 не пробельных символов')
        return;
    }
    goAjaxRequest({
        url: "/ListPhysics/CreateList",
        data: formData,
        func_success: function (req, status, jqXHR) {
            $('#ListAct_ExistList_id').append(req);
        }, type: 'POST'
    });

}


//событие изменения радиокнопок
$(document).on('change', ':radio', function () {

    if (this.name == 'ListPhysExistItem') {
        ListActMainObject.ListId = this.id.split('ListPhysExistItem_')[1];
        LoadOneListData(ListActMainObject.ListId);

        //список пользователей чистим
        var divlistuser = document.getElementById('UsersHasCurrentList_id');
        var butlistuser = document.getElementById('butLoadUserHasCurList');
        divlistuser.innerHTML = '';
        divlistuser.style.display = 'none';
        butlistuser.innerHTML = 'Показать пользователей';
    }
});


//метод загрузки данных одного списка
function LoadOneListData(id) {

    var formData = { id: id };
    goAjaxRequest({
        url: "/ListPhysics/LoadPhysInList",
        data: formData,
        func_success: function (req, status, jqXHR) {
            document.getElementById('PhysInCurrentList').innerHTML = req;
            document.getElementById('divEditCurrentList_id').style.display = 'block';
            document.getElementById('inputNameEditCurrentList_id').value = document.getElementById('ListHiddenName_id_' + id).value;
            document.getElementById('idCurrentList_id').innerHTML = id;

        }, type: 'GET'
    });
}


//метод для добавления записи ФЭ в текущий список
function AddNewPhysInCurList() {
    var formData = { idphys: document.getElementById('AddNewPhysInCurListInput').value };
    formData.idlist = ListActMainObject.ListId;

    if (!formData.idlist) {
        alert("Выберите список");
        return;
    }
    if (!formData.idphys) {
        alert("Введите id");
        return;
    }
    goAjaxRequest({
        url: "/ListPhysics/AddToList",
        data: formData,
        func_success: function (req, status, jqXHR) {
            if (jqXHR.status == 230) {
                alert('Запись не была добавлена, возможно она была не найдена или добавлена ранее');
            }
            else
                document.getElementById('PhysInCurrentList').innerHTML += req;

        }, type: 'POST'
    });
}


//метод для сохранения изменений текущего списка
function SaveChangesCurrentList() {
    var formData = { name: document.getElementById('inputNameEditCurrentList_id').value };
    var blockList;
    formData.id = ListActMainObject.ListId;
    goAjaxRequest({
        url: "/ListPhysics/EditList",
        data: formData,
        func_success: function (req, status, jqXHR) {
            if (req.trim()) {
                document.getElementById('ListPhysExistItemNameLabel_' + formData.id).innerHTML = formData.name;
                alert("Успешно изменено");
            }
            else {
                alert("Что то пошло не так");
            }
        }, type: 'POST'
    });
}


//метод удаления текущего списка
function DeleteCurrentList() {
    var formData = { name: document.getElementById('inputNameEditCurrentList_id').value };

    formData.id = ListActMainObject.ListId;
    goAjaxRequest({
        url: "/ListPhysics/DeleteList",
        data: formData,
        func_success: function (req, status, jqXHR) {
            if (req.trim()) {
                document.getElementById('ListPhysExistItemMainDiv_' + formData.id).remove();
                document.getElementById('divEditCurrentList_id').style.display = 'none';
                document.getElementById('PhysInCurrentList').innerHTML = '';
                ListActMainObject.ListId = null;
                var div = document.getElementById('UsersHasCurrentList_id');
                var but = document.getElementById('butLoadUserHasCurList');
                div.innerHTML = '';
                div.style.display = 'none';
                but.innerHTML = 'Показать пользователей';

            }
            else {
                alert("Что то пошло не так");
            }
        }, type: 'POST'
    });
}


//метод удаления записи ФЭ из текущего списка
function DeleteFromCurrentList(id) {

    var formData = { idphys: id };
    formData.idlist = ListActMainObject.ListId;
    goAjaxRequest({
        url: "/ListPhysics/DeleteFromList",
        data: formData,
        func_success: function (req, status, jqXHR) {
            if (req.trim()) {
                document.getElementById('oneP_onePhysInListItem_id_' + id).remove();
            }
            else {
                alert("Что то пошло не так");
            }
        }, type: 'POST'
    });
}


//метод для отображения ссылки на список
function ShowLinkFetext() {
    let link = '/Physic/Details/';
    let idfe = document.getElementById("AddNewPhysInCurListInput").value;
    link += idfe;
    let tegA = '<a href="' + link + '">Страница ФЭ</a>';
    if (idfe)
        document.getElementById('divLinkCurrFetext_id').innerHTML = tegA;
}
function clearFeLink() {
    document.getElementById('divLinkCurrFetext_id').innerHTML = '';
}



//метод для загрузки пользователей которым выдан список
function LoadUsersForCurrentList() {
    var formData = { id: ListActMainObject.ListId };
    var div = document.getElementById('UsersHasCurrentList_id');
    var but = document.getElementById('butLoadUserHasCurList');

    if (!formData.id) {
        alert("Выберите список");
        return;
    }
    if (!div.innerHTML.trim()) {
        //загрузить
        goAjaxRequest({
            url: "/ListPhysics/LoadUsersForList",
            data: formData,
            func_success: function (req, status, jqXHR) {
                div.innerHTML = req;

            }, type: 'GET'
        });
        div.style.display = 'block';
        but.innerHTML = 'Скрыть пользователей';
    }
    else {
        //загружать не надо, надо показать\скрыть
        if (div.style.display == 'block') {
            //скрыть
            div.style.display = 'none';
            but.innerHTML = 'Показать пользователей';
        }
        else {
            //показать
            div.style.display = 'block';
            but.innerHTML = 'Скрыть пользователей';
        }
    }

}


;;;