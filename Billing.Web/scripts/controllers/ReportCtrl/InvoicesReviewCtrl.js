(function(){
    app.controller("InvoicesReviewCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
     ListCustomers();
     $scope.showInvoice = false;
     $scope.showProducts = false;
     $scope.showPrint = false;
     $scope.showHeader = false;
     function ListCustomers(){
            DataService.list("customers/all", function(data){ $scope.customers = data});           
        };      
      
        $scope.getInvoices = function(currentId) {
            $scope.requestData.id=currentId;
            DataService.insert("invoicesreview", $scope.requestData, function(data) {
                $scope.invoices = data;
                $scope.showInvoice = true;
                $scope.showPrint = true;
            });
        }
        
        $scope.getProducts = function (item) {            
                DataService.read("invoicereview",item, function (data) { 
                    $scope.invoice = data; 
                    $scope.showProducts = true;
                })
            };
        
        //TYPEAHEAD CATEGORIES
        var _selected;
        $scope.selected = (undefined);

        $scope.ngModelOptionsSelected = function(value) {
            if (arguments.length) {
                _selected = value;
            } else {
                return _selected;
            }
        };
           $scope.printDiv = function (divName,data) {
           document.getElementById("hide").style.visibility="hidden";
           for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    document.getElementById(obj.invoiceNo).style.visibility="hidden";             
               }
            
           var printContents = document.getElementById(divName).innerHTML;
           var popupWin = window.open('', '_blank', 'width=1000,height=1000');
           $scope.showHeader = true;
           popupWin.document.open();
           popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="styles/bootstrap.min.css" /><link rel="stylesheet" type="text/css" href="styles/style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
           popupWin.document.close();
           $scope.showHeader = false;
           document.getElementById("hide").style.visibility="visible";
           for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    document.getElementById(obj.invoiceNo).style.visibility="visible";             
               }
       };

        $scope.modelOptions = {
            debounce: {
                default: 500,
                blur: 250
            },
            getterSetter: true
        };

        
    }]);
}());