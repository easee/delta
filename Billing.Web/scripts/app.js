

    var app = angular.module("Billing", ["ngRoute"]);

    app.config(function($routeProvider){
        $routeProvider
            .when("/agents", {
                templateUrl: "views/agents.html",
                controller: "AgentsCtrl" })
            .when("/customers", {
                templateUrl: "views/customers.html",
                controller: "CustomersCtrl" })
            .otherwise({ redirectTo: "/agents" });
    });


