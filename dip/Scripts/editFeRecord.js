


;;;;;;

//метод для удаления изображения
function delete_img(but) {
    var id=but.id.split("_")[1];

    deleteImages.push(id);
        
    document.getElementById("div_img_hide_block_" + id).style.display = 'none';

    document.getElementById("div_img_reset_block_" + id).style.display = 'block';
}

//метод для восстановления изображения
//but-кнопка
function reset_img(but) {
    var id = but.id.split("_")[1];
        
    for(let i=0;i<deleteImages.length;++i)
        if(deleteImages[i]==id){
            old=deleteImages[i];
            deleteImages.splice(i, 1);
            break;
        }
        
    document.getElementById("div_img_hide_block_" + id).style.display = 'block';
    document.getElementById("div_img_reset_block_" + id).style.display = 'none';
        
}

//методдля восстановления latex изображения
//id-id latex
//num
function restoreLatexImg(id,num){
    var div=document.getElementById('oneLatexExist_'+id);
    if(div){
        let old;
        for(let i=0;i<deleteLatexImage.length;++i)
            if(deleteLatexImage[i].id==id){
                old=deleteLatexImage[i];
                deleteLatexImage.splice(i, 1);
                break;
            }
        var strappend = '<div id="latexImgIdDiv_' + id + '"><input  type="hidden" id="numForExistLatex_id'+num+'" value="'+id+'" /><input id="oneLatexExistInput_' + id 
       + '" value="'+old.value+'" type="text"/><button class="btn btn-default" onclick="deleteLatexImg(\''+id+'\',\''+num+'\')">Удалить</button></div>';
            
        div.innerHTML=strappend;
    }
}


//метод для удаления latex
function deleteLatexImg(id,num){
    var div=document.getElementById('oneLatexExist_'+id);
    if(div){
        let obj={
            id:id,
            value:document.getElementById('oneLatexExistInput_'+id).value
        };
        deleteLatexImage.push(obj);
        let newstr='<button class="btn btn-default" onclick="restoreLatexImg(\''+id+'\',\''+num+'\')">Восстановить</button>';
        div.innerHTML=newstr;
    }
    else{
        document.getElementById('latexImgIdDiv_'+id).remove();
    }
       
}

//метод для добавления latex
function addLatex(){
    var strappend = '<div id="latexImgIdDiv_new_' + countLatex + '"><input id="latexImgId_new_' + countLatex 
        + '" type="text"/><button class="btn btn-default" onclick="deleteLatexImg(\'new_'+countLatex+'\')">Удалить</button></div>';
    countLatex++;
    $('#physCreateFormLatexImagesImg').append(strappend);
}

//метод для сохранения изменений записи ФЭ
function EditPhysFunc() {

    data_descr_search.function_trigger = function () {

        var data = new FormData();
        let massFiles = document.getElementById('upLoadImagesId').files;

        $.each(massFiles, function (key, value) {
            data.append('uploadImage', value);
        });
        for (key in data_descr_search) {
            if (key != 'function_trigger' && key != 'search') {
                if (key == 'stateBegin' || key == 'stateEnd')
                    data.append('Obj.' + key + 'Id', data_descr_search[key]);
                else if (key == 'CountInput' || key == 'ChangedObject') {
                    data.append('Obj.' + key, data_descr_search[key]);
                }
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
        data.append('obj.IDFE', document.getElementById('IDFE').value);

        for (let i = 0; i < deleteImages.length; ++i) {

            data.append('deleteImg_', parseInt(deleteImages[i]));
        }

        //add latex
        let numForMass = 0;
        for (let i = 0; i < countLatex; ++i) {
            let inp = document.getElementById('latexImgId_new_' + i);
            if (inp) {
                data.append('latexformulas[' + numForMass + '].Text', inp.value);
                data.append('latexformulas[' + numForMass + '].Action', 0);
                ++numForMass;
            }

        }

        //change latex
        //numForMass=0;
        for (let i = 0; i < countLatexExist; ++i) {
            let inp = document.getElementById('numForExistLatex_id' + i);
            if (inp) {
                let idlatex = inp.value;
                data.append('latexformulas[' + numForMass + '].Id', idlatex);
                data.append('latexformulas[' + numForMass + '].Text', document.getElementById('oneLatexExistInput_' + idlatex).value);
                data.append('latexformulas[' + numForMass + '].Action', 1);
                ++numForMass;
            }
        }


        //delete latex
        for (let i = 0; i < deleteLatexImage.length; ++i) {

            data.append('latexformulas[' + numForMass + '].Id', deleteLatexImage[i].id);
            data.append('latexformulas[' + numForMass + '].Action', 2);
            ++numForMass;
        }


        $.ajax({
            url: '/Physic/Edit',
            type: 'POST',
            data: data,
            cache: false,
            dataType: 'html',
            processData: false, // Не обрабатываем файлы 
            contentType: false, // Так jQuery скажет серверу что это строковой запрос

            complete: function (respond, textStatus, jqXHR) {
                if (respond.status == 201)
                    alert("Запись изменена успешно, ссылка- " + window.location.origin + respond.responseText);
                //if (respond.status==200)
                //    alert("Запись создана успешно, ссылка- " + window.location.origin + respond.responseText);
                if (respond.status == 220)
                    alert("Запись не изменена, поля записи или состояний заполнены не верно");
                if (respond.status == 221)
                    alert("Запись не изменена, возникла ошибка с входными\выходными дескрипторами");
                if (respond.status == 222)
                    alert("Запись не изменена, возникла ошибка с состоянием объекта или его характеристиками");
                if (respond.status == 223)
                    alert("Запись не изменена,не найден редактируемый объект");
                if (respond.status == 224)
                    alert("Запись не изменена, данная запись редактируется только на странице полнотекстового поиска, в разделе 'семантический поиск'");
                if (respond.status == 225)
                    alert("Запись не изменена, что то пошло не так");
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




;;;;;;