;;;

//метод для удаления id текущего пользователя
function changeUserId() {
    document.getElementById('idCurrentUser_id').innerHTML = '';
}

//метод для изменения id
function ListUsersShortDataButton(id) {
    document.getElementById('userIdInput_id').value = id;
}

//метод для получения id пользователя по uname
function GetIdByUsername() {

    goAjaxRequest({
        url: "/Admin/GetUserIdByUsername",
        data: { username: document.getElementById('usernameInput_id').value },
        func_success: function (req, status, jqXHR) {
            if (req.trim()) {
                document.getElementById('userIdInput_id').value = req;
            }
            else {
                alert('Не найдено');
                document.getElementById('userIdInput_id').value = '';
            }

        }, type: 'GET'
    });
}


//метод для получения id по фамилии
function GetIdByNameSurname() {
    goAjaxRequest({
        url: "/Admin/GetUserIdByFI",
        data: { name: document.getElementById('nameOfuserInput_id').value, surname: document.getElementById('surnameOfuserInput_id').value },
        func_success: function (req, status, jqXHR) {
            let div = document.getElementById('ListFIUsers_id');
            if (req.trim()) {
                div.innerHTML = req;
                div.style.display = 'block';
            }
            else {
                alert('Не найдено');

                div.style.display = 'none';
            }

        }, type: 'GET'
    });
}



//метод для отображения ссылки на пользователя
function showLinkCurUser() {
    let id = document.getElementById('userIdInput_id').value;
    if (id) {
        let strLink = '/Person/PersonalRecord?personId=' + id;
        document.getElementById('idCurrentUser_id').innerHTML = '<a href="' + strLink + '">Страница пользователя</a>';
    }
    else
        alert('Введите id пользователя');
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




;;;;