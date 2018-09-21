﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="data_entry_tracker.aspx.cs" Inherits="SOCY_WEBAppTest.data_entry_tracker" %>

<!--
Author: W3layouts
Author URL: http://w3layouts.com
License: Creative Commons Attribution 3.0 Unported
License URL: http://creativecommons.org/licenses/by/3.0/
-->
<!DOCTYPE HTML>
<html>
<head>
    <title>Sustainable Outcomes for Children & Youth | Reports :: CRS::USAID</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="keywords" content="Modern Responsive web template, Bootstrap Web Templates, Flat Web Templates, Andriod Compatible web template, 
Smartphone Compatible web template, free webdesigns for Nokia, Samsung, LG, SonyErricsson, Motorola web design" />
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
    <form id="frmMain" runat="server">
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
                    <a class="navbar-brand" href="index.html">Sustainable Outcomes for Children & Youth Data Dashboard</a>
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
                            <asp:Button ID="btnLogOut" runat="server" class="btn-success btn" OnClick="btnLogOut_Click" Text="LOGOUT" /><span class="badge"></span></a>
                    </li>
                </ul>
                <div class="navbar-default sidebar" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li>
                                <a href="default.aspx"><i class="fa fa-dashboard fa-fw nav_icon"></i>Dashboard</a>
                            </li>

                            <%-- Household data reports--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Household data reports<span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=OVCIdentification">OVC ident. & prioritization</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=Household">Household data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdMember">Household member data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdAssessment">HAT data(Archive)</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdAssessment_New">HAT data(New)</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdAssessmentMember">HAT member data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdReferral">Household refferal data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HomeVisit">Home visit data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HomeVisitMember">Home visit member data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HomeVisitArcive">Home visist archive data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HIP">Household improvement plan</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=RASM">Risk assessment register</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=Linkages">Linkages for ES</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SocialWorker">Social worker data</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>
                            <%-- Household data reports--%>

                            <%--Result Area one and 2 Reports--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Result Area 1 & 2 Reports<span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=ActivityTraining">Activity training</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=ApprenticeshipRegister">Apprenticeship register</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=ServiceRegister">Service register</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=AlternativeCarePanel">Alternative care panel</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=CBSDResourceAllocation">CBSD resource allocation</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=CBSDStaffAppraisalTracking">CBSD staff appraisal tracking</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=DistrictOVCCheckList">District OVC check list</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=InstitutionalCareSummary">Institutional care summary</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=CommunityTrainingRegister">Community Training Register</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>

                            <%--Result Area one and 2 Reports--%>


                            <%--Dreams and Silk--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Dreams & SILC Reports<span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=DREAMSEnrolment">DREAMS enrolment</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SILCGroups">SILC groups</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SILCGroupMembers">SILC group members</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SILCFinancialRegister">SILC financial register</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SILCSavingsRegister">SILC savings register</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>
                            <%-- Dreams and Silk--%>

                            <%--Dreams and Silk--%>

                            <%--  Indicators--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>
                                    <asp:Label ID="Label1" runat="server" Text="Indicator Reports"></asp:Label>
                                    <span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="OvcServe_Indicators.aspx">OVC service indicators</a>
                                    </li>
                                    <li>
                                        <a href="Hiv_stat_indicators.aspx">HIV stat indicators</a>
                                    </li>
                                    <li>
                                        <a href="Ovc_refferal_indicators.aspx">OVC refferal completion indicators</a>
                                    </li>
                                    <li>
                                        <a href="Households_beneficiaries_stats.aspx">Household & beneficiary numbers</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>

                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>
                                    <asp:Label ID="Label2" runat="server" Text="Quick Dashboard Reports"></asp:Label>
                                    <span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="Household_economic_strengthening_reports.aspx">Household Economic Strengthening</a>
                                    </li>

                                    <li>
                                        <a href="Result_area_three_dashboard_reports.aspx">Result Area 3 </a>
                                    </li>

                                    <li>
                                        <a href="Result_area_three_reports_refferals.aspx">Result Area 3 : Referrals </a>
                                    </li>

                                    <li>
                                        <a href="Dreams_dashboard_reports.aspx">Dreams </a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>
                            <%-- Indicators--%>

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
                        <h3>
                            <asp:Label ID="lblHeader" runat="server" Text="SOCY District Data Entry Tracker:"></asp:Label><asp:Label ID="lblReportTitle" runat="server" Text="" Font-Bold="true"></asp:Label></h3>
                        <div class="tab-content">
                            <div class="tab-pane active" id="horizontal-form">
                                <div id="frmMain2" class="form-horizontal">

                                    <div class="form-group">

                                        <label for="cbpPartner" class="col-sm-2 control-label">
                                            <asp:Label ID="lblPartner" runat="server" Text="Partner:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboPartner" class="form-control1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <label for="cboRegion" class="col-sm-2 control-label">
                                            <asp:Label ID="lblCSO" runat="server" Text="Region:"></asp:Label>
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboRegion" class="form-control1" runat="server" OnSelectedIndexChanged="cboRegion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="cboDistrict" class="col-sm-2 control-label">
                                            <asp:Label ID="lblDistrict" runat="server" Text="District:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboDistrict" class="form-control1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div id="datediv" class="form-group" runat="server">

                                        <label for="txtCreateDateFrom" class="col-sm-2 control-label">
                                            <asp:Label ID="lblDateFrom" runat="server" Text="Weekly Date From:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtCreateDateFrom" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                        <label for="txtCreateDateTo" class="col-sm-2 control-label">
                                            <asp:Label ID="lblDateTo" runat="server" Text="Weekly Date To:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtCreateDateTo" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label for="FileUploadControl" class="col-sm-2 control-label">
                                            <asp:Label ID="lblupload" runat="server" Text="Browse Excel file:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:FileUpload ID="FileUploadControl" class="form-control1" ToolTip="Browse for the file" accept=".xlsx" runat="server" />
                                        </div>
                                    </div>

                                    <div class="panel-footer">
                                        <div class="row">
                                            <div class="col-sm-8 col-sm-offset-2">
                                                <asp:Button ID="btnupload" runat="server" class="btn-success btn" OnClick="btnupload_Click" Text="Upload File" />
                                                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" class="btn-default btn" Text="Clear Content" />
                                            </div>
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <div class="panel-body">
                               
                                <asp:GridView ID="gdvDataEntry" runat="server" AllowPaging="True" AutoGenerateColumns="false" OnRowDataBound="gdvDataEntry_RowDataBound" OnPageIndexChanging="gdvDataEntry_PageIndexChanging" PageSize="10" class="table table-bordered">
                                    <Columns>
                                        <asp:BoundField DataField="prt_name" HeaderText="Partner"
                                            SortExpression="dst_name"></asp:BoundField>

                                        <asp:BoundField DataField="dst_name" HeaderText="District"
                                            SortExpression="dst_name"></asp:BoundField>

                                        <asp:TemplateField HeaderText="File Name">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnldte_tracker" Text='<%# Eval("dte_tracker") %>'  CommandArgument='<%# Eval("tracker_id") %>' OnClick="GetFileDetails" runat="server">LinkButton</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="tracker_date_from" HeaderText="Data Entry Date From"
                                            SortExpression="tracker_date_from"></asp:BoundField>

                                        <asp:BoundField DataField="tracker_date_to" HeaderText="Data Entry Date To"
                                            SortExpression="tracker_date_to"></asp:BoundField>

                                        <asp:BoundField DataField="date_uploaded" HeaderText="Date Uploaded"
                                            SortExpression="date_uploaded"></asp:BoundField>

                                         <asp:TemplateField HeaderText="File Name" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnltracker_id" Text='<%# Eval("tracker_id") %>' runat="server">LinkButton</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnldownload" Text="Download"  CommandArgument='<%# Eval("tracker_id") %>' OnClick="DownloadFile" runat="server">Download</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                    <div class="copy_layout">
                        <p>Copyright © 2017 SOCY. All Rights Reserved | <a href="https://www.crs.org/" target="_blank">CRS</a>| Designed by <a href="http://thepalladiumgroup.com/" target="_blank">Palladium Group</a>| Powered by <a href="http://w3layouts.com/" target="_blank">W3layouts</a></p>
                    </div>
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
    </form>
</body>
</html>

