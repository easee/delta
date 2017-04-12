(function() {
    app.directive('adminView', [function () {
        return {
            restrict: 'A',
            link: function (scope, elem, attr) {
                if (credentials.currentUser.roles.indexOf("admin") > -1)
                    elem.css("display", "block");
                else
                    elem.css("display", "none");
            }
        };
    }]);
}());