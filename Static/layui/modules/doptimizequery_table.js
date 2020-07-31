
layui.define(['table', 'form'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form;

    //管理员表
    table.render({
        elem: '#doptimize_list'
        , url: '../DoptimizeQuery/GetAllDoptimizeExperiment' //模拟接口
        //, data: { 'page': Id }
        , height: 'full-130' //高度最大化减去差值
        , page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
            layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
            , curr: 1 //设定初始在第 5 页
            , groups: 3 //只显示 1 个连续页码
            , first: '首页' //不显示首页
            , last: '尾页'//不显示尾页
            , theme: '#c71585'

        }
        , limit: 20
        , cols: [[
            { field: 'number', title: '编号', sort: true }
            , { field: 'PrecisionInstruments', title: '仪器精度', minWidth: 100 }
            , { field: 'StimulusQuantityFloor', title: '刺激量下限' }
            , { field: 'StimulusQuantityCeiling', title: '刺激量上限' }
            , { field: 'StandardDeviationEstimate', title: '标准差预估值' }
            , { field: 'Power', title: '幂' }
            , { field: 'DistributionState', title: '分布状态' }
            , { field: 'count', title: '次数' }
            , { field: 'FlipTheResponse', title: '反转响应', templet: '#reverse_state', minWidth: 80, align: 'center' }
            , { field: 'ExperimentalDate', title: '日期' }
            , { title: '操作', align: 'center', fixed: 'right', toolbar: '#table-doptimizequery-tool' }
        ]]
        , text: '对不起，加载出现异常！'
    });

    //监听工具条
    table.on('tool(doptimizelist)', function (obj) {
        var data = obj.data;
        if (obj.event === 'delete') {  //删除
            var massage = '确认删除编号' + data.number + '么？'
            layer.confirm(massage, {
                btn: ['确认', '取消'] //按钮
            }, function () {
                $.ajax({
                    url: "../DoptimizeQuery/Doptimize_delete",
                    type: "post",
                    data: {
                        "dop_id": data.Id
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data) {
                            layer.msg('删除成功', {
                                icon: 1,
                                time: 1000 //1秒关闭（如果不配置，默认是3秒）
                            });
                            table.reload('doptimize_list'); //数据刷新
                        } else {
                            layer.msg('删除失败', {
                                icon: 2,
                                shift: 6,  //震动效果
                                time: 1000  //1秒关闭
                            });
                        }
                    }
                });
            });
        } else if (obj.event === 'edit') {     //编辑
            top.layui.index.openTabsPage('/DoptimizeExperiment/DoptimizeExperiment?dop_id=' + data.Id, '编辑' + data.projectname+ 'D优化法实验');
        }
    });
    exports('doptimizequery_table', {})
});