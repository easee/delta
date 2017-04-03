(function() {

    var app = angular.module("Billing");

    var LoginCtrl = function($scope, $rootScope, $http, $location, LoginService) {

        $scope.login = function() {
            $http.defaults.headers.common.Authorization = "Basic " + LoginService.encode($scope.user.name + ":" + $scope.user.pass);
            $http.defaults.headers.common.Signature = "CaByDlSnGLx0gOtlDClK6L94Jb/cwJoJAdUDlhtNpnI=";
            $http.defaults.headers.common.ApiKey = "RGVsdGEtQmlsbGluZw==";
            var promise = $http({
                method: "post",
                url: "http://localhost:9000/api/login",
                data: {
                    "apiKey": "RGVsdGEtQmlsbGluZw==",
                    "signature": "CaByDlSnGLx0gOtlDClK6L94Jb/cwJoJAdUDlhtNpnI="
                }});
            promise.then(
                function(response) {
                    authenticated = true;
                    currentUser = response.data.name;
                    $location.path("/agents");
                },
                function(reason){
                    authntication = false;
                    currentUser = "";
                    $location.path("/login");
                });
        };
    };
    app.controller("LoginCtrl", LoginCtrl);

    var LogoutCtrl = function($http) {

        var request = $http({
            method: "get",
            url: source + "logout"
        });
        request.then(
            function (response) {
                authenticated = false;
                return true;
            },
            function (reason) {
                return false;
            });
    }
    app.controller("LogoutCtrl", LogoutCtrl);

}());