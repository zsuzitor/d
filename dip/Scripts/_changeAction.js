/* 
Скрипт, отвечающий за обновление характеристик воздействия при изменении воздействия.
Вайнгольц Илья Игоревич (WeiLTS) © 2016
E-mail: ilyavayngolts@gmail.com
*/

// Функция, обновляющая характеристики воздействия
function changeParams()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Получаем дескриптор воздействия
    var id = $('#action').val();

    // Формируем ajax-запрос
    $.ajax(
    {
        type: 'GET',
        url: prefix + 'GetFizVels/' + id,
        success: function (fizVelId)
        {
            // Заменяем часть представления, отвечающего за выбор физической величины
            $('#fizVel').replaceWith(fizVelId);
        }
    });

   
    if (id != 'VOZ11') // непараметрическое воздействие
    {
        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetPros/' + id,
            success: function (pros)
            {
                // Заменяем часть представления, отвечающего за выбор пространственных характеристик
                $('#pros').replaceWith(pros);
            }
        });

        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetSpec/' + id,
            success: function (spec)
            {
                // Заменяем часть представления, отвечающего за выбор специальных характеристик
                $('#spec').replaceWith(spec);
            }
        });

        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetVrem/' + id,
            success: function (vrem)
            {
                // Заменяем часть представления, отвечающего за выбор временных характеристик
                $('#vrem').replaceWith(vrem);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels/' + 'NULL',
            success: function (parametricFizVel)
            {
                // Заменяем часть представления, отвечающего за выбор физической величины
                $('#parametricFizVel').replaceWith(parametricFizVel);
            }
        });
    }
    else
    {
        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels/' + 'VOZ11_FIZVEL_R1',
            success: function (parametricFizVel)
            {
                // Заменяем часть представления, отвечающего за выбор физической величины
                $('#parametricFizVel').replaceWith(parametricFizVel);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetPros/' + 'NULL',
            success: function (pros)
            {
                // Заменяем часть представления, отвечающего за выбор пространственных характеристик
                $('#pros').replaceWith(pros);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
                type: 'GET',
                url: prefix + 'GetSpec/' + 'NULL',
            success: function (spec)
            {
                // Заменяем часть представления, отвечающего за выбор специальных характеристик
                $('#spec').replaceWith(spec);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetVrem/' + 'NULL',
            success: function (vrem)
            {
                // Заменяем часть представления, отвечающего за выбор временных характеристик
                $('#vrem').replaceWith(vrem);
            }
        });
    }
};

// Назначение на событие change функции changeParams
$('#action').on('change', function ()
{
    changeParams();
});