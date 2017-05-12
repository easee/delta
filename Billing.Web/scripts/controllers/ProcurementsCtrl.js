(function() {
    app.controller("ProcurementsCtrl", ['$scope', 'DataService', function($scope, DataService) {
        $scope.showProcurements = false;
        $scope.searchPage = false;
        $scope.pagination = false;
        $scope.number = false;
        $scope.onSubmit = false;
        ListProcurements(0);
        ListSuppliers('');
        ListProducts('');
        $scope.selectSearch = "";

        $scope.edit = function(currentProcurement) {
            $scope.procurement = currentProcurement;
            $scope.showProcurements = true;
        };
        $scope.hideval = function() {
            $scope.onSubmit = false;
        };
        $scope.save = function() {
            if (!$scope.myForm.$valid) {
                $scope.onSubmit = true;
                $scope.modal('show');
            }
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
            $scope.procurement = {
                id: 0,
                date: new Date(),
                document: "",
                quantity: null,
                price: null,
                suppliers: [],
                products: []
            };
            document.getElementById('suppliersel').style.visibility = 'hidden';
            document.getElementById('productsel').style.visibility = 'hidden';
            $scope.showProcurements = true;

            //DataService.insert("procurements", $scope.procurement, function(data){ ListProcurements();} );
        };

        $scope.page = 0;
        $scope.search = function(page = 0, direction = 0) {
            DataService.list("procurements/pagination?item=" + $scope.selectSearch + "&page=" + page, function(data) {

                $scope.pagination = false;
                $scope.procurements = data.procurementsList;
                $scope.totalPages = data.totalPages;
                $scope.currentPage = data.currentPage + 1;

                if ($scope.totalPages < 11)
                    $scope.pages = new Array($scope.totalPages);
                else
                    $scope.pages = new Array(10);
                $scope.size = data.size;
                if ($scope.currentPage == $scope.totalPages && $scope.totalPages > 1) {
                    document.getElementById("nextSearch").disabled = true;
                    document.getElementById("previousSearch").disabled = false;
                } else if ($scope.currentPage == 1 && $scope.totalPages == 1) {
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
                if ($scope.totalPages > 0) {
                    $scope.searchPage = true;
                    $scope.number = true;
                } else {
                    $scope.number = false;
                    $scope.searchPage = false;
                }

                console.log($scope.currentPage);
            });
        };
        //PAGINATION
        function ListProcurements(page) {
            DataService.list("procurements?page=" + page, function(data) {

                $scope.searchPage = false;
                $scope.procurements = data.procurementsList;
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
                console.log($scope.currentPage);
            });
        }
        //GO TO
        $scope.goto = function(page, direction) {
            if (direction == -1) {
                ListProcurements(page - 2);
                document.getElementById(page - 1).focus();
            } else if (direction == 1) {
                ListProcurements(page);
                document.getElementById(page + 1).focus();
            } else
                ListProcurements(page - 1);
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
        //ARROW DOWN EVENT SO IT COULD FOCUS ON DROPDOWN
        $scope.textUp = function(keyEvent) {
            if (keyEvent.key == "ArrowDown") document.getElementById('suppliersel').focus();
        };

        $scope.textUp2 = function(keyEvent) {
            if (keyEvent.key == "ArrowDown") document.getElementById('productsel').focus();
        };

        $scope.supplierSelected = function(keyEvent) {
            if (keyEvent.key == "Enter") {
                for (var i = 0; i < $scope.suppliers.length; i++) {
                    if ($scope.suppliers[i].id === $scope.procurement.supplierId) { //Onaj ID koji ima istu vrijednost kao
                        $scope.procurement.supplier = $scope.suppliers[i].name;
                        document.getElementById('suppliersel').style.visibility = 'hidden';
                        break;
                    }
                }
            }
        };

        $scope.productSelected = function(keyEvent) {
            if (keyEvent.key == "Enter") {
                for (var i = 0; i < $scope.products.length; i++) {
                    if ($scope.products[i].id === $scope.procurement.productId) { //Onaj ID koji ima istu vrijednost kao
                        $scope.procurement.product = $scope.products[i].name;
                        document.getElementById('productsel').style.visibility = 'hidden';
                        break;
                    }
                }
            }
        };

        //AUTOCOMPLETE in BOX
        $scope.autocomplete = function(autoStr) {
            if (autoStr.length >= 3) {
                ListSuppliers(autoStr);
                document.getElementById('suppliersel').style.visibility = 'visible'; //Otkrivamo combobox
                document.getElementById('suppliersel').size = 8;
            } else {
                document.getElementById('suppliersel').style.visibility = 'hidden';
            }
        };

        $scope.autocomplete2 = function(autoStr) {
            if (autoStr.length >= 3) {
                ListProducts(autoStr);
                document.getElementById('productsel').style.visibility = 'visible'; //Otkrivamo combobox
                document.getElementById('productsel').size = 8;
            } else {
                document.getElementById('productsel').style.visibility = 'hidden';
            }
        };

    }]);
}());