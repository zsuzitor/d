
// �������, ����������� ���������� ��������
function changeFizVel(type)
{
    //var url = window.location.pathname.split('/');
    var prefix = '/Actions/';
    //var prefix = '';
    var postfix='?type='+type;
    //for (var i = 0; i < url.length - 1; i++)
        //prefix += url[i] + '/';

    // �������� ���������� ������� ������
    var id = $('#fizVelId'+type).val();
    //TODO ���� �������� �� �� �������

    var checkParametrics = document.getElementById('parametric_action_or_not').value.split(' ');
    var actionId = document.getElementById('action'+type);
    for (var i = 0; i < checkParametrics.length; ++i) {
        if (checkParametrics[i] == actionId.value) {
            goAjaxRequest({
                url: prefix + 'GetParametricFizVels/' + id + postfix,
                func_success: function (data, status, jqXHR) {
                    // �������� ����� �������������, ����������� �� ����� ���������� ��������
                    //$('#parametricFizVel' + type).replaceWith(data);
                    document.getElementById('parametricFizVel' + type).innerHTML = data;
                }
            });
            break;
        }

    }
    //if (checkParametric.value == true) //{

    //}
    //if (id == 'VOZ11_FIZVEL_R1' || id == 'VOZ11_FIZVEL_R2' || // ������ ��������������� �����������
    //    id == 'VOZ11_FIZVEL_R3' || id == 'VOZ11_FIZVEL_R4' ||
    //    id == 'VOZ11_FIZVEL_R5')
    {


//goAjaxRequest({url:prefix + 'GetParametricFizVels/' + id+postfix,
//    func_success: function (data, status, jqXHR)
//            {
//                // �������� ����� �������������, ����������� �� ����� ���������� ��������
//                $('#parametricFizVel'+type).replaceWith(data);
//            }});


/*
        // ��������� ajax-������
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels/' + id+postfix,
            success: function (parametricFizVel)
            {
                // �������� ����� �������������, ����������� �� ����� ���������� ��������
                $('#parametricFizVel'+type).replaceWith(parametricFizVel);
            }
        });*/
    }
};




$(document).ready(function () {

    var i = 0;
    var inpdiv = document.getElementById('fizVelGroupI' + i);
    while (inpdiv) {
        $('#fizVelGroupI' + i).on('change', function () {
            changeFizVel('I' + i);

        });
        inpdiv = document.getElementById('fizVelGroupI' + ++i);
    }
    i = 0;
    inpdiv = document.getElementById('fizVelGroupO' + i);
    while (inpdiv) {
        $('#fizVelGroupO' + i).on('change', function () {
            changeFizVel('O' + i);

        });
        inpdiv = document.getElementById('fizVelGroupO' + ++i);
    }

});


// ���������� �� ������� change ������� changeFizVel
//$('#fizVelGroupI').on('change', function ()
//{
//    changeFizVel('I');

//});
//$('#fizVelGroupO').on('change', function ()
//{
//    changeFizVel('O');
//});