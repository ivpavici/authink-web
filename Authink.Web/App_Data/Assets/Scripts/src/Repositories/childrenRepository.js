'use strict';

authink.factory('childrenRepository', ['$resource', function ($resource) {

    var config = {

        apiUrls: {

            getAllForUser_shortDetails: "/api/children",
            getOne_shortDetails:        "/api/children/:childId",
            create:                     "/api/children/create",
            edit:                       "/api/children/edit"
        }
    };

    return {

        getAllForUser_shortDetails: function () {

            var resource = $resource(config.apiUrls.getAllForUser_shortDetails);

            return resource.query().$promise;
        },
        
        getOne_shortDetails: function (childId) {
            
            var resource = $resource(config.apiUrls.getOne_shortDetails);
            
            return resource.get({ childId: childId }).$promise;
        },
        
        create: function (child) {
            
            var resource = $resource(config.apiUrls.create);
            
            return  resource.save(child).$promise;
        },
        
        edit: function (child) {
            
            var resource = $resource(config.apiUrls.edit);
            
            return resource.save(child).$promise;
        }
    };
}]);