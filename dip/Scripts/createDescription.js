;;;
//mainObjEdit.massOldValue
var mainObjEdit = {
    //newAction: false,
    //newFizVel: false,
    //newParamFizVel: false,
    curentActionId: '',
    curentFizVelId: '',
    parametricAction: false,
    //parametricActionNew: false,

    maxNewFizVelId: 1,
    maxNewParamFizVelId: 1,
    maxNewSpecId: 1,
    maxNewProId: 1,
    maxNewVremId: 1,

    massEditActionId: [],
    massEditFizVels: [],
    massEditParamFizVels: [],
    massEditPros: [],
    massEditVrems: [],
    massEditSpecs: [],

    massDeletedPros: [],
    massDeletedVrems: [],
    massDeletedSpecs: [],
    massDeletedFizVels: [],
    massDeletedActionId: [],


    massOldValue: []

};








$(document).on('change', ':radio', function () {//$('input[type=radio][name=bedStatus]')

    switch (this.name) {
        case 'actionId':


            mainObjEdit.curentActionId = this.value;
            var value = this.value;

            if (this.value !== 'VOZ0') {
                document.getElementById('changeTypeParamActionId').style.display = 'none';

                var formData = {
                    fizVelId: value,

                };


                goAjaxRequest({
                    url: "/Actions/ChangeActionEdit",
                    data: formData,
                    func_success: function (req, status, jqXHR) {
                        var data = req.split('<hr />');
                        document.getElementById('fizVel').innerHTML = data[1];


                        document.getElementById('prosGroup').innerHTML = data[2];


                        document.getElementById('specGroup').innerHTML = data[3];


                        document.getElementById('vremGroup').innerHTML = data[4];


                        document.getElementById('fizVelGroup_all').style.display = 'block';

                        document.getElementById('div_all_checkboxes').style.display = 'block';

                        $("html, body").animate({ scrollTop: $("#ScrollChangeActionId").offset().top }, "slow");
                        if (checkParametricAction(value)) {
                            SetParametricalType(true);
                        }
                        else {
                            SetParametricalType(false);

                        }

                    }, type: 'POST'
                });
            }
            else {//voz0
                document.getElementById('fizVelGroup_all').style.display = 'block';
                document.getElementById('div_all_checkboxes').style.display = 'block';

                document.getElementById('changeTypeParamActionId').style.display = 'block';
                document.getElementById('fizVel').innerHTML = '';
                document.getElementById('parametricFizVel').innerHTML = '';
                document.getElementById('prosGroup').innerHTML = '<div class="form-group descrCheckBoxChildEdit">' +
                    '<div class="" id=\'VOZ0_PROS\'> </div> <div class=""> <button class="btn btn-default" onclick="addNewPro(\'VOZ0\')">Добавить</button> </div></div>';

                document.getElementById('specGroup').innerHTML = '<div class="form-group descrCheckBoxChildEdit">' +
                    '<div class="" id=\'VOZ0_SPEC\'> </div> <div class=""> <button class="btn btn-default" onclick="addNewSpec(\'VOZ0\')">Добавить</button> </div></div>';
                document.getElementById('vremGroup').innerHTML = '<div class="form-group descrCheckBoxChildEdit">' +
                    '<div class="" id=\'VOZ0_VREM\'> </div> <div class=""> <button class="btn btn-default" onclick="addNewVrem(\'VOZ0\')">Добавить</button> </div></div>';
                //определить тип и спрятать лишнее 
                if (checkParametricAction(value)) {
                    SetParametricalType(true);
                }
                else {
                    SetParametricalType(false);

                }

            }
            document.getElementById('parametricFizVel_all').style.display = 'none';

            break;


        case 'fizVelId':
            mainObjEdit.curentFizVelId = this.value;
            if (this.value.indexOf("FizVelNameNEW") === -1)
                goAjaxRequest({
                    url: '/Actions/GetParametricFizVelsEdit/' + this.value,
                    func_success: function (data, status, jqXHR) {
                        // Заменяем часть представления, отвечающего за выбор физической величины
                       
                        document.getElementById('parametricFizVel').innerHTML = data;
                        $("html, body").animate({ scrollTop: $("#ScrollChangeParamFizvelId").offset().top }, "slow");

                    }
                });
            if (mainObjEdit.parametricAction) {
                document.getElementById('parametricFizVel').innerHTML = '';
                document.getElementById('parametricFizVel_all').style.display = 'block';
            }

            break;

    }


});


