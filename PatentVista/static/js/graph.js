$(document).ready(function() {    
    
    
    var currentCurrency = $.cookie("PvSettings");
    if (currentCurrency == undefined) {
        currentCurrency = "EUR";
    }
    checkCurrentCurrency(currentCurrency);
    loadGraph(currentCurrency);

    $("input[name='currency']:radio").change(function () {
        var currency = $(this).val();
        window.location = "?currency=" + currency;
        $.cookie('PvSettings', currency, { expires: 30, path: '/' });
    });

});


function checkCurrentCurrency(currentCurrency) {
    $("#" + $.trim(currentCurrency)).prop('checked', true);
}


function loadGraph(currentCurrency) {

    var siteBaseUrl = $("#siteBaseUrl").text();
    if (siteBaseUrl.charAt(siteBaseUrl.length - 1) == "/") {
        siteBaseUrl = siteBaseUrl.slice(0, -1);
    }
    
    var landId = $("#landId").text();
    var language = $("html").attr("lang");
    var dataServiceUrl = siteBaseUrl + "/country-data-service?landId=" + landId + "&currency=" + currentCurrency + "&lang=" + language;

    
    $.ajax({
        url: dataServiceUrl,
        type: "get",
        dataType: "html",
        success: function (result) {
            
            result = result.trim();
            result = result.substring(0, result.length - 1);
            var data = $.parseJSON(result);
    
            var options = {
                
                scaleOverlay: true,
                scaleShowGridLines: true,
                scaleOverride: true,
                scaleSteps: 10,
                scaleStepWidth: data.StepWidth,
                scaleStartValue: 0,
                scaleLineColor: "#004A80",
                scaleLineWidth: 1,
                scaleShowLabels: true,
                scaleFontSize: 12,
                scaleFontColor: "#000",
                barDatasetSpacing: 4,
                barStrokeWidth: 1
            };
            
            var ctx = $("#myChart").get(0).getContext("2d");
            new Chart(ctx).Bar(data, options);

        },
        error: function(xhr, status, error) {
            alert(status + " : " + error);
        }
        
    });
}