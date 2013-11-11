'use strict';

authink.directive('testPreview', function () {
    
    return {
        
        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/TestPreview.cshtml',
        
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
        }]
    };
});

authink.factory('testPreviewApi', function () {

    return {

        testId:    null,
        needReset: false,

        setActiveTest: function (testId) {

            this.testId = testId;
        },
        
        reset: function() {

            this.needReset = true;
        }
    };
});