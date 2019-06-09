;;;


var block_load = false;


//метод для первоначальных настроек формы перед отправкой
function GoFirstDescrSearch() {
    document.getElementById('div_search_ft_but_id').style.display = 'none';
    data_descr_search.function_trigger = goSave;
    Go_next_step();
}



//метод для отправки формы сохранения дескрипторного  поиска
function goSave() {
    var twoObject = document.getElementById('changeObjectCheckbox').checked;

    data_descr_search.function_trigger = null;



    goAjaxRequest({
        url: "/Search/DescriptionSearch",
        data: data_descr_search,
        func_complete: function (data, status, jqXHR) {
            if (status == 'error')
                console.log('error partial search');
            else {
                document.getElementById('search_content_div_id').innerHTML = data.responseText;
                document.getElementById('div_search_ft_but_id').style.display = 'block';
            }
            data_descr_search = {
                search: data_descr_search.search,
                function_trigger: goSave
            };
            showHideDescrForm(false);
        },
        type: 'POST'
    });

}





//метод для отображения\сокрытия дескрипторной формы
//action:true- показать
function showHideDescrForm(action) {
    var form = document.getElementById('DescriptionFormMainAll');
    var but = document.getElementById('showHideDescrForm');
    if (action == null || action == undefined) {
        if (form.style.display == 'block') {
            hideForm();
        }

        else {
            showForm();
        }
    }
    else {
        if (action) {
            showForm();
        }
        else {
            hideForm();
        }
    }


    //метод для отображения дескрипторной формы
    function showForm() {
        form.style.display = 'block';
        but.innerHTML = 'Скрыть форму';
    }

    //метод длясокрытия дескрипторной формы
    function hideForm() {
        form.style.display = 'none';
        but.innerHTML = 'Показать форму';
    }
}


//метод для очистки формы
function clearForm() {
    var i = 0;
    var inpdiv = document.getElementById('actionI' + i);
    while (inpdiv) {
        $("#actionI" + i).val($("#actionI" + i + " option:first").val());
        $("#actionTypeI" + i).val($("#actionTypeI" + i + " option:first").val());
        changeParams('I' + i);
        inpdiv = document.getElementById('actionI' + ++i);
    }
    i = 0;
    inpdiv = document.getElementById('actionO' + i);
    while (inpdiv) {
        $("#actionO" + i).val($("#actionO" + i + " option:first").val());
        $("#actionTypeO" + i).val($("#actionTypeO" + i + " option:first").val());
        changeParams('O' + i);
        inpdiv = document.getElementById('actionO' + ++i);
    }


    $('#changeTwoInputFormDescr_id').removeAttr("checked");
    var div2Input = document.getElementById('block_one_descr_input_1');

    div2Input.style.display = 'none';


    var url_ = document.getElementById("descr_search_href_id").value;
    history.pushState(null, null, url_);


    //object
    $('#changeObjectCheckbox').removeAttr("checked");
    document.getElementById('formPartEndId').style.display = 'none';

    for (let i = 0; i < 3; ++i) {
        document.getElementById('PhaseObject_data_S_' + i).innerHTML = '';
        document.getElementById('PhaseObject_all_S_' + i).style.display = 'none';
        document.getElementById('PhaseObject_data_E_' + i).innerHTML = '';
        document.getElementById('PhaseObject_all_E_' + i).style.display = 'none';

    }
    $('#mainBlockStates_S input:radio:checked').each(function () {//[type=checkbox] 
        $(this).removeAttr("checked");

        let dv = document.getElementById('stateChilds_' + this.id.split('change_state_radio_id_')[1]);
        if (dv)
            dv.innerHTML = '';
    });
    $('#mainBlockStates_E input:radio:checked').each(function () {//[type=checkbox] 
        $(this).removeAttr("checked");

        let dv = document.getElementById('stateChilds_' + this.id.split('change_state_radio_id_')[1]);
        if (dv)
            dv.innerHTML = '';
    });

}




//метод для предварительной настройки перед дополнительной загрузкой записей ФЭ
function loadMoreFTBut() {
    data_descr_search.function_trigger = loadMoreFT;
    Go_next_step();
}



//метод для дополнительной загрузки записей ФЭ
function loadMoreFT() {

    //если вернулся пустой список то возвращать статус серва с ошибкой, блокировать множественное нажатие кнопки

    if (!block_load) {
        block_load = true;


        var div_last_load_c = document.getElementById('search_content_div_id').children;
        if (div_last_load_c.length > 0) {
            var div_last_load = div_last_load_c[div_last_load_c.length - 1];
            var div_last_fe_c = div_last_load.children;
            var div_last_fe = div_last_fe_c[div_last_fe_c.length - 1];
            if (!div_last_fe) {
                document.getElementById('div_search_ft_but_id').style.display = 'none';
                return;
            }
            data_descr_search['lastId'] = div_last_fe.id.split('_')[5];
            data_descr_search['emptyResult'] = true;

        }




        var twoObject = document.getElementById('changeObjectCheckbox').checked;

        data_descr_search.function_trigger = null;



        goAjaxRequest({
            url: "/Search/DescriptionSearch",
            data: data_descr_search,
            func_complete: function (data, status, jqXHR) {
                block_load = false;
                if (status == 'error')
                    console.log('error partial search');
                else {
                    if (data.responseText.trim())
                        document.getElementById('search_content_div_id').innerHTML += data.responseText;//responseText убрать если будет success
                    else
                        document.getElementById('div_search_ft_but_id').style.display = 'none';
                }
                data_descr_search = {
                    search: data_descr_search.search,
                    function_trigger: goSave
                };
                showHideDescrForm(false);
            },
            func_error: function (req, status, jqXHR) {
                document.getElementById('div_search_ft_but_id').style.display = 'none';
            },
            type: 'POST'
        });
    }
}

;;;;