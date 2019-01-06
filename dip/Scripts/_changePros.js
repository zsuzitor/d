

// Функция, добавляющая на представление значения выбранной пространственной характеристики
function setChildPros(type)
{
    //var url = window.location.pathname.split('/');
    //var prefix = '';
    var prefix = '/Actions/';
    var postfix='?type='+type;
    //for (var i = 0; i < url.length - 1; i++)
        //prefix += url[i] + '/';

    // Обходим все отмеченные checkbox'ы
    $('#pros'+type+' input:checkbox:checked').each(function ()
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

goAjaxRequest({url:prefix + 'GetProsChild'+postfix+'&id=' + str,
    func_success: function (data, status, jqXHR)
                {
                    // Заменяем часть представления, отвечающего за отображение значений характеристики
                    $('#' + id+type).replaceWith(data);
                }});




/*
            $.ajax(
            {
                type: 'GET',
                url: prefix + 'GetProsChild'+postfix+'&id=' + str,
                success: function (prosChild)
                {
                    // Заменяем часть представления, отвечающего за отображение значений характеристики
                    $('#' + id+type).replaceWith(prosChild);
                }
            });*/
        }
    });
}

// Функция, удаляющая из представления значения выбранной специальной характеристики
function delChildPros(type)
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    var postfix='?type='+type;
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // Обходим все checkbox'ы
    $('#pros'+type+' input:checkbox').each(function ()
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
    func_success: function (data, status, jqXHR)
                    {
                        // Заменяем часть представления, отвечающего за отображение значений характеристики
                        $('#' + id+type).replaceWith(data);
                    }});


/*
                $.ajax(
                {
                    type: 'GET',
                    url: prefix + 'GetEmptyChild'+postfix+'&id=' + id,
                    //url: prefix + 'GetEmptyChild' + id+postfix,
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

// Назначение на событие change функций setChildPros и delChildPros
$('#prosGroupI').on('change', function ()
{
    setChildPros('I');
    delChildPros('I');

});
$('#prosGroupO').on('change', function ()
{

    setChildPros('O');
    delChildPros('O');
});