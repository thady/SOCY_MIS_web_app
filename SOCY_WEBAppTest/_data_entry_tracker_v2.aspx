<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_data_entry_tracker_v2.aspx.cs" Inherits="SOCY_WEBAppTest._data_entry_tracker_v2" %>

<!--
Author: W3layouts
Author URL: http://w3layouts.com
License: Creative Commons Attribution 3.0 Unported
License URL: http://creativecommons.org/licenses/by/3.0/
-->
<!DOCTYPE HTML>
<html>
<head>
    <title>Data Entry Tracker | SOCY :: CRS :: Palladium</title>
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
                                      
                                        <asp:Label ID="lblToolsReceived" runat="server" Text="Tools Received + Carried over" ForeColor="Red"></asp:Label>
                                    </div>

                                     
                                      <div class="col-sm-2">
                                        <asp:Label ID="lblToolsEntered" runat="server" Text="Tools Entered" ForeColor="Red"></asp:Label>
                                         
                                    </div>
                                </div>


                                <div class="form-group">
                                    <label for="txt_ipt" class="col-sm-2 control-label">Identification & Prioritization</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_ipt" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>
                                    </div>

                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txt_ipt_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>

                                </div>


                                <div class="form-group">
                                    <label for="txt_hat" class="col-sm-2 control-label">Household Assessment</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_hat" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>
                                    </div>

                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txt_hat_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="form-group">
                                    <label for="txt_hip" class="col-sm-2 control-label">Household Improvement Plan</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_hip" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>
                                    </div>

                                     <div class="col-sm-2">
                                        <asp:TextBox ID="txt_hip_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>

                                </div>

                                <div class="form-group">
                                    <label for="txt_home_visit" class="col-sm-2 control-label">Home visit</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_home_visit" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_home_visit_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_rat" class="col-sm-2 control-label">Risk Assessment Tool</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_rat" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>
                                    </div>

                                    
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_rat_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_linkages" class="col-sm-2 control-label">Linkages</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_linkages" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>

                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_linkages_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_referal" class="col-sm-2 control-label">Referral</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_referal" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_referal_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <label for="txt_viral_load" class="col-sm-2 control-label">Viral Load</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_viral_load" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_viral_load_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_edu_subsidy" class="col-sm-2 control-label">Education Subsidy tool</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_edu_subsidy" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_edu_subsidy_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_comm_SILC" class="col-sm-2 control-label">Community training – SILC</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_comm_SILC" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_comm_SILC_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_comm_youth" class="col-sm-2 control-label">Community training – Youth</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_comm_youth" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_comm_youth_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_youth_saving" class="col-sm-2 control-label">Youth savings and utilization</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_youth_saving" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_youth_saving_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_app_progress" class="col-sm-2 control-label">SOCY Apprentice Progress Tracking Tool</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_app_progress" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_app_progress_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_training_inventory" class="col-sm-2 control-label">Training register/Inventory</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_training_inventory" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_training_inventory_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_skill_aquisition" class="col-sm-2 control-label">Apprentice Skill Acquisition Tracking Tool</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_skill_aquisition" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_skill_aquisition_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_training_completion" class="col-sm-2 control-label">SOCY Training Completion Form</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_training_completion" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_training_completion_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_ass_awarding_tool_kit" class="col-sm-2 control-label">Youth Assessment for awarding a tool kit</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_ass_awarding_tool_kit" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_ass_awarding_tool_kit_entered" CssClass="form-control1" placeholder="Enter tools received" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_youth_tracer" class="col-sm-2 control-label">SOCY Youth Tracer Form</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_youth_tracer" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_youth_tracer_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_dovcc" class="col-sm-2 control-label">DOVCC functionality and data use checklist</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_dovcc" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_dovcc_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_sovcc" class="col-sm-2 control-label">SOVCC functionality and data use checklist</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_sovcc" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_sovcc_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_cbsd_resource" class="col-sm-2 control-label">SOCY CBSD resource allocation tool</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_cbsd_resource" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_cbsd_resource_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_staff_appraisal" class="col-sm-2 control-label">CBSD Staff and Appraisal Tracking Tool</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_staff_appraisal" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_staff_appraisal_entered" CssClass="form-control1" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_reintergration" class="col-sm-2 control-label">SOCY Reintegration Summary Tool</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_reintergration" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_reintergration_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_dreams_registration" class="col-sm-2 control-label">Enrolment registration</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_dreams_registration" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_dreams_registration_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_hct_registration" class="col-sm-2 control-label">HCT registration</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_hct_registration" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_hct_registration_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_dreams_partner" class="col-sm-2 control-label">DREAMS partner register</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_dreams_partner" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_dreams_partner_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_sinovuyo" class="col-sm-2 control-label">SINOVUYO Missed sessions</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_sinovuyo" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_sinovuyo_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_stepping_stones" class="col-sm-2 control-label">stepping stones missed session register</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_stepping_stones" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_stepping_stones_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <label for="txt_stepping_stones" class="col-sm-2 control-label">DREAMS Screening tool</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_dreams_screening" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_dreams_screening_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <label for="txt_sasa_session" class="col-sm-2 control-label">SASA session attendance</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_sasa_session" CssClass="form-control1" placeholder="Enter tools received" runat="server"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txt_sasa_session_entered" CssClass="form-control1" Text="0" runat="server"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="txt_comment" class="col-sm-2 control-label">Tracker Comments</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_comment" TextMode="multiline" Columns="50" Rows="5" CssClass="form-control1" runat="server" /></div>
                                </div>

                                <div class="panel-footer">
                                    <div class="row">
                                        <div class="col-sm-8 col-sm-offset-2">
                                            <asp:Button ID="btnsave" CssClass="btn-success btn" runat="server" Text="Save Tracker"  OnClick="btnsave_Click" />
                                            <asp:Button ID="btnreset" CssClass="btn-inverse btn" runat="server" OnClick="btnreset_Click" Text="Reset Fields" />
                                        </div>
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


