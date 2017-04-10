(function(){
    app.controller("CustomersCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showCustomer = false;
        ListCustomers();
        ListTowns('');

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
            $scope.showCustomers = true;

        
            
            //$scope.showCustomer = true;
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

        //LIST ALL TOWNS
        function ListTowns(name){
            DataService.list("towns", function(data){ $scope.towns = data});
        };
    }]);

}());