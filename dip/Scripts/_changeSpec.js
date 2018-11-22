

// Функция, добавляющая на представление значения выбранной специальной характеристики
function setChildSpec(type)
{
    //var url = window.location.pathname.split('/');
    //var prefix = '';
    var postfix='?type='+type;
    var prefix = '/Actions/';
    //for (var i = 0; i < url.length - 1; i++)
        //prefix += url[i] + '/';

    // Обходим все отмеченные checkbox'ы
    $('#spec'+type+' input:checkbox:checked').each(function ()
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



goAjaxRequest({url:prefix + 'GetSpecChild'+postfix+'&id=' + str,
 func_success: function (specChild)
                {
                    // Заменяем часть представления, отвечающего за отображение значений характеристики
                    $('#' + id+type).replaceWith(specChild);
                }});





/*
            // Формируем ajax-запрос
            $.ajax(
            {
                type: 'GET',
                url: prefix + 'GetSpecChild'+postfix+'&id=' + str,
                //url: prefix + 'GetSpecChild/' + str+postfix,
                success: function (specChild)
                {
                    // Заменяем часть представления, отвечающего за отображение значений характеристики
                    $('#' + id+type).replaceWith(specChild);
                }
            });*/
        }
    });
}

// Функция, удаляющая из представления значения выбранной специальной характеристики
function delChildSpec(type)
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    var postfix='?type='+type;
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Обходим все checkbox'ы
    $('#spec'+type+' input:checkbox').each(function ()
    {
        if ($(this).is(':checked') == false) // checkbox не отмечен
        {
            // Получаем дескриптор характеристики
            var id = $(this).val();

            // Узнаем, загружены ли в представление характеристики значения
            var hasChild = $('#' + id+type).attr('name');

            if (hasChild == '1') // загружены
            {


goAjaxRequest({url:prefix + 'GetEmptyChild'+postfix+'&id=' + id,
 func_success: function (emptyChild)
                    {
                        // Заменяем часть представления, отвечающего за отображение значений характеристики
                        $('#' + id+type).replaceWith(emptyChild);
                    }});


/*
                // Формируем ajax-запрос
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

// Назначение на событие change функций setChildSpec и delChildSpec
$('#specGroupI').on('change', function ()
{
    setChildSpec('I');
    delChildSpec('I');

});
$('#specGroupO').on('change', function ()
{

    setChildSpec('O');
    delChildSpec('O');
});