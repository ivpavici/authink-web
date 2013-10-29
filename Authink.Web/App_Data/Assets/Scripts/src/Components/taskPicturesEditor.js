'use strict';

authink.directive('taskPicturesEditor', function () {

    return {

        restrict: 'E',
        templateUrl: '/Assets/Templates/Components/TaskPicturesEditor.html',
        scope: {},

        controller: ['$scope', 'taskPicturesEditorApi', function ($scope, taskPicturesEditorApi) {

            $scope.taskPicturesEditorApi = taskPicturesEditorApi;

            var taskTypes = {
                 
                PairSameItems:        '#001',
                DetectDifferentItems: '#002',
                DetectColors:         '#003',
                ContinueSequence:     '#004',
                PairHalves:           '#005',
                Affiliation:          '#007',
                OrderBySize:          '#008',
                VoiceCommands:        '#009'
            };

            $scope.$watch('taskPicturesEditorApi.picture', function (picture) {

                if (picture) {
                    
                    $scope.picture = picture;
                }
            });

            $scope.isSimpleEdit = function () {

                var taskType = $scope.taskPicturesEditorApi.taskType;

                return taskType === taskTypes.OrderBySize          || taskType === taskTypes.PairSameItems        ||
                       taskType === taskTypes.DetectDifferentItems || taskType === taskTypes.DetectDifferentItems ||
                       taskType === taskTypes.ContinueSequence     || taskType === taskTypes.PairHalves;
            };

            $scope.isDetectColorsEdit = function () {

                var taskType = $scope.taskPicturesEditorApi.taskType;

                return taskType === taskTypes.DetectColors;
            };
        }]
    };
});

authink.service('taskPicturesEditorApi', function () {

    return {

        picture:  null,
        taskType: null,
        taskId:   null,

        setupPictureEditor: function (taskType, taskId, picture) {

            this.taskType = taskType;
            this.taskId   = taskId;
            this.picture  = picture;
        }
    };
});