(function() {
    app.controller("DashboardCtrl", ['$scope', '$anchorScroll', 'DataService', function($scope, $anchorScroll, DataService) {
       ListDashboard();
       $scope.sales = ["Invoices Status","Sales by Agent","Sales by Category","Sales by Region"];
       $scope.charts = ["invoicesStatus","salesByAgent","salesByCategory","salesByRegion"];
       $scope.selectedSale = "Sales By Agent";
       $scope.showFirst = true;
       $scope.showSecond = false;
       
        function ListDashboard() {
            DataService.list("dashboard", function(data) { 
            $scope.dashData = data;

           $scope.name = $scope.selectedSale;
           $scope.showAgent = true;
           
           var maxv = 0;
           agentSales2 = new Array();
           agentLabels2 = new Array();
           annualSales = new Array();
           annualLabels = new Array();
           top5Revenue = new Array();
           top5Name = new Array();
           hotsName = new Array();
           hotsDifference = new Array(); 
           $scope.remove = 0;
           
           angular.forEach(data.hots, function(key, value){      
                hotsDifference.push(key.difference);
                hotsName.push(key.name);
               });
           angular.forEach(data.top5, function(key, value){      
                top5Revenue.push(key.revenue);
                top5Name.push(key.name);
               });
           angular.forEach(data.sales, function(key, value){
               if($scope.remove>0){
                annualSales.push(key);
                 annualLabels.push(value);
               }
                if (key > maxv) maxv = key;
                $scope.remove =1;
            });
            angular.forEach(data.agents, function (key, value) {
                agentSales2.push(key.sales);
                agentLabels2.push(key.label);
            });

                maxv = 100000 * Math.ceil(maxv / 100000);
                var step = maxv / 5;
           var ctx = document.getElementById("burningItems");
            var chart = new Chart(ctx, {
                type: "doughnut",
                data: {
                    labels: hotsName,
                    datasets: [
                        {
                            label: "revenue",
                            data: hotsDifference,
                            backgroundColor: [
                                'rgba(0, 0, 153, 1)',
                                'rgba(79, 171, 201, 1)',
                                'rgba(255, 102, 0, 1)',
                                'rgba(255, 166, 77, 1)',
                                'rgba(0, 153, 0, 1)',
                                'rgba(144, 238, 144, 1)',
                                'rgba(204, 0, 0, 1)',
                                'rgba(255, 179, 179, 1)'
                            ],
                            borderColor: 'rgba(255, 255, 230, 1)',
                            borderWidth: 1,
                            yAxisID: "rev"
                        }]
                },
                options: {
                    responsive:true,
                    padding: 14,
                    title: {display: true, text: "Hots", padding: 8, fontFamily: 'Open Sans', fontSize: 16},
                    legend: {position: "right"},
                    scales: {}
                }
            });     
          var ctx = document.getElementById("top5");
            var chart = new Chart(ctx, {
                type: "doughnut",
                data: {
                    labels: top5Name,
                    datasets: [
                        {
                            label: "revenue",
                            data: top5Revenue,
                            backgroundColor: [
                                'rgba(0, 0, 153, 1)',
                                'rgba(79, 171, 201, 1)',
                                'rgba(255, 102, 0, 1)',
                                'rgba(255, 166, 77, 1)',
                                'rgba(0, 153, 0, 1)',
                                'rgba(144, 238, 144, 1)',
                                'rgba(204, 0, 0, 1)',
                                'rgba(255, 179, 179, 1)'
                            ],
                            borderColor: 'rgba(255, 255, 230, 1)',
                            borderWidth: 1,
                            yAxisID: "rev"
                        }]
                },
                options: {
                    responsive:true,
                    padding: 14,
                    title: {display: true, text: "Top 5", padding: 8, fontFamily: 'Open Sans', fontSize: 16},
                    legend: {position: "right"},
                    scales: {}
                }
            });     
         var ctx = document.getElementById("sales");
            var chart = new Chart(ctx, {
                type: "pie",
                data: {
                    labels: agentLabels2,
                    datasets: [
                        {
                            label: "revenue",
                            data: agentSales2,
                            backgroundColor: [
                                'rgba(0, 0, 153, 1)',
                                'rgba(79, 171, 201, 1)',
                                'rgba(255, 102, 0, 1)',
                                'rgba(255, 166, 77, 1)',
                                'rgba(0, 153, 0, 1)',
                                'rgba(144, 238, 144, 1)',
                                'rgba(204, 0, 0, 1)',
                                'rgba(255, 179, 179, 1)'
                            ],
                            borderColor: 'rgba(255, 255, 230, 1)',
                            borderWidth: 1,
                            yAxisID: "rev"
                        }]
                },
                options: {
                    responsive:true,
                    padding: 14,
                    title: {display: true, text: $scope.name, padding: 8, fontFamily: 'Open Sans', fontSize: 16},
                    legend: {position: "right"},
                    scales: {}
                }
            });
             ctx = document.getElementById("annualSales");
            var myChart = new Chart(ctx, {
                type: "bar",
                data: {
                    labels: annualLabels,
                    datasets: [
                        {
                            label: "revenue",
                            data: annualSales,
                            backgroundColor: 'rgba(0, 0, 153, 0.8)',
                            borderColor: 'rgba(0, 0, 153, 1)',
                            borderWidth: 1,
                            yAxisID: "rev"
                        }]
                },
                options: {
                    padding: 14,
                    title: { display: true, text: "Annual sales", fontFamily:'Open Sans', fontSize:16},
                    legend: { position: "none" },
                    scales: {
                        yAxes: [
                            { type: "linear", id: "rev", display:true, position:"right", ticks: { stepSize: step, min: 0, max: maxv } }
                        ]
                    }
                }});
                 $scope.showFirst = true;
                 $scope.showSecond = false;
            });
        };
        
         $scope.getChart = function(){
            $scope.showFirst = false;   
            $scope.showSecond = true;
            DataService.list("dashboard", function(data) { 
            $scope.dashData = data; 
                
            $scope.salesByRegion = false;
            $scope.salesByCategory = false;
            $scope.salesByAgent = false;
            $scope.invoicesStatus = false;
            $scope.showRegion = false;
            $scope.showAgent = false;
            $scope.showCategory = false;
            $scope.showStatus = false;
            $scope.name = $scope.selectedSale;
            $scope.chart = "";
           Sales = new Array();
           Labels = new Array();
            
            if($scope.name=="Sales by Region"){
                $scope.chart = $scope.charts[3];
                 $scope.salesByRegion = true;
                 $scope.showRegion = true;
            angular.forEach(data.regions, function (key, value) {
               Sales.push(key.sales);
               Labels.push(key.label)
            });
            }
           else if($scope.name=="Sales by Category"){
               $scope.chart = $scope.charts[2];
               $scope.salesByCategory = true;
               $scope.showCategory = true;
            angular.forEach(data.categories, function (key, value) {
               Sales.push(key.sales);
               Labels.push(key.label)
            });
           }
           else if($scope.name=="Sales by Agent"){
               $scope.chart = $scope.charts[1];
               $scope.salesByAgent = true;
               $scope.showAgent = true;
            angular.forEach(data.agents, function (key, value) {
                Sales.push(key.sales);
                Labels.push(key.label);
            });
           }
           else{
               $scope.chart = $scope.charts[0];
               $scope.invoicesStatus = true;
               $scope.showStatus = true;
             angular.forEach(data.invoices, function (key, value) {
                Sales.push(key.count);
                Labels.push(key.status);
            });
           }
        
         var ctx = document.getElementById($scope.chart);
            var chart = new Chart(ctx, {
                type: "pie",
                data: {
                    labels: Labels,
                    datasets: [
                        {
                            label: "revenue",
                            data: Sales,
                            backgroundColor: [
                                'rgba(0, 0, 153, 1)',
                                'rgba(79, 171, 201, 1)',
                                'rgba(255, 102, 0, 1)',
                                'rgba(255, 166, 77, 1)',
                                'rgba(0, 153, 0, 1)',
                                'rgba(144, 238, 144, 1)',
                                'rgba(204, 0, 0, 1)',
                                'rgba(255, 179, 179, 1)',
                                'rgba(255, 255, 0, 1)',
                                'rgba(0, 0, 0, 1)'
                            ],
                            borderColor: 'rgba(255, 255, 230, 1)',
                            borderWidth: 1,
                            yAxisID: "rev"
                        }]
                },
                options: {
                    responsive:true,
                    padding: 14,
                    title: {display: true, text: $scope.name, padding: 8, fontFamily: 'Open Sans', fontSize: 16},
                    legend: {position: "right"},
                    scales: {}
                }
            });
                     
            });
                  
         }
    }]);
}());