'use strict';

var authink = angular.module('authink', ['ui.bootstrap', 'ngRoute', 'ngResource', 'ngCookies', 'angularFileUpload'])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        
        $routeProvider.when('/', {

            templateUrl: '/application/templates/home'
        })
        .when('/login', {

            templateUrl: '/application/templates/login'
        });
        
        $locationProvider.html5Mode(true);
    }]);

authink.service('application', ['childMenuApi', 'testListApi', 'testPreviewApi', 'editChildApi', 'createTestApi', 'editTestApi', 'taskPreviewApi', 'testTasksListApi', 'editTaskApi', 'editTaskPicturesListApi', 'taskPicturesEditorApi', function (childMenuApi, testListApi, testPreviewApi, editChildApi, createTestApi, editTestApi, taskPreviewApi, testTasksListApi, editTaskApi, editTaskPicturesListApi, taskPicturesEditorApi) {

    return {
        
        childMenuApi:            childMenuApi,
        testListApi:             testListApi,
        testPreviewApi:          testPreviewApi,
        editChildApi:            editChildApi,
        createTestApi:           createTestApi,
        editTestApi:             editTestApi,
        taskPreviewApi:          taskPreviewApi,
        testTasksListApi:        testTasksListApi,
        editTaskApi:             editTaskApi,
        editTaskPicturesListApi: editTaskPicturesListApi,
        taskPicturesEditorApi:   taskPicturesEditorApi
    };
}])