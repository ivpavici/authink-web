'use strict';

authink.directive('taskPreview', function() {

    return {
        
        restrict:   'E',
        templateUrl: '/Assets/Templates/Components/TaskPreview.html',
        scope:       {},
        
        controller: ['$scope', 'tasksRepository', 'taskPreviewApi', function ($scope, tasksRepository, taskPreviewApi) {

            $scope.taskPreviewApi = taskPreviewApi;
            
            tasksRepository.getSingle_whereId($scope.taskPreviewApi.taskId)
            .then(function (task) {

                $scope.task = task;
            });

            $scope.editTask = function () {

                var component = '<edit-task> </edit-task>';

                $scope.$emit('openModal', component);

                $scope.$emit('testTasksList:taskEditStarted', $scope.task.Id);
            };
        }]
    };
});

authink.factory('taskPreviewApi', function() {

    return {
        
        taskId: null
    };
});