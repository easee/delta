(function() {

    app.controller("AgentsCtrl", ['$scope', 'DataService', function($scope, DataService) {
        $scope.showAgents = false;
        ListAgents();

        $scope.edit = function(currentAgent) {
            $scope.agent = currentAgent;
            $scope.showAgents = true;
        };

        $scope.save = function() {
            if ($scope.agent.id == 0)
                DataService.insert("agents", $scope.agent, function(data) { ListAgents(); });
            else
                DataService.update("agents", $scope.agent.id, $scope.agent, function(data) { ListAgents(); });
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