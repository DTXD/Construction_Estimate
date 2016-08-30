
angular.module('app_work').factory('dataService', ['$http', function ($http) {
    var getNormworks = function () {
        return $http.get('/HangMuc/GetNormWorks').then(function (response) {
            return response.data
        }, function (response) {
            //Showing errors
        });
    };

    

    var getListPrice = function () {
        return $http.get('/HangMuc/GetDSDonGia').then(function (response) {
            return response.data
        }, function (response) {
            //Showing errors
        });
    };

    

    var GetDetailNormWork_Price = function () {
        return $http.get('/HangMuc/GetDetailNormWork_Price').then(function (response) {
            return response.data
        }, function (response) {
            //Showing errors
        });
    };



    return {
        getNormworks: getNormworks,
        getListPrice: getListPrice,
        GetDetailNormWork_Price: GetDetailNormWork_Price
    };

}]);
