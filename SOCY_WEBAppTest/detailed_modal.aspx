<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="detailed_modal.aspx.cs" Inherits="SOCY_WEBAppTest.detailed_modal" %>

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
    <link href="css/css_download_button.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="frmMain" runat="server">


        <div class="graphs">

            <div class="xs">
                <div class="tab-content">

                    <div class="form-group">
                        <h3>
                            <asp:Label ID="lblPageHeader" runat="server" Text=""></asp:Label></h3>
                        <asp:Button ID="btnDownload" runat="server" Text="Download CSv" BackColor="#66ffcc" OnClick="btnDownload_Click" />

                    </div>

                    <div class="panel-body">
                        <asp:GridView ID="gdvModel" runat="server" AllowPaging="True" AutoGenerateColumns="false" PageSize="10" class="table table-bordered">
                            <Columns>

                                 <asp:BoundField  DataField="dst_name"
                                    HeaderText="District" />

                                 <asp:BoundField  DataField="sct_name"
                                    HeaderText="Sub County" />

                                <asp:BoundField  DataField="sct_name"
                                    HeaderText="Sub County" />

                                <asp:BoundField DataField="wrd_name"
                                    HeaderText="Parish" />

                                <asp:BoundField DataField="hh_code"
                                    HeaderText="Household Code" />

                                <asp:BoundField DataField="hhm_name"
                                    HeaderText="Beneficiary Name" />

                                 <asp:BoundField DataField="gnd_name"
                                    HeaderText="Sex" />

                                 <asp:BoundField DataField="hst_name"
                                    HeaderText="HIV Status" />

                                <asp:BoundField DataField="ynna_name"
                                    HeaderText="On ART" />
                            </Columns>

                        </asp:GridView>


                         <asp:GridView ID="gdvModel_ovc_serv" runat="server" AllowPaging="True" AutoGenerateColumns="false" PageSize="10" class="table table-bordered">
                            <Columns>

                                 <asp:BoundField  DataField="dst_name"
                                    HeaderText="District" />

                                 <asp:BoundField  DataField="sct_name"
                                    HeaderText="Sub County" />

                                <asp:BoundField  DataField="sct_name"
                                    HeaderText="Sub County" />

                                <asp:BoundField DataField="total_ovc"
                                    HeaderText="OVC Served" />
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


        <!-- /#wrapper -->
        <!-- Nav CSS -->
        <link href="css/custom.css" rel="stylesheet">
        <!-- Metis Menu Plugin JavaScript -->
        <script src="js/metisMenu.min.js"></script>
        <script src="js/custom.js"></script>
    </form>
</body>
</html>