$(document).on('change', ':checkbox', function () {

    switch (this.name) {
        case 'listSelectedPros':
            if (this.checked) {
                loadCheckBoxChild(this, 'GetProsChildEdit');

            }
            else {
                document.getElementById(this.value + '_childs').innerHTML = '';
            }

            break;

        case 'listSelectedSpecs':
            if (this.checked) {
                loadCheckBoxChild(this, 'GetSpecChildEdit');
            }
            else {
                document.getElementById(this.value + '_childs').innerHTML = '';
            }
            break;

        case 'listSelectedVrems':
            if (this.checked) {
                loadCheckBoxChild(this, 'GetVremChildEdit');
            }
            else {
                document.getElementById(this.value + '_childs').innerHTML = '';
            }
            break;

        case 'changeTypeParamActionName':
            if (this.checked) {
                SetParametricalType(true);

            }
            else {
                SetParametricalType(false);

            }
            break;

        case 'listSelectedProsNew':
            if (this.checked) {
                document.getElementById(this.value + '_childs').innerHTML = ' <div class="form-group descrCheckBoxChildEdit"> \
           <div class="" id="'+ this.value + '_PROS"> </div>\
           <div class="">\
               <button class="btn btn-default" onclick="addNewPro(\'' + this.value + '\')">Добавить</button>\
           </div>\
       </div>';
            }
            else
                document.getElementById(this.value + '_childs').innerHTML = '';
            //_PROS

            break;


        case 'listSelectedVremsNew':
            if (this.checked) {
                document.getElementById(this.value + '_childs').innerHTML = ' <div class="form-group descrCheckBoxChildEdit"> \
           <div class="" id="' + this.value + '_VREM"> </div>\
           <div class="">\
               <button class="btn btn-default" onclick="addNewVrem(\'' + this.value + '\')">Добавить</button>\
           </div>\
       </div>';
            }
            else
                document.getElementById(this.value + '_childs').innerHTML = '';
            //_VREM

            break;


        case 'listSelectedSpecsNew':
            if (this.checked) {
                document.getElementById(this.value + '_childs').innerHTML = ' <div class="form-group descrCheckBoxChildEdit"> \
           <div class="" id="' + this.value + '_SPEC"> </div>\
           <div class="">\
               <button class="btn btn-default" onclick="addNewSpec(\'' + this.value + '\')">Добавить</button>\
           </div>\
       </div>';
            }
            else
                document.getElementById(this.value + '_childs').innerHTML = '';
            //_SPEC
            break;
    }


});




function SetParametricalType(set) {
    if (set) {
        document.getElementById('prosGroup_all').style.display = 'none';
        document.getElementById('specGroup_all').style.display = 'none';
        document.getElementById('vremGroup_all').style.display = 'none';
        document.getElementById('parametricBlock_all').style.display = 'block';
        $('#fizVel input:radio').each(function () {//[type=checkbox] 
            $(this).css('display', 'block');
        });
        mainObjEdit.parametricAction = true;
        document.getElementById('fizVelGroup_np_label').style.display = 'none';
        document.getElementById('fizVelGroup_p_label').style.display = 'block';
    }
    else {
        document.getElementById('prosGroup_all').style.display = 'block';
        document.getElementById('specGroup_all').style.display = 'block';
        document.getElementById('vremGroup_all').style.display = 'block';
        document.getElementById('parametricBlock_all').style.display = 'none';
        $('#fizVel input:radio').each(function () {
            $(this).css('display', 'none');
        });
        mainObjEdit.parametricAction = false;
        document.getElementById('fizVelGroup_np_label').style.display = 'block';
        document.getElementById('fizVelGroup_p_label').style.display = 'none';
    }


}




