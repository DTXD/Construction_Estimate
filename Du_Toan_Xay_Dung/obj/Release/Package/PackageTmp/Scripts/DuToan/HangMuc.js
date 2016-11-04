'use strict';

$(document).ready(function () {
    $("#btn_edit_ten_hangmuc").click(function () {
        var s = prompt("Nhập tên Hạng Mục mới", "");
        $('#txt_ten_hangmuc').val(s);
        $('#h3_hangmuc').text(s);

    });

    var _tr = $('.tr_congviec').html();
    $('#btn_themcvct').click(function (e) {
        var _trNew = $('<tr class="tr_congviec"></tr>').append(_tr);
        $('.tr_congviec').parent().append(_trNew);
        $('.selectpicker').selectpicker('refresh');
    });


    _getDonviFromSelect();
});


$('#div_primary').on('change', 'select', function () {
    var _idDinhMuc = $(this).val();
    var _donvi = $(this).find('option:selected').attr('data-donvi');
    var _tr = $(this).parent().parent().parent();
    var _txtDonvi = _tr.find('input').eq(0 + 1);
    var _txtKhoiLuong = _tr.find('input').eq(1 + 1);
    var _txtGiaVatLieu = _tr.find('input').eq(2 + 1);
    var _txtGiaNhanCong = _tr.find('input').eq(3 + 1);
    var _txtGiaMayThiCong = _tr.find('input').eq(4 + 1);
    var _taga = _tr.find(".tag_a").find('a');
    var _lblThanhTien = _tr.find('.sum_thanhtien');
    var _txtThanhTien = _tr.find('input').eq(5 + 1);

    _txtDonvi.val(_donvi);
    $.ajax({
        type: "POST",
        url: '/HangMuc/getAllPrice',
        data: { idDinhMuc: _idDinhMuc },
        cache: false,
        dataType: "json",
        success: function (result) {
            _txtGiaVatLieu.val(result.totalGiaVL);
            _txtGiaNhanCong.val(result.totalGiaNC);
            _txtGiaMayThiCong.val(result.totalGiaMay);
            _taga.attr('href', '/HangMuc/ChiTiet_VL_NC_MTC/?ID=' + result.idDinhMuc);
            _lblThanhTien.html((parseFloat(result.totalGiaVL) + parseFloat(result.totalGiaNC) + parseFloat(result.totalGiaMay)).toFixed(3));
            _txtThanhTien.val((parseFloat(result.totalGiaVL) + parseFloat(result.totalGiaNC) + parseFloat(result.totalGiaMay)).toFixed(3));
            Total();
        },
        error: function (err) {
            console.log(err.status + " - " + err.statusText);
        }
    });
});


$(document).ready(function () {
    $('#div_primary').on('click', '.btn_xoa_cv', function () {
        var con_cv = confirm("Bạn có thật sự muốn xóa công việc này...!!!");
        if (con_cv == true) {
            $(this).closest('tr').remove();
            var sum_thanhtien_cv = $(this).parent().parent().find("input[name='txtthanhtien[]']").val();
            var price_old = $('#div_primary').find('#txt_tongtien').val();
            var price_new = parseFloat(price_old) - parseFloat(sum_thanhtien_cv);
            price_new = price_new.toFixed(3);
            $('#txt_tongtien').val(price_new);
            $('#tongtien').html(price_new);
        }
    });
});

function _getDonviFromSelect() {
    var _donvi = $('#cmbtencv option:selected').attr('data-donvi');
    $('#txtdonvi').val(_donvi);
}



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

function Total() {
    var total = 0;
    $('#div_primary .sum_thanhtien').each(function () {
        total += parseFloat($(this).html());
    });
    total = total.toFixed(3);
    $('#txt_tongtien').val(total);
    $('#tongtien').html(total);
}

$('#btnSubmit').click(function () {
    $('#formAdd').submit();
});

