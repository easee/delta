(function() {
    app.controller("SalesByCategoryCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showSalesByCategory = false;
        $scope.save = function() {
            console.log("listing" + $scope.requestModel);
            DataService.insert("SalesByCategory", $scope.requestData, function(data) {
                $scope.SalesByCategoryData = data;
                $scope.showSalesByCategory = true;
            });
        }
    }]);
}());