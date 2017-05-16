(function() {
    app.controller("SalesByCustomerCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showCustomer = false;
        $scope.showPrint = false;
        $scope.showHeader = false;
        $scope.save = function() {
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
       //Angular UI Datepicker JS must have
        $scope.open1 = function () {
            $scope.popup1.opened = true;
        };
        $scope.open2 = function () {
            $scope.popup2.opened = true;
        };

        $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[2];
        $scope.altInputFormats = ['M!/d!/yyyy'];

        $scope.popup1 = {
            opened: false
        };
        $scope.popup2 = {
            opened: false
        };

    }]);
}());