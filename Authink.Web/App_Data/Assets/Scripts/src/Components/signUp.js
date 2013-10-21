'use strict';

authink.directive('signUp', ['accountServices', function (accountServices) {

    return {

        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/SignUp.html',
        
        controller: ['$scope','$location', function ($scope, $location) {

            $scope.login = function() {

                var user = { username: $scope.username, password: $scope.password };

                var response = accountServices.login(user);
                response.then(function(responseData) {
                    
                    if (responseData.isSuccessful) {

                        $location.path('/');
                    } else {
                        
                        //dodat error message
                    }
                });
            };
            
            $scope.signUp = function () {
                var user = {
                    
                    firstname: $scope.user.firstname,
                    lastname:  $scope.user.lastname,
                    email:     $scope.user.email,
                    username:  $scope.user.username,
                    password:  $scope.user.password
                };

                var response = accountServices.signUp(user);
                response.then(function (responseData) {

                    if (responseData.isSuccessful) {

                        $scope.isSignUp = false;
                    } else {

                        //dodat error message
                    }
                });
            };

            $scope.toggleForm = function () {
                
                $scope.isSignUp = !$scope.isSignUp;
            };
        }]
    };
}]);