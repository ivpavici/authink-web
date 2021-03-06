'use strict';

authink.controller('shellController', ['$rootScope', '$cookies', '$modal', 'application', function ($rootScope, $cookies, $modal, application) {

    $rootScope.currentModalInstances = [];
    $rootScope.isTestEditModeOn      = false;

    $rootScope.activeLanguage = $cookies.AuLanguage;

    $rootScope.$on('testsList:testCreatingStarted', function (event, childId) {

        application.createTestApi.setChildToAddTests(childId);
    });
    $rootScope.$on('testsList:testCreated',         function (event, test) {

        application.testListApi.addNewTest(test);
    });
    $rootScope.$on('testsList:testSelected',        function (event, test) {

        if (test.Id) {
            application.testPreviewApi.setActiveTest(test.Id);

            application.testTasksListApi.loadTasks(test.Id);
        } else {

            application.testPreviewApi.reset();
        }
    });
    $rootScope.$on('testsList:testEditCanceled',    function (event) {
        
        $rootScope.isTestEditModeOn = false;
    });
    
    $rootScope.$on('testPreview:testEditStarted', function (event, test) {

        $rootScope.isTestEditModeOn = true;
        
        application.editTestApi.testToEdit = test;
    });

    $rootScope.$on('editTest:testEditEnded', function (event, test) {

        $rootScope.isTestEditModeOn = false;

        application.testPreviewApi.setActiveTest(test.Id);

        application.testListApi.refreshTests();
    });
    $rootScope.$on('editTest:testDeleted',   function (event) {

        $rootScope.isTestEditModeOn = false;

        application.testListApi.removeDisplayedTest();

        application.testListApi.refreshTests();

        application.testPreviewApi.reset();

    });
    $rootScope.$on('editTest:editCanceled', function (event) {
    
        $rootScope.isTestEditModeOn = false;
    });

    $rootScope.$on('editTask:taskEditEnded',     function (event) {

        application.testTasksListApi.refreshTasks();

        application.taskPreviewApi.refresh();
    });
    $rootScope.$on('editTask:taskForEditLoaded', function (event, task) {

        application.editTaskPicturesListApi.setupPicturesList(task.Type, task.Id, task.Pictures);
    });

    $rootScope.$on('testTasksList:taskSelected',    function (event, taskId) {

        application.taskPreviewApi.taskId = taskId;
    });
    $rootScope.$on('testTasksList:taskEditStarted', function (event, taskId) {

        application.editTaskApi.taskId = taskId;
    });
    
    $rootScope.$on('editTaskPicturesList:taskPictureEditStarted', function (event, taskType, taskId, picture) {

        application.taskPicturesEditorApi.setupPictureEditor(taskType, taskId, picture);
    });
    
    $rootScope.$on('taskPicturesEditor:pictureUpdated', function(event) {

        application.editTaskPicturesListApi.forceRefresh();
    });
    
    $rootScope.$on('childMenu:childSelected', function (event, childId) {

        application.testListApi.reset();

        $rootScope.isTestEditModeOn = false;

        application.childMenuApi.setDisplayedChild(childId);
        
        application.testListApi.setChildId(childId);
    });
    $rootScope.$on('childMenu:childEditStarted', function (event, childId) {

        application.editChildApi.setChildToEdit(childId);
    });
    $rootScope.$on('childMenu:childDeleted', function (event) {

        window.location='/';
    });

    $rootScope.$on('createChild:childCreated', function (event, child) {

        application.testPreviewApi.reset();

        application.childMenuApi.addNewChild(child);
    });

    $rootScope.$on('editChild:childEditEnded', function (event, childId) {

        application.childMenuApi.loadChildren();
        
        application.childMenuApi.setDisplayedChild(childId);
    });
    $rootScope.$on('editChild:pictureUpdated', function (event, childId) {

        application.childMenuApi.setDisplayedChild(childId);
    });
        
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