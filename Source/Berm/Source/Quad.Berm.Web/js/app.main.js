$.app = {};
$.app.settings = { baseurl: "/" };

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
                    '<button class="btn" data-dismiss="modal" aria-hidden="true">No</button>' +
                    '<button class="btn btn-primary">Yes</button>' +
                '</div>' +
            '</div>');
    }
    
    $('#dataConfirmModal')
        .find('.modal-body')
            .text(text)
            .end()
        .find('.btn-primary')
            .one('click', function () {
                $(this).parents(".modal").modal('hide');
                if (callback !== null) {
                    callback();
                }
            })
            .end()
        .modal('show');
    return false;
};