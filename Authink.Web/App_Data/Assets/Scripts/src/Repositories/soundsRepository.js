
authink.factory('soundsRepository', ['$http', function ($http) {

    var config = {

        apiUrls: {

            task_insertSoundForUpdate: '/api/sounds/update/',
            task_insertSound:          '/api/sounds/create/'
        }
    };

    return {

        task_insertSoundForUpdate: function (file, model) {

            return $http.uploadFile({
                
                url:  config.apiUrls.task_insertSoundForUpdate + model.soundId + '/'+ model.taskId,
                file: file,
            });
        },

        task_insertSound: function (file, taskId) {

            return $http.uploadFile({
                
                url:  config.apiUrls.task_insertSound + taskId,
                file: file,
            });
        }}
}]);