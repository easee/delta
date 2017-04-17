(function() {
    app.directive('isFromAgent', [function() {
        return {
            priority: 10,
            restrict: 'A',
            scope: { param: '@' },
            link: function(scope, element, attr) {
                var username = scope.param;
                if (username === credentials.currentUser.username)
                    element.css("display", "table-cell");
            }
        };
    }]);
}());