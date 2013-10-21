'use strict';

authink.directive('createTest', function () {

    return {

        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/CreateTest.html',

        controller: ['$scope', 'testsRepository', 'createTestApi', function ($scope, testsRepository, createTestApi) {

            $scope.createTestApi = createTestApi;
            
            $scope.createTest= function() {
                
                var test = { name: $scope.test.name, shortDescription: $scope.test.shortDescription, longDescription: $scope.test.longDescription, childId: $scope.createTestApi.childId };

                var promise = testsRepository.create(test);
                promise.then(function (response) {

                    if (response.testId) {
                        
                        $scope.$emit('testListChanged', $scope.createTestApi.childId);
                        
                        $scope.$emit('testSelected', response.testId);
                        
                        $scope.$emit('closeModal');
                    }
                });
            };
        }]
    };
});

authink.factory('createTestApi',  function () {

    return {

        childId: null,

        setChildToAddTests: function (childId) {
            
            this.childId = childId;
        }
    };
});