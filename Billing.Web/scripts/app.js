(function() {

    app = angular.module("Billing", ["ngRoute", "LocalStorageModule", "ui.bootstrap", "ngSanitize", "chart.js"]);

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

    app.config(function($routeProvider) {
        $routeProvider
            .when("/agents", {
                templateUrl: "views/agents.html",
                controller: "AgentsCtrl"
            })
            .when("/customers", {
                templateUrl: "views/customers.html",
                controller: "CustomersCtrl"
            })
            .when("/categories", {
                templateUrl: "views/categories.html",
                controller: "CategoriesCtrl"
            })
            .when("/suppliers", {
                templateUrl: "views/suppliers.html",
                controller: "SuppliersCtrl"
            })
            .when("/salesbycustomercategory", {
                templateUrl: "views/Reports/salesbycustomercategory.html",
                controller: "SalesByCustomerCategoryCtrl"
            })
            .when("/salesbycategory", {
                templateUrl: "views/reports/salesbycategory.html",
                controller: "SalesByCategoryCtrl"
            })
            .when("/salesbycustomer", {
                templateUrl: "views/Reports/salesbycustomer.html",
                controller: "SalesByCustomerCtrl"
            })
            .when("/shippers", {
                templateUrl: "views/shippers.html",
                controller: "ShippersCtrl"
            })
            .when("/procurements", {
                templateUrl: "views/procurements.html",
                controller: "ProcurementsCtrl"
            })
            .when("/products", {
                templateUrl: "views/products.html",
                controller: "ProductsCtrl"
            })
            .when("/agentregion", {
                templateUrl: "views/reports/agentregion.html",
                controller: "AgentRegionCtrl"
            })
            .when("/invoices", {
                templateUrl: "views/invoices.html",
                controller: "InvoicesCtrl"
            })
            .when("/dashboard", {
                templateUrl: "views/dashboard.html",
                controller: "DashboardCtrl"
            })
            .when("/login", {
                templateUrl: "views/login.html",
                controller: "LoginCtrl"
            })
            .when("/logout", {
                template: "",
                controller: "LogoutCtrl"
            })
            .when("/salesbycategory", {
                templateUrl: "views/reports/salesbycategory.html",
                controller: "SalesByCategoryCtrl"
            })
            .when("/salesbyregion", {
                templateUrl: "views/reports/salesbyregion.html",
                controller: "SalesByRegionCtrl"
            })
            .when("/stocklevel", {
                templateUrl: "views/reports/stocklevel.html",
                controller: "StockLevelCtrl"
            })
            .when("/invoicesreview", {
                templateUrl: "views/reports/invoicesreview.html",
                controller: "InvoicesReviewCtrl"
            })
            .otherwise({ redirectTo: "/dashboard" });
    }).run(function($rootScope, $location) {
        $rootScope.$on("$routeChangeStart", function(event, next, current) {
            if (!authenticated()) {
                if (next.templateUrl != "views/login.html") {
                    redirectTo = $location.path();
                    if (redirectTo == "/login") redirectTo = "/dashboard";
                    if (redirectTo == "/logout") redirectTo = "/dashboard";
                    $location.path("/login");
                }
            }
        })
        $rootScope.authenticated = authenticated;
    });
}());