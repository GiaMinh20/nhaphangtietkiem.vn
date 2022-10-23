<%@ Page Title="Chi tiết đơn hàng ký gửi" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="transportationdetail.aspx.cs" Inherits="NHST.manager.transportationdetail" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<%@ Import Namespace="MB.Extensions" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App_Themes/OrderList/js/fancybox-master/dist/jquery.fancybox.min.css" rel="stylesheet" />
    <style>

        .btn-upload {
            background-color: #F64302;
            color: white;
        }

        .preview-img img {
            width: 50% !important;
            height: 50% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">CHI TIẾT ĐƠN HÀNG</h4>
                    <div class="right-action">
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="order-detail-wrap col s12 section">
                <div class="row">
                    <div class="col s12 m12 l4 sticky-wrap">

                        <div class="card-panel">
                            <div class="order-stick-detail">
                                <div class="order-stick order-owner">
                                    <table class="table">
                                        <tbody>
                                            <tr>
                                                <td class="tb-date">Mã đơn hàng</td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltrOrderID"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Trạng thái</td>
                                                <td>
                                                    <asp:ListBox runat="server" ID="ddlOrderStatus">
                                                        <asp:ListItem Value="0" Text="Đã hủy"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Chờ duyệt"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Đã duyệt"></asp:ListItem>
                                                        <asp:ListItem Value="4" Text="Hàng về kho TQ"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="Vận chuyển Quốc tế"></asp:ListItem>
                                                        <asp:ListItem Value="5" Text="Hàng về kho VN"></asp:ListItem>
                                                        <asp:ListItem Value="6" Text="Đã thanh toán"></asp:ListItem>
                                                        <asp:ListItem Value="7" Text="Đã hoàn thành"></asp:ListItem>
                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Kho TQ</td>
                                                <td>
                                                    <asp:ListBox runat="server" ID="ddlOrderWareHouseFrom"></asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Nhận hàng tại</td>
                                                <td>
                                                    <asp:ListBox runat="server" ID="ddlOrderWareHouseTo"></asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Phương thức vận chuyển</td>
                                                <td>
                                                    <asp:ListBox runat="server" ID="ddlOrderShippingType"></asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Tổng tiền</td>
                                                <td>
                                                    <asp:Literal runat="server" ID="OrderTotalPrice"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Đã trả</td>
                                                <td>
                                                    <asp:Literal runat="server" ID="OrderPaidPrice"></asp:Literal></td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Còn lại</td>
                                                <td>
                                                    <asp:Literal runat="server" ID="OrderRemainingPrice"></asp:Literal></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="action-btn center-align">
                                    <asp:Button runat="server" ID="btnUpdate" class="btn mt-2" OnClick="btnUpdate_Click" Text="Cập nhật" />
                                    <a href="/manager/transportation-list" class="btn mt-2">Trở về</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col s12 m12 l8">
                        <div class="card-panel">
                            <div class="section history-list">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Danh sách kiện hàng</h6>
                                </div>
                                <div class="child-fee">
                                    <div class="content-panel">
                                        <div class="responsive-tb">
                                            <table class="table   highlight   ">
                                                <thead>
                                                    <tr>
                                                        <th>ID</th>
                                                        <th class="tb-date">Mã vận đơn</th>
                                                        <th style="min-width: 100px">Loại hàng hóa</th>
                                                        <th>Số lượng</th>
                                                        <th>Cân nặng</th>
                                                        <th>KĐ</th>
                                                        <th>ĐG</th>
                                                        <th>BH</th>
                                                        <th>COD TQ (Tệ)</th>
                                                        <th class="tb-date">Ghi chú</th>
                                                        <th class="no-wrap">Trạng thái</th>
                                                    </tr>
                                                </thead>
                                                <tbody class="list-product">
                                                    <asp:Literal ID="ltrPackages" runat="server"></asp:Literal>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-panel">
                            <div class="section fee-order">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Thông tin đơn hàng</h6>
                                </div>
                                <div class="child-fee">
                                    <div class="content-panel">
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Tổng cân nặng</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox runat="server" ID="txtTotalWeight" placeholder="0" type="text" disabled></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Phí vận chuyển TQ-VN</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox runat="server" ID="txtFeeWeight" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Việt Nam đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Phí giao hàng</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox runat="server" ID="txtFeeHome" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Việt Nam đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Phí kiểm hàng</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox runat="server" ID="txtFeeCheckPackage" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Việt Nam đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Phí đóng gỗ</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox runat="server" placeholder="0" ID="txtFeePack" type="text"></asp:TextBox>
                                                        <label>Việt Nam đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Phí bảo hiểm</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox runat="server" ID="txtFeeInsurrance" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Việt Nam đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Phí vận chuyển TQ-VN</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox runat="server" ID="txtUnitPrice" onkeyup="CountFeeWeight('unit')" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Đơn giá (VNĐ)</label>
                                                    </div>
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox Enabled="false" runat="server" ID="txtFeeWeight" onkeyup="CountFeeWeight('count')" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Tổng phí (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Tổng COD Trung Quốc</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox runat="server" ID="txtTotalCODCNY" onkeyup="CountShipFee('ndt')" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Tệ (¥)</label>
                                                    </div>
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox Enabled="false" runat="server" ID="txtTotalCODVN" onkeyup="CountShipFee('vnd')" placeholder="0" type="text"></asp:TextBox>
                                                        <label>Việt Nam đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Tổng tiền</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox ReadOnly="true" runat="server" ID="txtTotalPriceCNY" placeholder="0" type="text"
                                                            class="red-text bold"></asp:TextBox>
                                                        <label>Tệ (¥)</label>
                                                    </div>
                                                    <div class="input-field col s12 m6">
                                                        <asp:TextBox ReadOnly="true" runat="server" ID="txtTotalPriceVND" placeholder="0" type="text"
                                                            class="red-text bold"></asp:TextBox>
                                                        <label>Việt Nam đồng (VNĐ)</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Ghi chú khách hàng</p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m12">
                                                        <asp:TextBox runat="server" ID="lblNote" placeholder="" TextMode="MultiLine" Width="100%" Height="100px" type="text"
                                                            class="red-text bold"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="order-row">
                                            <div class="left-fixed">
                                                <p class="txt">Ghi chú nội bộ </p>
                                            </div>
                                            <div class="right-content">
                                                <div class="row">
                                                    <div class="input-field col s12 m12">
                                                        <asp:TextBox runat="server" ID="txtStaffNote" placeholder="" TextMode="MultiLine" Width="100%" Height="100px" type="text"
                                                            class="red-text bold"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfCurrency" runat="server" />
    <asp:HiddenField ID="hdfStatus" runat="server" />
    <script src="/App_Themes/OrderList/js/fancybox-master/dist/jquery.fancybox.min.js"></script>
    <script>
        $(document).ready(function () {

            $(window).scroll(function () {
                var id = $('.table-of-contents li a.active').attr('href');

                $('.scrollspy').each(function () {
                    var itemId = $(this).attr('id');
                    if (('#' + itemId) == id) {
                        $(this).parent().css({
                            'box-shadow': '0 8px 17px 2px rgba(0, 0, 0, 0.14), 0 3px 14px 2px rgba(0, 0, 0, 0.12), 0 5px 5px -3px rgba(0, 0, 0, 0.2)',

                        });
                        $('.scrollspy').not(this).parent().css({
                            'box-shadow': 'rgba(0, 0, 0, 0.14) 0px 2px 2px 0px, rgba(0, 0, 0, 0.12) 0px 3px 1px -2px, rgba(0, 0, 0, 0.2) 0px 1px 5px 0px',

                        });
                    }

                });

            });

        });

        var Currency = $("#<%=hdfCurrency.ClientID%>").val();
        function CountShipFee(type) {
            var shipfeendt = $("#<%= txtTotalCODCNY.ClientID%>").val();
            var shipfeevnd = $("#<%= txtTotalCODVN.ClientID%>").val();
            if (type == "vnd") {
                if ((shipfeevnd) != null || shipfeevnd != "") {
                    var ndt = shipfeevnd / Currency;
                    $("#<%= txtTotalCODCNY.ClientID%>").val(ndt);
                }
                else {
                    $("#<%= txtTotalCODVN.ClientID%>").val(0);
                    $("#<%= txtTotalCODCNY.ClientID%>").val(0);
                }
            }
            else {
                if ((shipfeendt) != null || shipfeendt != "") {

                    var vnd = shipfeendt * Currency;
                    $("#<%= txtTotalCODVN.ClientID%>").val(vnd);
                }
                else {
                    $("#<%= txtTotalCODVN.ClientID%>").val(0);
                    $("#<%= txtTotalCODCNY.ClientID%>").val(0);
                }
            }
        }

        var TotalWeight = $("#<%=txtTotalWeight.ClientID%>").val();
        function CountFeeWeight(type) {
            var unitPrice = $("#<%= txtUnitPrice.ClientID%>").val();
            var feeWeight = $("#<%= txtFeeWeight.ClientID%>").val();
            if (type == "unit") {
                if ((unitPrice) != null || unitPrice != "") {
                    var total = unitPrice * TotalWeight;
                    $("#<%= txtFeeWeight.ClientID%>").val(total);
                }
                else {
                    $("#<%= txtUnitPrice.ClientID%>").val(0);
                    $("#<%= txtFeeWeight.ClientID%>").val(0);
                }
            }
            else {
                if ((feeWeight) != null || feeWeight != "") {

                    var unit = feeWeight / TotalWeight;
                    $("#<%= txtUnitPrice.ClientID%>").val(unit);
                }
                else {
                    $("#<%= txtUnitPrice.ClientID%>").val(0);
                    $("#<%= txtFeeWeight.ClientID%>").val(0);
                }
            }
        }
    </script>
</asp:Content>
