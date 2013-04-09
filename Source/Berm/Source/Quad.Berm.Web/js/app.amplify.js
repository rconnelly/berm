amplify.confirm = function (message, operation, data, callback) {
    $.app.confirm(
        message,
        function () {
            amplify.request({
                resourceId: operation,
                data: data,
                success: callback
            });
        }
    );
};

amplify.request.define("get_users", "ajax", {
    url: $.app.settings.baseurl + 'Manage/Membership',
    dataType: "json",
    type: "POST",
    cache: false
});

amplify.request.define("get_membership_roles", "ajax", {
    url: $.app.settings.baseurl + "Manage/Membership/Roles",
    dataType: "json",
    type: "POST",
    cache: false
});

amplify.request.define("delete_user", "ajax", {
    url: $.app.settings.baseurl + "Manage/Membership/Delete",
    dataType: "json",
    type: "POST",
    cache: false,
    decoder: "jsend"
});

