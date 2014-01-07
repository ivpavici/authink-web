'use strict';

authink.directive('taskPreview', function() {

    return {
        
        restrict:   'E',
        templateUrl: '/application/templates/taskPreview',
        scope: {

            api: '=',
            onTaskEditStarted: '&'
        },
        
        controller: ['$scope', 'tasksRepository', function ($scope, tasksRepository) {

            $scope.taskId = null;

            $scope.init = function (taksId) {

                $scope.taksId = taksId;

                tasksRepository.getSingle_whereId($scope.taskId)
                .then(function (task) {

                    $scope.task = task;
                });
            }

            $scope.editTask = function () {

                var component = '<edit-task> </edit-task>';

                $scope.$emit('openModal', component);

                $scope.onTaskEditStarted({taskId:$scope.task.Id});
            };

            $scope.closeModal = function() {

                $scope.$emit('closeModal');
            };

            var resetScopeState = function() {

                $scope.taskId = null;
            };

            $scope.api = {

                init: $scope.init
            }

            resetScopeState();
        }]
    };
});