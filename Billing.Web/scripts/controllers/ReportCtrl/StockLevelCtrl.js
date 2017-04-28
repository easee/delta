(function(){
    app.controller("StockLevelCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
     ListCategories();
      $scope.showProducts=false;
      $scope.showPrint = false;
      $scope.showHeader = false;
     function ListCategories(){
            DataService.list("categories", function(data){ $scope.categories = data});
         
        };      

        $scope.getProducts = function (item) {            
                DataService.read("productsbycategory",item, function (data) { $scope.products = data; })
                 $scope.showProducts=true;
                 $scope.showPrint = true;
            };
           $scope.printDiv = function (divName) {
           var printContents = document.getElementById(divName).innerHTML;
           var popupWin = window.open('', '_blank', 'width=1000,height=1000');
           $scope.showHeader = true;
           popupWin.document.open();
           popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="styles/bootstrap.min.css" /><link rel="stylesheet" type="text/css" href="styles/style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
           popupWin.document.close();
           $scope.showHeader = false;
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