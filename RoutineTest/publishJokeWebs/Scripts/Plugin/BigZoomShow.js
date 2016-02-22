(function ($, window, document) {
    var defaults = {
        //fontsize: 15,//如果设置字体大小，使用设置大小，如果未设置，放大相应倍数
        fontcolor: 'blue',//字体颜色
        zindex: 1001,//z-index
        fontfamily: '黑体',//字体
        BigRate: 1.2//放大倍数
    };

    $.fn.BigShow = function (options) {
        var opt = $.extend({}, defaults, options);
        var _this = $(this);
        $(document).delegate(_this.selector, "click", function (e) {
            var zoomdiv = '<div class="_zoomdiv" style="height:' + document.body.scrollHeight + 'px;width:' + document.body.clientWidth + 'px; position: absolute; top:0px; filter: alpha(opacity=60);background-color: #777;z-index: 100; left: 0px;opacity:0.5; -moz-opacity:0.5; "></div>';

            $(document.body).append(zoomdiv);//生成遮罩层


            var temp = $(e.target).closest(_this.selector);//获取需要放大的元素  

            var _tempwidth = temp.width();//取得放大元素的原始高度
            var _fontsize = temp.css("font-size").match(/\d+/)[0];//取得放大元素的原始字体大小
            var _temptopheight = 50;//放大框距顶部高度
            if (temp.height() * opt.BigRate + _temptopheight < $(window).height()) {
                scrollfree(); //禁用滚动条
            }

            var bigtemp = temp.prop("outerHTML");//复制出放大元素

            bigtemp = bigtemp.replace("data=", "tempdata=");//替换掉，防止事件冲突
            bigtemp = $(bigtemp);
            var _imgs = bigtemp.find("img");
            if (_imgs.length > 0) {
                _imgs.each(function () {
                    var _img = $(this);
                    _img.css("width", _tempwidth * opt.BigRate);//等比放大图片


                });
            }
            var winWidth = $(window).width();//窗体宽度
            var scrolltop = $(document).scrollTop();//滚动高度
            $(bigtemp).css({
                'position': 'absolute',
                'background-color': 'white',
                'left': (winWidth - _tempwidth * opt.BigRate) / 2,
                'top': scrolltop + _temptopheight,
                'color': opt.fontcolor,
                'font-size': opt.fontsize ? opt.fontsize : _fontsize * 1.5 + "px",
                'z-index': opt.zindex,
                'font-family': opt.fontfamily,
                'max-width': _tempwidth * opt.BigRate

            });//设置元素
            $(document.body).append(bigtemp);

            $(document).delegate("._zoomdiv", "click", function () {
                $('._zoomdiv').remove();
                $(bigtemp).remove();
                unscrollfree();//释放滚动条
            });

            function scrollfree() {
                var scrollFunc = function (e) {
                    e = e || window.event;
                    if (e && e.preventDefault) {
                        e.preventDefault();
                        e.stopPropagation();
                    } else {
                        e.returnvalue = false;

                        return false;
                    }
                }
                $(window).bind('mousewheel', function (e) {
                    scrollFunc(e);
                }).bind('DOMMouseScroll', function (e) {
                    scrollFunc(e);
                });;
            }
            function unscrollfree() {
                $(window).unbind('mousewheel').unbind('DOMMouseScroll');
            }

        });


    }
}(jQuery, window, document));