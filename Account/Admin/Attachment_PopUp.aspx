<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Attachment_PopUp.aspx.cs" Inherits="Account.Admin.Attachment_PopUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
     <script src="<%=ResolveClientUrl("~/js/jquery.min 3.2.1.js")%>"></script>
     <script src="<%=ResolveClientUrl("~/js/jquery.min.js")%>"></script>
     <script src="<%=ResolveClientUrl("~/js/moment.min.js")%>"></script>    

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style>
      .cstmBtn
        {
            display: inline-block;
            padding: 6px 12px;
            margin-bottom: 0;
            font-size: 14px;
            font-weight: 400;
            line-height: 1.42857143;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
            margin-left: 15px;
        }
           .fileUpload
        {
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            outline: 0;
            border: 1px solid rgba(255, 255, 255, 0.4);
            background-color: #E6E6FA;
            width: 250px;
            border-radius: 3px;
            padding: 10px 15px;
            margin: 0 auto;
            margin-top: 0px;
            margin-right: auto;
            margin-bottom: 0px;
            margin-left: auto;
            display: block;
            text-align: center;
            font-size: 15px;
            color: black;
            -webkit-transition-duration: 0.25s;
            transition-duration: 0.25s;
            font-weight: 300;
        }
        .btn-outline
        {
            color: inherit;
            background-color: transparent;
            transition: all .5s;
        }
</style>
</head>
<body>
    <form id="form1" runat="server">
    <!-- Modal -->

<div class="modal-open" id="myModal" role="dialog" style="padding-right:17px;">
    <div class="modal-dialog" >
    
      <!-- Modal content-->
      
            <div class="modal-header">
              <h4 class="modal-title">Add Attachment</h4>
            </div>

            <div class="modal-body" >
                <asp:Panel ID="panel3" runat="server" Visible="false">
                    <center>
                    <asp:GridView ID="gdvSavedFile" CssClass="table-condensed" GridLines="None" runat="server" AutoGenerateColumns="false" EnableViewState="false" OnRowCommand="gdvSavedFile_RowCommand">
                      
                         <Columns>

                            <asp:HyperLinkField DataTextField="FileName" DataNavigateUrlFields="FilePath" Target="_blank"/>
                            <asp:HyperLinkField DataTextField="ID" DataNavigateUrlFields="FilePath" Target="_blank" ControlStyle-CssClass="hidden"/> 
                            <asp:ButtonField ButtonType="Link" CommandName="file_Delete" Text="<i class='glyphicon glyphicon-remove'></i>"  ControlStyle-CssClass="btn-sm btn-danger"/>
                          
                        </Columns>

                    </asp:GridView>
                    </center>
                </asp:Panel>
                <center>
                <asp:Panel ID="panel2" runat="server">
                <asp:FileUpload ID="attFile1" runat="server" CssClass="fileUpload"/>   
                <asp:FileUpload ID="attFile2" runat="server" CssClass="fileUpload"/>
                <asp:FileUpload ID="attFile3" runat="server" CssClass="fileUpload"/>
                <asp:FileUpload ID="attFile4" runat="server" CssClass="fileUpload"/>
                <asp:FileUpload ID="attFile5" runat="server" CssClass="fileUpload"/>   
                               
                </asp:Panel>  
                </center>                           
            </div>

            <div class="modal-footer">
              <asp:Button ID="btnSaveFiles" runat="server" Text="OK" CssClass="cstmBtn btn-primary" OnClick="btnSaveFiles_Click" />
              <asp:Button ID="btnCancelModal" runat="server" Text="Close" CssClass="btn btn-outline btn-primary" OnClick="btnCancelModal_Click"/>
            </div>
        
    </div>
</div>
    </form>
</body>
</html>
