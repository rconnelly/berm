$.app = {};
$.app.settings = { baseurl: "/" };

/*
$.app.linkConfirm = function() {
    var href = $(this).attr('href');
    $.app.showConfirmation.showConfirmation(
        $(this).attr('data-confirm'),
        function() {
            document.location = href;
        }
    );
    return false;
};
$(document).on('click', 'a[data-confirm]', $.app.linkConfirm);
*/

$.app.confirm = function (text, callback) {
    if (!$('#dataConfirmModal').length) {
        $('body').append(
            '<div id="dataConfirmModal" class="modal" role="dialog" aria-labelledby="dataConfirmLabel" aria-hidden="true">' +
                '<div class="modal-header">' +
                    '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
                    '<h3 id="dataConfirmLabel">Please Confirm</h3>' +
                '</div>' +
                '<div class="modal-body"></div>' +
                '<div class="modal-footer">' +
                    '<button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button>' +
                    '<button class="btn btn-primary" id="dataConfirmOK">OK</button>' +
                '</div>' +
            '</div>');
    }
    
    $('#dataConfirmModal').find('.modal-body').text(text);
    $('#dataConfirmOK')
        .unbind("click")
        .bind("click", function () {
            $('#dataConfirmModal').modal('hide');
            if (callback !== null) {
                callback();
            }
        });
    $('#dataConfirmModal').modal('show');
    return false;
};

$.app.manage = {};

//---------------------------------------------------------------------
// Membership
//---------------------------------------------------------------------
$.app.manage.membership = {};
$.app.manage.membership.details = {};
$.app.manage.membership.list = {};
$.app.manage.membership.details.ready = function() {
    $("#Client").change(function() {
        var $client = $(this);
        var $option = $('#Option');
        var clientId = $client.val();
        var mode = $option.val();

        $.ajax({
            "dataType": "json",
            "type": "POST",
            "cache": false,
            "url": $.app.settings.baseurl + "Manage/Membership/Roles",
            "data": { "option": mode, "clientId": clientId },
            "success": function(data) {
                var $role = $("#Role");
                $role.empty();

                if (data.length > 1) {
                    $role.append($('<option></option>')
                        .attr("value", 0)
                        .text(""));
                }

                $.each(data, function() {
                    $role.append($('<option></option>')
                        .attr("value", this.Id)
                        .text(this.Name));
                });
            }
        });
    });
};

$.app.manage.membership.list.ready = function() {
    var oTable = $('#userList').dataTable({
        "bServerSide": true,
        "sAjaxSource": $.app.settings.baseurl + 'Manage/Membership',
        "fnServerData": function (sSource, aoData, fnCallback) {
            $.ajax({
                "dataType": "json",
                "type": "POST",
                "cache": false,
                "url": sSource,
                "data": aoData,
                "success": fnCallback,
                "error": function (jqXHR, textStatus, errorThrown) {
                    // todo: error handling required
                    alert(errorThrown);
                }
            });
        },
        "fnRowCallback": function (nRow, aData) {
            if (aData[5] === "True") {
                $(nRow).addClass("user-disabled");
            }
            return nRow;
        },
        "aoColumnDefs": [
            {
                "aTargets": [0],
                "mData": 0,
                "sTitle": "",
                "bSearchable": false,
                "bSortable": false,
                "sWidth": "10px",
                "mRender": function (data, type, full) {
                    var content =
                        '<a title="edit" href="' + $.app.settings.baseurl + 'Manage/Membership/Edit/' + data + '"><i class="icon-edit"></i></a>';
                    return content;

                }
            },
            {
                "aTargets": [1],
                "mData": 0,
                "sName": "Id",
                "sTitle": "Id",
                "bSearchable": false,
                "bSortable": false,
                "bVisible": false
            },
            {
                "aTargets": [2],
                "mData": 1,
                "sName": "Name",
                "sTitle": "Name",
                "mRender": function (data, type, full) {
                    var content = '<a href="' + $.app.settings.baseurl + 'Manage/Membership/Edit/' + full[0] + '">' + data + '</a>';
                    return content;
                }
            },
            {
                "aTargets": [3],
                "mData": 2,
                "sName": "Identifier",
                "sTitle": "Identifier"
            },
            {
                "aTargets": [4],
                "mData": 4,
                "sName": "Client",
                "sTitle": "Client"
            },
            {
                "aTargets": [5],
                "mData": 3,
                "sName": "Role",
                "sTitle": "Role"
            },
            {
                "aTargets": [6],
                "mData": 5,
                "sName": "Disabled",
                "sTitle": "Disabled",
                "bSearchable": false,
                "bSortable": false,
                "sWidth": "60px",
                "sClass": "text-center",
                "mRender": function (data) {
                    return data == "True" ? '<i class="icon-check"></i>' : '';
                }
            },
            {
                "aTargets": [7],
                "mData": 0,
                "sTitle": "",
                "bSearchable": false,
                "bSortable": false,
                "sWidth": "10px",
                "mRender": function (data, type, full) {
                    var content =
                        '<a href="#" title="delete" data-action="deleteUser" data-user-id="' + data + '" data-user-name="' + full[1] + '"><i class="icon-remove"></i></a>';
                    return content;

                }
            }
        ]
    });
    
    var deleteUser = function (id, name, callback) {
        $.app.confirm(
            "Are you sure you want to delete user '" + name + "'?",
            function () {
                $.ajax({
                    "dataType": "json",
                    "type": "POST",
                    "cache": false,
                    "url": $.app.settings.baseurl + "Manage/Membership/Delete/",
                    "data": { "id": id },
                    "success": callback,
                    "error": function (jqXHR, textStatus, errorThrown) {
                        // todo: error handling required
                        alert(errorThrown);
                    }
                });
            }
        );
    };
    
    $(document).on('click', 'a[data-action=deleteUser]', function () {
        var id = $(this).attr('data-user-id');
        var name = $(this).attr('data-user-name');
        deleteUser(id, name, function (data) {
            $('#userActionComplete span').text(data.message);
            $('#userActionComplete').show();
            oTable.fnDraw();
        });

        return false;
    });
};