function checkParametricAction(id) {
    if (id == 'VOZ0') {
        if (document.getElementById('changeTypeParamActionId').checked)
            return true;
    }
    else {
        var checkParametrics = document.getElementById('parametric_action_or_not').value.split(' ');
        for (var i = 0; i < checkParametrics.length; ++i) {//todo вынести в фукнцию

            if (checkParametrics[i] == id) {
                return true;
            }
        }
    }
    return false;
}



function loadCheckBoxChild(ch, meth) {

    var div = document.getElementById((ch.value + "_childs"));

    goAjaxRequest({
        url: '/Actions/' + meth + '?id=' + ch.value,
        func_success: function (data, status, jqXHR) {

            if (data.trim()) {
                div.innerHTML = data;
                //loadCheckBoxChild(ch, meth);
            }
            else {
                div.remove();
            }
        }
    });
}






//прячем
function ReplaceDisplayNoneNew(newdiv, oldDiv, changeBut) {
    newdiv.style.display = 'none';
    oldDiv.style.display = 'block';
    if (changeBut)
        changeBut.innerHTML = 'Добавить новый';
}

function ReplaceDisplayNoneOld(newdiv, oldDiv, changeBut) {
    newdiv.style.display = 'block';
    oldDiv.style.display = 'none';
    if (changeBut)
        changeBut.innerHTML = 'Использовать существущие';
}


function addNewPro(id) {

    var div = document.getElementById(id + '_PROS');
    var strCur = id.split('_')[0] + '_PROS_NEW' + (mainObjEdit.maxNewProId++);



    $(div).append('<div class="one_check_line_edit" id="div_' + div.id + '_all"><input value="' + strCur +
        '" id="' + strCur + '_chch" class="checkBoxClass" name="listSelectedProsNew" type="checkbox"/>' +
        '<button class="btn btn-default" id="' + strCur + '_delbut"  onclick=\'deleteItemPro("' + strCur + '")\'>Удалить</button>' +
        '<input class="form-control" id="'
        + strCur + '_inp" type="text" name="' + id + '" /><div id="' + strCur + '_childs"></div></div>');

}
function addNewSpec(id) {
    var div = document.getElementById(id + '_SPEC');
    var strCur = id.split('_')[0] + '_SPEC_NEW' + (mainObjEdit.maxNewSpecId++);
    $(div).append('<div class="one_check_line_edit"  id="div_' + div.id + '_all"><input value="' + strCur +
        '" id="' + strCur + '_chch" class="checkBoxClass" name="listSelectedSpecsNew" type="checkbox"/>' +
        '<button class="btn btn-default" id="' + strCur + '_delbut"  onclick=\'deleteItemSpec("' + strCur + '")\'>Удалить</button>' +
        '<input class="form-control" id="'
        + strCur + '_inp"  type="text" name="' + id + '" /><div id="' + strCur + '_childs"></div></div>');
}
function addNewVrem(id) {
    var div = document.getElementById(id + '_VREM');
    var strCur = id.split('_')[0] + '_VREM_NEW' + (mainObjEdit.maxNewVremId++);
    $(div).append('<div class="one_check_line_edit"  id="div_' + div.id + '_all"><input value="' + strCur +
        '" id="' + strCur + '_chch" class="checkBoxClass" name="listSelectedVremsNew" type="checkbox"/>' +
        '<button class="btn btn-default" id="' + strCur + '_delbut" onclick=\'deleteItemVrem("' + strCur + '")\'>Удалить</button>' +
        '<input class="form-control" id="'
        + strCur + '_inp"  type="text" name="' + id + '" /><div id="' + strCur + '_childs"></div></div>');
}

