(function(){
    app.controller("ShippersCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showShipper = false;
        ListShippers();
        ListTowns('');

        //READ AND EDIT SHIPPERS
        $scope.edit = function(currentShipper){
            $scope.shipper = currentShipper;
            $scope.showShipper = true;
        };

        //UPDATE SHIPPER
        $scope.save = function(){
            if($scope.shipper.id == 0)
                DataService.insert("shippers", $scope.shipper, function(data){ ListShippers();} );
            else
                DataService.update("shippers", $scope.shipper.id, $scope.shipper, function(data){ListShippers();});
        };

        //CREATE NEW SHIPPER
        $scope.new = function(){
            $scope.shipper = {
                id: 0,
                name: "",
                address: "",
                towns: []
            };
            $scope.showShippers = true;
        };
        
        //DELETE SHIPPER
        $scope.delete = function (shipper) {
            DataService.delete("shippers",shipper.id, function (data) {
                ListShippers();
            });
            $scope.showShippers= false;
        }; 
        //LIST ALL SHIPPERS
        function ListShippers(){
            DataService.list("shippers", function(data){ $scope.shippers = data});
        };

        //LIST ALL TOWNS
        function ListTowns(name){
            DataService.list("towns", function(data){ $scope.towns = data});
        };
    }]);

}());