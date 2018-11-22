

// Функция, добавляющая на представление значения выбранной временной характеристики
function setChildVrem(type)
{
    //var url = window.location.pathname.split('/');
    //var prefix = '';
    //for (var i = 0; i < url.length - 1; i++)
        //prefix += url[i] + '/';
var prefix = '/Actions/';

    // Обходим все отмеченные checkbox'ы
    $('#vrem'+type+' input:checkbox:checked').each(function ()
    {
        // Получаем дескриптор характеристики
        var id = $(this).val();

        // Получаем дескриптор воздействия
        var actionId = $('#actionLabel'+type).attr('value');

        // Формируем строку для ajax-запроса
        var str = id + '@' + actionId;

        // Узнаем, загружены ли в представление характеристики значения
        var hasChild = $('#' + id+type).attr('name');

        if (hasChild == '0') // не загружены
        {
            // Формируем ajax-запрос


goAjaxRequest({url:prefix + 'GetVremChild'+postfix+'&id=' + str,
 func_success: function (vremChild)
                {
                    // Заменяем часть представления, отвечающего за отображение значений характеристики
                    $('#' + id+type).replaceWith(vremChild);
                }});



/*
            $.ajax(
                {
                    type: 'GET',
                    url: prefix + 'GetVremChild'+postfix+'&id=' + str,
                    //url: prefix + 'GetVremChild/' + str,
                success: function (vremChild)
                {
                    // Заменяем часть представления, отвечающего за отображение значений характеристики
                    $('#' + id+type).replaceWith(vremChild);
                }
            });*/
        }
    });
}

// Функция, удаляющая из представления значения выбранной временной характеристики
function delChildVrem(type)
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    var postfix='?type='+type;
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Обходим все checkbox'ы
    $('#vrem'+type+' input:checkbox').each(function ()
    {
        if ($(this).is(':checked') == false) // checkbox не отмечен
        {
            // Получаем дескриптор характеристики
            var id = $(this).val();

            // Узнаем, загружены ли в представление характеристики значения
            var hasChild = $('#' + id+type).attr('name');

            if (hasChild == '1') // загружены
            {
                // Формируем ajax-запрос


goAjaxRequest({url:prefix + 'GetEmptyChild'+postfix+'&id=' + id,
 func_success: function (emptyChild)
                    {
                        // Заменяем часть представления, отвечающего за отображение значений характеристики
                        $('#' + id+type).replaceWith(emptyChild);
                    }});


/*
                $.ajax(
                {
                    type: 'GET',
                    url: prefix + 'GetEmptyChild'+postfix+'&id=' + id,
                    //url: prefix + 'GetEmptyChild/' + id+postfix,
                    success: function (emptyChild)
                    {
                        // Заменяем часть представления, отвечающего за отображение значений характеристики
                        $('#' + id+type).replaceWith(emptyChild);
                    }
                });*/
            }
        }
    });
}

// Назначение на событие change функций setChildVrem и delChildVrem
$('#vremGroupI').on('change', function ()
{
    setChildVrem('I');
    delChildVrem('I');
});
$('#vremGroupO').on('change', function ()
{
    setChildVrem('O');
    delChildVrem('O');
});