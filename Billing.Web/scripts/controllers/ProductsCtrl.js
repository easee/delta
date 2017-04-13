(function(){
    app.controller("ProductsCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showProduct = false;
        ListCategories('');
        ListProducts(0);
        //READ AND EDIT PRODUCT
        $scope.edit = function(currentProduct){
            $scope.product = currentProduct;
            $scope.showProduct = true;
        };

        //UPDATE PRODUCT
        $scope.save = function(){
            if($scope.product.id == 0)
                DataService.insert("products", $scope.product, function(data){ ListProducts($scope.currentPage-1);} );
            else
                DataService.update("products", $scope.product.id, $scope.product, function(data){ListProducts($scope.currentPage-1);});
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
        //PAGINATION
        function ListProducts(page){
                DataService.list("products?page="+page, function(data){
                    $scope.products= data.productsList;
                    $scope.totalPages = data.totalPages;
                    $scope.currentPage = data.currentPage+1;
                    $scope.pages = new Array($scope.totalPages);
                    for(var i=0; i<$scope.totalPages; i++) $scope.pages[i] = i+1;
                    console.log($scope.currentPage);
                });
            }
        //GO TO
         $scope.goto = function(page){
                ListProducts(page-1);
            }
        //DELETE PRODUCT
        $scope.delete = function (product) {
            DataService.delete("products", product.id, function (data) {
                ListProducts();
            });
            $scope.showProducts = false;
        }; 
        //LIST ALL PRODUCTS
//        function ListProducts(){
//            DataService.list("products", function(data){ $scope.products = data});
//        };

        //LIST/GET ALL CATEGORIES
        function ListCategories(name){
            DataService.list("categories", function(data){ $scope.categories = data});
        };
    }]);

}());