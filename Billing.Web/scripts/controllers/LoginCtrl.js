(function() {

    var app = angular.module("Billing");

    var LoginCtrl = function($scope, $rootScope, $http, $location, LoginService) {
        
        $http.get("config.json").then(function(response){
            BillingConfig = response.data;
            $scope.debug = BillingConfig.debugMode;
        });

        $scope.login = function() {
            $http.defaults.headers.common.Authorization = "Basic " + LoginService.encode($scope.user.name + ":" + $scope.user.pass);
            var promise = $http({
                method: "post",
                url: BillingConfig.source + "login",
                data: {
                    "apiKey": BillingConfig.apiKey,
                    "signature": BillingConfig.signature
                }});
            promise.then(
                function(response) {
                    credentials = response.data,
                    console.log(credentials),
                    $rootScope.currentUser = credentials.currentUser.name;
                    $location.path("/agents");
                },
                function(reason){
                    authenticated = false;
                    currentUser = "";
                    $location.path("/login");
                });
        };
    };
    app.controller("LoginCtrl", LoginCtrl);
    
    //Logout handling
    var LogoutCtrl = function($http, $location) {
        console.log("Logout");
        $scope.logout = function(){
        var request = $http({
            method: "get",
            url: BillingConfig.source + "logout"
        });   
        
        
        request.then(
            function (response) {
                //currentUser = null,
                //window.location.reload();
                authenticated = false;
                credentials = null;
                $location.path("/login");
            },
            function (reason) {
                return false;
            });
        };
    };
        
    app.controller("LogoutCtrl", LogoutCtrl);
    
}());