function addNewFizVelId() {
    var div = document.getElementById('fizVel');
    var newMaxId = mainObjEdit.maxNewFizVelId++;
    var newMaxIdStr = "FizVelNameNEW" + newMaxId;

    var res = "";
    res += '<div class="padding_10_top">';

    res += '<input name="fizVelId" type="radio" value="' + newMaxIdStr +
'" id="' + newMaxIdStr +
'_radio" class="radioClass" style="';

    if (mainObjEdit.parametricAction) {
        res += 'display: block;">';
    }
    else {
        res += 'display: none;">';
    }
    res += '<button id="' + newMaxIdStr + '_delbut" class="btn btn-default" onclick="deleteItemFizVels(\'' + newMaxIdStr +
        '\')">Удалить</button>' +
                '<input class="form-control" type="text" value="" name="' + mainObjEdit.curentActionId +
        '" id="' + newMaxIdStr + '_inp" >' +
           ' </div>';

    $(div).append(res);

}

function addNewParametricFizVelId() {
    var div = document.getElementById('parametricFizVel');

    $(div).append(' <div class="padding_10_top">' +
                    '<button id="ParamFizVelNameNEW' + mainObjEdit.maxNewParamFizVelId + '_delbut" class="btn btn-default" onclick="deleteItemFizVels(\'ParamFizVelNameNEW' + mainObjEdit.maxNewParamFizVelId + '\')">Удалить</button>' +
                    '<input class="form-control" type="text" value="" name="' + mainObjEdit.curentFizVelId + '" id="ParamFizVelNameNEW' + mainObjEdit.maxNewParamFizVelId++ +
        '_inp" > </div>');


}





function changeActionId(id) {

    var inp = $('#' + id + '_inp');
    var inpVal = inp.val();
    if ($(inp).prop('readonly')) {
        $(inp).removeProp('readonly');
        if (!mainObjEdit.massEditActionId.includes(id)) {
            mainObjEdit.massOldValue.push({
                id: id,
                val: inpVal
            });
            mainObjEdit.massEditActionId.push(id);
        }

    }
    else {
        $(inp).prop('readonly', true);

        for (var i = 0; i < mainObjEdit.massOldValue.length; ++i)
            if (mainObjEdit.massOldValue[i].id == id) {
                if (mainObjEdit.massOldValue[i].val == inpVal) {
                    restoreValue(1, id);
                    return;
                }
                break;
            }

    }
    document.getElementById('restoreInput_but_' + id).style.display = 'inline';
}

function changeFizVels(id) {

    var inp = $('#' + id + '_inp');
    var inpVal = inp.val();
    if ($(inp).prop('readonly')) {
        $(inp).removeProp('readonly');
        if (!mainObjEdit.massEditFizVels.includes(id)) {
            mainObjEdit.massOldValue.push({
                id: id,
                val: inpVal
            });
            mainObjEdit.massEditFizVels.push(id);
        }
    }
    else {
        $(inp).prop('readonly', true);
        for (var i = 0; i < mainObjEdit.massOldValue.length; ++i)
            if (mainObjEdit.massOldValue[i].id == id) {
                if (mainObjEdit.massOldValue[i].val == inpVal) {
                    restoreValue(2, id);
                    return;
                }
                break;
            }
    }
    document.getElementById('restoreInput_but_' + id).style.display = 'inline';
}


function changeParamFizVels(id) {

    var inp = $('#' + id + '_inp');
    var inpVal = inp.val();
    if ($(inp).prop('readonly')) {
        $(inp).removeProp('readonly');
        if (!mainObjEdit.massEditParamFizVels.includes(id)) {
            mainObjEdit.massOldValue.push({
                id: id,
                val: inpVal
            });
            mainObjEdit.massEditParamFizVels.push(id);
        }
    }
    else {
        $(inp).prop('readonly', true);
        for (var i = 0; i < mainObjEdit.massOldValue.length; ++i)
            if (mainObjEdit.massOldValue[i].id == id) {
                if (mainObjEdit.massOldValue[i].val == inpVal) {
                    restoreValue(3, id);
                    return;
                }
                break;
            }
    }
    document.getElementById('restoreInput_but_' + id).style.display = 'inline';
}

