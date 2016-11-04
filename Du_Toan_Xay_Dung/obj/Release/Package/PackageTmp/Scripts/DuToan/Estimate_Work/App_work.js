var app = angular.module("app_work", ["ui.router", "ui.bootstrap"]);

app.run(function ($rootScope) {
    $rootScope.works = [];
    $rootScope.allmaterials = [];
});

app.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/');              //home
    $stateProvider

        // HOME STATES AND NESTED VIEWS ========================================
        .state('DuToan', {
            url: '/',
            templateUrl: '/Views_Angularjs/Estimate.htm'
        })
        .state('HaoPhi', {
            url: '/HaoPhi',
            templateUrl: '/Views_Angularjs/Material.htm'
        })
        .state('PhanTich', {
            url: '/PhanTich',
            templateUrl: '/Views_Angularjs/Specification.htm'
        })
        .state('TongHop', {
            url: '/TongHop',
            templateUrl: '/Views_Angularjs/General.htm'
        })
});

app.controller("mainController", ['$scope', '$rootScope', 'dataService', '$http', function ($scope, $rootScope, dataService, $http) {

    $scope.treeview = function ($event) {

        var div = angular.element($event.currentTarget);
        var icon = div.find("i").eq(1);
        var div_view = div.parent().find(".treeview-menu");
        //console.log(div.parent().hasClass("menu-open"));
        if (div_view.hasClass("menu-open")) {
            div_view.css("display", "none");
            div_view.removeClass("menu-open");
            icon.css({
                "-ms-transform": "",
                "-webkit-transform": "",
                "transform": ""
            });
            //console.log("haha");
        }
        else {
            div_view.css("display", "");
            div_view.addClass("menu-open");
            icon.css({
                "-ms-transform": "rotate(-90deg) /* IE 9 */",
                "-webkit-transform": "rotate(-90deg) /* Chrome, Safari, Opera */",
                "transform": "rotate(-90deg)"
            });
            //console.log("hoho");
        }
    };

    get_areapirce();
    $rootScope.ListDetailNormWork_Price = [];

    function get_areapirce() {
        dataService.GetArea_Price().then(function (data) {
            $scope.unitprice = {
                availableOptions: data,
                selectedOption: { ID: '1' } //This sets the default value of the select in the ui
            };
            GetDetailNormWork_Price($scope.unitprice.selectedOption.ID);
        });
    };

    function GetDetailNormWork_Price(area_id) {
        dataService.GetDetailNormWork_Price(area_id).then(function (data) {
            $rootScope.ListDetailNormWork_Price = data;
        });
    };


    $scope.change_price = function (selection) {
        GetDetailNormWork_Price(selection);
    };

    

    $scope.get_UnitPrice = function () {
        
    };

    $rootScope.index_work = 1;

    //check session and building id
    var buildingItem_id = angular.element("#txt_building_item").val();
    var session_user = angular.element("#txt_session_user").val();

    //create list works
    for (var i = 0; i < 10; i++) {
        var item = {
            IndexSheet: i,
            ID: "",
            NormWork_ID: "",
            Name: "",
            Unit: "",
            Number: "",
            Horizontal: "",
            Vertical: "",
            Height: "",
            Area: "",
            PriceMaterial: "",
            PriceLabor: "",
            PriceMachine: "",
            SumMaterial: "",
            SumLabor: "",
            SumMachine: "",
            BuildingItem_ID: buildingItem_id,
            Sub_BuildingItem_ID: ""
        };
        $rootScope.works.push(item);
    }


    if (typeof (buildingItem_id) != "undefined" && typeof (session_user) != "undefined") {
        dataService.getAllSheet(buildingItem_id).then(function (data) {
            angular.forEach(data, function (value, key) {
                $rootScope.works[value.IndexSheet].ID = value.ID;
                $rootScope.works[value.IndexSheet].NormWork_ID = value.NormWork_ID;
                $rootScope.works[value.IndexSheet].Name = value.Name;
                $rootScope.works[value.IndexSheet].Unit = value.Unit;
                $rootScope.works[value.IndexSheet].Number = value.Number;
                $rootScope.works[value.IndexSheet].Horizontal = value.Horizontal;
                $rootScope.works[value.IndexSheet].Vertical = value.Vertical;
                $rootScope.works[value.IndexSheet].Height = value.Height;
                $rootScope.works[value.IndexSheet].Area = value.Area;
                $rootScope.works[value.IndexSheet].PriceMaterial = value.PriceMaterial;
                $rootScope.works[value.IndexSheet].PriceLabor = value.PriceLabor;
                $rootScope.works[value.IndexSheet].PriceMachine = value.PriceMachine;
                $rootScope.works[value.IndexSheet].SumMaterial = value.SumMaterial;
                $rootScope.works[value.IndexSheet].SumLabor = value.SumLabor;
                $rootScope.works[value.IndexSheet].SumMachine = value.SumMachine;
                $rootScope.works[value.IndexSheet].BuildingItem_ID = value.BuildingItem_ID;
                $rootScope.works[value.IndexSheet].Sub_BuildingItem_ID = value.Sub_BuildingItem_ID;

                var regular_expression = /^\d+$/;
                if (regular_expression.test(value.ID)) {
                    $rootScope.index_work = parseInt($rootScope.index_work) + 1;
                }
            });
        });
    }

}]);
