'use strict';

authink.directive('createChild', function() {

    return {
      
        restrict:    'E',
        templateUrl: '/application/templates/createChild',
        scope: {
            
            onChildCreated: '&'
        },
        
        controller: ['$scope', 'childrenRepository', function ($scope, childrenRepository) {

            $scope.child         = {};
            $scope.isServerError = false;

            $scope.createChild = function() {
                
                var child = { firstName: $scope.child.firstName, lastName: $scope.child.lastName };

                childrenRepository.create(child)
                .then(function (newChild) {

                    $scope.onChildCreated({ child: newChild });

                    reserScopeState();

                    $scope.$emit('closeModal');
                }, function (response) {

                    $scope.isServerError = true;
                });
            };

            $scope.closeModal = function() {

                $scope.$emit('closeModal');
            };

            var reserScopeState = function () {

                $scope.child = {};
                $scope.isServerError = false;
            };

            reserScopeState();
        }]
    };
});