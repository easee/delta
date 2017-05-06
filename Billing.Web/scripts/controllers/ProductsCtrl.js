(function() {
    app.controller("ProductsCtrl", ['$scope', 'DataService', function($scope, DataService) {
        $scope.showProduct = false;
        $scope.searchPage = false;
        $scope.pagination = false;
        $scope.number = false;
        ListCategories('');
        ListProducts(0);
        $scope.selectSearch = "";
        //READ AND EDIT PRODUCT
        $scope.edit = function(currentProduct) {
            $scope.product = currentProduct;
            $scope.showProduct = true;
        };

        //UPDATE PRODUCT
        $scope.save = function() {
            if ($scope.product.id == 0)
                DataService.insert("products", $scope.product, function(data) { ListProducts($scope.currentPage - 1); });
            else
                DataService.update("products", $scope.product.id, $scope.product, function(data) { ListProducts($scope.currentPage - 1); });
        };

        //CREATE NEW PRODUCT
        $scope.new = function() {
            $scope.product = {
                id: 0,
                name: "",
                address: "",
                category: []
            };
            $scope.showProducts = true;
        };
          $scope.page=0;
        $scope.search = function(page=0,direction=0) {
            DataService.list("products/pagination?item="+$scope.selectSearch+"&page=" +page, function(data) {
                        
                $scope.pagination = false;
                $scope.products = data.productsList;
                $scope.totalPages = data.totalPages;
                $scope.currentPage = data.currentPage + 1;
                
                if ($scope.totalPages < 11)
                    $scope.pages = new Array($scope.totalPages);
                else
                    $scope.pages = new Array(10);
                $scope.size = data.size;    
                if ($scope.currentPage == $scope.totalPages && $scope.totalPages>1) {
                    document.getElementById("nextSearch").disabled = true;
                    document.getElementById("previousSearch").disabled = false;
                } else if ($scope.currentPage == 1 && $scope.totalPages==1) {
                    document.getElementById("previousSearch").disabled = true;
                    document.getElementById("nextSearch").disabled = true;
                } else if ($scope.currentPage == 1) {
                    document.getElementById("previousSearch").disabled = true;
                    document.getElementById("nextSearch").disabled = false;
                } else {
                    document.getElementById("nextSearch").disabled = false;
                    document.getElementById("previousSearch").disabled = false;
                }
                if ($scope.totalPages < 11)
                    for (var i = 0; i < $scope.totalPages; i++)
                        $scope.pages[i] = i + 1;

                else {

                    if ($scope.currentPage <= 5)
                        for (var i = 0; i <= 9; i++)
                            $scope.pages[i] = i + 1;

                    else if ($scope.currentPage + 4 >= $scope.totalPages) {
                        var d = 9 - ($scope.totalPages - $scope.currentPage);
                        for (var i = $scope.currentPage - d; i <= $scope.currentPage + 9 - d; i++)
                            $scope.pages[i - $scope.currentPage + d] = i;
                    } else {
                        var d = 0,
                            tmp = 4;
                        for (var i = $scope.currentPage; i <= $scope.currentPage + 9; i++) {
                            $scope.pages[d] = $scope.currentPage - tmp + d;
                            d++;
                        }
                    }
                }
               if($scope.totalPages>0){
                $scope.searchPage = true;
                $scope.number = true;
                }
                else{
                $scope.number = false;
                $scope.searchPage = false;
                }
                
                console.log($scope.currentPage);
            });
        };
        //PAGINATION
        function ListProducts(page) {
            DataService.list("products?page=" + page, function(data) {
                
                $scope.searchPage = false;
                $scope.products = data.productsList;
                $scope.totalPages = data.totalPages;
                $scope.currentPage = data.currentPage + 1;
                if ($scope.totalPages < 11)
                    $scope.pages = new Array($scope.totalPages);
                else
                    $scope.pages = new Array(10);

                $scope.size = data.size;


                if ($scope.currentPage == $scope.totalPages) {
                    document.getElementById("next").disabled = true;
                    document.getElementById("previous").disabled = false;
                } else if ($scope.currentPage == 1) {
                    document.getElementById("previous").disabled = true;
                    document.getElementById("next").disabled = false;
                } else {
                    document.getElementById("next").disabled = false;
                    document.getElementById("previous").disabled = false;
                }
                if ($scope.totalPages < 11)
                    for (var i = 0; i < $scope.totalPages; i++)
                        $scope.pages[i] = i + 1;

                else {

                    if ($scope.currentPage <= 5)
                        for (var i = 0; i <= 9; i++)
                            $scope.pages[i] = i + 1;

                    else if ($scope.currentPage + 4 >= $scope.totalPages) {
                        var d = 9 - ($scope.totalPages - $scope.currentPage);
                        for (var i = $scope.currentPage - d; i <= $scope.currentPage + 9 - d; i++)
                            $scope.pages[i - $scope.currentPage + d] = i;
                    } else {
                        var d = 0,
                            tmp = 4;
                        for (var i = $scope.currentPage; i <= $scope.currentPage + 9; i++) {
                            $scope.pages[d] = $scope.currentPage - tmp + d;
                            d++;
                        }
                    }
                }
                 if($scope.totalPages>0){
                $scope.pagination = true;
                $scope.number = true;
                }
                else{
                $scope.number = false;
                $scope.pagination = false;
                }
                console.log($scope.currentPage);
            });
        }
        //GO TO
        $scope.goto = function(page, direction) {
                if (direction == -1) {
                    ListProducts(page - 2);
                    document.getElementById(page - 1).focus();
                } else if (direction == 1) {
                    ListProducts(page);
                    document.getElementById(page + 1).focus();
                } else
                    ListProducts(page - 1);
            }
            //DELETE PRODUCT
        $scope.delete = function(product) {
            DataService.delete("products", product.id, function(data) {
                swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this Product!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    },
                    function() {
                        ListProducts();
                        swal("Deleted!", "Product has been deleted.", "success");
                    });
            });
            $scope.showProducts = false;
        };
        //LIST ALL PRODUCTS
        //        function ListProducts(){
        //            DataService.list("products", function(data){ $scope.products = data});
        //        };

        //LIST/GET ALL CATEGORIES
        function ListCategories(name) {
            DataService.list("categories", function(data) { $scope.categories = data });

        };
    }]);

}());