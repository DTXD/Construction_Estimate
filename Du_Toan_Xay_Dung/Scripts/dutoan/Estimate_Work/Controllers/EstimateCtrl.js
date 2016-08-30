'use strict';

angular.module('app_work').controller('EstimateCtrl', ['$scope', 'dataService', function ($scope, dataService) {

    //
    $scope.list_Normwork = [];
    getNormWorks();

    /*
    $scope.list_AllPrice = [];
    getListPrice();
    */

    $scope.ListDetailNormWork_Price = [];
    GetDetailNormWork_Price();


    //data searched
    $scope.list_searched = [];
    var index_work = 1;                     //id work in sheet


    function getNormWorks() {
        dataService.getNormworks().then(function (data) {
            var eachItem;
            angular.forEach(data, function (value, key) {
                eachItem = {
                    ID: value.MaHieuCV_DM,
                    Name: value.CongTac + " " + value.RangBuoc,
                    Unit: value.DonVi,
                    pricematerial: 0,
                    pricelabor: 0,
                    pricemachine: 0
                };
                $scope.list_Normwork.push(eachItem);
            });
        });
    };

    function getListPrice() {
        dataService.getListPrice().then(function (data) {
            $scope.list_AllPrice = data;
        });
    };

    function GetDetailNormWork_Price() {
        dataService.GetDetailNormWork_Price().then(function (data) {
            $scope.ListDetailNormWork_Price = data;
        });
    };

    //create list works
    $scope.works = [];

    //create sheet
    for (var i = 0; i < 10; i++) {
        $scope.works.push({ index: i });
    }


    //hide div popup
    $scope.popupsearch = false;

    //intialize variable get current element when double click
    $scope.current_doubleclick;
    //double click and show div popup for search
    $scope.search_work = function ($event) {
        $scope.popupsearch = !($scope.popupsearch);
        $scope.popupsearchcss = {
            "display": "",
            "position": "fixed",
            "top": "10%",
            "left": "420px"
        };
        $scope.popupsearchclass = 'popupsearch';

        //change content button when list == null
        var btn_save = angular.element(document.querySelector("#btn_search_normwork"));
        if ($scope.list_searched.length == 0) {
            btn_save.text("Thoát");
        }
        else {
            btn_save.text("Chọn");
        }
        $scope.current_doubleclick = angular.element($event.currentTarget);
    };

    //Amazing code: checked and copy list data to listsearch and calculate price
    $scope.checkbox_search = function (x) {
        var btn_save = angular.element(document.querySelector("#btn_search_normwork"));
        var d = $scope.list_searched.indexOf(x);
        if (d > -1) {
            $scope.list_searched.splice(d, 1);
        }
        else {
            //push price data from database
            var pricematerial = 0;
            var pricelabol = 0;
            var pricemachine = 0;
            angular.forEach($scope.ListDetailNormWork_Price, function (value, key) {
                if (x.ID == value.Key_NormWork) {
                    if (value.Key_Material.substring(0, 1) == "N") {
                        pricematerial = parseFloat(pricematerial + (value.Number * value.Price));
                    }
                    if (value.Key_Material.substring(0, 1) == "M") {
                        pricelabol = parseFloat(pricelabol + (value.Number * value.Price));
                    }
                    if (value.Key_Material.substring(0, 1) == "V") {
                        pricemachine = parseFloat(pricemachine + (value.Number * value.Price));
                    }
                }
            });
            x.pricematerial = pricematerial.toFixed(3);
            x.pricelabor = pricelabol.toFixed(3);
            x.pricemachine = pricemachine.toFixed(3);
            $scope.list_searched.push(x);
        }

        //change content button when list != null
        if ($scope.list_searched.length != 0) {
            btn_save.text("Chọn");
        }
        else {
            btn_save.text("Thoát");
        }
    };


    $scope.save_search = function () {
        if ($scope.popupsearch == true && $scope.list_searched != 0) {
            var div = angular.element($scope.current_doubleclick.parent().parent());
            var id = div.find(".column_header").html();
            var index = parseInt(id);

            angular.forEach($scope.list_searched, function (value, key) {
                $scope.works[index].id = index_work;
                $scope.works[index].key = value.ID;
                $scope.works[index].name = value.Name;
                $scope.works[index].unit = value.Unit;
                $scope.works[index].pricematerial = value.pricematerial;
                $scope.works[index].pricelabor = value.pricelabor;
                $scope.works[index].pricemachine = value.pricemachine;

                index = index + 1;
                index_work = parseInt(index_work) + 1;
            });
        }
        $scope.popupsearchcss = {
            "display": "none"
        };
        $scope.popupsearchclass = '';
        $scope.popupsearch = !($scope.popupsearch);
    };

    function substring_array(s, begin, end) {
        var string = String(s);
        string = string.substring(begin, end);
        return string;
    };

    $scope.change = function ($event) {
        var div = angular.element($event.currentTarget).parent().parent();
        var id_work = div.find(".column_header").html();
        var id = div.find("input").eq(0).val();
        var number = div.find("input").eq(4).val();
        var horizontal = div.find("input").eq(5).val();
        var vertical = div.find("input").eq(6).val();
        var height = div.find("input").eq(7).val();
        var area = div.find("input").eq(8).val();

        //mean work
        var regular_expression1 = /^\d+$/;
        if (regular_expression1.test(id)) {

            if (number == "") {
                $scope.works[id_work].number = 1;
            }
            if (horizontal == "") {
                $scope.works[id_work].horizontal = 1;
            }
            if (vertical == "") {
                $scope.works[id_work].vertical = 1;
            }
            if (height == "") {
                $scope.works[id_work].height = 1;
            }

            $scope.works[id_work].area = ($scope.works[id_work].number * $scope.works[id_work].horizontal * $scope.works[id_work].vertical * $scope.works[id_work].height).toFixed(3);

            $scope.works[id_work].summaterial = (parseFloat($scope.works[id_work].pricematerial) * parseFloat($scope.works[id_work].area)).toFixed(3);
            $scope.works[id_work].sumlabor = (parseFloat($scope.works[id_work].pricelabor) * parseFloat($scope.works[id_work].area)).toFixed(3);
            $scope.works[id_work].summachine = (parseFloat($scope.works[id_work].pricemachine) * parseFloat($scope.works[id_work].area)).toFixed(3);
        }


        //description work
        var regular_expression2 = /^\d+\.\d+$/;
        if (regular_expression2.test(id)) {
            if (number == "") {
                $scope.works[id_work].number = 1;
            }
            if (horizontal == "") {
                $scope.works[id_work].horizontal = 1;
            }
            if (vertical == "") {
                $scope.works[id_work].vertical = 1;
            }
            if (height == "") {
                $scope.works[id_work].height = 1;
            }

            $scope.works[id_work].area = ($scope.works[id_work].number * $scope.works[id_work].horizontal * $scope.works[id_work].vertical * $scope.works[id_work].height).toFixed(3);

            var d = id.indexOf(".");
            var temp = id.substring(0, d);
            var id_meanwork = 0;
            
            for (var i = id_work; i >= 0; i--) {
                if ($scope.works[i].id == temp) {
                    id_meanwork = i;
                    break;
                }
            }

            var sum = 0;
            var index_while = id_meanwork + 1;
            while (substring_array($scope.works[index_while].id, 0, d) == temp && $scope.works[index_while].id != "") {

                sum = parseFloat(sum) + parseFloat($scope.works[index_while].area);
                index_while = parseInt(index_while) + 1;
            }

            $scope.works[id_meanwork].area = sum.toFixed(3);
            $scope.works[id_meanwork].summaterial = (parseFloat($scope.works[id_meanwork].pricematerial) * parseFloat($scope.works[id_meanwork].area)).toFixed(3);
            $scope.works[id_meanwork].sumlabor = (parseFloat($scope.works[id_meanwork].pricelabor) * parseFloat($scope.works[id_meanwork].area)).toFixed(3);
            $scope.works[id_meanwork].summachine = (parseFloat($scope.works[id_meanwork].pricemachine) * parseFloat($scope.works[id_meanwork].area)).toFixed(3);
        }

    };
}])