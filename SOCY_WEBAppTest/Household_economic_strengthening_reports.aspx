<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Household_economic_strengthening_reports.aspx.cs" Inherits="SOCY_WEBAppTest.Household_economic_strengthening_reports" %>

<!--
Author: W3layouts
Author URL: http://w3layouts.com
License: Creative Commons Attribution 3.0 Unported
License URL: http://creativecommons.org/licenses/by/3.0/
-->
<!DOCTYPE HTML>
<html>
<head>
    <title>Sustainable Outcomes for Children & Youth | OVC Serve Indicators :: CRS::USAID</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="Modern Responsive web template, Bootstrap Web Templates, Flat Web Templates, Andriod Compatible web template, 
Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyErricsson, Motorola web design" />
    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>
    <!-- Bootstrap Core CSS -->
    <link href="css/bootstrap.min.css" rel='stylesheet' type='text/css' />
    <!-- Custom CSS -->
    <link href="css/style.css" rel='stylesheet' type='text/css' />
    <!-- Graph CSS -->
    <link href="css/lines.css" rel='stylesheet' type='text/css' />
    <link href="css/font-awesome.css" rel="stylesheet">
    <!-- jQuery -->
    <script src="js/jquery.min.js"></script>
    <!----webfonts--->
    <link href='http://fonts.googleapis.com/css?family=Roboto:400,100,300,500,700,900' rel='stylesheet' type='text/css'>
    <!---//webfonts--->
    <!-- Nav CSS -->
    <link href="css/custom.css" rel="stylesheet">
    <!-- Metis Menu Plugin JavaScript -->
    <script src="js/metisMenu.min.js"></script>
    <script src="js/custom.js"></script>
    <!-- Graph JavaScript -->
    <script src="js/d3.v3.js"></script>
    <script src="js/rickshaw.js"></script>

    
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="js/jquery.dynDateTime.min.js" type="text/javascript"></script>
    <script src="js/calendar-en.min.js" type="text/javascript"></script>
    <link href="css/calendar-blue.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
         $(document).ready(function () {
        $("#<%=txtCreateDateFrom.ClientID %>").dynDateTime({
             showsTime: true,
             ifFormat: "%Y/%m/%d %H:%M",
             daFormat: "%l;%M %p, %e %m, %Y",
             align: "BR",
             electric: false,
             singleClick: false,
             displayArea: ".siblings('.dtcDisplayArea')",
             button: ".next()"
         });
         });

            $(document).ready(function () {
        $("#<%=txtCreateDateTo.ClientID %>").dynDateTime({
             showsTime: true,
             ifFormat: "%Y/%m/%d %H:%M",
             daFormat: "%l;%M %p, %e %m, %Y",
             align: "BR",
             electric: false,
             singleClick: false,
             displayArea: ".siblings('.dtcDisplayArea')",
             button: ".next()"
         });
            });
    </script>
