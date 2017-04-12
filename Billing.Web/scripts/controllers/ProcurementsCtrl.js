(function(){
    app.controller("ProcurementsCtrl", ['$scope', 'DataService', function($scope,DataService) {
      $scope.showProcurements = false;
        ListProcurements();
        ListSuppliers('');
        
        $scope.edit = function(currentProcurement){
            $scope.procurement = currentProcurement;
            $scope.showProcurements = true;                                        
    };
    $scope.save = function(){
            if($scope.procurement.id == 0)
                DataService.insert("procurements", $scope.procurement, function(data){ ListProcurements();} );
            else
                DataService.update("procurements", $scope.procurement.id, $scope.procurement, function(data){ListProcurements();});
        };

        //CREATE NEW PROCUREMENT
        $scope.new = function(){
            $scope.procurement = {
                id: 0,
                date: new Date,
                document: "",
                quantity: null,
                price: null,
                suppliers: [],
                products: ""            
            };
            document.getElementById('suppliersel').style.visibility = 'hidden';
            $scope.showProcurements = true;      

            //DataService.insert("procurements", $scope.procurement, function(data){ ListProcurements();} );
        };
        
        //DELETE PROCUREMENTS
        $scope.delete = function (procurement) {
            DataService.delete("procurements", procurement.id, function (data) {
                ListProcurements();
            });
            $scope.showProcurements = false;
        };
        function ListProcurements(){
            DataService.list("procurements", function(data){ $scope.procurements = data});
        }
        
        function ListSuppliers(name){
            DataService.list("suppliers/" + name, function(data){ $scope.suppliers = data});
        };
             //ARROW DOWN EVENT SO IT COULD FOCUS ON DROPDOWN
        $scope.textUp = function(keyEvent){
                if(keyEvent.key == "ArrowDown") document.getElementById('suppliersel').focus();
            };

        $scope.supplierSelected = function(keyEvent){
                if(keyEvent.key == "Enter") {
                    for(var i=0; i<$scope.suppliers.length; i++){
                        if($scope.suppliers[i].id === $scope.procurement.supplierId){ //Onaj ID koji ima istu vrijednost kao
                            $scope.procurement.supplier = $scope.suppliers[i].name;
                            document.getElementById('suppliersel').style.visibility = 'hidden';
                            break;
                        }
                    }
                }
            };

        //AUTOCOMPLETE in BOX
        $scope.autocomplete = function(autoStr){
                if (autoStr.length >= 3){ 
                    ListSuppliers(autoStr);
                    document.getElementById('suppliersel').style.visibility = 'visible';//Otkrivamo combobox
                    document.getElementById('suppliersel').size = 8;
                }
                else {
                    document.getElementById('suppliersel').style.visibility = 'hidden';
                }
            };      
       
    }]);
}());