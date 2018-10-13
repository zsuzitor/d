/* 
Скрипт, обновляющий физические величины для случая параметрического воздействия.
Вайнгольц Илья Игоревич (WeiLTS) © 2016
E-mail: ilyavayngolts@gmail.com
*/

// Функция, обновляющий физические величины
function changeFizVel()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Получаем дескриптор раздела физики
    var id = $('#fizVelId').val();

    if (id == 'VOZ11_FIZVEL_R1' || id == 'VOZ11_FIZVEL_R2' || // задано параметрическое воздействие
        id == 'VOZ11_FIZVEL_R3' || id == 'VOZ11_FIZVEL_R4' ||
        id == 'VOZ11_FIZVEL_R5')
    {
        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels/' + id,
            success: function (parametricFizVel)
            {
                // Заменяем часть представления, отвечающего за выбор физической величины
                $('#parametricFizVel').replaceWith(parametricFizVel);
            }
        });
    }
};

// Назначение на событие change функции changeFizVel
$('#fizVelGroup').on('change', function ()
{
    changeFizVel();
});