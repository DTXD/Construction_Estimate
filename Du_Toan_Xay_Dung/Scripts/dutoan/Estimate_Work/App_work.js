var app = angular.module('app_work', ['ui.bootstrap', 'ngRoute']);

app.config(['$routeProvider', function ($routeProvider) {

    $routeProvider
		.when('/formatCells', { templateUrl: '/Views_Angularjs/formatCells.htm', controller: 'formatCellsCtrl' })
		// default...
        .when('/', { templateUrl: '/Views_Angularjs/formatCells.htm', controller: 'formatCellsCtrl' })
		.otherwise({ redirectTo: '/' });
}]);