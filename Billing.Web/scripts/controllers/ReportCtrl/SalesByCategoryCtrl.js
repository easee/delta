(function() {
    app.controller("SalesByCategoryCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showSalesByCategory = false;
        $scope.requestData = {
            startDate: new Date(2016, 1, 1),
            endDate: new Date(2017, 1, 1)
        };
        $scope.openStart = function($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.startOpened = true;
        };

        $scope.openEnd = function($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.endOpened = true;
        };
        $scope.save = function() {
            console.log("listing" + $scope.requestModel);
            DataService.insert("SalesByCategory", $scope.requestData, function(data) {
                $scope.SalesByCategoryData = data;
                $scope.showSalesByCategory = true;
            });
        }
    }]);
}());