/**

 @Name：药剂数控中心 药剂管理
 @Author：lalalayt
    
 */


layui.define(['table', 'form'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form;

    //药剂管理
    table.render({
        elem: '#potion_list'
        , url: ' ../Potion/GetAllPotions' //模拟接口
        , height: 'full-240' //高度最大化减去差值
        , cols: [[
            { field: 'xh', title: '序号', sort: true }
            , { field: 'zwmc', title: '中文名称', minWidth: 100 }
            , { field: 'ywmc', title: '英文名称' }
            , { field: 'fzs', title: '分子式', templet: '#imgTpl' }
            , { field: 'zc', title: '组成' }
            , { field: 'wg', title: '外观' }
            , { field: 'scdw', title: '生产单位' }
            , { title: '操作', width: 300, align: 'center', fixed: 'right', toolbar: '#table-useradmin-webuser' }
        ]]
        , page: true
        , limit: 30
        , text: '对不起，加载出现异常！'
    });

    //监听工具条
    table.on('tool(potion_list)', function (obj) {
        var data = obj.data;
        if (obj.event === 'detail') {
            layer.open({
                type: 2
                , title: '药剂详情'
                , content: '../Potion/PotionDetailForm'
                , maxmin: true
                , area: ['80%', '90%']
                , btn: ['取消']
                , yes: function (index, layero) {
                    layer.close(index); //关闭弹层
                }
                , success: function (layero, index) {
                    var body = layer.getChildFrame('body', index);  // body.html() body里面的内容 
                    body.find("#id").val(data.id);
                    body.find("#xh").val(data.xh);
                    body.find("#zwmc").val(data.zwmc);
                    body.find("#ywmc").val(data.ywmc);
                    body.find("#ywjc").val(data.ywjc);
                    body.find("#fzs").val(data.fzs);
                    body.find("#zc").val(data.zc);
                    body.find("#ys").val(data.ys);
                    body.find("#cd").val(data.cd);
                    body.find("#jx").val(data.jx);
                    body.find("#wg").val(data.wg);
                    body.find("#yybj").val(data.yybj);
                    body.find("#scdw").val(data.scdw);
                    body.find("#sjly").val(data.sjly);

                    body.find("#wgzp").val(data.wgzp);
                    body.find("#llmd").val(data.llmd);
                    body.find("#djmd").val(data.djmd);
                    body.find("#ldd50").val(data.ldd50);
                    body.find("#ldfbqx").val(data.ldfbqx);
                    body.find("#lsx").val(data.lsx);
                    body.find("#yyylmdqx").val(data.yyylmdqx);
                    body.find("#yyylmd").val(data.yyylmd);
                    body.find("#xsbfs").val(data.xsbfs);
                    body.find("#rjd").val(data.rjd);
                    body.find("#phz").val(data.phz);
                    body.find("#zljzbfs").val(data.zljzbfs);
                    body.find("#jxqbyl").val(data.jxqbyl);
                    body.find("#sb").val(data.sb);
                    body.find("#rs").val(data.rs);
                    body.find("#yqsj").val(data.yqsj);
                    body.find("#dhnl").val(data.dhnl);
                    body.find("#xgbsy").val(data.xgbsy);
                    body.find("#zjysx").val(data.zjysx);
                    body.find("#rkbxsy").val(data.rkbxsy);
                    body.find("#rd").val(data.rd);
                    body.find("#sfjhff").val(data.sfjhff);
                    body.find("#bbmj").val(data.bbmj);
                    body.find("#dzl").val(data.dzl);
                    body.find("#p_tqx").val(data.p_tqx);

                    body.find("#zjgd").val(data.zjgd);
                    body.find("#zjg").val(data.zjg);
                    body.find("#zcgd").val(data.zcgd);
                    body.find("#mcgd").val(data.mcgd);
                    body.find("#hygd").val(data.hygd);
                    body.find("#rsgd").val(data.rsgd);
                    body.find("#jdgd").val(data.jdgd);
                    body.find("#zmcgd").val(data.zmcgd);
                    body.find("#jgfhgd").val(data.jgfhgd);
                    body.find("#dlztfhgd").val(data.dlztfhgd);
                    body.find("#qbl").val(data.qbl);
                    body.find("#qbgd").val(data.qbgd);

                    body.find("#crfxqx").val(data.crfxqx);
                    body.find("#rzfxqx").val(data.rzfxqx);
                    body.find("#jrlrqx").val(data.jrlrqx);
                    body.find("#rjxfxqx").val(data.rjxfxqx);
                    body.find("#wrlrsj").val(data.wrlrsj);
                    body.find("#rksysj").val(data.rksysj);
                    body.find("#hhn").val(data.hhn);
                    body.find("#zqyz").val(data.zqyz);
                    body.find("#frl").val(data.frl);
                    body.find("#drl").val(data.drl);
                    body.find("#br").val(data.br);
                    body.find("#rsr").val(data.rsr);
                    body.find("#rswd").val(data.rswd);
                    body.find("#bw").val(data.bw);
                    body.find("#brd").val(data.brd);
                    body.find("#brr").val(data.brr);
                    body.find("#bfd").val(data.bfd);
                    body.find("#fjwd").val(data.fjwd);

                    body.find("#zkadxvsp").val(data.zkadxvsp);
                    body.find("#gwgsadx").val(data.gwgsadx);
                    body.find("#rsz").val(data.rsz);
                    body.find("#xrx").val(data.xrx);
                    body.find("#radx").val(data.radx);
                    body.find("#qjsy").val(data.qjsy);

                    body.find("#zcsm").val(data.zcsm);

                    if (data.fzs != '') body.find('#picture_fzs').attr('src', data.fzs);
                    if (data.wgzp != '') body.find('#picture_wgzp').attr('src', data.wgzp);
                    if (data.ldfbqx != '') body.find('#picture_ldfbqx').attr('src', data.ldfbqx);
                    if (data.yyylmdqx != '') body.find('#picture_yyylmdqx').attr('src', data.yyylmdqx);
                    if (data.crfxqx != '') body.find('#picture_crfxqx').attr('src', data.crfxqx);
                    if (data.rzfxqx != '') body.find('#picture_rzfxqx').attr('src', data.rzfxqx);
                    if (data.jrlrqx != '') body.find('#picture_jrlrqx').attr('src', data.jrlrqx);
                    if (data.rjxfxqx != '') body.find('#picture_rjxfxqx').attr('src', data.rjxfxqx);

                    var i;
                    for (i = 0; i < data.Components.length; i++) {
                        var aa = "<div class='layui-col-md6' id='" + i + "'>"
                            + "<div class='layui-col-md12 layui-col-space10'>"

                            + "<div class='layui-col-md6'>"
                            + "<label class='layui-form-label'>药剂名称</label>"
                            + "<div class='layui-input-block'>"
                            + "<input type='text' name='Components[" + i + "].name' placeholder='药剂名称' lay-verify='required' autocomplete='off' class='layui-input' value=" + data.Components[i].name + " readonly>"
                            + "</div>"
                            + "</div>"

                            + "<div class='layui-col-md6'>"
                            + "<label class='layui-form-label'>含量</label>"
                            + "<div class='layui-input-block'>"
                            + "<input type='text' name='Components[" + i + "].content' placeholder='含量' lay-verify='required' autocomplete='off' class='layui-input'  value=" + data.Components[i].content + " readonly>"
                            + "</div>"
                            + "</div>"

                            + "</div>";
                        + "</div>";
                        body.find("#test").append(aa);
                    }
                    form.render();
                }
            });
        } 
    });

    exports('index_table', {})
});