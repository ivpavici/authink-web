'use strict';

authink.factory('tasksRepository', ['$resource', function ($resource) {

    var config = {

        apiUrls: {
            
            getSingle_whereId:            '/api/tasks/:taskId',
            getAll_shortDetails_byTestId: '/api/test/:testId/tasks',
            update:                       '/api/task/update',
            remove:                       '/api/task/remove/:taskId'
        }
    };

    return {

        getSingle_whereId: function (taskId) {

            var resource = $resource(config.apiUrls.getSingle_whereId);

            return resource.get({ taskId: taskId }).$promise;
        },
        
        getAll_shortDetails_byTestId: function(testId) {
            
            var resource = $resource(config.apiUrls.getAll_shortDetails_byTestId, {testId : '@Id'}, { query : { method:'GET', isArray: true }});

            return resource.query({ testId: testId }).$promise;
        },
        
        update: function(task) {

            var resource = $resource(config.apiUrls.update);

            return resource.save(task).$promise;
        },
        
        remove: function(taskId) {

            var resource = $resource(config.apiUrls.remove);

            return resource.remove({ taskId: taskId }).$promise;
        }
    };
}]);