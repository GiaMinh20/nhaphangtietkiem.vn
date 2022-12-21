<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_Banner.ascx.cs" Inherits="NHST.UC.uc_Banner" %>
<div class="company-intro">
    <div class="container">
        <div class="columns">
            <div class="column wow fadeInUp" data-wow-duration="1s" data-wow-delay="0s">
                <div class="intro-item">
                    <div class="intro-img">
                        <img src="/App_Themes/kyguinhanh/images/intro-1.png" alt="" class="-img">
                    </div>
                    <p class="intro-text">Dễ dàng tạo vào quản lý đơn hàng, Tìm nguồn hàng, tư vấn miễn phí</p>
                </div>
            </div>
            <div class="column wow fadeInUp" data-wow-duration="1s" data-wow-delay=".2s">
                <div class="intro-item">
                    <div class="intro-img">
                        <img src="/App_Themes/kyguinhanh/images/intro-2.png" alt="" class="-img">
                    </div>
                    <p class="intro-text">Đảm bảo 100% hàng hoá, đền bù khi có sai sót. thất lạc</p>
                </div>
            </div>
            <div class="column wow fadeInUp" data-wow-duration="1s" data-wow-delay=".4s">
                <div class="intro-item">
                    <div class="intro-img">
                        <img src="/App_Themes/kyguinhanh/images/intro-3.png" alt="" class="-img">
                    </div>
                    <p class="intro-text">Giao hàng tận nơi, nhanh chóng dù đơn hàng chỉ có 1 sản phẩm</p>
                </div>
            </div>
        </div>
    </div>
</div>
<section class="sec main-banner">
    <h2
        class="banner-title wow slideInLeft"
        data-wow-duration="1s"
        data-wow-delay="0s">Hợp tác<br>
        vươn đến thành công</h2>
    <div
        class="setting-tool wow slideInLeft"
        data-wow-duration="1s"
        data-wow-delay=".2s">
        <p class="title">Cài đặt công cụ</p>
        <div class="tool-btn-box">
            <a
                href="https://chrome.google.com/webstore/detail/công-cụ-đặt-hàng-của-kgn/eecbdaehpmfnmhnclolompcbnnpejfan"
                target="_blank"
                class="main-btn chrome">
                <i class="icon fab fa-chrome"></i>Chrome</a>
            <a
                href="https://chrome.google.com/webstore/detail/công-cụ-đặt-hàng-của-kgn/eecbdaehpmfnmhnclolompcbnnpejfan"
                target="_blank"
                class="main-btn coccoc">
                <i class="icon fab fa-chrome"></i>Cốc cốc</a>
        </div>
    </div>
</section>
<style>
    .banner-btn-box .title {
        font-size: 15px;
        font-weight: bold;
    }

    .app {
        width: 180px;
    }

    .app-strore {
        width: 180px;
    }

    .app img {
        width: auto;
        height: 60px;
    }

    .app-strore img {
        width: auto;
        height: 60px;
    }

    @media only screen and (max-width: 600px) {
        .app img {
            width: auto;
            height: 60px;
        }

        .app-strore img {
            width: auto;
            height: 60px;
        }
    }
</style>
