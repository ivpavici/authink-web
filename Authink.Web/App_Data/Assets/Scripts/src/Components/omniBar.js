'use strict';

authink.directive('omniBar', function() {

    return {
      
        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/OmniBar.cshtml',
        controller: ['$scope','$location', 'accountServices', function ($scope, $location, accountServices) {

            $scope.currentUser = accountServices.tryGetSignedInUser();

            $scope.signOut = function () {
                
                accountServices.signOut();
                
                $location.path('login');
            };
        }]        
    };
});