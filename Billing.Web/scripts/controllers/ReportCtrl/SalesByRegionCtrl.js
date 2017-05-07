(function() {
    app.controller("SalesByRegionCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showRegion = false;
        $scope.showPrint = false;
        $scope.showHeader = false;
        $scope.showHeader2 = false;
        $scope.save = function() {
            DataService.insert("SalesByRegion", $scope.requestData, function(data) {
                $scope.salesbyRegiondata = data;
                $scope.showRegion = true;
                $scope.showPrint = true;
            });
        }

        // $scope.agent.id = $scope.requestData.id;
        $scope.agentSales = function(currentAgent) {
                            $scope.requestData.id=currentAgent;
                            DataService.insert("salesbyagent", $scope.requestData, function(data){
                                $scope.salesByAgentData = data;
                        })
                
            };
        $scope.printDiv = function (divName) {
           var printContents = document.getElementById(divName).innerHTML;
           var popupWin = window.open('', '_blank', 'width=1000,height=1000');
           if(divName=="printable")
           $scope.showHeader = true;
            else if(divName=="printable2")
            $scope.showHeader2 = true;
           popupWin.document.open();
           popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="styles/bootstrap.min.css" /><link rel="stylesheet" type="text/css" href="styles/style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
           popupWin.document.close();
           $scope.showHeader = false;
           $scope.showHeader2 = false;
       };
       //Datepicker
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
    }]);
}());