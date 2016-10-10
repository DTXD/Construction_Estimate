'use strict';

angular.module('app_work').controller('MaterialCtrl', ['$scope', '$http', 'dataService', function ($scope, $http, dataService) {


    $scope.materials = [];
    var buildingItem_id = angular.element("#txt_building_item").val();
    var session_user = angular.element("#txt_session_user").val();


    if (typeof (buildingItem_id) != "undefined" && typeof (session_user) != "undefined") {
        dataService.getGroupbyResources(buildingItem_id).then(function (data) {

            var d = 0;
            //load data saved of user
            angular.forEach(data, function (value, key) {

                var obj = {
                    IndexSheet: d,
                    Category: value.UnitPrice_ID.substring(0,1),
                    Name: value.Name,
                    Unit: value.Unit,
                    Number: value.Number_Norm,
                    Price: value.Price,
                    Sum: parseFloat(value.Number_Norm) * parseFloat(value.Price),
                    UnitPrice_ID: value.UnitPrice_ID,
                    BuildingItem_ID: ""
                };

                $scope.materials.push(obj);
                d = parseInt(d) + 1;

            });

            for (var i = d; i < 10; i++) {
                $scope.materials.push({ IndexSheet: i });
            }

        });
    }
    else {
        //create sheet
        for (var i = 0; i < 10; i++) {
            $scope.materials.push({ IndexSheet: i });
        }
    }




}]);