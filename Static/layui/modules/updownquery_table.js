/**

 @Name：升降法查询表格
 @Author：Yant
    
 */


layui.define(['table', 'form'], function(exports){
  var $ = layui.$
  ,table = layui.table
  ,form = layui.form;

  //升降查询表格
  table.render({
    elem: '#updownquery_list'
    ,url: '../UpDownMethod/GetAllExperiments' //模拟接口
      , height: 'full-130' //高度最大化减去差值
      , page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
          layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
          , curr: 1 //设定初始在第 5 页
          , groups: 3 //只显示 1 个连续页码
          , first: '首页' //不显示首页
          , last: '尾页' //不显示尾页

      }
      , limit: 20
    ,cols: [[
        { field: 'number', title: '编号', sort: true, minWidth: 80}
        , { field: 'InitialStimulus',title: '初始刺激量'}
        , { field: 'StepLength',title: '步长'}
        , { field: 'PublishStatusMethods', title: '分布状态/方法', minWidth: 160}
        , { field: 'Groping', title: '分组', templet: '#fz_state'}
        , { field: 'pow', title: '幂', minWidth: 10 }
        , { field: 'GroupNumber', title: '组数'}
        , { field: 'TotalNumberSaples', title: '总样本量'}
        , { field: 'FilpResponse', title: '翻转响应', templet: '#fzxy_state', align: 'center', minWidth: 80}
        , { field: 'DateTime', title: '日期'} 
        ,{title: '操作',align:'center', fixed: 'right', toolbar: '#table-langleyquery-tool'}
    ]]
    ,text: '对不起，加载出现异常！'
  });

  //监听工具条
  table.on('tool(updownquery_list)', function(obj){
    var data = obj.data;
      if (obj.event === 'delete') {  //删除
        var massage = '确认删除编号'+data.cpmc+'么？'
        layer.confirm(massage, {
            btn: ['确认', '取消'] //按钮
        }, function () {
            $.ajax({
                url: "../UpDownMethod/GetAllExperiments",
                type: "post",
                data: {
                    "ude_id": data.id
                },
                dataType: "json",
                success: function (data) {
                    if (data) {
                        layer.msg('删除成功', {
                            icon: 1,
                            time: 1000 //1秒关闭（如果不配置，默认是3秒）
                        });
                        table.reload('updownquery_list'); //数据刷新
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
      else if (obj.event === 'edit') {     //编辑
          top.layui.index.openTabsPage('/UpDownMethod/UpDownMethod?udg_id=' + data.id, '编辑' + data.projectname + '升降法实验')
      }
    });

  exports('updownquery_table', {})
});