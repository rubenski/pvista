$(document).ready(function() {

    $("#euro").attr("checked", true);
    
    loadGraph();

    $("input[name='currency']:radio").change(function() {
        loadGraph();
    });

});



function loadGraph() {

    var currency = $("input[name='currency']:checked").val();

    var siteBaseUrl = $("#siteBaseUrl").text();
    if (siteBaseUrl.charAt(siteBaseUrl.length - 1) == "/") {
        siteBaseUrl = siteBaseUrl.slice(0, -1);
    }
    
    var landId = $("#landId").text();

    $.ajax({
        url: siteBaseUrl + "/country-data-service?landId=" + landId + "&currency=" + currency,
        type: "get",
        dataType: "html",
        success: function (result) {

            result = result.trim();

            result = result.substring(0, result.length - 1);
            var data = $.parseJSON(result);

            var maxTakse = data.MaxTakse;

            var steps = Math.ceil(maxTakse / 100);
            
            if (steps < 5) {
                steps = 5;
            }
            
            var options = {
                
                scaleOverlay: true,
                scaleShowGridLines: true,
                scaleOverride: true,
                scaleSteps: steps,
                scaleStepWidth: 100,
                scaleStartValue: 0,
                scaleLineColor: "rgba(0,0,0,.1)",
                scaleLineWidth: 1,
                scaleShowLabels: true,
                scaleFontSize: 12,
                scaleFontColor: "#000",
                barDatasetSpacing: 4,
                barStrokeWidth: 1
            };
            
            var ctx = $("#myChart").get(0).getContext("2d");
            //This will get the first returned node in the jQuery collection.
            new Chart(ctx).Bar(data, options);
            
        },
        error: function(xhr, status, error) {
            alert(status + " : " + error);
        }
        
    });
}