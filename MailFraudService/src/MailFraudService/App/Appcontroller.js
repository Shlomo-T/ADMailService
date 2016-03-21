(function () {
    'use strict';
    //Just an angular test
    angular
        .module('app')
        .controller('Appcontroller', Appcontroller);

    Appcontroller.$inject = ['$scope']; 

    function Appcontroller($scope) {
        $scope.title = 'Appcontroller';

        activate();

        function activate() { }
    }
})();
