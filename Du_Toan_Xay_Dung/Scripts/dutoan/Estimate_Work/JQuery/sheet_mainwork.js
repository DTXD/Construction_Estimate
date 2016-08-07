"use strict"

var list_work = [];
var spot_keysearch_input;

function get_normwork() {
    var congviecs;
    $.ajax({
        type: "POST",
        url: '/HangMuc/GetNormWorks',
        data: {},
        cache: false,
        dataType: "json",
        async: false,
        success: function (result) {
            congviecs = result;
        }
    });
    return congviecs;
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

list_work = get_normwork();


$('#sheet_cellcenter').on('keyup', 'input[name="txt_namework"]', function () {
    spot_keysearch_input = $(this).parent().parent();
    // var value = gia tri nhap vao 
    // dung ajax call sever lay du lieu
    // duyet cai mang roi dua vao all table 
    //<table id=kjadhksaj>
    //<tr><td><input type=''checkbox data-id='' class='adas'> <td><sapn>
    //
    //append div pop
    //div.show()
    //

    $("#popupsearch").css({ "display": "", "position": "fixed", "top": "10%", "left": "420px" });                   //important
    $("#popupsearch").addClass("popupsearch");



    var data_search = $(this).val();
    $("#txt_search").val(data_search);
    data_search = $("#txt_search").val();

    $("#txt_search").focus();
    $("#txt_search").keyup(function () {


        data_search = $(this).val();
        var result_search = SearchText(list_work, 'MaHieuCV_DM', data_search.toLowerCase());
        //show data search in table
        var table_search = $("#table_search_normwork");
        table_search.html('');
        for (var i = 0; i < result_search.length; i++) {
            var checkbox = '<td><input type="checkbox" class="checkbox_search" data-id="' + result_search[i].MaHieuCV_DM + '" name="txt_checkbox"></td>';
            var tr_id = '<td>' + result_search[i].MaHieuCV_DM + '</td>';
            var tr_name = '<td>' + result_search[i].CongTac + ' ' + list_work[i].RangBuoc + '</td>';
            var tr_donvi = '<td>' + result_search[i].DonVi + '</td>';

            var tr = '<tr>' + checkbox + tr_id + tr_name + tr_donvi + '</tr>';

            table_search.append(tr);

        }

    });
    /*
    var result_search = SearchText(list_work, 'MaHieuCV_DM', data_search);
    //show data search in table
    var table_search = $("#table_search_normwork");
    table_search.html('');
    for (var i = 0; i < result_search.length; i++) {
        var checkbox = '<td><input type="checkbox" class="checkbox_search" data-id="result_search[i].MaHieuCV_DM" name="txt_checkbox"></td>';
        var tr_id = '<td>' + result_search[i].MaHieuCV_DM + '</td>';
        var tr_name = '<td>' + result_search[i].CongTac + ' ' + list_work[i].RangBuoc + '</td>';
        var tr_donvi = '<td>' + result_search[i].DonVi + '</td>';

        var tr = '<tr>' + checkbox + tr_id + tr_name + tr_donvi + '</tr>';

        table_search.append(tr);

    }
    */
    
    //Exit modal
    $("#wrapper").mouseup(function (e) {
        //$("#popupsearch").css({ "display": "", "position": "", "top": "", "left": "" });

        var container = $("#popupsearch");

        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0) // ... nor a descendant of the container
        {
            container.hide();
        }
    });
});


$('#btn_search_normwork').click(function () {
    var list_search = [];

    $("#table_search_normwork input[type='checkbox']").each(function () {
        if ($(this).is(':checked')) {


            var tr = $(this).parent().parent();
            var id = $(this).attr("data-id");
            var name = tr.find("td").eq(2).html();
            var donvi = tr.find("td").eq(3).html();

            var object = {
                MaHieuCV_DM: id,
                TenCV_DM: name,
                DonVi: donvi
            };


            list_search.push(object);
        }
    });

    /*
    for (var i = 0; i < list_search.length; i++) {
        spot_keysearch_input.find("input").eq(1).val(list_search[i].MaHieuCV_DM);
        spot_keysearch_input.find("input").eq(2).val(list_search[i].TenCV_DM);
        spot_keysearch_input.find("input").eq(3).val(list_search[i].DonVi);
        
    }
    
    $("#popupsearch").hide();
    $("#table_search_normwork").html('');
    */
});
