(function(){

    app.controller("AgentsCtrl", ['$scope', 'DataService', function($scope, DataService) {
        $scope.showAgents = false;
        ListAgents();

        $scope.edit = function(currentAgent){
            $scope.agent = currentAgent;
            $scope.showAgents = true;
        };

        $scope.save = function(){
            if($scope.agent.id == 0)
                DataService.insert("agents", $scope.agent, function(data){ ListAgents();} );
            else
                DataService.update("agents", $scope.agent.id, $scope.agent, function(data){ListAgents();});
        };
        
         $scope.delete = function (agent) {
            DataService.delete("agents", agent.id, function (data) {
                ListAgents();
            });
            $scope.showAgents = false;
        };
        $scope.new = function(){
            $scope.agent = {
                id: 0,
                name: "",
                username: ""
            };
            $scope.showAgents = true;
        };

        function ListAgents(){
            DataService.list("agents", function(data){ $scope.agents = data});
        }
    }]);
}());