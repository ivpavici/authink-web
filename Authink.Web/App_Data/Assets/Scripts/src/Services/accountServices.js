'use strict';

authink.factory('accountServices', ['$resource', function ($resource) {

    var config = {

        apiUrls: {

            tryGetSignedInUser: "/api/user",
            login:              "/api/user/login",
            signUp:             "/api/user/sign-up",
            signOut:            "/api/user/sign-out"
        }
    };

    return {

        tryGetSignedInUser: function () {

            var resource = $resource(config.apiUrls.tryGetSignedInUser);

            return resource.get().$promise;
        },
        
        login: function(user) {

            var resource = $resource(config.apiUrls.login);
            
            return  resource.save(user).$promise;
        },
        
        signUp: function (user) {
            
            var resource = $resource(config.apiUrls.signUp);
            
            return resource.save(user).$promise;
        },
        
        signOut: function () {
            
            var resource = $resource(config.apiUrls.signOut);

            resource.get({});
        }
    };
}]);