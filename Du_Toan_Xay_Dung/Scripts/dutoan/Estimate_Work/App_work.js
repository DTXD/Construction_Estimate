var app = angular.module('app_work', ['ui.bootstrap', 'ngRoute']);

app.config(['$routeProvider', function ($routeProvider) {

    $routeProvider
		.when('/formatCells', { templateUrl: '/Scripts/DuToan/Estimate_Work/views/formatCells.htm', controller: 'formatCellsCtrl' })
		// default...
        .when('/', { templateUrl: '/Scripts/DuToan/Estimate_Work/views/formatCells.htm', controller: 'formatCellsCtrl' })
		.otherwise({ redirectTo: '/' });
}]);