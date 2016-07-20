"use strict"

//thanh phan hao phi
var list_haophi = [];
var mahangmuc = $("#txt_ma_hangmuc").val();

function getcongviecs_hm() {
    var congviecs;
    //alert("haha");
    $.ajax({
        type: "POST",
        url: '/HangMuc/GetDSCongViec',
        data: { mahangmuc: mahangmuc },
        cache: false,
        dataType: "json",
        async: false,
        success: function (result) {
            congviecs = result;
        }
    });
    return congviecs;
}

var list_congviec = getcongviecs_hm();
list_haophi.push(list_congviec);

//console.log(list_haophi);

$(document).ready(function () {
    $('#cmbtencv').selectpicker({
        size: 10,
        liveSearch: true,
        width: "600px",
    });
    /*
    $('#cbb_haophis').selectpicker({
        liveSearch: true,
        width: "538px"
    });
    */
    $('#div_primary').on('click', '.btn_xoa_cv', function () {
        var con_cv = confirm("Bạn có thật sự muốn xóa công việc này...!!!");
        if (con_cv == true) {
            var sum_thanhtien_cv = $(this).parent().parent().find("input[name='txtthanhtien[]']").val();
            var old_price = $('#container').find('#txt_tongtien').val();
            var new_price = parseFloat(old_price) - parseFloat(sum_thanhtien_cv);
            new_price = new_price.toFixed(3);
            $('#txt_tongtien').val(new_price);
            $('#span_tongtien').html(new_price);
            $(this).closest('tr').remove();
        }
    });

    $('#div_primary').on('click', '.btn_cthaophi', function () {

        var iddinhmuc = $(this).parent().parent().find("input[name='txtmahieucv_dm[]']").val();

        if (typeof iddinhmuc != "undefined") {
            var temp = [];
            for (var j = 0; j < list_haophi[0].length; j++) {
                if (list_haophi[0][j]["MaHieuCV_DM"].indexOf(iddinhmuc) != -1) {
                    temp.push(j);
                }
            }

            /*
            //remove thanh phan hao phi
            $('#table_cthaophi_vatlieu').find('.tr_cthaophi').remove();
            $('#table_cthaophi_nhancong').find('.tr_cthaophi').remove();
            $('#table_cthaophi_maythicong').find('.tr_cthaophi').remove();
            
            //remove tong hao phi
            $('#table_cthaophi_vatlieu').find('.tr_haophi_tongvl').remove();
            $('#table_cthaophi_nhancong').find('.tr_haophi_tongnc').remove();
            $('#table_cthaophi_maythicong').find('.tr_haophi_tongmtc').remove();
            */
            var tongvl = 0;
            var tongnc = 0;
            var tongmtc = 0;

            for (var i = 0; i < temp.length; i++) {

                var s = String(list_haophi[0][temp[i]]['MaHP']).substr(0, 1).toUpperCase();

                var _trnew_cthaophi = '<tr class="tr_cthaophi">' +
                    '<td>' +
                    '<input style="width:70px" type="hidden" value="' + list_haophi[0][temp[i]]['MaHP'] + '" name="txtma_hp[]" />' +
                    '<input style="width:70px" type="hidden" value="' + list_haophi[0][[temp[i]]]['MaHieuCV_DM'] + '" name="txtmahieucvdm_hp[]" />' +
                    '</td>' +
                    '<td>' + list_haophi[0][[temp[i]]]['Ten'] +
                    '<input style="width:70px" type="hidden" value="' + list_haophi[0][[temp[i]]]['Ten'] + '" name="txtten_hp[]" />' +
                    '</td>' +
                    '<td><input style="width:70px" type="text" value="' + list_haophi[0][[temp[i]]]['DonVi'] + '" name="txtdonvi_hp[]" /></td>' +
                    '<td><input style="width:70px" type="text" value="' + list_haophi[0][[temp[i]]]['SoLuong_DM'] + '" name="txtsoluongdm_hp[]" /></td>' +
                    '<td><input style="width:70px" type="text" value="' + list_haophi[0][[temp[i]]]['Gia'] + '" name="txtgia_hp[]" /></td>' +
                    '<td><span style="width:70px" class="spanthanhtien_hp" >' + parseFloat(list_haophi[0][[temp[i]]]['SoLuong_DM']) * parseFloat(list_haophi[0][[temp[i]]]['Gia']) + '</span></td>' +
                    '</tr>';

                if (s == "V") {
                    tongvl = tongvl + (parseFloat(list_haophi[0][[temp[i]]]['SoLuong_DM']) * parseFloat(list_haophi[0][[temp[i]]]['Gia']));
                    $('#table_cthaophi_vatlieu').find('tbody').append(_trnew_cthaophi);
                }
                if (s == "N") {
                    tongnc = tongnc + (parseFloat(list_haophi[0][[temp[i]]]['SoLuong_DM']) * parseFloat(list_haophi[0][[temp[i]]]['Gia']));
                    $('#table_cthaophi_nhancong').find('tbody').append(_trnew_cthaophi);
                }
                if (s == "M") {
                    tongmtc = tongmtc + (parseFloat(list_haophi[0][[temp[i]]]['SoLuong_DM']) * parseFloat(list_haophi[0][[temp[i]]]['Gia']));
                    $('#table_cthaophi_maythicong').find('tbody').append(_trnew_cthaophi);
                }
            }
            var tr_tongvl = '<tr class="tr_haophi_tongvl" style="font-size: large; background-color: lavender">' +
                        '<td></td>' +
                        '<td>Tổng :</td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td><span class="spantongthanhtien_hp">' + tongvl + '</span></td>' +
                        '</tr>';
            var tr_tongnc = '<tr class="tr_haophi_tongnc" style="font-size: large; background-color: lavender">' +
                        '<td></td>' +
                        '<td>Tổng :</td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td><span class="spantongthanhtien_hp">' + tongnc + '</span></td>' +
                        '</tr>';
            var tr_tongmtc = '<tr class="tr_haophi_tongmtc" style="font-size: large; background-color: lavender">' +
                        '<td></td>' +
                        '<td>Tổng :</td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td></td>' +
                        '<td><span class="spantongthanhtien_hp">' + tongmtc + '</span></td>' +
                        '</tr>';
            $('#table_cthaophi_vatlieu').find('tbody').append(tr_tongvl);
            $('#table_cthaophi_nhancong').find('tbody').append(tr_tongnc);
            $('#table_cthaophi_maythicong').find('tbody').append(tr_tongmtc);
        }
    });


    //thay doi so luong tinh thanh tien
    $('#Modal_ChiTiet_HaoPhi').on('change', "input[name='txtsoluongdm_hp[]']", function () {
        //var tonghaophi = 0;
        var tongthanhphan_new = 0;
        var gia = $(this).parent().parent().find("input[name='txtgia_hp[]']").val();
        var soluong = $(this).val();
        var thanhtien = $(this).parent().parent().find(".spanthanhtien_hp");
        if (gia != '' && soluong != '') {

            thanhtien.html(parseFloat(gia) * parseFloat(soluong));
            $(this).parent().parent().parent().find(".tr_cthaophi").each(function () {
                var temp = $(this).find(".spanthanhtien_hp").html();
                tongthanhphan_new = tongthanhphan_new + parseFloat(temp);
            });
            $(this).parent().parent().parent().find(".spantongthanhtien_hp").html(tongthanhphan_new);

            /*
            $("#Modal_ChiTiet_HaoPhi").find(".spantongthanhtien_hp").each(function () {
                tonghaophi = tonghaophi + parseFloat($(this).html());
            });
            $("#span_tong_haophis").html(tonghaophi);
            */
        }
    });

    //thay doi gia tinh thanh tien
    $('#Modal_ChiTiet_HaoPhi').on('change', "input[name='txtgia_hp[]']", function () {
        var tong = 0;
        var gia = $(this).parent().parent().find("input[name='txtsoluongdm_hp[]']").val();
        var soluong = $(this).val();
        var thanhtien = $(this).parent().parent().find(".spanthanhtien_hp");

        if (gia != '' && soluong != '') {
            thanhtien.html(parseFloat(gia) * parseFloat(soluong));
            $(this).parent().parent().parent().find(".tr_cthaophi").each(function () {
                var temp = $(this).find(".spanthanhtien_hp").html();
                tong = tong + parseFloat(temp);
            });
            $(this).parent().parent().parent().find(".spantongthanhtien_hp").html(tong);
        }
    });

    $('#btn_save_cthaophi').click(function () {

        //get ma hieu CV_DM
        var mahieucvdm_hp = $('#table_cthaophi_nhancong .tr_cthaophi').find("input[name='txtmahieucvdm_hp[]']").val();

        if (typeof mahieucvdm_hp != "undefined") {
            for (var j = 0; j < list_haophi[0].length; j++) {
                if (list_haophi[0][j]["MaHieuCV_DM"].indexOf(mahieucvdm_hp) != -1) {
                    list_haophi[0].splice(j, 1);
                    j--;
                }
            }
            $('#table_cthaophi_vatlieu .tr_cthaophi').each(function () {
                var obj = {
                    MaHP: $(this).find("input[name='txtma_hp[]']").val(),
                    Ten: $(this).find("input[name='txtten_hp[]']").val(),
                    MaHieuCV_DM: $(this).find("input[name='txtmahieucvdm_hp[]']").val(),
                    DonVi: $(this).find("input[name='txtdonvi_hp[]']").val(),
                    SoLuong_DM: $(this).find("input[name='txtsoluongdm_hp[]']").val(),
                    Gia: $(this).find("input[name='txtgia_hp[]']").val()
                };

                list_haophi[0].push(obj);

            });
            $('#table_cthaophi_nhancong .tr_cthaophi').each(function () {
                var obj = {
                    MaHP: $(this).find("input[name='txtma_hp[]']").val(),
                    Ten: $(this).find("input[name='txtten_hp[]']").val(),
                    MaHieuCV_DM: $(this).find("input[name='txtmahieucvdm_hp[]']").val(),
                    DonVi: $(this).find("input[name='txtdonvi_hp[]']").val(),
                    SoLuong_DM: $(this).find("input[name='txtsoluongdm_hp[]']").val(),
                    Gia: $(this).find("input[name='txtgia_hp[]']").val()
                };

                list_haophi[0].push(obj);

            });
            $('#table_cthaophi_maythicong .tr_cthaophi').each(function () {
                var obj = {
                    MaHP: $(this).find("input[name='txtma_hp[]']").val(),
                    Ten: $(this).find("input[name='txtten_hp[]']").val(),
                    MaHieuCV_DM: $(this).find("input[name='txtmahieucvdm_hp[]']").val(),
                    DonVi: $(this).find("input[name='txtdonvi_hp[]']").val(),
                    SoLuong_DM: $(this).find("input[name='txtsoluongdm_hp[]']").val(),
                    Gia: $(this).find("input[name='txtgia_hp[]']").val()
                };

                list_haophi[0].push(obj);

            });
            //console.log(list_haophi);


            var tongvl = $('#table_cthaophi_vatlieu').find('.spantongthanhtien_hp').html();
            var tongnc = $('#table_cthaophi_nhancong').find('.spantongthanhtien_hp').html();
            var tongmtc = $('#table_cthaophi_maythicong').find('.spantongthanhtien_hp').html();

            $("#mytable").find("input[name='txtmahieucv_dm[]']").each(function () {
                if(mahieucvdm_hp === $(this).val())
                {
                    var tr = $(this).parent().parent();
                    tr.find("input[name='txtgiavl[]']").val(tongvl);
                    tr.find("input[name='txtgianc[]']").val(tongnc);
                    tr.find("input[name='txtgiamtc[]']").val(tongmtc);

                    $(this).parent().parent().find('.sum_thanhtien').html(parseFloat(tongvl) + parseFloat(tongnc) + parseFloat(tongmtc));
                    
                    Total();

                    return false;
                }
            });
        }
    });

    $("#btn_edit_ten_hangmuc").click(function () {
        bootbox.prompt("Nhập tên hạng mục", function (result) {
            if (result === null) {
                $('#txt_ten_hangmuc').val("Hạng Mục Mới");
                $('#span_tenhm').text("Hạng Mục Mới");
            } else {
                $('#txt_ten_hangmuc').val(result);
                $('#span_tenhm').text(result);
            }
        });
        /*
        var s = prompt("Nhập tên Hạng Mục mới", "");
        if (s != null)
        {
            alert("haha");
            $('#txt_ten_hangmuc').val(s);
            $('#span_tenhm').text(s);
        }
        */
    });
});


