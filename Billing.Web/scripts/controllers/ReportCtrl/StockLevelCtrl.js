(function(){
    app.controller("StockLevelCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
     ListCategories();
      $scope.showProducts=false;
     function ListCategories(){
            DataService.list("categories", function(data){ $scope.categories = data});
            
        };      

        $scope.getProducts = function (item) {            
                DataService.read("productsbycategory",item, function (data) { $scope.products = data; })
                 $scope.showProducts=true;
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