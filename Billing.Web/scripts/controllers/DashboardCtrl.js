(function() {
    app.controller("DashboardCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
       ListDashboard();

        function ListDashboard() {
            DataService.list("dashboard", function(data) { 
            $scope.dashData = data;
            console.log($scope.dashData);
        
            });
        };

    }]);
}());