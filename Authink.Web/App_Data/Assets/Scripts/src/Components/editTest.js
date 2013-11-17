'use strict';

authink.directive('editTest', function() {

    return {
      
        restrict:   'E',
        templateUrl: '/application/templates/editTest',
        
        controller: ['$scope', '$modal', 'editTestApi', 'testsRepository', function ($scope, $modal, editTestApi, testsRepository) {

            $scope.editTestApi = editTestApi;

            $scope.$watch('editTestApi.testToEdit', function (testToEdit) {
                
                    if (testToEdit) {
                    
                    $scope.test = {
                        
                        Id:               testToEdit.Id,
                        Name:             testToEdit.Name,
                        ShortDescription: testToEdit.ShortDescription,
                        LongDescription:  testToEdit.LongDescription,
                        IsDeleted:        testToEdit.IsDeleted,
                        Tasks:            testToEdit.Tasks,
                        UserId:           testToEdit.UserId
                    };
                }
            });
            
            $scope.saveTest = function() {

                testsRepository.edit($scope.test)
                .then(function (response) {

                  $scope.$emit('editTest:testEditEnded');
                });
            };

            $scope.removeTest = function() {

                testsRepository.remove($scope.test.Id)
                .then(function (response) {

                    $scope.$emit('editTest:testDeleted');
                });
            };

            $scope.openConfirmationDialog = function () {

                var modal = $modal.open({

                    templateUrl: '/application/templates/testDeleteConfirmDialog',
                    scope: $scope
                });

                $scope.confirm = function () {

                    modal.close();
                    $scope.removeTest();
                };

                $scope.cancel = function () {

                    modal.dismiss('cancel');
                };
            }
        }]
    };
});

authink.factory('editTestApi', function() {

    return {
      
        testToEdit: null
    };
});