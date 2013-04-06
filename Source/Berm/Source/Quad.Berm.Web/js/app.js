$.app = {};
$.app.confirm = function() {
    var href = $(this).attr('href');

    if (!$('#dataConfirmModal').length) {
        $('body').append('<div id="dataConfirmModal" class="modal" role="dialog" aria-labelledby="dataConfirmLabel" aria-hidden="true"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button><h3 id="dataConfirmLabel">Please Confirm</h3></div><div class="modal-body"></div><div class="modal-footer"><button class="btn" data-dismiss="modal" aria-hidden="true">Cancel</button><a class="btn btn-primary" id="dataConfirmOK">OK</a></div></div>');
    }
    $('#dataConfirmModal').find('.modal-body').text($(this).attr('data-confirm'));
    $('#dataConfirmOK').attr('href', href);
    $('#dataConfirmModal').modal({ show: true });
    return false;
};

$.app.manage = {};

$.app.manage.membership = {};
$.app.manage.membership.settings = { roleurl: null };
$.app.manage.membership.ready = function() {
    $("#Client").change(function() {
        var $client = $(this);
        var $option = $('#Option');
        var clientId = $client.val();
        var mode = $option.val();

        $.ajax({
            "dataType": "json",
            "type": "POST",
            "cache": false,
            "url": $.app.manage.membership.settings.roleurl,
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

$(document).on('click', 'a[data-confirm]', $.app.confirm);

