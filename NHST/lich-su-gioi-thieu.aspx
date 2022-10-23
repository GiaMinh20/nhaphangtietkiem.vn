<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lich-su-gioi-thieu.aspx.cs" MasterPageFile="~/UserMasterNew.Master" Inherits="NHST.lich_su_gioi_thieu" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
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
                                        <h4>LỊCH SỬ GIỚI THIỆU</h4>
                                    </div>
                                </div>
                                <div class="col s12">
                                    <div class="order-list-info">
                                        <div class="total-info mb-2">
                                            <div class="row section">
                                                <div class="col s12">

                                                    <p class="center-align">
                                                        Tỷ giá quy đổi 1 xu = <span class="teal-text text-darken-4 font-weight-700">
                                                            <asp:Literal runat="server" ID="lblQuyDoi"></asp:Literal>
                                                            Đồng</span>
                                                        <span class="black-text divi">|</span> Số xu hiện tại: <span class="teal-text text-darken-4 font-weight-700">
                                                            <asp:Literal runat="server" ID="lblAccount"></asp:Literal></span> Xu
                                                    </p>

                                                    <a href="javascript:;" class="btn" id="filter-btn">Bộ lọc</a>
                                                    <a href="javascript:;" class="btn btnAddWallet" onclick="AddWallet($(this))" style="float: right; margin-left: 10px; background-color: green">Quy đổi xu vào ví</a>
                                                    <asp:Button runat="server" ID="btnUpdate" UseSubmitBehavior="false" Style="display: none" OnClick="btnUpdate_Click" />

                                                    <div class="filter-wrap mb-2">
                                                        <div class="row">
                                                            <div class="input-field col s6 l4">
                                                                <asp:TextBox runat="server" ID="rFD" placeholder="" CssClass="datetimepicker from-date"></asp:TextBox>
                                                                <label>Từ ngày</label>
                                                            </div>
                                                            <div class="input-field col s6 l4">
                                                                <asp:TextBox runat="server" placeholder="" ID="rTD" CssClass="datetimepicker to-date"></asp:TextBox>
                                                                <label>Đến ngày</label>
                                                                <span class="helper-text"
                                                                    data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                                                            </div>

                                                            <div class="col s12 right-align">
                                                                <asp:Button ID="btnSear" runat="server"
                                                                    CssClass="btn" OnClick="btnSear_Click" Text="Lọc kết quả" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="responsive-tb">
                                                        <table class="table highlight bordered centered mt-2">
                                                            <thead>
                                                                <tr>
                                                                    <th class="tb-date">Ngày giờ</th>
                                                                    <th>Nội dung</th>
                                                                    <th>Số xu</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:Literal ID="ltr" runat="server"></asp:Literal>
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
    <script>
        function AddWallet(obj) {
            var r = confirm("Bạn muốn quy đổi xu vào ví điện tử?");
            if (r == true) {
                obj.removeAttr("onclick");
                $('#<%=btnUpdate.ClientID%>').click();
            }
        }
    </script>
</asp:Content>
