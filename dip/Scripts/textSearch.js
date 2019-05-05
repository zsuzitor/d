;;



var TextSearchObj = {};

$(document).on('change', ':radio', function () {

    if (this.name == 'searchTypeRadio') {
        let infodiv = document.getElementById('InfoCurrentTypeSearch');
        let strInfo = '';
        switch (this.value) {

            case 'lucene':
                strInfo = 'Полнотекстовый поиск lucene';
                break;

            case 'fullTextSearchF':
                strInfo = 'Обычный полнотекстовый поиск fullTextSearch';
                break;

            case 'fullTextSearchCf':
                strInfo = 'Поиск форм слов, можно использовать (#слово) для повышения важности слова';
                break;

            case 'fullTextSearchCl':
                strInfo = 'Поиск по вхождениям, можно использовать (#слово) для повышения важности слова';
                break;

            case 'fullTextSearchNear':
                strInfo = 'Поиск фразы, включенные слова ищутся рядом с другими включенными словами, поиск для 2+ слов';
                break;

            case 'fullTextSearchSemantic':
                strInfo = 'Семантический поиск. В бд создана запись, в нее заносится строка поиска и' +
                    'определяются похожие средствами sql server. С данным типом поиска можно работать ' +
                    'одновременно только 1 человеку(тк запись 1 и она блокируется до окончания выполнения запроса, ' +
                    'запись можно насильно разблокировать нажав на кнопку). ' +
                    '<button onclick="ReloadSemantic()">Насильно обнулить запись для поиска</button>';
                break;
            case 'NGrammSearch':
                strInfo = 'Поиск N граммами, ресурсоемкий, лучше использовать для короткой строки запроса';
                break;
            case 'fullTextSearchMainSemanticWord':
                strInfo = 'В каждой записи ФЭ семантически выбираются главные слова  результат сопостовляется с запросом' +
                    'Наименее точный поиск. Поиск происходит не по всем записям(зависит от текста)';
                break;
        }
        infodiv.innerHTML = strInfo
    }
});



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