'use strict';

authink.directive('createChild', function() {

    return {
      
        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/CreateChild.html',
        
        controller: ['$scope', 'childrenRepository', function ($scope, childrenRepository) {

            $scope.createChild = function() {
                
                var child = { firstName: $scope.child.firstName, lastName: $scope.child.lastName };

                var promise = childrenRepository.create(child);
                promise.then(function(response) {

                    if(response.childId) {

                        $scope.$emit('childSelected', response.childId);

                        $scope.$emit('closeModal');
                    }
                });
            };
        }]
    };
});