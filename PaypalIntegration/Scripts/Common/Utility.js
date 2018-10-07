
//Ajax related  functions
//#Ajax-related-functions
function CallActionByAjax(inType, inURL, inParameters, successCallback, errorCallBack, inAsync) {
    //show loading... image
    inType = inType.toUpperCase() != "GET" &&
        inType.toUpperCase() != "POST" &&
        inType.toUpperCase() != "PUT" &&
        inType.toUpperCase() != "DELETE" ? "POST" : inType;

    inAsync = IsEmpty(inAsync) ? true : inAsync;
    errorCallBack = IsEmpty(errorCallBack) ? commonAjaxErrorBlock : errorCallBack;

    $.ajax({
        type: inType,
        url: inURL,
        data: inParameters,
        async: inAsync,
        cache: false,
        contentType: 'application/json;',
        //dataType: 'json',
        success: successCallback,
        error: errorCallBack,
        //    function (xhr, textStatus, errorThrown) {
        //    console.log('error');
        //}
    });
}


function commonAjaxErrorBlock(xhr, textStatus, errorThrown) {
    if (xhr.status == 403) {
        location.href = "AccessDenied/Index";
    }
    else {
        ShowWarningModal("Error - " + errorThrown + ": Please inform to Admin about error", xhr.responseText);
    }
}


//Getting thee query value.
function GetQueryStringValueByParameter(name, url) {
    try {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    }
    catch (err) {
        return null;
    }
}