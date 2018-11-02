<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="default.aspx.cs" Inherits="SOCY_WEBAppTest._default" %>

<!--
Author: W3layouts
Author URL: http://w3layouts.com
License: Creative Commons Attribution 3.0 Unported
License URL: http://creativecommons.org/licenses/by/3.0/
-->
<!DOCTYPE HTML>
<html>
<head>
    <title>Sustainable Outcomes for Children & Youth | Home :: CRS::USAID</title>
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

    <script type="text/javascript" src="js/fusioncharts.js"></script>
    <script type="text/javascript" src="js/themes/fusioncharts.theme.fusion.js"></script>
    <script type="text/javascript" src="js/themes/fusioncharts.theme.gammel.js"></script>
    <script type="text/javascript" src="js/themes/fusioncharts.theme.candy.js"></script>
    <script type="text/javascript" src="js/themes/fusioncharts.theme.zune.js"></script>
    <script type="text/javascript" src="js/themes/fusioncharts.theme.ocean.js"></script>
    <script type="text/javascript" src="js/themes/fusioncharts.theme.carbon.js"></script>

    <style type="text/css">
        g[class$='creditgroup'] {
             display:none !important;
        }
    </style>

    <!-- Bootstrap -->
    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <link rel="stylesheet" href='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css'
        media="screen" />
    <!-- Bootstrap -->

    <script type="text/javascript">

        function getNode_position(Node_id) {
            alert(Node_id)
          <%-- document.getElementById('<%=hidden.ClientID %>').value = Node_id;--%>

            '<% CodeBehind(); %>';
           // ShowPopup();

       }

        function ShowPopup() {
            $("#MyPopup").modal("show");
        }
    </script>


</head>
<body>
    <div id="wrapper">
        <form id="frmMain" runat="server">
            <asp:HiddenField ID="hidden" runat="server" />
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
                            <asp:Button ID="btnLogOut" runat="server" class="btn-success btn" Text="LOGOUT" OnClick="btnLogOut_Click" /><span class="badge"></span></a>
                    </li>

                </ul>


                <div class="navbar-default sidebar" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li>
                                <a href="default.aspx"><i class="fa fa-dashboard fa-fw nav_icon"></i>Dashboard</a>
                            </li>

                            <li>
                                <a href="CapturedDataReports.aspx?reportid=Data Management Dashoboard"><i class="fa fa-check-square-o nav_icon"></i>Data Downloads</a>
                            </li>

                            <%-- Admin--%>
                            <li style="visibility: hidden">
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>
                                    <asp:Label ID="lblAdmin" runat="server" Text="Administration"></asp:Label>
                                    <span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="Cluster_data_capture_offices.aspx">Cluster Data Capture Offices</a>
                                    </li>
                                    <li>
                                        <a href="User.aspx">Create User</a>
                                    </li>
                                    <li>
                                        <a href="UserSearch.aspx">Search User</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>
                            <%--End Admin--%>
                        </ul>
                    </div>
                    <!-- /.sidebar-collapse -->
                </div>
                <!-- /.navbar-static-side -->

            </nav>
            <div id="page-wrapper">
                <div class="graphs">

                    <div class="panel-footer" style="margin-bottom: 0px"></div>


                    <div class="col-md-6  stats-info" style="margin-left: 0px; height: 400px">

                        <div class="panel-body" style="height: 400px">
                             <asp:Literal ID="lit_households_served" runat="server"></asp:Literal>
                            
                        </div>
                        <div class="clearfix"></div>
                    </div>


                    <div class="col-md-6  stats-info" style="margin-left: 0px; height: 400px">

                        <div class="panel-body" style="height: 400px">
                            <asp:Literal ID="lit_ben_served" runat="server"></asp:Literal>
                           
                        </div>
                        <div class="clearfix"></div>
                    </div>

                    <div class="clearfix"></div>

                    <div class="col-md-6  stats-info" style="margin-left: 0px; height: 400px">

                        <div class="panel-body" style="height: 400px">
                            <asp:Literal ID="lit_active_household_members" runat="server"></asp:Literal>
                        </div>
                        <div class="clearfix"></div>
                    </div>


                    <div class="col-md-6  stats-info" style="margin-left: 0px; height: 400px">

                        <div class="panel-body" style="height: 400px">
                            <asp:Literal ID="lit_active_households" runat="server"></asp:Literal>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <div class="clearfix"></div>

                    <!-- Modal Popup -->
                    <div id="MyPopup" class="modal fade" role="dialog">
                        <div class="modal-dialog">
                            <!-- Modal content-->
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal">
                                        &times;</button>
                                    <h4 class="modal-title">
                                        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h4>
                                </div>
                                <div class="modal-body">
                                    <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false" Font-Names="Arial"
                                        Font-Size="10pt" RowStyle-BackColor="#A1DCF2" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White">
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width="150px" DataField="CustomerID" HeaderText="CustomerID" />
                                            <asp:BoundField ItemStyle-Width="150px" DataField="ContactName" HeaderText="ContactName" />
                                            <asp:BoundField ItemStyle-Width="150px" DataField="City" HeaderText="City" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                                        Close</button>
                                </div>
                            </div>
                        </div>
                    </div>



                    <div class="copy" style="margin-top: 5px">
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

