
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

    if (id == 'VOZ11_FIZVEL_R1' || id == 'VOZ11_FIZVEL_R2' || // ������ ��������������� �����������
        id == 'VOZ11_FIZVEL_R3' || id == 'VOZ11_FIZVEL_R4' ||
        id == 'VOZ11_FIZVEL_R5')
    {


goAjaxRequest({url:prefix + 'GetParametricFizVels/' + id+postfix,
 func_success: function (parametricFizVel)
            {
                // �������� ����� �������������, ����������� �� ����� ���������� ��������
                $('#parametricFizVel'+type).replaceWith(parametricFizVel);
            }});


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

// ���������� �� ������� change ������� changeFizVel
$('#fizVelGroupI').on('change', function ()
{
    changeFizVel('I');

});
$('#fizVelGroupO').on('change', function ()
{
    changeFizVel('O');
});