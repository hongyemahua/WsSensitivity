/**

 @Name：layuiAdmin 主页示例
 @Author：star1029
 @Site：http://www.layui.com/admin/
 @License：GPL-2
    
 */

layui.define(function(exports){
    var admin = layui.admin;


    //区块轮播切换
    layui.use(['admin', 'carousel'], function () {
        var $ = layui.$
            , admin = layui.admin
            , carousel = layui.carousel
            , element = layui.element
            , device = layui.device();

        //轮播切换
        $('.layadmin-carousel').each(function () {
            var othis = $(this);
            carousel.render({
                elem: this
                , width: '100%'
                , arrow: 'none'
                , interval: othis.data('interval')
                , autoplay: othis.data('autoplay') === true
                , trigger: (device.ios || device.android) ? 'click' : 'hover'
                , anim: othis.data('anim')
            });
        });

        element.render('progress');

    });

    //扇形图
    layui.use(['carousel', 'echarts'], function () {
        var $ = layui.$
            , carousel = layui.carousel
            , echarts = layui.echarts;

        var echartsApp = [], options = [
            {
                title: {
                    subtext: '完全实况球员数据',
                    textStyle: {
                        fontSize: 14
                    }
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    type: 'scroll',
                    orient: 'vertical',
                    right: 10,
                    top: 40,
                    bottom: 20,
                    data: ['rose1', 'rose2', 'rose3', 'rose4', 'rose5', 'rose6', 'rose7', 'rose8']
                },
                toolbox: {
                    show: true,
                    feature: {
                        dataView: { show: true, readOnly: false },
                        magicType: {
                            show: true,
                            type: ['pie', 'funnel']
                        },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                calculable: true,
                series: [
                    {
                        name: '半径模式',
                        type: 'pie',
                        radius: [20, 110],
                        center: ['50%', '50%'],
                        roseType: 'radius',
                        label: {
                            normal: {
                                show: false
                            },
                            emphasis: {
                                show: true
                            }
                        },
                        lableLine: {
                            normal: {
                                show: false
                            },
                            emphasis: {
                                show: true
                            }
                        },
                        data: [
                            { value: 10, name: 'rose1' },
                            { value: 5, name: 'rose2' },
                            { value: 15, name: 'rose3' },
                            { value: 25, name: 'rose4' },
                            { value: 20, name: 'rose5' },
                            { value: 35, name: 'rose6' },
                            { value: 30, name: 'rose7' },
                            { value: 40, name: 'rose8' }
                        ]
                    },

                ]
            }
        ]
        , elemDataView = $('#pie_chart').children('div')
        , renderDataView = function (index) {
            echartsApp[index] = echarts.init(elemDataView[index], layui.echartsTheme);
            echartsApp[index].setOption(options[index]);
            window.addEventListener("resize", () => {
                echartsApp[index].resize()
            });
        };
        //没找到DOM，终止执行
        if (!elemDataView[0]) return;
        renderDataView(0);
    });

    //雷达图
    layui.use(['carousel', 'echarts'], function () {
        var $ = layui.$
            , carousel = layui.carousel
            , echarts = layui.echarts;

        var echartsApp = [], options = [
            {
                title: {
                    subtext: '完全实况球员数据',
                    textStyle: {
                        fontSize: 14
                    }
                },
                tooltip: {
                    trigger: 'item',
                    axisPointer: {
                        type: 'cross'
                    },
                    padding: 10,
                    position: function (pos, params, el, elRect, size) {
                        var obj = { top: 10 };
                        obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 30;
                        return obj;
                    }
                    // extraCssText: 'width: 170px'
                },
                legend: {
                    type: 'scroll',
                    orient: 'vertical',
                    right: 10,
                    top: 40,
                    bottom: 20,
                    data: ['罗纳尔多', '舍普琴科']
                },
                toolbox: {
                    show: true,
                    feature: {
                        dataView: { show: true, readOnly: false },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                polar: [
                    {
                        indicator: (function () {
                            var res = [];
                            for (var i = 1; i <= 12; i++) {
                                res.push({ text: i + '月', max: 100 });
                            }
                            return res;
                        })(),
                        center: ['50%', '50%'],
                        radius: 130
                    }
                ],
                series: [
                    {
                        type: 'radar',
                        center: ['50%', '50%'],
                        itemStyle: {
                            normal: {
                                areaStyle: {
                                    type: 'default'
                                }
                            }
                        },
                        data: [
                            { value: [97, 42, 88, 94, 90, 86], name: '舍普琴科' },
                            { value: [97, 32, 74, 95, 88, 92], name: '罗纳尔多' }
                        ]
                    }
                ]
            }
        ]
        , elemDataView = $('#radar_map').children('div')
        , renderDataView = function (index) {
            echartsApp[index] = echarts.init(elemDataView[index], layui.echartsTheme);
            echartsApp[index].setOption(options[index]);
            window.addEventListener("resize", () => {
                echartsApp[index].resize()
            });
        };
        //没找到DOM，终止执行
        if (!elemDataView[0]) return;
        renderDataView(0);
    });

    //折现图
    layui.use(['carousel', 'echarts'], function () {
        var $ = layui.$
            , carousel = layui.carousel
            , echarts = layui.echarts;

        var echartsApp = [], options = [
            {
                tooltip: {
                    trigger: 'axis'
                },
                calculable: true,
                legend: {
                    data: ['访问量', '下载量', '平均访问量']
                },
                toolbox: {
                    show: true,
                    feature: {
                        dataView: { show: true, readOnly: false },
                        magicType: {
                            show: true,
                            type: ['line', 'bar']
                        },
                        restore: { show: true },
                        saveAsImage: { show: true }
                    }
                },
                xAxis: [
                    {
                        type: 'category',
                        boundaryGap: false,
                        data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
                    }
                ],
                yAxis: [
                    {
                        type: 'value',
                        name: '访问量',
                        axisLabel: {
                            formatter: '{value} 万'
                        }
                    }
                ],
                series: [
                    {
                        name: '访问量',
                        type: 'line',
                        data: [900, 850, 950, 1000, 1100, 1050, 1000, 1150, 1250, 1370, 1250, 1100]
                    },
                    {
                        name: '平均访问量',
                        type: 'line',
                        data: [870, 850, 850, 950, 1050, 1000, 980, 1150, 1000, 1300, 1150, 1000]
                    }
                ]
            }
        ]
        , elemDataView = $('#discount').children('div')
        , renderDataView = function (index) {
            echartsApp[index] = echarts.init(elemDataView[index], layui.echartsTheme);
            echartsApp[index].setOption(options[index]);
            //window.onresize = echartsApp[index].resize;
            window.addEventListener("resize", () => {
                echartsApp[index].resize()
            });
        };
        //没找到DOM，终止执行
        if (!elemDataView[0]) return;
        renderDataView(0);
    });

    //项目进展
    layui.use('table', function () {
        var $ = layui.$
            , table = layui.table;

        table.render({
            elem: '#LAY-index-prograss'
            , url: layui.setter.base + 'json/console/prograss.js' //模拟接口
            , height: 312
            , cols: [[
                { field: 'prograss', title: '任务' }
                , { field: 'time', title: '所需时间' }
                , {
                    field: 'complete', title: '完成情况'
                    , templet: function (d) {
                        if (d.complete == '已完成') {
                            return '<del style="color: #5FB878;">' + d.complete + '</del>'
                        } else if (d.complete == '进行中') {
                            return '<span style="color: #FFB800;">' + d.complete + '</span>'
                        } else {
                            return '<span style="color: #FF5722;">' + d.complete + '</span>'
                        }
                    }
                }
            ]]
            , skin: 'line'
        });
    });

    //回复留言
    admin.events.replyNote = function (othis) {
        var nid = othis.data('id');
        layer.prompt({
            title: '回复留言 ID:' + nid
            , formType: 2
        }, function (value, index) {
            //这里可以请求 Ajax
            //…
            layer.msg('得到：' + value);
            layer.close(index);
        });
    };

    exports('sample', {})
});