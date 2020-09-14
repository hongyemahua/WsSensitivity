/**

 @Name：角色管理
 @Author：Yant
    
 */


layui.define(['table', 'form'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form;

    //角色管理
    table.render({
        elem: '#role_list'
        , url: '../Role/GetAllRoles' //模拟接口
        , height: 'full-130' //高度最大化减去差值
        , cols: [[
            { field: 'number', width: 80, title: '序号', sort: true }
            , { field: 'rolename', title: '角色名' }
            , { field: 'limit', title: '拥有权限', templet: '#button_limit', minWidth: 400, align: 'center' }
            , { field: 'descr', title: '具体描述' }
            , { title: '操作', width: 150, align: 'center', fixed: 'right', toolbar: '#role_tool' }
        ]]
        , page: true
        , limit: 30
        , text: '对不起，加载出现异常！'
    });

    //监听工具条
    table.on('tool(role_list)', function (obj) {
        var data = obj.data;
        if (obj.event === 'delete') {
            var massage = '确认删除角色' + data.rolename + '么？'
            layer.confirm(massage, {
                btn: ['确认', '取消'] //按钮
            }, function () {
                $.ajax({
                    url: "../Role/Role_delete",
                    type: "post",
                    data: {
                        "id": data.id
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data) {
                            layer.msg('删除成功', {
                                icon: 1,
                                time: 1000 //1秒关闭（如果不配置，默认是3秒）
                            });
                            table.reload('role_list'); //数据刷新
                        } else {
                            layer.msg('删除失败,该角色正在被占用', {
                                icon: 2,
                                shift: 6  //震动效果
                            });
                        }
                    }
                });
            });
        } else if (obj.event === 'edit') {
            var tr = $(obj.tr);

            layer.open({
                type: 2
                , title: '编辑角色'
                , content: '../Role/RoleEditorForm'
                , area: ['500px', '480px']
                , btn: ['确定', '取消']
                , yes: function (index, layero) {
                    var iframeWindow = window['layui-layer-iframe' + index]
                        , submit = layero.find('iframe').contents().find("#role_editor_submit");

                    //监听提交
                    iframeWindow.layui.form.on('submit(role_editor_submit)', function (data) {
                        var field = data.field; //获取提交的字段
                        var role = JSON.stringify(field);
                        //提交 Ajax 成功后，静态更新表格中的数据
                        $.ajax({
                            type: "post",
                            url: '../Role/Role_editor',
                            datatype: "json",
                            contenttype: "application/json; charset=utf-8",
                            data: role,
                            async: false, 
                            success: function (data) {
                                if (data) {
                                    layer.msg('修改成功', {
                                        icon: 1,
                                        time: 1000 //1秒关闭（如果不配置，默认是3秒）
                                    });
                                    table.reload('role_list'); //数据刷新
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
                , success: function (layero, index) {
                    var body = layer.getChildFrame('body', index);  // body.html() body里面的内容 
                    body.find("#id").val(data.id);
                    body.find("#rolename").val(data.rolename);
                    var limit = data.limit.split('-'); 
                    if (limit[0] == 'close') {
                        body.find("#admin").removeAttr("checked");
                    } else {
                        body.find("#admin").attr("checked", "checked");
                    }
                    if (limit[1] == 'close') {
                        body.find("#langley").removeAttr("checked");
                    } else {
                        body.find("#langley").attr("checked", "checked");
                    }
                    if (limit[2] == 'close') {
                        body.find("#liftingMethod").removeAttr("checked");
                    } else {
                        body.find("#liftingMethod").attr("checked", "checked");
                    }
                    if (limit[3] == 'close') {
                        body.find("#Doptimize").removeAttr("checked");
                    } else {
                        body.find("#Doptimize").attr("checked", "checked");
                    }
                    if (limit[4] == 'close') {
                        body.find("#mysetting").removeAttr("checked");
                    } else {
                        body.find("#mysetting").attr("checked", "checked");
                    }
                    if (limit[5] == 'close') {
                        body.find("#about").removeAttr("checked");
                    } else {
                        body.find("#about").attr("checked", "checked");
                    }
                    body.find("#descr").val(data.descr);

                    if (data.id == '1') {
                        body.find("#admin").attr('disabled', 'disabled');
                        body.find("#langley").attr('disabled', 'disabled');
                        body.find("#liftingMethod").attr('disabled', 'disabled');
                        body.find("#Doptimize").attr('disabled', 'disabled');
                        body.find("#mysetting").attr('disabled', 'disabled');
                        body.find("#about").attr('disabled', 'disabled');
                    }

                    form.render();
                }
            })
        }
    });

    exports('role_table', {})
});