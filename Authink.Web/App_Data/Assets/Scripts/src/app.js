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