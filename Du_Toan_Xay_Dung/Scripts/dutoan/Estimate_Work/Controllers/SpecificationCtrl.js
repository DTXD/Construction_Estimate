'use strict';

angular.module('app_work').controller('SpecificationCtrl', ['$scope', '$http', 'dataService', function ($scope, $http, dataService) {



    $scope.specifications = [];
    var buildingitem_ID = angular.element("#txt_building_item").val();
    var session_user = angular.element("#txt_session_user").val();
    if (typeof (buildingitem_ID) != "undefined" && typeof (session_user) != "undefined") {
        dataService.getAllSheet(buildingitem_ID).then(function (data) {
            //load data saved of user
        });
    }
    else {
        //create sheet
        for (var i = 0; i < 10; i++) {
            $scope.specifications.push({ IndexSheet: i });
        }
    }

    //var works = dataService.getProperty();
    

}]);