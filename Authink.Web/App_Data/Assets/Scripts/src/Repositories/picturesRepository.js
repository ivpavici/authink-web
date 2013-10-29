'use strict';

authink.factory('picturesRepository', ['$http', '$resource', function ($http, $resource) {

    var config = {

        apiUrls: {

            insertPictureForUpdate: '/api/pictures/update/',
            getAll_forTaskGameplay: '/api/task/:taskId/pictures'
        }
    };

    return {

        insertPictureForUpdate: function (file, model) {

            return $http.uploadFile({
                
                url:  config.apiUrls.insertPictureForUpdate + model.pictureId +'/'+model.taskId,
                file: file,
            });
        },
        
        getAll_forTaskGameplay: function(taskId) {

            var resource = $resource(config.apiUrls.getAll_forTaskGameplay, {}, { query: { method: 'GET', isArray: true } });

            return resource.query({ taskId: taskId }).$promise;
        }
    };
}]);