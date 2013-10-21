'use strict';

var authink = angular.module('authink', ['ui.bootstrap', 'ngRoute', 'ngResource'])
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        
        $routeProvider.when('/', {
            templateUrl: '/Assets/Templates/Home.html'
        }).when('/login', {
            templateUrl: '/Assets/Templates/Login.html'
        });

        $locationProvider.html5Mode(true);
    }]);

authink.service('application', ['childMenuApi', 'testListApi', 'testPreviewApi', 'editChildApi', 'createTestApi','editTestApi', function (childMenuApi, testListApi, testPreviewApi, editChildApi, createTestApi, editTestApi) {

    return {
        
        childMenuApi:   childMenuApi,
        testListApi:    testListApi,
        testPreviewApi: testPreviewApi,
        editChildApi:   editChildApi,
        createTestApi:  createTestApi,
        editTestApi:    editTestApi
    };
}])