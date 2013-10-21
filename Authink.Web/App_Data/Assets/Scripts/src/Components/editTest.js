'use strict';

authink.directive('editTest', function() {

    return {
      
        restrict:   'E',
        templateUrl: '/Assets/Templates/Components/EditTest.html',
        
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

                var editPromise = testsRepository.edit($scope.test);
                editPromise.then(function(response) {

                  $scope.$emit('testEditEnded');
                });
            };

            $scope.removeTest = function() {

                var deletePromise = testsRepository.remove($scope.test.Id);
                deletePromise.then(function(response) {

                    $scope.$emit('testDeleted');
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