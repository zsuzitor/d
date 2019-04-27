;;;



function loadFavouritePhysic(id) {

    document.getElementById('FavouritePhysic_div_but_id').innerHTML = '';
    goAjaxRequest({
        url: '/Person/ListFavouritePhysics',
        type: 'GET',
        data: { id: id },
        func_success: function (data, status, jqXHR) {
            // Заменяем часть представления, отвечающего за отображение значений характеристики
            document.getElementById('FavouritePhysic_div_id').innerHTML = data;
        }
    });

}

function loadRolesUser(id) {
    var div = document.getElementById('divRolesUser_id');
    var but = document.getElementById('butRolesUser_id');

    if (!div.innerHTML.trim()) {
        //загрузить
        goAjaxRequest({
            url: '/Person/GetRoles',
            type: 'GET',
            data: { personId: id },
            func_success: function (data, status, jqXHR) {
                // Заменяем часть представления, отвечающего за отображение значений характеристики
                div.innerHTML = data;
            }
        });
        div.style.display = 'block';
        but.innerHTML = 'Скрыть роли';
    }
    else {
        //загружать не надо, надо показать\скрыть
        if (div.style.display == 'block') {
            //скрыть
            div.style.display = 'none';
            but.innerHTML = 'Показать роли';
        }
        else {
            //показать
            div.style.display = 'block';
            but.innerHTML = 'Скрыть роли';
        }
    }

}


;;;