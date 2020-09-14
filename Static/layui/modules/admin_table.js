/**

 @Name：管理员页面
 @Author：Yant
    
 */


layui.define(['table', 'form'], function(exports){
  var $ = layui.$
  ,table = layui.table
  ,form = layui.form;

  //管理员表
  table.render({
    elem: '#admin_list'
    ,url: '../Admin/GetAllAdmins' //模拟接口
    , height: 'full-130' //高度最大化减去差值
    ,cols: [[
       { field: 'number',title: '序号', sort: true}
      ,{field: 'name',title: '名称', minWidth: 100}
      ,{field: 'sex',title: '性别'}
      ,{field: 'phone',title: '手机'}
      ,{ field: 'email', minWidth: 195, title: '邮箱' }
      ,{ field: 'rolename', title: '角色' }
      ,{field: 'state', title:'状态',templet: '#button_state', align: 'center'}
      ,{fixed: '操作', minWidth:370 ,align:'center', fixed: 'right', toolbar: '#table-useradmin-webuser'}
    ]]
    ,page: true
    ,limit: 30
    ,text: '对不起，加载出现异常！'
  });
  
  //监听工具条
  table.on('tool(admin_list)', function(obj){
    var data = obj.data;
      if (obj.event === 'delete') {  //删除
        var massage = '确认删除管理员'+data.name+'么？'
        layer.confirm(massage, {
            btn: ['确认', '取消'] //按钮
        }, function () {
            $.ajax({
                url: "../Admin/Admin_delete",
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
                        table.reload('admin_list'); //数据刷新
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
      } else if (obj.event === 'detail') {  //查看详情
          layer.open({
              type: 2
              , title: '查看管理员'
              , content: '../Admin/AdminDetailForm'
              , area: ['500px', '450px']
              , btn: ['取消']
              , yes: function (index, layero) {
                  layer.close(index); //关闭弹层
              }
              , success: function (layero, index) {
                  var body = layer.getChildFrame('body', index);  // body.html() body里面的内容 
                  body.find("#name").val(data.name); 
                  body.find("#sex").val(data.sex);
                  body.find("#role").val(data.role);
                  body.find("#phone").val(data.phone);
                  body.find("#email").val(data.email);
                  body.find("#state").val(data.state);
                  form.render();
              }
          });
      } else if (obj.event === 'reset') {    //重置密码
          var massage = '确认重置管理员' + data.name + '的密码为admin么？'
          layer.confirm(massage, {
              btn: ['确认', '取消'] //按钮
          }, function () {
              $.ajax({
                  url: "../Admin/Admin_resetPassword",
                  type: "post",
                  data: {
                      "id": data.id
                  },
                  dataType: "json",
                  success: function (data) {
                      if (data) {
                          layer.msg('重置成功', {
                              icon: 1,
                              time: 1000 //1秒关闭（如果不配置，默认是3秒）
                          });
                          table.reload('admin_list'); //数据刷新
                      } else {
                          layer.msg('重置失败', {
                              icon: 2,
                              shift: -1,  //震动效果
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
          ,title: '编辑管理员'
          ,content: '../Admin/AdminEditorForm'
          ,area: ['500px', '450px']
          ,btn: ['确定', '取消']
          ,yes: function(index, layero){
            var iframeWindow = window['layui-layer-iframe'+ index]
            ,submitID = 'LAY-user-front-submit'
            ,submit = layero.find('iframe').contents().find('#'+ submitID);

            //监听提交
            iframeWindow.layui.form.on('submit('+ submitID +')', function(data){
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
          ,success: function(layero, index){
              var body = layer.getChildFrame('body', index);  // body.html() body里面的内容 
              body.find("#id").val(data.id);  
              body.find("#name").val(data.name);  
              if (data.sex == '男') {
                  body.find("#man").prop("checked", true)
              } else {
                  body.find("#woman").prop("checked", true)
              }

              layero.find('iframe').contents().find('[name="role"]').val(data.role);
             
              body.find("#phone").val(data.phone); 
              body.find("#email").val(data.email); 

              if (data.state == "启用") {
                  body.find("#state").attr("checked", "checked");
              } else {
                  body.find("#state").removeAttr("checked");
              }

              if (data.id == '1') {
                  body.find("#role").attr('disabled', 'disabled');
                  body.find("#state").attr('disabled', 'disabled');
              }
              form.render();
          }
        });
      }
  });

  exports('admin_table', {})
});