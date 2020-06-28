/**

 @Name：药剂数控中心 管理员验证
 @Author：lalalayt
    
 */

layui.define('form', function (exports) {
    var $ = layui.$
        , layer = layui.layer
        , laytpl = layui.laytpl
        , setter = layui.setter
        , view = layui.view
        , admin = layui.admin
        , form = layui.form;

    var $body = $('body');

    //自定义验证
    form.verify({
        name: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '用户名不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '用户名首尾不能出现下划线\'_\'';
            }
            if (/^\d+\d+\d$/.test(value)) {
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

        //我们既支持上述函数式的方式，也支持下述数组的形式
        //数组的两个值分别代表：[正则匹配、匹配不符时的提示文字]
        , pass: [
            /^[\S]{5,12}$/
            , '密码必须5到12位，且不能出现空格'
        ]

        , repass: function (value) {
            var passvalue = $('#pass').val();
            if (value != passvalue) {
                return '两次输入的密码不一致!';
            }
        }
    });

    //对外暴露的接口
    exports('admin_validation', {});
});