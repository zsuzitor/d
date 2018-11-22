

// �������, ����������� �� ������������� �������� ��������� ����������� ��������������
function setChildSpec(type)
{
    //var url = window.location.pathname.split('/');
    //var prefix = '';
    var postfix='?type='+type;
    var prefix = '/Actions/';
    //for (var i = 0; i < url.length - 1; i++)
        //prefix += url[i] + '/';

    // ������� ��� ���������� checkbox'�
    $('#spec'+type+' input:checkbox:checked').each(function ()
    {
        // �������� ���������� ��������������
        var id = $(this).val();

        // �������� ���������� �����������
        var actionId = $('#actionLabel'+type).attr('value');

        // ��������� ������ ��� ajax-�������
        var str = id + '@' + actionId;

        // ������, ��������� �� � ������������� �������������� ��������
        var hasChild = $('#' + id+type).attr('name');

        if (hasChild == '0') // �� ���������
        {



goAjaxRequest({url:prefix + 'GetSpecChild'+postfix+'&id=' + str,
 func_success: function (specChild)
                {
                    // �������� ����� �������������, ����������� �� ����������� �������� ��������������
                    $('#' + id+type).replaceWith(specChild);
                }});





/*
            // ��������� ajax-������
            $.ajax(
            {
                type: 'GET',
                url: prefix + 'GetSpecChild'+postfix+'&id=' + str,
                //url: prefix + 'GetSpecChild/' + str+postfix,
                success: function (specChild)
                {
                    // �������� ����� �������������, ����������� �� ����������� �������� ��������������
                    $('#' + id+type).replaceWith(specChild);
                }
            });*/
        }
    });
}

// �������, ��������� �� ������������� �������� ��������� ����������� ��������������
function delChildSpec(type)
{
    var url = window.location.pathname.split('/');
    var prefix = '';
    var postfix='?type='+type;
    for (var i = 0; i < url.length - 1; i++)
        prefix += url[i] + '/';

    // ������� ��� checkbox'�
    $('#spec'+type+' input:checkbox').each(function ()
    {
        if ($(this).is(':checked') == false) // checkbox �� �������
        {
            // �������� ���������� ��������������
            var id = $(this).val();

            // ������, ��������� �� � ������������� �������������� ��������
            var hasChild = $('#' + id+type).attr('name');

            if (hasChild == '1') // ���������
            {


goAjaxRequest({url:prefix + 'GetEmptyChild'+postfix+'&id=' + id,
 func_success: function (emptyChild)
                    {
                        // �������� ����� �������������, ����������� �� ����������� �������� ��������������
                        $('#' + id+type).replaceWith(emptyChild);
                    }});


/*
                // ��������� ajax-������
                $.ajax(
                {
                    type: 'GET',
                    url: prefix + 'GetEmptyChild'+postfix+'&id=' + id,
                    //url: prefix + 'GetEmptyChild/' + id+postfix,
                    success: function (emptyChild)
                    {
                        // �������� ����� �������������, ����������� �� ����������� �������� ��������������
                        $('#' + id+type).replaceWith(emptyChild);
                    }
                });*/
            }
        }
    });
}

// ���������� �� ������� change ������� setChildSpec � delChildSpec
$('#specGroupI').on('change', function ()
{
    setChildSpec('I');
    delChildSpec('I');

});
$('#specGroupO').on('change', function ()
{

    setChildSpec('O');
    delChildSpec('O');
});