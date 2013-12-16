'use strict';

authink.directive('taskPreview', function() {

    return {
        
        restrict:   'E',
        templateUrl: '/application/templates/taskPreview',
        scope:       {},
        
        controller: ['$scope', 'tasksRepository', 'taskPreviewApi', function ($scope, tasksRepository, taskPreviewApi) {

            $scope.taskPreviewApi = taskPreviewApi;
            
            $scope.$watch('taskPreviewApi.taskId', function (taskId) {

                if (taskId) {

                    tasksRepository.getSingle_whereId($scope.taskPreviewApi.taskId)
                    .then(function (task) {

                        $scope.task = task;
                    });
                }
            });

            $scope.editTask = function () {

                var component = '<edit-task> </edit-task>';

                $scope.$emit('openModal', component);

                $scope.$emit('testTasksList:taskEditStarted', $scope.task.Id);
            };

            $scope.closeModal = function() {

                $scope.$emit('closeModal');
            };
        }]
    };
});

authink.factory('taskPreviewApi', function() {

    return {
        
        taskId: null,

        refresh: function(){

            this.taskId = new Number(this.taskId);
        }
    };
});