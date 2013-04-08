(function() {
    $.app = $.app || {};
    $.app.manage = $.app.manage || {};
    $.app.manage.membership = $.app.manage.membership || {};
    $.app.manage.membership.list = {};

    $.app.manage.membership.list.model = {
        successMessage: ko.observable(),
        
        hideAlert: function() {
            this.successMessage(null);
        },
        
        deleteUser: function (id, name) {
            var self = this;
            amplify.confirm(
                "Are you sure you want to delete user '" + name + "'?",
                "delete_user",
                { id: id },
                function (data) {
                    self.successMessage(data.message);
                    $.event.trigger("userdeleted");
                });
        }
    };

    $.app.manage.membership.list.ready = function () {
        ko.applyBindings($.app.manage.membership.list.model);

        var aoColumnDefs = [
            {
                "aTargets": [0],
                "mData": 0,
                "sTitle": "",
                "bSearchable": false,
                "bSortable": false,
                "sWidth": "10px",
                "mRender": function(data) {
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
                "mRender": function(data, type, full) {
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
                "mRender": function(data) {
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
                "mRender": function(data, type, full) {
                    var content =
                        '<a href="#" title="delete" data-bind="click: deleteUser.bind($data, \'' + data + '\', \'' + full[1] + '\')"><i class="icon-remove"></i></a>';
                    return content;

                }
            }
        ];

        var oTable = $('#userList').dataTable({
            "bServerSide": true,
            "sAjaxSource": "get_users",
            "fnServerData": function (sSource, aoData, fnCallback) {
                amplify.request({
                    resourceId: sSource,
                    data: jQuery.param(aoData),
                    success: fnCallback
                });
            },
            "fnRowCallback": function (nRow, aData) {
                if (aData[5] === "True") {
                    $(nRow).addClass("user-disabled");
                }
                return nRow;
            },
            "fnDrawCallback": function (oSettings) {
                ko.applyBindings($.app.manage.membership.list.model, document.getElementById(oSettings.sTableId));
            },
            "aoColumnDefs": aoColumnDefs
        });        
        
        $(document).on('userdeleted', oTable.fnDraw);
    };

})();




