
// Функция, обновляющий физические величины
function changeFizVel(type)
{
    var prefix = '/Actions/';
    var postfix='?type='+type;

    // Получаем дескриптор раздела физики
    var id = $('#fizVelId'+type).val();

    var checkParametrics = document.getElementById('parametric_action_or_not').value.split(' ');
    var actionId = document.getElementById('action'+type);
    for (var i = 0; i < checkParametrics.length; ++i) {
        if (checkParametrics[i] == actionId.value) {
            goAjaxRequest({
                url: prefix + 'GetParametricFizVels/' + id + postfix,
                func_success: function (data, status, jqXHR) {
                    document.getElementById('parametricFizVel' + type).innerHTML = data;
                }
            });
            break;
        }
    }
};




$(document).ready(function () {

    var i = 0;
    var inpdiv = document.getElementById('fizVelGroupI' + i);
    while (inpdiv) {
        $('#fizVelGroupI' + i).on('change', function () {
            var type = this.id.split('fizVelGroupI')[1];
            changeFizVel('I' + type);
        });
        inpdiv = document.getElementById('fizVelGroupI' + ++i);
    }
    i = 0;
    inpdiv = document.getElementById('fizVelGroupO' + i);
    while (inpdiv) {
        $('#fizVelGroupO' + i).on('change', function () {
            var type = this.id.split('fizVelGroupO')[1];
            changeFizVel('O' + type);
        });
        inpdiv = document.getElementById('fizVelGroupO' + ++i);
    }
});

