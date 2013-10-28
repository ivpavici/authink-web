'use strict';

authink.directive('editTask', function() {

    return {
      
        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/EditTask.html',
        scope:       {},
        
        controller: ['$scope', 'editTaskApi', 'tasksRepository', function ($scope, editTaskApi, tasksRepository) {

            $scope.editTaskApi = editTaskApi;
            
            $scope.$watch('editTaskApi.taskId', function(taskId) {

                if (taskId) {

                    tasksRepository.getSingle_whereId(taskId)
                    .then(function (task) {
                        
                        $scope.task = task;
                    });
                }
            });
            
            $scope.editTask = function() {

                tasksRepository.update($scope.task)
                .then(function (response) {

                    if(response.StatusCode === 200) {

                        $scope.$emit('taskEditEnded');
                        
                        $scope.$emit('closeModal');
                    } else {
                        
                        //Add validation
                    }
                });
            };
        }]
    };
});

authink.service('editTaskApi', function() {

    return {
        
        taskId: null
    }; 
});