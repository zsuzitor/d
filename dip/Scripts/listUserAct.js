;;



//метод для отображения присвоенных списков
function ShowAssignList() {
    document.getElementById('AssignsList_id').style.display = 'block';

    document.getElementById('AssignsPhys_id').style.display = 'none';
    changeTypeButton('buttonChangeViewHList', 'buttonChangeViewHPhys');

    var formData = { iduser: document.getElementById('currentUserId_id').value };
    if (!formData.iduser.trim())
        alert('Введите id пользователя');
    goAjaxRequest({
        url: "/ListPhysics/AssignsUsersList",
        data: formData,
        func_success: function (req, status, jqXHR) {
            document.getElementById('AssignsList_id').innerHTML = req;

        }, type: 'GET'
    });


}

//метод для отображения присвоенных записей ФЭ
function ShowAssignPhys() {
    document.getElementById('AssignsList_id').style.display = 'none';

    document.getElementById('AssignsPhys_id').style.display = 'block';
    changeTypeButton('buttonChangeViewHPhys', 'buttonChangeViewHList');
    var formData = { iduser: document.getElementById('currentUserId_id').value };
    if (!formData.iduser.trim())
        alert('Введите id пользователя');
    goAjaxRequest({
        url: "/ListPhysics/AssignsUsersPhysics",
        data: formData,
        func_success: function (req, status, jqXHR) {
            document.getElementById('AssignsPhys_id').innerHTML = req;

        }, type: 'GET'
    });
}


//метод для изменения визуального отображения кнопки
//idna-id кнопки списка
//ida-id кнопки ФЭ
function changeTypeButton(idna, ida) {
    var butlist = document.getElementById(idna);
    butlist.classList.remove('btn-default');
    butlist.classList.add('btn-primary');

    var butphys = document.getElementById(ida);
    butphys.classList.add('btn-default');
    butphys.classList.remove('btn-primary');
}

//метод для того что бы сделать видимым блок для присвоения списков
function ShowNotAssignList() {
    changeTypeButton('buttonChangeViewNHList', 'buttonChangeViewNHPhys');
    var butlist = document.getElementById('ForAssignsList_id');
    butlist.style.display = 'block';
    var butlist = document.getElementById('ForAssignsPhys_id');
    butlist.style.display = 'none';

}

//метод для того что бы сделать видимым блок для присвоения записей ФЭ
function ShowNotAssignPhys() {
    changeTypeButton('buttonChangeViewNHPhys', 'buttonChangeViewNHList');
    var butlist = document.getElementById('ForAssignsPhys_id');
    butlist.style.display = 'block';
    var butlist = document.getElementById('ForAssignsList_id');
    butlist.style.display = 'none';


}

//метод для присвоения списка пользователю
function AssignListToUser() {

    var formData = { iduser: document.getElementById('currentUserId_id').value, idlist: document.getElementById('newAssignList').value };
    if (!formData.idlist) {
        alert('Введите id');
        return;
    }
    goAjaxRequest({
        url: "/ListPhysics/AssignListToUser",
        data: formData,
        func_success: function (req, status, jqXHR) {
            var blockLists = document.getElementById('listListsFeExist');
            if (blockLists) {
                if (req.trim())
                    blockLists.innerHTML += req;
                else
                    alert('Что то пошло не так, возможно список уже присвоен');
            }
        }, type: 'POST'
    });
}

//метод для присвоения записи ФЭ пользователю
function AssignPhysicToUser() {
    var formData = { iduser: document.getElementById('currentUserId_id').value, idphys: document.getElementById('newAssignPhys').value };
    if (!formData.idphys) {
        alert('Введите id');
        return;
    }
    goAjaxRequest({
        url: "/ListPhysics/AssignPhysicToUser",
        data: formData,
        func_success: function (req, status, jqXHR) {
            var blockfes = document.getElementById('divAllFetextExist_id');
            if (blockfes) {
                if (req.trim())
                    blockfes.innerHTML += req;
                else
                    alert('Что то пошло не так, возможно ФЭ уже присвоен');
            }
        }, type: 'POST'
    });
}


