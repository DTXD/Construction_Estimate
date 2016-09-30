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

    
    //intialize object file excel
    $scope.stepsModel = null;

    $scope.uploadedFile = function (event) {
        $scope.$apply(function ($scope) {
            $scope.files = event.files;
        });
        var files = event.target.files; //FileList object
        var file = files[0];
        if (typeof (file) != 'undefined') {
            var filename = file.name;
            var allowedFiles = [".xls", ".xlsx"];
            var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
            if (regex.test(filename.toLowerCase())) {
                var reader = new FileReader();
                reader.readAsDataURL(file);
                $scope.stepsModel = file;
            }
        }
    }

    $scope.submit_unitprice = function () {
        if ($scope.stepsModel != null) {

            $http.post('/CongTrinh/Post_ThemCongTrinh', fd, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
             .success(function (result) {
                 if (result == "error") {
                     return;
                 } else {
                     window.location.href = "/CongTrinh/Index";

                 }
             });

        }
        else {

        }
    }
}]);
