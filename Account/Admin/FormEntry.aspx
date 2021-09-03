<%@ Page Title="" Language="C#" MasterPageFile="~/Account.Master" AutoEventWireup="true" CodeBehind="FormEntry.aspx.cs" Inherits="Account.Admin.FormEntry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%--   <script type="text/javascript">
        jQuery(function () {
            $(<%=btnCancel.ClientID %>).click(function () {
                $(<%=txtFrmName.ClientID %>).val("");
                $(<%=txtFrmDir.ClientID %>).val("");
                $(<%=txtParent.ClientID %>).val("");
            });
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
         <div class="container">
              <h1>Form Entry</h1>
    <div class="row">
         <div class="col-lg-3" >
      
         <div class="form-group">  
                <asp:HiddenField ID="hdfID" runat="server" Visible="false" />
                 <label for="usr">Form Name</label>
                <asp:TextBox ID="txtFrmName" runat="server" placeholder="Form Name..." CssClass="input-control" onkeypress="return ctrlEnter(event)"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqFrmName" runat="server" ForeColor="Red" ErrorMessage="**Form Name is Required**" ControlToValidate="txtFrmName" ValidationGroup="Form"></asp:RequiredFieldValidator> 
              <br />
               <label for="usr">Directory Name</label>
                <asp:TextBox ID="txtFrmDir" runat="server" placeholder="Directory Name..." CssClass="input-control" onkeypress="return ctrlEnter(event)"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqDirName" runat="server" ForeColor="Red" ErrorMessage="**Directory Name is Required**" ControlToValidate="txtFrmDir" ValidationGroup="Form"></asp:RequiredFieldValidator> 
              <br />
              <label for="usr">Parent Menu</label>
                <asp:TextBox ID="txtParent" runat="server" placeholder="Parent Menu..." CssClass="input-control" onkeypress="return ctrlEnter(event)"></asp:TextBox>
              <br />
                <asp:Button  runat="server" ID="btnSave" onclick="btnSave_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="Form"/>
              <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
        </div>
             </div>
         <div class="col-lg-9" >
            <div class="panel panel-default">
        <div class="table-responsive">    
           <asp:GridView ID="gdvFrms" runat="server" AutoGenerateColumns="false" GridLines="None" CssClass="table table-hover" OnRowCommand="gdvFrms_RowCommand" AllowPaging="true" PageSize="9" OnPageIndexChanging="gdvFrms_PageIndexChanging" EmptyDataText="There is no form entry" OnRowDataBound="gdvFrms_RowDataBound">
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
                                            
                    <asp:TemplateField HeaderText="Form Name" >
                            <ItemTemplate>
                                <asp:Label ID="lblFormName" runat="server" Text='<%#Eval("FormName") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Form Directory" >
                            <ItemTemplate>
                                <asp:Label ID="lblFormDir" runat="server" Text='<%#Eval("DirectoryPath") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>

                   <asp:TemplateField HeaderText="Parent Menu" >
                            <ItemTemplate>
                                <asp:Label ID="lblParent" runat="server" Text='<%#Eval("ParentMenu") %>'></asp:Label> 
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
                    <asp:TemplateField >
                            <ItemTemplate>
                             <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn-sm btn-primary"
                                        CommandName="Cmd_Edit" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Are you sure you want to update?');"><span class="glyphicon glyphicon-pencil" ></span></asp:LinkButton>
                            <br />
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
    </div>
</asp:Content>
