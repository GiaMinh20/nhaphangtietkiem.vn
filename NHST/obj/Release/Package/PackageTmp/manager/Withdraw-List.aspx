<%@ Page Title="Lịch sử rút tiền" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="Withdraw-List.aspx.cs" Inherits="NHST.manager.Withdraw_List" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button runat="server" ID="excel" UseSubmitBehavior="false" OnClick="btnExcel_Click" />
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnSearch" runat="server" OnClick="btnSearch_Click" />
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Lịch sử rút tiền</h4>
                    <div class="right-action">
                        <a class="btn" id="excel-btn" onclick="ExportExcel()" style="background-color: green; margin-right: 10px;">Xuất Excel</a>
                        <a href="#" class="btn" id="filter-btn">Bộ lọc</a>
                    </div>
                    <div class="clearfix"></div>
                    <div class="filter-wrap">
                        <div class="row mt-2 pt-2">
                            <div class="search-name input-field col s12 l6">
                                <asp:TextBox ID="search_name" name="txtSearchName" type="text" onkeypress="myFunction()" runat="server" />
                                <label for="search_name"><span>Username</span></label>
                                <%-- <span class="material-icons search-action">search</span>--%>
                            </div>
                            <div class="input-field col s12 l6">
                                <asp:ListBox runat="server" ID="select_by">
                                    <asp:ListItem Value="0" Selected="True">Tất cả</asp:ListItem>
                                    <asp:ListItem Value="1">Chờ duyệt</asp:ListItem>
                                    <asp:ListItem Value="2">Đã duyệt</asp:ListItem>
                                    <asp:ListItem Value="3">Đã hủy</asp:ListItem>
                                </asp:ListBox>
                                <label for="select_by">Trạng thái</label>
                            </div>
                            <div class="input-field col s6 l6">
                                <asp:TextBox runat="server" ID="rFD" type="text" class="datetimepicker from-date"></asp:TextBox>
                                <label>Từ ngày</label>
                            </div>
                            <div class="input-field col s6 l6">
                                <asp:TextBox runat="server" ID="rTD" type="text" class="datetimepicker to-date"></asp:TextBox>
                                <label>Đến ngày</label>
                                <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                            </div>
                            <div class="col s12 right-align">
                                <span class="search-action btn">Lọc kết quả</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="list-donate-money col s12 section">
                <div class="list-table card-panel">
                    <%--<div class="filter">
                        <div class="search-name input-field">
                            <asp:TextBox ID="search_name" name="txtSearchName" type="text" onkeypress="myFunction()" runat="server" />
                            <label for="search_name"><span>Username</span></label>
                            <span class="material-icons search-action">search</span>
                        </div>
                        <div class="clearfix"></div>
                    </div>--%>
                    <div class="responsive-tb mt-2">
                        <table class="table responsive-table  bordered highlight">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Username</th>
                                    <th>Ngân hàng</th>
                                    <th>Nội dung</th>
                                    <th>Số tiền rút</th>
                                    <th>Trạng thái</th>
                                    <th>Ngày rút</th>
                                    <th>Người duyệt</th>
                                    <th>Ngày duyệt</th>
                                    <th>Thao tác</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal ID="ltr" runat="server" EnableViewState="false"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                    <div class="pagi-table float-right mt-2">
                        <%this.DisplayHtmlStringPaging1();%>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <!-- Edit mode -->
        <div class="detail-fixed  col s12 m5 l5 xl4 section" id="draw-detail">
            <div class="rp-detail card-panel row">
                <div class="col s12">
                    <div class="page-title">
                        <h5>Thông tin rút tiền #<asp:Label runat="server" ID="lbID"></asp:Label></h5>
                        <a href="#!" class="close-editmode top-right valign-wrapper"><i
                            class="material-icons">close</i>Close</a>
                    </div>
                </div>
                <div class="col s12">
                    <div class="row pb-2 border-bottom-1 ">
                        <div class="input-field col s12">
                            <asp:TextBox runat="server" placeholder="" ID="lblUsername" type="text" class="validate" Enabled="false"></asp:TextBox>
                            <label for="rp_username">Username</label>
                        </div>

                        <div class="input-field col s12 m12">
                            <asp:TextBox runat="server" placeholder="" ID="pWallet" type="text" class="validate" Enabled="false"></asp:TextBox>
                            <label for="rp_vnd">Số tiền rút (VNĐ)</label>
                        </div>
                        <div class="input-field col s12">
                            <asp:TextBox runat="server" placeholder="" TextMode="MultiLine" ID="pContent"
                                class="materialize-textarea"></asp:TextBox>
                            <label for="rp_textarea">Nội dung</label>
                        </div>
                        <div class="input-field col s12">
                            <asp:ListBox runat="server" placeholder="" ID="ddlStatus">
                                <asp:ListItem Value="1" Text="Chờ duyệt"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Đã duyệt"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Hủy"></asp:ListItem>
                            </asp:ListBox>
                            <label>Trạng thái</label>
                        </div>

                    </div>
                    <div class="row section mt-2">
                        <div class="col s12">
                            <a href="javascript:;" onclick="btnSave()" class="btn">Cập nhật</a>
                            <a href="#" class="btn close-editmode">Trở về</a>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <!-- END : Edit mode -->
    </div>
    <div id="printcontent" style="display: none">
    </div>
    <asp:HiddenField runat="server" ID="hdfIDWR" />
    <asp:Button ID="btnSaveEdit" runat="server" OnClick="btncreateuser_Click" Style="display: none" UseSubmitBehavior="false" />
    <script type="text/javascript">
        function btnSave() {
            $('#<%=btnSaveEdit.ClientID%>').click();
        }
        function EditFunction(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/Withdraw-List.aspx/GetData",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data != null) {
                        $('#<%=lbID.ClientID%>').text(ID);
                        $('#<%=hdfIDWR.ClientID%>').val(ID);
                        $('#<%=lblUsername.ClientID%>').val(data.Username);
                        $('#<%=pWallet.ClientID%>').val(data.Amount);
                        $('#<%=pContent.ClientID%>').val(data.Note);
                        $('#<%=ddlStatus.ClientID%>').val(data.Status);
                        $('#<%=ddlStatus.ClientID%>').prop('disabled', false);
                        if (data.Status != '1') {
                            console.log(data.Status);
                            $('#<%=ddlStatus.ClientID%>').prop('disabled', true);
                        }
                        $('select').formSelect();
                    }
                    else
                        swal("Error", "Không thành công", "error");
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    swal("Error", "Fail updateInfoAcc", "error");
                }
            });
        }
        function myFunction() {
            if (event.which == 13 || event.keyCode == 13) {
                console.log($('#<%=search_name.ClientID%>').val());
                $('#<%=btnSearch.ClientID%>').click();
            }
        }
        $('.search-action').click(function () {
            console.log('dkm ngon');
            console.log($('#<%=search_name.ClientID%>').val());
            $('#<%=btnSearch.ClientID%>').click();
        })
        $(document).ready(function () {
            $('#search_name').autocomplete({
                data: {
                    "Apple": null,
                    "Microsoft": null,
                    "Google": 'https://placehold.it/250x250',
                    "Asgard": null
                },
            });

        });

        function printPhieuchi(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/Withdraw-List.aspx/GetData",
                data: "{ID:'" + ID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (data != "null") {
                        var ret = JSON.parse(data);
                        var html = "";
                        html += "<div class=\"print-bill\">";
                        html += "   <div class=\"top\">";
                        html += "       <div class=\"left\">";
                        html += "          <span class=\"company-info\">NHẬP HÀNG TIẾT KIỆM</span>";
                        html += "          <span class=\"company-info\">Hà Nội</span>";
                        html += "       </div>";
                        html += "       <div class=\"right\">";
                        html += "           <span class=\"bill-num\">Mẫu số 01 - TT</span>";
                        html += "           <span class=\"bill-promulgate-date\">(Ban hành theo Thông tư số 133/2016/TT-BTC ngày 26/8/2016 của Bộ Tài chính)</span>";
                        html += "       </div>";
                        html += "   </div>";
                        html += "   <div class=\"bill-title\">";
                        html += "       <h1>PHIẾU CHI</h1>";
                        html += "       <span class=\"bill-date\">" + ret.CreateDate + " </span>";
                        html += "   </div>";
                        html += "   <div class=\"bill-content\">";
                        html += "       <div class=\"bill-row\">";
                        html += "           <label class=\"row-name\">Họ và tên người nhận tiền: </label>";
                        html += "           <label class=\"row-info\">" + ret.FullName + "</label>";
                        html += "       </div>";
                        html += "       <div class=\"bill-row\">";
                        html += "           <label class=\"row-name\">Địa chỉ: </label>";
                        html += "           <label class=\"row-info\">" + ret.Address + "</label>";
                        html += "       </div>";
                        html += "       <div class=\"bill-row\">";
                        html += "           <label class=\"row-name\">Lý do chi: </label>";
                        html += "           <label class=\"row-info\"></label>";
                        html += "       </div>";
                        html += "       <div class=\"bill-row\">";
                        html += "           <label class=\"row-name\">Số tiền: </label>";
                        html += "           <label class=\"row-info\">" + ret.Money + " VND</label>";
                        html += "       </div>";
                        html += "       <div class=\"bill-row\">";
                        html += "           <label class=\"row-name\">Bằng chữ: </label>";
                        html += "           <label class=\"row-info\"></label>";
                        html += "       </div>";
                        html += "       <div class=\"bill-row\">";
                        html += "           <div class=\"row-col\">";
                        html += "               <label class=\"row-name\">Kèm theo: </label>";
                        html += "               <label class=\"row-info\"></label>";
                        html += "           </div>";
                        html += "           <div class=\"row-col\">";
                        html += "               <label class=\"row-name\">Chứng từ gốc: </label>";
                        html += "               <label class=\"row-info\"></label>";
                        html += "           </div>";
                        html += "       </div>";
                        html += "   </div>";
                        html += "   <div class=\"bill-footer\">";
                        html += "       <div class=\"bill-row-all right\">" + ret.CreateDate + "</div>";
                        html += "   </div>";
                        html += "   <div class=\"bill-footer\">";
                        html += "       <div class=\"bill-row-one\">";
                        html += "           <strong>Giám đốc</strong>";
                        html += "           <span class=\"note\">(Ký, họ tên, đóng dấu)</span>";
                        html += "       </div>";
                        html += "       <div class=\"bill-row-one\">";
                        html += "           <strong>Kế toán trưởng</strong>";
                        html += "           <span class=\"note\">(Ký, họ tên)</span>";
                        html += "       </div>";
                        html += "       <div class=\"bill-row-one\">";
                        html += "           <strong>Người nộp tiền</strong>";
                        html += "           <span class=\"note\">(Ký, họ tên)</span>";
                        html += "       </div>";
                        html += "       <div class=\"bill-row-one\">";
                        html += "           <strong>Thủ quỹ</strong>";
                        html += "           <span class=\"note\">(Ký, họ tên)</span>";
                        html += "       </div>";
                        html += "   </div>";
                        html += "</div>";
                        $("#printcontent").html(html);

                        printDiv('printcontent');
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                }
            });

        }
        function printDiv(divid) {
            var divToPrint = document.getElementById('' + divid + '');
            var newWin = window.open('', 'Print-Window');
            newWin.document.open();
            newWin.document.write('<html><head><link rel="stylesheet" href="/App_Themes/NewUI/css/custom.css" type="text/css"/></head><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
            setTimeout(function () { newWin.close(); }, 10);
        }
        function ExportExcel() {
            $('#<%=excel.ClientID%>').click();
        }
    </script>
</asp:Content>
