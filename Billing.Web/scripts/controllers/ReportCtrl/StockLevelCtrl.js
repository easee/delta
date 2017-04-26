(function(){
    app.controller("StockLevelCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
     ListCategories();
     function ListCategories(){
            DataService.list("categories", function(data){ $scope.categories = data});
            
        };      
        
    }]);
}());