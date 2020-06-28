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
        double: function (value) {
            var regu = "^[0-9]+\.?[0-9]*$";
            var re = new RegExp(regu);
            if (!re.test(value)) {
                return "请输入小数类型数据!";
            }
        }
    });

    //对外暴露的接口
    exports('potion_validation', {});
});