
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpeningBalance.aspx.cs" MasterPageFile="~/Account.Master" Inherits="Account.OpeningBalance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <script type="text/javascript">
            $(function () {
                $('#datetimepicker1').datetimepicker({
                    format: 'DD/MM/YYYY LT',
                    showClear: true
                });
                $('#datetimepicker2').datetimepicker({
                    format: 'DD/MM/YYYY LT',
                    showClear: true
                });
            });
            jQuery(function () {
                $(<%=btnCancel.ClientID %>).click(function () {
                    $(<%=txtbalanceUSD.ClientID %>).val("");
                    $(<%=txtbalanceKs.ClientID %>).val("");
                    $(<%=datetimepicker4.ClientID %>).val("");
                    $(<%=datetimepicker5.ClientID %>).val("");
            });
            });
         
       </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="container">
       <h1>Opening Balance Entry</h1>
          <br />
           <div class="row">
         <div class="col-lg-3" >
         <div class="form-group">  
            <label for="usr">Account Name</label>
                <asp:TextBox ID="txtid" runat="server" CssClass="input-control" Visible="false"></asp:TextBox>
                <asp:DropDownList ID="ddlAccountName" runat="server" CssClass="input-control"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="reqName" runat="server" ForeColor="Red" ErrorMessage="**Account Name is Required**" ControlToValidate="ddlAccountName" ValidationGroup="obal"></asp:RequiredFieldValidator> 
             <br />
            <label for="usr">Opening Balance (USD) </label>
                <asp:TextBox ID="txtbalanceUSD" runat="server" CssClass="input-control" onkeypress ="return isNumberKey(event)" Placeholder="Opening Balance(USD)..."></asp:TextBox>
<%--                <asp:RequiredFieldValidator ID="reqbalanceS" runat="server" ForeColor="Red" ErrorMessage="**Opening Balance is Required**" ControlToValidate="txtbalanceUSD" ValidationGroup="obal"></asp:RequiredFieldValidator>--%>
              <br />
              <label for="usr">Opening Balance (Kyat) </label>
                <asp:TextBox ID="txtbalanceKs" runat="server" CssClass="input-control" onkeypress ="return isNumberKey(event)" Placeholder="Opening Balance(Kyat)..."></asp:TextBox>
<%--                <asp:RequiredFieldValidator ID="reqbalanceKs" runat="server" ForeColor="Red" ErrorMessage="**Opening Balance is Required**" ControlToValidate="txtbalanceKs" ValidationGroup="obal"></asp:RequiredFieldValidator>--%>
              <br />

              <label for="usr">Opening Balance (￥) </label>
                <asp:TextBox ID="txtbalanceYen" runat="server" CssClass="input-control" onkeypress ="return isNumberKey(event)" Placeholder="Opening Balance(￥)..."></asp:TextBox>
<%--                <asp:RequiredFieldValidator ID="reqbalanceKs" runat="server" ForeColor="Red" ErrorMessage="**Opening Balance is Required**" ControlToValidate="txtbalanceKs" ValidationGroup="obal"></asp:RequiredFieldValidator>--%>
              <br />
             <label for="usr">Remarks </label>
             <asp:TextBox ID="txtRemarks" runat="server" CssClass="input-control" Placeholder="Remarks..." Height="75px" TextMode="MultiLine"></asp:TextBox>
              <br />
                <asp:Button  runat="server" ID="btnCreate" onclick="btnCreate_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="obal"/>
                <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" OnClick="btnUpdate_Click" Visible="false" ValidationGroup="obal"/>
                <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" Text="Cancel" />
             

              <div style="display:none;">
            <label for="usr">From Date</label>
             <div class="row" >
                <div class="calendar-container">
                  <div class="form-group">
                    <div class='input-group date' id='datetimepicker1'>
                        <asp:TextBox ID="datetimepicker4" runat="server" CssClass="form-control" Placeholder="From Date..." Visible="false" ></asp:TextBox>
                        <span class="input-group-addon" >
                                    <span class="glyphicon glyphicon-calendar" ></span>
                        </span>
                    </div>
                   </div>
                </div>
              </div>
                 </div>
<%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red" ErrorMessage="**From Date is Required**" ControlToValidate="datetimepicker4" ValidationGroup="obal"></asp:RequiredFieldValidator>--%>

            
               <div style="display:none">
                <label for="usr">To Date</label>
              <div class="row">
                <div class="calendar-container">
                  <div class="form-group">
                    <div class='input-group date' id='datetimepicker2'>
                        <asp:TextBox ID="datetimepicker5" runat="server" CssClass="form-control" Placeholder="To Date..." Visible="false"></asp:TextBox>
                            <span class="input-group-addon">
                                        <span class="glyphicon glyphicon-calendar"></span>
                            </span>
                    </div>
                   </div>
                 </div>
                </div>
                   </div>
