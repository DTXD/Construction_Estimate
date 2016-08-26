'use strict';


angular.module('postApp').factory('hangmucservice', ['$http', function ($http) {
    var getNormworks = function () {
        return $http({
            method: 'POST',
            url: '/CongTrinh/listhangmuc',
            headers: { 'Content-Type': 'application/json' }
        })
                  .success(function (data) {
                      if (data.errors) {
                          // Showing errors.
                          $scope.errorEmail = data.errors.email;
                      } else {
                          return data;
                      }
                  });
    };

    return {
        hangmucs: getNormworks       
    };

}]);