function changePros(id) {

    var inp = $('#' + id + '_inp');
    var inpVal = inp.val();
    if ($(inp).prop('readonly')) {
        $(inp).removeProp('readonly');
        if (!mainObjEdit.massEditPros.includes(id)) {
            mainObjEdit.massOldValue.push({
                id: id,
                val: inpVal
            });
            mainObjEdit.massEditPros.push(id);
        }
    }
    else {
        $(inp).prop('readonly', true);
        for (var i = 0; i < mainObjEdit.massOldValue.length; ++i)
            if (mainObjEdit.massOldValue[i].id == id) {
                if (mainObjEdit.massOldValue[i].val == inpVal) {
                    restoreValue(4, id);
                    return;
                }
                break;
            }
    }
    document.getElementById('restoreInput_but_' + id).style.display = 'inline';
}

function changeSpecs(id) {

    var inp = $('#' + id + '_inp');
    var inpVal = inp.val();
    if ($(inp).prop('readonly')) {
        $(inp).removeProp('readonly');
        if (!mainObjEdit.massEditSpecs.includes(id)) {
            mainObjEdit.massOldValue.push({
                id: id,
                val: inpVal
            });
            mainObjEdit.massEditSpecs.push(id);
        }
    }
    else {
        $(inp).prop('readonly', true);
        for (var i = 0; i < mainObjEdit.massOldValue.length; ++i)
            if (mainObjEdit.massOldValue[i].id == id) {
                if (mainObjEdit.massOldValue[i].val == inpVal) {
                    restoreValue(5, id);
                    return;
                }
                break;
            }
    }
    document.getElementById('restoreInput_but_' + id).style.display = 'inline';
}



function changeVrems(id) {

    var inp = $('#' + id + '_inp');
    var inpVal = inp.val();
    if ($(inp).prop('readonly')) {
        $(inp).removeProp('readonly');
        if (!mainObjEdit.massEditVrems.includes(id)) {
            mainObjEdit.massOldValue.push({
                id: id,
                val: inpVal
            });
            mainObjEdit.massEditVrems.push(id);
        }
    }
    else {
        $(inp).prop('readonly', true);
        for (var i = 0; i < mainObjEdit.massOldValue.length; ++i)
            if (mainObjEdit.massOldValue[i].id == id) {
                if (mainObjEdit.massOldValue[i].val == inpVal) {
                    restoreValue(6, id);
                    return;
                }
                break;
            }
    }
    document.getElementById('restoreInput_but_' + id).style.display = 'inline';
}




//type: 1-pros 2-spec,3-vrem
function deleteItemPro(id) {
    deleteItem(id, 1);
}
function deleteItemSpec(id) {
    deleteItem(id, 2);
}
function deleteItemVrem(id) {
    deleteItem(id, 3);
}
function deleteItemFizVels(id) {
    deleteItem(id, 4);
}
function deleteItemActionId(id) {
    deleteItem(id, 5);
}

