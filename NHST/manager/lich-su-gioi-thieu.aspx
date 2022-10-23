<%@ Page Title="Lịch sử giới thiệu" Language="C#" AutoEventWireup="true" MasterPageFile="~/manager/adminMasterNew.Master" CodeBehind="lich-su-gioi-thieu.aspx.cs" Inherits="NHST.manager.lich_su_gioi_thieu" %>

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
                    <h4 class="title no-margin" style="display: inline-block;">Lịch sử giới thiệu</h4>
                    <div class="right-action">
                        <a href="#" class="btn" id="filter-btn">Bộ lọc</a>
                    </div>                   
                    <div class="clearfix"></div>
                    <div class="filter-wrap" style="display:block">
                        <div class="row mt-2 pt-2">
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
                                <asp:Button runat="server" ID="search" class="btn" OnClick="btnSearch_Click" Text="Lọc kết quả"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="list-commission col s12 section">
                <div class="list-table card-panel">
                    <div class="table-info row center-align-xs">
                        <div class="col s12 m6">
                            <p class="total">
                                Username: <span class="blue-text font-weight-500">
                                    <asp:Literal runat="server" ID="ltrUsername"></asp:Literal></span>
                            </p>
                        </div>
                        <div class="col s12 m6">
                            <p class="total" style="float:right">
                                Tổng số xu hiện tại: <span class="blue-text font-weight-500">
                                    <asp:Literal runat="server" ID="ltrTongSoDu"></asp:Literal></span>
                            </p>
                        </div>
                    </div>
                    <div class="responsive-tb mt-2">
                        <table class="table bordered highlight  ">
                            <thead>
                                <tr>
                                    <th class="tb-date">Ngày giờ</th>
                                    <th>Nội dung</th>
                                    <th>Số xu</th>
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
    <script>
        $(document).ready(function () {
            $('#txtSearchName').autocomplete({
                data: {
                    "Apple": null,
                    "Microsoft": null,
                    "Google": 'https://placehold.it/250x250',
                    "Asgard": null
                },
            });
        });
        function myFunction() {
            if (event.which == 13 || event.keyCode == 13) {
                $('#<%=search.ClientID%>').click();
            }
        }
        $('.search-action').click(function () {

            $('#<%=search.ClientID%>').click();
        })
    </script>
    <style>
        .total {
            font-size: 20px;
            font-weight: bold;
        }
    </style>
</asp:Content>
