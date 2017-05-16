(function() {
    app.controller("InvoicesCtrl", ['$scope', 'DataService', '$http', function($scope, DataService, $http) {
        $scope.showInvoice = false;
        $scope.showInvoices = false;
        $scope.searchPage = false;
        $scope.pagination = false;
        $scope.number = false;
        $scope.onSubmit = false;
        getShippers('');
        getAgents('');
        ListInvoices(0);
        $scope.selectSearch = "";
        $scope.mailData = {
            invoiceId: 0,
            mailTo: ""
        };
        $scope.selectedCustomer = { id: 0, name: "" };

        $scope.edit = function(invoice) {
            if (invoice.id == 0) {
                $scope.selectedCustomer = { id: 0, name: '' };
            } else {
                $scope.selectedCustomer = { id: invoice.customerId, name: invoice.customer };
            }
            $scope.invoice = invoice;
        };

        $scope.info = function(invoice) {
            DataService.read("invoicereport", invoice.id, function(data) { $scope.invoices = data; })
            $scope.showInvoices = true;
        };
        //--------------------- 
        $scope.hideval = function() {
            $scope.onSubmit = false;
        };
        $scope.printDiv = function(divName) {
            var printContents = document.getElementById(divName).innerHTML;
            var popupWin = window.open('', '_blank', 'width=1000,height=1000');
            popupWin.document.open();
            popupWin.document.write('<html><head><link rel="stylesheet" type="text/css" href="styles/bootstrap.min.css" /><link rel="stylesheet" type="text/css" href="styles/style.css" /></head><body onload="window.print()">' + printContents + '</body></html>');
            popupWin.document.close();
        };

        $scope.saveAsPdf = function(id) {
            DataService.download(id);
        };

        $scope.send = function(invoiceId) {
                $scope.mailData.invoiceId = invoiceId;
                DataService.insert("invoices/mail", $scope.mailData, function(data) {
                    swal("Success!", "Your email is sent to given address!", "success")
                });
            }
            //PDF
        $scope.pdf = function(invoice) {
            html2canvas(document.getElementById("printable"), {
                onrendered: function(canvas) {
                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            image: data,
                            width: 500,
                        }]
                    };
                    pdfMake.createPdf(docDefinition).download(invoice.invoiceNo + "-invoice-report.pdf");
                }
            });
        };
        /*Code to generate pdf ends here*/
        //PDF
        //UPDATE INVOICE
        $scope.save = function() {
            if (!$scope.myForm.$valid) {
                $scope.onSubmit = true;
                // $scope.modal('show');Izbačeno jer ne radi s ovim
            }
            console.log($scope.selectedCustomer);
            $scope.invoice.customerId = $scope.selectedCustomer.id;
            console.log($scope.invoice);
            if ($scope.invoice.id == 0) {
                DataService.insert("invoices", $scope.invoice, function(data) { ListInvoices($scope.currentPage - 1); });
                $('.modal').modal('hide');
            } else {
                DataService.update("invoices", $scope.invoice.id, $scope.invoice, function(data) { ListInvoices($scope.currentPage - 1); });
                $('.modal').modal('hide');
            }
        };

        //CREATE NEW INVOICE
        var dbase = new Date();
        var invGenNum = dbase.valueOf(); //Generate invoice number
        var currentDate = new Date();
        var dateShipped = new Date(new Date(currentDate).setDate(currentDate.getDate() + 5)); // Set default shipping date to current + 5 days.
        $scope.new = function() {
            $scope.invoice = {
                id: 0,
                invoiceNo: invGenNum,
                date: new Date(),
                shippedOn: dateShipped,
                vat: 17
            };
            $scope.showInvoices = true;

            $scope.getTotal = function() {
                var total = 0;
                angular.forEach($scope.invoice.items, function(item) {
                    total += item.quantity * item.price;
                })
                return total;
            }

        };

        $scope.page = 0;
        $scope.search = function(page, direction) {
            DataService.list("invoices/pagination?item=" + $scope.selectSearch + "&page=" + page, function(data) {

                $scope.pagination = false;
                $scope.invoices = data.invoicesList;
                $scope.totalPages = data.totalPages;
                $scope.currentPage = data.currentPage + 1;

                if ($scope.totalPages < 11)
                    $scope.pages = new Array($scope.totalPages);
                else
                    $scope.pages = new Array(10);
                $scope.size = data.size;
                if ($scope.currentPage == $scope.totalPages && $scope.totalPages > 1) {
                    document.getElementById("nextSearch").disabled = true;
                    document.getElementById("previousSearch").disabled = false;
                } else if ($scope.currentPage == 1 && $scope.totalPages == 1) {
                    document.getElementById("previousSearch").disabled = true;
                    document.getElementById("nextSearch").disabled = true;
                } else if ($scope.currentPage == 1) {
                    document.getElementById("previousSearch").disabled = true;
                    document.getElementById("nextSearch").disabled = false;
                } else {
                    document.getElementById("nextSearch").disabled = false;
                    document.getElementById("previousSearch").disabled = false;
                }
                if ($scope.totalPages < 11)
                    for (var i = 0; i < $scope.totalPages; i++)
                        $scope.pages[i] = i + 1;

                else {

                    if ($scope.currentPage <= 5)
                        for (var i = 0; i <= 9; i++)
                            $scope.pages[i] = i + 1;

                    else if ($scope.currentPage + 4 >= $scope.totalPages) {
                        var d = 9 - ($scope.totalPages - $scope.currentPage);
                        for (var i = $scope.currentPage - d; i <= $scope.currentPage + 9 - d; i++)
                            $scope.pages[i - $scope.currentPage + d] = i;
                    } else {
                        var d = 0,
                            tmp = 4;
                        for (var i = $scope.currentPage; i <= $scope.currentPage + 9; i++) {
                            $scope.pages[d] = $scope.currentPage - tmp + d;
                            d++;
                        }
                    }
                }
                if ($scope.totalPages > 0) {
                    $scope.searchPage = true;
                    $scope.number = true;
                } else {
                    $scope.number = false;
                    $scope.searchPage = false;
                }

                console.log($scope.currentPage);
            });
        };

        $scope.checkPage = 1;
        $scope.checkFirst = 0;
        $scope.checkLast = 0;
        $scope.total = 0;
        //PAGINATION
        function ListInvoices(page) {
            DataService.list("invoices?page=" + page, function(data) {

                $scope.checkFirst = 0;
                $scope.checkLast = 0;
                $scope.searchPage = false;
                $scope.invoices = data.invoicesList;
                $scope.totalPages = data.totalPages;
                $scope.currentPage = data.currentPage + 1;
                if($scope.currentPage>1)
                     $scope.checkPage = 0;
                if ($scope.totalPages < 11)
                    $scope.pages = new Array($scope.totalPages);
                else
                    $scope.pages = new Array(10);

                $scope.size = data.size;

                if ($scope.currentPage == $scope.totalPages && $scope.totalPages > 1) {
                    document.getElementById("next").disabled = true;
                    document.getElementById("previous").disabled = false;
                    $scope.checkLast = 1;
                } else if ($scope.currentPage == 1 && $scope.totalPages == 1) {
                    document.getElementById("previous").disabled = true;
                    document.getElementById("next").disabled = true;
                    $scope.checkFirst = 1;
                } else if ($scope.currentPage == 1) {
                    document.getElementById("previous").disabled = true;
                    document.getElementById("next").disabled = false;
                } else {
                    document.getElementById("next").disabled = false;
                    document.getElementById("previous").disabled = false;
                }
                if ($scope.totalPages < 11)
                    for (var i = 0; i < $scope.totalPages; i++)
                        $scope.pages[i] = i + 1;

                else {

                    if ($scope.currentPage <= 5)
                        for (var i = 0; i <= 9; i++)
                            $scope.pages[i] = i + 1;

                    else if ($scope.currentPage + 4 >= $scope.totalPages) {
                        var d = 9 - ($scope.totalPages - $scope.currentPage);
                        for (var i = $scope.currentPage - d; i <= $scope.currentPage + 9 - d; i++)
                            $scope.pages[i - $scope.currentPage + d] = i;
                    } else {
                        var d = 0,
                            tmp = 4;
                        for (var i = $scope.currentPage; i <= $scope.currentPage + 9; i++) {
                            $scope.pages[d] = $scope.currentPage - tmp + d;
                            d++;
                        }
                    }
                }
                if ($scope.totalPages > 0) {
                    $scope.pagination = true;
                    $scope.number = true;
                } else {
                    $scope.number = false;
                    $scope.pagination = false;
                }
                console.log($scope.currentPage);
            });
        }

        //GO TO
          //GO TO
        $scope.goto = function(page, direction) {
            
            var check=0;
                if (direction == -1) {
                    ListInvoices(page - 2);
                    document.getElementById(page - 1).focus();
                    if(page-1==1)
                        check==1;
                } else if (direction == 1) {
                    ListInvoices(page);
                    document.getElementById(page + 1).focus();
                } else
                    ListInvoices(page - 1);
            
                if(page!=0 && checkPage==1)
                    {
                      document.getElementById(1).style.color="#333";
                      document.getElementById(1).style.backgroundColor="#fff";
                      document.getElementById(1).style.borderColor="#ccc";
                      document.getElementById(1).style.boxShadow="none";
                    }
            
            if(check==1)
                 document.getElementById(page - 1).focus();
            
            }
            //END OF ITEM SECTION IN MODAL


        //TYPEAHEAD PRODUCTS AND CUSTOMERS
        var _selected;
        $scope.selected = (undefined);

        $scope.getCustomers = function(name) {
            return $http.get('http://api-delta.gigischool.com/api/customers/' + name).then(function(response) {
                return response.data;
            });
        };

        $scope.getProducts = function(name) {
            return $http.get('http://api-delta.gigischool.com/api/products/' + name).then(function(response) {
                return response.data;
            });
        };

        $scope.ngModelOptionsSelected = function(value) {
            if (arguments.length) {
                _selected = value;
            } else {
                return _selected;
            }
        };

        $scope.modelOptions = {
            debounce: {
                default: 500,
                blur: 250
            },
            getterSetter: true
        };

        //DELETE INVOICE
        $scope.delete = function(invoice) {
            DataService.delete("invoices", invoice.id, function(data) {
                swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this Invoice!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false
                    },
                    function() {
                        ListInvoices();
                        swal("Deleted!", "Invoice has been deleted.", "success");
                    });
            });
            $scope.showInvoices = false;
        };
        //LIST ALL INVOICES
        /*function ListInvoices() {
            DataService.list("invoices", function(data) { $scope.invoices = data });
        };*/

        $scope.statuses = [
            { "-1": "Canceled" },
            { "0": "OrderCreated" },
            { "1": "InvoiceCreated" },
            { "2": "InvoiceSent" },
            { "3": "InvoicePaid" },
            { "4": "InvoiceOnHold" },
            { "5": "InvoiceReady" },
            { "6": "InvoiceShipped" }
        ];

        // Ovaj dio sluzi za view onih sarenih buttona na invoice tabeli
        $scope.states = BillingConfig.stat;
        //--------------------------------------------------------------

        $scope.getKey = function(status) {
            return Object.keys(status)[0];
        }


        //GET AND SAVE ITEMS
        $scope.saveItem = function(item) {
            if (item.id == undefined) {
                item.productId = $scope.selectedProduct.id;
                item.invoiceId = $scope.invoice.id;
                DataService.insert("items", item, function() {
                    DataService.read("invoices", $scope.invoice.id, function(data) {
                        $scope.invoice = data;
                        $scope.newItem = { productId: 0, quantity: 0, price: 0 };
                        $scope.selectedProduct = { id: 0, name: "" };
                    })
                });
            } else {
                DataService.update("items", item.id, item, function() {});
            }
        };

        $scope.removeItem = function(item) {
            DataService.delete("items", item.id, function() {
                DataService.read("invoices", $scope.invoice.id, function(data) { $scope.invoice = data; })
            });
        }

        //LIST/GET ALL SHIPPERS
        function getShippers(name) {
            DataService.list("shippers/" + name, function(data) { $scope.shippers = data });
        };

        //LIST/GET ALL AGENTS
        function getAgents(name) {
            DataService.list("agents", function(data) { $scope.agents = data });
        };

    }]);

}());