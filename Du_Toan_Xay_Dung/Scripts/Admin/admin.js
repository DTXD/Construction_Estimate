
function timkiem() {
    //var select1 = angular.element(document.querySelector('#select-tphp1'));
    var select1 = document.getElementById("select-tphp1");
    //var select2 = angular.element(document.querySelector('#select-tphp2'));
    var select2 = document.getElementById("select-tphp2");
   // var txt = angular.element(document.querySelector('#txttimkiem')).value.toLowerCase();
    var txt = document.getElementById("txttimkiem").value.toLowerCase();
    for (var option in select2) {
        select2.remove(option);
    }

    if (txt.length == 0) {
        return;
    }

    for (var i = 0; i < select1.length; i++) {
        var row = select1.options[i].text.toLowerCase();
        var row3 = select1.options[i].value;
        if (row.search(txt) != -1) {
            var opt = document.createElement('option');
            opt.text = row;
            opt.value = row3;
            select2.add(opt);
        }
    }
}
function add() {
    var selectvatlieu = document.getElementById("select-tphp2").value;
    var haophi = document.getElementById("haophi").value;
    var arr = selectvatlieu.split(";");
    var rows = "";
    rows += "<td>"
    + arr[0] + "<input type='hidden' name='makhuvuc' value='" + arr[0] + "'>" + "</td><td>"
    + arr[1] + "<input type='hidden' name='mathanhphan' value='" + arr[1] + "'>" + "</td><td>"
    + haophi + "<input type='hidden' name='haophi1' value='" + haophi + "'>" + "</td><td>"
    +arr[2] + "<input type='hidden' name='ten' value='" + arr[2] + "'>" + "</td><td>"
    + arr[3] + "<input type='hidden' name='donvithanhphan' value='" + arr[3] + "'>" + "</td><td>"
    + arr[4] + "<input type='hidden' name='gia' value='" + arr[4] + "'>" + "</td>";
    var tbody = document.querySelector("#datatable tbody");
    var tr = document.createElement("tr");
    tr.innerHTML = rows;
    tbody.appendChild(tr);
}
