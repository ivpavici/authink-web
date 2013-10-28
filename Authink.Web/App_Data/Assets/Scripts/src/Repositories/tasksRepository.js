'use strict';

authink.factory('tasksRepository', ['$resource', function ($resource) {

    var config = {

        apiUrls: {
            
            getSingle_whereId: "api/tasks/:taskId"
        }
    };

    return {

        getSingle_whereId: function (taskId) {

            var resource = $resource(config.apiUrls.getSingle_whereId);

            return resource.get({ taskId: taskId }).$promise;
        }
    };
}]);