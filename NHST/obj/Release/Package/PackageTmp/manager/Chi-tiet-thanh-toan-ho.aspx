<%@ Page Title="Chi tiết thanh toán hộ" Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="Chi-tiet-thanh-toan-ho.aspx.cs" Inherits="NHST.manager.Chi_tiet_thanh_toan_ho" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
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
                    <h4 class="title no-margin" style="display: inline-block;">CHI TIẾT YÊU CẦU</h4>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="order-detail-wrap col s12 section">
                <div class="row">
                    <div class="col s12 m12 l4 sticky-wrap">
                        <div class="card-panel">
                            <p>Ảnh thanh toán hộ:</p>
                            <asp:Literal runat="server" ID="ltrImagePay"></asp:Literal>
                            <div style="display: flex;">
                                <asp:FileUpload runat="server" ID="EditIMG" class="upload-img" type="file" AllowMultiple="true" onchange="readFile(this)" title=""></asp:FileUpload>
                                <a class="btn-upload">Upload</a>
                            </div>
                            <div class="preview-img">
                            </div>
                        </div>
                        <div class="card-panel">
                            <div class="order-stick-detail">
                                <div class="order-stick order-owner">
                                    <table class="table">
                                        <tbody>
                                            <tr>
                                                <td class="tb-date">ID</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lbID"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Trạng thái</td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltrStatus"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tb-date">Tổng tiền</td>
                                                <td>
                                                    <asp:Label runat="server" ID="lbTongTien"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="action-btn center-align">
                                    <a href="javascript:;" class="btn mt-2" onclick="update()">Cập nhật</a>
                                    <a href="javascript:;" class="btn mt-2" style="background-color: blue; margin-left: 5px; margin-right: 5px;" onclick="Payment()">Thanh toán</a>
                                    <asp:Literal runat="server" ID="ltrBack"></asp:Literal>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col s12 m12 l8">
                        <div class="card-panel">
                            <div class="section">
                                <div class="title-header bg-dark-gradient">
                                    <h6 class="white-text ">Chi tiết hóa đơn</h6>
                                </div>
                                <div class="child-fee">
                                    <div class="content-panel">
                                        <div class="row section">
                                            <div class="col s12">
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Username:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtUserName" type="text" disabled></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Số điện thoại:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtPhoneNumber" placeholder="0" disabled></asp:TextBox>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Tỉ giá thật:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtCurrency" placeholder="0"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Tỉ giá khách:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtPriceVND" placeholder="0"></asp:TextBox>
                                                                <label>Việt Nam Đồng (VNĐ)</label>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="order-row top-justify">
                                                    <div class="left-fixed">
                                                        <p class="txt">Hóa đơn thanh toán hộ:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="list-order">
                                                            <asp:Literal runat="server" ID="ltrList"></asp:Literal>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Tổng tiền ban đầu:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtFinalPriceCNY" placeholder="0" class="" disabled></asp:TextBox>
                                                                <label>Tệ (¥)</label>
                                                            </div>
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtFinalPriceVND" placeholder="0" disabled></asp:TextBox>
                                                                <label>Việt Nam Đồng (VNĐ)</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Phần trăm (%):</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtPercent" placeholder="0" value="0" type="number"></asp:TextBox>
                                                                <label>Phần trăm (%)</label>
                                                            </div>
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtFeeBuyPro" value="0" placeholder="0"></asp:TextBox>
                                                                <label>Việt Nam Đồng (VNĐ)</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Tổng tiền đã có phí dịch vụ:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtTotalPriceCNY" placeholder="0" class=""></asp:TextBox>
                                                                <label>Tệ (¥)</label>
                                                            </div>
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtTotalPriceVND" placeholder="0"></asp:TextBox>
                                                                <label>Việt Nam Đồng (VNĐ)</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="order-row" style="display: none">
                                                    <div class="left-fixed">
                                                        <p class="txt">Mã đơn hàng:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtMaDonHang" placeholder="0"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="order-row" style="display: none">
                                                    <div class="left-fixed">
                                                        <p class="txt">Cân nặng:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:TextBox runat="server" ID="txtCanNang" placeholder="0"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Ghi chú:</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12">
                                                                <asp:TextBox TextMode="MultiLine" runat="server" ID="txtSummary"
                                                                    CssClass="materialize-textarea">Lorem ipsum dolor sit amet consectetur adipisicing elit. Neque praesentium consectetur optio ipsa laborum! Consequuntur sed voluptatum non eum fugit reiciendis, quia nobis culpa eligendi rerum! Repudiandae fugit corrupti quidem!</asp:TextBox>
                                                                <label for="textarea2">Nội dung</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="order-row">
                                                    <div class="left-fixed">
                                                        <p class="txt">Trạng thái</p>
                                                    </div>
                                                    <div class="right-content">
                                                        <div class="row">
                                                            <div class="input-field col s12 m6">
                                                                <asp:ListBox runat="server" ID="ddlStatusDetail">
                                                                    <asp:ListItem Text="Đã hủy" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Đơn mới" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Đã xác nhận" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Đã thanh toán" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="Hoàn thành" Value="4"></asp:ListItem>
                                                                </asp:ListBox>
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

        </div>
    </div>
    <asp:HiddenField ID="hdfList" runat="server" />
    <asp:HiddenField ID="hdfListIMG" runat="server" />
    <asp:Button Style="display: none" runat="server" ID="btnSave" OnClick="btnSave_Click" UseSubmitBehavior="false" />
    <asp:Button Style="display: none" runat="server" ID="btnPay" OnClick="btnPayment_Click" UseSubmitBehavior="false" />
    <asp:Button Style="display: none" runat="server" ID="btnBack" OnClick="btnBack_Click" UseSubmitBehavior="false" />
    <script src="/App_Themes/OrderList/js/fancybox-master/dist/jquery.fancybox.min.js"></script>
    <script>        
        function update() {
            var base64 = "";
            $(".preview-img img").each(function () {
                base64 += $(this).attr('src') + "|";
            })
            $("#<%=hdfListIMG.ClientID%>").val(base64);
            $("#<%=btnSave.ClientID%>").click();
        }
        function Payment() {

            $("#<%=btnPay.ClientID%>").click();
        }
        function Delete(obj) {
            obj.parent().remove();
        }
        function readFile(input) {
            var k = 0;
            var counter = input.files.length;
            for (x = 0; x < counter; x++) {
                if (input.files && input.files[x]) {
                    var reader = new FileReader();
                    var t = k + x;
                    reader.onload = function (e) {
                        var a = "<div class=\"img-block\"><img class=\"materialboxed 2" + t + "\" src =\"" + e.target.result + "\" ><span class=\"material-icons red-text delete\" onclick=\"Delete($(this))\">clear</span></div>";
                        $(".preview-img").append(a);
                        $(".materialboxed").materialbox({
                            inDuration: 150,
                            onOpenStart: function (element) {
                                $(element).parents('.material-placeholder').attr('style', 'overflow:visible !important;');
                            },
                            onCloseStart: function (element) {
                                $(element).parents('.material-placeholder').attr('style', '');
                            }
                        });
                        //$(".preview-img").append('<li class=\"2' + t + '\"><img src="' + e.target.result + '" class="img-thumbnail"><a href=\"javascript:;\" onclick=\"Delete($(this))\">Xóa</a></li>');
                    };
                    reader.readAsDataURL(input.files[x]);
                    k++;
                }
            }
        }
        </script>
</asp:Content>