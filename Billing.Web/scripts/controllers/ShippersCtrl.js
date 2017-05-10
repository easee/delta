(function() {
    app.controller("ShippersCtrl", ['$scope', 'DataService', function($scope, DataService) {
        $scope.showShipper = false;
        $scope.onSubmit = false;
        ListShippers();
        ListTowns('');

        //READ AND EDIT SHIPPERS
        $scope.edit = function(currentShipper) {
            $scope.shipper = currentShipper;
            $scope.showShipper = true;
        };

        //UPDATE SHIPPER
        $scope.save = function() {
            if (!$scope.myForm.$valid) {
                $scope.onSubmit = true;
                $('.modal').modal('hide');
            }

            if ($scope.shipper.id == 0) {
                DataService.insert("shippers", $scope.shipper, function(data) { ListShippers(); });
                $('.modal').modal('hide');
            } else {
                DataService.update("shippers", $scope.shipper.id, $scope.shipper, function(data) { ListShippers(); });
                $('.modal').modal('hide');
            }
        };
        $scope.hideval = function() {
            $scope.onSubmit = false;
        };
        //CREATE NEW SHIPPER
        $scope.new = function() {
            $scope.shipper = {
                id: 0,
                name: "",
                address: "",
                towns: []
            };
            document.getElementById('townsel').style.visibility = 'hidden'; //sakrivamo combobox na otvaranju modala
            $scope.showShippers = true;
        };

        //DELETE SHIPPER
        $scope.delete = function(shipper) {
            DataService.delete("shippers", shipper.id, function(data) {
                swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this Shipper!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    },
                    function() {
                        ListShippers();
                        swal("Deleted!", "Shipper has been deleted.", "success");
                    });
            });
            $scope.showShippers = false;
        };
        //LIST ALL SHIPPERS
        function ListShippers() {
            $scope.onSubmit = false;
            DataService.list("shippers", function(data) { $scope.shippers = data });
        };

        //LIST ALL TOWNS
        function ListTowns(name) {
            $scope.onSubmit = false;
            DataService.list("towns/" + name, function(data) { $scope.towns = data });
        };
        //ARROW DOWN EVENT SO IT COULD FOCUS ON DROPDOWN
        $scope.textUp = function(keyEvent) {
            if (keyEvent.key == "ArrowDown") document.getElementById('townsel').focus();
        };

        $scope.townSelected = function(keyEvent) {
            if (keyEvent.key == "Enter") {
                for (var i = 0; i < $scope.towns.length; i++) {
                    if ($scope.towns[i].id === $scope.shipper.townId) { //Onaj ID koji ima istu vrijednost kao
                        $scope.shipper.town = $scope.towns[i].name;
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