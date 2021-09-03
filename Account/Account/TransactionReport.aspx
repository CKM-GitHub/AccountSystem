<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransactionReport.aspx.cs" Inherits="Account.TransactionReport" MasterPageFile="~/Account.Master"%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker1').datetimepicker();
        });
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
       <h1>Transaction Report</h1>
        <div class="form-group">  
            <label for="usr">Account Name</label>
            <asp:DropDownList ID="ddlAccountName" runat="server" CssClass="input-control"></asp:DropDownList>
            <br />
             <label for="usr">Date</label>
            <div class='input-group date' id='datetimepicker1'>
                    <asp:TextBox ID="txtdate" runat="server" CssClass="form-control"></asp:TextBox>
                <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                </span>
            </div>
            <br />
             <label for="usr">Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-control"></asp:DropDownList>
            <br />
            <asp:Button  runat="server" ID="btnSearch" onclick="btnSearch_Click" Text="Search" CssClass="btn btn-primary"/>
              <br />
              <br />
            <rsweb:ReportViewer ID="ReportViewer1" CssClass="table" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="1288px" Visible="false" ZoomMode="FullPage">
                <LocalReport ReportEmbeddedResource="Account.Account.Report1.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="TransactionDataSet" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="TransactionDataSetTableAdapters."></asp:ObjectDataSource>
            <br />
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        </div>
     </div>
</asp:Content>
