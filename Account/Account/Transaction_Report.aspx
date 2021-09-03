<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Transaction_Report.aspx.cs" Inherits="Account.Transaction_Report" MasterPageFile="~/Account.Master" EnableEventValidation = "false"   %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
     <script type="text/javascript">
         function __doPostBack(eventTarget, eventArgument) {
             if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                 theForm.__EVENTTARGET.value = eventTarget;
                 theForm.__EVENTARGUMENT.value = eventArgument;
                 theForm.submit();
             }
         }
    </script>
        <script type="text/javascript" language="javascript">
            $(document).ready(function(){
            //$('#datetimepicker1').datetimepicker();
            $('#datetimepicker1').datetimepicker({
                format: 'DD/MM/YYYY LT',
                showClear: true,
                useCurrent: false
               
            }).on('dp.change', function (e) {
                
              //__doPostBack("txtFromDate", "TextChanged");
            });
            $('#datetimepicker2').datetimepicker({
                format: 'DD/MM/YYYY LT',
                showClear: true,
                useCurrent: false

            }).on('dp.change', function (e) {
               
               //__doPostBack("txtFromDate", "TextChanged");

            });
            $('#edtDate').datetimepicker({
                format: 'DD/MM/YYYY',
                showClear: true
            }).on('dp.change', function (e) {
                if (!e.oldDate || !e.date.isSame(e.oldDate, 'day')) {
                    $(this).data('DateTimePicker').hide();
                }

            });
            });
            var retval = null;
            function ShowAtta_PopUp(ctrl) {
                var width = 600;
                var height = 500;
                var left = (screen.width - width) / 2;
                var top = (screen.height - height) / 2;
                var params = 'width=' + width + ', height=' + height;
                params += ', top=' + top + ', left=' + left;
                params += ', toolbar=no';
                params += ', menubar=no';
                params += ', resizable=yes';
                params += ', directories=no';
                params += ', scrollbars=yes';
                params += ', status=no';
                params += ', location=no';
                params += ',setBackgroundDrawable=yes ';
                 retval = window.open('<%= ResolveClientUrl("~/attPop")%>', window, params);

                DisabledEffect();

              if (window.focus) {
                  retval.focus()
              }
              return false;
            }
            function DisabledEffect() {
                $("[id$=MainDiv]").style.display = '';
                $('#MainDiv').style.visibility = 'visible';
                $('#MainDiv').style.position = 'absolute';

                $('#MainDiv').style.top = '0px';
                $('#MainDiv').style.left = '0px';
                $('#MainDiv').style.width = '100%';
                $('#MainDiv').style.height = '100%';

                $('#MainDiv').style.backgroundColor = "Gray";
                $('#MainDiv').style.filter = "alpha(opacity=60)";
                $('#MainDiv').style.opacity = "0.6";

                //alert("Parent window in disabled effect!!!");

                $('#MainDiv').style.display = 'none';
                $('#MainDiv').style.visibility = 'hidden';

                $('#MainDiv').style.top = '0px';
                $('#MainDiv').style.left = '0px';
                $('#MainDiv').style.width = '0px';
                $('#MainDiv').style.height = '0px';
            }
            function parent_disable(ctrL) {
                if (retval && !retval.closed)
                    retval.focus();
            }
           
    </script> 
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
                height:30px;
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
    <div onclick="parent_disable(this);">
          <center>
         <div class="container-fluid" id="MainDiv" >
        <div class="container">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
            <h1>Transaction Report</h1>
              <table  style="text-align:right;" >
                <tr >
                    <td  style="padding-left:4px">
                        <label class="classlabel ">Account : </label>
                    </td>
                    <td>
                         <div class="input-group" style="padding-left:4px;padding-bottom:10px">
                        <asp:UpdatePanel ID="upSetSession1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                           <asp:DropDownList ID="ddlAccName" runat="server" CssClass="input-control"  OnSelectedIndexChanged="ddlAccName_SelectedIndexChanged"></asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlAccName" 
                        EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
                        </div>
                    </td>
                    <td colspan="4" style="padding-left:55px">
                      <div class="row">
                        <div class="col-md-9" align="right" style="padding-right:0px ;padding-left:11px">
                        <asp:DataList ID="rptMonths" runat="server" RepeatDirection="Horizontal"  CssClass="viewMore_pager" OnItemDataBound="rptMonths_ItemDataBound" >
                           <ItemTemplate>
                                <asp:UpdatePanel ID="upMonth" runat="server" >
                        <ContentTemplate>

                                <asp:LinkButton ID="lnkBtnMonth" OnClick="lnkBtnMonth_Click" CommandArgument='<%#Eval("MonthName")%>' CssClass="page_disabled"
                                 runat="server" Text='<%#Eval("MonthName").ToString()+"月" %>' Font-Bold="true"/></td>
                            </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkBtnMonth" EventName="Click" />
                </Triggers>
                </asp:UpdatePanel>

                           </ItemTemplate>
                        </asp:DataList>
                           
                        </div>
                       <label class="classlabel " style="float:left;padding-left:3px;padding-top:10px;padding-right:2px;">Year:</label>
                           <asp:UpdatePanel ID="upSetSession2" runat="server">
                        <ContentTemplate>
                        <div class="col-md-2" style="padding-bottom:10px;padding-left:0px;">
                            <div class="input-group-sm" style="padding-top: 3px">
                       
                        <asp:DropDownList ID="ddlYear" runat="server" width="83px" CssClass="form-control" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                         </div>
                        </div>
                          </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlYear" 
                        EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
                        </div>
                      </div>
                    </td>
                    
                </tr> 
                  <tr>
                      <td colspan="2">

                      </td>
                      <td>
                        <label class="classlabel " style="padding-left:4px">From Date : </label>
                    </td>
                    <td>
                       <div class="input-group" style="padding-left:4px;padding-bottom:5px">
                         <div class="row">
                            <div class="calendar-container" >
                               <asp:UpdatePanel ID="UpFrom"  runat="server" UpdateMode="Conditional">
                                     <ContentTemplate>
                              <div class='input-group date' id='datetimepicker1' style="padding-left: 4px">
                                   
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-username form-control" placeholder="From Date..." OnTextChanged="txtFromDate_TextChanged" AutoPostBack= "True" ></asp:TextBox>
                                        
                                                        <span class="input-group-addon">
                                              <span class="glyphicon glyphicon-calendar" ></span>
                                        </span>
                                </div>  
                            </ContentTemplate>
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtFromDate" 
                            EventName="TextChanged" />
                            </Triggers>
                            </asp:UpdatePanel>
                             </div>
                         </div>
                         </div>
                    </td>
                    <td>
                        <label class="classlabel ">To Date : </label>
                    </td>
                    <td>
                       <div class="input-group" style="padding-left:4px;padding-bottom:5px">
                         <div class="row">
                            <div class="calendar-container" >
                              <div class="form-group">
                                  <asp:UpdatePanel ID="UpTo"  runat="server" UpdateMode="Conditional">
                                     <ContentTemplate>
                               
                                 <div class='input-group date' id='datetimepicker2' style="padding-left: 4px; top: 0px; left: 0px;">
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-username form-control" placeholder="To Date..." OnTextChanged="txtToDate_TextChanged" AutoPostBack= "True"></asp:TextBox>
                                        
                                        <span class="input-group-addon">
                                                    <span class="glyphicon glyphicon-calendar"></span>
                                        </span>
                                </div>
                                          </ContentTemplate>
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="txtToDate" 
                                        EventName="TextChanged" />
                                        </Triggers>
                                        </asp:UpdatePanel>
                             </div>
                            </div>
                         </div>
                         </div>
                    </td>
                  </tr>             
                <tr>
                    <td>
                        <label class="classlabel ">Type : </label>
                    </td>
                    <td>
                        <div class="input-group" style="padding-left:4px;padding-bottom:5px">
                            <asp:UpdatePanel ID="upSetSession4" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlTransType" runat="server" CssClass="input-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTransType_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlTransType" 
                        EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
                       </div>
                    </td>
                    <td>
                         <label class="classlabel ">Status : </label>
                    </td>
                    <td>
                         <div class="input-group" style="padding-left:4px;padding-bottom:5px">
                             <asp:UpdatePanel ID="upSetSession3" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="input-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlStatus" 
                        EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
                       </div>
                    </td>
                    <td>
                         <label class="classlabel" style="padding-left:4px;padding-bottom:5px">Cash Unit : </label>
                    </td>
                    <td>
                        
                        <div class="input-group">
                            <asp:UpdatePanel ID="upSetSession5" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCashUnit" runat="server" CssClass="input-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCashUnit_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCashUnit" 
                        EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
                       </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                    <td colspan="2"> 
                        <asp:UpdatePanel ID="Urdo" runat="server">
                        <ContentTemplate>
                       <center><asp:RadioButtonList ID="rdOpt" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdOpt_SelectedIndexChanged" >
                            <asp:ListItem Value="1" Selected="True" Text="Overall&nbsp;&nbsp;"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Specific"></asp:ListItem>
                        </asp:RadioButtonList>    
                     </center> 
                            </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rdOpt" 
                        EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
                    </td>   
                    <td colspan="2"></td>
                </tr>
              </table>
        <br />
            <asp:Button  runat="server" ID="btnSearch" onclick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" />
            <asp:Button  runat="server" ID="btnExport" onclick="btnExport_Click"  Text="Export" CssClass="btn btn-primary" Visible="false"/>
           <asp:Button  runat="server" ID="btnExportPDF" Text="Export PDF" CssClass="btn btn-primary"  OnClick="btnExportPDF_Click" Visible="false" />
            <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" Text="Cancel" OnClick="btnCancel_Click"/>
               
   </div>  
                
    </center>
           <br />
     <div class="row col-xs-offset-7" id="divLast" >
         <asp:Panel ID="pnlLast" runat="server">
         <%--<asp:Label ID="Label1" Text="<b>Closing From:</b> " runat="server" ></asp:Label>
         <asp:Label ID="lblLastDate" runat="server" ></asp:Label>
         <asp:Label ID="Label2" Text="<b>USD:</b> " runat="server"></asp:Label>
         <asp:Label ID="lblLastOBLUSD" runat="server"></asp:Label>
         <asp:Label ID="Label3"  Text="<b>Kyat:</b> " runat="server"></asp:Label>
         <asp:Label ID="lblLastOBLKs" runat="server"></asp:Label>--%>
        <asp:Label ID="lblLstOBL" runat="server" />
        </asp:Panel> 
     </div>
    <center>
       <%-- <asp:UpdatePanel ID="UPanel" runat="server" UpdateMode="Conditional" >
            <ContentTemplate>--%>
        <div class="table-condensed">
            <asp:GridView ID="gdvTransReport" runat="server"  EnableViewState="false" Visible="false" HeaderStyle-CssClass="cntrHeaderTxt" AutoGenerateColumns="false"
                 CssClass="table-condensed" OnRowDataBound="gdvTransReport_RowDataBound" ShowFooter="true" OnRowCommand="gdvTransReport_RowCommand"
                OnRowCreated="gdvTransReport_RowCreated"  EmptyDataText="There is no transaction entry" >
                     <PagerStyle CssClass="pagination-ys" />
                     <HeaderStyle BackColor="#404c54" ForeColor="White" />
                     <RowStyle Font-Size="14px"/>
                    <Columns> 
                    <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>                              
                                <asp:Label ID="lblEntryDate" runat="server" Text='<%#Eval("Date","{0:dd/MM/yyyy}") %>'></asp:Label> 
                            </ItemTemplate>
                            <%-- <EditItemTemplate>
                                 <div class="row">
                                    <div style="width:170px; position: relative; min-height: 1px;padding-right: 15px; padding-left: 15px;"> 
                                      <div class="form-group">
                                         <div class='input-group date' id='edtDate'>
                                                <asp:TextBox ID="txtEditDate" runat="server" CssClass="form-username form-control" Text='<%#Eval("Date","{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                                <span class="input-group-addon">
                                                            <span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                        </div>
                                     </div>
                                    </div>
                                 </div>
                            </EditItemTemplate>--%>
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="AccName">
                        <ItemTemplate>
                            <asp:Label ID="lblAccName" runat="server" Text='<%#Eval("AccountName") %>'></asp:Label> 
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                             <asp:DropDownList ID="ddlEditAcc" runat="server" Width="85px" CssClass="form-control input-sm" >

                             </asp:DropDownList>
                         </EditItemTemplate>--%>
                    </asp:TemplateField>       
                    <asp:TemplateField  HeaderText="Type" >
                        <ItemTemplate >
                            <asp:Label ID="lblTransType" runat="server" Text='<%# Eval("Type") %>' />
                            <asp:Label ID="lblTransTyID" runat="server" Text='<%# Eval("TransType")%>' Visible="false"/>
                        </ItemTemplate>
                         <%--<EditItemTemplate>
                             <asp:DropDownList ID="ddlEditTransTyp" runat="server" Width="85px" CssClass="form-control input-sm" >

                             </asp:DropDownList>
                         </EditItemTemplate>--%>
                    </asp:TemplateField> 
                    <asp:TemplateField  HeaderText="Particular" >
                    <ItemTemplate >
                        <asp:Label ID="lblParticular" runat="server" Text='<%# Eval("Particular") %>' />
                    </ItemTemplate>
                    <%--<EditItemTemplate>
                            <asp:TextBox ID="txtParticular" runat="server" width="120px"  height="41px" TextMode="MultiLine" CssClass="form-control input-sm"  Text='<%#Eval("Particular") %>' ></asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>  
                    
                    <asp:TemplateField HeaderText="USD" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblExpenseS" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseUSD").ToString())?Eval("ExpenseUSD").ToString():"" %>'></asp:Label> 
                        
                            <asp:Label ID="lblUnitUSD1"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseUSD").ToString())? " <br/>USD": "" %>' Font-Size="X-Small"></asp:Label>

                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:TextBox ID="txtExpenseUSD" runat="server" Width="85px" CssClass="form-control input-sm" Text='<%#Eval("ExpenseUSD") %>' onkeypress ="return isNumberKey(event)"></asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kyat" >
                        <ItemTemplate>
                            <asp:Label ID="lblExpenseKs" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseKs").ToString())?Eval("ExpenseKs").ToString():"" %>'></asp:Label> 
                                                            
                             <asp:Label ID="lblUnitKs1"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseKs").ToString())? " <br/>Kyat": "" %>' Font-Size="X-Small"></asp:Label>

                        </ItemTemplate>
                       <%-- <EditItemTemplate>
                            <asp:TextBox ID="txtExpenseKs" runat="server" Width="85px" CssClass="form-control input-sm" Text='<%#Eval("ExpenseKs") %>' onkeypress ="return isNumberKey(event)"></asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>
                        <asp:TemplateField HeaderText="￥" >
                        <ItemTemplate>
                            <asp:Label ID="lblExpenseYen" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseYen").ToString())?Eval("ExpenseYen").ToString():"" %>'></asp:Label> 
                                                            
                             <asp:Label ID="lblUnitYen1"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseYen").ToString())? " <br/>￥": "" %>' Font-Size="X-Small"></asp:Label>

                            </ItemTemplate>
                            <%--<EditItemTemplate>
                            <asp:TextBox ID="txtExpenseYen" runat="server" Width="85px" CssClass="form-control input-sm" Text='<%#Eval("ExpenseYen") %>' onkeypress ="return isNumberKey(event)"></asp:TextBox>
                        </EditItemTemplate>--%>



                            </asp:TemplateField>
                    <asp:TemplateField HeaderText="USD" >
                            <ItemTemplate>
                                <asp:Label ID="lblIncomeS" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeUSD").ToString())?Eval("IncomeUSD").ToString():"" %>'></asp:Label> 
                            
                                <asp:Label ID="lblUnitUSD2"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeUSD").ToString())? " <br/>USD": "" %>' Font-Size="X-Small"></asp:Label>

                            </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:TextBox ID="txtIncomeUSD" runat="server" Width="85px" CssClass="form-control input-sm" Text='<%#Eval("IncomeUSD") %>' onkeypress ="return isNumberKey(event)"></asp:TextBox>
                        </EditItemTemplate>--%>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Kyat" >
                            <ItemTemplate>
                                <asp:Label ID="lblIncomeKs" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeKs").ToString())?Eval("IncomeKs").ToString():"" %>'></asp:Label> 

                                 <asp:Label ID="lblUnitKs2"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeKs").ToString())? " <br/>Kyat": "" %>' Font-Size="X-Small"></asp:Label>

                            </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:TextBox ID="txtIncomeKs" runat="server" Width="85px" CssClass="form-control input-sm"  Text='<%#Eval("IncomeKs") %>' onkeypress ="return isNumberKey(event)"></asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField>

                         <asp:TemplateField HeaderText="￥" >
                        <ItemTemplate>
                            <asp:Label ID="lblIncomeYen" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeYen").ToString())?Eval("IncomeYen").ToString():"" %>'></asp:Label> 

                                 <asp:Label ID="lblUnitYen2"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeYen").ToString())? " <br/>￥": "" %>' Font-Size="X-Small"></asp:Label>

                            </ItemTemplate>
                             <%--<EditItemTemplate>
                            <asp:TextBox ID="txtIncomeYen" runat="server" Width="85px" CssClass="form-control input-sm"  Text='<%#Eval("IncomeYen") %>' onkeypress ="return isNumberKey(event)"></asp:TextBox>
                        </EditItemTemplate>--%>
                            </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="USD" ><%--(((Label)gdvTransReport.Rows[Container.DataItemIndex-1].FindControl("lblRemainUSD")).Text)--%>
                            <ItemTemplate >
                            <asp:Label ID="lblRemainUSD" runat="server" Text='<%#Eval("ResultAmtUSD")%>' Visible="false" Type="Integer"></asp:Label> 
                             <asp:Label ID="lblHidAcc" runat="server" Text='<%#Eval("AccountName") %>' Visible="false"></asp:Label> 

                            <asp:Label ID="lblNewRemainUSD" runat="server" 

                                Text='<%# 
                             Convert.ToDecimal(Eval("OpeningBalanceUSD").ToString())                            
                             %>'>
                            </asp:Label>  
                         
                            <asp:Label ID="lblUnitUSD3"  runat="server" Text="<br/> USD" Font-Size="X-Small"></asp:Label>

                            </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Kyat" >
                            <ItemTemplate>
                                <asp:Label ID="lblRemainKs" runat="server" Text='<%#Eval("ResultAmtKs") %>' Visible="false" ></asp:Label> 
                                <asp:Label ID="lblHidAcc2" runat="server" Text='<%#Eval("AccountName") %>' Visible="false"></asp:Label> 

                                <asp:Label ID="lblNewRemainKs" runat="server"
                                Text='<%#
                             Convert.ToDecimal(Eval("OpeningBalanceKs").ToString())                             
                                       %>'>
                            </asp:Label>                                

                                 <asp:Label ID="lblUnitKs3"  runat="server" Text="<br/> Kyat" Font-Size="X-Small" ></asp:Label>

                            </ItemTemplate>
                    </asp:TemplateField>
                         <asp:TemplateField HeaderText="￥" >
                        <ItemTemplate>
                            <asp:Label ID="lblRemainYen" runat="server" Text='<%#Eval("ResultAmtYen") %>' Visible="false" ></asp:Label> 
                                <asp:Label ID="lblHidAcc3" runat="server" Text='<%#Eval("AccountName") %>' Visible="false"></asp:Label> 

                                <asp:Label ID="lblNewRemainYen" runat="server"
                                Text='<%#
                             Convert.ToDecimal(Eval("OpeningBalanceYen").ToString())                             
                                       %>'>
                            </asp:Label>                                

                                 <asp:Label ID="lblUnitYen3"  runat="server" Text="<br/> ￥" Font-Size="X-Small" ></asp:Label>
                            </ItemTemplate>
                            </asp:TemplateField>


                    <asp:TemplateField  HeaderText="Remark" >
                    <ItemTemplate >
                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' />
                    </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:TextBox ID="txtRemarks" runat="server" width="120px"  height="41px" CssClass="form-control input-sm"  Text='<%#Eval("Remarks") %>' TextMode="MultiLine"></asp:TextBox>
                        </EditItemTemplate>--%>
                    </asp:TemplateField> 

                    <asp:TemplateField  HeaderText="Attachments" >
                    <ItemTemplate >
                        <asp:LinkButton ID="lnkTransAttach" runat="server" Text='<%#String.IsNullOrWhiteSpace(Eval("isAttached").ToString())? " ": "See Attachments"%>'  OnClick="lnkTransAttach_Click" CommandArgument='<%# Eval("TransID") %>' CommandName='<%# Eval("ACCID") %>'/>
                    </ItemTemplate>                     
                    </asp:TemplateField> 
    
                    <asp:TemplateField HeaderText="CreateBy" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreateBy" runat="server" Text='<%#Eval("CreatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UpdateBy" >
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfID" Value='<%#Eval("TransID") %>' runat="server" />
                                <asp:HiddenField ID="hdfAccID" Value='<%#Eval("ACCID") %>' runat="server"/>
                                <asp:HiddenField ID="hdfStsID" Value='<%#Eval("StatusID") %>' runat="server"/>

                                <asp:Label ID="lblUpdateBy" runat="server" Text='<%#Eval("UpdatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                  <%--  <asp:TemplateField HeaderText="CreatedDate" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("Created_Date", "{0:dd/MM/yyyy hh:mm}")%>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>--%>
                  
                       <%-- <asp:CommandField ShowEditButton="true"  ButtonType="Link" ControlStyle-CssClass="btn-sm btn-primary"  EditText="<i class='glyphicon glyphicon-pencil'></i>" UpdateText="<i class='glyphicon glyphicon-ok'></i>" CancelText="<i class='glyphicon glyphicon-ban-circle'></i>"/>
                        <asp:CommandField  ShowDeleteButton="true"  ButtonType="Link" ControlStyle-CssClass="btn-sm btn-danger" DeleteText="<i class='glyphicon glyphicon-trash'></i>" />--%>
                <%--<asp:ButtonField ShowHeader="true" Text="<i class='glyphicon glyphicon-pencil'></i>"></asp:ButtonField>--%>
                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUpdate" runat="server" CssClass="btn-sm btn-primary" CommandName="Edit" CommandArgument="<%# Container.DataItemIndex %>" Text="<i class='glyphicon glyphicon-pencil'></i>"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkdelete" runat="server" CssClass="btn-sm btn-danger" CommandName="Delete" CommandArgument="<%# Container.DataItemIndex %>" Text="<i class='glyphicon glyphicon-trash'></i>"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                        
                </Columns>
                <FooterStyle BorderWidth="0px"/>
                </asp:GridView>
        </div>
        <div class="table-condensed" >   
            <asp:GridView ID="gdvTransReportSnd" runat="server" Visible="false" EnableViewState="false" HeaderStyle-CssClass="cntrHeaderTxt" 
                AutoGenerateColumns="false" CssClass="table-condensed" OnRowDataBound="gdvTransReportSnd_RowDataBound" 
                ShowFooter="true" OnRowCreated="gdvTransReportSnd_RowCreated"  EmptyDataText="There is no transaction entry" >
                     <PagerStyle CssClass="pagination-ys" />
                     <HeaderStyle BackColor="#404c54" ForeColor="White" />
                     <RowStyle Font-Size="14px"/>
                    <Columns> 
                    <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>                              

                                <asp:Label ID="lblEntryDate" runat="server" Text='<%#Eval("Date","{0:dd/MM/yyyy}") %>'></asp:Label> 

                            </ItemTemplate>                       
                    </asp:TemplateField>
                    <asp:TemplateField  HeaderText="AccName">
                        <ItemTemplate>
                            <asp:Label ID="lblAccName" runat="server" Text='<%#Eval("AccountName") %>'></asp:Label> 
                        </ItemTemplate>
                    </asp:TemplateField>       
                    <asp:TemplateField  HeaderText="Type" >
                        <ItemTemplate >
                            <asp:Label ID="lblTransType" runat="server" Text='<%# Eval("Type") %>' />
                            <asp:Label ID="lblTransTyID" runat="server" Text='<%# Eval("TransType")%>' Visible="false"/>
                        </ItemTemplate>                         
                    </asp:TemplateField> 
                    <asp:TemplateField  HeaderText="Particular" >
                        <ItemTemplate >
                            <asp:Label ID="lblParticular" runat="server" Text='<%# Eval("Particular") %>' CssClass="preformatted"/>
                        </ItemTemplate>                   
                    </asp:TemplateField>                      
                    <asp:TemplateField HeaderText="USD" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblExpenseS" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseUSD").ToString())?Eval("ExpenseUSD").ToString():"" %>'></asp:Label> 
                        
                            <asp:Label ID="lblUnitUSD1"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseUSD").ToString())? " <br/>USD": "" %>' Font-Size="X-Small"></asp:Label>

                        </ItemTemplate>                       
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kyat" >
                        <ItemTemplate>
                            <asp:Label ID="lblExpenseKs" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseKs").ToString())?Eval("ExpenseKs").ToString():"" %>'></asp:Label> 
                                                            
                             <asp:Label ID="lblUnitKs1"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseKs").ToString())? " <br/>Kyat": "" %>' Font-Size="X-Small"></asp:Label>

                        </ItemTemplate>                       
                    </asp:TemplateField>
                         <asp:TemplateField HeaderText="￥" >
                        <ItemTemplate>
                            <asp:Label ID="lblExpenseYen" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseYen").ToString())?Eval("ExpenseYen").ToString():"" %>'></asp:Label> 
                                                            
                             <asp:Label ID="lblUnitYen1"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("ExpenseYen").ToString())? " <br/>￥": "" %>' Font-Size="X-Small"></asp:Label>


                            </ItemTemplate>
                            </asp:TemplateField>
                    <asp:TemplateField HeaderText="USD" >
                            <ItemTemplate>
                                <asp:Label ID="lblIncomeS" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeUSD").ToString())?Eval("IncomeUSD").ToString():"" %>'></asp:Label> 
                            
                                <asp:Label ID="lblUnitUSD2"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeUSD").ToString())? " <br/>USD": "" %>' Font-Size="X-Small"></asp:Label>

                            </ItemTemplate>                     
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Kyat" >
                        <ItemTemplate>
                            <asp:Label ID="lblIncomeKs" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeKs").ToString())?Eval("IncomeKs").ToString():"" %>'></asp:Label> 

                                <asp:Label ID="lblUnitKs2"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeKs").ToString())? " <br/>Kyat": "" %>' Font-Size="X-Small"></asp:Label>

                        </ItemTemplate>                       
                    </asp:TemplateField>
                         <asp:TemplateField HeaderText="￥" >
                        <ItemTemplate>
                            <asp:Label ID="lblIncomeYen" runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeYen").ToString())?Eval("IncomeYen").ToString():"" %>'></asp:Label> 

                                <asp:Label ID="lblUnitYen2"  runat="server" Text='<%#!String.IsNullOrWhiteSpace(Eval("IncomeYen").ToString())? " <br/>￥": "" %>' Font-Size="X-Small"></asp:Label>

                            </ItemTemplate>
                            </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="USD" ><%--(((Label)gdvTransReport.Rows[Container.DataItemIndex-1].FindControl("lblRemainUSD")).Text)--%>
                            <ItemTemplate >
                            <asp:Label ID="lblRemainUSD" runat="server" Text='<%#Eval("ResultAmtUSD")%>' Visible="false" Type="Integer"></asp:Label> 
                             <asp:Label ID="lblHidAcc" runat="server" Text='<%#Eval("AccountName") %>' Visible="false"></asp:Label> 

                            <asp:Label ID="lblNewRemainUSD" runat="server" 

                                 Text='<%# 
                             Container.DisplayIndex==0 ||((Label)gdvTransReportSnd.Rows[Container.DisplayIndex-1].FindControl("lblHidAcc")).Text!=Eval("AccountName").ToString()?                              
                             Convert.ToDecimal(Eval("OpeningBalanceUSD").ToString())+
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("IncomeUSD").ToString())?"0":Eval("IncomeUSD").ToString())-
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("ExpenseUSD").ToString())?"0":Eval("ExpenseUSD").ToString()) 
                             :
                             Convert.ToDecimal(((Label)gdvTransReportSnd.Rows[Container.DisplayIndex-1].FindControl("lblNewRemainUSD")).Text)+
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("IncomeUSD").ToString())?"0":Eval("IncomeUSD").ToString())-
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("ExpenseUSD").ToString())?"0":Eval("ExpenseUSD").ToString())
                             %>'>
                            </asp:Label> 
                         
                            <asp:Label ID="lblUnitUSD3"  runat="server" Text="<br/> USD" Font-Size="X-Small"></asp:Label>

                            </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Kyat" >
                            <ItemTemplate>
                                <asp:Label ID="lblRemainKs" runat="server" Text='<%#Eval("ResultAmtKs") %>' Visible="false" ></asp:Label> 
                                 <asp:Label ID="lblHidAcc2" runat="server" Text='<%#Eval("AccountName") %>' Visible="false"></asp:Label> 

                                <asp:Label ID="lblNewRemainKs" runat="server"
                                Text='<%#
                                Container.DisplayIndex==0 ||((Label)gdvTransReportSnd.Rows[Container.DisplayIndex-1].FindControl("lblHidAcc2")).Text!=Eval("AccountName").ToString()?                              
                             Convert.ToDecimal(Eval("OpeningBalanceKs").ToString())+
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("IncomeKs").ToString())?"0":Eval("IncomeKs").ToString())-
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("ExpenseKs").ToString())?"0":Eval("ExpenseKs").ToString()) 
                             :
                             Convert.ToDecimal(((Label)gdvTransReportSnd.Rows[Container.DisplayIndex-1].FindControl("lblNewRemainKs")).Text)+
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("IncomeKs").ToString())?"0":Eval("IncomeKs").ToString())-
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("ExpenseKs").ToString())?"0":Eval("ExpenseKs").ToString())
                                       %>'>
                            </asp:Label>                                

                                 <asp:Label ID="lblUnitKs3"  runat="server" Text="<br/> Kyat" Font-Size="X-Small" ></asp:Label>

                            </ItemTemplate>
                    </asp:TemplateField>

                         <asp:TemplateField HeaderText="￥" >
                        <ItemTemplate>

                            <asp:Label ID="lblRemainYen" runat="server" Text='<%#Eval("ResultAmtYen") %>' Visible="false" ></asp:Label> 
                                 <asp:Label ID="lblHidAcc3" runat="server" Text='<%#Eval("AccountName") %>' Visible="false"></asp:Label> 

                                <asp:Label ID="lblNewRemainYen" runat="server"
                                Text='<%#
                                Container.DisplayIndex==0 ||((Label)gdvTransReportSnd.Rows[Container.DisplayIndex-1].FindControl("lblHidAcc3")).Text!=Eval("AccountName").ToString()?                              
                             Convert.ToDecimal(Eval("OpeningBalanceYen").ToString())+
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("IncomeYen").ToString())?"0":Eval("IncomeYen").ToString())-
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("ExpenseYen").ToString())?"0":Eval("ExpenseYen").ToString()) 
                             :
                             Convert.ToDecimal(((Label)gdvTransReportSnd.Rows[Container.DisplayIndex-1].FindControl("lblNewRemainYen")).Text)+
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("IncomeYen").ToString())?"0":Eval("IncomeYen").ToString())-
                             Convert.ToDecimal(String.IsNullOrWhiteSpace(Eval("ExpenseYen").ToString())?"0":Eval("ExpenseYen").ToString())
                                       %>'>
                            </asp:Label>                                

                                 <asp:Label ID="lblUnitYen3"  runat="server" Text="<br/> ￥" Font-Size="X-Small" ></asp:Label>

                            </ItemTemplate>
                            </asp:TemplateField>

                    <asp:TemplateField  HeaderText="Remark" >
                    <ItemTemplate >
                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>' CssClass="preformatted"/>
                    </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemarks" runat="server" width="120px"  height="41px" CssClass="form-control input-sm"  Text='<%#Eval("Remarks") %>' TextMode="MultiLine"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField> 

                    <asp:TemplateField HeaderText="InsertBy" >
                            <ItemTemplate>
                                <asp:Label ID="lblCreateBy" runat="server" Text='<%#Eval("CreatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UpdateBy" >
                            <ItemTemplate>
                                 <asp:HiddenField ID="hdfID" Value='<%#Eval("TransID") %>' runat="server" />
                                <asp:HiddenField ID="hdfAccID" Value='<%#Eval("ACCID") %>' runat="server"/>
                                <asp:HiddenField ID="hdfStsID" Value='<%#Eval("StatusID") %>' runat="server"/>

                                <asp:Label ID="lblUpdateBy" runat="server" Text='<%#Eval("UpdatedByUser") %>'></asp:Label> 
                            </ItemTemplate>
                    </asp:TemplateField>                          
                </Columns>
                <FooterStyle BorderWidth="0px"/>
            </asp:GridView>
        </div>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center> 
 </div>
   <%-- </div>--%>
</asp:Content>
