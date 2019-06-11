/*файл скриптов для изменения ролей пользователей
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

;;;
////функция удаления ссылки на пользователя
function changeUserId() {
    document.getElementById('idCurrentUser_id').innerHTML = '';
}

//функция удаления ссылки на пользователя
function clearLinkUser() {
    document.getElementById('idCurrentUser_id').innerHTML = '';
}

//функция для вывода на экран ссылки на пользователя
function showLinkCurUser() {
    let id = document.getElementById('userIdInput_id').value;
    if (id) {
        let strLink = '/Person/PersonalRecord?personId=' + id;
        document.getElementById('idCurrentUser_id').innerHTML = '<a href="' + strLink + '">Страница пользователя</a>';
    }
    else
        alert('Введите id пользователя');

}


//функция для добавления роли пользователю
function AddRole() {
    var formData = {
        userId: document.getElementById('userIdInput_id').value,
        roleName: document.getElementById('roleNameSelect_id').value
    };
    goAjaxRequest({
        url: "/Admin/AddRole",
        data: formData,
        func_success: function (req, status, jqXHR) {
            alert(req)

        }, type: 'POST'
    });
}


//функция для удаления роли у пользователя
function RemoveRole() {
    var formData = {
        userId: document.getElementById('userIdInput_id').value,
        roleName: document.getElementById('roleNameSelect_id').value
    };
    goAjaxRequest({
        url: "/Admin/RemoveRole",
        data: formData,
        func_success: function (req, status, jqXHR) {
            //alert('Роль успешно удалена')
            alert(req)
        }, type: 'POST'
    });
}
;;;;;;;;;