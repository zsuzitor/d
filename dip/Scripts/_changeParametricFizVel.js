/* 
������, ����������� ���������� �������� ��� ������ ���������������� �����������.
��������� ���� �������� (WeiLTS) � 2016
E-mail: ilyavayngolts@gmail.com
*/

// �������, ����������� ���������� ��������
function changeFizVel()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // �������� ���������� ������� ������
    var id = $('#fizVelId').val();

    if (id == 'VOZ11_FIZVEL_R1' || id == 'VOZ11_FIZVEL_R2' || // ������ ��������������� �����������
        id == 'VOZ11_FIZVEL_R3' || id == 'VOZ11_FIZVEL_R4' ||
        id == 'VOZ11_FIZVEL_R5')
    {
        // ��������� ajax-������
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels/' + id,
            success: function (parametricFizVel)
            {
                // �������� ����� �������������, ����������� �� ����� ���������� ��������
                $('#parametricFizVel').replaceWith(parametricFizVel);
            }
        });
    }
};

// ���������� �� ������� change ������� changeFizVel
$('#fizVelGroup').on('change', function ()
{
    changeFizVel();
});