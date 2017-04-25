(function() {
    app.controller("SalesByCategoryCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showSalesByCategory = false;

        $scope.requestData = {
            startDate: new Date(Date.UTC(2016, 0, 1)),
            endDate: new Date(Date.UTC(2016, 11, 31)),
            id: 0
        };

        $scope.results = function() {
            DataService.insert("salesbycategory", $scope.requestData, function(data) {
                $scope.showSalesByCategoryData = data;
                $scope.showSalesByCategory = true;
            });
        }
    }]);
}());