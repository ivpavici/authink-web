'use strict';

authink.directive('createTest', function () {

    return {

        restrict:    'E',
        templateUrl: '/application/templates/createTest',

        controller: ['$scope', 'testsRepository', 'childrenRepository', 'createTestApi', function ($scope, testsRepository, childrenRepository, createTestApi) {

            $scope.createTestApi = createTestApi;
            $scope.test          = {};
            $scope.isServerError = false;

            $scope.$watch('createTestApi.childId', function (childId) {

                if (childId) {

                    childrenRepository.getOne_shortDetails(childId)
                    .then(function (child) {

                        $scope.child = child;
                    });
                }
            });

            $scope.createTest= function() {
                
                var test = { name: $scope.test.name, shortDescription: $scope.test.shortDescription, longDescription: $scope.test.longDescription, childId: $scope.createTestApi.childId };

                var promise = testsRepository.create(test);
                promise.then(function (newTest) {

                    if (newTest) {
                        
                        $scope.$emit('testsList:testCreated', newTest);
                        
                        $scope.$emit('testsList:testSelected', newTest);
                        
                        resetScopeState();

                        $scope.$emit('closeModal');
                    }
                }, function(response) {

                    $scope.isServerError = true;
                });
            };

            $scope.closeModal = function () {

                $scope.$emit('closeModal');
            }

            var resetScopeState = function(){

                $scope.test          = {};
                $scope.isServerError = false;
            };

            resetScopeState();
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