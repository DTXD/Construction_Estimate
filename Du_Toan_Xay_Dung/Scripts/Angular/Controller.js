app.controller("postController", function ($scope, angularService) {
    //$scope.divEmployee = false;
    GetALLProJect();
    //To Get All Records  
    function GetALLProJect() {
        var getData = angularService.getproject();
        getData.then(function (emp) {
            $scope.projects = emp.data;
        },function () {
            alert('Xảy ra lỗi');
        });
    }
    $scope.deleteproject = function (project)
    {
        var getData = angularService.DeleteProject(project.MaCT);
        getData.then(function (msg) {
            //GetALLProJect();
            alert('Xóa thành công');
        },function(){
            alert('Xảy ra lỗi');
        });
    }  
});