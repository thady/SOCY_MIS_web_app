﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="SOCY_WEBAppTest.User" %>

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
</head>
<body>
    <form id="frmMain" runat="server">
        <asp:ScriptManager ID="scriptManger" runat="server"></asp:ScriptManager>
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
                           <asp:Button ID="btnLogOut" runat="server" class="btn-success btn" Text="LOGOUT" OnClick="btnLogOut_Click"/><span class="badge"></span></a>
                    </li>
                </ul>
                <div class="navbar-form navbar-right">
                    <%-- <input type="text" class="form-control" value="Search..." onfocus="this.value = '';" onblur="if (this.value == '') {this.value = 'Search...';}">--%>
                </div>

                <div class="navbar-default sidebar" role="navigation">
                    <div class="sidebar-nav navbar-collapse">
                        <ul class="nav" id="side-menu">
                            <li>
                                <a href="default.aspx"><i class="fa fa-dashboard fa-fw nav_icon"></i>Dashboard</a>
                            </li>

                            <%-- Household data reports--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Household Data Reports<span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=OVCIdentification">OVC Ident. & Prioritization</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=Household">Household Data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdMember">Household Member Data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdAssessment">HAT Data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdAssessmentMember">HAT Member Data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HouseholdReferral">Household Refferal Data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HomeVisit">Home Visit Data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HomeVisitMember">Home Visit Member Data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=HomeVisitArcive">Home Visist Archive Data</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SocialWorker">Social Worker Data</a>
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
                                        <a href="CapturedDataReports.aspx?reportid=ActivityTraining">Activity Training</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=ApprenticeshipRegister">Apprenticeship Register</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=ServiceRegister">Service Register</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=AlternativeCarePanel">Alternative Care Panel</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=CBSDResourceAllocation">CBSD Resource Allocation</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=CBSDStaffAppraisalTracking">CBSD Staff Appraisal Tracking</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=DistrictOVCCheckList">District OVC Check List</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=InstitutionalCareSummary">Institutional Care Summary</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>

                            <%--Result Area one and 2 Reports--%>


                            <%--Dreams and Silk--%>
                            <li>
                                <a href="#"><i class="fa fa-check-square-o nav_icon"></i>Dreams & Silk Reports<span class="fa arrow"></span></a>
                                <ul class="nav nav-second-level">
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=DREAMSEnrolment">DREAMS Enrolment</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SILCGroups">SILC Groups</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SILCGroupMembers">SILC Group Members</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SILCFinancialRegister">SILC Financial Register</a>
                                    </li>
                                    <li>
                                        <a href="CapturedDataReports.aspx?reportid=SILCSavingsRegister">SILC Savings Register</a>
                                    </li>
                                </ul>
                                <!-- /.nav-second-level -->
                            </li>
                            <%-- Dreams and Silk--%>

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
                            <asp:Label ID="lblHeader" runat="server" Text="Account Details:"></asp:Label><asp:Label ID="lblReportTitle" runat="server" Text="" Font-Bold="true"></asp:Label></h3>
                        <div class="tab-content">
                            <div class="tab-pane active" id="horizontal-form">
                                <div id="frmMain2" class="form-horizontal">
                                    <div class="form-group">

                                        <label for="txtFirstname" class="col-sm-2 control-label">
                                            <asp:Label ID="lblfname" runat="server" Text="First Name:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtFirstname" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                        <label for="txtlastname" class="col-sm-2 control-label">
                                            <asp:Label ID="lbllastname" runat="server" Text="Last Name:"></asp:Label>
                                        </label>
                                        <div class="col-sm-3">
                                             <asp:TextBox ID="txtlastname" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">

                                        <label for="cboTitle" class="col-sm-2 control-label">
                                            <asp:Label ID="lblTitle" runat="server" Text="Title:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboTitle" class="form-control1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">

                                        <label for="txtEmail" class="col-sm-2 control-label">
                                            <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label></label>
                                        <div class="col-sm-3">
                                        <asp:TextBox ID="txtEmail" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                        <label for="txtSkype" class="col-sm-2 control-label">
                                            <asp:Label ID="lblParish" runat="server" Text="Skype:"></asp:Label></label>
                                        <div class="col-sm-3">
                                             <asp:TextBox ID="txtSkype" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">

                                        <label for="txtphone" class="col-sm-2 control-label">
                                            <asp:Label ID="lblphone" runat="server" Text="Phone Number:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtphone" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                        
                                         <label for="chkActive" class="col-sm-2 control-label">
                                            <asp:Label ID="lblActive" Visible="false" runat="server" Text="Phone Number:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:CheckBox ID="chkActive" class="form-control1"  Text="Active User" runat="server" />
                                        </div>
                                    </div>

                                    <div class="form-group">

                                        <label for="cboRegion" class="col-sm-2 control-label">
                                            <asp:Label ID="lblRegion" runat="server" Text="Region Restriction:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboRegion" class="form-control1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboRegion_SelectedIndexChanged"></asp:DropDownList>
                                        </div>

                                         <label for="cboDistrict" class="col-sm-2 control-label">
                                            <asp:Label ID="lbldistrict" runat="server" Text="District Restriction:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="cboDistrict" class="form-control1" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">

                                        <label for="txtpassword" class="col-sm-2 control-label">
                                            <asp:Label ID="lblpassword" runat="server" Text="Password:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtpassword" TextMode="Password" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                        <label for="txtpasswordConfirm" class="col-sm-2 control-label">
                                            <asp:Label ID="lblpasswordConfirm" runat="server" Text="Confirm Password:"></asp:Label></label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtpasswordConfirm" TextMode="Password" class="form-control1" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">

                                        <label for="chkSetPassword" class="col-sm-2 control-label">
                                            <asp:Label ID="lblSetPassword" Visible="false" runat="server" Text="Set Password:"></asp:Label></label>
                                        <div class="col-sm-3">
                                              <asp:CheckBox ID="chkSetPassword" class="form-control1"  Text="Active User" runat="server" />
                                        </div>
                                    </div>


                                    <div class="panel-footer">
                                        <div class="row">
                                            <div class="col-sm-8 col-sm-offset-2">
                                                <asp:Button ID="btnsave" runat="server" class="btn-success btn" Text="Save User"/>
                                               <asp:Button ID="btnClear" runat="server" class="btn-default btn" Text="Clear" />
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
