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

            $scope.editPicture = function (picture) {
                
                var component = '<task-pictures-editor> </task-pictures-editor>';
                
                $scope.$emit('openModal', component);

                $scope.$emit('editTaskPicturesList:taskPictureEditStarted', $scope.editTaskPicturesListApi.taskType, $scope.editTaskPicturesListApi.taskId, picture);
            };
        }]
    };
});

authink.service('editTaskPicturesListApi', ['picturesRepository', function (picturesRepository) {

    return {

        pictures: null,
        taskType: null,
        taskId:   null,
        
        setupPicturesList: function(taskType, taskId, pictures) {

            this.taskType = taskType;
            this.taskId   = taskId;
            this.pictures = pictures;
        },
        
        forceRefresh: function() {

            this.pictures = picturesRepository.getAll_forTaskGameplay(this.taskId);
        }
    };
}]);