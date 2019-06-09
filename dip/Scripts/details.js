;;;



var detailsCurrentLiMenuId = 'detailsUl_text';

//событие загрузки страницы
$(document).ready(function () {
    let newhref = '/Physic/Details/' + document.getElementById('inputIdCurrentPhysEffect').value;
    if (document.location.pathname != newhref)
        history.pushState(null, null, newhref);

});


//метод для изменения границ элемента
function hidenLastOnLiMenu() {
    var li = document.getElementById(detailsCurrentLiMenuId);
    li.style.borderBottom = '';
    var cr = li.id.split('_')[1];
    var div = document.getElementById('detailsdiv_' + cr);
    div.style.display = 'none';

}

//метод для загрузки текстового описания записи ФЭ
function detailsLoadText() {
    hidenLastOnLiMenu();
    detailsCurrentLiMenuId = 'detailsUl_text';
    var li = document.getElementById('detailsUl_text');
    li.style.borderBottom = '3px solid red';
    var cr = li.id.split('_')[1];
    var div = document.getElementById('detailsdiv_' + cr);
    div.style.display = 'block';


}

//метод для загрузки дескрипторного(вход\выход) описания записи ФЭ
function detailsLoadDescr() {
    hidenLastOnLiMenu();
    detailsCurrentLiMenuId = 'detailsUl_descr';
    var li = document.getElementById('detailsUl_descr');
    li.style.borderBottom = '3px solid red';
    var cr = li.id.split('_')[1];
    var div = document.getElementById('detailsdiv_' + cr);
    div.style.display = 'block';
    if (!div.innerHTML.trim()) {
        //загрузить
        goAjaxRequest({
            url: '/Actions/LoadDescr',
            type: 'GET',
            data: { id: document.getElementById('inputIdCurrentPhysEffect').value },
            func_success: function (data, status, jqXHR) {
                div.innerHTML = data;
            }
        });
    }
}


//метод для загрузки дескрипторного(объект) описания записи ФЭ
function detailsLoadObj() {
    hidenLastOnLiMenu();
    detailsCurrentLiMenuId = 'detailsUl_obj';
    var li = document.getElementById('detailsUl_obj');
    li.style.borderBottom = '3px solid red';
    var cr = li.id.split('_')[1];
    var div = document.getElementById('detailsdiv_' + cr);
    div.style.display = 'block';
    if (!div.innerHTML.trim()) {
        //загрузить
        goAjaxRequest({
            url: '/Actions/LoadObject',
            type: 'GET',
            data: { id: document.getElementById('inputIdCurrentPhysEffect').value },
            func_success: function (data, status, jqXHR) {
                div.innerHTML = data;
            }
        });
    }
}


//метод для загрузки похожих записей
function detailsLoadSimilar() {
    hidenLastOnLiMenu();
    detailsCurrentLiMenuId = 'detailsUl_similar';
    var li = document.getElementById('detailsUl_similar');
    li.style.borderBottom = '3px solid red';
    var cr = li.id.split('_')[1];
    var div = document.getElementById('detailsdiv_' + cr);
    div.style.display = 'block';
    if (!div.innerHTML.trim()) {
        //загрузить
        goAjaxRequest({
            url: '/Physic/ShowSimilar',
            type: 'GET',
            data: { id: document.getElementById('inputIdCurrentPhysEffect').value },
            func_success: function (data, status, jqXHR) {
                div.innerHTML = data;
            }
        });
    }
}




//метод для загрузки списков
function detailsLoadLists() {
    hidenLastOnLiMenu();
    detailsCurrentLiMenuId = 'detailsUl_lists';
    var li = document.getElementById('detailsUl_lists');
    li.style.borderBottom = '3px solid red';
    var cr = li.id.split('_')[1];
    var div = document.getElementById('detailsdiv_' + cr);
    div.style.display = 'block';
    if (!div.innerHTML.trim()) {
        //загрузить
        goAjaxRequest({
            url: '/Physic/LoadLists',
            type: 'GET',
            data: { idfe: document.getElementById('inputIdCurrentPhysEffect').value },
            func_success: function (data, status, jqXHR) {
                document.getElementById('DivListsFe_id').innerHTML = data;
            }
        });
    }
}




//метод для изменения адреса страницы
function GoPhysicId() {
    var newid = document.getElementById('inputForGoPhysic').value;
    if (newid) {
        document.location.href = "/Physic/Details/" + newid;
    }
}




;;;