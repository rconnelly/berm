(function() {
    $.app = $.app || {};
    $.app.manage = $.app.manage || {};
    $.app.manage.membership = $.app.manage.membership || {};
    $.app.manage.membership.details = {};

    function UserDetailsView() {
        var self = this;
        self.mode = ko.observable($('#Option').val());
        self.client = ko.observable($('#Client').val());
        self.role = ko.observable($('#Role').val());
        self.availableRoles = ko.observableArray();
        
        self.roleCaption = ko.computed(function () {
            var text = null;
            if (this.availableRoles().length > 1) {
                text = 'Choose...';
            }

            return text;
        }, self);
        
        self.updateRoles = function () {
            amplify.request({
                resourceId: "get_membership_roles",
                data: { option: self.mode(), clientId: self.client() },
                success: function (data) {
                    var current = self.role();
                    var isCurrentPresent = false;
                    
                    self.availableRoles.removeAll();
                    $.each(data, function () {
                        isCurrentPresent = isCurrentPresent || this.Id == current;
                        self.availableRoles.push(this);
                    });
                    
                    if (data.length > 1) {
                        if (isCurrentPresent) {
                            self.role(current);
                        } else {
                            self.role(null);
                        }
                    }
                }
            });
        };
    }

    $.app.manage.membership.details.model = new UserDetailsView();
    
    $.app.manage.membership.details.ready = function () {
        $.app.manage.membership.details.model.updateRoles();
        ko.applyBindings($.app.manage.membership.details.model);
    };

})();