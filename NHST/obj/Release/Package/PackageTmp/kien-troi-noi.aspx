<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kien-troi-noi.aspx.cs" MasterPageFile="~/UserMasterNew.Master" Inherits="NHST.kien_troi_noi" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
    <style>
        .yellow-gold.darken-2 {
            background-color: #e87e04 !important;
        }

        .bronze.darken-2 {
            background: #e6cb78;
        }

        .display-flex {
            display: flex;
            align-items: center;
        }

        .margin-15 {
            margin-left: 5px;
            height: 36px;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div id="main">
        <div class="row">
            <div class="content-wrapper-before blue-grey lighten-5"></div>
            <div class="col s12">
                <div class="container">
                    <div class="all">
                        <div class="card-panel mt-3 no-shadow">
                            <div class="row">
                                <div class="col s12">
                                    <div class="page-title mt-2 center-align">
                                        <h4>Danh sách hàng hóa trôi nổi</h4>
                                    </div>
                                </div>
                                <div class="col s12">
                                    <div class="order-list-info">
                                        <div class="total-info mb-2 pb-10">
                                            <div class="row section mt-1">
                                                <div class="col s12">
                                                    <div class="row">
                                                        <div class="input-field col s12 14 display-flex">
                                                            <asp:TextBox ID="txtOrderCode" placeholder="" runat="server" CssClass="search_name" Style="width: 25%"></asp:TextBox>
                                                            <label for="search_name">
                                                                <span>Nhập mã vận đơn</span></label>
                                                        
                                                            <asp:Button ID="btnSear" runat="server"
                                                                CssClass="btn margin-15" OnClick="btnSearch_Click" Text="TÌM KIẾM" />                                                        
                                                        </div>
                                                    </div>                                            
                                  
                                                    <div class="responsive-tb">
                                                        <table class="table   highlight bordered  centered bordered mt-2">
                                                            <thead>
                                                                <tr>                                                                   
                                                                    <th>ID</th>
                                                                    <th>Mã vận đơn</th>
                                                                    <th>Trạng thái</th>
                                                                    <th>Người nhận</th>
                                                                    <th>Trạng thái xác nhận</th>
                                                                    <th>Ngày tạo</th>
                                                                    <th class="tb-date">Action</th>
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
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modalConfirm" class="modal modal-fixed-footer">
        <div class="modal-hd">
            <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
            <h4 class="no-margin center-align">Xác nhận kiện trôi nổi</h4>
            <div class="page-title">
                <h5>Mã hệ thống #<asp:Label runat="server" ID="lbID"></asp:Label></h5>
            </div>
        </div>
        <div class="modal-bd">
            <div class="row">
                <div class="input-field col s12">
                    <asp:TextBox runat="server" placeholder="" ID="pBarcode" type="text" Enabled="false" class="validate" data-type="text-only"></asp:TextBox>
                    <label class="active" for="edit__step-name">Mã vận đơn</label>
                </div>
                 <div class="input-field col s12">
                    <asp:TextBox runat="server" placeholder="" ID="pPhone" type="text" class="validate" data-type="text-only"></asp:TextBox>
                    <label class="active" for="edit__step-name">Số điện thoại xác nhận</label>
                </div>                
                 <div class="input-field col s12">
                    <asp:TextBox runat="server" placeholder="" ID="pProductName" type="text" class="validate" data-type="text-only"></asp:TextBox>
                    <label class="active" for="edit__step-name">Tên sản phẩm</label>
                </div>
                 <div class="input-field col s12">
                    <asp:TextBox runat="server" placeholder="" ID="pQuantity" type="number" class="validate" data-type="text-only"></asp:TextBox>
                    <label class="active" for="edit__step-name">Số lượng sản phẩm</label>
                </div>
                 <div class="input-field col s12">
                    <asp:TextBox runat="server" placeholder="" ID="pNote" type="text" TextMode="MultiLine" class="validate" data-type="text-only"></asp:TextBox>
                    <label class="active" for="edit__step-name">Ghi chú xác nhận</label>
                </div>
                <div class="col s12 m12">
                    <span class="black-text">Hình ảnh</span>
                    <div style="display: inline-block; margin-left: 15px;">
                        <input class="upload-img" type="file" onchange="previewFiles(this);" multiple title="">
                        <button type="button" class="btn-upload">Upload</button>
                    </div>
                    <div class="preview-img">
                    </div>
                </div>
            </div>
        </div>
        <div class="modal-ft">
            <div class="ft-wrap center-align">
                <a class="modal-action btn modal-close waves-effect waves-green mr-2" id="btnUpdate">Cập nhật</a>              
                <a href="#!" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdfSMID" runat="server" Value="0" />
    <asp:HiddenField runat="server" ID="hdfListIMG" />
    <asp:Button runat="server" ID="buttonUpdate" OnClick="btncreateuser_Click" UseSubmitBehavior="false" Style="display: none" />
    <script>
        $('#btnUpdate').click(function () {
            var base64 = "";
            $(".preview-img .img-block img").each(function () {
                base64 += $(this).attr('src') + "|";
            })
            $("#<%=hdfListIMG.ClientID%>").val(base64);
            console.log(base64);
            $('#<%=buttonUpdate.ClientID%>').click();
        });

        function ConfirmFunction(ID) {
            $.ajax({
                type: "POST",
                url: "/kien-troi-noi.aspx/LoadInforVer2",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data != null) {
                        $('#<%=lbID.ClientID%>').text(ID);
                        $('#<%=pBarcode.ClientID%>').val(data.OrderTransactionCode);
                        $('#<%=pPhone.ClientID%>').val(data.ReceiptPhone);
                        $('#<%=pQuantity.ClientID%>').val(data.Quantity);
                        $('#<%=pProductName.ClientID%>').val(data.ProductName);
                        $('#<%=pNote.ClientID%>').val(data.ReceiptNotes);
                        $('#<%=hdfSMID.ClientID%>').val(data.ID);

                        var list = data.ListIMG;
                        if (list != null) {
                            var IMG = list.split('|');
                            var html = "";
                            for (var i = 0; i < IMG.length - 1; i++) {
                                if (IMG[i] != "") {
                                    html += "<div class=\"img-block\"><img class=\"materialboxed\" src =\"" + IMG[i] + "\" ><span class=\"material-icons red-text delete\" onclick=\"Delete($(this))\">clear</span></div>";
                                }
                            }
                            $(".preview-img").html(html);
                        }
                    }
                    else
                        swal("Error", "Không thành công", "error");
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    swal("Error", "Fail updateInfoAcc", "error");
                }
            });
        }
        
    </script>
</asp:Content>