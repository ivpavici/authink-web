'use strict';

authink.directive('testTasksList', function () {
    
    return {
        
        restrict:    'E',
        templateUrl: '/application/templates/testTasksList',
        scope:       {},
        
        controller: ['$scope', '$modal', 'testTasksListApi', 'tasksRepository', function ($scope, $modal, testTasksListApi, tasksRepository) {

            $scope.testTasksListApi = testTasksListApi;
            
            $scope.$watch('testTasksListApi.tasks', function (tasks) {

                if (tasks) {

                    $scope.tasks = tasks;
                }
            });

            $scope.previewTask = function(task) {

                var component = '<task-preview> </task-preview>';

                $scope.$emit('openModal', component);
                
                $scope.$emit('testTasksList:taskSelected', task.Id);
            };

            $scope.editTask = function(task) {

                var component = '<edit-task> </edit-task>';

                $scope.$emit('openModal', component);
                
                $scope.$emit('testTasksList:taskEditStarted', task.Id);
            };

            $scope.removeTask = function(task){
            
                tasksRepository.remove(task.Id);
                
                var taskIndex = $scope.tasks.indexOf(task);
                $scope.tasks.splice(taskIndex, 1);
            }

            $scope.openConfirmationDialog = function (task) {

                var modal = $modal.open({

                    templateUrl: '/application/templates/taskDeleteConfirmDialog',
                    scope:       $scope
                });

                $scope.confirm = function () {

                    modal.close();
                    $scope.removeTask(task);
                };

                $scope.cancel = function () {

                    modal.dismiss('cancel');
                };
            }
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