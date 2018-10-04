<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChartSample.aspx.cs" Inherits="SOCY_WEBAppTest.ChartSample" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="//cdn.jsdelivr.net/excanvas/r3/excanvas.js" type="text/javascript"></script>
    <script src="//cdn.jsdelivr.net/chart.js/0.2/Chart.js" type="text/javascript"></script>
    <form id="form1" runat="server">
    <script type="text/javascript">
        $(function () {
            LoadChart();
            $("[id*=ddlCountries]").bind("change", function () {
                LoadChart();
            });
            $("[id*=rblChartType] input").bind("click", function () {
                LoadChart();
            });
        });
        function LoadChart() {
            var chartType = parseInt($("[id*=rblChartType] input:checked").val());
            $.ajax({
                type: "POST",
                url: "ChartSample.aspx/GetChart",
                data: "{status: '" + $("[id*=ddlCountries]").val() + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    $("#dvChart").html("");
                    $("#dvLegend").html("");
                    var data = eval(r.d);
                    var el = document.createElement('canvas');
                    $("#dvChart")[0].appendChild(el);

                    //Fix for IE 8
                    if ($.browser.msie && $.browser.version == "8.0") {
                        G_vmlCanvasManager.initElement(el);
                    }
                    var ctx = el.getContext('2d');
                    var userStrengthsChart;
                    switch (chartType) {
                        case 1:
                            userStrengthsChart = new Chart(ctx).Pie(data);
                            break;
                        case 2:
                            userStrengthsChart = new Chart(ctx).Doughnut(data);
                            break;
                    }
                    for (var i = 0; i < data.length; i++) {
                        var div = $("<div />");
                        div.css("margin-bottom", "10px");
                        div.html("<span style = 'display:inline-block;height:10px;width:10px;background-color:" + data[i].color + "'></span> " + data[i].text);
                        $("#dvLegend").append(div);
                    }
                },
                failure: function (response) {
                    alert('There was an error.');
                }
            });
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                Country:

                <asp:DropDownList ID="ddlCountries" runat="server">
                    <asp:ListItem Text="Active" Value="1" />
                </asp:DropDownList>

                <asp:RadioButtonList ID="rblChartType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Pie" Value="1" Selected="True" />
                    <asp:ListItem Text="Doughnut" Value="2" />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>
                <div id="dvChart">
                </div>
            </td>
            <td>
                <div id="dvLegend">
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
