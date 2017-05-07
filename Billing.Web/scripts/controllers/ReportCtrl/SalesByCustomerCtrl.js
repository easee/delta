(function() {
    app.controller("SalesByCustomerCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showCustomer = false;
        $scope.showPrint = false;
        $scope.showHeader = false;
        $scope.save = function() {
            console.log("listing" + $scope.requestModel);
            DataService.insert("SalesByCustomer", $scope.requestData, function(data) {
                $scope.salesByCustomerdata = data;
                $scope.showCustomer = true;
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