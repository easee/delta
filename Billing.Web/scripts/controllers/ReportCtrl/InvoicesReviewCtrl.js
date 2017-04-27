(function(){
    app.controller("InvoicesReviewCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
     ListCustomers();
     $scope.showInvoice = false;
     $scope.showProducts = false;
     function ListCustomers(){
            DataService.list("customers/all", function(data){ $scope.customers = data});           
        };      
      
        $scope.getInvoices = function(currentId) {
            $scope.requestData.id=currentId;
            DataService.insert("invoicesreview", $scope.requestData, function(data) {
                $scope.invoices = data;
                $scope.showInvoice = true;
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

        $scope.modelOptions = {
            debounce: {
                default: 500,
                blur: 250
            },
            getterSetter: true
        };

        
    }]);
}());