</head>
<body>
    <div id="wrapper">
        <form id="frmMain" runat="server">
            <!-- Navigation -->
            <nav class="top1 navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="default.aspx">Sustainable Outcomes for Children & Youth Data Dashboard</a>
                </div>
                <!-- /.navbar-header -->
                <ul class="nav navbar-nav navbar-right">

                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle avatar" data-toggle="dropdown">
                            <img src="images/Logo.jpg"><span class="badge"></span></a>
                    </li>
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle avatar" data-toggle="dropdown">
                            <img src="images/palladium.png"><span class="badge"></span></a>
                    </li>
                    <li class="dropdown">
                        <a href="LogIn.aspx" class="dropdown-toggle avatar">
                            <asp:Button ID="btnLogOut" runat="server" Text="LOGOUT" OnClick="btnLogOut_Click" class="btn-success btn"/><span class="badge"></span></a>
                    </li>

                </ul>
                <div class="navbar-default sidebar" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li>
                                <a href="default.aspx"><i class="fa fa-dashboard fa-fw nav_icon"></i>Dashboard</a>
                            </li>
                        </ul>
                    </div>
                    <!-- /.sidebar-collapse -->
                </div>
                <!-- /.navbar-static-side -->
            </nav>
            <div id="page-wrapper">
                <div class="graphs">
                
                    <div class="col_1">
                        <h4 style="margin-left:100px">Household Economic Strengthening:Dashboard Reports</h4>
                    </div>

                     <div class="tab-content">
                        <div class="tab-pane active" id="horizontal-form">
                            <div id="frmMain2" class="form-horizontal">

                                 <div class="form-group">
                                    <label for="cboCSO" class="col-sm-2 control-label">
                                        <asp:Label ID="lblcso" runat="server" Text="Select CSO:"></asp:Label></label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="cboCSO" class="form-control1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="cboDistrict" class="col-sm-2 control-label">
                                        <asp:Label ID="lbldistrict" runat="server" Text="Select District:"></asp:Label></label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="cboDistrict" class="form-control1" runat="server" OnSelectedIndexChanged="cboDistrict_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>

                                    <label for="cboReportname" class="col-sm-2 control-label">
                                        <asp:Label ID="Label1" runat="server" Text="Select Category:"></asp:Label></label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="cboReportname" class="form-control1" OnSelectedIndexChanged="cboReportname_SelectedIndexChanged" runat="server" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>

                                 <div id="datediv" class="form-group" runat="server">

                                        <label for="txtCreateDateFrom" class="col-sm-2 control-label"><asp:Label ID="lblDateFrom" runat="server" Text="Date between:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtCreateDateFrom" class="form-control1" runat="server"></asp:TextBox></div>
                                        <label for="txtCreateDateTo" class="col-sm-2 control-label"><asp:Label ID="lblDateTo" runat="server" Text="To:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtCreateDateTo" class="form-control1" runat="server"></asp:TextBox></div>
                                    </div>

                                 <div class="form-group" style="margin-bottom:1px" runat="server">

                                        <label for="txtCreateDateFrom" class="col-sm-2 control-label"><asp:Label ID="Label2" Visible="false" runat="server" Text="Date between:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="TextBox1" Visible="false" class="form-control1" runat="server"></asp:TextBox></div>
                                        <label for="txtCreateDateTo" class="col-sm-2 control-label"><asp:Label ID="Label3" Visible="false" runat="server" Text="To:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:Button ID="btnsearch" runat="server" class="btn-success btn" OnClick="btnsearch_Click" BackColor="#66ffcc" Text="Load Data" /></div>
                                    </div>

                            </div>

                        </div>
                    </div>

                    <div class="col-sm-6">
                        <%--Indicators--%>
                        <div class="panel panel-warning" data-widget="{&quot;draggable&quot;: &quot;false&quot;}" data-widget-static="">
                            <div class="panel-body no-padding" style="height:400px">
                                <asp:Label ID="lblGridHeader" runat="server" Text="" ForeColor="#ff3300"></asp:Label>
                                <asp:GridView ID="gdvHES" AutoGenerateColumns="False" class="table table-striped" runat="server" AllowPaging="true" GridLines="None">
                                    <Columns>
                                        <asp:BoundField DataField="sct_name" HeaderText="Sub County"
                                            SortExpression="sct_name"></asp:BoundField>
                                        <asp:BoundField DataField="Total" HeaderText="Total Number"
                                            SortExpression="Total"></asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="warning" Font-Bold="true" />
                                </asp:GridView>
                            </div>
                        </div>
                        <%-- Indicators--%>

                        <div class="clearfix"></div>
                    </div>

                     <div class="col-sm-6">
                        <%--Indicators--%>
                        <div class="panel panel-warning" data-widget="{&quot;draggable&quot;: &quot;false&quot;}" data-widget-static="">
                            <div class="panel-body no-padding" style="height:400px">
                                <asp:Label ID="lblgraphHeader" runat="server" Text="" ForeColor="#ff3300"></asp:Label>
                                <asp:RadioButton ID="rdnBar" Text="Bar Chart" runat="server" OnCheckedChanged="rdnBar_CheckedChanged" AutoPostBack="true" /> <asp:RadioButton ID="rdnPie" Text="Pie Chart" OnCheckedChanged="rdnPie_CheckedChanged" AutoPostBack="true" runat="server" /><asp:RadioButton ID="rdnLine" Text="Line Chart" AutoPostBack="true" OnCheckedChanged="rdnLine_CheckedChanged" runat="server" />
                                <asp:Chart ID="char_HES" runat="server" Height="300px" Width="500px" Visible = "false">
                                <Titles>
                                    <asp:Title ShadowOffset="3" Name="Items" />
                                </Titles>
                                <Legends>
                                    <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
                                </Legends>
                                <Series>
                                    <asp:Series Name="Default" />
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                                </ChartAreas>
                            </asp:Chart>
                            </div>
                        </div>
                        <%-- Indicators--%>

                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-sm-8 col-sm-offset">
                                <asp:Button ID="btndownload" runat="server" class="btn-success btn" Text="Download Report(Excel)" />
                            </div>
                        </div>
                    </div>

                    <div class="copy">
                        <p>Copyright © 2017 SOCY. All Rights Reserved | <a href="https://www.crs.org/" target="_blank">CRS</a>| Designed by <a href="http://thepalladiumgroup.com/" target="_blank">Palladium Group</a>| Powered by <a href="http://w3layouts.com/" target="_blank">W3layouts</a></p>
                    </div>
                </div>
            </div>
            <!-- /#page-wrapper -->
        </form>
    </div>
    <!-- /#wrapper -->
    <!-- Bootstrap Core JavaScript -->
    <script src="js/bootstrap.min.js"></script>
</body>
</html>
