<%@ Page Title="" Language="C#" MasterPageFile="~/Account.Master" AutoEventWireup="true" CodeBehind="Error_Log.aspx.cs" Inherits="Account.Admin.Error_Log" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="table-responsive">
        <asp:GridView ID="gdvErrorLog" runat="server" AutoGenerateColumns="true" CssClass="table" OnPageIndexChanging="gdvErrorLog_PageIndexChanging" AllowPaging="true" PageSize="9" 
            EmptyDataText="There is no data at row 0">
        <PagerStyle CssClass="pagination-ys" />
        </asp:GridView>
       </div>
</asp:Content>
