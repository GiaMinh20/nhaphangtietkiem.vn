<%@ Page Title="Danh sách đơn hàng ký gửi" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="transportation-list.aspx.cs" Inherits="NHST.manager.transportation_list" %>

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
                    <h4 class="title no-margin" style="display: inline-block;">ĐƠN HÀNG KÝ GỬI</h4>
                </div>
            </div>
            <div class="clearfix"></div>

            <div class="list-staff col s12 section">

                <div class="list-table card-panel">
                    <div class="row section">
                        <div class="col s12">
                            <div class="top-table-filter">
                                <div class="sort-tb-wrap">
                                    <div class="filter-link select-sort">
                                        <span>Sắp xếp theo</span>
                                        <asp:DropDownList runat="server" ID="ddlSortType" onchange="SearchSort();">
                                            <asp:ListItem Value="0" Text="--Sắp xếp--"></asp:ListItem>
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
                                <div class="filter-wrap" style="display:block">
                                    <div class="row">
                                        <div class="input-field col s12 l3">
                                            <asp:ListBox runat="server" ID="select_byType">
                                                <asp:ListItem Value="" Selected="True">Tất cả</asp:ListItem>
                                                <asp:ListItem Value="1">ID</asp:ListItem>
                                                <asp:ListItem Value="2">Mã vận đơn</asp:ListItem>
                                                <asp:ListItem Value="3">User name</asp:ListItem>
                                            </asp:ListBox>
                                            <label for="select_byType">Tìm kiếm theo</label>
                                        </div>
                                        <div class="input-field col s12 l3">
                                            <asp:TextBox runat="server" ID="tSearchName" type="text" class="validate"></asp:TextBox>
                                            <label for="tSearchName">
                                                <asp:Literal runat="server" ID="lrtSearch"></asp:Literal></label>
                                        </div>
                                        <div class="input-field col s6 l3">
                                            <asp:TextBox runat="server" ID="rFD" type="text" class="datetimepicker from-date"></asp:TextBox>
                                            <label>Từ ngày</label>
                                        </div>
                                        <div class="input-field col s6 l3">
                                            <asp:TextBox runat="server" ID="rTD" type="text" class="datetimepicker to-date"></asp:TextBox>
                                            <label>Đến ngày</label>
                                            <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                                        </div>
                                        <div class="input-field col s6 l3">
                                            <asp:ListBox runat="server" ID="ddlWarehouseFrom"></asp:ListBox>
                                            <label for="ddlWarehouseFrom">Kho nhận</label>
                                        </div>
                                        <div class="input-field col s6 l3">
                                            <asp:ListBox runat="server" ID="ddlWarehouseTo"></asp:ListBox>
                                            <label for="ddlWarehouseTo">Kho đích</label>
                                        </div>
                                        <div class="input-field col s12 l3">
                                            <asp:ListBox runat="server" ID="ddlShippingType"></asp:ListBox>
                                            <label for="ddlShippingType">Phương thức vận chuyển</label>
                                        </div>
                                        <div class="input-field col s12 l3">
                                            <asp:ListBox runat="server" SelectionMode="Multiple" class="select_all" ID="ddlStatus">
                                                <asp:ListItem Value="0">Đã hủy</asp:ListItem>
                                                <asp:ListItem Value="1">Chờ duyệt</asp:ListItem>
                                                <asp:ListItem Value="2">Đã duyệt</asp:ListItem>
                                                <asp:ListItem Value="4">Hàng về kho TQ</asp:ListItem>
                                                <asp:ListItem Value="3">Vận chuyển Quốc tế</asp:ListItem>
                                                <asp:ListItem Value="5">Hàng về kho VN</asp:ListItem>
                                                <asp:ListItem Value="6">Khách đã thanh toán</asp:ListItem>
                                                <asp:ListItem Value="7">Đã hoàn thành</asp:ListItem>
                                            </asp:ListBox>
                                            <label for="ddlStatus">Trạng thái</label>
                                        </div>
                                        <div class="input-field col s12 l6">
                                            <asp:TextBox runat="server" TextMode="Number" ID="rPriceFrom"></asp:TextBox>
                                            <label for="rPriceFrom">Giá từ</label>
                                        </div>
                                        <div class="input-field col s12 l6">
                                            <asp:TextBox runat="server" TextMode="Number" ID="rPriceTo"></asp:TextBox>
                                            <label for="rPriceTo">Giá đến</label>
                                        </div>
                                        <div class="col s12 right-align">
                                            <a href="javascript:;" class="btn primary-btn" onclick="fulterGet()">Tìm kiếm</a>
                                            <asp:Button runat="server" ID="btnSearch" Text="Tìm" CssClass="btn primary-btn" OnClick="btnSearch_Click" Style="display: none" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <asp:Panel runat="server" ID="pnStaff">
                    <div class="list-table card-panel">
                        <div class="row section">
                            <div class="input-field col s12 l3">
                                <asp:DropDownList runat="server" ID="ddlUpdateStatus">
                                    <asp:ListItem Value="-1" Selected="True">---Trạng thái---</asp:ListItem>
                                    <asp:ListItem Value="1">Chờ duyệt</asp:ListItem>
                                    <asp:ListItem Value="2">Đã duyệt</asp:ListItem>
                                    <asp:ListItem Value="0">Hủy đơn</asp:ListItem>
                                </asp:DropDownList>
                                <label for="select_by">Chọn trạng thái</label>
                            </div>
                            <div class="input-field col s12 l3">
                                <a href="javascript:;" onclick="UpdateStatus($(this))" class="btn">Cập nhật</a>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="list-table card-panel">
                    <div class="responsive-tb">
                        <table class="table responsive-table  bordered highlight  ">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>ID</th>
                                    <th>User đặt hàng</th>
                                    <th>Nhân viên </br> kinh doanh</th>
                                    <th>Mã vận đơn</th>
                                    <th>Tổng tiền</th>
                                    <th>Tiền đã cọc</th>
                                    <th>Tổng cân nặng</th>
                                    <th>Kho nhận</th>
                                    <th>Kho đích</th>
                                    <th>Phương thức</th>
                                    <th>Trạng thái</th>
                                    <th>Ngày đặt</th>
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
    <asp:HiddenField ID="hdfOrderID" runat="server" />
    <asp:HiddenField ID="hdfStaffID" runat="server" />
    <asp:HiddenField ID="hdfStatus" runat="server" Value="-1" />
    <asp:Button ID="btnPay" runat="server" OnClick="btnPay_Click" Style="display: none" UseSubmitBehavior="false" />
    <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btnUpdateStaff" runat="server" OnClick="btnUpdateStaff_Click" />
    <script type="text/javascript">
        function fulterGet() {
            var st = $("#<%=ddlStatus.ClientID%>").val();
            $("#<%=hdfStatus.ClientID%>").val(st);
            $("#<%=btnSearch.ClientID%>").click();
        }
        $(document).ready(function () {
            $("#tag").select2({
                tags: true,
                maximumInputLength: 10,

                templateSelection: function (selection) {
                    if (!selection.id) {
                        return selection.text;
                    }
                    return $('<span class="' + selection.id + '">' + selection.text + '</span>');
                }
            });
        });
        function SearchSort() {
            $('#<%=btnSearch.ClientID%>').click();
        }
        function payallorder(orderID, obj) {
            var r = confirm('Bạn muốn thanh toán đơn hàng này: ' + orderID);
            if (r == true) {
                obj.removeAttr("onclick");
                $("#<%=hdfOrderID.ClientID%>").val(orderID);
                $("#<%=btnPay.ClientID%>").click();
            }
        }
        function UpdateStatus(obj) {
            var staff = -1;
            var staff = $("#<%=ddlUpdateStatus.ClientID%>").val();

            if (staff > -1) {
                var c = confirm("Bạn muốn cập nhật trạng thái đơn hàng?");
                if (c) {
                    obj.attr('disabled');
                    $("#<%=hdfStaffID.ClientID%>").val(staff);
                    $("#<%=btnUpdateStaff.ClientID%>").click();
                }
            }
            else {
                alert('Vui lòng chọn trạng thái khác để cập nhật');
            }
        }

        function CheckStaff(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/transportation-list.aspx/CheckStaff",
                data: "{MainOrderID:'" + ID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert(errorthrow);
                }
            });
        }
    </script>
</asp:Content>
