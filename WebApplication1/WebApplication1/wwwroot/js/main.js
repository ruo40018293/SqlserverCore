
$(function () {
    $('.cbt').click(function () {
        $(this).toggleClass('cb_active');
        if ($(this).attr('val') == 16 && $(this).hasClass('cb_active')) {
            $('#Other').removeAttr("disabled");
        } else if ($(this).attr('val') == 16 && !$(this).hasClass('cb_active')) {
            $('#Other').attr("disabled", "disabled").val("");
        }
    });
    $('.rb').click(function () {
        $(this).toggleClass('rb_active').siblings().removeClass('rb_active');
        if ($(this).attr('val') == 5 && $(this).hasClass('rb_active')) {
            $('#otherShape').removeAttr("disabled");
        } else {
            $('#otherShape').attr("disabled", "disabled");
        }
    });
    var windowHeight = $(window).height() + 64;
    var bodyHeight = $('body').height();
    if (bodyHeight < windowHeight) {
        $('.footer').addClass('gudingfooter');
    }
});

//倒计时
function resetCode() {
    $('.yzm_button').hide();
    $('.yzm_button1').children('span').html('60');
    $('.yzm_button1').show();
    var second = 60;
    var timer = null;
    timer = setInterval(function () {
        second -= 1;
        if (second > 0) {
            $('.yzm_button1').children('span').html(second);
        } else {
            clearInterval(timer);
            $('.yzm_button').show();
            $('.yzm_button1').hide();
        }
    }, 1000);
}

//Common
function showError(Msg) {
    if (Msg == "ToLogin") {
        layer.msg("登录超时，请重新登陆！", { icon: 2, time: 2000 });
        setTimeout(function () {  //使用  setTimeout（）方法设定定时1000毫秒
            window.location.href = "/Login/Index";//页面刷新
        }, 2000);
    }
    else {
        layer.msg(Msg, { icon: 2, time: 2000 });
    }
}

function showMsg(Msg, IsReload) {
    layer.msg(Msg, { icon: 1, time: 2000 });
    if (IsReload === true) {
        setTimeout(function () {  //使用  setTimeout（）方法设定定时1000毫秒
            window.location.reload();//页面刷新
        }, 1000);
    }
}
function showMsgAndDoAction(Msg, Action) {
    layer.msg(Msg, { icon: 1, time: 2000 });
    if (Action != null) {
        Action();
    }
}


//获取url中的参数
function getUrlParam(name) {
    // 获取参数
    var url = window.location.search;
    // 正则筛选地址栏
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    // 匹配目标参数
    var result = url.substr(1).match(reg);
    //返回参数值
    return result ? decodeURIComponent(result[2]) : null;
}
