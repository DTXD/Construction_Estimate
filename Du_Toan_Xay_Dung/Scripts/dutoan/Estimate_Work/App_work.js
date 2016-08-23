var app = angular.module("app_work", ["ui.router", "ui.bootstrap"]);

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


app.controller("mainController", ['$scope', function ($scope) {

    /*
    $scope.tabs = [
        { heading: "Tab 1", route: "/Views_Angularjs/Estimate.htm", active: true },
        { heading: "Tab 2", route: "/Views_Angularjs/Estimate.htm", active: true }
    ];
    */
}]);
