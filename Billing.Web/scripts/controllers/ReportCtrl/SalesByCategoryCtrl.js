(function() {
    app.controller("SalesByCategoryCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showSalesByCategory = false;
        $scope.showPrint = false;
        $scope.showHeader = false;
        $scope.showHeader2 = false;
        $scope.showProduct = false;
        
 $scope.salesByProduct = function(item) {
                            $scope.requestData.id=item;
                            DataService.insert("salesbyproduct", $scope.requestData, function(data){
                                $scope.product = data;
                                $scope.showProduct = true;
                        })
                
            };
       
        $scope.printDiv = function (divName,data=0) {
        if(data!=0){
           document.getElementById("hide").style.visibility="hidden";
           for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    document.getElementById(obj.categoryName).style.visibility="hidden";             
               }
           document.getElementById("emptyGrid").style.visibility="hidden";
        }
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
           if(data!=0){
           document.getElementById("hide").style.visibility="visible";
            for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    document.getElementById(obj.categoryName).style.visibility="visible";             
               }
           document.getElementById("emptyGrid").style.visibility="visible";
           }
       };

        $scope.save = function() {
            console.log("listing" + $scope.requestModel);
            DataService.insert("SalesByCategory", $scope.requestData, function(data) {
                $scope.SalesByCategoryData = data;
                $scope.showSalesByCategory = true;
                $scope.showPrint = true;
                $scope.infoHide = true;
            });
        }

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