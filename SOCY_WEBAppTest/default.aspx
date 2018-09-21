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
                            <asp:Button ID="btnLogOut" runat="server" class="btn-success btn" Text="LOGOUT" OnClick="btnLogOut_Click" /><span class="badge"></span></a>
                    </li>

                </ul>


                <div class="navbar-default sidebar" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li>
                                <a href="default.aspx"><i class="fa fa-dashboard fa-fw nav_icon"></i>Dashboard</a>
                            </li>

                            <%--Result Area one and 2 Reports--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Result Area I Reports<span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=agroEnterpriseRanking">Agro Enterprise Ranking Matrix Reports</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=cottageEnterpriseRanking">Cottage Enterprise Selection Reports</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=apprenticeshipSkillAquisitionTracking">Apprenticeship Skill Acquisition Reports</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=YouthTrainingCompletiion">Youth Training Completion Reports</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=YouthAssessmentScoring">Youth Assessment Scoring Reports</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=ActivityTraining">Activity training</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=ApprenticeshipRegister">Apprenticeship register</a>
                                    </li>

                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=CommunityTrainingRegister">Community Training Register</a>
                                    </li>

                                     <li>
                                        <a href="CapturedDataReports.aspx?reportid=benYouthTrainingInventory">Youth Training Inventory</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>

                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Result Area II Reports<span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
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
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>

                            <%--Result Area one and 2 Reports--%>

                            <%-- Household data reports--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Result Area III Reports<span class="fa arrow"></span></a>
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
                                        <a href="Home_visit_aggregate.aspx?reportid=hv_aggregate">Aggregated Home  Visit Report</a>
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
                                        <a href="CapturedDataReports.aspx?reportid=ovcViralLoadMonitoring">OVC Viral Load Monitoring Reports</a>
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

                            <%--Education Subsidy--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Education Subsidy<span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=beneficiarySchoolReadiness">Beneficiary School Readiness Assessment Reports</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>
                            <%--Education Subsidy--%>


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

                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>
                                    <asp:Label ID="lblDataEntryTracker" runat="server" Text="Weekly Data Entry Tracker"></asp:Label>
                                    <span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="data_entry_tracker.aspx">Upload Data Entry Tracker</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
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

                    <div class="panel-footer" style="margin-bottom: 0px"></div>



                    <div class="col-md-12  stats-info" style="margin-left: 0px; height: 360px">
                        <%-- <div class="panel-heading">
                            <h4 class="panel-title">
                                <asp:Label ID="lblHeader" runat="server" Text="Data upload status:"></asp:Label><asp:Label ID="lblQuarter" runat="server" Text="" ForeColor="#0066ff"></asp:Label></h4>

                        </div>--%>
                        <div class="panel-body" style="height:355px">
                            <asp:GridView ID="gdvDataEntry" runat="server" AllowPaging="True" AutoGenerateColumns="false" PageSize="5" class="table table-bordered" OnPageIndexChanging="gdvDataEntry_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="dst_name" HeaderText="District"
                                        SortExpression="dst_name"></asp:BoundField>
                                    <asp:BoundField DataField="hv" HeaderText="Home Visits"
                                        SortExpression="hv"></asp:BoundField>
                                    <asp:BoundField DataField="Referrals" HeaderText="Referrals"
                                        SortExpression="Referrals"></asp:BoundField>
                                    <asp:BoundField DataField="RAS" HeaderText="Risk Assessments"
                                        SortExpression="RAS"></asp:BoundField>
                                    <asp:BoundField DataField="HIP" HeaderText="Household Improvement Plan"
                                        SortExpression="HIP"></asp:BoundField>
                                    <asp:BoundField DataField="coomunity_tr" HeaderText="Community Training Register"
                                        SortExpression="coomunity_tr"></asp:BoundField>
                                </Columns>
                            </asp:GridView>

                            <asp:Chart ID="DataEntryChart" runat="server" BorderlineWidth="0" Width="1200px" Height="350px">
                                <Series>
                                    <asp:Series Name="Series1" XValueMember="dst_name" YValueMembers="hv"
                                        LegendText="Home Visits" IsValueShownAsLabel="false" ChartArea="ChartArea1" MarkerBorderColor="#DBDBDB">
                                    </asp:Series>

                                    <asp:Series Name="Series2" XValueMember="dst_name" YValueMembers="Referrals"
                                        LegendText="Referrals" IsValueShownAsLabel="false" ChartArea="ChartArea1" MarkerBorderColor="#DBDBDB">
                                    </asp:Series>

                                    <asp:Series Name="Series3" XValueMember="dst_name" YValueMembers="RAS"
                                        LegendText="Risk Assessment" IsValueShownAsLabel="false" ChartArea="ChartArea1" MarkerBorderColor="#DBDBDB">
                                    </asp:Series>

                                    <asp:Series Name="Series4" XValueMember="dst_name" YValueMembers="HIP"
                                        LegendText="Household Improvement Plan" IsValueShownAsLabel="false" ChartArea="ChartArea1" MarkerBorderColor="#DBDBDB">
                                    </asp:Series>

                                    <asp:Series Name="Series5" XValueMember="dst_name" YValueMembers="coomunity_tr"
                                        LegendText="Community Training Register" IsValueShownAsLabel="false" ChartArea="ChartArea1" MarkerBorderColor="#DBDBDB">
                                    </asp:Series>
                                </Series>

                                <Legends>
                                    <asp:Legend Title="Tools" Alignment="Near"/>
                                </Legends>

                                <Titles>
                                    <asp:Title Docking="Top" Text="Number of Tools entered in the system for the current Quarter" />
                                </Titles>

                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1">
                                        <AxisX Interval="1" TextOrientation="Rotated90"></AxisX>

                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </div>
                        <div class="clearfix"></div>
                    </div>

                    <div class="clearfix"></div>

                    <div class="col-md-6  stats-info" style="height: 492px;margin-top:10px">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <asp:Label ID="Label3" runat="server" Text="OVC HIVSTAT(<18):"></asp:Label><asp:Label ID="lblovchiv_stat" runat="server" Text="" ForeColor="#0066ff"></asp:Label>
                            </h4>
                        </div>
                        <div class="panel-body">
                            <ul class="list-unstyled">
                                <li>Total OVC(<18) served<div class="text-success pull-right">
                                    <asp:Label ID="lblTotalovc" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>OVC With reported HIV status<div class="text-success pull-right">
                                    <asp:Label ID="lblTotalovcReportedHIVstatus" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>OVC Reported Positive<div class="text-success pull-right">
                                    <asp:Label ID="lblTotalOVCPositive" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>OVC Reported Negative<div class="text-success pull-right">
                                    <asp:Label ID="lblTotalOVCNegative" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>OVC HIV status reported percentage<div class="text-success pull-right">
                                    <asp:Label ID="lblReportedstatuspercentage" runat="server" Text=""></asp:Label><asp:Label ID="lblpercentage" runat="server" Text="%"></asp:Label>
                                </div>
                                </li>
                                <li>Total OVC +ve on ART<div class="text-success pull-right">
                                    <asp:Label ID="lblARTYes" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>Total OVC +ve on ART and adhering<div class="text-success pull-right">
                                    <asp:Label ID="lblAdhering" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li class="last">Total OVC +ve not on ART<div class="text-success pull-right">
                                    <asp:Label ID="lblARTNo" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li class="last">Total OVC with unknown status<div class="text-success pull-right">
                                    <asp:Label ID="lblNostatus" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                            </ul>
                        </div>
                        <div class="clearfix"></div>
                    </div>


                    <div class="col-md-6  stats-info" style="margin-top: 10px; height: 492px">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <asp:Label ID="lblcpaHeader" runat="server" Text="No. of Beneficiaries receiving services by Core Program Area(CPA)"></asp:Label><asp:Label ID="lblCpaQuarter" runat="server" Text="" ForeColor="#0066ff"></asp:Label>
                            </h4>
                        </div>
                        <div class="panel-body" style="height: 450px">
                            <ul class="list-unstyled">
                                <li>Enonomic Strengthening:<div class="text-success pull-right">
                                    <asp:Label ID="lbles" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>Food Secuity and nutrition<div class="text-success pull-right">
                                    <asp:Label ID="lblfsn" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>Health & HIV Prevention<div class="text-success pull-right">
                                    <asp:Label ID="lblhiv" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>Protection<div class="text-success pull-right">
                                    <asp:Label ID="lblprotection" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>Education<div class="text-success pull-right">
                                    <asp:Label ID="lblEducation" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                                <li>Psychosocial Support<div class="text-success pull-right">
                                    <asp:Label ID="lblps" runat="server" Text=""></asp:Label>
                                </div>
                                </li>
                            </ul>
                        </div>
                        <div class="clearfix"></div>
                    </div>

                    <div class="col-md-12  stats-info" style="margin-top: 5px">
                        <div class="panel-heading">
                            <h4 class="panel-title">
                                <asp:Label ID="Label6" runat="server" Text="OVC_SERV "></asp:Label><asp:Label ID="lblheaderOVCServe" runat="server" Text="" ForeColor="#0066ff"></asp:Label>
                            </h4>
                        </div>

                        <div class="row">
                            <div class="col-md-4" style="height: 500px">
                                <div class="form-group">

                                    <label for="cbpPartner" class="col-sm-2 control-label">
                                        <asp:Label ID="lbldistrict" runat="server" Text="District:" Font-Bold="true"></asp:Label></label>
                                    <div class="col-sm-12">
                                        <asp:DropDownList ID="cboDistrict" Width="220px" runat="server" ViewStateMode="Enabled" EnableViewState="true" class="form-control1" AutoPostBack="true" OnSelectedIndexChanged="cboDistrict_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                </div>

                                <div class="form-group">

                                    <label for="txtCreateDateFrom" class="col-sm-12 control-label">
                                        <asp:Label ID="lblDateFrom" runat="server" Text="Date From:"></asp:Label></label>
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtCreateDateFrom" class="form-control1" Width="220px" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">

                                    <label for="txtCreateDateTo" class="col-sm-2 control-label">
                                        <asp:Label ID="lblDateTo" runat="server" Text="To:"></asp:Label></label>
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtCreateDateTo" class="form-control1" Width="220px" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="panel-footer">
                                    <div class="row">
                                        <div class="col-sm-8 col-sm-offset-2" style="margin-top: 5px; margin-left: 0px">

                                            <asp:Button ID="btnFilter" runat="server" class="btn-success btn" OnClick="btnFilter_Click" Text="Filter" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-8">
                                <asp:GridView ID="gdvOVCServe" AutoGenerateColumns="False" class="table table-striped" runat="server" GridLines="None">

                                    <Columns>
                                        <asp:BoundField DataField="OVC_SERVE_INDICATOR" HeaderText="Age Groups"
                                            SortExpression="OVC_SERVE_INDICATOR"></asp:BoundField>
                                        <asp:BoundField DataField="Target" HeaderText="Target"
                                            SortExpression="Target"></asp:BoundField>
                                        <asp:BoundField DataField="Value" HeaderText="Number Served"
                                            SortExpression="Value"></asp:BoundField>
                                        <asp:BoundField DataField="percentage_Archived" HeaderText="Percentage Archived"
                                            SortExpression="percentage_Archived"></asp:BoundField>
                                    </Columns>
                                    <HeaderStyle CssClass="warning" Font-Bold="true" />
                                </asp:GridView>

                            </div>

                        </div>

                        <div class="clearfix"></div>
                    </div>

                    <div class="clearfix"></div>

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

