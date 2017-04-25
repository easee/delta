(function() {
    app.controller("SalesByCustomerCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showCustomer = false;
        $scope.save = function() {
            console.log("listing" + $scope.requestModel);
            DataService.insert("SalesByCustomer", $scope.requestData, function(data) {
                $scope.salesByCustomerdata = data;
                $scope.showCustomer = true;
            });
        }
    }]);
}());