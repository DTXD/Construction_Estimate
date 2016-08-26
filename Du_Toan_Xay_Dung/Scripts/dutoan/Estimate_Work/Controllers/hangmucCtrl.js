'use strict';

angular.module('postApp').controller('postController', ['$scope', 'hangmucservice', function ($scope, hangmucservice) {
    //
    $scope.listhangmuc = [];
    getNormWorks();

    function getNormWorks() {
        hangmucservice.getNormworks().then(function (obj) {
            var eachItem;
            angular.forEach(obj.data, function (value, key) {
                eachItem = {
                    MaHM: value.MaHM,
                    TenHM: value.TenHM,
                    MoTa: value.MoTa,
                    Gia:value.Gia
                };
                $scope.listhangmuc.push(eachItem);
            });
        });
    };

   
}])