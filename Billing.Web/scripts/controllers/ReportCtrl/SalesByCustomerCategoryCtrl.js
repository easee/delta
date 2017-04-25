(function() {
    application.controller("SalesByCustomerCategoryCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showCustomerCategory = false;

        $scope.save = function() {
            console.log("listing" + $scope.requestModel);
            DataService.insert("CustomersByCategory", $scope.requestData, function(data) {
                $scope.salesByCustomerCategorydata = data;
                $scope.showCustomerCategory = true;
            });
        }
    }]);
}());