(function() {
    app.controller("AgentRegionCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showAgentRegion = false;
        $scope.showHeader = false;
        $scope.showPrint = false;
        $scope.save = function() {
            console.log("listing" + $scope.requestModel);
            DataService.insert("CrossAgentRegion", $scope.requestData, function(data) {
                $scope.salesByAgentRegiondata = data;
                $scope.showAgentRegion = true;
                 $scope.showPrint = true;
            });
        }
         $scope.printDiv = function (divName) {
           var printContents = document.getElementById(divName).innerHTML;
           var popupWin = window.open('', '_blank', 'width=1000,height=1000');
           $scope.showHeader = true;
           popupWin.document.open();
           popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="styles/bootstrap.min.css" /><link rel="stylesheet" type="text/css" href="styles/style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
           popupWin.document.close();
           $scope.showHeader = false;
       };
    }]);
}());