;;;


$(document).on('change', ':radio', function () {//$('input[type=radio][name=bedStatus]')

    //switch (this.name) {
    //    case 'change_state_radio':
    if (this.name.indexOf('change_state_radio') >= 0) {
        var valSplit = this.value.split('_');


        var groupRadio = document.getElementsByName(this.name);
        for (let i = 0; i < groupRadio.length; ++i) {
            let parts = groupRadio[i].value.split('change_state_radio_');
            document.getElementById('stateChilds_' + parts[1]).innerHTML = '';

        }



        var formData = {
            id: valSplit[4],
            type: valSplit[3]
        };
        goAjaxRequest({
            url: "/Actions/ChangeStateObject",
            data: formData,
            func_success: function (req, status, jqXHR) {
                var data = req.split('<hr />');//.responseText
                //var type = data[0].trim();

                //$('#fizVel').replaceWith(data[1]);
                document.getElementById('stateChilds_' + valSplit[3] + "_" + valSplit[4]).innerHTML = data[0];
                document.getElementById('PhaseObject_data_' + valSplit[3] + "_0").innerHTML = data[1];
                document.getElementById('PhaseObject_data_' + valSplit[3] + "_1").innerHTML = data[2];
                document.getElementById('PhaseObject_data_' + valSplit[3] + "_2").innerHTML = data[3];



            }, type: 'POST'
        });

        //скрыть\показать нужные\не нужные фазы именно чекбоксы
        let countPhase = document.getElementById('CountPhase_state_' + valSplit[4]);
        //if (countPhase) {

        for (let i = 0; i < 3; ++i) {
            if (countPhase && i < countPhase.value)
                document.getElementById('PhaseObject_all_' + valSplit[3] + '_' + i).style.display = 'block';
            else
                document.getElementById('PhaseObject_all_' + valSplit[3] + '_' + i).style.display = 'none';

        }




    }
});

$(document).on('change', ':checkbox', function () {
    if (this.name == 'change_phase_checbox') {
        //valArr = this.value.split('_');
        loadCheckBoxChildObject(this);

    }
    else if (this.name == 'changeObjectCheckbox') {
        if ($(this).prop("checked")) {
            document.getElementById('formPartEndId').style.display = 'block';
        }
        else {
            document.getElementById('formPartEndId').style.display = 'none';
        }

    }

});






function loadCheckBoxChildObject(ch) {
    var valArr = ch.value.split('_');
    var type = valArr[3]
    var id = valArr[4]
    var div = document.getElementById(("phaseChilds_" + type + "_" + id));

    if (div && !div.innerHTML.trim() && $(ch).prop("checked")) {
        var dataform = {
            id: valArr[4],
            type: valArr[3]
        };

        goAjaxRequest({
            url: "/Actions/GetPhaseObject",
            data: dataform,
            func_success: function (data, status, jqXHR) {

                //alert(data);
                if (data.trim())
                    div.innerHTML = data;
                else
                    div.remove();

            }
        });
    }
    else {
        //уже загружен либо не существует, надо снять чекбоксы при снятии на родителе
        if (div) {
            if (!$(ch).prop("checked")) {
                //галка снята, снимаем у детей и прячем

                $('#' + ("phaseChilds_" + type + "_" + id) + ' input:checkbox:checked').each(function () {//[type=checkbox]
                    $(this).removeAttr("checked")
                });
                div.style.display = 'none';
            }
            else {
                div.style.display = 'block';
            }

        }



    }

}


;;;