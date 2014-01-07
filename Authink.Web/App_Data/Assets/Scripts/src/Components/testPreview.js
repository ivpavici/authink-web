'use strict';

authink.directive('testPreview', function () {
    
    return {
        
        restrict:    'E',
        templateUrl: '/application/templates/testPreview',
        scope: {

            api: '=',
            onTestEditStarted: '&'
        },
        
        controller: ['$scope','testsRepository', function ($scope, testsRepository) {

            $scope.testId = null;

            $scope.init = function (testId) {
                
                $scope.isLoading = true;
                $scope.testId = testId;

                testsRepository.getOne_longDetails(testId)
                    .then(function (test) {

                    $scope.isLoading = false;
                    $scope.test = test;
                });
            }

            $scope.editTest = function() {

                $scope.onTestEditStarted({ test: $scope.test });
            };

            var resetState = function () {

                $scope.test = null;
            }

            $scope.api = {
                init: $scope.init,
                resetState: resetState
            };

            resetState();
        }]
    };
});