$('#div_primary').on('change', 'select', function () {

    var _idDinhMuc = $(this).val();
    var _donvi = $(this).find('option:selected').attr('data-donvi');
    var _tencv = $(this).find(':selected').text()
    var _txtGiaVatLieu = 0;
    var _txtGiaNhanCong = 0;
    var _txtGiaMayThiCong = 0;
    var _taga = '';
    var _lblThanhTien = '0';
    var _txtThanhTien = 0;



    $.ajax({
        type: "POST",
        url: '/HangMuc/getAllPrice',
        data: { idDinhMuc: _idDinhMuc },
        cache: false,
        dataType: "json",
        success: function (result) {
            _txtGiaVatLieu = result.totalGiaVL;
            _txtGiaNhanCong = result.totalGiaNC;
            _txtGiaMayThiCong = result.totalGiaMay;
            _lblThanhTien = (parseFloat(result.totalGiaVL) + parseFloat(result.totalGiaNC) + parseFloat(result.totalGiaMay)).toFixed(3);
            _txtThanhTien = (parseFloat(result.totalGiaVL) + parseFloat(result.totalGiaNC) + parseFloat(result.totalGiaMay)).toFixed(3);

            //get list hao phi cong viec map tung item day vao list hao phi da luu tru
            var tempgethp = result.list_haophi.map(function (item) {
                list_haophi[0].push(item);
            });
            

            setTimeout(function () {
                var _trNew = '<tr class="tr_primary">' +
        '<td></td>' +
        '<td class="td_tencv">' + _tencv +
        '<input type="hidden" value="' + _idDinhMuc + '" name="txtmahieucv_dm[]" />' +
        '<input type="hidden" value="' + _tencv + '" name="txttencv[]" />' +
        '</td>' +
        '<td>' +
        '<input style="width:50px" type="text" value="' + _donvi + '" name="txtdonvi[]" />' +
        '</td>' +
        '<td>' +
        '<input style="width:50px" type="text" value="1" class="txt_khoiluong" name="txtkhoiluong[]" />' +
        '</td>' +
        '<td>' +
        '<input style="width:80px" type="text" value="' + _txtGiaVatLieu + '" name="txtgiavl[]" />' +
        '</td>' +
        '<td>' +
        '<input style="width: 80px" type="text" value="' + _txtGiaNhanCong + '" name="txtgianc[]" />' +
        '</td>' +
        '<td>' +
        '<input style="width: 80px" type="text" value="' + _txtGiaMayThiCong + '" name="txtgiamtc[]" />' +
        '</td>' +
        '<td class="tag_a">' +
        '<span class="btn btn-primary btn-flat btn-xs btn_cthaophi" data-toggle="modal" data-target="#Modal_ChiTiet_HaoPhi">' +
        '<i class="fa fa-edit"></i> Detail' +
        '</span>' +
        '</td>' +
        '<td>' +
        '<span class="sum_thanhtien">' + _lblThanhTien + '</span>' +
        '<input type="hidden" value="' + _txtThanhTien + '" name="txtthanhtien[]" />' +
        '</td>' +
        '<td>' +
        '<span class="btn btn-danger btn-flat btn-xs btn_xoa_cv">' +
        '<i class="fa fa-edit"></i>Xóa' +
        '</span>' +
        '</td>' +
        '</tr>';
                $('#mytable').find('tbody').append(_trNew);
                Total();
            }, 500);

        },
        error: function (err) {
            console.log(err.status + " - " + err.statusText);
        }
    });
});

