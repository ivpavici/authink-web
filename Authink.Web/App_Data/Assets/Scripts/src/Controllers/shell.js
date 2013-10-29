﻿'use strict';

authink.controller('shellController', ['$rootScope', '$modal', 'application', function ($rootScope, $modal, application) {

    $rootScope.currentModalInstance = null;
    $rootScope.isTestEditModeOn     = false;
    
    $rootScope.$on('testSelected', function (event, testId) {

        application.testPreviewApi.setActiveTest(testId);

        application.testTasksListApi.loadTasks(testId);
    });
    $rootScope.$on('testEditStarted', function(event, test) {

        $rootScope.isTestEditModeOn = true;
        
        application.editTestApi.testToEdit = test;
    });
    $rootScope.$on('testEditEnded', function(event) {

        $rootScope.isTestEditModeOn = false;
        
        application.testListApi.refreshTests();
    });
    $rootScope.$on('testEditCanceled', function (event) {
        
        $rootScope.isTestEditModeOn = false;
    });
    $rootScope.$on('testDeleted', function(event) {

        $rootScope.isTestEditModeOn = false;

        application.testListApi.refreshTests();

        application.testPreviewApi.reset();
    });

    $rootScope.$on('taskSelected', function(event, taskId) {

        application.taskPreviewApi.taskId = taskId;
    });
    $rootScope.$on('taskEditStarted', function(event, taskId) {

        application.editTaskApi.taskId = taskId;
    });
    $rootScope.$on('taskForEditLoaded', function(event, task) {

        application.editTaskPicturesListApi.setupPicturesList(task.Type, task.Id, task.Pictures);
    });
    $rootScope.$on('taskEditEnded', function(event) {

        application.testTasksListApi.refreshTasks();
    });

    $rootScope.$on('taskPictureEditStarted', function(event, taskType, taskId, picture) {

        application.taskPicturesEditorApi.setupPictureEditor(taskType, taskId, picture);
    });
    
    $rootScope.$on('taskPicturesEditor:pictureUpdated', function(event) {

        application.editTaskPicturesListApi.forceRefresh();
    });
    
    $rootScope.$on('testListChanged', function (event, childId){
        
        application.testListApi.loadTests(childId);
    });
    
    $rootScope.$on('testCreatingStarted', function (event, childId) {

        application.createTestApi.setChildToAddTests(childId);
    });

    $rootScope.$on('childEditStarted', function(event, childId) {

        application.editChildApi.setChildToEdit(childId);
    });
    $rootScope.$on('childEditEnded', function (event, childId) {

        application.childMenuApi.loadChildren();
        
        application.childMenuApi.setDisplayedChild(childId);

        application.testListApi.loadTests(childId);
    });
    $rootScope.$on('childSelected', function (event, childId) {

        application.childMenuApi.setDisplayedChild(childId);
        
        application.testListApi.loadTests(childId);
        
        application.testPreviewApi.reset();
    });
    
    $rootScope.$on('closeModal', function (event) {
        
        $rootScope.currentModalInstance.close();
    });
    $rootScope.$on('openModal', function(event, component, backdrop) {

        var modal = $rootScope.showDialog(component, backdrop);
        
        $rootScope.currentModalInstance = modal;
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