(function() {
    app.controller("SalesByCategoryCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showSalesByCategory = false;
        $scope.showPrint = false;
        $scope.showHeader = false;
        $scope.showHeader2 = false;
        $scope.showProduct = false;
        
        $scope.openStart = function($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.startOpened = true;
        };
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
                $scope.showPrint = true;
                $scope.infoHide = true;
            });
        }
    }]);
}());