/* 
Скрипт, обновляющий структуры ФПД в случае изменения ранга.
Вайнгольц Илья Игоревич (WeiLTS) © 2016
E-mail: ilyavayngolts@gmail.com
*/

// Функция, обновляющая структуры ФПД в случае изменения ранга
function loadChain()
{
	var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 2; i++)
        prefix += url[i] + '/';
	
    // Извлечение идентификатора задания на синтез из строки браузера
    var str = window.location.href.split('/');
    var task = str[str.length - 1];

    // Получаем значение ранга
    var id = $('#rank').val();

    // Формируем ajax-запрос
    $.ajax(
    {
        type: 'GET',
        url: prefix + 'GetChains/' + task + "@" + id,
        success: function (chains)
        {
            // Заменяем часть представления, отвечающего за отображение структур ФПД
            $('#chains').replaceWith(chains);
        }
    });
}

// Назначение на событие change функции loadChain
$('#rank').on('change', function ()
{
    loadChain();
});