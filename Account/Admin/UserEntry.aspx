<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="UserEntry.aspx.cs" Inherits="Account.UserEntry" MasterPageFile="~/Account.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript">
        jQuery(function () {
            $(<%=btnCancel.ClientID %>).click(function () {
                $(<%=txtUserName.ClientID %>).val("");
                $(<%=txtPass.ClientID %>).val("");
            });
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
       <h1>User Entry</h1>
         <div class="form-group">  
            <label for="usr">User Name</label>
                <asp:TextBox ID="txtUserName" runat="server" CssClass="input-control" placeholder="User Name..." onkeypress="return ctrlEnter(event)"></asp:TextBox>
                <asp:TextBox ID="txtid" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqUserName" runat="server" ForeColor="Red" ErrorMessage="**UserName is Required**" ControlToValidate="txtUserName" ValidationGroup="user"></asp:RequiredFieldValidator> 
             <br />
            <label for="pwd">Password</label>
                <asp:TextBox ID="txtPass" runat="server" CssClass="input-control" placeholder="Password..." onkeypress="return ctrlEnter(event)"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqPass" runat="server" ForeColor="Red" ErrorMessage="**Password is Required**" ControlToValidate="txtPass" ValidationGroup="user"></asp:RequiredFieldValidator>
              <br />
             <label for="usr">Roles Name</label>
                <asp:DropDownList ID="ddlRoleName" runat="server" CssClass="input-control"></asp:DropDownList>
             <br />
                <asp:Button  runat="server" ID="btnCreate" onclick="btnCreate_Click" Text="Create User" CssClass="btn btn-primary" ValidationGroup="user"/>
                <asp:Button ID="btnUpdate" CssClass="btn btn-primary " runat="server" Text="Update" OnClick="btnUpdate_Click" Visible="false" ValidationGroup="user"/>
                <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
        </div>
       <div class="panel panel-default">
        <div class="table-responsive">    
           <asp:GridView ID="gdvUser" runat="server" GridLines="None" AutoGenerateColumns="false" CssClass="table table-hover" OnRowCommand="gdvUser_RowCommand" AllowPaging="true" PageSize="7" OnPageIndexChanging="gdvUser_PageIndexChanging" EmptyDataText="There is no user entry" OnRowDataBound="gdvUser_RowDataBound">
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
                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("UserID") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                                            
                    <asp:TemplateField HeaderText="User Name" >
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Password" >
                            <ItemTemplate>
                                <asp:Label ID="lblPassword" runat="server" Text='<%#Eval("Password") %>'></asp:Label> 
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
                    <asp:TemplateField HeaderText="Create by" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreateBy" runat="server" Text='<%#Eval("CreatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Update by" >
                            <ItemTemplate>
                                <asp:Label ID="lblUpdateBy" runat="server" Text='<%#Eval("UpdatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField >
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server"  CssClass="btn-sm btn-primary"
                                        CommandName="Editting" CommandArgument='<%# Eval("UserID") %>'><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="btnDelete" CssClass="btn-sm btn-danger" runat="server" Text="Delete" commandName="Deleting" OnClientClick="return confirm('Are you sure you want to delete?');" CommandArgument='<%#Eval("UserID") %>'><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>
                            </ItemTemplate>  
                    </asp:TemplateField>
                </Columns>
           </asp:GridView>
            </div>
           </div>
    </div>
</asp:Content>