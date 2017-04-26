(function(){
    application.controller("StockLevelCtrl",['$scope','$rootScope','$anchorScroll','DataService',function($scope,$rootScope,$anchorScroll,DataService){
       
        
        function ListAll(){
            DataService.insert("productsbycategory",$scope.request,function(data){
               $scope.Inventory=data; 
            });
        };
        
        function ListCategories(){
            DataService.list("categories",function(data){
              $scope.categories=data;
          });  
        };
        
        
    }]);
}());