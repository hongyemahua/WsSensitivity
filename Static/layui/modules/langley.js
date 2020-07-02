/**

 @Name：兰利法计算页面
 @Author：Yant
    
 */


layui.define(['table', 'form'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form;

    //兰利法表格
    table.render({
        elem: '#langley_list'
        , url: '../Langley/GetAllLangleys' //模拟接口
        , height: 'full-130'//高度最大化减去差值
        //, toolbar: 'default' 
       // , toolbar: '#toolbarDemo' 
        , cols: [[
                
            { field: 'ldt_Number', width: 80, title: '编号', sort: true }
            , { field: 'ldt_StimulusQuantity', title: '刺激量' }
            , { field: 'ldt_Response', title: '响应', templet: '#response_state', minWidth: 80, align: 'center'}
            , { field: 'ldt_Mean', title: '均值' }
            , { field: 'ldt_StandardDeviation', title: '标准值' }
            , { field: 'ldt_MeanVariance', title: '均值的方差' }
            , { field: 'ldt_StandardDeviationVariance', title: '标准值的方差' }
            , { field: 'ldt_Note1', title: '备注' }
        ]]
        , page: true
        , limit: 30
        , text: '对不起，加载出现异常！'
    });

    exports('langley', {})
});