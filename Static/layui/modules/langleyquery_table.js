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
    ,url: '../Langley/GetAllLangleys' //模拟接口
    , height: 'full-130' //高度最大化减去差值
    ,cols: [[
      {field: 'id',title: '编号', sort: true}
      ,{field: 'percision',title: '仪器精度', minWidth: 100}
      ,{field: 'lowlimit',title: '刺激量下限'}
      ,{field: 'upperlimit',title: '刺激量上限'}
      ,{ field: 'power', title: '幂' }
      ,{ field: 'distribution', title: '分布状态' }
      ,{ field: 'correct', title: '标准差修正',templet: '#correct_state', minWidth: 80, align: 'center'}
      ,{ field: 'testN', title: '次数' }
      ,{ field: 'reverse', title: '反转响应',templet: '#reverse_state', minWidth: 80, align: 'center'}
      ,{ field: 'opdate', title: '日期' }
      ,{title: '操作',align:'center', fixed: 'right', toolbar: '#table-langleyquery-tool'}
    ]]
    ,page: true
    ,limit: 30
    ,text: '对不起，加载出现异常！'
  });
  
  //监听工具条
  table.on('tool(langley_list)', function(obj){
    var data = obj.data;
      if (obj.event === 'delete') {  //删除
        var massage = '确认删除编号'+data.id+'么？'
        layer.confirm(massage, {
            btn: ['确认', '取消'] //按钮
        }, function () {
            $.ajax({
                url: "../Langley/Langley_delete",
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
      }
  });

  exports('langleyquery_table', {})
});