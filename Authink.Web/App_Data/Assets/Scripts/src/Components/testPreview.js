'use strict';

authink.directive('testPreview', function () {
    
    return {
        
        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/TestPreview.html',
        
        controller: ['$scope', 'testPreviewApi', function ($scope, testPreviewApi) {

            $scope.api = testPreviewApi;

            $scope.$watch('api.test', function(test) {

                $scope.test = test;
            });

            $scope.editTest = function() {

                $scope.$emit('testEditStarted', $scope.test);
            };
        }]
    };
});

authink.factory('testPreviewApi', ['testsRepository', function (testsRepository) {

    return {

        test: null,

        setActiveTest: function (testId) {

            this.test = testsRepository.getOne_longDetails(testId);;
        },
        
        reset: function() {

            this.test = null;
        }
    };
}]);