$('#div_primary').on('change', "input[name='txtkhoiluong[]']", function () {
    var giavl = $(this).parent().parent().find("input[name='txtgiavl[]']").val();
    var gianc = $(this).parent().parent().find("input[name='txtgianc[]']").val();
    var giamtc = $(this).parent().parent().find("input[name='txtgiamtc[]']").val();


    var tong = parseFloat(giavl) + parseFloat(gianc) + parseFloat(giamtc);
    var price = $(this).val() * tong;

    price = price.toFixed(3);

    $(this).parent().parent().find('.sum_thanhtien').html(price);
    $(this).parent().parent().find("input[name='txtthanhtien[]']").val(price);
    Total();
});

/*
$('#div_primary').on('change', "input[name='txtgiavl[]']", function () {


    var giavl_new = $(this).parent().parent().find("input[name='txtgiavl[]']").val();
    var gianc = $(this).parent().parent().find("input[name='txtgianc[]']").val();
    var giamtc = $(this).parent().parent().find("input[name='txtgiamtc[]']").val();

    var khoiluong = $(this).parent().parent().find("input[name='txtkhoiluong[]']").val();

    var tong = parseFloat(giavl_new) + parseFloat(gianc) + parseFloat(giamtc);
    var price = tong * khoiluong;

    price = price.toFixed(3);

    $(this).parent().parent().find('.sum_thanhtien').html(price);
    $(this).parent().parent().find("input[name='txtthanhtien[]']").val(price);
    Total();
});

$('#div_primary').on('change', "input[name='txtgianc[]']", function () {


    var giavl = $(this).parent().parent().find("input[name='txtgiavl[]']").val();
    var gianc_new = $(this).parent().parent().find("input[name='txtgianc[]']").val();
    var giamtc = $(this).parent().parent().find("input[name='txtgiamtc[]']").val();

    var khoiluong = $(this).parent().parent().find("input[name='txtkhoiluong[]']").val();

    var tong = parseFloat(giavl) + parseFloat(gianc_new) + parseFloat(giamtc);
    var price = tong * khoiluong;

    price = price.toFixed(3);

    $(this).parent().parent().find('.sum_thanhtien').html(price);
    $(this).parent().parent().find("input[name='txtthanhtien[]']").val(price);
    Total();
});

$('#div_primary').on('change', "input[name='txtgiamtc[]']", function () {

    var giavl = $(this).parent().parent().find("input[name='txtgiavl[]']").val();
    var gianc = $(this).parent().parent().find("input[name='txtgianc[]']").val();
    var giamtc_new = $(this).parent().parent().find("input[name='txtgiamtc[]']").val();

    var khoiluong = $(this).parent().parent().find("input[name='txtkhoiluong[]']").val();

    var tong = parseFloat(giavl) + parseFloat(gianc) + parseFloat(giamtc_new);
    var price = tong * khoiluong;

    price = price.toFixed(3);

    $(this).parent().parent().find('.sum_thanhtien').html(price);
    $(this).parent().parent().find("input[name='txtthanhtien[]']").val(price);
    Total();
});
*/
function Total() {
    var total = 0;
    $('#div_primary .sum_thanhtien').each(function () {
        total += parseFloat($(this).html());
    });
    total = total.toFixed(3);
    $('#txt_tongtien').val(total);
    $('#span_tongtien').html(total);
}

