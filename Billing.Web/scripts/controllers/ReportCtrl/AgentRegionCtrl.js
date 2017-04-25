(function() {
    app.controller("AgentRegionCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showAgentRegion = false;
        $scope.save = function() {
            console.log("listing" + $scope.requestModel);
            DataService.insert("CrossAgentRegion", $scope.requestData, function(data) {
                $scope.salesByAgentRegiondata = data;
                $scope.showAgentRegion = true;
            });
        }
    }]);
}());