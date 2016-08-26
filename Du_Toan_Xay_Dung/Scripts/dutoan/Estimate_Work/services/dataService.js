

angular.module('app_work').factory('dataService', ['$http', function ($http) {
    var getNormworks = function () {
        return $http({
            method: 'POST',
            url: '/HangMuc/GetNormWorks',
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

    var getListPrice = function () {
        return $http({
            method: 'POST',
            url: '/HangMuc/GetDSDonGia',
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


    var GetDetailNormWork_Price = function () {
        return $http({
            method: 'POST',
            url: '/HangMuc/GetDetailNormWork_Price',
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
        getNormworks: getNormworks,
        getListPrice: getListPrice,
        GetDetailNormWork_Price: GetDetailNormWork_Price
    };

}]);
