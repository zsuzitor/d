
// Функция, обновляющая характеристики воздействия
// type: I -input , O-output
function changeParams(type)
{
    //var url = window.location.pathname.split('/');
    var prefix = '/Actions/';

    var postfix='?type='+type+'&';
    //for (var i = 0; i < url.length - 1; i++)
        //prefix += url[i] + '/';

    // Получаем дескриптор воздействия
    var id = $('#action'+type).val();


//data_descr_search 
            //have_input:null,
            data_descr_search.actionIdI= '';
            data_descr_search.actionTypeI= '';
            data_descr_search.FizVelIdI= '';
            data_descr_search.parametricFizVelIdI= '';
            data_descr_search.listSelectedProsI= '';
            data_descr_search.listSelectedSpecI= '';
            data_descr_search.listSelectedVremI= '';
            data_descr_search.actionIdO= '';
            data_descr_search.actionTypeO= '';
            data_descr_search.FizVelIdO= '';
            data_descr_search.parametricFizVelIdO= '';
            data_descr_search.listSelectedProsO= '';
            data_descr_search.listSelectedSpecO= '';
            data_descr_search.listSelectedVremO= '';
            data_descr_search.search= 'yes';
            //function_trigger:null
        



var formData={
fizVelId:id,
// prosId:'',
// specId:'',
// vremId:'',
type:type
};
goAjaxRequest({
    url: prefix + "ChangeAction",
    data: formData,
    func_success: function (req, status, jqXHR) {
        var data = req.split('<hr />');//.responseText
        var type = data[0].trim();
        $('#fizVel' + type).replaceWith(data[1]);
        //document.getElementById('fizVel'+type).innerHTML = data[1];

        //$('#prosGroup' + type).replaceWith(data[2]);
        document.getElementById('prosGroup' + type).innerHTML = data[2];

        //$('#specGroup' + type).replaceWith(data[3]);
        document.getElementById('specGroup' + type).innerHTML = data[3];

        // $('#vremGroup' + type).replaceWith(data[4]);
        document.getElementById('vremGroup' + type).innerHTML = data[4];

        $('#parametricFizVel' + type).replaceWith(data[5]);
        //document.getElementById('parametricFizVel' + type).innerHTML = data[5];

    }, type: 'POST'
});

};
$('#actionI').on('change', function () {
    changeParams('I');

});
$('#actionO').on('change', function () {

    changeParams('O');
});


// if (id != 'VOZ11') {// непараметрическое воздействие

// formData.prosId=id;
// formData.specId=id;
// formData.vremId=id;


// goAjaxRequest({url:prefix + 'GetParametricFizVels'+postfix+'id=' + '&type='+type,
// func_success:function (parametricFizVel)
//             {
//                 // Заменяем часть представления, отвечающего за выбор физической величины
//                 $('#parametricFizVel'+type).replaceWith(parametricFizVel);
//             }
// });


//     }
//     else{

// formData.prosId='';
// formData.specId='';
// formData.vremId='';


// goAjaxRequest({url:prefix + 'GetParametricFizVels'+postfix+'id=' + 'VOZ11_FIZVEL_R1&type='+type,
//  func_success: function(a){
// $('#parametricFizVel'+type).replaceWith(a);
//  }});


//     }




/*
            $.ajax({
                url: prefix+"ChangeAction",
                data:formData,
                
                //success: OnComplete_Add_section,//функция
                //error: function () { alert("ошибка загрузки"); },
                // shows the loader element before sending.
                //beforeSend: function () { alert("before"); },
                // hides the loader after completion of request, whether successfull or failor.             
                complete: function (req) { 
var data = req.responseText.split('<hr />');
                            var type = data[0].trim();
$('#fizVel'+type).replaceWith(data[1]);
$('#pros'+type).replaceWith(data[2]);
$('#spec'+type).replaceWith(data[3]);
                     $('#vrem'+type).replaceWith(data[4]); 
                	
                	 },
                type: 'POST', dataType: 'html'//'html'
            });

*/



/*
var xhr = new XMLHttpRequest();
xhr.open("POST", prefix+"ChangeAction");

                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4) {
                        if (xhr.status == 200) {
                            //при возврате actionresult и тд, вернет -"<html>"
                            var data = xhr.responseText.split('<hr />');
                            var type = data[0].trim();
$('#fizVel'+type).replaceWith(data[1]);
$('#pros'+type).replaceWith(data[2]);
$('#spec'+type).replaceWith(data[3]);
                     $('#vrem'+type).replaceWith(data[4]);        
                        }
                    }
                };

                xhr.send(formData);
*/


// Назначение на событие change функции changeParams


/*
    // Формируем ajax-запрос
    $.ajax(
    {
        type: 'GET',
        url: prefix + 'GetFizVels'+postfix+'id=' + id,
        success: function (fizVelId)
        {
            // Заменяем часть представления, отвечающего за выбор физической величины
            $('#fizVel'+type).replaceWith(fizVelId);
        }
    });

   
    if (id != 'VOZ11') // непараметрическое воздействие
    {
        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetPros'+postfix+'id=' + id,
            
            success: function (pros)
            {
                // Заменяем часть представления, отвечающего за выбор пространственных характеристик
                $('#pros'+type).replaceWith(pros);
            }
        });

        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetSpec'+postfix+'id=' + id,
            
            success: function (spec)
            {
                // Заменяем часть представления, отвечающего за выбор специальных характеристик
                $('#spec'+type).replaceWith(spec);
            }
        });

        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
           
			url: prefix + 'GetVrem'+postfix+'id=' + id,
            success: function (vrem)
            {
                // Заменяем часть представления, отвечающего за выбор временных характеристик
                $('#vrem'+type).replaceWith(vrem);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels'+postfix+'id=' + 'NULL',
            

            success: function (parametricFizVel)
            {
                // Заменяем часть представления, отвечающего за выбор физической величины
                $('#parametricFizVel'+type).replaceWith(parametricFizVel);
            }
        });
    }
    else
    {
        // Формируем ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetParametricFizVels'+postfix+'id=' + 'VOZ11_FIZVEL_R1',
            
            success: function (parametricFizVel)
            {
                // Заменяем часть представления, отвечающего за выбор физической величины
                $('#parametricFizVel'+type).replaceWith(parametricFizVel);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetPros'+postfix+'id=' + 'NULL',
           
            success: function (pros)
            {
                // Заменяем часть представления, отвечающего за выбор пространственных характеристик
                $('#pros'+type).replaceWith(pros);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
                type: 'GET',
                url: prefix + 'GetSpec'+postfix+'id=' + 'NULL',
                
            success: function (spec)
            {
                // Заменяем часть представления, отвечающего за выбор специальных характеристик
                $('#spec'+type).replaceWith(spec);
            }
        });

        // Формируем холостой ajax-запрос
        $.ajax(
        {
            type: 'GET',
            url: prefix + 'GetVrem'+postfix+'id=' + 'NULL',
            
            success: function (vrem)
            {
                // Заменяем часть представления, отвечающего за выбор временных характеристик
                $('#vrem'+type).replaceWith(vrem);
            }
        });
    }
    */
