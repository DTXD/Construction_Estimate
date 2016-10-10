
angular.module('app_work').factory('dataService', ['$http', function ($http) {

    var getAllSheet = function (buildingitem_id) {
        return $http({
            method: "GET",
            url: "/HangMuc/getAllSheets",
            params: { buildingitem_id: buildingitem_id }
        })
            .then(function (response) {
                return response.data;
            }, function (response) {
                //showing errors
            });
    };

    var getAllResource = function (buildingitem_id) {
        return $http({
            method: "GET",
            url: "/HangMuc/getAllResources",
            params: { buildingitem_id: buildingitem_id }
        })
            .then(function (response) {
                return response.data;
            }, function (response) {
                //showing errors
            });
    };


    var getGroupbyResources = function (buildingitem_id) {
        return $http({
            method: "GET",
            url: "/HangMuc/getGroupbyResources",
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



    var GetDetailNormWork_Price = function (area_id) {
        return $http({
            method: "GET",
            url: "/HangMuc/GetDetailNormWork_Price",
            params: { area_id: area_id }
        })
            .then(function (response) {
                return response.data;
            }, function (response) {
                //showing errors
            });
    };

    var GetArea_Price = function () {
        return $http.get('/HangMuc/GetArea_Price').then(function (response) {
            return response.data
        }, function (response) {
            //Showing errors
        });
    };

    return {
        GetArea_Price: GetArea_Price,
        getAllResource: getAllResource,
        getAllSheet: getAllSheet,
        getGroupbyResources: getGroupbyResources,
        getNormworks: getNormworks,
        getListPrice: getListPrice,
        GetDetailNormWork_Price: GetDetailNormWork_Price,
    };

}]);
