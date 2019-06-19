/*файл скриптов для страницы полнотекстового поиска
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

;;



var TextSearchObj = {};

//метод события изменения радио кнопки
$(document).on('change', ':radio', function () {

    if (this.name == 'searchTypeRadio') {
        let infodiv = document.getElementById('InfoCurrentTypeSearch');
        let strInfo = '';
        switch (this.value) {

            case 'lucene':
                strInfo = 'Полнотекстовый поиск lucene, желательно минимизировать знаки пунктуации, буквы не из русского алфавита, цифры, стоп слова. Слова должны быть разделены пробелами';
                break;

            case 'fullTextSearchF':
                strInfo = 'Обычный полнотекстовый поиск fullTextSearch, желательно минимизировать знаки пунктуации, буквы не из русского алфавита, цифры, стоп слова. Слова должны быть разделены пробелами';
                break;

            case 'fullTextSearchCf':
                strInfo = 'Поиск форм слов, можно использовать (#слово) для повышения важности слова.' +
                    ' Желательно минимизировать количество знаков пунктуации, букв не из русского алфавита, цифр, стоп слов.' +
                    ' Поисковой запрос включающий поисковую строку не должен превышать 4000 символов, следовательно при средней длине слова, поисковая строка не должна превышать примерно 1000 символов. Слова должны быть разделены пробелами';
                break;

            case 'fullTextSearchCl':
                strInfo = 'Поиск по вхождениям, можно использовать (#слово) для повышения важности слова.' +
                    ' Желательно минимизировать количество знаков пунктуации, букв не из русского алфавита, цифр, стоп слов.' +
                    ' Поисковой запрос включающий поисковую строку не должен превышать 4000 символов, следовательно при средней длине слова, поисковая строка не должна превышать примерно 2000 символов. Слова должны быть разделены пробелами';
                break;

            case 'fullTextSearchNear':
                strInfo = 'Поиск фразы, включенные слова ищутся рядом с другими включенными словами, поиск для 2+ слов.' +
                    ' Желательно минимизировать количество знаков пунктуации, букв не из русского алфавита, цифр, стоп слов.' +
                    ' Поисковая строка не должна превышать 60 слов. Слова должны быть разделены пробелами';
                break;

            case 'fullTextSearchSemantic':
                strInfo = 'Семантический поиск. В бд создана запись, в нее заносится строка поиска и' +
                    'определяются похожие средствами sql server. С данным типом поиска можно работать ' +
                    'одновременно только 1 человеку(тк запись 1 и она блокируется до окончания выполнения запроса, ' +
                    'запись можно насильно разблокировать нажав на кнопку). ' +
                    '<button class="btn btn-default" onclick="ReloadSemantic()">Насильно обнулить запись для поиска</button>';
                break;
            case 'NGrammSearch':
                strInfo = 'Поиск N граммами, ресурсоемкий, лучше использовать для короткой строки запроса.' +
                    ' Желательно минимизировать количество знаков пунктуации, букв не из русского алфавита, цифр, стоп слов.' +
                    ' Поисковая строка не должна превышать 20 слов. Слова должны быть разделены пробелами';
                break;
            case 'fullTextSearchMainSemanticWord':
                strInfo = 'В каждой записи ФЭ семантически выбираются главные слова  результат сопостовляется с запросом' +
                    'Наименее точный поиск. Поиск происходит не по всем записям(зависит от текста). Слова должны быть разделены пробелами';
                break;
        }
        infodiv.innerHTML = strInfo
    }
});


//метод загрузки дополнительных записей
function loadNewFT() {
    TextSearchObj.curTextS = document.getElementById('TextForSearch_id').value;

    $('#divTypesSearch_id input:radio:checked').each(function () {
        TextSearchObj.curTypeS = this.value;
    });

    if (!TextSearchObj.curTypeS) {
        alert('Выберите тип поиска');
        return;
    }
    if (!TextSearchObj.curTextS) {
        alert('Введите текст');
        return;
    }

    document.getElementById('div_search_ft_result_id').innerHTML = '';
    loadMoreFT();
}


var block_load = false;

//метод загрузки дополнительных записей
function loadMoreFT() {

    //если вернулся пустой список то возвращать статус серва с ошибкой, блокировать множественное нажатие кнопки

    if (!block_load) {
        block_load = true;

        var formData = {
            str: TextSearchObj.curTextS
        };

        formData.type = TextSearchObj.curTypeS;

        var div_last_load_c = document.getElementById('div_search_ft_result_id').children;
        if (div_last_load_c.length > 0) {
            var div_last_load = div_last_load_c[div_last_load_c.length - 1];
            formData.countLoad = +div_last_load.id.split('_')[6] + 1;
            var div_last_fe_c = div_last_load.children;
            var div_last_fe = div_last_fe_c[div_last_fe_c.length - 1];
            formData.lastId = div_last_fe.id.split('_')[5];
        }

        goAjaxRequest({
            url: '/Search/TextSearchPartial',
            data: formData,
            func_complete: function (req, status, jqXHR) {
                block_load = false;

            },
            func_error: function (req, status, jqXHR) {
                document.getElementById('div_search_ft_but_id').style.display = 'none';
            },
            func_success: function (req, status, jqXHR) {
                if (jqXHR.status == 204) {
                    document.getElementById('div_search_ft_but_id').style.display = 'none';
                }
                else if (jqXHR.status == 207) {
                    alert('Попробуйте позже, запись занята');
                }
                else {
                    document.getElementById('div_search_ft_but_id').style.display = 'inline';
                    var div = document.getElementById('div_search_ft_result_id');
                    div.innerHTML += req;
                }
            }, type: 'POST',
            beforeSend: function () {

            }
        });
    }
}

//метод для перезагрузки семантической записи ФЭ
function ReloadSemantic() {
    goAjaxRequest({
        url: '/Physic/ReloadSemanticRecord',
        data: {},

        func_success: function (req, status, jqXHR) {

            alert('Запись очищена');
        }, type: 'POST',
        beforeSend: function () {

        }
    });
}



;;;