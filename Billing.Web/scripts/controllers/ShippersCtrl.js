(function(){

    var app = angular.module("Billing");

    var ShippersCtrl = function($scope, $http) {
        $http.defaults.headers.common.Token = "12345678901234567890";
        $http.defaults.headers.common.ApiKey = "RGVsdGEtQmlsbGluZw==";

        $scope.showShipper = false;
        ListShippers();

        $scope.getShipper = function(currentShipper){
            $scope.shipper = currentShipper;
            $scope.showShipper = true;
        };

        $scope.save = function(){
            var promise = $http({
                method: "put",
                url: "http://localhost:9000/api/shippers/" + $scope.shipper.id,
                data: $scope.shipper
            });

            $scope.message = "Please wait...";
            promise.then(function(response){
                $scope.shipper = response.data;
                $scope.message = " ";
                ListShippers();
            }, function(reason){
                $scope.message = "No data for that request";
                });
            };

            $scope.new = function(){
                $scope.shipper.id = 0;
            var promise = $http({
                method: "post",
                url: "http://localhost:9000/api/shippers",
                data: $scope.shipper
            });
            
            $scope.message = "Please wait...";
            promise.then(function(response){
                $scope.shipper = response.data;
                $scope.message = " ";
                ListShippers();
            }, function(reason){
                $scope.message = "No data for that request";
            });
        };

            function ListShippers(){
            var promise = $http.get("http://localhost:9000/api/shippers");
            $scope.message = "Please wait for shippers...";
            promise.then(function(response){
                $scope.shippers = response.data;
                $scope.message = " ";
            }, function(reason){
                $scope.message = "No data for that request";
            });
        }
    };

    app.controller("ShippersCtrl", ShippersCtrl);

}());