//type: 1-pros 2-spec,3-vrem,4-fizvels
function deleteItem(id, type) {
    var delbut = document.getElementById(id + '_delbut');
    var checkboxChilds = $('#' + id + '_chch');
    var inp = $('#' + id + '_inp');
    if (delbut.innerHTML.trim() == 'Удалить') {
        //"удаляем"
        if (id.indexOf('NEW') < 0) {
            switch (type) {
                case 1:
                    mainObjEdit.massDeletedPros.push(id);
                    break;
                case 2:
                    mainObjEdit.massDeletedSpecs.push(id);
                    break;
                case 3:
                    mainObjEdit.massDeletedVrems.push(id);
                    break;
                case 4:
                    mainObjEdit.massDeletedFizVels.push(id);
                    break;
                case 5:
                    mainObjEdit.massDeletedActionId.push(id);
                    break;
            }
        }

        delbut.innerHTML = 'Восстановить';
        var divChilds = document.getElementById(id + '_childs');
        if ($("div").is('#' + id + '_childs'))
            divChilds.innerHTML = '';
        if ($("input").is('#' + id + '_chch')) {
            $(checkboxChilds).removeAttr("checked")
            $(checkboxChilds).css('display', 'none');
        }

        $(inp).css('background-color', 'grey');
        $(inp).prop('readonly', true);

    }
    else {
        //восстанавливаем
        if (id.indexOf('NEW') < 0) {
            switch (type) {
                case 1:
                    for (var i = 0; i < mainObjEdit.massDeletedPros.length; ++i) {
                        if (mainObjEdit.massDeletedPros[i] == id) {
                            mainObjEdit.massDeletedPros.splice(i, 1);
                            break;
                        }
                    }

                    break;
                case 2:
                    for (var i = 0; i < mainObjEdit.massDeletedSpecs.length; ++i) {
                        if (mainObjEdit.massDeletedSpecs[i] == id) {
                            mainObjEdit.massDeletedSpecs.splice(i, 1);
                            break;
                        }
                    }
                    break;
                case 3:
                    for (var i = 0; i < mainObjEdit.massDeletedVrems.length; ++i) {
                        if (mainObjEdit.massDeletedVrems[i] == id) {
                            mainObjEdit.massDeletedVrems.splice(i, 1);
                            break;
                        }
                    }
                    break;
                case 4:
                    for (var i = 0; i < mainObjEdit.massDeletedFizVels.length; ++i) {
                        if (mainObjEdit.massDeletedFizVels[i] == id) {
                            mainObjEdit.massDeletedFizVels.splice(i, 1);
                            break;
                        }
                    }
                    break;
                case 5:
                    for (var i = 0; i < mainObjEdit.massDeletedActionId.length; ++i) {
                        if (mainObjEdit.massDeletedActionId[i] == id) {
                            mainObjEdit.massDeletedActionId.splice(i, 1);
                            break;
                        }
                    }
                    break;
            }
        }
        else {
            $(inp).removeProp('readonly');
        }
        delbut.innerHTML = 'Удалить';
        $(checkboxChilds).css('display', 'inline');
        //
        $(inp).css('background-color', '');

    }
}




function restoreValue(type, id) {

    for (var i = 0; i < mainObjEdit.massOldValue.length; ++i) {
        if (mainObjEdit.massOldValue[i].id == id) {
            document.getElementById(id + '_inp').value = mainObjEdit.massOldValue[i].val;
            mainObjEdit.massOldValue.splice(i, 1);
            switch (type) {
                case 1:
                    for (var i2 = 0; i2 < mainObjEdit.massEditActionId.length; ++i2) {
                        if (mainObjEdit.massEditActionId[i2] == id) {
                            mainObjEdit.massEditActionId.splice(i2, 1);
                            break;
                        }
                    }
                    break;
                case 2:
                    for (var i2 = 0; i2 < mainObjEdit.massEditFizVels.length; ++i2) {
                        if (mainObjEdit.massEditFizVels[i2] == id) {
                            mainObjEdit.massEditFizVels.splice(i2, 1);
                            break;
                        }
                    }
                    break;
                case 3:
                    for (var i2 = 0; i2 < mainObjEdit.massEditParamFizVels.length; ++i2) {
                        if (mainObjEdit.massEditParamFizVels[i2] == id) {
                            mainObjEdit.massEditParamFizVels.splice(i2, 1);
                            break;
                        }
                    }
                    break;
                case 4:
                    for (var i2 = 0; i2 < mainObjEdit.massEditPros.length; ++i2) {
                        if (mainObjEdit.massEditPros[i2] == id) {
                            mainObjEdit.massEditPros.splice(i2, 1);
                            break;
                        }
                    }
                    break;

                case 5:
                    for (var i2 = 0; i2 < mainObjEdit.massEditSpecs.length; ++i2) {
                        if (mainObjEdit.massEditSpecs[i2] == id) {
                            mainObjEdit.massEditSpecs.splice(i2, 1);
                            break;
                        }
                    }
                    break;
                case 6:
                    for (var i2 = 0; i2 < mainObjEdit.massEditVrems.length; ++i2) {
                        if (mainObjEdit.massEditVrems[i2] == id) {
                            mainObjEdit.massEditVrems.splice(i2, 1);
                            break;
                        }
                    }
                    break;
            }

            break;
        }
    }
    var inp = $('#' + id + '_inp');
    $(inp).prop('readonly', true);
    document.getElementById('restoreInput_but_' + id).style.display = 'none';
}



