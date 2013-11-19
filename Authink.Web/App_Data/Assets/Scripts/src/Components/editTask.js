'use strict';

authink.directive('editTask', function() {

    return {
      
        restrict:    'E',
        templateUrl: '/application/templates/editTask',
        scope:       {},
        
        controller: ['$scope', '$element', 'editTaskApi', 'tasksRepository', 'soundsRepository', function ($scope, $element, editTaskApi, tasksRepository, soundsRepository) {

            $scope.editTaskApi           = editTaskApi;
            $scope.isVoiceCommandPlaying = false;
            
            $scope.$watch('editTaskApi.taskId', function(taskId) {

                if (taskId) {

                    tasksRepository.getSingle_whereId(taskId)
                    .then(function (task) {

                        $scope.$emit('editTask:taskForEditLoaded', task);
                        
                        $scope.task = task;
                        console.log($scope.task);
                    });
                }
            });
            
            $scope.editTask = function () {

                tasksRepository.update($scope.task)
                .then(function (response) {

                    if(response.StatusCode === 200) {

                        $scope.$emit('editTask:taskEditEnded');
                        
                        $scope.$emit('closeModal');
                    } else {
                        
                        //Add validation
                    }
                });
            };
            $scope.onSoundFileSelect = function ($files) {

                angular.forEach($files, function (file) {
                    
                    if(!isFileAudio(file)){return;}

                    if ($scope.task.VoiceCommand) {

                        soundsRepository.task_insertSoundForUpdate(file, { soundId: $scope.task.VoiceCommand.Id, taskId: $scope.task.Id })
                        .progress(function (evt) {

                            $scope.uploadProgress = parseInt(100.0 * evt.loaded / evt.total);
                            $scope.$apply();
                        })
                        .success(function (sound) {

                            $scope.task.VoiceCommand.Url = sound.Url;
                            $scope.$apply();
                        });
                    } else {

                        soundsRepository.task_insertSound(file, $scope.task.Id)
                        .progress(function (evt) {

                            $scope.uploadProgress = parseInt(100.0 * evt.loaded / evt.total);
                            $scope.$apply();
                        })
                        .success(function (sound) {

                            $scope.task.VoiceCommand = sound;
                            $scope.$apply();
                        });
                    }
                });
            };
            $scope.playVoiceCommand = function () {

                var audioElement = $element.find("#voiceCommandPlayer")[0];

                if ($scope.isVoiceCommandPlaying) {

                    audioElement.pause();
                    audioElement.currentTime = 0;

                    $scope.isVoiceCommandPlaying = false;
                } else {

                    audioElement.load();
                    audioElement.play();
                    $scope.isVoiceCommandPlaying = true;
                }
            }
            var isFileAudio = function (file) {

                return file.type.indexOf("audio") != -1;
            }
        }]
    };
});

authink.service('editTaskApi', function() {

    return {
        
        taskId: null
    }; 
});