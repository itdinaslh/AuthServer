$(document).ready(function () {
    loadTable();
});

function loadContent() {
    loadTable();
}

function loadTable() {
    $('#UserTable').DataTable().clear().destroy();
    $('#UserTable').DataTable({
        processing: false,
        serverSide: true,
        lengthMenu: [5,10,25,50],
        filter: true,
        orderMulti: false,
        ajax: {
            url: "/api/users/all",
            type: "POST",
            dataType: "json"
        },
        columns: [
            { data: "id", name: "id", autoWidth: true },
            { data: "userName", name: "userName", autoWidth: true },
            { data: "fullName", name: "fullName", autoWidth: true },
            { data: "email", name: "fullName", autoWidth: true },
            {
                render: function (data, type, row) { return "<a class='btn btn-sm btn-outline-success mr-2 showMe' style='width:100%;' href='/manage/users/roles/?userId=" + row.id + "'><i class='fal fa-edit'></i> Manage Roles</button>" }
            }
        ],
        order: [[0, "desc"]]
    })
}