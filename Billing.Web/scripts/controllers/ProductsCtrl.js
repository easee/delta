(function(){
    app.controller("ProductsCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showProduct = false;
        ListCategories('');
        ListProducts();

        //READ AND EDIT PRODUCT
        $scope.edit = function(currentProduct){
            $scope.product = currentProduct;
            $scope.showProduct = true;
        };

        //UPDATE PRODUCT
        $scope.save = function(){
            if($scope.product.id == 0)
                DataService.insert("products", $scope.product, function(data){ ListProducts();} );
            else
                DataService.update("products", $scope.product.id, $scope.product, function(data){ListProducts();});
        };

        //CREATE NEW PRODUCT
        $scope.new = function(){
            $scope.product = {
                id: 0,
                name: "",
                address: "",
                category: []
            };
            $scope.showProducts = true;
        };
        
        //DELETE PRODUCT
        $scope.delete = function (product) {
            DataService.delete("products", product.id, function (data) {
                ListProducts();
            });
            $scope.showProducts = false;
        }; 
        //LIST ALL PRODUCTS
        function ListProducts(){
            DataService.list("products", function(data){ $scope.products = data});
        };

        //LIST/GET ALL CATEGORIES
        function ListCategories(name){
            DataService.list("categories", function(data){ $scope.categories = data});
        };
    }]);

}());