<%@ Page Title="" Language="C#" MasterPageFile="~/Account.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="Account.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>
            .viewMore_pager
            {
                margin-top: 3px;
                margin-left: 30px;
                clear: both;
                /*margin-bottom: 20px;*/
                /*display: block;*/
                /*position: relative;*/
                top: 10px;
                /*width: 1000px;*/
                text-align: center;
            }
            .page_enabled, .page_disabled
            {
                display: inline-block;
                /*height: 25px;*/
                height:25px;
                min-width: 30px;
                text-align: center;
                text-decoration: none !important;
                border: 1px solid #ccc;
                border-radius: 3px;
                -moz-border-radius: 3px;
                -webkit-border-radius: 3px;
                font-size:13px;
                padding-top:5px;
            }
            .page_enabled
            {
                background-color: #404C54;
                color:#FFF;
            }
        
             .page_enabled:hover
            {
                background-color:#404C54 ;
                color: #FFF;
            }
        
            .page_disabled
            {
                background-color: #FFF;
                color: #404C54 !important;
            }

             .page_disabled:hover
            {
                background-color:#404C54;
                color: #FFF !important;
            }
            .hideGridColumn
            {
                display: none;
            }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container">
          <h1>Account</h1>
         <div class="row">              
          <div class="row" style="padding-bottom:20px">
              <div class="input-group-sm ">
                  <div class="col-sm-3 col-xs-offset-3" >
                     <label class="classlabel ">1 USD → </label>
                    <asp:TextBox ID="txtUSDtoYen" runat="server" CssClass="input-control-noBlock" onkeypress ="return isNumberKey(event)"  placeholder="" AutoPostBack="true" OnTextChanged="txtUSDtoYen_TextChanged"></asp:TextBox> ¥
                   </div>
                  <div class="col-sm-3 " > 
                      <label class="classlabel ">1 USD → </label>
                    <asp:TextBox ID="txtUSDtoKyat" runat="server" CssClass="input-control-noBlock" onkeypress ="return isNumberKey(event)"  placeholder="" AutoPostBack="true" OnTextChanged="txtUSDtoKyat_TextChanged"></asp:TextBox> Kyat
                  </div>                  
              </div>
              </div> 
             <div class="row">
             <div class="col-md-10" align="right" style="padding-right:0px ;padding-left:0px">
                        <asp:DataList ID="rptMonths" runat="server" RepeatDirection="Horizontal"  CssClass="viewMore_pager" OnItemDataBound="rptMonths_ItemDataBound" >
                           <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnMonth" OnClick="lnkBtnMonth_Click" CommandArgument='<%#Eval("MonthName")%>' CssClass="page_disabled"
                                 runat="server" Text='<%#Eval("MonthName").ToString()+"月" %>' Font-Bold="true"/></td>
                           </ItemTemplate>
                        </asp:DataList>
                           
               </div>
                       <label class="classlabel " style="float:left;padding-left:48px;padding-top:6px;">Year:</label>

                <div class="col-md-1" style="padding-left:11px;">
                    <div class="input-group-sm">
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true">

                    </asp:DropDownList>
                    </div>
                </div>
                </div>
          <div class="table-responsive">    
           <asp:GridView ID="gdvAccs" runat="server" HeaderStyle-CssClass="cntrHeaderTxt" AutoGenerateColumns="false"  CssClass="table table-hover" OnRowCommand="gdvAccs_RowCommand" OnRowDataBound="gdvAccs_RowDataBound" EmptyDataText="There is no entry">
               <HeaderStyle BackColor="#404c54" ForeColor="White" />
               <Columns>
                   <asp:TemplateField >
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDetail" runat="server" CssClass="btn-sm btn-primary"
                                        CommandName="Cmd_Detail" CommandArgument='<%# Eval("ID") %>'><span class="glyphicon glyphicon-list"></span></asp:LinkButton>
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
                    <asp:TemplateField HeaderText="USD" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBLUSD" runat="server" Text='<%# Convert.ToDecimal(Eval("ProcessAmtUSD")).ToString("N2") %>'></asp:Label> 
                                <asp:Label ID="lblUnitUSD"  runat="server" Text=" <br/>USD" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Kyat" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBLKs" runat="server" Text='<%#Convert.ToDecimal(Eval("ProcessAmtKs")).ToString("N2") %>'></asp:Label> 
                                <asp:Label ID="lblUnitKs"  runat="server" Text=" <br/>Kyat" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="¥" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBLYen1" runat="server" Text='<%#Convert.ToDecimal(Eval("ProcessAmtYen")).ToString("N2") %>'></asp:Label> 
                                <asp:Label ID="lblUnitYen1"  runat="server" Text=" <br/>¥" Font-Size="X-Small"></asp:Label>
                                </ItemTemplate>
                       </asp:TemplateField>
                   <asp:TemplateField HeaderText="¥" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBLYen" runat="server" ></asp:Label> 
                                <asp:Label ID="lblUnitYen"  runat="server" Text=" <br/>¥" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>  
                     <asp:TemplateField HeaderText="From Date" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFromDate" runat="server" Text=""></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="To Date"  Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblToDate" runat="server" Text=""></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>                
                </Columns>
           </asp:GridView>
            </div>
         
         <div class="col-md-8 col-xs-offset-2">
            <div class="table-responsive">    
                <asp:GridView ID="gdvTotalOBL" runat="server" HeaderStyle-CssClass="cntrHeaderTxt"  AutoGenerateColumns="false" ShowFooter="true"  CssClass="table table-hover" OnRowDataBound="gdvTotalOBL_RowDataBound" OnRowCreated="gdvTotalOBL_RowCreated" >
                    <HeaderStyle BackColor="#404c54" ForeColor="White" />
                    <Columns>                   
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
                    <asp:TemplateField HeaderText="USD" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBLUSD" runat="server" Text='<%#Convert.ToDecimal( Eval("ProcessAmtUSD")).ToString("N2") %>'></asp:Label> 
                                <asp:Label ID="lblUnitUSD"  runat="server" Text=" <br/>USD" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField HeaderText="Kyat" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBLKs" runat="server" Text='<%#Convert.ToDecimal(Eval("ProcessAmtKs")).ToString("N2") %>'></asp:Label> 
                                <asp:Label ID="lblUnitKs"  runat="server" Text=" <br/>Kyat" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField> 
                         <asp:TemplateField HeaderText="￥" >
                             <ItemTemplate>
                             <asp:Label ID="lblOBLYen1" runat="server" Text='<%#Convert.ToDecimal(Eval("ProcessAmtYen")).ToString("N2") %>'></asp:Label> 
                                <asp:Label ID="lblUnitYen1"  runat="server" Text=" <br/>￥" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                            
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="¥" >
                            <ItemTemplate>
                                <asp:Label ID="lblOBLYen" runat="server" ></asp:Label> 
                                <asp:Label ID="lblUnitYen"  runat="server" Text=" <br/>¥" Font-Size="X-Small"></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>   
                                
                </Columns>
                    
                </asp:GridView>
            </div>
        </div>
     </div>
     </div>
</asp:Content>
