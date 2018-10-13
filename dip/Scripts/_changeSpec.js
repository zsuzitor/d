/* 
Скрипт, добавляющий/удаляющий на/из представлени(е/я) значения выбранной специальной характеристики.
Вайнгольц Илья Игоревич (WeiLTS) © 2016
E-mail: ilyavayngolts@gmail.com
*/

// Функция, добавляющая на представление значения выбранной специальной характеристики
function setChildSpec()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Обходим все отмеченные checkbox'ы
    $('#spec input:checkbox:checked').each(function ()
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
                url: prefix + 'GetSpecChild/' + str,
                success: function (specChild)
                {
                    // Заменяем часть представления, отвечающего за отображение значений характеристики
                    $('#' + id).replaceWith(specChild);
                }
            });
        }
    });
}

// Функция, удаляющая из представления значения выбранной специальной характеристики
function delChildSpec()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Обходим все checkbox'ы
    $('#spec input:checkbox').each(function ()
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

// Назначение на событие change функций setChildSpec и delChildSpec
$('#specGroup').on('change', function ()
{
    setChildSpec();
    delChildSpec();
});