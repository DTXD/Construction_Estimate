'use strict';

angular.module('app_work').controller('EstimateCtrl', ['$scope', '$http', '$rootScope', 'dataService', function ($scope, $http, $rootScope, dataService) {

    //
    $scope.list_Normwork = [];
    getNormWorks();

    //
    $scope.checked = [];
    //
    $scope.listResource = [];
    /*
    $scope.list_AllPrice = [];
    getListPrice();
    */

    //data searched
    $scope.list_searched = [];

    function getNormWorks() {
        dataService.getNormworks().then(function (data) {
            var eachItem;
            angular.forEach(data, function (value, key) {
                eachItem = {
                    ID: value.ID,
                    Name: value.Name,
                    Unit: value.Unit,
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

    var buildingItem_id = angular.element("#txt_building_item").val();
    var session_user = angular.element("#txt_session_user").val();

    function SaveWorktoDatabase(items) {
        if (typeof (buildingItem_id) != "undefined" && typeof (session_user) != "undefined") {
            $scope.message_save = true;
            $http({
                method: "post",
                url: "/HangMuc/post_savework",
                data: JSON.stringify(items),
                dataType: "json",
            })
                .success(function (result) {
                    //display message
                    window.setTimeout(function () { $scope.message_save = false; }, 100);
                });
        }
        else {
            $scope.message_save = true;
            angular.element("#Message_saved").text("Bạn chưa đăng nhập...!!!");
        }
        
    };

    function UpdateWorktoDatabase(items) {
        if (typeof (buildingItem_id) != "undefined" && typeof (session_user) != "undefined") {
            $scope.message_save = true;
            $http({
                method: "post",
                url: "/HangMuc/post_updatework",
                data: JSON.stringify(items),
                dataType: "json",
            })
                .success(function (result) {
                    //display message
                    window.setTimeout(function () { $scope.message_save = false; }, 100);
                });
        }
        else {
            $scope.message_save = true;
            angular.element("#Message_saved").text("Bạn chưa đăng nhập...!!!");
        }
        
    };

    function SaveResourcetoDatabase(items) {
        if (typeof (buildingItem_id) != "undefined" && typeof (session_user) != "undefined") {
            $scope.message_save = true;
            $http({
                method: "post",
                url: "/HangMuc/post_updateresource",
                data: JSON.stringify(items),
                dataType: "json",
            })
                .success(function (result) {
                    window.setTimeout(function () { $scope.message_save = false; }, 100);
                });
        }
        else {
            $scope.message_save = true;
            angular.element("#Message_saved").text("Bạn chưa đăng nhập...!!!");
        }
    };


    //No in sheet
    $scope.No = -1;
    $scope.subcategory;
    $scope.focus = function (value_focus, $event) {
        if (typeof (buildingItem_id) != "undefined" && typeof (session_user) != "undefined") {
            //focus and get id sheet
            var div = angular.element($event.currentTarget).parent().parent();
            var id = div.find(".column_header input").val();
            //check if No get input changed and No != id
            if ($scope.No != -1 && $scope.No != id) {

                //save to db
                //check if ID ==null vaf Area ==null ==> Subcategory
                if ($rootScope.works[$scope.No].ID == "" && $rootScope.works[$scope.No].Area == "" && $rootScope.works[$scope.No].Name != "") {
                    $scope.subcategory = $scope.No;
                    SaveWorktoDatabase($rootScope.works[$scope.No]);
                }
                if ($rootScope.works[$scope.No].ID != "") {
                    $rootScope.works[$scope.No].Sub_BuildingItem_ID = $scope.subcategory;
                    SaveWorktoDatabase($rootScope.works[$scope.No]);
                }
                //....
                $scope.No = -1;
            }
            $scope.blur = function (value_blur) {
                //check input changed
                if (value_focus != value_blur) {
                    //changed put indexsheet to $scope.No
                    $scope.No = id;
                }
            };
        }
        else {
            $scope.message_save = true;
            angular.element("#Message_saved").text("Bạn chưa đăng nhập...!!!");
        }
    };

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

        //checked and then unchecked
        if (d > -1) {
            //delete work when uncheck
            $scope.list_searched.splice(d, 1);

            //delete resource when uncheck
            for (var i = 0; i < $scope.listResource.length; i++) {
                if ($scope.listResource[i].NormWork_ID == x.ID) {
                    $scope.listResource.splice(i, 1);
                    i--;
                }
            };

            //$scope.checked.splice(d, 1);

        }
        else {

            //push price data from database
            var pricematerial = 0;
            var pricelabol = 0;
            var pricemachine = 0;

            //should use filter array..................
            angular.forEach($rootScope.ListDetailNormWork_Price, function (value, key) {
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

                    var item_resource = {
                        NormWork_ID: value.Key_NormWork,
                        BuildingItem_ID: buildingItem_id,
                        UserWork_ID: "",
                        ID: "",
                        UnitPrice_ID: value.Key_Material,
                        Name: value.Name_Material,
                        Unit: value.Unit,
                        Number_Norm: value.Number,
                        Price: value.Price,
                    };
                    $scope.listResource.push(item_resource);
                }
            });
            x.pricematerial = pricematerial.toFixed(3);
            x.pricelabor = pricelabol.toFixed(3);
            x.pricemachine = pricemachine.toFixed(3);
            $scope.list_searched.push(x);

            //push index to list checked

            //$scope.checked.push(x.$$hashKey);
        }

        //change content button when list != null
        if ($scope.list_searched.length != 0) {
            btn_save.text("Chọn");
        }
        else {
            btn_save.text("Thoát");
        }

        //console.log($scope.checked);
    };

    $scope.save_search = function () {

        //check ID work and add new ID
        //...

        if ($scope.popupsearch == true && $scope.list_searched != 0) {
            var div = angular.element($scope.current_doubleclick.parent().parent());
            var id = div.find(".column_header input").val();
            var index = parseInt(id);

            angular.forEach($scope.list_searched, function (value, key) {
                $rootScope.works[index].ID = $rootScope.index_work;
                $rootScope.works[index].NormWork_ID = value.ID;
                $rootScope.works[index].Name = value.Name;
                $rootScope.works[index].Unit = value.Unit;
                $rootScope.works[index].PriceMaterial = value.pricematerial;
                $rootScope.works[index].PriceLabor = value.pricelabor;
                $rootScope.works[index].PriceMachine = value.pricemachine;
                $rootScope.works[index].SumMaterial = value.pricematerial;
                $rootScope.works[index].SumLabor = value.pricelabor;
                $rootScope.works[index].SumMachine = value.pricemachine;




                //save work and check log in
                $rootScope.works[index].Sub_BuildingItem_ID = $scope.subcategory;
                SaveWorktoDatabase($rootScope.works[index]);

                //save resource and check log in
                var temp_list = [];
                angular.forEach($scope.listResource, function (value, key) {
                    if (value.NormWork_ID == $rootScope.works[index].NormWork_ID) {
                        value.UserWork_ID = $rootScope.index_work;
                        temp_list.push(value);
                    }
                });
                SaveResourcetoDatabase(temp_list);


                //must fix index work when show work
                index = index + 1;
                $rootScope.index_work = parseInt($rootScope.index_work) + 1;


            });
        }
        $scope.popupsearchcss = {
            "display": "none"
        };
        $scope.popupsearchclass = '';
        $scope.popupsearch = !($scope.popupsearch);

        //console.log($scope.checked);

        //angular.forEach($scope.checked, function (value, key) {
        //    var arr_t = value.split(':');
        //    console.log($scope.list_Normwork[196]);
        //    //$scope.list_Normwork[arr_t[1]].checked = false;

        //});
        
        //$scope.checked = [];
        $scope.list_searched = [];
        angular.forEach($scope.list_Normwork, function (value, key) {
            value.checked = false;
        });
    };



    function substring_array(s, begin, end) {
        var string = String(s);
        string = string.substring(begin, end);
        return string;
    };

    $scope.change = function ($event) {
        var div = angular.element($event.currentTarget).parent().parent();
        var id_work = div.find(".column_header input").val();
        var id = div.find("input").eq(1).val();
        var number = div.find("input").eq(4).val();
        var horizontal = div.find("input").eq(5).val();
        var vertical = div.find("input").eq(6).val();
        var height = div.find("input").eq(7).val();
        var area = div.find("input").eq(8).val();

        //mean work
        var regular_expression1 = /^\d+$/;
        if (regular_expression1.test(id)) {

            if (number == "") {
                $rootScope.works[id_work].Number = 1;
            }
            if (horizontal == "") {
                $rootScope.works[id_work].Horizontal = 1;
            }
            if (vertical == "") {
                $rootScope.works[id_work].Vertical = 1;
            }
            if (height == "") {
                $rootScope.works[id_work].Height = 1;
            }
            $rootScope.works[id_work].Area = ($rootScope.works[id_work].Number * $rootScope.works[id_work].Horizontal * $rootScope.works[id_work].Vertical * $rootScope.works[id_work].Height).toFixed(3);

            $rootScope.works[id_work].SumMaterial = (parseFloat($rootScope.works[id_work].PriceMaterial) * parseFloat($rootScope.works[id_work].Area)).toFixed(3);
            $rootScope.works[id_work].SumLabor = (parseFloat($rootScope.works[id_work].PriceLabor) * parseFloat($rootScope.works[id_work].Area)).toFixed(3);
            $rootScope.works[id_work].SumMachine = (parseFloat($rootScope.works[id_work].PriceMachine) * parseFloat($rootScope.works[id_work].Area)).toFixed(3);


            //save work and check log in
            UpdateWorktoDatabase($rootScope.works[id_work]);
        }


        //description work
        var regular_expression2 = /^\d+\.\d+$/;
        if (regular_expression2.test(id)) {
            if (number == "") {
                $rootScope.works[id_work].Number = 1;
            }
            if (horizontal == "") {
                $rootScope.works[id_work].Horizontal = 1;
            }
            if (vertical == "") {
                $rootScope.works[id_work].Vertical = 1;
            }
            if (height == "") {
                $rootScope.works[id_work].Height = 1;
            }

            $rootScope.works[id_work].Area = ($rootScope.works[id_work].Number * $rootScope.works[id_work].Horizontal * $rootScope.works[id_work].Vertical * $rootScope.works[id_work].Height).toFixed(3);

            var d = id.indexOf(".");
            var temp = id.substring(0, d);
            var id_meanwork = -1;

            for (var i = id_work; i >= 0; i--) {
                if ($rootScope.works[i].ID == temp) {
                    id_meanwork = i;
                    break;
                }
            }

            if (id_meanwork != -1) {
                var sum = 0;
                var index_while = id_meanwork + 1;
                while (substring_array($rootScope.works[index_while].ID, 0, d) == temp && $rootScope.works[index_while].ID != "") {

                    sum = parseFloat(sum) + parseFloat($rootScope.works[index_while].Area);
                    index_while = parseInt(index_while) + 1;
                }

                $rootScope.works[id_meanwork].Area = sum.toFixed(3);
                $rootScope.works[id_meanwork].SumMaterial = (parseFloat($rootScope.works[id_meanwork].PriceMaterial) * parseFloat($rootScope.works[id_meanwork].Area)).toFixed(3);
                $rootScope.works[id_meanwork].SumLabor = (parseFloat($rootScope.works[id_meanwork].PriceLabor) * parseFloat($rootScope.works[id_meanwork].Area)).toFixed(3);
                $rootScope.works[id_meanwork].SumMachine = (parseFloat($rootScope.works[id_meanwork].PriceMachine) * parseFloat($rootScope.works[id_meanwork].Area)).toFixed(3);

                //save work and check log in
                UpdateWorktoDatabase($rootScope.works[id_meanwork]);
            }


        }

    };
}])