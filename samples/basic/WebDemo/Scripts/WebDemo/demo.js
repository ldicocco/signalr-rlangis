
$(function () {
    $('#request').hide();
    $('#request').on('click', function () { addFunc().done(function (res) { $('#result').text(res); }); });
    $('#activate').on('click', function () { lh.activate($('#localHubName').val()); });

    var addFunc = function () {
        return rlangisHubProxy.sendRequest("server01", "add", 2, 40);
    };

    var connection = $.hubConnection();
    var rlangisHubProxy = connection.createRlangisHubProxy();
    rlangisHubProxy.onRlangisConnected(
        function (name, connectionId) { $('#server01').text('CONNECTED ' + name + ' ' + connectionId); $('#request').show(); });
    rlangisHubProxy.onRlangisDisconnected(
        function (name, connectionId) { $('#server01').text('DISCONNECTED ' + name + ' ' + connectionId); $('#request').hide(); });
    var lh = new Ldc.SignalR.Rlangis.LocalHub(rlangisHubProxy);
    lh.onRlangis('testJS', function () { return 84; });
//    alert(lh.getName());
    connection.start().done(function () {
        //alert('Started');
    });
});
