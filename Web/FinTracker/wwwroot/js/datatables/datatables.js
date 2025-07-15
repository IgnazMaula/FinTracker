window.addEventListener('DOMContentLoaded', event => {

    window.initializeDataTables = function (tableId) {
        var datatables = document.getElementById(tableId);

        if (tableId == "defaultTable") {
            const dataTable = new DataTable(datatables, {
                responsive: true,
                drawCallback: function () {
                    window.feather.replace();
                }
            });
        }
        else if (tableId == "dashboardTable") {
            const dataTable = new DataTable(datatables, {
                responsive: true,
                pageLength: 100,
                searching: false,
                order: [],
                columnDefs: [
                    { targets: '_all', orderable: false }
                ]
            });
        }
        else if (tableId == "bankTransactionTable") {
            const dataTable = new DataTable(datatables, {
                responsive: true,
                order: [],
                columnDefs: [
                    { targets: '_all', orderable: false }
                ]
            });
        }

        else if (tableId == "dcaTable") {
            const dataTable = new DataTable(datatables, {
                responsive: true,
                pageLength: 100,
                searching: false,
                order: [],
                columnDefs: [
                    { targets: '_all', orderable: false }
                ]
            });
        }

        else if (tableId == "coinPriceSummaryTable") {
            const dataTable = new DataTable(datatables, {
                responsive: true,
                paging: false,
                lengthChange: false,
                pageLength: 8,
                searching: false,
                order: [],
                columnDefs: [
                    { targets: '_all', orderable: false }
                ]
            });
        }

        //Add another type of table here

        else {
            const dataTable = new DataTable(datatables, {
                responsive: true,
                drawCallback: function () {
                    window.feather.replace();
                }
            });
        }
    }

});


