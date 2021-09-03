<%@ Page Title="" Language="C#" MasterPageFile="~/Account.Master" AutoEventWireup="true" CodeBehind="Menu_Entry.aspx.cs" Inherits="Account.Admin.Menu_Entry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <%--  <script type="text/javascript">
        jQuery(function () {
            $(<%=btnCancel.ClientID %>).click(function () {
                $('#chkLstRoles:checked').removeAttr('checked');
                $(<%=chkLstForms.ClientID %>).removeAttr('checked');
            });
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container">
       <h1>Menu Entry</h1>
         <div class="row">
             <div class="col-lg-3">
         <div class="form-group">  
             <label for="usr">Roles</label>
             <asp:HiddenField ID="hdfID" runat="server" Visible="false" />
             <asp:DropDownList ID="ddlRoles" runat="server" CssClass="input-control" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged" AutoPostBack="true"/>
             <br />           
        </div>
         </div>      
        </div>
         <div class="row">
             <div class="panel panel-default">
        <div class="table-responsive">    
           <asp:GridView ID="gdvMenus" runat="server" AutoGenerateColumns="false"  GridLines="None"  CssClass="table table-hover"  AllowPaging="true" PageSize="9" OnPageIndexChanging="gdvMenus_PageIndexChanging" EmptyDataText="There is no menu entry" PagerSettings-Mode="Numeric" OnRowEditing="gdvMenus_RowEditing" OnRowUpdating="gdvMenus_RowUpdating" OnRowCancelingEdit="gdvMenus_RowCancelingEdit" OnRowDataBound="gdvMenus_RowDataBound">
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
                    <asp:TemplateField HeaderText="Forms" >
                            <ItemTemplate>
                                <asp:Label ID="lblMenuName" runat="server" Text='<%#Eval("FormName") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="FormID"  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFormID" runat="server" Text='<%#Eval("FormID") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Parent Menu" >
                            <ItemTemplate>
                                <asp:Label ID="lblParentMenu" runat="server" Text='<%#Eval("ParentMenu") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="CanRead" >
                            <ItemTemplate>
                                <asp:Label ID="lblRoles" runat="server" Text='<%#String.IsNullOrWhiteSpace(Eval("RoleName").ToString())?"":"√" %>'></asp:Label> 
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="edtChkCanRead" runat="server" Text="" AutoPostBack="true" CssClass="mastercheckbox" Checked='<%#String.IsNullOrWhiteSpace(Eval("RoleName").ToString())? false: true %>' OnCheckedChanged="edtChkCanRead_CheckedChanged" />             
                            </EditItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="CanEdit" >
                            <ItemTemplate>
                                <asp:Label ID="lblCanEdt" runat="server" Text='<%#String.IsNullOrWhiteSpace(Eval("canEdit").ToString())?"":"√" %>'></asp:Label> 
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="edtChkCanEdt" runat="server" Text="" CssClass="mastercheckbox" Checked='<%#String.IsNullOrWhiteSpace(Eval("canEdit").ToString())? false: true %>' AutoPostBack="true" OnCheckedChanged="edtChkCanEdt_CheckedChanged" />             
                            </EditItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="CanDelete" >
                            <ItemTemplate>
                                <asp:Label ID="lblCanDel" runat="server" Text='<%#String.IsNullOrWhiteSpace(Eval("canDel").ToString())?"":"√" %>'></asp:Label> 
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="edtChkCanDel" runat="server" Text="" CssClass="mastercheckbox" Checked='<%#String.IsNullOrWhiteSpace(Eval("canDel").ToString())? false: true %>'  AutoPostBack="true" OnCheckedChanged="edtChkCanDel_CheckedChanged" />             
                            </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="RoleID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRoleID" runat="server" Text='<%#Eval("RoleID") %>' Visible="false"></asp:Label> 
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
                   <asp:TemplateField HeaderText="Create By" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedUsr" runat="server" Text='<%#Eval("CreateBy") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Update By" >
                            <ItemTemplate>
                                <asp:Label ID="lblUpdatedUsr" runat="server" Text='<%#Eval("UpdateBy") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>

                    <asp:CommandField  ShowEditButton="true"  ButtonType="Link" ControlStyle-CssClass="btn-sm btn-primary"  EditText="<i class='glyphicon glyphicon-pencil'></i>" UpdateText="<i class='glyphicon glyphicon-ok'></i>" CancelText="<i class='glyphicon glyphicon-ban-circle'></i>"/>
                </Columns>
           </asp:GridView>
            </div>
             </div>
         </div>
    </div>
</asp:Content>
