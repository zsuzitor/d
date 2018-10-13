/* 
Скрипт, добавляющий/удаляющий на/из представлени(е/я) значения выбранной пространственной характеристики.
Вайнгольц Илья Игоревич (WeiLTS) © 2016
E-mail: ilyavayngolts@gmail.com
*/

// Функция, добавляющая на представление значения выбранной пространственной характеристики
function setChildPros()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Обходим все отмеченные checkbox'ы
    $('#pros input:checkbox:checked').each(function ()
    {
        // Получаем дескриптор характеристики
        var id = $(this).val();

        // Получаем дескриптор воздействия
        var actionId = $('#actionLabel').attr('value');

        // Формируем строку для ajax-запроса
        var str = id + '@' + actionId;

        // Узнаем, загружены ли в представление характеристики значения
        var hasChild = $('#' + id).attr('name');

        if (hasChild == '0') // не загружены
        {
            // Формируем ajax-запрос
            $.ajax(
            {
                type: 'GET',
                url: prefix + 'GetProsChild/' + str,
                success: function (prosChild)
                {
                    // Заменяем часть представления, отвечающего за отображение значений характеристики
                    $('#' + id).replaceWith(prosChild);
                }
            });
        }
    });
}

// Функция, удаляющая из представления значения выбранной специальной характеристики
function delChildPros()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Обходим все checkbox'ы
    $('#pros input:checkbox').each(function ()
    {
        if ($(this).is(':checked') == false) // checkbox не отмечен
        {
            // Получаем дескриптор характеристики
            var id = $(this).val();

            // Узнаем, загружены ли в представление характеристики значения
            var hasChild = $('#' + id).attr('name'); 

            if (hasChild == '1') // загружены
            {
                // Формируем ajax-запрос
                $.ajax(
                {
                    type: 'GET',
                    url: prefix + 'GetEmptyChild/' + id,
                    success: function (emptyChild)
                    {
                        // Заменяем часть представления, отвечающего за отображение значений характеристики
                        $('#' + id).replaceWith(emptyChild);
                    }
                });
            }
        }
    });
}

// Назначение на событие change функций setChildPros и delChildPros
$('#prosGroup').on('change', function ()
{
    setChildPros();
    delChildPros();
});