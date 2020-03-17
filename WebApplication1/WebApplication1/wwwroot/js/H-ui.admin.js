/* -----------H-ui前端框架-------------
* H-ui.admin.js v2.3.1
* http://www.h-ui.net/
* Created & Modified by guojunhui
* Date modified 15:42 2015.08.19
*
* Copyright 2013-2015 北京颖杰联创科技有限公司 All rights reserved.
* Licensed under MIT license.
* http://opensource.org/licenses/MIT
*
*/
var num=0,oUl=$("#min_title_list"),hide_nav=$("#Hui-tabNav");

/*左侧菜单响应式*/
function Huiasidedisplay(){
	if($(window).width()>=768){
		$(".Hui-aside").show()
	} 
}
function getskincookie(){
	var v = getCookie("Huiskin");
	if(v==null||v==""){
		v="default";
	}
	$("#skin").attr("href","skin/"+v+"/skin.css");
}
function Hui_admin_tab(obj){
	if($(obj).attr('_href')){
		var bStop=false;
		var bStopIndex=0;
		var _href=$(obj).attr('_href');
		var _titleName=$(obj).attr("data-title");
		var topWindow=$(window.parent.document);
		$(".Hui-aside .menu_dropdown li a").removeClass("current");
		$(obj).addClass("current");
		if(!bStop){
			creatIframe(_href,_titleName);
		}
	}

}
function creatIframe(href,titleName){
	var topWindow=$(window.parent.document);
	var iframe_box=topWindow.find('#iframe_box').find("iframe");
	iframe_box.find('.loading').show();
	$(iframe_box).attr("src",href);
	iframe_box.load(function(){
		iframe_box.find('.loading').hide();
	});

}
/*弹出层*/
/*
	参数解释：
	title	标题
	url		请求的url
	id		需要操作的数据id
	w		弹出层宽度（缺省调默认值）
	h		弹出层高度（缺省调默认值）
*/
function layer_show(title,url,w,h){
	if (title == null || title == '') {
		title=false;
	};
	if (url == null || url == '') {
		url="404.html";
	};
	if (w == null || w == '') {
		w=800;
	};
	if (h == null || h == '') {
		h=($(window).height() - 50);
	};
	layer.open({
		type: 2,
		area: [w+'px', h +'px'],
		fix: false, //不固定
		maxmin: true,
		shade:0.4,
		title: title,
		content: url
	});
}
/*关闭弹出框口*/
function layer_close(){
	var index = parent.layer.getFrameIndex(window.name);
	parent.layer.close(index);
	
}
$(function(){
	getskincookie();
	//layer.config({extend: 'extend/layer.ext.js'});
	Huiasidedisplay();
	var resizeID;
	$(window).resize(function(){
		clearTimeout(resizeID);
		resizeID = setTimeout(function(){
			Huiasidedisplay();
		},500);
	});
	
	$(".Hui-nav-toggle").click(function(){
		$(".Hui-aside").slideToggle();
	});
	$(".Hui-aside").on("click",".menu_dropdown dd li a",function(){
		if($(window).width()<768){
			$(".Hui-aside").slideToggle();
		}
	});
	/*左侧菜单*/
	$.Huifold(".menu_dropdown dl dt",".menu_dropdown dl dd","fast",3,"click");
	//将其注释，用于取消左侧收缩
	/*选项卡导航*/

	$(".Hui-aside").on("click", ".menu_dropdown a", function () {
        var loadIndex = layer.load();
        Hui_admin_tab(this);
        layer.close(loadIndex);
	});
		
	/*换肤*/
	$("#Hui-skin .dropDown-menu a").click(function(){
		var v = $(this).attr("data-val");
		
		setCookie("Huiskin", v);
		$("#skin").attr("href","skin/"+v+"/skin.css");
		$(window.frames.document).contents().find("#skin").attr("href","skin/"+v+"/skin.css");
	});
}); 
