﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Hiv_stat_indicators.aspx.cs" Inherits="SOCY_WEBAppTest.Hiv_stat_indicators" %>

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
                            <asp:Button ID="btnLogOut" runat="server" Text="LOGOUT" class="btn-success btn" OnClick="btnLogOut_Click"/><span class="badge"></span></a>
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
                    <div class="col_3">
                        <div class="col-md-3 widget widget1">
                            <div class="r3_counter_box">
                                <i class="pull-left fa fa-users user3 icon-rounded"></i>
                                <div class="stats">
                                    <h5><strong>
                                        <asp:Label ID="lblTotalHouseholds" runat="server" Text=""></asp:Label>
                                    </strong></h5>
                                    <span>Total Households</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 widget widget1">
                            <div class="r3_counter_box">
                                <i class="pull-left fa fa-users user1 icon-rounded"></i>
                                <div class="stats">
                                    <h5><strong>
                                        <asp:Label ID="lblTotalBen" runat="server" Text=""></asp:Label>
                                    </strong></h5>
                                    <span>Total Beneficiaries</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 widget widget1">
                            <div class="r3_counter_box">
                                <i class="pull-left fa fa-dollar dollar1 icon-rounded"></i>
                                <div class="stats">
                                    <h5><strong>
                                        <asp:Label ID="lblTotalSILCGroups" runat="server" Text=""></asp:Label></strong></h5>
                                    <span>Total SILC Groups</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 widget">
                            <div class="r3_counter_box">
                                <i class="pull-left fa fa-dollar dollar1 icon-rounded"></i>
                                <div class="stats">
                                    <h5><strong>
                                        <asp:Label ID="lblSilcMembers" runat="server" Text=""></asp:Label>
                                    </strong></h5>
                                    <span>Silc Grp Members</span>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="col_1">
                        <h4>HIV Stat Indicators</h4>
                    </div>
                    <div class="col-sm-6">
                        <%--Indicators--%>
                        <div class="panel panel-warning" data-widget="{&quot;draggable&quot;: &quot;false&quot;}" data-widget-static="">
                            <div class="panel-body no-padding">
                                <asp:Label ID="lblChildren" runat="server" Text=" HIV STAT 0-17 Yrs" ForeColor="#ff3300"></asp:Label>
                                <asp:GridView ID="gdvHivStat" AutoGenerateColumns="False" class="table table-striped" runat="server" GridLines="None">

                                    <Columns>
                                        <asp:BoundField DataField="HIV_Stat_Indicator" HeaderText="HIV Stat Indicator Name"
                                            SortExpression="HIV_Stat_Indicator"></asp:BoundField>
                                        <asp:BoundField DataField="Total_Beneficiaries" HeaderText="Number of Beneficiaries"
                                            SortExpression="Total_Beneficiaries"></asp:BoundField>
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
                            <div class="panel-body no-padding">
                                <asp:Label ID="lblAdults" runat="server" Text=" HIV STAT 18+ Yrs" ForeColor="#ff3300"></asp:Label>
                               
                                <asp:GridView ID="gdvHivStat_adult" AutoGenerateColumns="False" class="table table-striped" runat="server" GridLines="None">

                                    <Columns>
                                        <asp:BoundField DataField="HIV_Stat_Indicator" HeaderText="HIV Stat Indicator Name"
                                            SortExpression="HIV_Stat_Indicator"></asp:BoundField>
                                        <asp:BoundField DataField="Total_Beneficiaries" HeaderText="Number of Beneficiaries"
                                            SortExpression="Total_Beneficiaries"></asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="warning" Font-Bold="true" />
                                </asp:GridView>
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