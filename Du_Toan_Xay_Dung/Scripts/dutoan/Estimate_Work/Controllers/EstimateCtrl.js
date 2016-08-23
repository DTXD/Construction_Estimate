'use strict';

angular.module('app_work').controller('EstimateCtrl', ['$scope', 'dataService', function ($scope, dataService) {

    //
    $scope.list_Normwork = [];
    getNormWorks();
    $scope.list_AllPrice = [];
    getListPrice();
    $scope.ListDetailNormWork_Price = [];
    GetDetailNormWork_Price();
    //data searched
    $scope.list_searched = [];
    

    function getNormWorks() {
        dataService.getNormworks().then(function (obj) {
            var eachItem;
            angular.forEach(obj.data, function (value, key) {
                eachItem = {
                    ID: value.MaHieuCV_DM,
                    Name: value.CongTac + value.RangBuoc,
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
        dataService.getListPrice().then(function (obj) {
            $scope.list_AllPrice = obj.data;
        });
    };

    function GetDetailNormWork_Price() {
        dataService.GetDetailNormWork_Price().then(function (obj) {
            $scope.ListDetailNormWork_Price = obj.data;
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
            angular.forEach($scope.list_searched, function (value, key) {
                console.log(div);
                //div.find("input").eq(1).val(value.Key_NormWork);
            });
        }
        $scope.popupsearchcss = {
            "display": "none"
        };
        $scope.popupsearchclass = '';
        $scope.popupsearch = !($scope.popupsearch);
    };
}])