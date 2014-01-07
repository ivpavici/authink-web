'use strict';

authink.directive('testTasksList', function () {
    
    return {
        
        restrict:    'E',
        templateUrl: '/application/templates/testTasksList',
        scope: {
            
            api: '=',
            onTaskEditStarted: '&',
            onTaskSelected: '&'
        },
        
        controller: ['$scope', '$modal', 'tasksRepository', function ($scope, $modal, tasksRepository) {

            $scope.tasks  = null;
            $scope.testId = null,

            $scope.init = function (testId) {

                $scope.testId = testId;

                tasksRepository.getAll_shortDetails_byTestId(testId)
                .then(function(tasks) {

                    $scope.tasks = tasks;
                });
            }

            $scope.reload = function() {
                
                tasksRepository.getAll_shortDetails_byTestId($scope.testId)
                .then(function(tasks) {

                    $scope.tasks = tasks;
                });
            }

            $scope.previewTask = function(task) {

                var component = '<task-preview api="taskPreviewApi" on-task-edit-started="onTaskEditStarted(taskId)"> </task-preview>';

                $scope.$emit('openModal', component);
                
                $scope.onTaskSelected({taskId:task.Id});
            };

            $scope.editTask = function(task) {

                var component = '<edit-task api="editTaskApi" on-task-edit-ended="onTaskEditEnded()" on-loaded="onLoaded(task)"> </edit-task>';

                $scope.$emit('openModal', component);
                
                $scope.onTaskEditStarted({taskId:task.Id});
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

            var resetScopeState = function () {

                $scope.tasks = null;
                $scope.testId = null;
            }

            $scope.api = {
                
                init:   $scope.init,
                reload: $scope.reload
            }

            resetScopeState();
        }]
    };
});