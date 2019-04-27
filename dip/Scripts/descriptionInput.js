;;

var data_descr_search = {

    function_trigger: null,
    search: 'yes'
};
//var dict = {};
$(document).on('change', ':checkbox', function () {



    if (this.name.indexOf('listSelectedPros') >= 0) {
        //pros
        loadCheckBoxChildDescr(this, 'GetProsChild', 'Pros');
    }
    else if (this.name.indexOf('listSelectedSpec') >= 0) {
        //spec
        loadCheckBoxChildDescr(this, 'GetSpecChild', 'Spec');
    }
    else if (this.name.indexOf('listSelectedVrem') >= 0) {
        //vrem
        loadCheckBoxChildDescr(this, 'GetVremChild', 'Vrem');
    }
    else if (this.name == 'changeTwoInputFormDescr') {
        //отобразить 2 форму
        var bl = document.getElementById('changeTwoInputFormDescr_id');
        var div = document.getElementById('block_one_descr_input_1');

        if (bl.checked) {


            div.style.display = 'block';
        }
        else {
            div.style.display = 'none';
        }
    }



});





function loadCheckBoxChildDescr(ch, meth, key) {
    var type = ch.name.split(key)[1];//[0]
    var div = document.getElementById((ch.value + type));

    if (div && !div.innerHTML.trim() && $(ch).prop("checked")) {
        goAjaxRequest({
            url: '/Actions/' + meth + '?id=' + ch.value + '&type=' + type,
            func_success: function (data, status, jqXHR) {

                if (data.trim()) {
                    div.innerHTML = data;
                    loadCheckBoxChildDescr(ch, meth, key);
                }
                else {
                    div.remove();
                }

            }
        });
    }
    else {
        //уже загружен либо не существует, надо снять чекбоксы при снятии на родителе
        if (div) {
            if (!$(ch).prop("checked")) {
                //галка снята, снимаем у детей и прячем

                $('#' + ch.value + type + ' input:checkbox:checked').each(function () {//[type=checkbox]
                    $(this).removeAttr("checked")
                });
                document.getElementById(ch.value + type).style.display = 'none';
            }
            else {
                document.getElementById(ch.value + type).style.display = 'block';
            }

        }



    }

}


//-------------------------------------------------------------------------------------------



//flag- true->param, false->not param
function changeTypeActionId(type, flag) {

    var fizNp = document.getElementById("fizVelGroup" + type + "_np_label");
    var fizP = document.getElementById("fizVelGroup" + type + "_p_label");
    var Pfiz = document.getElementById("parametricFizVel" + type + "_label");

    var pros = document.getElementById("prosGroup" + type + "_label");
    var spec = document.getElementById("specGroup" + type + "_label");
    var vrem = document.getElementById("vremGroup" + type + "_label");

    if (flag) {
        fizNp.style.display = 'none';
        fizP.style.display = 'block';
        Pfiz.style.display = 'block';
        pros.style.display = 'none';
        spec.style.display = 'none';
        vrem.style.display = 'none';
    }
    else {
        fizNp.style.display = 'block';
        fizP.style.display = 'none';
        Pfiz.style.display = 'none';
        pros.style.display = 'block';
        spec.style.display = 'block';
        vrem.style.display = 'block';
    }

}


;;;