'use strict';

authink.directive('testTasksList', function () {
    
    return {
        
        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/TestTasksList.html',
        scope:       {},
        
        controller: ['$scope', 'tasksRepository', 'testTasksListApi', function ($scope, tasksRepository, testTasksListApi) {

            $scope.testTasksListApi = testTasksListApi;
            
            $scope.$watch('testTasksListApi.testId', function (testId) {

                if (testId) {

                    tasksRepository.getAll_shortDetails_byTestId($scope.testTasksListApi.testId)
                    .then(function (tasks) {

                        $scope.tasks = tasks;
                    });
                }
            });

            $scope.previewTask = function(task) {

                var component = '<task-preview> </task-preview>';

                $scope.$emit('openModal', component);
                
                $scope.$emit('taskSelected', task.Id);
            };
           
        }]
    };
});

authink.factory('testTasksListApi', function() {

    return {
      
        testId: null
    };
});