<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Warehouse-Config.aspx.cs" MasterPageFile="~/manager/adminMasterNew.Master" Inherits="NHST.manager.ConfigWarehouse" %>

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
                    <h4 class="title no-margin" style="display: inline-block;">CẤU HÌNH KHO TRUNG QUỐC, VIỆT NAM, PHƯƠNG THỨC</h4>
                </div>
            </div>


            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="tb-header mb-2">
                        <h5>Danh sách kho Trung Quốc</h5>
                        <a href="#addKhoChina" class="btn modal-trigger waves-effect" style="margin-top: 1%;">Thêm kho Trung Quốc</a>
                    </div>
                    <div class="responsive-tb">
                        <table class="table bordered centered highlight">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Tên kho Trung Quốc</th>
                                    <th>Trạng thái</th>
                                    <th>Thao tác</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal runat="server" ID="ltrListKhoChina"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>


            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="tb-header mb-2">
                        <h5>Danh sách kho Việt Nam</h5>
                        <a href="#addKhoVietNam" class="btn modal-trigger waves-effect" style="margin-top: 1%;">Thêm kho Việt Nam</a>
                    </div>
                    <div class="responsive-tb">
                        <table class="table bordered centered highlight">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Tên kho Việt Nam</th>
                                    <th>Trạng thái</th>
                                    <th>Thao tác</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal runat="server" ID="ltrListKhoVietNam"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="tb-header mb-2">
                        <h5>Danh sách phương thức vận chuyển</h5>
                         <a href="#addShippingType" class="btn modal-trigger waves-effect" style="margin-top: 1%;">Thêm hình thức vận chuyển</a>
                    </div>
                    <div class="responsive-tb">
                        <table class="table bordered centered highlight">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Tên hình thức vận chuyển</th>
                                    <th>Trạng thái</th>
                                    <th>Thao tác</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal runat="server" ID="ltrListHinhThuc"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div id="modalEditKhoChina" class="modal">
                <div class="modal-hd">
                    <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
                    <h4 class="no-margin center-align">Chỉnh sửa kho Trung Quốc</h4>
                </div>
                <div class="modal-bd">
                    <div class="row">
                        <div class="input-field col s12 m12">
                            <asp:TextBox runat="server" ID="pNameKhoChina" placeholder="" type="text" class="validate" data-type="text-only"></asp:TextBox>
                            <label class="active" for="edit__step-name">Tên kho Trung Quốc</label>
                        </div>

                        <div class="input-field col s12">
                            <div class="switch status-func">
                                <span class="mr-2">Trạng thái:  </span>
                                <label>
                                    Ẩn<asp:TextBox ID="EditChinaStatus" runat="server" type="checkbox" onclick="StatusChinaFunction()"></asp:TextBox><span class="lever"></span>
                                    Hiện
                                </label>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-ft">
                    <div class="ft-wrap center-align">
                        <a id="BtnUpChina" onclick="btnUpKhoChina()" class="modal-action btn modal-close waves-effect waves-green mr-2">Cập nhật</a>
                        <a href="#!" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
                    </div>
                </div>
            </div>

            <asp:Button ID="buttonUpdateKhoChina" runat="server" OnClick="BtnUpKhoChina_Click" UseSubmitBehavior="false" Style="display: none" />
            <asp:HiddenField ID="hdfKhoChinaID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfKhoChinaStatus" runat="server" Value="0" />

            <div id="modalEditKhoVietNam" class="modal">
                <div class="modal-hd">
                    <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
                    <h4 class="no-margin center-align">Chỉnh sửa kho Việt Nam</h4>
                </div>
                <div class="modal-bd">
                    <div class="row">
                        <div class="input-field col s12 m12">
                            <asp:TextBox runat="server" ID="pNameKhoVietNam" placeholder="" type="text" class="validate" data-type="text-only"></asp:TextBox>
                            <label class="active" for="edit__step-name">Tên kho Việt Nam</label>
                        </div>

                        <div class="input-field col s12">
                            <div class="switch status-func">
                                <span class="mr-2">Trạng thái:  </span>
                                <label>
                                    Ẩn<asp:TextBox ID="EditVietNamStatus" runat="server" type="checkbox" onclick="StatusVietNamFunction()"></asp:TextBox><span class="lever"></span>
                                    Hiện
                                </label>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-ft">
                    <div class="ft-wrap center-align">
                        <a id="BtnUpVietNam" onclick="btnUpKhoViet()" class="modal-action btn modal-close waves-effect waves-green mr-2">Cập nhật</a>
                        <a href="#!" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
                    </div>
                </div>
            </div>

            <asp:Button ID="buttonUpdateKhoVietNam" runat="server" OnClick="BtnUpKhoVietNam_Click" UseSubmitBehavior="false" Style="display: none" />
            <asp:HiddenField ID="hdfKhoVietNamID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfKhoVietNamStatus" runat="server" Value="0" />

            <div id="modalEditShippingType" class="modal">
                <div class="modal-hd">
                    <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
                    <h4 class="no-margin center-align">Chỉnh sửa hình thức vận chuyển</h4>
                </div>
                <div class="modal-bd">
                    <div class="row">
                        <div class="input-field col s12 m12">
                            <asp:TextBox runat="server" ID="pNameShippingType" placeholder="" type="text" class="validate" data-type="text-only"></asp:TextBox>
                            <label class="active" for="edit__step-name">Tên hình thức vận chuyển</label>
                        </div>

                        <div class="input-field col s12">
                            <div class="switch status-func">
                                <span class="mr-2">Trạng thái:  </span>
                                <label>
                                    Ẩn<asp:TextBox ID="EditShippingTypeStatus" runat="server" type="checkbox" onclick="StatusShippingTypeFunction()"></asp:TextBox><span class="lever"></span>
                                    Hiện
                                </label>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-ft">
                    <div class="ft-wrap center-align">
                        <a id="BtnUpShippingType" onclick="btnUpShippingType()" class="modal-action btn modal-close waves-effect waves-green mr-2">Cập nhật</a>
                        <a href="#!" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
                    </div>
                </div>
            </div>

            <asp:Button ID="buttonUpdateShippingType" runat="server" OnClick="BtnUpShippingType_Click" UseSubmitBehavior="false" Style="display: none" />
            <asp:HiddenField ID="hdfShippingTypeID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShippingTypeStatus" runat="server" Value="0" />

            <div id="addKhoChina" class="modal">
                <div class="modal-hd">
                    <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
                    <h4 class="no-margin center-align">Thêm kho Trung Quốc</h4>
                </div>
                <div class="modal-bd">
                    <div class="row">
                        <div class="input-field col s12 m12">
                            <asp:TextBox runat="server" ID="txtNameKhoChina"></asp:TextBox>
                            <label>Tên kho Trung Quốc</label>
                        </div>
                    </div>
                </div>
                <div class="modal-ft">
                    <div class="ft-wrap center-align">
                        <asp:Button runat="server" ID="btnCreate" class="modal-action btn waves-effect waves-green mr-2" Text="Thêm" UseSubmitBehavior="false" OnClick="btnCreateKhoChina_Click" />
                        <a href="javascript:;" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
                    </div>
                </div>
            </div>

            <div id="addKhoVietNam" class="modal">
                <div class="modal-hd">
                    <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
                    <h4 class="no-margin center-align">Thêm kho Việt Nam</h4>
                </div>
                <div class="modal-bd">
                    <div class="row">
                        <div class="input-field col s12 m12">
                            <asp:TextBox runat="server" ID="txtNameKhoVietNam"></asp:TextBox>
                            <label>Tên kho Việt Nam</label>
                        </div>
                    </div>
                </div>
                <div class="modal-ft">
                    <div class="ft-wrap center-align">
                        <asp:Button runat="server" ID="Button1" class="modal-action btn waves-effect waves-green mr-2" Text="Thêm" UseSubmitBehavior="false" OnClick="btnCreateKhoVietNam_Click" />
                        <a href="javascript:;" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
                    </div>
                </div>
            </div>

            <div id="addShippingType" class="modal">
                <div class="modal-hd">
                    <span class="right"><i class="material-icons modal-close right-align">clear</i></span>
                    <h4 class="no-margin center-align">Thêm hình thức vận chuyển</h4>
                </div>
                <div class="modal-bd">
                    <div class="row">
                        <div class="input-field col s12 m12">
                            <asp:TextBox runat="server" ID="txtNameShippingType"></asp:TextBox>
                            <label>Tên hình thức vận chuyển</label>
                        </div>
                    </div>
                </div>
                <div class="modal-ft">
                    <div class="ft-wrap center-align">
                        <asp:Button runat="server" ID="Button2" class="modal-action btn waves-effect waves-green mr-2" Text="Thêm" UseSubmitBehavior="false" OnClick="btnCreateShippingType_Click" />
                        <a href="javascript:;" class="modal-action btn orange darken-2 modal-close waves-effect waves-green ml-2">Hủy</a>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        function btnUpKhoChina() {
            $('#<%=buttonUpdateKhoChina.ClientID%>').click();
        }
        function btnUpKhoViet() {
            $('#<%=buttonUpdateKhoVietNam.ClientID%>').click();
        }
        function btnUpShippingType() {
            $('#<%=buttonUpdateShippingType.ClientID%>').click();
        }

        function EditKhoChina(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/Warehouse-Config.aspx/LoadInforChina",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data != null) {
                        $('#<%=pNameKhoChina.ClientID%>').val(data.WareHouseName);
                        var a = data.IsHidden;
                        if (a == false) {
                            $('#<%=EditChinaStatus.ClientID%>').prop('checked', true);
                            $('#<%=hdfKhoChinaStatus.ClientID%>').val('0');
                        }
                        else {
                            $('#<%=EditChinaStatus.ClientID%>').prop('checked', false);
                            $('#<%=hdfKhoChinaStatus.ClientID%>').val('1');
                        }
                        $('#<%=hdfKhoChinaID.ClientID%>').val(data.ID);
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

        function EditKhoVN(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/Warehouse-Config.aspx/LoadInforVN",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data != null) {
                        $('#<%=pNameKhoVietNam.ClientID%>').val(data.WareHouseName);
                        var a = data.IsHidden;
                        if (a == false) {
                            $('#<%=EditVietNamStatus.ClientID%>').prop('checked', true);
                            $('#<%=hdfKhoVietNamStatus.ClientID%>').val('0');
                        }
                        else {
                            $('#<%=EditVietNamStatus.ClientID%>').prop('checked', false);
                            $('#<%=hdfKhoVietNamStatus.ClientID%>').val('1');
                        }
                        $('#<%=hdfKhoVietNamID.ClientID%>').val(data.ID);
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

        function EditShippingType(ID) {
            $.ajax({
                type: "POST",
                url: "/manager/Warehouse-Config.aspx/LoadInforShippingType",
                data: '{ID: "' + ID + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    if (data != null) {
                        $('#<%=pNameShippingType.ClientID%>').val(data.ShippingTypeName);
                        var a = data.IsHidden;
                        if (a == false) {
                            $('#<%=EditShippingTypeStatus.ClientID%>').prop('checked', true);
                            $('#<%=hdfShippingTypeStatus.ClientID%>').val('0');
                        }
                        else {
                            $('#<%=EditShippingTypeStatus.ClientID%>').prop('checked', false);
                            $('#<%=hdfShippingTypeStatus.ClientID%>').val('1');
                        }
                        $('#<%=hdfShippingTypeID.ClientID%>').val(data.ID);
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

        function StatusChinaFunction() {
            var a = $('#<%=hdfKhoChinaStatus.ClientID%>').val();
            if (a == '0') {

                $('#<%=hdfKhoChinaStatus.ClientID%>').val('1');
            }
            else {

                $('#<%=hdfKhoChinaStatus.ClientID%>').val('0');
            }
        }

        function StatusVietNamFunction() {
            var a = $('#<%=hdfKhoVietNamStatus.ClientID%>').val();
            if (a == '0') {

                $('#<%=hdfKhoVietNamStatus.ClientID%>').val('1');
            }
            else {

                $('#<%=hdfKhoVietNamStatus.ClientID%>').val('0');
            }
        }

        function StatusShippingTypeFunction() {
            var a = $('#<%=hdfShippingTypeStatus.ClientID%>').val();
            if (a == '0') {

                $('#<%=hdfShippingTypeStatus.ClientID%>').val('1');
             }
             else {

                 $('#<%=hdfShippingTypeStatus.ClientID%>').val('0');
            }
        }
    </script>
</asp:Content>
