'use strict';

authink.directive('testsList', ['testsRepository', 'testListApi', function (testsRepository, testListApi) {
    
    return {
        
        restrict:   ' E',
        transclude: true,
        
        templateUrl: '/Assets/Templates/Components/TestsList.html',
        
        controller: ['$scope', function ($scope) {

            $scope.testListApi = testListApi;
            $scope.activeTest  = {};
            
            $scope.$watch('testListApi.tests', function (tests) {
                
                if (tests) {
                    
                    $scope.tests = tests;
                }
            });
            
            $scope.selectTest = function (test) {

                $scope.activeTest = test;
                
                if ($scope.isTestEditModeOn) {
                    
                    $scope.$emit('testEditCanceled');
                }
                
                $scope.$emit('testSelected', test.Id);
            };

            $scope.addNewTest = function() {

                var component = '<create-test> </create-test>';

                $scope.$emit('openModal', component);

                $scope.$emit('testCreatingStarted', $scope.testListApi.childId);
            };
        }]
    };
}]);

authink.factory('testListApi', ['testsRepository', function (testsRepository) {

    return {

        tests:   null,
        childId: null,

        loadTests: function (childId) {
            
            this.childId = childId;
            this.tests   = testsRepository.getAllTestsForChild_shortDetails(childId);
        },
        
        refreshTests: function () {
            
            this.tests   = testsRepository.getAllTestsForChild_shortDetails(this.childId);
        }
    };
}]);