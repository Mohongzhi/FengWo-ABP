Vue.component('todo-item', {
    template: '<li>\
             <a  class="waves-effect waves-block" v-on:click="readMessage(url)">\
              <div class="icon-circle bg-light-green">\
               <i class="material-icons">{{ icon }}</i>\
                </div>\
                  <div class="menu-info">\
                       <h4>{{ title }}</h4>\
                            <p>\
                               <i class="material-icons">access_time</i> {{ creationTime }}\
                            </p>\
                  </div>\
              </a>\
            </li>\
  ',
    props: ['title', 'icon', 'url', 'creationTime']
});
var objNotification = {
    labelCount: '', datas: []
};

new Vue({
    el: '#nav-notification',
    data: objNotification
});

var chatHub = null;

abp.signalr.startConnection(abp.appPath + 'signalr-chatHub', function (connection) {
    chatHub = connection; // Save a reference to the hub

    connection.on('getNotificationMessage', function () { // Register for incoming messages
        getMessages();
    });

}).then(function (connection) {
    abp.event.trigger('chatHub.connected');
});

abp.event.on('chatHub.connected', function () { // Register for connect event
    getMessages();
});

function getMessages() {
    abp.services.app.message.getMyMessages(null).done(function (data) {
        objNotification.datas = data;
        if (data.length > 0) {
            objNotification.labelCount = data.length;
            abp.notify.info('收到' + data.length + '条新消息,请查看右上角小铃铛', { delay: 10000 });
        }
        else {
            objNotification.labelCount = '';
        }
    });
}

function readMessage(url) {
    abp.services.app.message.removeMessage(url, null).done(function () {
        window.location.href = url;
    });
}
$(function () {
    var menus = $('.ml-menu li a');
    menus.on('click', function () {
        $.blockUI({ message: $('.page-loader-wrapper') });
    });

    $('#btnSaveResetPassword').click(function () {
        var currentPassword = $('#CurrentPassword').val();
        var newPassword = $('#NewPassword').val();
        var dto = {};
        dto.currentPassword = currentPassword;
        dto.newPassword = newPassword;
        abp.services.app.user.changePassword(dto, null).done(function (data) {
            if (data) {
                abp.message.success('更改密码成功');
            }
        });
    });
});