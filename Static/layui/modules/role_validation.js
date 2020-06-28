/**

 @Name：药剂数控中心 角色验证
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
        rolename: function (value, item) { //value：表单的值、item：表单的DOM对象
            if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                return '角色名不能有特殊字符';
            }
            if (/(^\_)|(\__)|(\_+$)/.test(value)) {
                return '角色名首尾不能出现下划线\'_\'';
            }
            if (/^\d+\d+\d$/.test(value)) {
                return '角色名不能全为数字';
            }

            var message = '';
            var idvalue = $('#id').val();
            if (idvalue == undefined) {
                $.ajax({
                    url: "../Role/Role_hasname",
                    type: "post",
                    async: false, //改为同步请求
                    data: {
                        "strid": "",
                        "newrolename": value
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data) {

                        } else {
                            message = "角色名已存在，请重新输入！"
                        }
                    }
                });
            } else {
                $.ajax({
                    url: "../Role/Role_hasname",
                    type: "post",
                    async: false, //改为同步请求
                    data: {
                        "strid": idvalue,
                        "newrolename": value
                    },
                    dataType: "json",
                    success: function (data) {
                        if (data) {

                        } else {
                            message = "角色名已存在，请重新输入！"
                        }
                    }
                });
            }

            if (message !== '') {
                return message;
            }
        }

    });

    //对外暴露的接口
    exports('role_validation', {});
});