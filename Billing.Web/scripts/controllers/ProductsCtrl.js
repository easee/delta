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
                  
                    if($scope.totalPages<11)
                        {
                            document.getElementById("next").style.visibility = 'hidden';
                            document.getElementById("previous").style.visibility = 'hidden';
                            document.getElementById("last").style.visibility = 'hidden';
                            document.getElementById("first").style.visibility = 'hidden';
                        }
                    else
                    {
                            document.getElementById("next").style.visibility = 'visible';
                            document.getElementById("previous").style.visibility = 'visible';
                            document.getElementById("last").style.visibility = 'visible';
                            document.getElementById("first").style.visibility = 'visible';
                    }
                    $scope.products= data.productsList;
                    $scope.totalPages = data.totalPages;
                    $scope.currentPage = data.currentPage+1;
                    $scope.pages = new Array(10);
                    $scope.size=data.size;
                        
                    
                    if($scope.currentPage== $scope.totalPages)
                        {
                            document.getElementById("next").disabled = true;
                            document.getElementById("previous").disabled = false;
                            
                        }
                    else if($scope.currentPage== 1)
                        {
                            document.getElementById("previous").disabled = true;
                            document.getElementById("next").disabled = false;
                        }
                    
                    else
                        {
                            document.getElementById("next").disabled = false;
                            document.getElementById("previous").disabled = false;
                        }
                    if($scope.currentPage+9<$scope.totalPages)
                    for(var i=$scope.currentPage; i<=$scope.currentPage+9;i++) 
                        $scope.pages[i-$scope.currentPage] = i;
                                        
                    else
                            var d=9-($scope.totalPages-$scope.currentPage)
                            for(var i=$scope.currentPage-d; i<=$scope.currentPage+9-d;i++) 
                                $scope.pages[i-$scope.currentPage+d] = i;
                                                   
                    console.log($scope.currentPage);
                });
            }
        //GO TO
         $scope.goto = function(page,direction){
                if(direction==-1)
                    {
                     ListProducts(page-2); 
                     document.getElementById(page-1).focus();
                    }
             else if(direction==1)
                 {
                      ListProducts(page);
                      document.getElementById(page+1).focus();
                 }
                
             else       
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