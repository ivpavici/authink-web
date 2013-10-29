'use strict';

authink.factory('picturesRepository', ['$http', '$resource', function ($http, $resource) {

    var config = {

        apiUrls: {

            insertPictureForUpdate: '/api/pictures/update/',
            getAll_forTaskGameplay: '/api/task/:taskId/pictures',
            updateColorsForPicture: '/api/colors/update'
        }
    };

    return {

        insertPictureForUpdate: function (file, model) {

            return $http.uploadFile({
                
                url:  config.apiUrls.insertPictureForUpdate + model.pictureId +'/'+model.taskId,
                file: file,
            });
        },
        
        updateColorsForPicture: function (model) {
            
            var resource = $resource(config.apiUrls.updateColorsForPicture);

            return resource.save(model).$promise;
        },
        
        getAll_forTaskGameplay: function(taskId) {

            var resource = $resource(config.apiUrls.getAll_forTaskGameplay, {}, { query: { method: 'GET', isArray: true } });

            return resource.query({ taskId: taskId }).$promise;
        },
     };
}]);