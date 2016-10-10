'use strict';

angular.module('app_work').controller('SpecificationCtrl', ['$scope', '$http', '$rootScope', 'dataService', function ($scope, $http, $rootScope, dataService) {



    $scope.specifications = [];


    //check session and building id
    var buildingItem_id = angular.element("#txt_building_item").val();
    var session_user = angular.element("#txt_session_user").val();

    if (typeof (buildingItem_id) != "undefined" && typeof (session_user) != "undefined") {

        var temp = [];
        var d = 0;

        dataService.getAllResource(buildingItem_id).then(function (resources) {
            //console.log(works);
            angular.forEach($rootScope.works, function (value, key) {

                //each work
                var regular_expression = /^\d+$/;
                if (regular_expression.test(value.ID) && value.ID != null) {
                    var obj_work = {
                        IndexSheet: d,
                        ID: value.ID,
                        NormWork_ID: value.NormWork_ID,
                        Name: value.Name,
                        Unit: value.Unit,
                        Number_Work: value.Area,
                        Norm: "",
                        Number_Resource: "",
                        Price: value.Price,
                        Category: "",
                        Sum: "",
                        BuildingItem_ID: ""
                    };

                    $scope.specifications.push(obj_work);
                    d = parseInt(d) + 1;
                }

                //filter resource
                temp = resources.filter(function (item) {
                    return (item.UserWork_ID == value.ID);
                });

                //display resource
                angular.forEach(temp, function (v, k) {
                    var obj_resource = {
                        IndexSheet: d,
                        ID: value.ID,
                        NormWork_ID: "",
                        Name: v.Name,
                        Unit: v.Unit,
                        Number_Work: "",
                        Norm: v.Number_Norm,
                        Number_Resource: parseFloat(value.Area) * parseFloat(v.Number_Norm),
                        Price: v.Price,
                        Category: v.UnitPrice_ID.substring(0, 1),
                        Sum: parseFloat(value.Area) * parseFloat(v.Number_Norm) * parseFloat(v.Price),
                        BuildingItem_ID: ""
                    };
                    $scope.specifications.push(obj_resource);
                    d = parseInt(d) + 1;
                });


            });

            for (var i = d; i < 10; i++) {
                $scope.specifications.push({ IndexSheet: i });
            }

        });


    }
    else {
        //create sheet
        for (var i = 0; i < 10; i++) {
            $scope.specifications.push({ IndexSheet: i });
        }
    }

    //var works = dataService.getProperty();
    //user just change Norm, Price resource.

}]);