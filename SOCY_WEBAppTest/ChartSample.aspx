<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChartSample.aspx.cs" Inherits="SOCY_WEBAppTest.ChartSample" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pie Chart Asp.net with database ms sqlserver</title>
    <script src="js/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="js/Chart.js" type="text/javascript"></script>

    <script type="text/javascript">
                $(document).ready(function () {

                    $("btnGeneratePieChart").on('click', function (e) {
                        e.preventDefault();
                        var gData = [];
                        gData[0] = $("#txtYear1").val();
                        gData[1] = $("#txtYear2").val();

                        var jsonData = JSON.stringify({
                            gData: gData
                        });
                        $.ajax({
                            type: "POST",
                            url: "WebService1.asmx/getTrafficSourceData",
                            data: jsonData,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: OnSuccess_,
                            error: OnErrorCall_
                        });

                        function OnSuccess_(response) {
                            var aData = response.d;
                            var arr = [];
                            $.each(aData, function (inx, val) {
                                var obj = {};
                                obj.color = val.color;
                                obj.value = val.value;
                                obj.label = val.label;
                                arr.push(obj);
                            });
                            var ctx = $("#myChart").get(0).getContext("2d");
                            var myPieChart = new Chart(ctx).Bar(arr);
                        }

                        function OnErrorCall_(response) { }
                        e.preventDefault();
                    });
                });
            </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>

            <asp:TextBox ID="txtYear1" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtYear2" runat="server"></asp:TextBox>
            <button id="btnGeneratePieChart">Show</button>
            <div>
                <canvas id="myChart" width="500" height="400"></canvas>
            </div>
        </div>
    </form>
</body>
</html>
