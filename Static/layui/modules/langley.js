/**

 @Name：兰利法计算页面
 @Author：Yant
    
 */


layui.define(['table', 'form'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form;

    //兰利法表格
    table.render({
        elem: '#langley_list'
        , url: '../Langley/GetAllLangleys' //模拟接口
        , height: 'full-130'//高度最大化减去差值
        //, toolbar: 'default' 
        //, toolbar: '#toolbarDemo' 
        , page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
            layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
            , curr: 1 //设定初始在第 5 页
            , groups: 3 //只显示 1 个连续页码
            , first: false //不显示首页
            , last: false //不显示尾页

        }
        , limit: 20
        , cols: [[
            { field: 'ldt_Number', width: 80, title: '编号', sort: true }
            , { field: 'ldt_StimulusQuantity', title: '刺激量' }
            , { field: 'ldt_Response', title: '响应', align: 'center' }
            , { field: 'ldt_Mean', title: '均值' }
            , { field: 'ldt_StandardDeviation', title: '标准差' }
            , { field: 'ldt_MeanVariance', title: '均值的方差' }
            , { field: 'ldt_StandardDeviationVariance', title: '标准值的方差' }
            , { field: 'ldt_Covmusigma', title: 'Covmusigma' }
            , { title: '操作', align: 'center', fixed: 'right', toolbar: '#response_state' }
        ]]
        , text: '对不起，加载出现异常！'
    });

    //监听工具条
    table.on('tool(langleylist)', function (obj) {
        var data = obj.data;
        if (obj.event === 'response') {
            var massage = '确认增加响应么？'
            layer.confirm(massage, {
                btn: ['确认', '取消'] //按钮
            }, function () {
                $.ajax({
                    url: "../Langley/InsertData",
                    type: "post",
                    data: {
                        "response": 1
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data[0]) {
                            layer.msg('增加成功', {
                                icon: 1,
                                time: 1000 //1秒关闭（如果不配置，默认是3秒）
                            });
                            table.reload('langley_list'); //数据刷新
                            document.getElementById("testNumber").value = data[1];
                            document.getElementById("NM").value = data[2];
                            document.getElementById("conversionNumber").value = "";
                        } else {
                            layer.msg('增加失败', {
                                icon: 2,
                                shift: 6,  //震动效果
                                time: 1000  //1秒关闭
                            });
                        }
                    }
                });
            });
        } else if (obj.event === 'notResponse') {
            var massage = '确认增加不响应么？'
            layer.confirm(massage, {
                btn: ['确认', '取消'] //按钮
            }, function () {
                $.ajax({
                    url: "../Langley/InsertData",
                    type: "post",
                    data: {
                        "response": 0
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data[0]) {
                            layer.msg('增加成功', {
                                icon: 1,
                                time: 1000 //1秒关闭（如果不配置，默认是3秒）
                            });
                            table.reload('langley_list'); //数据刷新
                            document.getElementById("testNumber").value = data[1];
                            document.getElementById("NM").value = data[2];
                            document.getElementById("conversionNumber").value = "";
                        } else {
                            layer.msg('增加失败', {
                                icon: 2,
                                shift: 6,  //震动效果
                                time: 1000  //1秒关闭
                            });
                        }
                    }
                });
            });
        } else if (obj.event === 'remove') {
            var massage = '确认撤销编号为' + data.ldt_Number + '么？'
            layer.confirm(massage, {
                btn: ['确认', '取消'] //按钮
            }, function () {
                $.ajax({
                    url: "../Langley/RevocationData",
                    type: "post",
                    data: {
                        "id": data.ldt_Id
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data) {
                            layer.msg('删除成功', {
                                icon: 1,
                                time: 1000 //1秒关闭（如果不配置，默认是3秒）
                            });
                            table.reload('langley_listExperiment'); //数据刷新
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
        }

    });
    exports('langley', {})
});