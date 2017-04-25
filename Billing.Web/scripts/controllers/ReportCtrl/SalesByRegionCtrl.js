(function() {
    app.controller("SalesByRegionCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showRegion = false;
        $scope.save = function() {
            DataService.insert("SalesByRegion", $scope.requestData, function(data) {
                $scope.salesbyRegiondata = data;
                $scope.showRegion = true;
            });
        }
    }]);
}());