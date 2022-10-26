<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OrderMaster.Master" CodeBehind="Default999.aspx.cs" Inherits="NHST.Default1" %>

<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <main>
        <div class="wrapper-banner">
            <div class="image-banner">
                <a>
                    <img src="/App_Themes/CSSHQT/images/banner-main.jpg" alt="">
                </a>
                <div class="content-banner">
                    <div class="search-banner">
                        <select id="brand-source">
                            <option value="taobao">Taobao.com</option>
                            <option value="tmall">Tmall.com</option>
                            <option value="1688">1688.com</option>
                        </select>
                        <asp:TextBox type="text" runat="server" ID="txtSearch" placeholder="Nhập tên sản phẩm tìm kiếm"></asp:TextBox>
                        <a href="javascript:" onclick="searchProduct()" class="btn-search-banner">Tìm kiếm</a>
                        <asp:Button ID="btnSearch" runat="server"
                            OnClick="btnSearch_Click" Style="display: none"
                            OnClientClick="document.forms[0].target = '_blank';" UseSubmitBehavior="false" />
                    </div>
                    <div class="ext-install">
                        <div class="title">
                            <p>Công cụ đặt hàng Taobao, 1688 trên Google Chorme & Cốc Cốc tiện lợi và nhanh chóng.</p>
                        </div>
                        <div class="all-ext">
                            <a href="" class="ext">
                                <div class="all">
                                    <div class="icon">
                                        <i class="fa fa-download"></i>
                                    </div>
                                    <div class="text">
                                        <p>Tải mẫu đơn hàng </p>
                                    </div>
                                </div>
                            </a>
                            <a href="" class="ext bg-fun">
                                <div class="all">
                                    <div class="icon">
                                        <img src="/App_Themes/CSSHQT/images/gg-img.png" alt="">
                                    </div>
                                    <div class="text">
                                        <p>Cài đặt vào Chrome </p>
                                    </div>
                                </div>
                            </a>
                            <a href="" class="ext bg-main">
                                <div class="all">
                                    <div class="icon">
                                        <img src="/App_Themes/CSSHQT/images/cc-img.png" alt="">
                                    </div>
                                    <div class="text">
                                        <p>Cài đặt vào Cốc Cốc </p>
                                    </div>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
                <div class="mask-color"></div>
            </div>
        </div>
        <div class="wrapper-services ">
            <div class="all-services">
                <div class="left-sv pd-page">
                    <div class="icon-truck">
                        <p><i class="fa fa-truck" aria-hidden="true"></i></p>
                    </div>
                    <div class="title-wrapper">
                        <h2>Về dịch vụ </h2>
                    </div>
                    <div class="desc-wrapper">
                        <p><span>Hàng quốc tế </span>là giải pháp nhập hàng tối ưu cho quý khách. Chúng tôi mang lại cho khách hàng nguồn hàng phong phú với mức giá cực rẻ. </p>
                    </div>
                </div>
                <div class="right-sv">
                    <div class="row">
                        <asp:Literal runat="server" ID="ltrServices"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapper-commit pd-page">
            <div class="container">
                <div class="title-wrapper">
                    <h2>HQT Logistics cam kết </h2>
                </div>
                <div class="all-commit">
                    <div class="row">
                        <asp:Literal runat="server" ID="ltrCamKet"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapper-timeline pd-page">
            <div class="container">
                <div class="title-wrapper">
                    <h2>Quy trình với đơn hàng </h2>
                </div>
                <div class="desc-wrapper">
                    <p>
                        Mọi quy trình với đơn hàng đều hiển thị trên hệ thống website.
                        <br>
                        Khách hàng có thể xem tình trạng xử lý đơn hàng, các khoản phí rõ ràng minh bạch.
                    </p>
                </div>
                <div class="all-timeline">
                    <div class="row">
                        <asp:Literal runat="server" ID="ltrStep"></asp:Literal>
                    </div>
                    <div class="air-absolute">
                        <img src="/App_Themes/CSSHQT/images/quytrinh-donhang.png" alt="">
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapper-customer pd-page">
            <div class="container">
                <div class="title-wrapper">
                    <h2>Khách hàng của chúng tôi </h2>
                </div>
                <div class="all-review">
                    <div class="swiper mySwiper">
                        <div class="swiper-wrapper">
                            <asp:Literal runat="server" ID="ltrCustomers"></asp:Literal>
                        </div>
                        <div class="swiper-button-next"></div>
                        <div class="swiper-button-prev"></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="wrapper-news pd-page">
            <div class="container">
                <div class="title-wrapper">
                    <h2>Tin tức - Sự kiện </h2>
                </div>
                <div class="all-news">
                    <div class="row">
                        <asp:Literal runat="server" ID="ltrNews"></asp:Literal>
                    </div>
                </div>
                <div class="btn-main-page">
                    <a href="" class="click-btn-main">Xem tất cả </a>
                </div>
            </div>
        </div>
    </main>
    <asp:HiddenField ID="hdfWebsearch" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('.txtSearch').on("keypress", function (e) {
                if (e.keyCode == 13) {
                    searchProduct();
                }
            });
        });
        function keyclose_ms(e) {
            if (e.keyCode == 27) {
                close_popup_ms();
            }
        }
        function searchProduct() {
            var web = $("#brand-source").val();
            $("#<%=hdfWebsearch.ClientID%>").val(web);
            $("#<%=btnSearch.ClientID%>").click();
        }
    </script>
    <script type="text/javascript">        
        function close_popup_ms() {
            $("#pupip_home").animate({ "opacity": 0 }, 400);
            $("#bg_popup_home").animate({ "opacity": 0 }, 400);
            setTimeout(function () {
                $("#pupip_home").remove();
                $(".zoomContainer").remove();
                $("#bg_popup_home").remove();
                $('body').css('overflow', 'auto').attr('onkeydown', '');
            }, 500);
        }
        function closeandnotshow() {
            $.ajax({
                type: "POST",
                url: "/Default.aspx/setNotshow",
                //data: "{ID:'" + id + "',UserName:'" + username + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    close_popup_ms();
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi');
                }
            });
        }
        $(document).ready(function () {
            $.ajax({
                type: "POST",
                url: "/Default.aspx/getPopup",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d != "null") {
                        var data = JSON.parse(msg.d);
                        var title = data.NotiTitle;
                        var content = data.NotiContent;
                        var email = data.NotiEmail;
                        var obj = $('form');
                        $(obj).css('overflow', 'hidden');
                        $(obj).attr('onkeydown', 'keyclose_ms(event)');
                        var bg = "<div id='bg_popup_home'></div>";
                        var fr = "<div id='pupip_home' class=\"columns-container1\">" +
                            "  <div class=\"center_column col-xs-12 col-sm-5\" id=\"popup_content_home\">";
                        fr += "<div class=\"popup_header\">";
                        fr += title;
                        fr += "<a style='cursor:pointer;right:5px;' onclick='close_popup_ms()' class='close_message'></a>";
                        fr += "</div>";
                        fr += "     <div class=\"changeavatar\">";
                        fr += "         <div class=\"content1\">";
                        fr += content;
                        fr += "         </div>";
                        fr += "         <div class=\"content2\">";
                        fr += "<a href=\"javascript:;\" class=\"btn btn-close-full\" onclick='closeandnotshow()'>Đóng & không hiện thông báo</a>";
                        fr += "<a href=\"javascript:;\" class=\"btn btn-close\" onclick='close_popup_ms()'>Đóng</a>";
                        fr += "         </div>";
                        fr += "     </div>";
                        fr += "<div class=\"popup_footer\">";
                        fr += "<span class=\"float-right\">" + email + "</span>";
                        fr += "</div>";
                        fr += "   </div>";
                        fr += "</div>";
                        $(bg).appendTo($(obj)).show().animate({ "opacity": 0.7 }, 800);
                        $(fr).appendTo($(obj));
                        setTimeout(function () {
                            $('#pupip').show().animate({ "opacity": 1, "top": 20 + "%" }, 200);
                            $("#bg_popup").attr("onclick", "close_popup_ms()");
                        }, 1000);
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi');
                }
            });
        });
    </script>
    <style>
        #bg_popup_home {
            position: fixed;
            width: 100%;
            height: 100%;
            background-color: #333;
            opacity: 0.7;
            filter: alpha(opacity=70);
            left: 0px;
            top: 0px;
            z-index: 999999999;
            opacity: 0;
            filter: alpha(opacity=0);
        }

        #popup_ms_home {
            background: #fff;
            border-radius: 0px;
            box-shadow: 0px 2px 10px #fff;
            float: left;
            position: fixed;
            width: 735px;
            z-index: 10000;
            left: 50%;
            margin-left: -370px;
            top: 200px;
            opacity: 0;
            filter: alpha(opacity=0);
            height: 360px;
        }

            #popup_ms_home .popup_body {
                border-radius: 0px;
                float: left;
                position: relative;
                width: 735px;
            }

            #popup_ms_home .content {
                /*background-color: #487175;     border-radius: 10px;*/
                margin: 12px;
                padding: 15px;
                float: left;
            }

            #popup_ms_home .title_popup {
                /*background: url("../images/img_giaoduc1.png") no-repeat scroll -200px 0 rgba(0, 0, 0, 0);*/
                color: #ffffff;
                font-family: Arial;
                font-size: 24px;
                font-weight: bold;
                height: 35px;
                margin-left: 0;
                margin-top: -5px;
                padding-left: 40px;
                padding-top: 0;
                text-align: center;
            }

            #popup_ms_home .text_popup {
                color: #fff;
                font-size: 14px;
                margin-top: 20px;
                margin-bottom: 20px;
                line-height: 20px;
            }

                #popup_ms_home .text_popup a.quen_mk, #popup_ms_home .text_popup a.dangky {
                    color: #FFFFFF;
                    display: block;
                    float: left;
                    font-style: italic;
                    list-style: -moz-hangul outside none;
                    margin-bottom: 5px;
                    margin-left: 110px;
                    -webkit-transition-duration: 0.3s;
                    -moz-transition-duration: 0.3s;
                    transition-duration: 0.3s;
                }

                    #popup_ms_home .text_popup a.quen_mk:hover, #popup_ms_home .text_popup a.dangky:hover {
                        color: #8cd8fd;
                    }

            #popup_ms_home .close_popup {
                background: url("/App_Themes/Camthach/images/close_button.png") no-repeat scroll 0 0 rgba(0, 0, 0, 0);
                display: block;
                height: 28px;
                position: absolute;
                right: 0px;
                top: 5px;
                width: 26px;
                cursor: pointer;
                z-index: 10;
            }

        #popup_content_home {
            height: auto;
            position: fixed;
            background-color: #fff;
            top: 17%;
            z-index: 999999999;
            left: 25%;
            border-radius: 10px;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            width: 50%;
            padding: 20px;
        }

        #popup_content_home {
            padding: 0;
        }

        .popup_header, .popup_footer {
            float: left;
            width: 100%;
            background: #34a8ec;
            padding: 10px;
            position: relative;
            color: #fff;
        }

        .popup_header {
            font-weight: bold;
            font-size: 16px;
            text-transform: uppercase;
        }

        .close_message {
            top: 10px;
        }

        .changeavatar {
            padding: 10px;
            margin: 5px 0;
            float: left;
            width: 100%;
        }

        .float-right {
            float: right;
        }

        .content1 {
            float: left;
            width: 100%;
        }

        .content2 {
            float: left;
            width: 100%;
            border-top: 1px solid #eee;
            clear: both;
            margin-top: 10px;
        }

        .btn.btn-close {
            float: right;
            background: #d00f0f;
            color: #fff;
            margin-top: 20px;
            text-transform: none;
            padding: 10px 20px;
            width: unset;
        }

            .btn.btn-close:hover {
                background: #5f0d0d;
            }

        .btn.btn-close-full {
            float: right;
            background: #7bb1c7;
            color: #fff;
            margin: 20px 5px;
            text-transform: none;
            padding: 10px 20px;
            width: unset;
        }

            .btn.btn-close-full:hover {
                background: #435156;
            }
    </style>
</asp:Content>