/*
$('#btnSubmit').click(function () {
    $('#formAdd').submit();
});
*/

function getcbbhaophis() {
    var data;
    //alert("haha");
    $.ajax({
        type: "POST",
        url: '/HangMuc/GetDSDonGia',
        data: {},
        cache: false,
        dataType: "json",
        async: false,
        success: function (result) {
            data = result;
        }
    });
    return data;
}


$(document).on('click', '#myDropdown .dropdown-menu', function (e) {
    e.stopPropagation();
});
var listNew = getcbbhaophis().map(function (item) {
    item.TenUnicode = remove_unicode(item.Ten);
    return item;
});
var ulShow = $('#cbb-haophis_show');
for (var i = 0; i < 100; i++) {
    var li = $('<li class="col-lg-12"></li>');
    li.attr('data-value', listNew[i].MaVL_NC_MTC);
    li.attr('data-donvi', listNew[i].DonVi);
    li.attr('data-gia', listNew[i].Gia);
    li.text(listNew[i].Ten);
    ulShow.append(li);
}

$('#search-cbb-haophis').on('input', function (e) {
    var search = $(this).val();
    var resultSearch = SearchText(listNew, 'TenUnicode', search);

    var ulShowS = $('#cbb-haophis_show');
    ulShowS.html('');
    for (var j = 0; j < resultSearch.length; j++) {
        var liS = $('<li class="col-lg-12"></li>');
        liS.attr('data-value', resultSearch[j].MaVL_NC_MTC);
        liS.attr('data-donvi', resultSearch[j].DonVi);
        liS.attr('data-gia', resultSearch[j].Gia);
        liS.text(resultSearch[j].Ten);
        ulShowS.append(liS);
        liS.bind("click");
    }
});

