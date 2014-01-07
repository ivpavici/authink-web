'use strict';

authink.directive('childMenu', function () {

    return {

        restrict:    'E',
        templateUrl: '/application/templates/childMenu',
        scope: {
            
            api: '=',
            onChildEditStarted: '&',
            onChildDeleted: '&',
            onChildSelected:'&'
        },
        
        controller: ['$scope', '$modal', 'childrenRepository', function ($scope, $modal, childrenRepository) {

            $scope.childId        = null;
            $scope.needLoad       = false;
            $scope.children       = null;
            $scope.displayedChild = null;

            childrenRepository.getAllForUser_shortDetails()
            .then(function (children) {

                if (children.length > 0) {

                    $scope.displayedChild = children[0];

                    $scope.onChildSelected({childId:children[0].Id});
                } else {

                    var component = '<create-child> </create-child>';

                    $scope.$emit('openModal', component, 'static');
                }

                $scope.children = children;
            })

            $scope.reload = function () {

                childrenRepository.getOne_shortDetails(childId)
                   .then(function (child) {

                       $scope.displayedChild = child;
                   });
            }

            $scope.addNewChild = function (child) {
                
                $scope.children.push(child);
            }

            $scope.selectChild = function(child) {

                //if ($scope.isTestEditModeOn) {

                //    $scope.$emit('testsList:testEditCanceled');
                //}

                $scope.displayedChild = child;

                $scope.onChildSelected({ childId: child.Id });
            };
            
            $scope.addChild = function () {

                var component = '<create-child on-child-created="onChildCreated(child)"> </create-child>';
                
                $scope.$emit('openModal', component);
            };

            $scope.editChild = function () {
                
                var component = '<edit-child api="editChildApi" on-child-edit-ended="onChildEditEnded(childId)" on-picture-updated="onPictureUpdated(childId)"> </edit-child>';

                $scope.onChildEditStarted({childId:$scope.displayedChild.Id});
                
                $scope.$emit('openModal', component);
            };

            $scope.removeChild = function () {

                var modal = $modal.open({
                
                    templateUrl: '/application/templates/childDeleteConfirmDialog',
                    scope: $scope
                });
                
                $scope.removeAndExit = function () {
                
                    childrenRepository.remove($scope.displayedChild)
                    .then(function (response) {
                
                        if (response.StatusCode === 200) {

                            $scope.onChildDeleted();

                            modal.close();
                        } else {

                            $scope.isServerError = true;
                        }
                    });
                };
                
                $scope.exit = function () {
                
                    modal.close();
                }
                
                $scope.close = function () {
                
                    modal.close();
                };

                $scope.isServerError = false;
            };

            var resetState = function () {

                $scope.childId = null;
                $scope.needLoad = false;
                $scope.children = null;
                $scope.displayedChild = null;
                $scope.isServerError = false;

            };

            $scope.api = {
                
                reload: $scope.reload,
                setDisplayedChild: $scope.setDisplayedChild
            }

            resetState();
        }]
    };
});