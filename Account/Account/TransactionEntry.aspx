<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransactionEntry.aspx.cs" Inherits="Account.TransactionEntry" MasterPageFile="~/Account.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   
   
 <script type="text/javascript" language="javascript">
     $(document).ready(function () {
         $('#datetimepicker1').datetimepicker({
             //datepicker: false,
           // ampm: true, // FOR AM/PM FORMAT
             //format : 'g:i A'
             //format: 'g:i A',

             //format: 'DD/MM/YYYY LT', // original code 
             //format: 'L',
            format: 'DD-MM-YYYY HH:mm:ss',
            defaultDate: new Date().toString(),
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

                //DisabledEffect();

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section id="main-content">
          <section class="wrapper">
     <div class="container-fluid" id="Div1" onclick="parent_disable(this);">
    <div class="container" id="MainDiv">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
<h1>Transaction Entry</h1>
    <div class="row">
        <div class="col-lg-12">
            <div class="col-lg-4">
            <div class="form-group">  

                <label class="title">Account Name</label>
                <asp:UpdatePanel ID="upSetSession" runat="server">
                <ContentTemplate>
                <asp:DropDownList ID="ddlAccName" runat="server" CssClass="form-username input-control" OnSelectedIndexChanged="ddlAccName_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlAccName" 
                EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
                <br />

                <label class="title">Status</label>
                <asp:DropDownList  ID="ddlStatus" runat="server" CssClass="form-username input-control" >
                </asp:DropDownList>
                <br />

                <label for="usr">Date</label>
                <div class="row">
                <div class="calendar-container">
                    <div class="form-group">
                    <div class='input-group date' id='datetimepicker1'>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-username form-control"></asp:TextBox>
                        <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                    </div>
                </div>
                </div>
                <br />
                <asp:RequiredFieldValidator ID="reqDate" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="** Date is Required **" ControlToValidate="txtDate"  ValidationGroup="trans"></asp:RequiredFieldValidator>
                        
                <label for="usr">Amount</label>   
                <asp:TextBox ID="txtAmount" runat="server" CssClass="input-control" placeholder="Amount..." onkeypress ="return isNumberKey(event)"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqAccID" runat="server" Display="Dynamic" ForeColor="Red" ErrorMessage="** Amount is Required **" ControlToValidate="txtAmount"  ValidationGroup="trans" ></asp:RequiredFieldValidator>
                <br />

                <label for="usr">Unit</label >
                <asp:DropDownList  ID="ddlCashUnit" runat="server" CssClass="input-control" >
                </asp:DropDownList>
            </div>
            </div>
            <div class="col-lg-4">
            <div class="form-group">  
            
                <label class="title">Transaction Type</label>
                <asp:DropDownList  ID="ddlTransType" runat="server" CssClass="input-control" >
                </asp:DropDownList>
                <br />

                <label class="title">Particular</label>
                <asp:TextBox ID="txtParticular" runat="server" CssClass="input-control" TextMode="MultiLine" placeholder="Particular..." Rows="0" Height="75px"></asp:TextBox>             
                <br />

                <label class="title">Remarks</label>
                <asp:TextBox ID="txtRemark" runat="server" CssClass="input-control" TextMode="MultiLine" placeholder="Remarks..." Rows="0" Height="75px"></asp:TextBox>
                <br />

                <asp:LinkButton ID="btnAddAttach" runat="server" Text="Add Attachment" CssClass="btn btn-info" OnClick="btnAddAttach_Click"/>
            </div>
            </div>
            <div class="col-lg-4" >               
                <asp:Panel ID="panelAttach" runat="server">
                <label class="title" >Attachments</label>  
                <div class="panel panel-default"  >
                </div>
                <asp:GridView ID="gdvAttachFiles"  GridLines="None"  runat="server" AutoGenerateColumns="false" EnableViewState="false" OnRowCommand="gdvSavedFile_RowCommand" >

                    <Columns>
                    <asp:HyperLinkField DataTextField="FileName" DataNavigateUrlFields="FilePath" Target="_blank"/>
                    <asp:BoundField DataField="" />
                    <asp:ButtonField ButtonType="Link" CommandName="file_Delete_GridOut" Text="<i class='glyphicon glyphicon-remove'></i>"  ControlStyle-CssClass="btn-sm btn-danger" />
                    </Columns>
                </asp:GridView>                
                </asp:Panel>             
           </div>
            <br />
    </div>
        <br />
    <br />
        <footer>
            <div class=" row col-lg-7" style="margin-left: 15px; top: 15px; left: 0px;" >
    <asp:Button runat="server" ID="btnSave"  Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="trans" style="width:96px"/>
    <asp:Button ID="btnCancel" CssClass="btn btn-outline btn-primary" runat="server" OnClick="btnCancel_Click" Text="Cancel"  style="width:96px"/>           
    </div>
        </footer>
    
</div> 
        <div>
            <br />
             <asp:HiddenField ID="hfTransactionID" runat="server" />
             <asp:HiddenField ID="hfMode" runat="server" />
        </div>
  </div>  
</div>
              </section>
         </section>
</asp:Content>
