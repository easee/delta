(function(){
    
    credentials = {
        token: "",
        expiration: "",
        currentUser: {
            id: 0,
            name: "",
            role: ""
        }
    };
    
    function authenticated() {
        return (credentials.currentUser.id != 0)
    } 
    
    var app = angular.module("Billing", ["ngRoute"]);

    app.config(function($routeProvider){
        $routeProvider
            .when("/agents", {
                templateUrl: "views/agents.html",
                controller: "AgentsCtrl" })
            .when("/customers", {
                templateUrl: "views/customers.html",
                controller: "CustomersCtrl" })
            .when("/login", {
                templateUrl: "views/login.html",
                controller: "LoginCtrl" })
            .otherwise({ redirectTo: "/agents" });
    }).run(function($rootScope, $location){
        $rootScope.$on("$routeChangeStart", function(event, next, current){
            if(!authenticated()){
                if(next.templateUrl != "views/login.html"){
                    $location.path("/login");
                }
            }
        })
        $rootScope.authenticated = authenticated;
    });
}());