function remove_unicode(str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    return str;
}

function SearchText(list, field, keyword) {
    var result = [];
    for (var s = 0; s < list.length; s++) {
        if (list[s][field].toLowerCase().indexOf(keyword) != -1) {
            result.push(list[s]);
        }
    }
    return result;
}

function SelectValue(input) {
    var ten = $(input).attr('data-ten');
    var donvi = $(input).attr('data-donvi');
    var gia = $(input).attr('data-gia');
    var mahp_tamtinh = $(input).val();

    /*
    if (mahp_tamtinh.substring(0, 1) == "V")
        giavl = parseFloat(giavl) + parseFloat(gia);
    if (mahp_tamtinh.substring(0, 1) == "N")
        gianc = parseFloat(gianc) + parseFloat(gia);
    if (mahp_tamtinh.substring(0, 1) == "M")
        giamtc = parseFloat(giamtc) + parseFloat(gia);
    */
    var _trnew_tamtinh = '<tr class="HaoPhi_TamTinh">' +
        '<td>' + mahp_tamtinh + '</td>' +
        '<td>' + ten + '</td>' +
        '<td><input style="width:70px" type="text" value="1" name="txtsoluong[]" /></td>' +
        '<td>' + donvi + '</td>' +
        '<td>' + gia + '</td>' +
        '<td><span class="btn btn-danger btn-flat btn-xs btnxoa_haophitt"><i class="fa fa-edit"></i>Xóa</span></td>' +
        '</tr>';
    $('#table_tamtinh').find('tbody').append(_trnew_tamtinh);
}

