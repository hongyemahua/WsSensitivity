/**

 @Name：药剂数控中心 我的设置
 @Author：lalalayt
    
 */
 
layui.define(['form', 'upload'], function(exports){
  var $ = layui.$
  ,layer = layui.layer
  ,laytpl = layui.laytpl
  ,setter = layui.setter
  ,view = layui.view
  ,admin = layui.admin
  ,form = layui.form
  ,upload = layui.upload;

  var $body = $('body');
  
  //自定义验证
  form.verify({
    nickname: function(value, item){ //value：表单的值、item：表单的DOM对象

      if(!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)){
        return '用户名不能有特殊字符';
      }
      if(/(^\_)|(\__)|(\_+$)/.test(value)){
        return '用户名首尾不能出现下划线\'_\'';
      }
      if(/^\d+\d+\d$/.test(value)){
        return '用户名不能全为数字';
      }
      var message = '';
      var idvalue = $('#id').val();
      if (idvalue == undefined) {
          $.ajax({
              url: "../Admin/Admin_hasname",
              type: "post",
              async: false, //改为同步请求
              data: {
                  "strid": "",
                  "newname": value
              },
              dataType: "json",
              success: function (data) {
                  if (data) {

                  } else {
                      message = "用户名已存在，请重新输入！"
                  }
              }
          });
      } else {
          $.ajax({
              url: "../Admin/Admin_hasname",
              type: "post",
              async: false, //改为同步请求
              data: {
                  "strid": idvalue,
                  "newname": value
              },
              dataType: "json",
              success: function (data) {
                  if (data) {

                  } else {
                      message = "用户名已存在，请重新输入！"
                  }
              }
          });
      }

      if (message !== '') {
          return message;
      }
    }
    ,oldpass: function(value){
        if (value !== $('#dbpass').val()) {
            return '密码输入错误';
        }
    }
    //我们既支持上述函数式的方式，也支持下述数组的形式
    //数组的两个值分别代表：[正则匹配、匹配不符时的提示文字]
    ,pass: [
      /^[\S]{5,12}$/
      ,'密码必须6到12位，且不能出现空格'
    ]
    
    //确认密码
    ,repass: function(value){
      if(value !== $('#LAY_password').val()){
        return '两次密码输入不一致';
      }
    }
  });
  
  //设置我的资料
  form.on('submit(setmyinfo)', function(obj){
      var admin = JSON.stringify(obj.field);
      //提交修改
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
                  refresh();
              } else {
                  layer.msg('修改失败', {
                      icon: 2,
                      shift: -1,  //震动效果
                      time: 1000  //1秒关闭
                  });
              }
          }
      });
    return false;
  });

  //设置密码
  form.on('submit(setmypass)', function(obj){
      var setpass = obj.field;
      var id = setpass.id;
      var newpass = setpass.pass;
    
      //提交修改
      $.ajax({
          type: "post",
          url: '../MySetting/AlterPassword',
          datatype: "json",
          contenttype: "application/json; charset=utf-8",
          data: {
              "id": id,
              "newpass": newpass,
          },
          async: false,
          success: function (data) {
              if (data) {
                  layer.msg('修改成功', {
                      icon: 1,
                      time: 1000 //1秒关闭（如果不配置，默认是3秒）
                  });
                  refresh();
              } else {
                  layer.msg('修改失败', {
                      icon: 2,
                      shift: -1,  //震动效果
                      time: 1000  //1秒关闭
                  });
              }
          }
      });
    return false;
  });
  
  //对外暴露的接口
  exports('mysetting', {});
});