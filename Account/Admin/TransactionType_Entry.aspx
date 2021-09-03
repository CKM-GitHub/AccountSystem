<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransactionType_Entry.aspx.cs" Inherits="Account.Setup.TransactionType_Entry" MasterPageFile="~/Account.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <script type="text/javascript">
     jQuery(function () {
            $(<%=btnCancel.ClientID %>).click(function () {
                $(<%=txtTransType.ClientID %>).val("");
            });
     });
        </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <div class="container">
       <h1>Transaction Type Entry</h1>
         <div class="form-group">  
            <label for="usr">Transaction Type</label>
                <asp:HiddenField ID="hdfID" runat="server" Visible="false" />
                <asp:TextBox ID="txtTransType" runat="server" CssClass="input-control" placeholder="Transaction Type..." onkeypress="return ctrlEnter(event)"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqIncomeType" runat="server" ForeColor="Red" ErrorMessage="**Transaction Type is Required**" ControlToValidate="txtTransType" ValidationGroup="Income"></asp:RequiredFieldValidator>
             <br />
                <asp:Button  runat="server" ID="btnSave" onclick="btnSave_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="Income"/>
              <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
        </div>
            <div class="panel panel-default">
        <div class="table-responsive">    
           <asp:GridView ID="gdvTransType" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataText="There is no transaction type entry" CssClass="table" OnRowCommand="gdvTransType_RowCommand" AllowPaging="true" PageSize="7" OnPageIndexChanging="gdvTransType_PageIndexChanging" OnRowDataBound="gdvTransType_RowDataBound">
                 <PagerStyle CssClass="pagination-ys" />
                 <HeaderStyle BackColor="#404c54" ForeColor="White" />
               <Columns>
                     <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                                            
                    <asp:TemplateField HeaderText="Category Name" >
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="CreateBy" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreateBy" runat="server" Text='<%#Eval("CreatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="UpdateBy" >
                            <ItemTemplate>
                                <asp:Label ID="lblUpdateBy" runat="server" Text='<%#Eval("UpdatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField >
                            <ItemTemplate> 
                             <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn-sm btn-primary"
                                        CommandName="Cmd_Edit" CommandArgument='<%# Eval("ID") %>'><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                             &nbsp;<asp:LinkButton ID="btnDelete" CssClass="btn-sm btn-danger" runat="server" commandName="Cmd_Delete" OnClientClick="return confirm('Are you sure you want to delete?');" CommandArgument='<%#Eval("ID") %>'>
                                 <span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </ItemTemplate>  
                    </asp:TemplateField>
                </Columns>
           </asp:GridView>
            </div>
                </div>
    </div>
</asp:Content>
