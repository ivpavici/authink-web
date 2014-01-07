'use strict';

authink.controller('shellController', ['$rootScope', '$scope', '$cookies', '$modal', function ($rootScope, $scope, $cookies, $modal) {

    $rootScope.currentModalInstances = [];
    $rootScope.isTestEditModeOn      = false;

    $rootScope.activeLanguage = $cookies.AuLanguage;

    $scope.onTestCreatingStarted = function(childId){

        $scope.createTestApi.init(childId);
    };

    $scope.onTestCreated = function (test) {

        $scope.testsListApi.addNewTest(test);
    }

    $scope.onTestSelected = function (test) {

        if (test.Id) {
            $scope.testPreviewApi.setActiveTest(test.Id);

            $scope.testTasksListApi.init(test.Id);
        } else {

            $scope.testPreviewApi.reload();
        }
    }

    $scope.onTestEditCanceled = function () {

        $rootScope.isTestEditModeOn = false;
    }
    
    $scope.onTestEditStarted = function (test) {

        $rootScope.isTestEditModeOn = true;

        $scope.editTestApi.init(test);
    }
    $scope.onTestEditEnded = function (test) {

        $rootScope.isTestEditModeOn = false;

        $scope.testPreviewApi.setActiveTest(test.Id);

        $scope.testListApi.reload();
    }
    $scope.onTestEditDeleted = function () {

        $rootScope.isTestEditModeOn = false;

        $scope.testListApi.removeDisplayedTest();

        $scope.testListApi.reload();

        $scope.testPreviewApi.reset();
    }
    $scope.onTestEditCanceled = function() {
        
        $rootScope.isTestEditModeOn = false;
    }

    $scope.onTaskEditEnded = function () {

        $scope.testTasksListApi.reload();

        $scope.taskPreviewApi.refresh();
    }
    $scope.onLoaded = function (task) {

        $scope.editTaskPicturesListApi.init(task.Type, task.Id, task.Pictures);
    }

    $scope.onTaskEditStarted = function (taskId) {

        $scope.taskPreviewApi.init(taskId);
    }
    $scope.onTaskEditStarted = function(taskId) {
        $scope.editTaskApi.init(taskId);
    }

    $scope.onTaskPictureEditStarted = function (taskType, taskId, picture) {

        $scope.taskPicturesEditorApi.init(taskType, taskId, picture);
    }
    $scope.onPictureUpdated = function() {
        $scope.editTaskPicturesListApi.reload();
    }
    
    $scope.onChildSelected = function (childId) {

        $rootScope.isTestEditModeOn = false;

        $scope.$watch('testsListApi', function (testsListApi) {
            if (testsListApi) {

                $scope.testsListApi.init(childId);
            }
        });
    }
    $scope.onChildEditStarted = function (childId) {

        $scope.editChildApi.init(childId);
    }
    $scope.onChildDeleted = function() {

        window.location = '/';
    }

    $scope.onChildCreated = function (child) {

        $scope.testPreviewApi.reset();

        $scope.childMenuApi.reload(child);
    }

    $scope.onChildEditEnded = function (childId) {

        $scope.childMenuApi.reload();

        $scope.childMenuApi.setDisplayedChild(childId);
    }
    $scope.onPictureUpdated = function (childId) {

        $scope.childMenuApi.setDisplayedChild(childId);
    }

    $rootScope.$on('closeModal', function (event) {
        
        var currentInstance = $rootScope.currentModalInstances.pop();

        currentInstance.close();
    });
    $rootScope.$on('openModal',  function(event, component, backdrop) {

        var modal = $rootScope.showDialog(component, backdrop);
        
        $rootScope.currentModalInstances.push(modal);
    });
    
    $rootScope.showDialog = function (componentToShow, backdrop) {
        
        if (!backdrop) {
            
            backdrop = true;
        }
        
        var modalInstance = $modal.open({
            
            backdrop:      backdrop,
            keyboard:      true,
            backdropClick: true,
            
            template: componentToShow
        });

        return modalInstance;
    };
}])