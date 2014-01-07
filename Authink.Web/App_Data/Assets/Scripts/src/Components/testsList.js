'use strict';

authink.directive('testsList', function () {
    
    return {
        
        restrict:   ' E',
        transclude: true,
        scope:      {
            
            api: '=',
            onTestSelected: '&',
            onTestEditCanceled: '&',
            onTestCreatingStarted: '&'
        },
        templateUrl: '/application/templates/testsList',
        
        controller: ['$scope', 'testsRepository', function ($scope, testsRepository) {

            $scope.tests         = null;
            $scope.childId       = null;
            $scope.displayedTest = null;

            $scope.init = function (childId) {

                $scope.childId = childId;

                $scope.isLoading = true;

                testsRepository.getAllTestsForChild_shortDetails(childId)
                .then(function (tests) {

                    $scope.isLoading = false;

                    $scope.tests = tests;

                    if (!$scope.displayedTest) {

                        setActiveTestOnLoad(tests);
                    }
                });
           }
            
            $scope.selectTest = function (test) {

                $scope.testListApi.displayedTest = test;
                
                if ($scope.isTestEditModeOn) {
                    
                    $scope.onTestEditCanceled();
                }
                
                $scope.onTestSelected({ test: test });
            };

            $scope.addNewTest = function() {

                var component = '<create-test api="createTestApi" on-test-created="onTestCreated(test)" on-test-selected="onTestSelected(test)"> </create-test>';

                $scope.$emit('openModal', component);

                $scope.onTestCreatingStarted({ childId: childId });
            };

            $scope.addNewTest = function(test){

                $scope.tests.unshift(test);

                $scope.displayedTest = test;
            }

            $scope.reload = function () {

                $scope.isLoading = true;

                testsRepository.getAllTestsForChild_shortDetails($scope.childId)
                .then(function (tests) {

                    $scope.isLoading = false;

                    $scope.tests = tests;

                    if (!$scope.displayedTest) {

                        setActiveTestOnLoad(tests);
                    }
                });
            }

            $scope.removeDisplayedTest = function () {

                $scope.displayedTest = null;
            }

            var setActiveTestOnLoad = function (tests) {

                if(tests.length > 0){
                    
                    $scope.selectTest(tests[0]);
                } else {

                    $scope.selectTest({});
                }
            }

            var resetState = function() {

                $scope.tests = null;
                $scope.childId = null;
                $scope.displayedTest = null;
            }

            $scope.api = {

                init: $scope.init,
                addNewTest: $scope.addNewTest,
                reload: $scope.reload,
                removeDisplayedTest: $scope.removeDisplayedTest
            }

            resetState();
        }]
    };
});