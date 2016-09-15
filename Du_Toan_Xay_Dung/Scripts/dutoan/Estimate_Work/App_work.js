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

app.controller("mainController", ['$scope', '$rootScope', function ($scope, root) {

    $scope.treeview = function ($event) {

        var div = angular.element($event.currentTarget);
        var icon = div.find("i").eq(1);
        var div_view = div.parent().find(".treeview-menu");
        console.log(div.parent().hasClass("menu-open"));
        if (div_view.hasClass("menu-open")) {
            div_view.css("display", "none");
            div_view.removeClass("menu-open");
            icon.css({
                "-ms-transform": "",
                "-webkit-transform": "",
                "transform": ""
            });
            console.log("haha");
        }
        else {
            div_view.css("display", "");
            div_view.addClass("menu-open");
            icon.css({
                "-ms-transform": "rotate(-90deg) /* IE 9 */",
                "-webkit-transform": "rotate(-90deg) /* Chrome, Safari, Opera */",
                "transform": "rotate(-90deg)"
            });
            console.log("hoho");
        }
    };

}]);
