<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataPivot.aspx.cs" Inherits="SOCY_WEBAppTest.DataPivot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pivot Tables| SOCY :: CRS:Palladium</title>
    <link href="css/bootstrap.min.css" rel='stylesheet' type='text/css' />
    <link href="css/bootstrap-multiselect.css" rel='stylesheet' type='text/css' />
     <script src="js/bootstrap-multiselect.js" type="text/javascript"></script>

     <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/css/bootstrap.min.css"
        rel="stylesheet" type="text/css" />
  
    <link href="https://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="https://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
      <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js"></script>

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

    <script type="text/javascript">
        $(function () {
            $('[id*=lstDistricts]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('[id*=lstSubcounty]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('[id*=lstParish]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('[id*=lst_quarter]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="row" style="background-color:#e6faff">
            <div class="form-group">
                <div class="col-sm-8">
                    <label class="navbar-brand" style="display: block; color: blue">SOCY MIS Pivot Tables:</label>
                </div>

                 <div class="col-sm-2" >
                    
                </div>
                <div class="col-sm-2" style="margin-top:10px">
                     <asp:Button ID="btndownload" CssClass="btn-success btn" runat="server" Text="Download CSV" OnClick="btndownload_Click"/>
                      <asp:Button ID="btnLogOut" CssClass="btn-success btn" BackColor="#66ccff" runat="server" Text="LogOut" OnClick="btnLogOut_Click"/>
                </div>
            </div>

        </div>


        <div class="row" style="margin-top:5px">
            <div class="form-group">
                <div class="col-sm-2" style="border-right: 2px solid green;">

                    <div class="form-group">
                        <div class="col-sm-12" style="border-bottom: 1px solid gray">
                            <asp:RadioButton ID="radOvcserv" Text="OVCSERV" AutoPostBack="true" OnCheckedChanged="radOvcserv_CheckedChanged" runat="server" />
                            <asp:RadioButton ID="radovcActive" Text="Active" AutoPostBack="true" ForeColor="#ff0000" Font-Size="Small" runat="server" OnCheckedChanged="radovcActive_CheckedChanged" />
                            <asp:RadioButton ID="radovcGraduated" Text="Graduated" AutoPostBack="true" ForeColor="#ff0000"  Font-Size="Small" runat="server" OnCheckedChanged="radovcGraduated_CheckedChanged" />
                        </div>
                        <div class="col-sm-12" style="border-bottom: 1px solid gray">
                            <asp:RadioButton ID="radES" Text="Economic Strengthening" AutoPostBack="true" OnCheckedChanged="radES_CheckedChanged" runat="server" />
                        </div>
                        <div class="col-sm-12" style="border-bottom: 1px solid gray">
                            <asp:RadioButton ID="radFS" Text="Food Security & Nutrition" AutoPostBack="true" OnCheckedChanged="radFS_CheckedChanged" runat="server" />
                        </div>
                        <div class="col-sm-12" style="border-bottom: 1px solid gray">
                            <asp:RadioButton ID="radHIV" Text="HIV Prevention" AutoPostBack="true" OnCheckedChanged="radHIV_CheckedChanged" runat="server" />
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-12" style="margin-top: 10px">

                            <label for="lbldistrict" style="display: block; color: blue">District:</label>
                            <asp:ListBox ID="lstDistricts" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="lstDistricts_SelectedIndexChanged"></asp:ListBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-12" style="margin-top: 10px">
                            <label for="lstSubcounty" style="display: block; color: blue">SubCounty:</label>
                            <asp:ListBox ID="lstSubcounty" runat="server" SelectionMode="Multiple" AutoPostBack="true" OnSelectedIndexChanged="lstSubcounty_SelectedIndexChanged"></asp:ListBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-12" style="margin-top: 10px">

                            <label for="lst_quarter" style="display: block; color: blue">Quarter:</label>
                            <asp:ListBox ID="lst_quarter" runat="server" SelectionMode="Multiple" AutoPostBack="true"></asp:ListBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-sm-4" style="margin-top: 10px">

                            <asp:Button ID="btnApply" CssClass="btn-success btn" runat="server" Text="Apply Pivot" OnClick="btnApply_Click" />
                        </div>
                    </div>


                </div>

                <div class="col-sm-10" style="">
                    <asp:Literal ID="LitPivot" runat="server"></asp:Literal>
                </div>
            </div>
        </div>

        <div class="loading" align="center">
    Loading Pivot. Please wait...<br />
    <br />
    <img src="images/loader.gif" alt="" />
</div>


    </form>
</body>

</html>
