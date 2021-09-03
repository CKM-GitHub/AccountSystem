<%@ Page Title="" Language="C#" MasterPageFile="~/Account.Master" AutoEventWireup="true" CodeBehind="CashUnit_Entry.aspx.cs" Inherits="Account.Admin.CashUnit_Entry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
       <h1>Account Entry</h1>
         <div class="form-group">  
                <asp:HiddenField ID="hdfID" runat="server" Visible="false" />
                 <label for="usr">Cash Unit</label>
                <asp:TextBox ID="txtCashUnit" runat="server" placeholder="Cash Unit..." CssClass="input-control" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqCashUnit" runat="server" ForeColor="Red" ErrorMessage="**Cash Unit Name is Required**" ControlToValidate="txtCashUnit" ValidationGroup="Cash"></asp:RequiredFieldValidator> 
               
              <br />
                <asp:Button  runat="server" ID="btnSave" onclick="btnSave_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="Cash"/>
              <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
        </div>
        <div class="panel panel-default">
        <div class="table-responsive">    
           <asp:GridView ID="gdvCashUnits" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-hover" OnRowCommand="gdvCashUnits_RowCommand" AllowPaging="true" PageSize="7" OnPageIndexChanging="gdvCashUnits_PageIndexChanging" EmptyDataText="There is no cash units entry"> 
                <PagerStyle CssClass="pagination-ys" />
                <HeaderStyle BackColor="#404c54" ForeColor="White" />
               <Columns>
                      <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField Visible="false">
                            <ItemTemplate >
                                <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn-sm btn-primary" Visible="false"
                                        CommandName="Cmd_Detail" CommandArgument='<%# Eval("ID") %>'><span class="glyphicon glyphicon-list"></span></asp:LinkButton>&nbsp;
                            </ItemTemplate>  
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                                            
                    <asp:TemplateField HeaderText="Cash Unit" >
                            <ItemTemplate>
                                <asp:Label ID="lblCashUnit" runat="server" Text='<%#Eval("CashUnit") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Created Date" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreated_Date" runat="server" Text='<%#Eval("Created_Date") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Updated Date" >
                            <ItemTemplate>
                                <asp:Label ID="lblUpdated_Date" runat="server" Text='<%#Eval("Updated_Date") %>'></asp:Label> 
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
                             
                                <asp:LinkButton ID="btnDelete" CssClass="btn-sm btn-danger" runat="server" commandName="Cmd_Delete" OnClientClick="return confirm('Are you sure you want to delete?');" CommandArgument='<%#Eval("ID") %>'>
                                 <span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </ItemTemplate>  
                    </asp:TemplateField>
                </Columns>
           </asp:GridView>
            </div>
            </div>
    </div>
</asp:Content>
