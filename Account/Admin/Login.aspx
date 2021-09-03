<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Account.LoginIn" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8"/>
        <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
        <meta name="viewport" content="width=device-width, initial-scale=1"/>
        <title></title>
        <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Roboto:400,100,300,500" type="text/css"/>
        <link rel="stylesheet" href="../assets/bootstrap/css/bootstrap.min.css" type="text/css"/>
        <link rel="stylesheet" href="../assets/font-awesome/css/font-awesome.min.css" type="text/css"/>
		<link rel="stylesheet" href="../assets/css/form-elements.css" type="text/css"/>
        <link rel="stylesheet" href="../assets/css/style.css" type="text/css"/>

<%--        <link rel="shortcut icon" href="../assets/ico/favicon.png"/>--%>
        <link rel="apple-touch-icon-precomposed" sizes="144x144" href="../assets/ico/apple-touch-icon-144-precomposed.png"/>
        <link rel="apple-touch-icon-precomposed" sizes="114x114" href="../assets/ico/apple-touch-icon-114-precomposed.png"/>
        <link rel="apple-touch-icon-precomposed" sizes="72x72" href="../assets/ico/apple-touch-icon-72-precomposed.png"/>
        <link rel="apple-touch-icon-precomposed" href="../assets/ico/apple-touch-icon-57-precomposed.png"/>

        <script src="../assets/js/jquery-1.11.1.min.js" type="text/javascript"></script>
        <script src="../assets/bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
        <script src="../assets/js/jquery.backstretch.min.js" type="text/javascript"></script>
        <script src="../assets/js/scripts.js" type="text/javascript"></script>
</head>
<body>
  <div class="inner-bg">
    <div class="container">
       <div class="row">
            <div class="col-sm-8 col-sm-offset-2 text">
                <h1>User Login</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 col-sm-offset-4 form-box">
                <div class="form-bottom">
                    <form id="form" class="login-form" runat="server">
                        <div class="form-group">
                            <asp:TextBox ID="txtName" runat="server" placeholder="Username..." CssClass="form-username form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqName" runat="server" ErrorMessage="**Required**" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                       
                            <asp:TextBox ID="txtPass" runat="server" placeholder="Password..." CssClass="form-password form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqPass" runat="server" ErrorMessage="**Required**" ControlToValidate="txtPass"></asp:RequiredFieldValidator>
                        <br />
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-sm" onclick="btnLogin_Click" />
                        </div>
                    </form>
                </div>
            </div>
        </div>
     </div>
</div>
</body>
</html>
