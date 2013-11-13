'use strict';

authink.directive('editTest', function() {

    return {
      
        restrict:   'E',
        templateUrl: '/application/templates/editTest',
        
        controller: ['$scope', 'editTestApi', 'testsRepository', function ($scope, editTestApi, testsRepository) {

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
        }]
    };
});

authink.factory('editTestApi', function() {

    return {
      
        testToEdit: null
    };
});