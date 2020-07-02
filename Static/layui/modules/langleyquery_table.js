/**

 @Name：兰利法查询表格
 @Author：Yant
    
 */


layui.define(['table', 'form'], function(exports){
  var $ = layui.$
  ,table = layui.table
  ,form = layui.form;

  //管理员表
  table.render({
    elem: '#langley_list'
      , url: '../Langley/GetAllLangleysExperiment' //模拟接口
    , height: 'full-130' //高度最大化减去差值
      , cols: [[
          { field: 'number', title: '编号', sort: true }
        , { field: 'PrecisionInstruments',title: '仪器精度', minWidth: 100}
        , { field: 'StimulusQuantityFloor',title: '刺激量下限'}
        , { field: 'StimulusQuantityCeiling',title: '刺激量上限'}
        , { field: 'Power', title: '幂' }
        , { field: 'DistributionState', title: '分布状态' }
        , { field: 'Correction', title: '标准差修正',templet: '#correct_state', minWidth: 80, align: 'center'}
        ,{ field: 'count', title: '次数' }
        , { field: 'FlipTheResponse', title: '反转响应',templet: '#reverse_state', minWidth: 80, align: 'center'}
        , { field: 'ExperimentalDate', title: '日期' }
      ,{title: '操作',align:'center', fixed: 'right', toolbar: '#table-langleyquery-tool'}
    ]]
    ,page: true
    ,limit: 30
    ,text: '对不起，加载出现异常！'
  });
  
    //监听工具条
    table.on('tool(langleylist)', function (obj) {
        var data = obj.data;
        if (obj.event === 'delete') {  //删除
            var massage = '确认删除编号' + data.number + '么？'
            layer.confirm(massage, {
                btn: ['确认', '取消'] //按钮
            }, function () {
                $.ajax({
                    url: "../Langley/Langley_delete",
                    type: "post",
                    data: {
                        "id": data.Id
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data) {
                            layer.msg('删除成功', {
                                icon: 1,
                                time: 1000 //1秒关闭（如果不配置，默认是3秒）
                            });
                            table.reload('langley_list'); //数据刷新
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
            var tr = $(obj.tr);
            layer.open({
                type: 2
                , title: '编辑管理员'
                , content: '../Langley/LangleyExperiment'
                , area: ['90%', '85%']
                , btn: ['确定', '取消']
                , yes: function (index, layero) {
                    var iframeWindow = window['layui-layer-iframe' + index]
                        , submitID = 'LAY-user-front-submit'
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
                            url: '../Admin/Admin_editor',
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
                                    table.reload('admin_list'); //数据刷新
                                } else {
                                    layer.msg('修改失败', {
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
        }
    });
    exports('langleyquery_table', {})
});