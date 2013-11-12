'use strict';

authink.directive('signUp', ['accountServices', function (accountServices) {

    return {

        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/SignUp.cshtml',
        
        controller: ['$scope', '$location', function ($scope, $location) {

            $scope.login = function() {

                var user = { username: $scope.username, password: $scope.password };

                var response = accountServices.login(user);
                response.then(function(responseData) {
                    
                    if (responseData.StatusCode === 200) {

                        $location.path('/');
                    } else {
                        
                        $scope.loginErrorMessage = "Invalid login or password.";
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

                    if (responseData.StatusCode === 200) {

                        $scope.isSignUp = false;
                    } else {

                        $scope.signUpErrorMessage = "Username or email are already in use. Please choose another one.";
                    }
                });
            };

            $scope.toggleForm = function () {
                
                $scope.isSignUp = !$scope.isSignUp;
            };
        }]
    };
}]);