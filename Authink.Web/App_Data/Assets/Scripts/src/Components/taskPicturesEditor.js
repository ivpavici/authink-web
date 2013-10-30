'use strict';

authink.directive('taskPicturesEditor', function () {

    return {

        restrict: 'E',
        templateUrl: '/Assets/Templates/Components/TaskPicturesEditor.html',
        scope: {},

        controller: ['$scope', 'taskPicturesEditorApi', 'picturesRepository', function ($scope, taskPicturesEditorApi, picturesRepository) {

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

            $scope.onFileSelect = function($files) {

                angular.forEach($files, function(file) {

                    picturesRepository.task_insertPictureForUpdate(file, { pictureId: $scope.taskPicturesEditorApi.picture.Id, taskId: $scope.taskPicturesEditorApi.taskId })
                    .progress(function(evt) {

                        $scope.uploadProgress = parseInt(100.0 * evt.loaded / evt.total);
                        $scope.$apply();
                    })
                    .success(function(picture) {

                        $scope.picture = picture;
                        $scope.$apply();

                        $scope.$emit('taskPicturesEditor:pictureUpdated');
                    });
                });
            };

            $scope.editWithColors = function() {

                var model = {
                  
                    correctColor: $scope.picture.CorrectColor,
                    wrongColors : $scope.picture.WrongColors
                };
                
                picturesRepository.updateColorsForPicture(model)
                    .then(function(response) {
                
                        if(response.StatusCode === 200) {

                            $scope.$emit('closeModal');
                        }
                });
            };

            $scope.closeModel = function(){

                $scope.$emit('closeModal');
            }
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