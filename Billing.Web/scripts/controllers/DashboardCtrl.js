(function() {
    app.controller("DashboardCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showRegion = false;
        $scope.save = function() {
            DataService.insert("SalesByRegion", $scope.requestData, function(data) {
                $scope.salesbyRegiondata = data;
                $scope.showRegion = true;
            });
        }

        // $scope.agent.id = $scope.requestData.id;
        $scope.agentSales = function(currentAgent) {
                            $scope.requestData.id=currentAgent;
                            DataService.insert("salesbyagent", $scope.requestData, function(data){
                                $scope.salesByAgentData = data;
                        })
                
            };
    }]);
}());