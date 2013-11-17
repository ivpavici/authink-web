'use strict';

authink.directive('createChild', function() {

    return {
      
        restrict:    'E',
        templateUrl: '/application/templates/createChild',
        
        controller: ['$scope', 'childrenRepository', function ($scope, childrenRepository) {

            $scope.createChild = function() {
                
                var child = { firstName: $scope.child.firstName, lastName: $scope.child.lastName };

                childrenRepository.create(child)
                .then(function (newChild) {

                    $scope.$emit('createChild:childCreated', newChild);

                    $scope.$emit('childMenu:childSelected', newChild.Id);

                    $scope.$emit('closeModal');
                });
            };
        }]
    };
});