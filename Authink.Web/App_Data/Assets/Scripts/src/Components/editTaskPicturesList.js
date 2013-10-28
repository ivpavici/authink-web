'use strict';

authink.directive('editTaskPicturesList', function () {

    return {

        restrict: 'E',
        templateUrl: '/Assets/Templates/Components/EditTaskPicturesList.html',
        scope: {},

        controller: ['$scope', 'editTaskPicturesListApi', function ($scope, editTaskPicturesListApi) {

            $scope.editTaskPicturesListApi = editTaskPicturesListApi;

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
            
            $scope.$watch('editTaskPicturesListApi.pictures', function(pictures) {

                if (pictures) {
                    
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
            });

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
        }]
    };
});

authink.service('editTaskPicturesListApi', function () {

    return {

        pictures: null,
        taskType: null,
        
        setupPicturesList: function(taskType, pictures) {

            this.taskType = taskType;
            this.pictures = pictures;
        }
    };
});