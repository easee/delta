(function(){
    app.controller("CategoriesCtrl", ['$scope', 'DataService', function($scope,DataService) {
      $scope.showCategories = false;
        ListCategories();
        
        $scope.edit = function(currentCategory){
            $scope.category = currentCategory;
            $scope.showCategories = true;                                        
    };
    $scope.save = function(){
            if($scope.category.id == 0)
                DataService.insert("categories", $scope.category, function(data){ ListCategories();} );
            else
                DataService.update("categories", $scope.category.id, $scope.category, function(data){ListCategories();});
        };

        //CREATE NEW CATEGORY
        $scope.new = function(){
            $scope.category = {
                id: 0,
                name: ""
            };
            $scope.showCategories = true;      

            //DataService.insert("categories", $scope.category, function(data){ ListCategories();} );
        };
        
        //DELETE CUSTOMER
        $scope.delete = function (category) {
            DataService.delete("categories", category.id, function (data) {
                ListCategories();
            });
            $scope.showCategories = false;
        };
        function ListCategories(){
            DataService.list("categories", function(data){ $scope.category = data});
        }
    }]);
}());