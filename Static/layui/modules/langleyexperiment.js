/**

 @Name：兰利法计算页面
 @Author：Yant
    
 */


layui.define(['table', 'form', 'index', 'layer'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form
        , jquery = layui.jquery
        , layer = layui.layer;

    var index = layer.open({
        type: 2
        , title: "兰利法参数设置" //不显示标题栏
        , area: ['70%', '85%']
        , id: 'LAY_layuipro' //设定一个id，防止重复弹出
        , content: ['../Langley/LanglieParameterSettings', 'no']
        , btnAlign: 'c'
        , btn: '确定'
        , anim: 1
        , closeBtn: 0
        , yes: function (index, layero) {
            var iframeWindow = window['layui-layer-iframe' + index]
                , submitID = 'submit_Btn'
                , submit = layero.find('iframe').contents().find("#" + submitID);

            //监听提交
            iframeWindow.layui.form.on('submit(' + submitID + ')', function (data) {
                var field = data.field; //获取提交的字段
                var role = JSON.stringify(field);
                //提交 Ajax 成功后，静态更新表格中的数据
                $.ajax({
                    type: "post",
                    url: '../Langley/LanglieParameterSettingsJson',
                    datatype: "json",
                    contenttype: "application/json; charset=utf-8",
                    data: role,
                    async: false,
                    success: function (data) {
                        if (data) {
                            layer.msg('设置成功', {
                                icon: 1,
                                time: 1000 //1秒关闭（如果不配置，默认是3秒）
                            });
                            layer.close(index);
                            //关闭弹层
                        }
                    }
                });
            });

            submit.trigger('click');
        },
        end: function () {
            //兰利法表格
            table.render({
                elem: '#langley_listExperiment'
                , url: '../Langley/GetAllLangleys' //模拟接口
                , height: 'full-130'//高度最大化减去差值
                //, toolbar: 'default' 
                // , toolbar: '#toolbarDemo' 
                , cols: [[
                    { field: 'ldt_Number', width: 80, title: '编号', sort: true }
                    , { field: 'ldt_StimulusQuantity', title: '刺激量' }
                    , { field: 'ldt_Response', title: '响应', align: 'center' }
                    , { field: 'ldt_Mean', title: '均值' }
                    , { field: 'ldt_StandardDeviation', title: '标准差' }
                    , { field: 'ldt_MeanVariance', title: '均值的方差' }
                    , { field: 'ldt_StandardDeviationVariance', title: '标准值的方差' }
                    , { field: 'ldt_Covmusigma', title: 'Covmusigma' }
                    , { title: '操作', align: 'center', fixed: 'right', toolbar:'#response_state'}
                ]]
                , page: true
                , limit: 30
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
                                    table.reload('langley_listExperiment'); //数据刷新
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
                                    table.reload('langley_listExperiment'); //数据刷新
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
        }
    });

    exports('langleyexperiment', {})
});