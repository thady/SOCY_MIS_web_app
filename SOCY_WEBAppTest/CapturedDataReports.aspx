﻿<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CapturedDataReports.aspx.cs" Inherits="SOCY_WEBAppTest.CapturedDataReports" %>

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

      <style type="text/css">
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
</style>

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

     <script type="text/javascript">
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }
    $('form').live("submit", function () {
        ShowProgress();
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
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=YouthSavingsRegister">Youth Savings Register</a>
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
                                        <a href="CapturedDataReports.aspx?reportid=RASMNEW">Peadiatric Risk Assessment</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=RASM">Risk assessment register(Old)</a>
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
                                        <a href="_data_entry_tracker_v2.aspx">Create New Tracker</a>
                                    </li>
                                    <li>
                                        <a href="_data_entry_tracker_v2_view.aspx?Token_id=View Trackers">View My Trackers</a>
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
                    <div class="xs">
                        <h3>
                            <asp:Label ID="lblHeader" runat="server" Text="Captured Data Reports:"></asp:Label><asp:Label ID="lblReportTitle" runat="server" Text="" Font-Bold="true"></asp:Label></h3>
                        <div class="tab-content">
                            <div class="tab-pane active" id="horizontal-form">
                                <div id="frmMain2" class="form-horizontal">

                                    <div class="form-group">

                                        <label for="cbpPartner" class="col-sm-2 control-label">
                                            <asp:Label ID="lblPartner" runat="server" Text="Partner:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboPartner" class="form-control1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboPartner_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <label for="cboCSO" class="col-sm-2 control-label">
                                            <asp:Label ID="lblCSO" runat="server" Text="CSO:"></asp:Label>
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboCSO" class="form-control1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">

                                        <label for="cboRegion" class="col-sm-2 control-label">
                                            <asp:Label ID="lblRegion" runat="server" Text="Region:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboRegion" class="form-control1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboRegion_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <label for="cboDistrict" class="col-sm-2 control-label">
                                            <asp:Label ID="lblDistrict" runat="server" Text="District:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboDistrict" class="form-control1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboDistrict_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">

                                        <label for="cboSubCounty" class="col-sm-2 control-label">
                                            <asp:Label ID="lblSubcounty" runat="server" Text="Sub County:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboSubCounty" class="form-control1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboSubCounty_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <label for="cboParish" class="col-sm-2 control-label">
                                            <asp:Label ID="lblParish" runat="server" Text="Parish:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboParish" class="form-control1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div id="datediv" class="form-group" runat="server">

                                        <label for="txtCreateDateFrom" class="col-sm-2 control-label">
                                            <asp:Label ID="lblDateFrom" runat="server" Text="Date From:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtCreateDateFrom" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                        <label for="txtCreateDateTo" class="col-sm-2 control-label">
                                            <asp:Label ID="lblDateTo" runat="server" Text="To:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtCreateDateTo" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div id="chk_div" class="form-group" runat="server">

                                        <label for="txtCreateDateFrom" class="col-sm-2 control-label">
                                            </label>
                                        <div class="col-sm-3">
                                            <asp:CheckBox ID="chkNotvisited" class="form-control1" Text="Export households not visited" OnCheckedChanged="chkNotvisited_CheckedChanged" BackColor="#ffff99" runat="server" AutoPostBack="true" />
                                        </div>
                                    </div>

                                    <div class="loading" align="center">
                                        Loading Data. Please wait...<br />
                                        <br />
                                        <img src="images/loader.gif" alt="" />
                                    </div>

                                    <div class="panel-footer">
                                        <div class="row">
                                            <div class="col-sm-8 col-sm-offset-2">
                                                <asp:Button ID="btnsearch" runat="server" class="btn-success btn" Text="Export Report(Excel)" OnClick="btnsearch_Click" />
                                                <asp:Button ID="btnClear" runat="server" class="btn-default btn" Text="Clear Search" />
                                            </div>
                                        </div>
                                    </div>


                                </div>

                            </div>
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
