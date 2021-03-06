(function() {

    app.controller("SuppliersCtrl", ['$scope', 'DataService', function($scope, DataService) {
        $scope.showSuppliers = false;
        $scope.searchPage = false;
        $scope.pagination = false;
        $scope.number = false;
        $scope.onSubmit = false;
        ListSuppliers(0);
        ListTowns('');
        $scope.selectSearch = "";

        $scope.edit = function(current) {
            $scope.supplier = current;
            $scope.showSuppliers = true;
        };

        $scope.save = function() {
            if (!$scope.myForm.$valid) {
                $scope.onSubmit = true;
                // $scope.modal('show');Izbačeno jer ne radi s ovim
            }
            if ($scope.supplier.id == 0) {
                DataService.insert("suppliers", $scope.supplier, function(data) { ListSuppliers(); });
                $('.modal').modal('hide');
                $scope.onSubmit = false;
            } else {
                $scope.supplier.townId = $scope.selected.id;
                DataService.update("suppliers", $scope.supplier.id, $scope.supplier, function(data) { ListSuppliers(); });
                $('.modal').modal('hide');
                $scope.onSubmit = false;
            }
        };
        $scope.hideval = function() {
            $scope.onSubmit = false;
        }

        $scope.delete = function(supplier) {
            DataService.delete("suppliers", supplier.id, function(data) {
                swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this Supplier!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    },
                    function() {
                        ListSuppliers();
                        swal("Deleted!", "Supplier has been deleted.", "success");
                    });

            });
            $scope.showSuppliers = false;
        };
        $scope.new = function() {
            $scope.supplier = {
                id: 0,
                name: "",
                address: "",
                towns: []
                    //DODATI DIO SA TOWNid
            };
            document.getElementById('townsel').style.visibility = 'hidden'; //sakrivamo combobox na otvaranju modala
            $scope.showSuppliers = true;
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
            DataService.list("suppliers/pagination?item=" + $scope.selectSearch + "&page=" + page, function(data) {

                $scope.pagination = false;
                $scope.suppliers = data.suppliersList;
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
        function ListSuppliers(page) {
            DataService.list("suppliers?page=" + page, function(data) {
                $scope.checkFirst = 0;
                $scope.checkLast = 0;
                $scope.onSubmit = false;
                $scope.searchPage = false;
                $scope.suppliers = data.suppliersList;
                $scope.totalPages = data.totalPages;
                $scope.currentPage = data.currentPage + 1;
                 if($scope.currentPage>1)
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
                    ListSuppliers(page - 2);
                    document.getElementById(page - 1).focus();
                    if(page-1==1)
                        check==1;
                } else if (direction == 1) {
                    ListSuppliers(page);
                    document.getElementById(page + 1).focus();
                } else
                    ListSuppliers(page - 1);
            if(check==1)
                 document.getElementById(page - 1).focus();
            
            }
            /*function ListSuppliers(){
                DataService.list("suppliers", function(data){ $scope.suppliers = data});
            };*/
        function ListTowns(name) {
            DataService.list("towns/" + name, function(data) { $scope.towns = data });
        };

        //ARROW DOWN EVENT SO IT COULD FOCUS ON DROPDOWN
        $scope.textUp = function(keyEvent) {
            if (keyEvent.key == "ArrowDown") document.getElementById('townsel').focus();
        };

        $scope.townSelected = function(keyEvent) {
            if (keyEvent.key == "Enter") {
                for (var i = 0; i < $scope.towns.length; i++) {
                    if ($scope.towns[i].id === $scope.supplier.townId) { //Onaj ID koji ima istu vrijednost kao
                        $scope.supplier.town = $scope.towns[i].name;
                        document.getElementById('townsel').style.visibility = 'hidden';
                        break;
                    }
                }
            }
        };

        //AUTOCOMPLETE in BOX
        $scope.autocomplete = function(autoStr) {
            if (autoStr.length >= 3) { //reaguje samo kada ima 3 ili više slova uneseno
                ListTowns(autoStr);
                document.getElementById('townsel').style.visibility = 'visible'; //Otkrivamo combobox
                document.getElementById('townsel').size = 8;
            } else {
                document.getElementById('townsel').style.visibility = 'hidden';
            }
        };
        //typeahead --- start
        var _selected;
        $scope.selected = undefined;
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
        //typeahead --- end    
    }]);
}());