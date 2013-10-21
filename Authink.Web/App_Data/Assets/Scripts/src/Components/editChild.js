'use strict';

authink.directive('editChild', function () {

    return {

        restrict: 'E',
        templateUrl: '/Assets/Templates/Components/EditChild.html',

        controller: ['$scope', 'childrenRepository', 'editChildApi', function ($scope, childrenRepository, editChildApi) {

            $scope.editChildApi = editChildApi;
            
            $scope.$watch('editChildApi.child', function (child) {

                if (child) {
                    
                    $scope.child = {

                        id:                child.Id,
                        firstName:         child.Firstname,
                        lastName:          child.Lastname,
                        profilePictureUrl: child.ProfilePictureUrl
                    };
                }
            });

            $scope.editChild = function () {

                var child = {childId: $scope.child.id, firstName: $scope.child.firstName, lastName: $scope.child.lastName, profilePictureUrl:$scope.child.profilePictureUrl };

                var promise = childrenRepository.edit(child);
                promise.then(function (response) {

                    $scope.$emit('childEditEnded', response.childId);
                    
                    $scope.$emit('closeModal');
                });
            };
        }]
    };
});

authink.factory('editChildApi', ['childrenRepository', function (childrenRepository) {

    return {

        child: null,
        
        setChildToEdit: function(childId) {

            this.child = childrenRepository.getOne_shortDetails(childId);
        }
    };
}]);