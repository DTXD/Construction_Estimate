'use strict';

angular.module('app_work').controller('EstimateCtrl', ['$scope', function ($scope) {

    //create sheet
    //get element div
    /*
    $scope.eachwork = [{
        'id': '',
        'key': '',
        'name': '',
        'unit': '',
        'number': '',
        'horizontal': '',
        'vertical': '',
        'height': '',
        'area': '',
        'pricematerial': '',
        'pricelabor': '',
        'pricemachine': '',
        'summaterial': '',
        'sumlabor': '',
        'summachine': '',
    }];
    */
    $scope.works = [];

    for (var i = 0; i < 10; i++) {
        $scope.works.push({ index: i });
    }
}])