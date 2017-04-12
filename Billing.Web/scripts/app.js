(function(){
    
    app = angular.module("Billing", ["ngRoute", "LocalStorageModule", "ui.bootstrap"]);
    
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
    
    redirectTo = '/';
    
    app.config(function($routeProvider){
        $routeProvider
            .when("/agents", {
                templateUrl: "views/agents.html",
                controller: "AgentsCtrl" })
            .when("/customers", {
                templateUrl: "views/customers.html",
                controller: "CustomersCtrl" })
            /*Dodao categories*/
            .when("/categories", {
            templateUrl: "views/categories.html",
            controller:"CategoriesCtrl"})
            /*--------------*/
            /*Dodao categories*/
            .when("/suppliers", {
            templateUrl: "views/suppliers.html",
            controller:"SuppliersCtrl"})
            /*--------------*/
            .when("/shippers", {
                templateUrl: "views/shippers.html",
                controller: "ShippersCtrl" })
            .when("/procurements", {
                templateUrl: "views/procurements.html",
                controller: "ProcurementsCtrl" })
            .when("/products", {
                templateUrl: "views/products.html",
                controller: "ProductsCtrl" })
            .when("/invoices", {
                templateUrl: "views/invoices.html",
                controller: "InvoicesCtrl" })
            .when("/login", {
                templateUrl: "views/login.html",
                controller: "LoginCtrl" })
            .when("/logout", {
                template: "",
                controller: "LogoutCtrl" })
            .otherwise({ redirectTo: "/agents" });
    }).run(function($rootScope, $location){
        $rootScope.$on("$routeChangeStart", function(event, next, current){
            if(!authenticated()){
                if(next.templateUrl != "views/login.html"){
                    redirectTo = $location.path();
                    $location.path("/login");
                }
            }
        })
        $rootScope.authenticated = authenticated;
    });
}());