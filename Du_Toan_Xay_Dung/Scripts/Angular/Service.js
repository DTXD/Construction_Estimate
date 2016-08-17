app.service("angularService", function ($http) {

    //get All Eployee
    this.getproject = function () {
        return $http.get("CongTrinh/getAll");
    };
    //Delete Employee
    this.DeleteProject = function (idproject) {
        var response = $http({
            method: "post",
            url: "CongTrinh/DeleteProject",
            params: {
                employeeId: JSON.stringify(idproject)
            }
        });
        return response;
    }
});