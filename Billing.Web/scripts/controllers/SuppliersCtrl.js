(function(){

    app.controller("SuppliersCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showSuppliers = false;
        ListSuppliers();

        $scope.edit = function(current){
            $scope.supplier = current;
            $scope.showSuppliers = true;
        };

        $scope.save = function(){
            if($scope.supplier.id == 0)
                DataService.insert("suppliers", $scope.supplier, function(data){ ListSuppliers();} );
            else
                DataService.update("suppliers", $scope.supplier.id, $scope.supplier, function(data){ListSuppliers();});
        };
        
        $scope.delete = function (supplier) {
            DataService.delete("suppliers", supplier.id, function (data) {
                ListSuppliers();
            });
            $scope.showSuppliers = false;
        };
        
        $scope.new = function(){
            $scope.supplier = {
                id: 0,
                name: "",
                address: "",
                town: ""
            };
            $scope.showSuppliers = true;
        };

        function ListSuppliers(){
            DataService.list("suppliers", function(data){ $scope.suppliers = data});
        }
    }]);
}());