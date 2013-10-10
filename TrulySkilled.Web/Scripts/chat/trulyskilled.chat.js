/// <reference path="../jquery/jquery-1.10.2.js" />
/// <reference path="../knockout/knckout-2.3.0.js" />

window.chat = window.chat || (function () {
    var hub = null;
    var vm = null;

    // ------- View Model --------
    function chatModel(name, message) {
        var self = this;
        self.name = name;
        self.message = message;
    }
    // -------- End View Model --------

    function addMessage(message) {
        if (vm.messages().length > 10) {
            vm.messages.shift();
        }
        vm.messages.push(message);
    }

    // -------- Event Handlers --------
    function handleMessageSubmit() {
        var message = $("#chat-input").val();
        if (message && message.length > 0) {
            hub.server.submitMessage(message);
            $("#chat-input").val(null);
        }
    }
    // --------- End Event Handlers ---------

    return {
        init: function (viewModel, hubRef) {
            viewModel.messages = ko.observableArray([]);
            hubRef.client.addMessage = function (name, message) {
                // called when a chat message is added
                console.log('Adding message ' + message + ' from ' + name);
                addMessage(new chatModel(name, message));
            };

            hub = hubRef;
            vm = viewModel;

            $("#submit").click(handleMessageSubmit);
            $("#chat-input").on("keypress", function (e) {
                if (e.keyCode == 13) {
                    handleMessageSubmit();
                    return false;
                }
            });
        },
        start: function (chatName) {
            $("#submit").removeAttr('disabled');
            $("#chat-input").removeAttr('disabled');

            hub.server.joinChatGroup(chatName + '-chat');
        }
    }
}());