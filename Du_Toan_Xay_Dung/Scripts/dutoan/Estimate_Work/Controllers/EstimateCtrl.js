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
                    ID: value.ID,
                    Name: value.Name,
                    Unit: value.Unit,
                    summaterial: 0,
                    sumlabor: 0,
                    summachine: 0
                };
                $scope.list_Normwork.push(eachItem);
            });
        });
    };

    //modify list_normwork only have 100 record


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
            var summaterial = 0;
            var sumlabor = 0;
            var summachine = 0;
            angular.forEach($scope.ListDetailNormWork_Price, function (value, key) {
                if (x.ID == value.Key_NormWork) {
                    if (value.Key_Material.substring(0, 1) == "N") {
                        summaterial = parseFloat(summaterial + (value.Number * value.Price));
                    }
                    if (value.Key_Material.substring(0, 1) == "M") {
                        sumlabor = parseFloat(sumlabor + (value.Number * value.Price));
                    }
                    if (value.Key_Material.substring(0, 1) == "V") {
                        summachine = parseFloat(summachine + (value.Number * value.Price));
                    }
                }
            });
            x.summaterial = summaterial.toFixed(3);
            x.sumlabor = sumlabor.toFixed(3);
            x.summachine = summachine.toFixed(3);
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
                $scope.works[index].summaterial = value.summaterial;
                $scope.works[index].sumlabor = value.sumlabor;
                $scope.works[index].summachine = value.summachine;

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
        var number = div.find("input").eq(3).val();
        var horizontal = div.find("input").eq(4).val();
        var vertical = div.find("input").eq(5).val();
        var height = div.find("input").eq(6).val();
        var area = div.find("input").eq(7).val();

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
            if (area == "") {
                area = 1;
            }

            $scope.works[id_work].area = ($scope.works[id_work].number * $scope.works[id_work].horizontal * $scope.works[id_work].vertical * $scope.works[id_work].height).toFixed(3);
            var summaterial = parseFloat($scope.works[id_work].summaterial) / parseFloat(area);
            var sumlabor = parseFloat($scope.works[id_work].sumlabor) / parseFloat(area);
            var summachine = parseFloat($scope.works[id_work].summachine) / parseFloat(area);

            $scope.works[id_work].summaterial = (parseFloat(summaterial) * parseFloat($scope.works[id_work].area)).toFixed(3);
            $scope.works[id_work].sumlabor = (parseFloat(sumlabor) * parseFloat($scope.works[id_work].area)).toFixed(3);
            $scope.works[id_work].summachine = (parseFloat(summachine) * parseFloat($scope.works[id_work].area)).toFixed(3);
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
            var id_meanwork = -1;


            for (var i = id_work; i >= 0; i--) {
                if ($scope.works[i].id == temp) {
                    id_meanwork = i;
                    console.log(id_meanwork);
                    break;
                }
            }

            if (id_meanwork != -1) {
                var sum = 0;
                var index_while = id_meanwork + 1;
                while (substring_array($scope.works[index_while].id, 0, d) == temp && $scope.works[index_while].id != "") {

                    sum = parseFloat(sum) + parseFloat($scope.works[index_while].area);
                    index_while = parseInt(index_while) + 1;
                }

                if (typeof ($scope.works[id_meanwork].area) == "undefined") {
                    $scope.works[id_meanwork].area = 1;

                }

                var summaterial = parseFloat($scope.works[id_meanwork].summaterial) / parseFloat($scope.works[id_meanwork].area);
                var sumlabor = parseFloat($scope.works[id_meanwork].sumlabor) / parseFloat($scope.works[id_meanwork].area);
                var summachine = parseFloat($scope.works[id_meanwork].summachine) / parseFloat($scope.works[id_meanwork].area);

                $scope.works[id_meanwork].area = sum.toFixed(3);

                $scope.works[id_meanwork].summaterial = (parseFloat(summaterial) * parseFloat($scope.works[id_meanwork].area)).toFixed(3);
                $scope.works[id_meanwork].sumlabor = (parseFloat(sumlabor) * parseFloat($scope.works[id_meanwork].area)).toFixed(3);
                $scope.works[id_meanwork].summachine = (parseFloat(summachine) * parseFloat($scope.works[id_meanwork].area)).toFixed(3);
            }
        }

    };


    $scope.save_work = function () {

        var buildingitem_ID = angular.element("#txt_building_item").val();
        console.log(buildingitem_ID);
        //check work
        angular.forEach($scope.works, function (value, key) {

        });


        /*
        $http({
            method: 'POST',
            url: '/HangMuc/post_updatework',
            data: JSON.stringify($scope.works),
            headers: { 'Content-Type': 'application/json' }
        })
                 .success(function (result) {
                     if (result == "error") {
                         angular.element("#response_savesheet").html("Lỗi...!!!")
                         return;
                     } else {
                         //$scope.message = data.message;
                         angular.element("#response_savesheet").html("Dữ liệu đã lưu...!!!")
                     }
                 });
                 */

    };

}])