<%@ Page Title="Danh sách thanh toán hộ" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="danh-sach-thanh-toan-ho.aspx.cs" Inherits="NHST.manager.danh_sach_thanh_toan_ho1" %>

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
                    <div class="title-flex">
                        <h4 class="title no-margin">YÊU CẦU THANH TOÁN HỘ</h4>
                    </div>
                </div>
            </div>

            <div class="list-staff col s12 section">

                <div class="list-table card-panel">
                    <div class="row section">
                        <div class="col s12">

                            <div class="top-table-filter">
                                <div class="sort-tb-wrap">
                                    <div class="filter-link select-sort">
                                        <span>Sắp xếp theo</span>
                                        <asp:DropDownList runat="server" ID="ddlSortType" onchange="SearchSort();">
                                            <asp:ListItem Value="-1" Text="--Sắp xếp--"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="ID đơn hàng tăng"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="ID đơn hàng giảm"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Trạng thái đơn hàng tăng"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Trạng thái đơn hàng giảm"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="filter-link">
                                        <a href="#" class="btn-icon btn" id="filter-btn"><i class="material-icons">filter_list</i><span>Bộ lọc nâng cao</span></a>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="filter-wrap">
                                    <div class="row">
                                        <div class="input-field col s12 l6">
                                            <asp:TextBox ID="search_name" type="text" placeholder="" class="validate" runat="server"></asp:TextBox>
                                            <label for="search_name"><span>Username</span></label>
                                        </div>
                                        <div class="input-field col s12 l6">
                                            <asp:ListBox ID="ddlStatus" runat="server">
                                                <asp:ListItem Value="" Selected="True">Tất cả</asp:ListItem>
                                                <asp:ListItem Value="1">Đã hủy</asp:ListItem>
                                                <asp:ListItem Value="0">Đơn mới</asp:ListItem>
                                                <asp:ListItem Value="2">Đã xác nhận</asp:ListItem>
                                                <asp:ListItem Value="3">Đã thanh toán</asp:ListItem>
                                                <asp:ListItem Value="4">Hoàn thành</asp:ListItem>
                                            </asp:ListBox>
                                            <label for="status">Trạng thái</label>
                                        </div>
                                        <div class="input-field col s6">
                                            <asp:TextBox runat="server" ID="rFD" type="text" placeholder="" class="datetimepicker from-date"></asp:TextBox>
                                            <label for="from-date">Từ ngày</label>
                                        </div>
                                        <div class="input-field col s6">
                                            <asp:TextBox runat="server" ID="rTD" type="text" placeholder="" class="datetimepicker to-date"></asp:TextBox>
                                            <label>Đến ngày</label>
                                            <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                                        </div>

                                        <div class="col s12 right-align">
                                            <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" class="btn" Text="Lọc kết quả"></asp:Button>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <div class="list-table card-panel mt-2">
                    <div class="responsive-tb">
                        <table class="table  highlight bordered   ">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Username</th>
                                    <th>Tổng tiền (Tệ)</th>
                                    <th>Tổng tiền (VNĐ)</th>
                                    <th style="min-width: 130px;">Tỷ giá</th>
                                    <th>Phần trăm <br /> dịch vụ (%)</th>
                                    <th style="min-width: 120px;">Nhân viên
                                        <br />
                                        kinh doanh</th>
                                   <%-- <th>Mã đơn hàng</th>--%>
                                    <th style="min-width: 130px;">Trạng thái</th>
                                    <th style="min-width: 100px;">Ngày tạo</th>
                                    <th style="min-width: 100px;">Thao tác</th>
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
    <asp:Button ID="btnPayment" runat="server" UseSubmitBehavior="false" Style="display: none" Text="Thanh Toán" CssClass="btn" OnClick="btnPayment_Click" />
    <asp:HiddenField ID="hdfOrderID" runat="server" />
    <script type="text/javascript">
        function SearchSort() {
            $('#<%=btnSearch.ClientID%>').click();
        }
        function AddPayment(orderID) {
            var r = confirm("Bạn muốn thanh toán đơn này?");
            if (r == true) {

                $("#<%=hdfOrderID.ClientID%>").val(orderID);
                $('#<%=btnPayment.ClientID%>').click();
            }
        }
        function ChooseSaler(OrderID, obj) {
            var SalerID = obj.val();
            $.ajax({
                type: "POST",
                url: "/manager/danh-sach-thanh-toan-ho.aspx/UpdateStaff",
                data: "{OrderID:'" + OrderID + "',StaffID:'" + SalerID + "',Type:'1'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (data != "null") {
                        if (data != "notpermission") {
                            location.reload();
                        }
                        else {
                            alert('Bạn không có quyền');
                        }
                    }
                    else {
                        alert('Vui lòng đăng nhập lại.');
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi checkend');
                }
            });
        }
    </script>
</asp:Content>

