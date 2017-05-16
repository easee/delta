(function() {
    app.controller("CategoriesCtrl", ['$scope', 'DataService', function($scope, DataService) {
        $scope.showCategories = false;
        ListCategories();

        $scope.edit = function(currentCategory) {
            $scope.category = currentCategory;
            $scope.showCategories = true;
        };
        $scope.save = function() {
            if ($scope.category.id == 0)
                DataService.insert("categories", $scope.category, function(data) { ListCategories(); });
            else
                DataService.update("categories", $scope.category.id, $scope.category, function(data) { ListCategories(); });
        };

        //CREATE NEW CATEGORY
        $scope.new = function() {
            swal({
                    title: "Add new Category",
                    text: "Insert a new Category:",
                    type: "input",
                    showCancelButton: true,
                    closeOnConfirm: false,
                    animation: "slide-from-top",
                    inputPlaceholder: "Enter Category"
                },
                function(inputValue) {
                    if (inputValue === false) return false;

                    if (inputValue === "") {
                        swal.showInputError("You need to write something!");
                        return false
                    }
                    $scope.category = {
                        id: 0,
                        name: inputValue
                    }
                    $scope.save();
                    swal("Nice!", "You added a new category", "success");
                    s
                });
            //DataService.insert("categories", $scope.category, function(data){ ListCategories();} );
            $scope.showCategories = true;
        };

        //DELETE CUSTOMER
        $scope.delete = function(category) {
            DataService.delete("categories", category.id, function(data) {
                swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this Category!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    },
                    function() {
                        ListCategories();
                        swal("Deleted!", "Category has been deleted.", "success");
                    });

            });
            $scope.showCategories = false;
        };

        function ListCategories() {
            DataService.list("categories", function(data) { $scope.category = data });
        }
    }]);
}());