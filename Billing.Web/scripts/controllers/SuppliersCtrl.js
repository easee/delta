(function(){

    app.controller("SuppliersCtrl", ['$scope', 'DataService',  function($scope, DataService) {
        $scope.showSuppliers = false;
        ListSuppliers();
        ListTowns('');

        $scope.edit = function(current){
            $scope.supplier = current;
            $scope.showSuppliers = true;
        };

        $scope.save = function(){
            if($scope.supplier.id == 0)
                DataService.insert("suppliers", $scope.supplier, function(data){ ListSuppliers();} );
            else{
                $scope.supplier.townId = $scope.selected.id;
                DataService.update("suppliers", $scope.supplier.id, $scope.supplier, function(data){ListSuppliers();});
        }
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
                towns: []
//DODATI DIO SA TOWNid
            };
            document.getElementById('townsel').style.visibility = 'hidden';//sakrivamo combobox na otvaranju modala
            $scope.showSuppliers = true;
        };

        function ListSuppliers(){
            DataService.list("suppliers", function(data){ $scope.suppliers = data});
        };
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
                        if($scope.towns[i].id === $scope.supplier.townId){ //Onaj ID koji ima istu vrijednost kao
                            $scope.supplier.town = $scope.towns[i].name;
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