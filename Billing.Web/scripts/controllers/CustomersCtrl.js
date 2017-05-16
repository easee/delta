(function() {
    app.controller("CustomersCtrl", ['$scope', 'DataService', function($scope, DataService) {
        $scope.showCustomer = false;
        $scope.searchPage = false;
        $scope.pagination = false;
        $scope.number = false;
        $scope.onSubmit = false;
        ListCustomers(0);
        ListTowns('');
        $scope.selectSearch = "";


        //READ AND EDIT CUSTOMERS
        $scope.edit = function(currentCustomer) {
            $scope.customer = currentCustomer;
            $scope.showCustomer = true;
        };

        $scope.hideval = function() {
            $scope.onSubmit = false;
        }

        //UPDATE CUSTOMER
        $scope.save = function() {
            if (!$scope.myForm.$valid) {
                $scope.onSubmit = true;
                // $scope.modal('show'); Izbačeno jer pravi problem i ne radi save
            }
            if ($scope.customer.id == 0) {
                DataService.insert("customers", $scope.customer, function(data) { ListCustomers(); });
                $('.modal').modal('hide');
            } else {
                DataService.update("customers", $scope.customer.id, $scope.customer, function(data) { ListCustomers(); });
                $('.modal').modal('hide');
            }
        };

        //CREATE NEW CUSTOMER
        $scope.new = function() {
            $scope.customer = {
                id: 0,
                name: "",
                address: "",
                towns: []
            };
            document.getElementById('townsel').style.visibility = 'hidden'; //sakrivamo combobox na otvaranju modala
            $scope.showCustomers = true;
        };
        $scope.page = 0;
        $scope.search = function(page, direction) {
            DataService.list("customers/pagination?item=" + $scope.selectSearch + "&page=" + page, function(data) {
                
                $scope.pagination = false;
                $scope.customers = data.customersList;
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

            });
        };
        $scope.checkPage = 1;
        $scope.checkFirst = 0;
        $scope.checkLast = 0;
        $scope.total = 0;
        //PAGINATION
        function ListCustomers(page) {
            DataService.list("customers?page=" + page, function(data) {
                $scope.checkFirst = 0;
                $scope.checkLast = 0;
                $scope.onSubmit = false;
                $scope.searchPage = false;
                $scope.customers = data.customersList;
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
                    ListCustomers(page - 2);
                    document.getElementById(page - 1).focus();
                    if(page-1==1)
                        check==1;
                } else if (direction == 1) {
                    ListCustomers(page);
                    document.getElementById(page + 1).focus();
                } else
                    ListCustomers(page - 1);
            
                if(page!=0 && checkPage==1)
                    {
                      document.getElementById(1).style.color="#333";
                      document.getElementById(1).style.backgroundColor="#fff";
                      document.getElementById(1).style.borderColor="#ccc";
                      document.getElementById(1).style.boxShadow="none";
                    }
            
            if(check==1)
                 document.getElementById(page - 1).focus();
            
            }

        //DELETE CUSTOMER
        $scope.delete = function(customer) {
            DataService.delete("customers", customer.id, function(data) {
                swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this Customer!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    },
                    function() {
                        ListCustomers();
                        swal("Deleted!", "Customer has been deleted.", "success");
                    });
            });
            $scope.showCustomers = false;
        };
        //LIST ALL CUSTOMERS
        /*function ListCustomers(){
            DataService.list("customers", function(data){ $scope.customers = data});
        };*/

        //LIST/GET ALL TOWNS
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
                    if ($scope.towns[i].id === $scope.customer.townId) { //Onaj ID koji ima istu vrijednost kao
                        $scope.customer.town = $scope.towns[i].name;
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
    }]);

}());