function saveForm() {
    //TODO валидация


    var formData = {
        currentActionId: mainObjEdit.curentActionId

    };
    if (!formData.currentActionId) {
        alert('Необходимо выбрать "Название воздействия" независимо от желаемого действия')
        return;
    }

    //type: 1-actionid ,2-fizvell,3-paramfizvell,4-pros,5-spec,6-vrem
    //TypeAction: 1-добавление ,2-редактирование, 3- удаление
    function construct_(iter, prop, id, parentId, text, parametric) {

        formData['obj.' + prop + '[' + iter + '].Id'] = id;
        formData['obj.' + prop + '[' + iter + '].ParentId'] = parentId;
        formData['obj.' + prop + '[' + iter + '].Text'] = text;
        formData['obj.' + prop + '[' + iter + '].Parametric'] = parametric;
    }



    var curentFizVel = null;


    $('#fizVel input:radio:checked').each(function () {
        curentFizVel = this.value;
    });

    var iteration = 0;
    //отредактированные actionId
    for (var i = 0; i < mainObjEdit.massEditActionId.length; ++i) {
        //var checkObj=$('#'+mainObjEdit.massEditActionId[i]+'_inp');
        var strId = '#' + mainObjEdit.massEditActionId[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId)) {
            construct_(iteration++, 'MassEditActionId', mainObjEdit.massEditActionId[i], null, $(checkObj).val(), false);
        }

    }
    iteration = 0;
    //отредактированные FizVels
    for (var i = 0; i < mainObjEdit.massEditFizVels.length; ++i) {
        var strId = '#' + mainObjEdit.massEditFizVels[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassEditFizVels', mainObjEdit.massEditFizVels[i], mainObjEdit.curentActionId, $(checkObj).val(), false);
    }
    iteration = 0;
    //отредактированные ParamFizVels
    for (var i = 0; i < mainObjEdit.massEditParamFizVels.length; ++i) {
        var strId = '#' + mainObjEdit.massEditParamFizVels[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassEditParamFizVels', mainObjEdit.massEditParamFizVels[i], checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //отредактированные pro
    for (var i = 0; i < mainObjEdit.massEditPros.length; ++i) {
        var strId = '#' + mainObjEdit.massEditPros[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassEditPros', mainObjEdit.massEditPros[i], checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //отредактированные spec
    for (var i = 0; i < mainObjEdit.massEditSpecs.length; ++i) {
        var strId = '#' + mainObjEdit.massEditSpecs[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassEditSpecs', mainObjEdit.massEditSpecs[i], checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //отредактированные vrem
    for (var i = 0; i < mainObjEdit.massEditVrems.length; ++i) {
        var strId = '#' + mainObjEdit.massEditVrems[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassEditVrems', mainObjEdit.massEditVrems[i], checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //удаленные pro
    for (var i = 0; i < mainObjEdit.massDeletedPros.length; ++i) {
        var strId = '#' + mainObjEdit.massDeletedPros[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassDeletedPros', mainObjEdit.massDeletedPros[i], checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //удаленные spec
    for (var i = 0; i < mainObjEdit.massDeletedSpecs.length; ++i) {
        var strId = '#' + mainObjEdit.massDeletedSpecs[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassDeletedSpecs', mainObjEdit.massDeletedSpecs[i], checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //удаленные vrem
    for (var i = 0; i < mainObjEdit.massDeletedVrems.length; ++i) {
        var strId = '#' + mainObjEdit.massDeletedVrems[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassDeletedVrems', mainObjEdit.massDeletedVrems[i], checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //удаленные fizvels
    for (var i = 0; i < mainObjEdit.massDeletedFizVels.length; ++i) {
        var strId = '#' + mainObjEdit.massDeletedFizVels[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassDeletedFizVels', mainObjEdit.massDeletedFizVels[i], checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //удаленные actionid
    for (var i = 0; i < mainObjEdit.massDeletedActionId.length; ++i) {
        var strId = '#' + mainObjEdit.massDeletedActionId[i] + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(iteration++, 'MassDeletedActionId', mainObjEdit.massDeletedActionId[i], checkObj.prop('name'), $(checkObj).val(), false);
    }


    //добавление actionId
    if (mainObjEdit.curentActionId == 'VOZ0') {

        var strId = '#VOZ0_inp';
        var checkObj = $(strId);
        if ($("input").is(strId))
            construct_(0, 'MassAddActionId', 'VOZ0', null, $(checkObj).val(), document.getElementById('changeTypeParamActionId').checked);
    }
    iteration = 0;
    //добавление FizVel
    for (var i = 1; i < mainObjEdit.maxNewFizVelId; ++i) {
        var strId = '#FizVelNameNEW' + i + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId) && $(checkObj).css('background-color') != 'rgb(128, 128, 128)')
            construct_(iteration++, 'MassAddFizVels', 'FizVelNameNEW' + i, checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    if (curentFizVel && $('#' + curentFizVel + '_inp').css('background-color') != 'rgb(128, 128, 128)')
        //добавление ParamFizVel
        for (var i = 1; i < mainObjEdit.maxNewParamFizVelId; ++i) {
            var strId = '#ParamFizVelNameNEW' + i + '_inp';
            var checkObj = $(strId);
            if ($("input").is(strId) && $(checkObj).css('background-color') != 'rgb(128, 128, 128)')
                construct_(iteration++, 'MassAddParamFizVels', 'ParamFizVelNameNEW' + i, curentFizVel, $(checkObj).val(), false);
        }

    iteration = 0;
    //добавление pro
    for (var i = 1; i < mainObjEdit.maxNewProId; ++i) {
        var strId = '#' + mainObjEdit.curentActionId + "_PROS_NEW" + i + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId) && $(checkObj).css('background-color') != 'rgb(128, 128, 128)')
            construct_(iteration++, 'MassAddPros', mainObjEdit.curentActionId + "_PROS_NEW" + i, checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //добавление spec
    for (var i = 1; i < mainObjEdit.maxNewSpecId; ++i) {
        var strId = '#' + mainObjEdit.curentActionId + "_SPEC_NEW" + i + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId) && $(checkObj).css('background-color') != 'rgb(128, 128, 128)')
            construct_(iteration++, 'MassAddSpecs', mainObjEdit.curentActionId + "_SPEC_NEW" + i, checkObj.prop('name'), $(checkObj).val(), false);
    }
    iteration = 0;
    //добавление vrem
    for (var i = 1; i < mainObjEdit.maxNewVremId; ++i) {

        var strId = '#' + mainObjEdit.curentActionId + "_VREM_NEW" + i + '_inp';
        var checkObj = $(strId);
        if ($("input").is(strId) && $(checkObj).css('background-color') != 'rgb(128, 128, 128)')
            construct_(iteration++, 'MassAddVrems', mainObjEdit.curentActionId + "_VREM_NEW" + i, checkObj.prop('name'), $(checkObj).val(), false);
    }










    goAjaxRequest({
        url: "/Physic/CreateDescription",
        data: formData,
        func_success: function (req, status, jqXHR) {
            var erdiv = document.getElementById('errorShowDiv');
            if (req == '+') {
                alert('Изменено, перезагрузите страницу');
                erdiv.style.display = 'none';
            }
            else {
                erdiv.innerHTML = req;
                erdiv.style.display = 'block';
            }

        }, type: 'POST'
    });


}


function showInfo() {
    var div = document.getElementById('divInfoBlockId');
    var but = document.getElementById('butInfoBlockId');
    if (div.style.display == 'block') {
        div.style.display = 'none';
        but.innerHTML = 'Показать информацию';
    }
    else {
        div.style.display = 'block';
        but.innerHTML = 'Скрыть информацию';
    }


}


;;;;