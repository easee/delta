(function(){

    app.controller("CustomersCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showCustomer = false;
        ListCustomers();

        $scope.getCustomer = function(currentCustomer){
            $scope.customer = currentCustomer;
            $scope.showCustomer = true;
        };

        $scope.save = function(){
            if($scope.customer.id == 0)
                DataService.insert("customers", $scope.customer, function(data){ ListCustomers();} );
            else
                DataService.update("customers", $scope.customer.id, $scope.customer, function(data){ListCustomers();});
        };

        $scope.new = function(){
            $scope.customer = {
                id: 0,
                name: "",
                address: "",
                town: ""
            };
            $scope.showCustomer = true;
        };

        function ListCustomers(){
            DataService.list("customers", function(data){ $scope.customers = data});
        }
    }]);

}());