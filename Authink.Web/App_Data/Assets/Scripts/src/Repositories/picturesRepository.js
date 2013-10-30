'use strict';

authink.factory('picturesRepository', ['$http', '$resource', function ($http, $resource) {

    var config = {

        apiUrls: {

            task_insertPictureForUpdate:     '/api/pictures/update/',
            children_insertPictureForUpdate: '/api/children/picture/',
            getAll_forTaskGameplay:          '/api/task/:taskId/pictures',
            updateColorsForPicture:          '/api/colors/update'
        }
    };

    return {

        task_insertPictureForUpdate: function (file, model) {

            return $http.uploadFile({
                
                url:  config.apiUrls.task_insertPictureForUpdate + model.pictureId + '/' + model.taskId,
                file: file,
            });
        },

        children_insertPictureForUpdate: function (file, model) {

            return $http.uploadFile({
                
                url:  config.apiUrls.children_insertPictureForUpdate + model.childId,
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