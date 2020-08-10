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
    ,height: 'full-130' //高度最大化减去差值
    ,cols: [[
      {field: 'id',title: '编号', sort: true}
      ,{ field: 'cpmc', title: '产品名称'}
      ,{field: 'cscjl',title: '初始刺激量', minWidth: 100}
      ,{field: 'bcd',title: '步长'}
      ,{field: 'fb',title: '分布状态'}
      ,{ field: 'hs', title: '函数' }
      ,{ field: 'sz', title: '组数'}
      ,{ field: 'zybl', title: '总样本量'}
      ,{ field: 'fzxy', title: '翻转响应',templet: '#fzxy_state', align: 'center'}
      ,{ field: 'Date', title: '日期',templet:function (d) { return showTime(d.cjsj);}} 
      ,{title: '操作',align:'center', fixed: 'right', toolbar: '#table-langleyquery-tool'}
    ]]
    ,page: true
    ,limit: 30
    ,text: '对不起，加载出现异常！'
  });

  //时间转换函数
  function showTime(tempDate) {
      var date = new Date(parseInt(tempDate.replace("/Date(", "").replace(")/", ""), 10));
      var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
      var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
      return date.getFullYear() + "-" + month + "-" + currentDate;
  }

  //监听工具条
  table.on('tool(updownquery_list)', function(obj){
    var data = obj.data;
      if (obj.event === 'delete') {  //删除
        var massage = '确认删除编号'+data.cpmc+'么？'
        layer.confirm(massage, {
            btn: ['确认', '取消'] //按钮
        }, function () {
            $.ajax({
                url: "../UpDownMethod/Experiment_delete",
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
      //else if (obj.event === 'edit') {//编辑
      //    setTimeout(function () { top.layui.index.openTabsPage('/UpDownMethod/UpDownMethod?upd_id=' + data.id, '编辑' + data.projectname + '升降法实验'); }, 1000);
      //}
    });
    table.on('rowDouble(updownquery_list)', function (obj) {
        var data = obj.data;
        setTimeout(function () { top.layui.index.openTabsPage('/UpDownMethod/UpDownMethod?upd_id=' + data.id, '编辑' + data.projectname + '升降法实验'); }, 1000);
    });

  exports('updownquery_table', {})
});