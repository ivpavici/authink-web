'use strict';

authink.directive('createTest', function () {

    return {

        restrict:    'E',
        templateUrl: '/application/templates/createTest',
        scope: {
            
            api: '=',
            onTestCreated: '&',
            onTestSelected: '&'
        },

        controller: ['$scope', 'testsRepository', 'childrenRepository', function ($scope, testsRepository, childrenRepository) {

            $scope.createTestApi = createTestApi;
            $scope.test          = {};
            $scope.isServerError = false;
            $scope.childId       = null;

            $scope.init = function (childId) {

                $scope.childId = childId;

                childrenRepository.getOne_shortDetails(childId)
                .then(function (child) {

                    $scope.child = child;
                });
            }

            $scope.createTest= function() {
                
                var test = { name: $scope.test.name, shortDescription: $scope.test.shortDescription, longDescription: $scope.test.longDescription, childId: $scope.createTestApi.childId };

                var promise = testsRepository.create(test);
                promise.then(function (newTest) {

                    if (newTest) {
                        
                        $scope.onTestCreated({test: newTest});
                        
                        $scope.onTestSelected({test: newTest});
                        
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
                $scope.childId = null;
            };

            resetScopeState();
        }]
    };
});