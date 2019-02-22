
// Функция, обновляющий физические величины
function changeFizVel(type)
{
    //var url = window.location.pathname.split('/');
    var prefix = '/Actions/';
    //var prefix = '';
    var postfix='?type='+type;
    //for (var i = 0; i < url.length - 1; i++)
        //prefix += url[i] + '/';

    // Получаем дескриптор раздела физики
    var id = $('#fizVelId'+type).val();
    //TODO надо изменить на НЕ хардкод

    var checkParametrics = document.getElementById('parametric_action_or_not').value.split(' ');
    var actionId = document.getElementById('action'+type);
    for (var i = 0; i < checkParametrics.length; ++i) {
        if (checkParametrics[i] == actionId.value) {
            goAjaxRequest({
                url: prefix + 'GetParametricFizVels/' + id + postfix,
                func_success: function (data, status, jqXHR) {
                    // Заменяем часть представления, отвечающего за выбор физической величины
                    //$('#parametricFizVel' + type).replaceWith(data);
                    document.getElementById('parametricFizVel' + type).innerHTML = data;
                }
            });
            break;
        }

    }
    //if (checkParametric.value == true) //{

    //}
    //if (id == 'VOZ11_FIZVEL_R1' || id == 'VOZ11_FIZVEL_R2' || // задано параметрическое воздействие
    //    id == 'VOZ11_FIZVEL_R3' || id == 'VOZ11_FIZVEL_R4' ||
    //    id == 'VOZ11_FIZVEL_R5')
    {


//goAjaxRequest({url:prefix + 'GetParametricFizVels/' + id+postfix,
//    func_success: function (data, status, jqXHR)
//            {
//                // Заменяем часть представления, отвечающего за выбор физической величины
//                $('#parametricFizVel'+type).replaceWith(data);
//            }});


/*
        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels/' + id+postfix,
            success: function (parametricFizVel)
            {
                // Заменяем часть представления, отвечающего за выбор физической величины
                $('#parametricFizVel'+type).replaceWith(parametricFizVel);
            }
        });*/
    }
};

// Назначение на событие change функции changeFizVel
$('#fizVelGroupI').on('change', function ()
{
    changeFizVel('I');

});
$('#fizVelGroupO').on('change', function ()
{
    changeFizVel('O');
});