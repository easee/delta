(function(){
    app.controller("CustomersCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showCustomer = false;
        //ListTowns('');
        ListCustomers();

        //READ AND EDIT CUSTOMERS
        $scope.edit = function(currentCustomer){
            $scope.customer = currentCustomer;
            $scope.showCustomer = true;
        };

        //UPDATE CUSTOMER
        $scope.save = function(){
            if($scope.customer.id == 0)
                DataService.insert("customers", $scope.customer, function(data){ ListCustomers();} );
            else
                DataService.update("customers", $scope.customer.id, $scope.customer, function(data){ListCustomers();});
        };

        //CREATE NEW CUSTOMER
        $scope.new = function(){
            $scope.customer = {
                id: 0,
                name: "",
                address: "",
                towns: []
            };
            document.getElementById('townsel').style.visibility = 'hidden';//sakrivamo combobox na otvaranju modala
            $scope.showCustomers = true;
        };
        
        //DELETE CUSTOMER
        $scope.delete = function (customer) {
            DataService.delete("customers", customer.id, function (data) {
                ListCustomers();
            });
            $scope.showCustomers = false;
        }; 
        //LIST ALL CUSTOMERS
        function ListCustomers(){
            DataService.list("customers", function(data){ $scope.customers = data});
        };

        //LIST/GET ALL TOWNS
        function ListTowns(name){
            DataService.list("towns/" + name, function(data){ $scope.towns = data});
        };

        //ARROW DOWN EVENT SO IT COULD FOCUS ON DROPDOWN
        $scope.textUp = function(keyEvent){
                if(keyEvent.key == "ArrowDown") document.getElementById('townsel').focus();
            };

        $scope.townSelected = function(keyEvent){
                if(keyEvent.key == "Enter") {
                    for(var i=0; i<$scope.towns.length; i++){
                        if($scope.towns[i].id === $scope.customer.townId){ //Onaj ID koji ima istu vrijednost kao
                            $scope.customer.town = $scope.towns[i].name;
                            document.getElementById('townsel').style.visibility = 'hidden';
                            break;
                        }
                    }
                }
            };

        //AUTOCOMPLETE in BOX
        $scope.autocomplete = function(autoStr){
                if (autoStr.length >= 3){ //reaguje samo kada ima 3 ili više slova uneseno
                    ListTowns(autoStr);
                    document.getElementById('townsel').style.visibility = 'visible';//Otkrivamo combobox
                    document.getElementById('townsel').size = 8;
                }
                else {
                    document.getElementById('townsel').style.visibility = 'hidden';
                }
            };
    }]);

}());