;;;



var countLatex = 0;

//метод для добавления нового поля latex
function addLatex() {
    var strappend = '<div id="latexImgIdDiv_' + countLatex + '"><input class="form-control" id="latexImgId_' + countLatex
        + '" type="text"/><button class="btn btn-default" onclick="deleteLatex(' + countLatex + ')">Удалить</button></div>';
    countLatex++;
    $('#physCreateFormLatexImagesImg').append(strappend);


}

//метод для удаления latex
function deleteLatex(num) {
    $('#latexImgIdDiv_' + num).remove();
}


//метод для сохранения изменения записи ФЭ
function CreatePhysFunc() {

    data_descr_search.function_trigger = function () {
        var data = new FormData();
        let massFiles = document.getElementById('upLoadImagesId').files;

        $.each(massFiles, function (key, value) {
            data.append('uploadImage', value);
        });



        for (key in data_descr_search) {
            if (key != 'function_trigger' && key != 'search') {
                if (key == 'stateBegin' || key == 'stateEnd')
                    data.append('Obj.' + key + 'Id', data_descr_search[key]);//Obj.
                else if (key == 'CountInput' || key == 'ChangedObject')
                    data.append('Obj.' + key, data_descr_search[key]);//Obj.
                else
                    data.append(key, data_descr_search[key]);

                if (data_descr_search[key] == 'undefined' || data_descr_search[key] == undefined) {
                    if (key.indexOf('state') >= 0)
                        alert('Не выбрано состояние');
                    else
                        alert('Что то пошло не так');
                    return;
                }
            }
        }


        data.append('obj.Name', document.getElementById('Name').value);
        data.append('obj.Text', document.getElementById('Text').value);
        data.append('obj.TextInp', document.getElementById('TextInp').value);
        data.append('obj.TextOut', document.getElementById('TextOut').value);
        data.append('obj.TextObj', document.getElementById('TextObj').value);
        data.append('obj.TextApp', document.getElementById('TextApp').value);
        data.append('obj.TextLit', document.getElementById('TextLit').value);

        for (let i = 0; i < countLatex; ++i) {
            let inp = document.getElementById('latexImgId_' + i);
            if (inp)
                data.append('latexformulas', inp.value);

        }

        $.ajax({
            url: '/Physic/Create',
            type: 'POST',
            data: data,
            cache: false,
            dataType: 'html',
            processData: false, // Не обрабатываем файлы 
            contentType: false, // Так jQuery скажет серверу что это строковой запрос

            complete: function (respond, textStatus, jqXHR) {
                if (respond.status == 201)
                    alert("Запись создана успешно, ссылка- " + window.location.origin + respond.responseText);
                //if (respond.status==200)
                //    alert("Запись создана успешно, ссылка- " + window.location.origin + respond.responseText);
                if (respond.status == 220)
                    alert("Запись не добавлена, поля записи или состояний заполнены не верно");
                if (respond.status == 221)
                    alert("Запись не добавлена, возникла ошибка с входными\выходными дескрипторами");
                if (respond.status == 222)
                    alert("Запись не добавлена, возникла ошибка с состоянием объекта или его характеристиками");
                if (respond.status == 225)
                    alert("Запись не добавлена, что то пошло не так");
                PreloaderShowChange(false);
            },
            beforeSend: function () {

                PreloaderShowChange(true);
            },
            error: function (xhr, status, error) {
                alert("ошибка загрузки");
            }
        });

    };
    Go_next_step();
}




;;;