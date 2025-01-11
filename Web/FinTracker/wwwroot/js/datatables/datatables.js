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


