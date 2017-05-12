(function() {

    app.controller("AgentsCtrl", ['$scope', 'DataService', '$http', function($scope, DataService, $http) {
        $scope.showAgents = false;
        ListAgents();

        //Add or remove town for Agent
        $scope.add = function(town) {
            $scope.agent.towns.push(town);
        };

        $scope.remove = function(town, towns){
            var i = $scope.agent.towns.indexOf(town);
            $scope.agent.towns.splice(i, 1);
        };

        //Typeahead
        var _selected;
        $scope.selected = (undefined);

        $scope.selectedTown = { id: 0, name: '', zip: 0, region: '' };
        $scope.getTowns = function(name) {
            return $http.get('http://api-delta.gigischool.com/api/towns/' + name).then(function(response) {
                return response.data;
            });
        };

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
        //End of typeahead
        //End of towns

        $scope.edit = function(currentAgent) {
            $scope.agent = currentAgent;
            $scope.showAgents = true;
            console.log(currentAgent);
        };
        $scope.hideval = function() {
            $scope.onSubmit = false;
        }

        $scope.save = function() {
            if (!$scope.myForm.$valid) {
                $scope.onSubmit = true;
                $scope.modal('show');
            }
            if ($scope.agent.id == 0) {
                DataService.insert("agents", $scope.agent, function(data) { ListAgents(); });
                $('.modal').modal('hide');
            } else {
                DataService.update("agents", $scope.agent.id, $scope.agent, function(data) { ListAgents(); });
                $('.modal').modal('hide');
            }
        };

        $scope.delete = function(agent) {
            DataService.delete("agents", agent.id, function(data) {
                swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this Agent!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    },
                    function() {
                        ListAgents();
                        swal("Deleted!", "Agent has been deleted.", "success");
                    });
            });
            $scope.showAgents = false;
        };
        $scope.new = function() {
            $scope.agent = {
                id: 0,
                name: "",
                username: ""
            };
            $scope.showAgents = true;
        };

        function ListAgents() {
            DataService.list("agents", function(data) { $scope.agents = data });
        }
    }]);
}());