'use strict';

authink.directive('editChild', function () {

    return {

        restrict: 'E',
        templateUrl: '/application/templates/editChild',

        controller: ['$scope', 'childrenRepository', 'editChildApi', 'picturesRepository', function ($scope, childrenRepository, editChildApi, picturesRepository) {

            $scope.editChildApi = editChildApi;
            $scope.isServerError = false;

            $scope.$watch('editChildApi.child', function (child) {

                if (child) {
                    
                    $scope.child = {

                        id:                child.Id,
                        firstName:         child.Firstname,
                        lastName:          child.Lastname,
                        profilePictureUrl: child.ProfilePictureUrl
                    };
                }
            });

            $scope.onFileSelect = function ($files) {

                angular.forEach($files, function (file) {

                    picturesRepository.children_insertPictureForUpdate(file, { childId: $scope.child.id })
                    .progress(function (evt) {

                        $scope.uploadProgress = parseInt(100.0 * evt.loaded / evt.total);
                        $scope.$apply();
                    })
                    .success(function (data) {

                        $scope.child.profilePictureUrl = data.pictureUrl;

                        $scope.$emit('editChild:pictureUpdated', $scope.child.id);

                        $scope.$apply();
                    });
                });
            }

            $scope.editChild = function () {

                var child = {childId: $scope.child.id, firstName: $scope.child.firstName, lastName: $scope.child.lastName};

                childrenRepository.edit(child)
                .then(function (response) {

                    if (response.StatusCode === 200) {

                        $scope.$emit('editChild:childEditEnded', $scope.child.id);

                        resetScopeState();

                        $scope.$emit('closeModal');
                    } else {

                        $scope.isServerError = true;
                    }
                }, function (data) {

                    $scope.isServerError = true;
                });
            };

            $scope.closeModal = function () {

                $scope.$emit('closeModal');
            };

            var resetScopeState = function () {

                $scope.isServerError = false;
            };

            resetScopeState();
        }]
    };
});

authink.factory('editChildApi', ['childrenRepository', function (childrenRepository) {

    return {

        child: null,
        
        setChildToEdit: function(childId) {

            this.child = childrenRepository.getOne_shortDetails(childId);
        }
    };
}]);