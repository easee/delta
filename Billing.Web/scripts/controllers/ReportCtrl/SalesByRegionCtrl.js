(function() {
    app.controller("SalesByRegionCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
        $scope.showRegion = false;
        $scope.showPrint = false;
        $scope.showHeader = false;
        $scope.showHeader2 = false;
        $scope.save = function() {
            DataService.insert("SalesByRegion", $scope.requestData, function(data) {
                $scope.salesbyRegiondata = data;
                $scope.showRegion = true;
                $scope.showPrint = true;
            });
        }

        // $scope.agent.id = $scope.requestData.id;
        $scope.agentSales = function(currentAgent) {
                            $scope.requestData.id=currentAgent;
                            DataService.insert("salesbyagent", $scope.requestData, function(data){
                                $scope.salesByAgentData = data;
                        })
                
            };
        $scope.printDiv = function (divName,data) {
            if (data == undefined) data = 0;
          for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    document.getElementById(obj.regionName).style.visibility="hidden";
               }
          
           for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    for(var j=0;j<data[i].agents.length;j++)
                        {
                            var object=data[i].agents[j];
                            var name=object.agentName+object.totalPercent;
                            document.getElementById(name).style.visibility="hidden";   
                        }          
               }
          for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    var object=obj.regionName+obj.regionTotal;
                    document.getElementById(object).style.visibility="hidden";
               }
           var printContents = document.getElementById(divName).innerHTML;
           var popupWin = window.open('', '_blank', 'width=1000,height=1000');
           if(divName=="printable")
           $scope.showHeader = true;
            else if(divName=="printable2")
            $scope.showHeader2 = true;
           popupWin.document.open();
           popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="styles/bootstrap.min.css" /><link rel="stylesheet" type="text/css" href="styles/style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
           popupWin.document.close();
           $scope.showHeader = false;
           $scope.showHeader2 = false;
           for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    document.getElementById(obj.regionName).style.visibility="visible";
               }
          
           for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    for(var j=0;j<data[i].agents.length;j++)
                        {
                            var object=data[i].agents[j];
                            var name=object.agentName+object.totalPercent;
                            document.getElementById(name).style.visibility="visible";   
                        }          
               }
          for(var i=0;i<data.length;i++)
               {
                    var obj=data[i];
                    var object=obj.regionName+obj.regionTotal;
                    document.getElementById(object).style.visibility="visible";
               }
       };
       //Angular UI Datepicker JS must have
        $scope.open1 = function () {
            $scope.popup1.opened = true;
        };
        $scope.open2 = function () {
            $scope.popup2.opened = true;
        };

        $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[2];
        $scope.altInputFormats = ['M!/d!/yyyy'];

        $scope.popup1 = {
            opened: false
        };
        $scope.popup2 = {
            opened: false
        };

    }]);
}());