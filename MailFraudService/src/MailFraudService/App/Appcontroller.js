(function () {
    'use strict';

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
