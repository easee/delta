(function(){

    var app = angular.module("Billing");

    var CategoriesCtrl = function($scope, $http) {
        $http.defaults.headers.common.Token = "12345678901234567890";
        $http.defaults.headers.common.ApiKey = "RGVsdGEtQmlsbGluZw==";

        $scope.showCategories = false;
        ListCategories();

        $scope.getCategory = function(currentCategory){
            $scope.category = currentCategory;
            $scope.showCategories = true;
        };

        $scope.save = function(){
            var promise = $http({
                method: "put",
                url: "http://localhost:9000/api/categories/" + $scope.category.id,
                data: $scope.category
            });

            $scope.message = "Please wait...";
            promise.then(function(response){
                $scope.category = response.data;
                $scope.message = " ";
                ListCategories();
            }, function(reason){
                $scope.message = "No data for that request";
                });
            };

            $scope.new = function(){
                $scope.category.id = 0;
            var promise = $http({
                method: "post",
                url: "http://localhost:9000/api/categories",
                data: $scope.category
            });
            
            $scope.message = "Please wait...";
            promise.then(function(response){
                $scope.category = response.data;
                $scope.message = " ";
                ListCategories();
            }, function(reason){
                $scope.message = "No data for that request";
            });
        };

            function ListCategories(){
            var promise = $http.get("http://localhost:9000/api/categories");
            $scope.message = "Please wait for category...";
            promise.then(function(response){
                $scope.category = response.data;
                $scope.message = " ";
            }, function(reason){
                $scope.message = "No data for that request";
            });
        }
    };

    app.controller("CategoriesCtrl", CategoriesCtrl);

}());