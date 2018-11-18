/* 
Скрипт, отвечающий за обновление характеристик воздействия при изменении воздействия.
Вайнгольц Илья Игоревич (WeiLTS) © 2016
E-mail: ilyavayngolts@gmail.com
*/

// Функция, обновляющая характеристики воздействия
function changeParams(type)
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    var postfix='?type='+type+'&';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Получаем дескриптор воздействия
    var id = $('#action'+type).val();

    // Формируем ajax-запрос
    $.ajax(
    {
        type: 'GET',
        url: prefix + 'GetFizVels'+postfix+'id=' + id,
        success: function (fizVelId)
        {
            // Заменяем часть представления, отвечающего за выбор физической величины
            $('#fizVel'+type).replaceWith(fizVelId);
        }
    });

   
    if (id != 'VOZ11') // непараметрическое воздействие
    {
        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetPros'+postfix+'id=' + id,
            
            success: function (pros)
            {
                // Заменяем часть представления, отвечающего за выбор пространственных характеристик
                $('#pros'+type).replaceWith(pros);
            }
        });

        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetSpec'+postfix+'id=' + id,
            
            success: function (spec)
            {
                // Заменяем часть представления, отвечающего за выбор специальных характеристик
                $('#spec'+type).replaceWith(spec);
            }
        });

        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
           
			url: prefix + 'GetSpec'+postfix+'id=' + id,
            success: function (vrem)
            {
                // Заменяем часть представления, отвечающего за выбор временных характеристик
                $('#vrem'+type).replaceWith(vrem);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels'+postfix+'id=' + 'NULL',
            

            success: function (parametricFizVel)
            {
                // Заменяем часть представления, отвечающего за выбор физической величины
                $('#parametricFizVel'+type).replaceWith(parametricFizVel);
            }
        });
    }
    else
    {
        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels'+postfix+'id=' + 'VOZ11_FIZVEL_R1',
            
            success: function (parametricFizVel)
            {
                // Заменяем часть представления, отвечающего за выбор физической величины
                $('#parametricFizVel'+type).replaceWith(parametricFizVel);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetPros'+postfix+'id=' + 'NULL',
           
            success: function (pros)
            {
                // Заменяем часть представления, отвечающего за выбор пространственных характеристик
                $('#pros'+type).replaceWith(pros);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
                type: 'GET',
                url: prefix + 'GetSpec'+postfix+'id=' + 'NULL',
                
            success: function (spec)
            {
                // Заменяем часть представления, отвечающего за выбор специальных характеристик
                $('#spec'+type).replaceWith(spec);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetVrem'+postfix+'id=' + 'NULL',
            
            success: function (vrem)
            {
                // Заменяем часть представления, отвечающего за выбор временных характеристик
                $('#vrem'+type).replaceWith(vrem);
            }
        });
    }
};

// Назначение на событие change функции changeParams
$('#actionI').on('change', function ()
{
    changeParams('I');

});
$('#actionO').on('change', function ()
{

    changeParams('O');
});