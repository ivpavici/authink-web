'use strict';

authink.directive('editTest', function() {

    return {
      
        restrict: 'E',
        templateUrl: '/application/templates/editTest',
        scope: {

            api: '=',
            onTestEditEnded: '&',
            onEditCanceled: '&',
            onTestDeleted:'&'
        },
        controller: ['$scope', '$modal', 'testsRepository', function ($scope, $modal, testsRepository) {

            $scope.testToEdit = null;

            $scope.init = function (testToEdit) {

                $scope.originalTest = testToEdit;

                $scope.editableTest = {

                    Id: testToEdit.Id,
                    Name: testToEdit.Name,
                    ShortDescription: testToEdit.ShortDescription,
                    LongDescription: testToEdit.LongDescription,
                    IsDeleted: testToEdit.IsDeleted,
                    Tasks: testToEdit.Tasks,
                    UserId: testToEdit.UserId
                };
            }
            
            $scope.saveTest = function() {

                testsRepository.edit($scope.editableTest)
                .then(function (response) {

                    if (response.StatusCode === 200) {

                        $scope.onTestEditEnded({test:$scope.editableTest });

                        resetState();

                    } else {

                        $scope.isServerError = true;
                    }
                });
            };

            $scope.cancelEdit = function () {

                if (areChangedMade()) {

                    var modal = $modal.open({

                        templateUrl: '/application/templates/testEditConfirmDialog',
                        scope: $scope
                    });

                    $scope.saveAndExit = function () {

                        $scope.saveTest();

                        modal.close();
                    };

                    $scope.exit = function () {

                        $scope.onEditCanceled();

                        resetState();
                        modal.close();
                    }
                    $scope.close = function () {

                        modal.close();
                    };
                } else {

                    resetState();

                    $scope.onEditCanceled();
                }
            };

            $scope.openConfirmationDialog = function () {

                var modal = $modal.open({

                    templateUrl: '/application/templates/testDeleteConfirmDialog',
                    scope: $scope
                });

                $scope.confirm = function () {

                  testsRepository.remove($scope.test.Id)
                  .then(function (response) {

                    if (response.StatusCode === 200) {

                        $scope.onTestDeleted();

                        resetState();

                        modal.close();
                    } else {

                        $scope.isServerError = true;
                    }});
                };

                $scope.cancel = function () {

                    resetState();

                    modal.dismiss('cancel');
                };
            }

            var areChangedMade = function () {

                return $scope.editableTest.Id               != $scope.originalTest.Id ||
                       $scope.editableTest.Name             != $scope.originalTest.Name ||
                       $scope.editableTest.ShortDescription != $scope.originalTest.ShortDescription ||
                       $scope.editableTest.LongDescription  != $scope.originalTest.LongDescription;
            };

            var resetState = function () {

                $scope.testToEdit    = null;
                $scope.editableTest  = null;
                $scope.originalTest  = null;
                $scope.isServerError = false;
            }

            resetState();
        }]
    };
});