//метод для отображения ссылки на запись ФЭ
function ShowLinkFetext() {
    let link = '/Physic/Details/';
    let idfe = document.getElementById("newAssignPhys").value;
    link += idfe;
    let tegA = '<a href="' + link + '">Страница ФЭ</a>';
    if (idfe)
        document.getElementById('divLinkCurrFetext_id').innerHTML = tegA;
}

//метод для удаления ссылки на запись ФЭ
function clearFeLink() {
    document.getElementById('divLinkCurrFetext_id').innerHTML = '';
}

//метод для отображения ссылки на список ФЭ
function ShowLinkListFe() {
    let link = '/ListPhysics/ListAct?currentListId=';
    let idfe = document.getElementById("newAssignList").value;
    link += idfe;
    let tegA = '<a href="' + link + '">Страница просмотра списка</a>';
    if (idfe)
        document.getElementById('divLinkCurrListFe_id').innerHTML = tegA;
}

//метод для удаления ссылки на список
function clearListFeLink() {
    document.getElementById('divLinkCurrListFe_id').innerHTML = '';
}

//метод для отображения ссылки на страницу ФЭ
function ShowLinkFetextExist() {
    let link = '/Physic/Details/';
    let idfe = document.getElementById("AssignPhysForDel").value;
    link += idfe;
    let tegA = '<a href="' + link + '">Страница ФЭ</a>';
    if (idfe)
        document.getElementById('divLinkCurrFetextExist_id').innerHTML = tegA;
}

//метод для удаления ссылки на ФЭ
function clearFeExistLink() {
    document.getElementById('divLinkCurrFetextExist_id').innerHTML = '';
}

//метод для отображения ссылки на список 
function ShowLinkListFeExist() {
    let link = '/ListPhysics/ListAct?currentListId=';
    let idfe = document.getElementById("AssignListForDel").value;
    link += idfe;
    let tegA = '<a href="' + link + '">Страница просмотра списка</a>';
    if (idfe)
        document.getElementById('divLinkCurrListFeExist_id').innerHTML = tegA;
}

//метод для очистки ссылки на список
function clearListFeLinkExist() {
    document.getElementById('divLinkCurrListFeExist_id').innerHTML = '';
}

//метод для удаления списка у пользователя
function removeListFromUser(id) {
    if (!id)
        id = document.getElementById('AssignListForDel').value;

    if (!id) {
        alert('Не выбран список');
        return;
    }

    var formData = { iduser: document.getElementById('currentUserId_id').value, idlist: id };

    goAjaxRequest({
        url: "/ListPhysics/RemoveListFromUser",
        data: formData,
        func_success: function (req, status, jqXHR) {
            if (req.trim()) {
                document.getElementById('divOneListFeExist_' + id).remove();
            }
        }, type: 'POST'
    });

}

//метод для удаления записи ФЭ у пользователя
function removePhysFromUser(id) {
    if (!id)
        id = document.getElementById('AssignPhysForDel').value;

    if (!id) {
        alert('Не выбран ФЭ');
        return;
    }

    var formData = { iduser: document.getElementById('currentUserId_id').value, idphys: id };

    goAjaxRequest({
        url: "/ListPhysics/RemovePhysicFromUser",
        data: formData,
        func_success: function (req, status, jqXHR) {
            if (req.trim()) {
                document.getElementById('divOnePhysExist_' + id).remove();
            }
        }, type: 'POST'
    });
}


//метод для изменения id текущего пользователя
function ListUsersShortDataButton(id) {
    document.getElementById('currentUserId_id').value = id;
}

//метод для загрузки всех пользователей
function LoadAllUsers() {
    var formData = {};
    var div = document.getElementById('ListAllUsers_id');
    var but = document.getElementById('butLoadAllUsers');
    if (!div.innerHTML.trim()) {
        //загрузить
        goAjaxRequest({
            url: "/Admin/GetAllUsersShortDataWithBut",
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
            but.innerHTML = 'Показать всех пользователей';
        }
        else {
            //показать
            div.style.display = 'block';
            but.innerHTML = 'Скрыть пользователей';
        }
    }
}






;;;