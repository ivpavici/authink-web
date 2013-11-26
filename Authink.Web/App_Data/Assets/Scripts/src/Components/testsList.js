'use strict';

authink.directive('testsList', function () {
    
    return {
        
        restrict:   ' E',
        transclude: true,
        
        templateUrl: '/application/templates/testsList',
        
        controller: ['$scope', 'testsRepository', 'testListApi', function ($scope, testsRepository, testListApi) {

            $scope.testListApi = testListApi;
            
            $scope.$watch('testListApi.childId', function (childId) {
                
                if (childId) {

                    $scope.isLoading = true;

                    testsRepository.getAllTestsForChild_shortDetails(childId).then(function (tests) {

                        $scope.isLoading         = false;
                        $scope.testListApi.tests = tests;

                        if (!$scope.testListApi.displayedTest) {

                            setActiveTestOnLoad(tests);
                        }
                    });
                }
            });
            
            $scope.selectTest = function (test) {

                $scope.testListApi.displayedTest = test;
                
                if ($scope.isTestEditModeOn) {
                    
                    $scope.$emit('testsList:testEditCanceled');
                }
                
                $scope.$emit('testsList:testSelected', test);
            };

            $scope.addNewTest = function() {

                var component = '<create-test> </create-test>';

                $scope.$emit('openModal', component);

                $scope.$emit('testsList:testCreatingStarted', $scope.testListApi.childId);
            };

            var setActiveTestOnLoad = function (tests) {

                if(tests.length > 0){
                    
                    $scope.selectTest(tests[0]);
                } else {

                    $scope.selectTest({});
                }
            }

            var resetState = function() {

                $scope.testListApi.reset();
            }

            resetState();
        }]
    };
});

authink.factory('testListApi', ['testsRepository', function (testsRepository) {

    return {

        tests:         null,
        childId:       null,
        displayedTest: null,

        setChildId: function (childId) {

            this.childId = childId;
        },
        
        refreshTests: function () {
            
            this.childId = new Number(this.childId);
        },

        addNewTest: function(test){

            this.tests.unshift(test);

            this.displayedTest = test;
        },

        removeDisplayedTest: function(){

            this.displayedTest = null;
        },

        reset: function () {

            this.tests         =  null;
            this.childId       =  null;
            this.displayedTest =  null;
        }
    };
}]);