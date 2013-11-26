'use strict';

authink.directive('testPreview', function () {
    
    return {
        
        restrict:    'E',
        templateUrl: '/application/templates/testPreview',
        
        controller: ['$scope', 'testPreviewApi', 'testsRepository', function ($scope, testPreviewApi, testsRepository) {

            $scope.testPreviewApi = testPreviewApi;

            $scope.$watch('testPreviewApi.testId', function (testId) {

                if (testId){

                    $scope.isLoading = true;

                    testsRepository.getOne_longDetails(testId).then(function (test) {

                        $scope.isLoading = false;
                        $scope.test      = test;
                    });
                }
            });

            $scope.$watch('testPreviewApi.needReset', function (needReset) {
               
                if(needReset){

                    $scope.test = null;

                    $scope.testPreviewApi.needReset = false;
                 }
            });

            $scope.editTest = function() {

                $scope.$emit('testPreview:testEditStarted', $scope.test);
            };

            var resetState = function () {

                $scope.testPreviewApi.testId = null;
            }

            resetState();
        }]
    };
});

authink.factory('testPreviewApi', function () {

    return {

        testId:    null,
        needReset: false,

        setActiveTest: function (testId) {

            this.testId = new Number(testId);
        },
        
        reset: function() {

            this.needReset = true;
        },
    };
});