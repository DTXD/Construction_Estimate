"use strict"


var list_work = [];

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
    // var value = gia tri nhap vao 
    // dung ajax call sever lay du lieu
    // duyet cai mang roi dua vao all table 
    //<table id=kjadhksaj>
    //<tr><td><input type=''checkbox data-id='' class='adas'> <td><sapn>
    //
    //append div pop
    //div.show()
    //

    $("#popupsearch").css({ "display": "", "position": "fixed", "top": "35%", "left": "420px" });
    $("#popupsearch").addClass("popupsearch");

    

    var data_search = $(this).val();
    $("#txt_search").val(data_search);
    data_search = $("#txt_search").val();

    $("#txt_search").focus();
    $("#txt_search").keyup(function () {

        data_search = $(this).val();
        var result_search = SearchText(list_work, 'MaHieuCV_DM', data_search);
        //show data search in table
        var table_search = $("#table_search_normwork");
        table_search.html('');
        var checkbox = '<td><input type="checkbox" name="txt_checkbox"></td>';
        for (var i = 0; i < result_search.length; i++) {
            var tr_id = '<td>' + result_search[i].MaHieuCV_DM + '</td>';
            var tr_name = '<td>' + result_search[i].CongTac + ' ' + list_work[i].RangBuoc + '</td>';
            var tr_donvi = '<td>' + result_search[i].DonVi + '</td>';

            var tr = '<tr>' + checkbox + tr_id + tr_name + tr_donvi + '</tr>';

            table_search.append(tr);

        }
    });
    
    var result_search = SearchText(list_work, 'MaHieuCV_DM', data_search);
    //show data search in table
    var table_search = $("#table_search_normwork");
    table_search.html('');
    var checkbox = '<td><input type="checkbox" name="txt_checkbox"></td>';
    for (var i = 0; i < result_search.length; i++) {
        var tr_id = '<td>' + result_search[i].MaHieuCV_DM + '</td>';
        var tr_name = '<td>' + result_search[i].CongTac + ' ' + list_work[i].RangBuoc + '</td>';
        var tr_donvi = '<td>' + result_search[i].DonVi + '</td>';

        var tr = '<tr>' + checkbox + tr_id + tr_name + tr_donvi + '</tr>';

        table_search.append(tr);

    }
    
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