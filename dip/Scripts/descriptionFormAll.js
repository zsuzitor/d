;;;

function Go_next_step() {
    //TODO
    var i = 0;
    var inpdiv = document.getElementById('actionI' + i);
    var twoInput = document.getElementById('changeTwoInputFormDescr_id').checked;


    data_descr_search = {
        search: data_descr_search.search,
        function_trigger: data_descr_search.function_trigger
    };



    data_descr_search['CountInput'] = twoInput ? 2 : 1;

    var massParamAction = document.getElementById('parametric_action_or_not').value.split(' ');


    while (inpdiv && (twoInput || i == 0)) {
        let valActionId = $('#actionI' + i).val();
        data_descr_search['forms[' + i + '].ActionId'] = UndefinedToStr(valActionId);

        data_descr_search['forms[' + i + '].Parametric'] = massParamAction.includes(valActionId);
        data_descr_search['forms[' + i + '].ActionType'] = UndefinedToStr($('#actionTypeI' + i).val());
        data_descr_search['forms[' + i + '].FizVelId'] = UndefinedToStr($('#fizVelIdI' + i).val());
        data_descr_search['forms[' + i + '].ParametricFizVelId'] = UndefinedToStr($('#parametricFizVelIdI' + i).val());
        data_descr_search['forms[' + i + '].InputForm'] = true;

        //checkbox
        //pros
        data_descr_search['forms[' + i + '].ListSelectedPros'] = '';
        $('#prosGroupI' + i + ' input:checkbox:checked').each(function () {//[type=checkbox]
            data_descr_search['forms[' + i + '].ListSelectedPros'] += $(this).attr('value') + ' ';
        });
        data_descr_search['forms[' + i + '].ListSelectedPros'] = data_descr_search['forms[' + i + '].ListSelectedPros'].trim();
        //spec
        data_descr_search['forms[' + i + '].ListSelectedSpec'] = '';
        $('#specGroupI' + i + ' input:checkbox:checked').each(function () {//[type=checkbox]
            data_descr_search['forms[' + i + '].ListSelectedSpec'] += $(this).attr('value') + ' ';
        });
        data_descr_search['forms[' + i + '].ListSelectedSpec'] = data_descr_search['forms[' + i + '].ListSelectedSpec'].trim();
        //vrem
        data_descr_search['forms[' + i + '].ListSelectedVrem'] = '';
        $('#vremGroupI' + i + ' input:checkbox:checked').each(function () {//[type=checkbox]
            data_descr_search['forms[' + i + '].ListSelectedVrem'] += $(this).attr('value') + ' ';
        });
        data_descr_search['forms[' + i + '].ListSelectedVrem'] = data_descr_search['forms[' + i + '].ListSelectedVrem'].trim();

        inpdiv = document.getElementById('actionI' + ++i);
    }

    var startOutput = i;
    i = 0;
    inpdiv = document.getElementById('actionO' + i);

    
    while (inpdiv) {
        let valActionId = $('#actionO' + i).val();
        data_descr_search['forms[' + startOutput + '].ActionId'] = UndefinedToStr(valActionId);

        data_descr_search['forms[' + startOutput + '].Parametric'] = massParamAction.includes(valActionId);
        data_descr_search['forms[' + startOutput + '].ActionType'] = UndefinedToStr($('#actionTypeO' + i).val());
        data_descr_search['forms[' + startOutput + '].FizVelId'] = UndefinedToStr($('#fizVelIdO' + i).val());
        data_descr_search['forms[' + startOutput + '].ParametricFizVelId'] = UndefinedToStr($('#parametricFizVelIdO' + i).val());
        data_descr_search['forms[' + startOutput + '].InputForm'] = false;

        //checkbox
        //pros
        data_descr_search['forms[' + startOutput + '].ListSelectedPros'] = '';
        $('#prosGroupO' + i + ' input:checkbox:checked').each(function () {//[type=checkbox]
            data_descr_search['forms[' + startOutput + '].ListSelectedPros'] += $(this).attr('value') + ' ';
        });
        data_descr_search['forms[' + startOutput + '].ListSelectedPros'] = data_descr_search['forms[' + startOutput + '].ListSelectedPros'].trim();
        //spec
        data_descr_search['forms[' + startOutput + '].ListSelectedSpec'] = '';
        $('#specGroupO' + i + ' input:checkbox:checked').each(function () {//[type=checkbox]
            data_descr_search['forms[' + startOutput + '].ListSelectedSpec'] += $(this).attr('value') + ' ';
        });
        data_descr_search['forms[' + startOutput + '].ListSelectedSpec'] = data_descr_search['forms[' + startOutput + '].ListSelectedSpec'].trim();
        //vrem
        data_descr_search['forms[' + startOutput + '].ListSelectedVrem'] = '';
        $('#vremGroupO' + i + ' input:checkbox:checked').each(function () {//[type=checkbox]
            data_descr_search['forms[' + startOutput + '].ListSelectedVrem'] += $(this).attr('value') + ' ';
        });
        data_descr_search['forms[' + startOutput + '].ListSelectedVrem'] = data_descr_search['forms[' + startOutput + '].ListSelectedVrem'].trim();

        inpdiv = document.getElementById('actionO' + ++i);
        startOutput++;
    }



    //OBJECT

    var twoObject = document.getElementById('changeObjectCheckbox').checked;
    data_descr_search['ChangedObject'] = twoObject;
    i = 0;
    inpdiv = document.getElementById('PhaseObject_data_S_' + i);


    //состояния
    {
        let funcGetState = function (mass) {
            for (let iter = 0; iter < mass.length; ++iter) {
                let searchFlag = false;
                for (let iter2 = 0; iter2 < mass.length; ++iter2) {
                    if (mass[iter2].name == mass[iter].id) {
                        searchFlag = true;
                        break;
                    }

                }
                if (!searchFlag) {
                    return mass[iter].id.split('_')[1];
                }
            }
        };



        let massStatesS = [];
        $('#mainBlockStates_S input:radio:checked').each(function () {//[type=checkbox] 
            massStatesS.push({ id: this.id.split('change_state_radio_id_')[1], name: this.name.split('change_state_radio_')[1] });//(this.id.split('change_state_radio_id_')[1]);
        });
        data_descr_search.stateBegin = funcGetState(massStatesS);


        if (twoObject) {
            let massStatesE = [];
            $('#mainBlockStates_E input:radio:checked').each(function () {//[type=checkbox] 
                massStatesE.push({ id: this.id.split('change_state_radio_id_')[1], name: this.name.split('change_state_radio_')[1] });//(this.id.split('change_state_radio_id_')[1]);
            });
            data_descr_search.stateEnd = funcGetState(massStatesE);

        }



    }

    //начальное
    //по фазам
    data_descr_search['objForms[' + 0 + '].Begin'] = true;

    while (inpdiv && inpdiv.innerHTML.trim()) {
        let valPhase = 'objForms[' + 0 + '].ListSelectedPhase' + (i + 1) + '.';
        data_descr_search[valPhase + 'NumPhase'] = (i + 1);
        data_descr_search[valPhase + 'Begin'] = 1;
        $('#PhaseObject_data_S_' + i + ' input:checkbox:checked').each(function () {//[type=checkbox]

            let thisId = this.value.split('_')[4];
            let mainParentId = thisId[0];
            switch (mainParentId) {
                case 'C':
                    if (!data_descr_search[valPhase + 'Special'])
                        data_descr_search[valPhase + 'Special'] = '';
                    data_descr_search[valPhase + 'Special'] += thisId + ' ';
                    break;
                case 'D':
                    if (!data_descr_search[valPhase + 'MechanicalState'])
                        data_descr_search[valPhase + 'MechanicalState'] = '';
                    data_descr_search[valPhase + 'MechanicalState'] += thisId + ' ';
                    break;
                case 'E':
                    if (!data_descr_search[valPhase + 'Conductivity'])
                        data_descr_search[valPhase + 'Conductivity'] = '';
                    data_descr_search[valPhase + 'Conductivity'] += thisId + ' ';
                    break;
                case 'F':
                    if (!data_descr_search[valPhase + 'PhaseState'])
                        data_descr_search[valPhase + 'PhaseState'] = '';
                    data_descr_search[valPhase + 'PhaseState'] += thisId + ' ';

                    break;
                case 'M':
                    if (!data_descr_search[valPhase + 'MagneticStructure'])
                        data_descr_search[valPhase + 'MagneticStructure'] = '';
                    data_descr_search[valPhase + 'MagneticStructure'] += thisId + ' ';
                    break;
                case 'O':
                    if (!data_descr_search[valPhase + 'OpticalState'])
                        data_descr_search[valPhase + 'OpticalState'] = '';
                    data_descr_search[valPhase + 'OpticalState'] += thisId + ' ';
                    break;
                case 'X':
                    if (!data_descr_search[valPhase + 'Composition'])
                        data_descr_search[valPhase + 'Composition'] = '';
                    data_descr_search[valPhase + 'Composition'] += thisId + ' ';
                    break;
            }


        });
        if (!data_descr_search[valPhase + 'Special'])
            data_descr_search[valPhase + 'Special'] = '';
        else
            data_descr_search[valPhase + 'Special'] = data_descr_search[valPhase + 'Special'].trim();

        if (!data_descr_search[valPhase + 'MechanicalState'])
            data_descr_search[valPhase + 'MechanicalState'] = '';
        else
            data_descr_search[valPhase + 'MechanicalState'] = data_descr_search[valPhase + 'MechanicalState'].trim();
        if (!data_descr_search[valPhase + 'Conductivity'])
            data_descr_search[valPhase + 'Conductivity'] = '';
        else
            data_descr_search[valPhase + 'Conductivity'] = data_descr_search[valPhase + 'Conductivity'].trim();
        if (!data_descr_search[valPhase + 'PhaseState'])
            data_descr_search[valPhase + 'PhaseState'] = '';
        else
            data_descr_search[valPhase + 'PhaseState'] = data_descr_search[valPhase + 'PhaseState'].trim();
        if (!data_descr_search[valPhase + 'MagneticStructure'])
            data_descr_search[valPhase + 'MagneticStructure'] = '';
        else
            data_descr_search[valPhase + 'MagneticStructure'] = data_descr_search[valPhase + 'MagneticStructure'].trim();
        if (!data_descr_search[valPhase + 'OpticalState'])
            data_descr_search[valPhase + 'OpticalState'] = '';
        else
            data_descr_search[valPhase + 'OpticalState'] = data_descr_search[valPhase + 'OpticalState'].trim();
        if (!data_descr_search[valPhase + 'Composition'])
            data_descr_search[valPhase + 'Composition'] = '';
        else
            data_descr_search[valPhase + 'Composition'] = data_descr_search[valPhase + 'Composition'].trim();



        inpdiv = document.getElementById('PhaseObject_data_S_' + ++i);
        startOutput++;
    }

    i = 0;
    inpdiv = document.getElementById('PhaseObject_data_E_' + i);

    //конечное
    //по фазам
    if (twoObject) {
        data_descr_search['objForms[' + 1 + '].Begin'] = false;

        while (inpdiv && inpdiv.innerHTML.trim()) {
            let valPhase = 'objForms[' + 1 + '].ListSelectedPhase' + (i + 1) + '.';
            data_descr_search[valPhase + 'Begin'] = 0;
            $('#PhaseObject_data_E_' + i + ' input:checkbox:checked').each(function () {//[type=checkbox]

                data_descr_search[valPhase + 'NumPhase'] = (i + 1);
                let thisId = this.value.split('_')[4];
                let mainParentId = thisId[0];
                switch (mainParentId) {
                    case 'C':
                        if (!data_descr_search[valPhase + 'Special'])
                            data_descr_search[valPhase + 'Special'] = '';
                        data_descr_search[valPhase + 'Special'] += thisId + ' ';
                        break;
                    case 'D':
                        if (!data_descr_search[valPhase + 'MechanicalState'])
                            data_descr_search[valPhase + 'MechanicalState'] = '';
                        data_descr_search[valPhase + 'MechanicalState'] += thisId + ' ';
                        break;
                    case 'E':
                        if (!data_descr_search[valPhase + 'Conductivity'])
                            data_descr_search[valPhase + 'Conductivity'] = '';
                        data_descr_search[valPhase + 'Conductivity'] += thisId + ' ';
                        break;
                    case 'F':
                        if (!data_descr_search[valPhase + 'PhaseState'])
                            data_descr_search[valPhase + 'PhaseState'] = '';
                        data_descr_search[valPhase + 'PhaseState'] += thisId + ' ';

                        break;
                    case 'M':
                        if (!data_descr_search[valPhase + 'MagneticStructure'])
                            data_descr_search[valPhase + 'MagneticStructure'] = '';
                        data_descr_search[valPhase + 'MagneticStructure'] += thisId + ' ';
                        break;
                    case 'O':
                        if (!data_descr_search[valPhase + 'OpticalState'])
                            data_descr_search[valPhase + 'OpticalState'] = '';
                        data_descr_search[valPhase + 'OpticalState'] += thisId + ' ';
                        break;
                    case 'X':
                        if (!data_descr_search[valPhase + 'Composition'])
                            data_descr_search[valPhase + 'Composition'] = '';
                        data_descr_search[valPhase + 'Composition'] += thisId + ' ';
                        break;
                }

            });

            if (!data_descr_search[valPhase + 'Special'])
                data_descr_search[valPhase + 'Special'] = '';
            else
                data_descr_search[valPhase + 'Special'] = data_descr_search[valPhase + 'Special'].trim();

            if (!data_descr_search[valPhase + 'MechanicalState'])
                data_descr_search[valPhase + 'MechanicalState'] = '';
            else
                data_descr_search[valPhase + 'MechanicalState'] = data_descr_search[valPhase + 'MechanicalState'].trim();
            if (!data_descr_search[valPhase + 'Conductivity'])
                data_descr_search[valPhase + 'Conductivity'] = '';
            else
                data_descr_search[valPhase + 'Conductivity'] = data_descr_search[valPhase + 'Conductivity'].trim();
            if (!data_descr_search[valPhase + 'PhaseState'])
                data_descr_search[valPhase + 'PhaseState'] = '';
            else
                data_descr_search[valPhase + 'PhaseState'] = data_descr_search[valPhase + 'PhaseState'].trim();
            if (!data_descr_search[valPhase + 'MagneticStructure'])
                data_descr_search[valPhase + 'MagneticStructure'] = '';
            else
                data_descr_search[valPhase + 'MagneticStructure'] = data_descr_search[valPhase + 'MagneticStructure'].trim();
            if (!data_descr_search[valPhase + 'OpticalState'])
                data_descr_search[valPhase + 'OpticalState'] = '';
            else
                data_descr_search[valPhase + 'OpticalState'] = data_descr_search[valPhase + 'OpticalState'].trim();
            if (!data_descr_search[valPhase + 'Composition'])
                data_descr_search[valPhase + 'Composition'] = '';
            else
                data_descr_search[valPhase + 'Composition'] = data_descr_search[valPhase + 'Composition'].trim();



            inpdiv = document.getElementById('PhaseObject_data_E_' + ++i);
            startOutput++;
        }
    }


    if (data_descr_search.function_trigger != null)
        data_descr_search.function_trigger();

}

;;