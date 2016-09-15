
angular.module('app_work').factory('dataService', ['$http', function ($http) {

    var getAllSheet = function (buildingitem_id) {
        return $http({
            method: "GET",
            url: "/HangMuc/getAllSheet",
            params: { buildingitem_id: buildingitem_id }
        })
            .then(function (response) {
                return response.data;
            }, function (response) {
                //showing errors
            });
    };

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



    var property = [];

    var getProperty = function () {
        return property;
    };

    var setProperty = function (value) {
        property = value;
    };

    return {
        getAllSheet : getAllSheet,
        getNormworks: getNormworks,
        getListPrice: getListPrice,
        GetDetailNormWork_Price: GetDetailNormWork_Price,
        getProperty: getProperty,
        setProperty: setProperty,
    };

}]);
