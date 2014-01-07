'use strict';

authink.directive('taskPicturesEditor', function () {

    return {

        restrict: 'E',
        templateUrl: '/application/templates/taskPicturesEditor',
        scope: {

            api: '=',
            onPictureUpdated: '&'
        },

        controller: ['$scope', 'taskPicturesEditorApi', 'picturesRepository', function ($scope, taskPicturesEditorApi, picturesRepository) {

            $scope.picture = null;
            $scope.taskType = null;
            $scope.taskId = null;

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

            $scope.init = function (taskType, taskId, picture) {

              $scope.taskType = taskType;
              $scope.taskId  = taskId;
              $scope.picture = picture;
            }

            $scope.isSimpleEdit = function () {

                var taskType = $scope.taskType;

                return taskType === taskTypes.OrderBySize          || taskType === taskTypes.PairSameItems        ||
                       taskType === taskTypes.DetectDifferentItems || taskType === taskTypes.DetectDifferentItems ||
                       taskType === taskTypes.ContinueSequence     || taskType === taskTypes.PairHalves;
            };

            $scope.isDetectColorsEdit = function () {

                var taskType = $scope.taskType;

                return taskType === taskTypes.DetectColors;
            };

            $scope.onFileSelect = function($files) {

                angular.forEach($files, function(file) {

                    picturesRepository.task_insertPictureForUpdate(file, { pictureId: $scope.picture.Id, taskId: $scope.taskId })
                    .progress(function(evt) {

                        $scope.uploadProgress = parseInt(100.0 * evt.loaded / evt.total);
                        $scope.$apply();
                    })
                    .success(function(picture) {

                        $scope.picture = picture;
                        $scope.$apply();

                        $scope.onPictureUpdated();
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

            var resetScopeState = function () {

                $scope.picture = null;
                $scope.taskType = null;
                $scope.taskId = null;
            }

            $scope.api = {
                
                init:$scope.init
            }

            resetScopeState();
        }]
    };
});