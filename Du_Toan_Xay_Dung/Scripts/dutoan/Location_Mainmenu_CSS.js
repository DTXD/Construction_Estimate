$(document).ready(function () {
    //lay url hien tai
    var _url = (window.location.pathname).toLowerCase();
    var temp_url = _url.substring(_url.length - 1, _url.length);
    if (temp_url == '/') {
        _url = _url.substring(0, _url.length - 1);
    }

    var d = _url.lastIndexOf('/');
    var s = _url.substring(0, d);
    if (s == '')
        s = '/'
    //duyet het menu
    $('#menu ul li a').each(function () {
        //kiem tra xem cai the <a> co href khong
        //neu khong kiem tra no se bao loi la attr khong ton tai
        if (typeof ($(this).attr('href')) !== 'undefined') {
            //lay link cua the <a>
            var href = $(this).attr('href').toLowerCase();
            var d1 = href.lastIndexOf('/');
            var s1 = href.substring(0, d);
            //so sanh url hien tai voi cai link
            if (s === s1) {
                //neu co no se addClass va thoat khoi vong lap
                $(this).parent().addClass('current_page_item');
                return;
            }
        }
    });
    /*
    $('#menu_congtrinh ul li a').each(function () {
        //kiem tra xem cai the <a> co href khong
        //neu khong kiem tra no se bao loi la attr khong ton tai
        if (typeof ($(this).attr('href')) !== 'undefined') {
            //lay link cua the <a>
            var href = $(this).attr('href').toLowerCase();
            var s2 = href.substring(href.length-1,href.length);
            if (s2=='/')
            {
                href = href.substring(0, href.length - 1);
            }
            //so sanh url hien tai voi cai link
            if (_url ===href) {
                //neu co no se addClass va thoat khoi vong lap
                $(this).parent().addClass('active');
                return;
            }
        }
    });
    */
});