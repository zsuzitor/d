/* 
������, �����������/��������� ��/�� ������������(�/�) �������� ��������� ����������� ��������������.
��������� ���� �������� (WeiLTS) � 2016
E-mail: ilyavayngolts@gmail.com
*/

// �������, ����������� �� ������������� �������� ��������� ����������� ��������������
function setChildSpec()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // ������� ��� ���������� checkbox'�
    $('#spec input:checkbox:checked').each(function ()
    {
        // �������� ���������� ��������������
        var id = $(this).val();

        // �������� ���������� �����������
        var actionId = $('#actionLabel').attr('value');

        // ��������� ������ ��� ajax-�������
        var str = id + '@' + actionId;

        // ������, ��������� �� � ������������� �������������� ��������
        var hasChild = $('#' + id).attr('name');

        if (hasChild == '0') // �� ���������
        {
            // ��������� ajax-������
            $.ajax(
            {
                type: 'GET',
                url: prefix + 'GetSpecChild/' + str,
                success: function (specChild)
                {
                    // �������� ����� �������������, ����������� �� ����������� �������� ��������������
                    $('#' + id).replaceWith(specChild);
                }
            });
        }
    });
}

// �������, ��������� �� ������������� �������� ��������� ����������� ��������������
function delChildSpec()
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // ������� ��� checkbox'�
    $('#spec input:checkbox').each(function ()
    {
        if ($(this).is(':checked') == false) // checkbox �� �������
        {
            // �������� ���������� ��������������
            var id = $(this).val();

            // ������, ��������� �� � ������������� �������������� ��������
            var hasChild = $('#' + id).attr('name');

            if (hasChild == '1') // ���������
            {
                // ��������� ajax-������
                $.ajax(
                {
                    type: 'GET',
                    url: prefix + 'GetEmptyChild/' + id,
                    success: function (emptyChild)
                    {
                        // �������� ����� �������������, ����������� �� ����������� �������� ��������������
                        $('#' + id).replaceWith(emptyChild);
                    }
                });
            }
        }
    });
}

// ���������� �� ������� change ������� setChildSpec � delChildSpec
$('#specGroup').on('change', function ()
{
    setChildSpec();
    delChildSpec();
});