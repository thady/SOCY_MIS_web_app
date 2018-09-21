<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="LogIn.aspx.cs" Inherits="SOCY_WEBAppTest.LogIn" %>

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
</head>
<body id="login">
    <div class="login-logo">
        <a href="index.html">
            <img src="images/loginLogo.png" alt="" /></a>
    </div>
    <h2 style="text-align:center">Sustainable Outcomes for Children & Youth [login]</h2>
    <div class="app-cam">
        <form id="frmMain" runat="server">
            <form>
                <asp:TextBox ID="txtEmail" class="text" value="E-mail address" onfocus="this.value = '';" onblur="if (this.value == '') {this.value = 'E-mail address';}" runat="server"></asp:TextBox>
                <asp:TextBox ID="txtpassword" TextMode="Password" class="text" value="Password" onfocus="this.value = '';" onblur="if (this.value == '') {this.value = 'Password';}" runat="server"></asp:TextBox>
                <div class="submit">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                </div>

            </form>
        </form>
    </div>
    <div class="copy_layout login">
        <p>Copyright © 2018 SOCY. All Rights Reserved | <a href="https://www.crs.org/" target="_blank">CRS</a>| Designed by <a href="http://thepalladiumgroup.com/" target="_blank">Palladium Group</a>| Powered by <a href="http://w3layouts.com/" target="_blank">W3layouts</a></p>
    </div>
</body>
</html>
