<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_data_entry_tracker_v2_view.aspx.cs" Inherits="SOCY_WEBAppTest._data_entry_tracker_v2_view" %>

<!--
Author: W3layouts
Author URL: http://w3layouts.com
License: Creative Commons Attribution 3.0 Unported
License URL: http://creativecommons.org/licenses/by/3.0/
-->
<!DOCTYPE HTML>
<html>
<head>
    <title>Data Entry Tracker| SOCY :: CRS:Palladium</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>
    <!-- Bootstrap Core CSS -->
    <link href="css/bootstrap.min.css" rel='stylesheet' type='text/css' />
    <!-- Custom CSS -->
    <link href="css/style.css" rel='stylesheet' type='text/css' />
    <link href="css/font-awesome.css" rel="stylesheet">
    <!-- jQuery -->
    <script src="js/jquery.min.js"></script>
    <!----webfonts--->
    <link href='http://fonts.googleapis.com/css?family=Roboto:400,100,300,500,700,900' rel='stylesheet' type='text/css'>
    <!---//webfonts--->
    <!-- Bootstrap Core JavaScript -->
    <script src="js/bootstrap.min.js"></script>

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="js/jquery.dynDateTime.min.js" type="text/javascript"></script>
    <script src="js/calendar-en.min.js" type="text/javascript"></script>
    <link href="css/calendar-blue.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txt_date.ClientID %>").dynDateTime({
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
                    <a href="LogIn.aspx" class="dropdown-toggle avatar"></li>

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
                <div class="xs">
                    <h3>Weekly Data Entry Tracker</h3>
                    <div class="tab-content">
                        <div class="tab-pane active" id="horizontal-form">
                            <form runat="server" id="parent" class="form-horizontal">

                                 <div class="form-group">
                                    <label for="cbo_district" class="col-sm-2 control-label">District</label>
                                    <div class="col-sm-2">
                                        <asp:DropDownList ID="cbo_district" AutoPostBack="true" CssClass="form-control1" runat="server"></asp:DropDownList>

                                    </div>

                                      <div class="col-sm-2">
                                        <asp:TextBox ID="txt_date" CssClass="form-control1" placeholder="Enter Tracker Date" runat="server"></asp:TextBox>
                                         
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <label for="cbo_district" class="col-sm-2 control-label"></label>
                                    <div class="col-sm-8">
                                        <asp:Button ID="btnsave" CssClass="btn-success btn" runat="server" Text="Search"/>
                                    </div>

                                </div>

                                  <div class="form-group">
                                    <div class="col-sm-12">

                                         <asp:GridView ID="gdvDataEntry" runat="server" AllowPaging="True" AutoGenerateColumns="false" OnPageIndexChanging="gdvDataEntry_PageIndexChanging"  PageSize="10" class="table table-bordered">
                                    <Columns>
                                        
                                        <asp:BoundField DataField="dst_name" HeaderText="District"
                                            SortExpression="dst_name"></asp:BoundField>

                                        <asp:TemplateField HeaderText="Edit Tracker">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnldte_tracker" Text='Edit'  CommandArgument='<%# Eval("tracker_id") %>' OnClick="GetTrackerDetails" runat="server">Data Entry Tracker</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="tracker_date" HeaderText="Tracker Date"
                                            SortExpression="tracker_date"></asp:BoundField>


                                        <asp:BoundField DataField="um_name" HeaderText="Uploaded by"
                                            SortExpression="um_name"></asp:BoundField>

                                         <asp:TemplateField HeaderText="File Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnltracker_id" Text='<%# Eval("tracker_id") %>' runat="server">LinkButton</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnldownload" Text="Download"  CommandArgument='<%# Eval("tracker_id") %>' OnClick="DownloadFile"  runat="server">Download</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>


                </div>
            </div>
            <div class="copy_layout">
                <p>Copyright © 2017 SOCY. All Rights Reserved | <a href="https://www.crs.org/" target="_blank">CRS</a>| Designed by <a href="http://thepalladiumgroup.com/" target="_blank">Palladium Group</a>| Powered by <a href="http://w3layouts.com/" target="_blank"></a></p>
            </div>
        </div>
        <!-- /#page-wrapper -->
    </div>
    <!-- /#wrapper -->
    <!-- Nav CSS -->
    <link href="css/custom.css" rel="stylesheet">
    <!-- Metis Menu Plugin JavaScript -->
    <script src="js/metisMenu.min.js"></script>
    <script src="js/custom.js"></script>
</body>
</html>