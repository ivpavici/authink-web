'use strict';

authink.directive('signUp', ['accountServices', function (accountServices) {

    return {

        restrict:    'E',
        templateUrl: '/application/templates/signUp',
        
        controller: ['$scope', '$location', function ($scope, $location) {

            $scope.errors = {
                isLoginError: false,
                isSignUpError: false,
                isUsernameOrEmailTakeError: false
            };

            $scope.login = function() {
                var user = { username: $scope.username, password: $scope.password };

                accountServices.login(user)
                .then(function(responseData) {
                    
                    if (responseData.StatusCode === 200) {

                        resetScopeState();

                        $location.path('/');
                    } else {

                        $scope.errors.isloginError = true;
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

                        resetScopeState();
                    } else if(responseData.StatusCode === 417){
                        
                        $scope.errors.isSignUpError = true;
                    } else {
                        $scope.errors.isUsernameOrEmailTakeError = true;
                    }
                });
            };

            var resetScopeState = function () {

                $scope.errors = {
                    isLoginError: false,
                    isSignUpError: false,
                    isUsernameOrEmailTakeError: false
                };
            };
            $scope.toggleForm = function () {
                
                $scope.isSignUp = !$scope.isSignUp;
            };

            resetScopeState();
        }]
    };
}]);