'use strict';

authink.directive('testTasksList', function () {
    
    return {
        
        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/TestTasksList.html',
        scope:       {},
        
        controller: ['$scope', 'testTasksListApi', function ($scope, testTasksListApi) {

            $scope.testTasksListApi = testTasksListApi;
            
            $scope.$watch('testTasksListApi.tasks', function (tasks) {

                if (tasks) {

                    $scope.tasks = tasks;
                }
            });

            $scope.previewTask = function(task) {

                var component = '<task-preview> </task-preview>';

                $scope.$emit('openModal', component);
                
                $scope.$emit('taskSelected', task.Id);
            };

            $scope.editTask = function(task) {

                var component = '<edit-task> </edit-task>';

                $scope.$emit('openModal', component);
                
                $scope.$emit('taskEditStarted', task.Id);
            };
        }]
    };
});

authink.factory('testTasksListApi', ['tasksRepository', function(tasksRepository) {

    return {
      
        tasks: null,
        testId: null,

        loadTasks: function(testId) {

            this.testId = testId;

            this.tasks = tasksRepository.getAll_shortDetails_byTestId(testId);
        },
        
        refreshTasks: function () {

            this.tasks = tasksRepository.getAll_shortDetails_byTestId(this.testId);
        }
    };
}]);