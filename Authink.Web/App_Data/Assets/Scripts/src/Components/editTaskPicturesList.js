'use strict';

authink.directive('editTaskPicturesList', function () {

    return {

        restrict: 'E',
        templateUrl: '/application/templates/editTaskPicturesList',
        scope: {
            
            api: '=',
            onTaskPictureEditStarted:'&'
        },

        controller: ['$scope', 'picturesRepository', function ($scope, picturesRepository) {

            $scope.pictures = null;
            $scope.taskType = null;
            $scope.taskId = null;

            var taskTypes ={
                
                PairSameItems        : '#001',
                DetectDifferentItems : '#002',
                DetectColors         : '#003',
                ContinueSequence     : '#004',
                PairHalves           : '#005',
                Affiliation          : '#007',
                OrderBySize          : '#008',
                VoiceCommands        : '#009'
            };
            
            $scope.init = function (taskType, taskId, pictures) {

                $scope.taskType = taskType;
                $scope.taskId = taskId;
                $scope.pictures = pictures;

                transformPictures();
            }

            $scope.reload = function () {

                picturesRepository.getAll_forTaskGameplay($scope.taskId)
                .then(function (pictures) {

                    $scope.pictures = pictures;

                    transformPictures();
                });
            }
            
            var transformPictures = function() {

                    if ($scope.isSimpleEdit() || $scope.isDetectColorsEdit()) {

                        $scope.pictures = pictures;
                    }else if ($scope.isDetectCorrectItemEdit()) {

                        $scope.wrongPictures = [];
                        
                        angular.forEach(pictures, function(picture) {

                            if(picture.IsAnswer) {

                                $scope.correctPicture = picture;
                            } else {
                                
                                $scope.wrongPictures.push(picture);
                            }
                        });
                    }
                }

            $scope.isSimpleEdit = function () {
                
                var taskType = $scope.editTaskPicturesListApi.taskType;

                return taskType === taskTypes.OrderBySize          || taskType === taskTypes.PairSameItems        ||
                       taskType === taskTypes.DetectDifferentItems || taskType === taskTypes.DetectDifferentItems ||
                       taskType === taskTypes.ContinueSequence     || taskType === taskTypes.PairHalves;
            };
            
            $scope.isDetectCorrectItemEdit = function () {

                var taskType = $scope.editTaskPicturesListApi.taskType;

                return taskType === taskTypes.VoiceCommands;
            };

            $scope.isDetectColorsEdit = function () {
                
                var taskType = $scope.editTaskPicturesListApi.taskType;

                return taskType === taskTypes.DetectColors;
            };

            $scope.editPicture = function (picture) {
                
                var component = '<task-pictures-editor api=""taskPicturesEditorApi" on-picture-updated="onPictureUpdated()"> </task-pictures-editor>';
                
                $scope.$emit('openModal', component);

                $scope.onTaskPictureEditStarted({ taskType: $scope.taskType, taskId: $scope.taskId, picture: picture });
            };

            var resetScopeState = function () {

                $scope.editTaskPicturesListApi.pictures = null;
                $scope.pictures = null;
                $scope.taskType = null;
                $scope.taskId  = null;
            }

            $scope.api = {
                init: $scope.init
            }

            resetScopeState();
        }]
    }
});