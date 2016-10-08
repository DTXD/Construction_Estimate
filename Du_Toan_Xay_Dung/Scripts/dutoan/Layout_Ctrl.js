'use strict';



//need window.onload to get document.getelementbyid
window.onload = function () {
    geturl();
};


function geturl() {
    var path = window.location.pathname.split('/');

    var parent_url = path[1];

    $('#main_menu ul li a').each(function () {
        //kiem tra xem cai the <a> co href khong
        //neu khong kiem tra no se bao loi la attr khong ton tai
        if (typeof ($(this).attr('href')) !== 'undefined') {
            //lay link cua the <a>
            var href = $(this).attr('href').toLowerCase();
            
            var path_now = href.split('/');

            //so sanh url hien tai voi cai link
            if (path_now[1] === parent_url.toLowerCase()) {
                //neu co no se addClass va thoat khoi vong lap
                $(this).parent().addClass('current_page_item');
                return;
            }
        }
    });

};

function open_alert(state) {

    var div = document.createElement("div");
    var elements = '';
    if (state == "success") {
        elements = '<div id="alert_success" style="color: #3c763d; background-color: #dff0d8; border-color: #d6e9c6; position: fixed; right: 46%; top:10%; opacity: 1; padding: 10px; border-radius: 4px; "><strong>Thành công !</strong>';
    }
    if (state == "fail") {
        elements = '<div id="alert_success" style="color: #a94442; background-color: #f2dede; border-color: #ebccd1; position: fixed; right: 46%; top:10%; opacity: 1; padding: 10px; border-radius: 4px; "><strong>Thất bại !</strong>';
    }

    div.innerHTML = elements;
    document.body.appendChild(div);

    window.setTimeout(function () { document.body.removeChild(div); }, 2000);

};
