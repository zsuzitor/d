;;

//метод для сохранения изменений учетной записи
function editPersonalRecordSave() {
    var formData = {
        'user.Name': document.getElementById('NameUser').value,
        'user.Surname': document.getElementById('SurnameUser').value,
        'user.Birthday': document.getElementById('BirthdayUser').value,
        'user.Male': document.getElementById('MaleUser').checked,
    };
    goAjaxRequest({
        url: "/Person/EditPersonalRecord",
        data: formData,
        func_success: function (req, status, jqXHR) {
            alert(req);


        }, type: 'POST'
    });
}

;;