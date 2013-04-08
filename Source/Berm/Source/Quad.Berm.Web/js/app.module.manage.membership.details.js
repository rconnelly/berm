(function() {
    $.app = $.app || {};
    $.app.manage = $.app.manage || {};
    $.app.manage.membership = $.app.manage.membership || {};
    $.app.manage.membership.details = {};
    
    $.app.manage.membership.details.ready = function() {
        $("#Client").change(function() {
            var $client = $(this);
            var $option = $('#Option');
            var clientId = $client.val();
            var mode = $option.val();

            amplify.request({
                resourceId: "get_membership_roles",
                data: { option: mode, clientId: clientId },
                success: function (data) {
                    var $role = $("#Role");
                    $role.empty();

                    if (data.length > 1) {
                        $role.append($('<option></option>')
                            .attr("value", 0)
                            .text(""));
                    }

                    $.each(data, function () {
                        $role.append($('<option></option>')
                            .attr("value", this.Id)
                            .text(this.Name));
                    });
                }
            });
        });
    };

})();