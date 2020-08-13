/**

 @Name：升降法页面
 @Author：Yant
    
 */

layui.define(['table', 'form','element'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form
        , element = layui.element;

    //实验表格
    table.render({
        elem: '#updown_list'
        , url: '../UpDownMethod/GetAllUpDownMethods' //模拟接口
        , height: 'full-120' //高度最大化减去差值
        , toolbar: '#toolbarDemo'
        , cols: [[
            { type: 'checkbox', fixed: 'left' }
            , { field: 'number', title: '编号', sort: true }
            , { field: 'stimulusQuantity', title: '刺激量' }
            , { field: 'response', title: '响应' }
            , { field: 'standardStimulus', title: '刺激量(标准)' }
        ]]
        , page: true
        , limit: 10
        , text: '对不起，加载出现异常！'
    });

    //实验监听头工具栏事件
    table.on('toolbar(updown_list)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id)
            , data = checkStatus.data; //获取选中的数据
        switch (obj.event) {
            case 'add':
                layer.open({
                    type: 2
                    , title: '添加实验数据'
                    , content: '../UpDownMethod/Add'
                    , area: ['400px', '537px']
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        var iframeWindow = window['layui-layer-iframe' + index]
                            , submitID = 'testdata-submit'
                            , submit = layero.find('iframe').contents().find('#' + submitID);

                        //监听提交
                        iframeWindow.layui.form.on('submit(' + submitID + ')', function (data) {
                            var field = data.field; //获取提交的字段
                            if (field.state == 'on') {
                                field.state = '启用';
                            } else {
                                field.state = '禁用';
                            }
                            var admin = JSON.stringify(field);
                            //提交 Ajax 成功后，静态更新表格中的数据
                            $.ajax({
                                type: "post",
                                url: '../UpDownMethod/UpDownParameter_add',
                                datatype: "json",
                                contenttype: "application/json; charset=utf-8",
                                data: admin,
                                async: false,
                                success: function (data) {
                                    if (data) {
                                        layer.msg('添加成功', {
                                            icon: 1,
                                            time: 1000 //1秒关闭（如果不配置，默认是3秒）
                                        });
                                    } else {
                                        layer.msg('添加失败', {
                                            icon: 2,
                                            shift: -1,  //震动效果
                                            time: 1000  //1秒关闭
                                        });
                                    }
                                }
                            });
                            table.reload('updown_list');//数据刷新
                            layer.close(index); //关闭弹层
                        });
                        submit.trigger('click');
                    }
                });
                break;

            case 'delete':
                if (data.length === 0) {
                    layer.msg('请选择一行');
                } else {
                    $.ajax({
                        type: "post",
                        url: '../UpDownMethod/Experiment_delete?id='+data[0].id,
                        datatype: "json",
                        contenttype: "application/json; charset=utf-8",
                        async: false,
                        success: function (data) {
                            if (data) {
                                layer.msg('删除成功', {
                                    icon: 1,
                                    time: 1000 //1秒关闭（如果不配置，默认是3秒）
                                });
                                
                            } else {
                                layer.msg('删除失败', {
                                    icon: 2,
                                    shift: -1,  //震动效果
                                    time: 1000  //1秒关闭
                                });
                            }
                            table.reload('updown_list');
                        }
                    });
                }
                break;

            case 'altersetting':
                layer.open({
                    type: 2
                    , title: '升降法分析参数设置'
                    , content: '../UpDownMethod/AlterParameterSetting'
                    , area: ['600px', '230px']
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        var iframeWindow = window['layui-layer-iframe' + index]
                            , submitID = 'altersetting_submit'
                            , submit = layero.find('iframe').contents().find('#' + submitID);

                        //监听提交
                        iframeWindow.layui.form.on('submit(' + submitID + ')', function (data) {
                            var field = data.field; //获取提交的字段
                            var admin = JSON.stringify(field);
                            //提交 Ajax 成功后，静态更新表格中的数据
                            $.ajax({
                                type: "post",
                                url: '../UpDownMethod/UpdateUPparSeting',
                                datatype: "json",
                                contenttype: "application/json; charset=utf-8",
                                data: admin,
                                async: false,
                                success: function (data) {
                                    if (data) {
                                        layer.msg('修改成功', {
                                            icon: 1,
                                            time: 1000 //1秒关闭（如果不配置，默认是3秒）
                                        });
                                    } else {
                                        layer.msg('修改失败', {
                                            icon: 2,
                                            shift: -1,  //震动效果
                                            time: 1000  //1秒关闭
                                        });
                                    }
                              table.reload('updown_list');
                                }
                            });
                            layer.close(index); //关闭弹层
                        });
                        submit.trigger('click');
                    }
                });
                break;
            case 'tabChange'://计算当前组数据

                element.tabChange('updown_tab', '22');
                break;

        };
    });

    //单组表格
    table.render({
        elem: '#singleunit_list'
        , url: '#' //模拟接口
        , height: 'full-435' //高度最大化减去差值
        , toolbar: '#singleunit_tool'
        , cols: [[
            { field: 'i', title: 'i' }
            , { field: 'i2', title: 'i2' }
            , { field: 'vi', title: 'vi' }
            , { field: 'mi', title: 'mi' }
            , { field: 'ivi', title: 'ivi' }
            , { field: 'i2vi', title: 'i2vi' }
            , { field: 'imi1', title: 'imi1' }
            , { field: 'i2mi1', title: 'i2mi1' }
        ]]
        , page: true
        , limit: 10
        , text: '对不起，加载出现异常！'
    });

    //单组监听头工具栏事件
    table.on('toolbar(singleunit_list)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id)
            , data = checkStatus.data; //获取选中的数据
        switch (obj.event) {
            case 'altersetting':
                layer.open({
                    type: 2
                    , title: '升降法分析参数设置'
                    , content: '../UpDownMethod/AlterParameterSetting'
                    , area: ['600px', '230px']
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        var iframeWindow = window['layui-layer-iframe' + index]
                            , submitID = 'lntervalestimation_submit'
                            , submit = layero.find('iframe').contents().find('#' + submitID);

                        //监听提交
                        iframeWindow.layui.form.on('submit(' + submitID + ')', function (data) {
                            var field = data.field; //获取提交的字段
                            var admin = JSON.stringify(field);
                            //提交 Ajax 成功后，静态更新表格中的数据
                            $.ajax({
                                type: "post",
                                url: '@Url.Action("Add","UpDownMethod")',
                                datatype: "json",
                                contenttype: "application/json; charset=utf-8",
                                data: admin,
                                async: false,
                                success: function (data) {
                                    if (data) {
                                        layer.msg('添加成功', {
                                            icon: 1,
                                            time: 1000 //1秒关闭（如果不配置，默认是3秒）
                                        });
                                    } else {
                                        layer.msg('添加失败', {
                                            icon: 2,
                                            shift: -1,  //震动效果
                                            time: 1000  //1秒关闭
                                        });
                                    }
                                }
                            });
                            layer.close(index); //关闭弹层
                        });
                        submit.trigger('click');
                    }
                });
                break;
            case 'lntervalestimation':
                layer.open({
                    type: 2
                    , title: '单组数据区间估计'
                    , content: '../UpDownMethod/LntervalEstimation'
                    , area: ['80%', '90%']
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        var iframeWindow = window['layui-layer-iframe' + index]
                            , submitID = 'altersetting_submit'
                            , submit = layero.find('iframe').contents().find('#' + submitID);

                        //监听提交
                        iframeWindow.layui.form.on('submit(' + submitID + ')', function (data) {
                            var field = data.field; //获取提交的字段
                            var admin = JSON.stringify(field);
                            //提交 Ajax 成功后，静态更新表格中的数据
                            $.ajax({
                                type: "post",
                                url: '@Url.Action("Add","UpDownMethod")',
                                datatype: "json",
                                contenttype: "application/json; charset=utf-8",
                                data: admin,
                                async: false,
                                success: function (data) {
                                    if (data) {
                                        layer.msg('添加成功', {
                                            icon: 1,
                                            time: 1000 //1秒关闭（如果不配置，默认是3秒）
                                        });
                                    } else {
                                        layer.msg('添加失败', {
                                            icon: 2,
                                            shift: -1,  //震动效果
                                            time: 1000  //1秒关闭
                                        });
                                    }
                                }
                            });
                            layer.close(index); //关闭弹层
                        });
                        submit.trigger('click');
                    }
                });
                break;
        };
    });

    //综合表格
    table.render({
        elem: '#comprehensive_list'
        , url: '#' //模拟接口
        , height: 'full-340' //高度最大化减去差值
        , toolbar: '#comprehensive_tool'
        , cols: [[
            { field: 'zc', title: '组次' }
            , { field: 'cjl', title: '刺激量' }
            , { field: 'bc', title: '步长' }
            , { field: 'ybl', title: '样本量' }
            , { field: 'pjz', title: '平均值' }
            , { field: 'bzc', title: '标准差' }
            , { field: 'jzbzwc', title: '均值标准误差', minWidth: 150 }
            , { field: 'bzcbzwc', title: '标准差标准误差', minWidth: 150 }
            , { field: 'fxysts', title: '分析用试探数', minWidth: 150 }
            , { field: 'G', title: 'G' }
            , { field: 'H', title: 'H' }
            , { field: 'A', title: 'A' }
            , { field: 'B', title: 'B' }
            , { field: 'M', title: 'M' }
            , { field: 'b', title: 'b' }
            , { field: 'p', title: 'p' }
            , { field: '0.1%', title: '0.1%' }
            , { field: '99.9%', title: '99.9%' }
        ]]
        , page: true
        , limit: 10
        , text: '对不起，加载出现异常！'
    });

    //综合监听头工具栏事件
    table.on('toolbar(comprehensive_list)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id)
            , data = checkStatus.data; //获取选中的数据
        switch (obj.event) {
            case 'add':
                layer.msg('添加');
                break;
            case 'lntervalestimation':
                layer.open({
                    type: 2
                    , title: '多组数据区间估计'
                    , content: '../UpDownMethod/LntervalEstimation'
                    , area: ['80%', '90%']
                    , btn: ['确定', '取消']
                    , yes: function (index, layero) {
                        var iframeWindow = window['layui-layer-iframe' + index]
                            , submitID = 'lntervalestimation_submit'
                            , submit = layero.find('iframe').contents().find('#' + submitID);

                        //监听提交
                        iframeWindow.layui.form.on('submit(' + submitID + ')', function (data) {
                            var field = data.field; //获取提交的字段
                            var admin = JSON.stringify(field);
                            //提交 Ajax 成功后，静态更新表格中的数据
                            $.ajax({
                                type: "post",
                                url: '@Url.Action("Add","UpDownMethod")',
                                datatype: "json",
                                contenttype: "application/json; charset=utf-8",
                                data: admin,
                                async: false,
                                success: function (data) {
                                    if (data) {
                                        layer.msg('添加成功', {
                                            icon: 1,
                                            time: 1000 //1秒关闭（如果不配置，默认是3秒）
                                        });
                                    } else {
                                        layer.msg('添加失败', {
                                            icon: 2,
                                            shift: -1,  //震动效果
                                            time: 1000  //1秒关闭
                                        });
                                    }
                                }
                            });
                            layer.close(index); //关闭弹层
                        });
                        submit.trigger('click');
                    }
                });
                break;
        };
    });

    exports('updown_table', {})
});