$('body').on('click', '#cbb-haophis_show li', function () {
    var liSelect = $(this);
    $('#cbb_haophis_1').html('<span class="pull-left">-- ' + liSelect.html() + '</span><span class="caret pull-right"></span>');
    var inputHidden = $('#select-cbb-haophis');
    inputHidden.val(liSelect.attr('data-value'));
    inputHidden.attr('data-donvi', liSelect.attr('data-donvi'));
    inputHidden.attr('data-gia', liSelect.attr('data-gia'));
    inputHidden.attr('data-ten', liSelect.html());
    $('#myDropdown').removeClass('open');
    SelectValue(inputHidden);
});

//Truyen ID cong viec qua trang dongiachitiet
$('#btn_dongiact').click(function () {

    var id = "";
    var result = "";
    $("#mytable input[name='txtmahieucv_dm[]']").each(function () {
        id = id + $(this).val() + ",";
    });

    var Arr_id = id.split(",");

    for (var i = 0; i < Arr_id.length; i++) {
        var d = 0;
        for (var j = i + 1; j < Arr_id.length; j++) {
            if (Arr_id[i] == Arr_id[j]) {
                d++;
                break;
            }
        }
        if (Arr_id[i] == '' || Arr_id[i] == '0') {
            d++;
        }
        if (d == 0) {
            result = result + Arr_id[i] + ',';
        }
    }

    result = result.substr(0, result.length - 1);
    location.href = "/HangMuc/DonGiaChiTiet/?ID=" + result;
});

