'use strict';

authink.directive('editChild', function () {

    return {

        restrict: 'E',
        templateUrl: '/application/templates/editChild',
        scope: {

            api: '=',
            onChildEditEnded: '&',
            onPictureUpdated: '&'
        },

        controller: ['$scope', 'childrenRepository', 'picturesRepository', function ($scope, childrenRepository, picturesRepository) {

            $scope.child = null;
            $scope.isServerError = false;

            $scope.init = function (childId) {

                childrenRepository.getOne_shortDetails(childId)
                .then(function(child) {

                    $scope.child = child;
                })

                //$scope.child = {

                //    id: child.Id,
                //    firstName: child.Firstname,
                //    lastName: child.Lastname,
                //    profilePictureUrl: child.ProfilePictureUrl
                //};
            }

            $scope.onFileSelect = function ($files) {

                angular.forEach($files, function (file) {

                    picturesRepository.children_insertPictureForUpdate(file, { childId: $scope.child.id })
                    .progress(function (evt) {

                        $scope.uploadProgress = parseInt(100.0 * evt.loaded / evt.total);
                        $scope.$apply();
                    })
                    .success(function (data) {

                        $scope.child.profilePictureUrl = data.pictureUrl;

                        $scope.onPictureUpdated({childId: $scope.child.id});

                        $scope.$apply();
                    });
                });
            }

            $scope.editChild = function () {

                var child = {childId: $scope.child.id, firstName: $scope.child.firstName, lastName: $scope.child.lastName};

                childrenRepository.edit(child)
                .then(function (response) {

                    if (response.StatusCode === 200) {

                        $scope.onChildEditEnded({childId: $scope.child.id});

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

                $scope.child = null;
                $scope.isServerError = false;
            };

            $scope.api = {

                init:$scope.init
            }

            resetScopeState();
        }]
    };
});