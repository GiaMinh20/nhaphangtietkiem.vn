<%@ Page Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="report-vch.aspx.cs" Inherits="NHST.manager.report_vch" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Thống kê cước vận chuyển hộ</h4>
                    <div class="right-action">
                        <a href="#" class="btn" id="filter-btn">Bộ lọc</a>
                    </div>
                    <div class="clearfix"></div>
                    <div class="filter-wrap">
                        <div class="row mt-2 pt-2">

                            <div class="input-field col s12 l2">
                                <asp:ListBox runat="server" ID="select_by">
                                    <asp:ListItem Value="0" Selected="True">Tất cả</asp:ListItem>
                                    <asp:ListItem Value="2">ID</asp:ListItem>
                                  <%--  <asp:ListItem Value="3">Mã vận đơn</asp:ListItem>--%>
                                    <asp:ListItem Value="1">Username</asp:ListItem>
                                </asp:ListBox>
                                <label for="select_by">Tìm kiếm theo</label>
                            </div>
                            <div class="input-field col s12 l4">
                                <asp:TextBox ID="txtSearchName" name="txtSearchName" type="text" onkeypress="myFunction()" runat="server" />
                                <label class="active" for="txtSearchName"><span>Nhập tìm kiếm</span></label>
                            </div>

                            <div class="input-field col s12 l6">
                                <asp:ListBox runat="server" ID="ddlstatus">
                                    <asp:ListItem Value="-1" Selected="true" Text="Tất cả"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Chưa thanh toán"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Đã thanh toán"></asp:ListItem>
                                </asp:ListBox>
                                <label for="status">Trạng thái</label>
                            </div>

                            <div class="input-field col s6 l6">
                                <asp:TextBox ID="rFD" runat="server" Type="text" class="datetimepicker from-date"></asp:TextBox>
                                <label>Từ ngày</label>
                            </div>
                            <div class="input-field col s6 l6">
                                <asp:TextBox runat="server" Type="text" ID="rTD" class="datetimepicker to-date"></asp:TextBox>
                                <label>Đến ngày</label>
                                <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                            </div>

                            <div class="col s12 right-align">
                                <asp:Button runat="server" ID="filter" OnClick="btnSearch_Click" class="btnSort btn" Text="Lọc kết quả"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="list-order-cus col s12 section">
                <div class="list-table card-panel">
                    <div class="table-info row center-align-xs" style="display: none">
                        <div class="checkout col s12 m6">
                            <p class="black-text">
                                <span class="lbl">Tổng cân nặng:</span><span
                                    class="black-text font-weight-700"><asp:Literal runat="server" ID="lblWeightAll"></asp:Literal></span>
                            </p>
                            <p class="black-text">
                                <span class="lbl">Tổng tiền đã thanh toán:</span><span
                                    class="black-text font-weight-700"><asp:Literal runat="server" ID="lblPriceAllVND"></asp:Literal></span>
                            </p>
                            <p class="black-text">
                                <span class="lbl">Tổng tiền chưa thanh toán: </span><span
                                    class="black-text font-weight-700">
                                    <asp:Literal runat="server" ID="lblPriceNotPay"></asp:Literal></span>
                            </p>
                        </div>
                    </div>
                    <div class="responsive-tb mt-3">
                        <table class="table bordered highlight   ">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Tên tài khoản</th>
                                    <th>Ngày YCXK</th>
                                    <th>Ngày XK</th>
                                    <th>Tổng số kiện</th>
                                    <th>Tổng số kg</th>
                                    <th>Tổng cước</th>
                                    <th>HTVC</th>
                                    <th>Trạng thái thanh toán</th>
                                    <th>Trạng thái xuất kho</th>
                                    <th>Ghi chú nội bộ</th>
                                    <th>Ghi chú khách</th>
                                    <th>Thao tác</th>
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
    <asp:HiddenField runat="server" ID="hdfID" />
    <asp:Button runat="server" ID="btnPayByWallet" Style="display: none" OnClick="btnPayByWallet_Click" UseSubmitBehavior="false" />
    <asp:Button runat="server" ID="btnPay" OnClick="btnPay_Click" UseSubmitBehavior="false" Style="display: none" />
    <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_Click" UseSubmitBehavior="false" Style="display: none" />
    <!-- END: Page Main-->
    <script type="text/javascript">
        function updateNote(obj, ID) {
            var staffNote = obj.parent().find(".txtNote").val();
            $.ajax({
                type: "POST",
                url: "/manager/Report-VCH.aspx/UpdateStaffNote",
                data: "{ID:'" + ID + "',staffNote:'" + staffNote + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var ret = msg.d;
                    if (ret == "ok") {
                        obj.parent().find(".update-info").show();
                    }
                    else {
                        obj.parent().find(".update-info").hide();
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    //alert('lỗi checkend');
                }
            });
        }

        function PayByWallet(obj, ID) {
            var c = confirm('Bạn muốn thanh toán bằng ví?');
            if (c) {
                $("#<%=hdfID.ClientID%>").val(ID);
                $("#<%=btnPayByWallet.ClientID%>").click();
            }
        }

        function Pay(obj, ID) {
            var c = confirm('Bạn muốn thanh toán trực tiếp?');
            if (c) {
                $("#<%=hdfID.ClientID%>").val(ID);
                $("#<%=btnPay.ClientID%>").click();
            }
        }

        function Delete(obj, ID) {
            var c = confirm('Bạn muốn hủy yêu cầu xuất kho này?');
            if (c) {
                $("#<%=hdfID.ClientID%>").val(ID);
                $("#<%=btnDelete.ClientID%>").click();
            }
        }
    </script>
</asp:Content>
