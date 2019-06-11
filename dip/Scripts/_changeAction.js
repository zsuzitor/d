/*файл скриптов для изменения формы при изменении воздействия
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

;;
// Функция, обновляющая характеристики воздействия
// type: I -input , O-output
function changeParams(type)
{
    var prefix = '/Actions/';

    // Получаем дескриптор воздействия
    var id = $('#action'+type).val();


            data_descr_search = {
                search: data_descr_search.search,
                function_trigger: data_descr_search.function_trigger
            };
        



var formData={
fizVelId:id,
type:type
};
goAjaxRequest({
    url: prefix + "ChangeAction",
    data: formData,
    func_success: function (req, status, jqXHR) {
        var data = req.split('<hr />');
        var type = data[0].trim();
        document.getElementById('fizVel'+type).innerHTML = data[1];

        document.getElementById('prosGroup' + type).innerHTML = data[2];

        document.getElementById('specGroup' + type).innerHTML = data[3];

        document.getElementById('vremGroup' + type).innerHTML = data[4];

        document.getElementById('parametricFizVel' + type).innerHTML = data[5];
        var massParamAction = document.getElementById('parametric_action_or_not').value.split(' ');

        //скрываем ненужные поля
        if (massParamAction.includes(id)) {
            //парам
            changeTypeActionId(type, true);
        }
        else {
            //не парам
            changeTypeActionId(type, false);
        }

    }, type: 'POST'
});

};

//функция при загрузке страницы
$(document).ready(function () {
    var i = 0;
    var inpdiv = document.getElementById('actionI' + i);
    while (inpdiv) {
        $('#actionI' + i).on('change', function () {
            var type = this.id.split('action')[1];
            changeParams(type);//'I'+i);

        });
        inpdiv = document.getElementById('actionI' + ++i);
    }
    i = 0;
    inpdiv = document.getElementById('actionO' + i);
    while (inpdiv) {
        $('#actionO' + i).on('change', function () {
            var type = this.id.split('action')[1];
            changeParams(type);//'O' + i);

        });
        inpdiv = document.getElementById('actionO' + ++i);
    }


});

;;;