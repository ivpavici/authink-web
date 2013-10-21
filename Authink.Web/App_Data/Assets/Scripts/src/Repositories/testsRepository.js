'use strict';

authink.factory('testsRepository', ['$resource', function ($resource) {

    var config = {
        
        apiUrls: {
            
            getAllTestsForChild_shortDetails: "api/children/:childId/tests",
            getOne_longDetails:               "api/tests/:testId",
            create:                           "api/tests/create",
            edit:                             "api/tests/edit",
            remove:                           "api/tests/delete/:testId"
        }
    };
    
    return {
        
        getAllTestsForChild_shortDetails: function(childId) {

            var resource = $resource(config.apiUrls.getAllTestsForChild_shortDetails, { childId: '@Id' }, { query: { method: "GET", isArray: true } });
            
            return resource.query({ childId: childId }).$promise;
        },
        
        getOne_longDetails: function (testId) {
            
            var resource = $resource(config.apiUrls.getOne_longDetails);
            
            return resource.get({ testId: testId }).$promise;
        },
        
        create: function(test) {
            
            var resource = $resource(config.apiUrls.create);
            
            return  resource.save(test).$promise;
        },
        
        edit: function(test) {

            var resource = $resource(config.apiUrls.edit);

            return  resource.save(test).$promise;
        },
        
        remove: function (testId) {
            
            var resource = $resource(config.apiUrls.remove, { 'remove': { method: 'DELETE' } });

            return resource.remove({ testId: testId }).$promise;
        }
    };
}]);