// Jquery-5LMT: 5 Levels of Media Type
// Wraps jquery's Ajax with 5LMT implementation to read content type 
// In this case, no need to use JSON.stringify when calling ajax with JSON Object


(function (parent, $) {


    var oldAjax = $.ajax,
        contentTypeName = "Content-Type";

    $.ajax = function (topSettings) {
        topSettings = topSettings || {};
        var originalBeforeSend = topSettings.beforeSend;

        topSettings.beforeSend = function (jqXHR, settings) {
            if (originalBeforeSend)
                originalBeforeSend(jqXHR, settings);
            addFiveLevelsOfMediaType(jqXHR, settings);
        };

        if (topSettings.data && topSettings.dataType && topSettings.dataType.toLowerCase() == "json") {
            topSettings.processData = false;
        }

        oldAjax(topSettings);

    };

    function addFiveLevelsOfMediaType(jqXHR, settings) {

        if (settings.data && settings.dataType && settings.dataType.toLowerCase() == "json") {
            settings.data = JSON.stringify(settings.data);
        }

        jqXHR.setRequestHeader(contentTypeName,
            "application/json;domain-model=" +
            (settings.data.constructor.domainModel || settings.data.constructor.name));

    }

})(window, $);
