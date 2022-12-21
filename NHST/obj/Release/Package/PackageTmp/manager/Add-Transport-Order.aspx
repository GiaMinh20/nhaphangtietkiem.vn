<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add-Transport-Order.aspx.cs" MasterPageFile="~/manager/adminMasterNew.Master" Inherits="NHST.manager.Add_Transport_Order" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">

            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Tạo đơn hàng vận chuyển hộ cho khách</h4>
                </div>
            </div>

            <div class="create-order-cus col s12 section">
                <div class="order-list-info card-panel">
                    <div class="total-info mb-2 create-product">
                        <div class="row">

                            <div class="search-name input-field col s12 l3">
                                <%-- <asp:TextBox ID="txtUsername" name="txtUsername" type="text" placeholder="" runat="server" />--%>
                                <asp:DropDownList ID="ddlUsername" runat="server" CssClass="select2"
                                    DataValueField="ID" DataTextField="Username">
                                </asp:DropDownList>
                                <label for="search_name"><span>Username</span></label>
                            </div>

                            <div class="search-name input-field col s12 l3">
                                <asp:DropDownList runat="server" ID="ddlKhoTQ" AppendDataBoundItems="true" DataTextField="WareHouseName" DataValueField="ID"></asp:DropDownList>
                                <label for="search_name"><span>Kho Trung Quốc</span></label>
                            </div>

                            <div class="search-name input-field col s12 l3">
                                <asp:DropDownList runat="server" ID="ddlKhoVN" AppendDataBoundItems="true" DataTextField="WareHouseName" DataValueField="ID"></asp:DropDownList>
                                <label for="search_name"><span>Kho Việt Nam</span></label>
                            </div>

                            <div class="search-name input-field col s12 l3">
                                <asp:DropDownList runat="server" ID="ddlShipping" AppendDataBoundItems="true" DataTextField="ShippingTypeName" DataValueField="ID"></asp:DropDownList>
                                <label for="search_name"><span>Phương thức vận chuyển</span></label>
                            </div>

                            <div class="row section">
                                <div class="col s12">
                                    <div class="order-row">
                                        <div class="left-fixed">
                                            <p class="txt">Danh sách kiện ký gửi:</p>
                                        </div>
                                        <div class="right-content">
                                            <div class="float-right mt-2 mb-2">
                                                <a href="javascript:;" class="btn add-product valign-wrapper" style="display: flex"><i class="material-icons">add</i><span>Thêm kiện</span></a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="responsive-tb">
                                        <table class="table    highlight bordered  centered   ">
                                            <thead>
                                                <tr>
                                                    <th class="tb-date">Mã kiện</th>
                                                    <th>Loại hàng hóa</th>
                                                    <th style="width: 100px">Số lượng</th>
                                                    <th style="width: 100px">Cân nặng<br />
                                                        kg</th>
                                                    <th>Kiểm đếm</th>
                                                    <th>Đóng gỗ</th>
                                                    <th>Bảo hiểm</th>
                                                    <th>COD TQ (Tệ)</th>
                                                    <th class="tb-date">Ghi chú</th>
                                                    <th class="no-wrap">Thao tác</th>
                                                </tr>
                                            </thead>
                                            <tbody class="list-product">
                                                <tr class="slide-up product-item">
                                                    <td>
                                                        <input class="pack-code" type="text"></td>
                                                    <td>
                                                        <input class="pack-type" type="text"></td>
                                                    <td>
                                                        <input class="pack-quantity" type="number" value="1"></td>
                                                    <td>
                                                        <input class="pack-weight" type="number" value="0"></td>

                                                    <td class="center-checkbox">
                                                        <label>
                                                            <input class="pack-checkproduct" type="checkbox" />
                                                            <span></span>
                                                        </label>
                                                    </td>
                                                    <td class="center-checkbox">
                                                        <label>
                                                            <input class="pack-packaged" type="checkbox" />
                                                            <span></span>
                                                        </label>
                                                    </td>
                                                    <td class="center-checkbox">
                                                        <label>
                                                            <input class="pack-insurrance" type="checkbox" />
                                                            <span></span>
                                                        </label>
                                                    </td>
                                                    <td>
                                                        <input class="pack-codtq" type="number" value="0"></td>
                                                    <td>
                                                        <input class="pack-note" type="text" value=""></td>
                                                    <td class="">
                                                        <!-- Dropdown Trigger -->
                                                        <a href='javascript:;' class="remove-product tooltipped" data-position="top" data-tooltip="Xóa"><i class="material-icons valign-center">remove_circle</i></a>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="float-right mt-2">
                                        <a href="javascript:;" onclick="CreateOrder()" class="btn create-order">Tạo đơn hàng</a>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfProductList" runat="server" />
    <asp:Button ID="btncreateuser" runat="server" Text="Cập nhật" UseSubmitBehavior="false" CssClass="btn btn-success btn-block pill-btn primary-btn main-btn hover" OnClick="btncreateuser_Click" Style="display: none" />
    <script>
        $(document).ready(function () {
            $('.create-product .add-product').on('click', function () {
                var tableHTML = $('.create-product table .list-product');
                var html = ` <tr class="slide-up product-item">
                        <td><input class="pack-code" type="text" value=""></td>
                        <td><input class="pack-type" type="text" value=""></td>          
                        <td><input class="pack-quantity" type="number" value="1"></td> 
                        <td><input class="pack-weight" type="number" value="0"></td>                       
                        <td class="center-checkbox">
                           <label>
                           <input class="pack-checkproduct" type="checkbox" />
                           <span></span>
                           </label>
                        </td>
                        <td class="center-checkbox">
                           <label>
                           <input class="pack-packaged" type="checkbox" />
                           <span></span>
                           </label>
                        </td>  
                        <td class="center-checkbox">
                           <label>
                           <input class="pack-insurrance" type="checkbox" />
                           <span></span>
                           </label>
                        </td>  
                        <td><input class="pack-codtq" type="number" value="0"></td>      
                        <td><input class="pack-note" type="text" value=""></td>   
                        <td class="">
                        <a href='javascript:;' class="remove-product tooltipped" data-position="top" data-tooltip="Xóa"><i class="material-icons valign-center">remove_circle</i></a>                        
                        </td>
                     </tr>`;
                tableHTML.append(html);

                $('.tooltipped')
                    .tooltip({
                        trigger: 'manual'
                    })
                    .tooltip('show');

            });
            $('.create-product').on('click', '.remove-product', function () {
                $(this).parent().parent().fadeOut(function () {
                    $(this).remove();
                });
            });
        });

        function keypress(e) {
            var keypressed = null;
            if (window.event) {
                keypressed = window.event.keyCode; //IE
            }
            else {
                keypressed = e.which; //NON-IE, Standard
            }
            if (keypressed < 48 || keypressed > 57) {
                if (keypressed == 8 || keypressed == 127) {
                    return;
                }
                return false;
            }
        }
        function CreateOrder() {
            var html = "";
            var check = false;
            var khotq = $("#<%=ddlKhoTQ.ClientID%>").val();
            var khovn = $("#<%=ddlKhoVN.ClientID%>").val();
            var shipping = $("#<%=ddlShipping.ClientID%>").val();
            if (khotq == 0) {
                alert('Vui lòng chọn kho TQ');
                return;
            }
            if (khovn == 0) {
                alert('Vui lòng chọn kho VN');
                return;
            }
            if (shipping == 0) {
                alert('Vui lòng chọn phương thức vận chuyển');
                return;
            }
            $(".product-item").each(function () {
                var item = $(this);
                var packcode_obj = item.find(".pack-code");
                var packcode = item.find(".pack-code").val();

                var packtype_obj = item.find(".pack-type");
                var packtype = item.find(".pack-type").val();

                var packquantity_obj = item.find(".pack-quantity");
                var packquantity = item.find(".pack-quantity").val();

                var packweight_obj = item.find(".pack-weight");
                var packweight = item.find(".pack-weight").val();
                var packweightfloat = parseFloat(item.find(".pack-weight").val());

                var packcheckproduct = item.find(".pack-checkproduct").val();
                var packpackaged = item.find(".pack-packaged").val();
                var packinsurrance = item.find(".pack-insurrance").val();

                var packcodtq_obj = item.find(".pack-codtq");
                var packcodtq = item.find(".pack-codtq").val();
                var packcodtqfloat = parseFloat(item.find(".pack-codtq").val());

                var packnote = item.find(".pack-note").val();

                if (isBlank(packcode)) {
                    check = true;
                }
                if (isBlank(packtype)) {
                    check = true;
                }
                if (isBlank(packquantity)) {
                    check = true;
                }

                validateText(packcode_obj);
                validateText(packtype_obj);
            });
            if (check == true) {
                alert('Vui lòng điền đầy đủ thông tin từng sản phẩm');
            }
            else {
                $(".product-item").each(function () {
                    var item = $(this);
                    var packcode = item.find(".pack-code").val();
                    var packtype = item.find(".pack-type").val();
                    var packquantity = item.find(".pack-quantity").val();
                    var packweight = item.find(".pack-weight").val();

                    var checkproduct = 0;
                    var packaged = 0;
                    var insurrance = 0;
                    if (item.find(".pack-checkproduct").is(":checked")) {
                        checkproduct = 1
                    }
                    if (item.find(".pack-packaged").is(":checked")) {
                        packaged = 1;
                    }
                    if (item.find(".pack-insurrance").is(":checked")) {
                        insurrance = 1;
                    }
                    var packcheckproduct = item.find(".pack-checkproduct").val();
                    var packpackaged = item.find(".pack-packaged").val();
                    var packinsurrance = item.find(".pack-insurrance").val();

                    var packcodtq = item.find(".pack-codtq").val();
                    var packnote = item.find(".pack-note").val();

                    html += packcode + "]" + packtype + "]" + packquantity + "]" + packweight + "]"
                        + checkproduct + "]" + packaged + "]" + insurrance + "]" + packcodtq + "]" + packnote + "|";
                });

                $.ajax({
                    type: "POST",
                    url: "/manager/Add-Transport-Order.aspx/checkbefore",
                    data: "{listStr:'" + html + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var ret = msg.d;
                        var base64 = "";
                        $(".preview-img img").each(function () {
                            base64 += $(this).attr('src') + "|";
                        })
                        if (ret == "ok") {
                            $("#<%=hdfProductList.ClientID%>").val(html);
                            $("#<%=btncreateuser.ClientID%>").click();
                        }
                        else {
                            alert('Các mã vận đơn tồn tại: ' + ret + '. Vui lòng thay đổi mã.');
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                    }
                });
            }
        }
        function validateText(obj) {
            var value = obj.val();
            if (isBlank(value)) {
                obj.addClass("border-select");
            }
            else {
                obj.removeClass("border-select");
            }
        }
        function validateNumberLessEqualzero(obj) {
            var value = parseFloat(obj.val());
            if (value <= 0)
                obj.addClass("border-select");
            else
                obj.removeClass("border-select");
        }
        function validateNumber(obj) {
            var value = parseFloat(obj.val());
            if (value < 0)
                obj.addClass("border-select");
            else
                obj.removeClass("border-select");
        }
        function isBlank(str) {
            return (!str || /^\s*$/.test(str));
        }
    </script>
    <style>
        .border-select {
            border: solid 2px red !important;
        }
    </style>
</asp:Content>
