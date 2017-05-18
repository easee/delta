(function() {
    app.controller("ProcurementsCtrl", ['$scope', 'DataService', '$http', function($scope, DataService, $http) {
        $scope.showProcurements = false;
        $scope.searchPage = false;
        $scope.pagination = false;
        $scope.number = false;
        $scope.onSubmit = false;
        ListProcurements(0);
        $scope.selectSearch = "";
        $scope.selectedProduct = { id: 0, name: ''};
        $scope.selectedSupplier = { id: 0, name: ''};

        $scope.edit = function(currentProcurement) {
            $scope.procurement = currentProcurement;
            console.log(currentProcurement);
            $scope.showProcurements = true;
        };
        $scope.hideval = function() {
            $scope.onSubmit = false;
        };
        $scope.save = function() {
            if (!$scope.myForm.$valid) {
                $scope.onSubmit = true;
                // $scope.modal('show');Izbačeno jer ne radi s ovim
            }
        $scope.procurement.supplierId = $scope.selectedSupplier.id;
        $scope.procurement.productId = $scope.selectedProduct.id;
            if ($scope.procurement.id == 0) {
                DataService.insert("procurements", $scope.procurement, function(data) { ListProcurements(); });
                $('.modal').modal('hide');
            } else {
                DataService.update("procurements", $scope.procurement.id, $scope.procurement, function(data) { ListProcurements(); });
                $('.modal').modal('hide');
            }
        };


        //CREATE NEW PROCUREMENT
        $scope.new = function() {
        $scope.selectedProduct = { id: 0, name: ''};
        $scope.selectedSupplier = { id: 0, name: ''};
            $scope.procurement = {
                id: 0,
                date: new Date(),
                document: "",
                quantity: null,
                price: null,
                suppliers: [],
                products: []
            };
            $scope.showProcurements = true;

            //DataService.insert("procurements", $scope.procurement, function(data){ ListProcurements();} );
        };
       
        $scope.search2 = function(page, direction) {
             if(direction==1)
                 {
                      $scope.search(page, direction);
                      var i=(page+1)+"Search";
                      document.getElementById(i).focus();
                 }
             else if(direction==-1)
                 {
                      $scope.search(page, direction);
                      var i=(page+1)+"Search";
                      document.getElementById(i).focus();
                 }
             else
                 $scope.search(page, direction);
                
         }
        $scope.page = 0;
        $scope.search = function(page, direction) {
            DataService.list("procurements/pagination?item=" + $scope.selectSearch + "&page=" + page, function(data) {

                $scope.pagination = false;
                $scope.procurements = data.procurementsList;
                $scope.totalPages = data.totalPages;
                $scope.currentPage = data.currentPage + 1;
                $scope.checkPage2 = 1;
                $scope.checkFirst2 = 0;
                $scope.checkLast2 = 0;

                if($scope.currentPage>1)
                     $scope.checkPage2 = 0;
                if ($scope.totalPages < 11)
                    $scope.pages = new Array($scope.totalPages);
                else
                    $scope.pages = new Array(10);
                $scope.size = data.size;
                if ($scope.currentPage == $scope.totalPages && $scope.totalPages > 1) {
                    document.getElementById("nextSearch").disabled = true;
                    document.getElementById("previousSearch").disabled = false;
                    $scope.checkLast2 = 1;
                } else if ($scope.currentPage == 1 && $scope.totalPages == 1) {
                    document.getElementById("previousSearch").disabled = true;
                    document.getElementById("nextSearch").disabled = true;
                    $scope.checkFirst2 = 1;
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
                if ($scope.totalPages > 0) {
                    $scope.searchPage = true;
                    $scope.number = true;
                } else {
                    $scope.number = false;
                    $scope.searchPage = false;
                }

            });
        };
        $scope.checkPage = 1;
        $scope.checkFirst = 0;
        $scope.checkLast = 0;
        $scope.total = 0;
        //PAGINATION
        function ListProcurements(page) {
            DataService.list("procurements?page=" + page, function(data) {
                $scope.checkFirst = 0;
                $scope.checkLast = 0;
                $scope.searchPage = false;
                $scope.procurements = data.procurementsList;
                $scope.totalPages = data.totalPages;
                $scope.currentPage = data.currentPage + 1;
                if($scope.currentPage>1 )
                     $scope.checkPage = 0;             

                if ($scope.totalPages < 11)
                    $scope.pages = new Array($scope.totalPages);
                else
                    $scope.pages = new Array(10);

                $scope.size = data.size;


                if ($scope.currentPage == $scope.totalPages) {
                    document.getElementById("next").disabled = true;
                    document.getElementById("previous").disabled = false;
                     $scope.checkLast = 1;
                } else if ($scope.currentPage == 1) {
                    document.getElementById("previous").disabled = true;
                    document.getElementById("next").disabled = false;
                     $scope.checkFirst = 1;
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

                    else if ($scope.currentPage + 5 >= $scope.totalPages) {
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
                if ($scope.totalPages > 0) {
                    $scope.pagination = true;
                    $scope.number = true;
                } else {
                    $scope.number = false;
                    $scope.pagination = false;
                }
            });
        }
           //GO TO
        $scope.goto = function(page, direction) {
            
            var check=0;
                if (direction == -1) {
                    ListProcurements(page - 2);
                    document.getElementById(page - 1).focus();
                    if(page-1==1)
                        check==1;
                } else if (direction == 1) {
                    ListProcurements(page);
                    document.getElementById(page + 1).focus();
                } else
                    ListProcurements(page - 1);
            
            if(check==1)
                 document.getElementById(page - 1).focus();
            
            }

        //DELETE PROCUREMENTS
        $scope.delete = function(procurement) {
            DataService.delete("procurements", procurement.id, function(data) {
                swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this Procurement!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    },
                    function() {
                        ListProcurements();
                        swal("Deleted!", "Procurement has been deleted.", "success");
                    });
            });
            $scope.showProcurements = false;
        };
        /*function ListProcurements(){
            DataService.list("procurements", function(data){ $scope.procurements = data});
        }*/

        function ListSuppliers(name) {
            DataService.list("suppliers/" + name, function(data) { $scope.suppliers = data });
        };

        function ListProducts(name) {
            DataService.list("products/" + name, function(data) { $scope.products = data });
        };
        
        //TYPEAHEAD Suppliers and Products
        var _selected;
        $scope.selected = (undefined);

        $scope.getSuppliers = function(name) {
            return $http.get('http://api-delta.gigischool.com/api/suppliers/' + name).then(function(response) {
                return response.data;
            });
        };

        $scope.getProducts = function(name) {
            return $http.get('http://api-delta.gigischool.com/api/products/' + name).then(function(response) {
                return response.data;
            });
        };


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