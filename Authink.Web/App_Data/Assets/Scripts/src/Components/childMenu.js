'use strict';

authink.directive('childMenu', function () {

    return {

        restrict:    'E',
        templateUrl: '/application/templates/childMenu',
        scope:       {},
        
        controller: ['$scope', 'childMenuApi', 'childrenRepository', function ($scope, childMenuApi, childrenRepository) {

            $scope.childMenuApi = childMenuApi;

            $scope.$watch('childMenuApi.needLoad', function (needLoad) {

                if(needLoad){

                    childrenRepository.getAllForUser_shortDetails()
                    .then(function(children){

                        $scope.childMenuApi.children = children;
                        $scope.childMenuApi.needLoad = false;
                    });
                }
            });
            
            $scope.$watch('childMenuApi.childId', function (childId) {

                if (childId){

                    childrenRepository.getOne_shortDetails(childId)
                    .then(function (child) {

                        $scope.childMenuApi.displayedChild = child;
                    });
                }
            });
            
            $scope.selectChild = function(child) {

                if ($scope.isTestEditModeOn) {

                    $scope.$emit('testsList:testEditCanceled');
                }
                
                $scope.$emit('childMenu:childSelected', child.Id);
            };
            
            $scope.addChild = function () {

                var component = '<create-child> </create-child>';
                
                $scope.$emit('openModal', component);
            };

            $scope.editChild = function () {
                
                var component = '<edit-child> </edit-child>';

                $scope.$emit('childMenu:childEditStarted', $scope.childMenuApi.displayedChild.Id);
                
                $scope.$emit('openModal', component);
            };

            var resetState = function () {

                childrenRepository.getAllForUser_shortDetails()
                   .then(function (children) {

                       if (children.length > 0) {

                           $scope.childMenuApi.setDisplayedChild(children[0].Id);

                           $scope.$emit('childMenu:childSelected', children[0].Id);
                       } else {

                           var component = '<create-child> </create-child>';

                           $scope.$emit('openModal', component, 'static');
                       }

                       $scope.childMenuApi.children = children;
                   })
            };

            resetState();
        }]
    };
});

authink.factory('childMenuApi', function () {

    return {

        childId:        null,
        needLoad:       false,
        children:       null,
        displayedChild: null,
        
        loadChildren: function () {
            
            this.needLoad = true;
        },
        
        setDisplayedChild: function (childId) {
            
            this.childId = new Number(childId);
        },

        addNewChild: function (child) {

            this.children.push(child);
        }
    };
});