<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RolesEntry.aspx.cs" Inherits="Account.Admin.RolesEntry" MasterPageFile="~/Account.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <script type="text/javascript">
        jQuery(function () {
            $(<%=btnCancel.ClientID %>).click(function () {
              $(<%=txtRoleName.ClientID %>).val("");
            });
      });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
       <h1>Roles Entry</h1>
         <div class="form-group">  
            <label for="usr">Role Name</label>
                <asp:HiddenField ID="hdfID" runat="server" Visible="false" />
                <asp:HiddenField ID="hdfRoleName" runat="server" Visible="false" />
                <asp:TextBox ID="txtRoleName" runat="server" CssClass="input-control" placeholder="Role Name..." onkeypress="return ctrlEnter(event)"></asp:TextBox>
                <asp:TextBox ID="txtid" runat="server" CssClass="form-control" Visible="false" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqRoleName" runat="server" ForeColor="Red" ErrorMessage="**RolesName is Required**" ControlToValidate="txtRoleName" ValidationGroup="Roles"></asp:RequiredFieldValidator> 
             <br />
                <asp:Button ID="btnSave" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="Roles" />
                <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
        </div>
          <div class="panel panel-default">
        <div class="table-responsive">    
           <asp:GridView ID="gdvRoles" runat="server" GridLines="None" EmptyDataText="There is no roles entry" AutoGenerateColumns="false" CssClass="table" OnRowCommand="gdvRoles_RowCommand" AllowPaging="true" PageSize="7" OnPageIndexChanging="gdvRoles_PageIndexChanging" OnRowDataBound="gdvRoles_RowDataBound">
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
                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("RoleID") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Roles" >
                            <ItemTemplate>
                                <asp:Label ID="lblRoleName" runat="server" Text='<%#Eval("RoleName") %>'></asp:Label> 
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
                                        CommandName="Cmd_Edit" CommandArgument='<%# Eval("RoleID") %>'><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>&nbsp;
                                <asp:LinkButton ID="btnDelete" CssClass="btn-sm btn-danger" runat="server"  commandName="Cmd_Delete" OnClientClick="return confirm('Are you sure you want to delete?');" CommandArgument='<%#Eval("RoleID") %>'><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </ItemTemplate>  
                    </asp:TemplateField>
                </Columns>
           </asp:GridView>
            </div>
              </div>
    </div>
</asp:Content>