<%--                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red" ErrorMessage="**From Date is Required**" ControlToValidate="datetimepicker5" ValidationGroup="obal"></asp:RequiredFieldValidator>--%>
            
         </div>
             </div>
               <div class="col-lg-9">
                   <br />
        <div class="table-responsive">    
           <asp:GridView ID="gdvOBal" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="cntrHeaderTxt" CssClass="table table-hover" OnRowCommand="gdvOBal_RowCommand" OnRowDataBound="gdvOBal_RowDataBound" AllowPaging="true" PageSize="7" OnPageIndexChanging="gdvOBal_PageIndexChanging" EmptyDataText="There is no opening balance entry">
                <PagerStyle CssClass="pagination-ys" />
                <HeaderStyle BackColor="#404c54" ForeColor="White" />
               <Columns>
                    <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAccID" runat="server" Text='<%#Eval("AccountsID") %>'></asp:Label> 
                                <asp:Label ID="lblID" runat="server" Text='<%#Eval("ID") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                                            
                    <asp:TemplateField HeaderText="Account Name" >
                            <ItemTemplate>
                                <asp:Label ID="lblAccountName" runat="server" Text='<%#Eval("AccountName") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="USD" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBalUSD" runat="server" Text='<%# !String.IsNullOrWhiteSpace(Eval("OpeningBalanceUSD").ToString())? Eval("OpeningBalanceUSD").ToString():"" %>'></asp:Label> 
                                <asp:Label ID="lblUnitUSD"  runat="server" Text="<br/> USD" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Kyat" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBalKs" runat="server" Text='<%# !String.IsNullOrWhiteSpace(Eval("OpeningBalanceKs").ToString())? Eval("OpeningBalanceKs").ToString():"" %>'></asp:Label> 
                                <asp:Label ID="lblUnitKs"  runat="server" Text="<br/> Kyat" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="￥" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBalYen" runat="server" Text='<%# !String.IsNullOrWhiteSpace(Eval("OpeningBalanceYen").ToString())? Eval("OpeningBalanceYen").ToString():"" %>'></asp:Label> 
                                <asp:Label ID="lblUnitYen"  runat="server" Text="<br/> ￥" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="USD" >
                            <ItemTemplate>
                                <asp:Label ID="lblCloseBalUSD" runat="server" Text='<%#  !String.IsNullOrWhiteSpace(Eval("ProcessAmtUSD").ToString())? Eval("ProcessAmtUSD").ToString():"" %>'></asp:Label> 
                                <asp:Label ID="lblUnitUSD1"  runat="server" Text="<br/> USD" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Kyat" >
                            <ItemTemplate>
                                <asp:Label ID="lblCloseBalKs" runat="server" Text='<%# !String.IsNullOrWhiteSpace(Eval("ProcessAmtKs").ToString())? Eval("ProcessAmtKs").ToString():"" %>'></asp:Label> 
                                <asp:Label ID="lblUnitKs1"  runat="server" Text="<br/> Kyat" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="￥" >
                            <ItemTemplate>
                                <asp:Label ID="lblCloseBalYen" runat="server" Text='<%# !String.IsNullOrWhiteSpace(Eval("ProcessAmtYen").ToString())? Eval("ProcessAmtYen").ToString():"" %>'></asp:Label> 
                                <asp:Label ID="lblUnitYen1"  runat="server" Text="<br/> ￥" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>

                    <%--<asp:TemplateField HeaderText="From Date" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFromDate" runat="server" Text='<%#Eval("FromDate","{0:dd/MM/yyyy}") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Remarks" >
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("oBLRemarks") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="To Date" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblToDate" runat="server" Text='<%#Eval("ToDate","{0:dd/MM/yyyy}") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Created Date" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreated_Date" runat="server" Text='<%#Eval("Created_Date","{0:dd/MM/yyyy}") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Updated Date" >
                            <ItemTemplate>
                                <asp:Label ID="lblUpdated_Date" runat="server" Text='<%#Eval("Updated_Date","{0:dd/MM/yyyy}") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CreateBy" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreateBy" runat="server" Text='<%#Eval("CreatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn-sm btn-primary"
                                        CommandName="Editting" CommandArgument='<%# Eval("ID") %>'  Visible="false"><span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                             &nbsp;<asp:LinkButton ID="btnDelete" CssClass="btn-sm btn-danger" runat="server" commandName="Deleting" OnClientClick="return confirm('Are you sure you want to delete?');" CommandArgument='<%#Eval("ID") %>' Visible="false"><span class="glyphicon glyphicon-trash"></span></asp:LinkButton>

                            </ItemTemplate>  
                    </asp:TemplateField>--%>
                </Columns>
           </asp:GridView>
        </div>
                   </div>
               </div>
    </div>
</asp:Content>
   

