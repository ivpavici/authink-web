'use strict';

authink.directive('childMenu', ['childrenRepository', 'childMenuApi', function (childrenRepository, childMenuApi) {

    return {

        restrict:    'E',
        templateUrl: '/Assets/Templates/Components/ChildMenu.html',
        scope:       {},
        
        controller: ['$scope', function ($scope) {

            $scope.childMenuApi = childMenuApi;
            
            $scope.childMenuApi.loadChildren();
            
            $scope.childMenuApi.children.then(function (children) {

                if (children.length > 0) {
                    
                    $scope.childMenuApi.setDisplayedChild(children[0].Id);

                    $scope.$emit('childSelected', children[0].Id);
                } else {
                    
                    var component = '<create-child> </create-child>';

                    $scope.$emit('openModal', component, 'static');
                }
            });

            $scope.$watch('childMenuApi.children', function (children) {

                $scope.children = children;
            });
            
            $scope.$watch('childMenuApi.displayedChild', function (displayedChild) {

                $scope.displayedChild = displayedChild;
            });
            
            $scope.selectChild = function(child) {

                if ($scope.isTestEditModeOn) {

                    $scope.$emit('testEditCanceled');
                }
                
                $scope.$emit('childSelected', child.Id);
            };
            
            $scope.addChild = function () {

                var component = '<create-child> </create-child>';
                
                $scope.$emit('openModal', component);
            };

            $scope.editChild = function () {
                
                var component = '<edit-child> </edit-child>';

                $scope.$emit('childEditStarted', $scope.displayedChild.Id);
                
                $scope.$emit('openModal', component);
            };
        }]
    };
}]);

authink.factory('childMenuApi', ['childrenRepository', function (childrenRepository) {

    return {
        
        children:       null,
        displayedChild: null,
        
        loadChildren: function () {
            
            this.children = childrenRepository.getAllForUser_shortDetails();
        },
        
        setDisplayedChild: function (childId) {
            
            this.displayedChild = childrenRepository.getOne_shortDetails(childId);
        }
    };
}]);