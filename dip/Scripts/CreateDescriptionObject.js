/*файл скриптов для изменения дескрипторов объекта
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

;;;

var mainObjEdit = {


    massEditState: [],
    massEditPhase: [],

    //massDeletedState: [],
    massDeletedPhase: [],

    massOldValue: [],

    maxNewPhase: 1,

};



//событие изменения чекбоксов
$(document).on('change', ':checkbox', function () {//$('input[type=radio][name=bedStatus]')
    var valSplit = this.value.split('_');
    var formData = {
        id: valSplit[3]

    };

    switch (this.name) {
        case 'change_state_checbox':
            if (this.checked) {
                // отправлять только если это не новый элемент
                if (this.value.indexOf('NEW') < 0) {
                    goAjaxRequest({
                        url: "/Actions/GetStateObjectEdit",
                        data: formData,
                        func_success: function (req, status, jqXHR) {

                            document.getElementById('stateChilds_' + valSplit[3]).innerHTML = req;

                        }, type: 'GET'
                    });
                }
                else {
                    let prId = this.value.split('_')[3];
                    document.getElementById('stateChilds_' + valSplit[3]).innerHTML = '<div class="objectInputChilds" id="phaseChildsAll_"' + prId + '>' +
   '<div id="phaseChildsInner_"' + prId + '></div>' +
      '<button class="btn btn-default" onclick="addNewPhase(' + prId + ')">Добавить</button></div>';

                }
            }
            else
                document.getElementById('stateChilds_' + valSplit[3]).innerHTML = '';
            break;

        case 'change_phase_checbox':

            if (this.checked) {
                goAjaxRequest({
                    url: "/Actions/GetPhaseObjectEdit",
                    data: formData,
                    func_success: function (req, status, jqXHR) {
                        document.getElementById('phaseChilds_' + valSplit[3]).innerHTML = req;
                    }, type: 'GET'
                });
            }
            else
                document.getElementById('phaseChilds_' + valSplit[3]).innerHTML = '';

            break;
    }
});



//мето добавления новой фазы
function addNewPhase(id) {
    var div = document.getElementById('phaseChildsInner_' + id);

    let elem = '<div><input id="inputPhaseCheckbox_NEW' + mainObjEdit.maxNewPhase + '" class="checkBoxClass" type="checkbox" name="change_phase_checbox" value="change_phase_checbox_NEW' + mainObjEdit.maxNewPhase + '"/>' +
            '<button class="btn btn-default" id="NEW' + mainObjEdit.maxNewPhase + '_delbut" onclick="deletePhase(\'NEW' + mainObjEdit.maxNewPhase + '\')">Удалить</button>' +
            '<input class="form-control" type="text" name="' + id + '" id="inputPhaseText_NEW' + mainObjEdit.maxNewPhase + '" value=""/>' +
            '<div><div id="phaseChilds_NEW' + mainObjEdit.maxNewPhase + '"></div></div></div>'
    $(div).append(elem);

    mainObjEdit.maxNewPhase++;
}

//метод для изменения состояния редактируемости состояния объекта
//id-id состояния
function changeState(id) {

    var inp = $('#inputStateText_' + id);
    var inpVal = inp.val();
    if ($(inp).prop('readonly')) {
        $(inp).removeProp('readonly');
        if (!mainObjEdit.massEditState.includes(id)) {
            mainObjEdit.massOldValue.push({
                id: id,
                val: inpVal
            });
            mainObjEdit.massEditState.push(id);

        }
    }
    else {
        $(inp).prop('readonly', true);
        for (var i = 0; i < mainObjEdit.massOldValue.length; ++i)
            if (mainObjEdit.massOldValue[i].id == id) {
                if (mainObjEdit.massOldValue[i].val == inpVal) {
                    restoreStateValue(id);
                    return;
                }
                break;
            }
    }
    document.getElementById('restoreInputState_but_' + id).style.display = 'inline';
}

//метод для изменения состояния редактируемости характеристики объекта
//id-id состояния
function changePhase(id) {

    var inp = $('#inputPhaseText_' + id);
    var inpVal = inp.val();
    if ($(inp).prop('readonly')) {
        $(inp).removeProp('readonly');
        if (!mainObjEdit.massEditPhase.includes(id)) {
            mainObjEdit.massOldValue.push({
                id: id,
                val: inpVal
            });
            mainObjEdit.massEditPhase.push(id);
        }
    }
    else {
        $(inp).prop('readonly', true);
        for (var i = 0; i < mainObjEdit.massOldValue.length; ++i)
            if (mainObjEdit.massOldValue[i].id == id) {
                if (mainObjEdit.massOldValue[i].val == inpVal) {
                    restorePhaseValue(id);
                    return;
                }
                break;
            }
    }
    document.getElementById('restoreInputPhase_but_' + id).style.display = 'inline';
}




//мето для удаления характеристики объекта
//id-id характеристики
function deletePhase(id) {
    var delbut = document.getElementById(id + '_delbut');
    var checkboxChilds = $('#inputPhaseCheckbox_' + id);
    var inp = $('#inputPhaseText_' + id);
    if (delbut.innerHTML.trim() == 'Удалить') {
        //"удаляем"
        if (id.indexOf('NEW') < 0) {
            mainObjEdit.massDeletedPhase.push(id);

        }

        delbut.innerHTML = 'Восстановить';
        var divChilds = document.getElementById('phaseChilds_' + id);
        divChilds.innerHTML = '';
        $(checkboxChilds).removeAttr("checked")
        $(checkboxChilds).css('display', 'none');
        $(inp).css('background-color', 'grey');
        $(inp).prop('readonly', true);

    }
    else {
        //восстанавливаем
        if (id.indexOf('NEW') < 0) {
            for (var i = 0; i < mainObjEdit.massDeletedPhase.length; ++i) {
                if (mainObjEdit.massDeletedPhase[i] == id) {
                    mainObjEdit.massDeletedPhase.splice(i, 1);
                    break;
                }
            }
        }
        else {
            $(inp).removeProp('readonly');
        }
        delbut.innerHTML = 'Удалить';
        $(checkboxChilds).css('display', 'inline');
        $(inp).css('background-color', 'white');

    }
}


//метод для восстановления значения характеристики
//id характеристики значение которой нужно восстановить
function restorePhaseValue(id) {

    for (var i = 0; i < mainObjEdit.massOldValue.length; ++i) {
        if (mainObjEdit.massOldValue[i].id == id) {
            document.getElementById('inputPhaseText_' + id).value = mainObjEdit.massOldValue[i].val;
            mainObjEdit.massOldValue.splice(i, 1);

            for (var i2 = 0; i2 < mainObjEdit.massEditPhase.length; ++i2) {
                if (mainObjEdit.massEditPhase[i2] == id) {
                    mainObjEdit.massEditPhase.splice(i2, 1);
                    break;
                }
            }
            break;
        }


    }
    var inp = $('#inputPhaseText_' + id);
    $(inp).prop('readonly', true);
    document.getElementById('restoreInputPhase_but_' + id).style.display = 'none';

}


//метод для восстановления значения состояния
//id состояния значение которого нужно восстановить
function restoreStateValue(id) {

    for (var i = 0; i < mainObjEdit.massOldValue.length; ++i) {
        if (mainObjEdit.massOldValue[i].id == id) {
            document.getElementById('inputStateText_' + id).value = mainObjEdit.massOldValue[i].val;
            mainObjEdit.massOldValue.splice(i, 1);

            for (var i2 = 0; i2 < mainObjEdit.massEditState.length; ++i2) {
                if (mainObjEdit.massEditState[i2] == id) {
                    mainObjEdit.massEditState.splice(i2, 1);
                    break;
                }
            }
            break;
        }


    }
    var inp = $('#inputStateText_' + id);
    $(inp).prop('readonly', true);
    document.getElementById('restoreInputState_but_' + id).style.display = 'none';

}


//метод сохранения формы
function saveForm() {
    formData = {};

    let iterAdd = 0;
    for (let i = 1; i < mainObjEdit.maxNewPhase; ++i) {
        let inp = document.getElementById('inputPhaseText_NEW' + i);
        if (inp) {
            formData['obj.MassAddCharacteristic[' + iterAdd + '].Text'] = inp.value;
            formData['obj.MassAddCharacteristic[' + iterAdd + '].ParentId'] = inp.name;
            formData['obj.MassAddCharacteristic[' + iterAdd + '].Id'] = 'NEW' + i;
            iterAdd++;
        }
    }

    iterAdd = 0;
    for (let i = 0; i < mainObjEdit.massEditPhase.length; ++i) {
        let inp = document.getElementById('inputPhaseText_' + mainObjEdit.massEditPhase[i]);
        if (inp) {
            formData['obj.MassEditCharacteristic[' + iterAdd + '].Text'] = inp.value;
            formData['obj.MassEditCharacteristic[' + iterAdd + '].ParentId'] = inp.name;
            formData['obj.MassEditCharacteristic[' + iterAdd + '].Id'] = inp.id.split('_')[1];
            iterAdd++;
        }
    }

    iterAdd = 0;
    for (let i = 0; i < mainObjEdit.massEditState.length; ++i) {
        let inp = document.getElementById('inputStateText_' + mainObjEdit.massEditState[i]);
        if (inp) {
            formData['obj.MassEditState[' + iterAdd + '].Text'] = inp.value;
            formData['obj.MassEditState[' + iterAdd + '].ParentId'] = inp.name;
            formData['obj.MassEditState[' + iterAdd + '].Id'] = inp.id.split('_')[1];
            iterAdd++;
        }
    }

    iterAdd = 0;
    for (let i = 0; i < mainObjEdit.massDeletedPhase.length; ++i) {
        let inp = document.getElementById('inputPhaseText_' + mainObjEdit.massDeletedPhase[i]);
        if (inp) {
            formData['obj.MassDeleteCharacteristic[' + iterAdd + '].Text'] = inp.value;
            formData['obj.MassDeleteCharacteristic[' + iterAdd + '].ParentId'] = inp.name;
            formData['obj.MassDeleteCharacteristic[' + iterAdd + '].Id'] = inp.id.split('_')[1];
            iterAdd++;
        }
    }



    goAjaxRequest({
        url: "/Physic/CreateDescriptionObject",
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



//функция для отображения информации о странице
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

;;;