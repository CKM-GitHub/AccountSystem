<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountEntry.aspx.cs" Inherits="Account.AccountEntry" MasterPageFile="~/Account.Master"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <%--<script type="text/javascript">
          jQuery(function () {
              $(<%=btnCancel.ClientID %>).click(function () {
                 
                $(<%=txtAccName.ClientID %>).val("");
                  $(<%=txtAccNo.ClientID %>).val("");
                 
            });
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
       <h1>Account Entry</h1>
        <div class="row">
            <div class="col-lg-3 " style="padding-right:5px">
                <div class="input-group-sm">
                <asp:HiddenField ID="hdfID" runat="server" Visible="false" />
                 <label for="usr">Account Name</label>
                <asp:TextBox ID="txtAccName" runat="server" placeholder="Account..." CssClass="input-control" onkeypress="return ctrlEnter(event)"></asp:TextBox>
                </div>
            </div>
            <div class="col-lg-9">
            <div class="input-group-sm" style="padding-top:15px;float:left"> 
                <asp:CheckBox ID="chkCmpAcc" runat="server" Text="Listed Account" CssClass="mastercheckbox" />
                <br />
            </div>
             </div>
           <div class="row">
            <div class="col-lg-10 " >
            <div class="input-group-sm" style="padding-left:15px">
                  <label for="usr">Roles</label> 
                <asp:CheckBoxList ID="chkLstRoles" runat="server" CssClass="mastercheckbox"></asp:CheckBoxList>             
             </div>
                </div>
           </div>

            <div class="input-group-sm" style="padding-left:15px">
                <asp:RequiredFieldValidator ID="reqAccName" runat="server" ForeColor="Red" ErrorMessage="**Acc Name is Required**" ControlToValidate="txtAccName" ValidationGroup="Acc"></asp:RequiredFieldValidator> 
            
                <asp:TextBox ID="txtAccNo" runat="server" CssClass="form-username form-control" Visible="false"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqAccID" runat="server" Enabled="false" ForeColor="Red" ErrorMessage="**Acc No is Required**" ControlToValidate="txtAccNo"  ValidationGroup="Acc"></asp:RequiredFieldValidator>
              <br />
                <asp:Button  runat="server" ID="btnSave" onclick="btnSave_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="Acc"/>
              <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
            </div>
          </div>
        <div class="row" style="padding-left:15px">
        <div class="panel panel-default" >
        <div class="table-responsive" >             
           <asp:GridView ID="gdvAccs" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-hover" OnRowCommand="gdvAccs_RowCommand" AllowPaging="true" PageSize="7" OnPageIndexChanging="gdvAccs_PageIndexChanging" EmptyDataText="There is no account entry" OnRowDataBound="gdvAccs_RowDataBound"> 
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
                                            
                    <asp:TemplateField HeaderText="Account Name" >
                            <ItemTemplate>
                                <asp:Label ID="lblAccName" runat="server" Text='<%#Eval("AccName") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Listed Account" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblIsRemainingTotal" runat="server" Text='<%#Eval("isRemainingTotal").ToString()=="1"?"√":""%>' ></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Roles" >
                            <ItemTemplate>
                                <asp:Label ID="lblRoleID" runat="server" Text='<%#Eval("RoleID") %>' Visible="false"></asp:Label> 
                                <asp:Label ID="lblRoles" runat="server" Text='<%#Eval("RoleName") %>'></asp:Label> 
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
    </div>
</asp:Content>
