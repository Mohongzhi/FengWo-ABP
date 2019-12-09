var id = window.id;// '@ViewBag.Id';
var parent = null;
var dto = {};
var listWhere = null;
var formJson = null;
var appObj = {
    loading: true,
    XQKey: null,
    XQKeys: [],
    YXKey: null,
    YXKeys: [],
    ZYKey: null,
    ZYKeys: [],
    BJKey: null,
    BJKeys: []
};
$(function () {
    abp.ui.setBusy('#lockRow');
    abp.services.app.basicDataMaintenance.getCascaderByKeyword('XQKey', appObj.XQKey, null).done(function (data) {
        appObj.XQKeys = data;
    });
    abp.services.app.basicDataMaintenance.getCascaderByKeyword('YXKey', appObj.XQKey, null).done(function (data) {
        appObj.YXKeys = data;
    });
    abp.services.app.questionnaire.getQuestionnaireLinkageListByItemId(id, null).done(function (list) {
        listWhere = list;
        abp.services.app.questionnaire.getQuestionnaireFormById(id, null).done(function (data) {
            var listToHide = [];
            for (var i = 0; i < list.length; i++) {
                listToHide.push(list[i].name);
            }
            //parent.$refs.generateForm.setJSON(JSON.parse(data));
            //var a = parent.$refs.generateForm;
            formJson = JSON.parse(data);
            appObj.jsonData = formJson;
            parent = new Vue({
                el: '#app',
                data:function() {
                    return appObj//{ jsonData: formJson }
                },
                methods: {
                    onSomeChanged: function (field, value, formData) {
                        var listToShow = [];
                        var listToHide = [];
                        ChangedSomeQuestion(listWhere, field, value, formData, listToShow, listToHide);
                        parent.$refs.generateForm.display(listToShow);
                        parent.$refs.generateForm.hide(listToHide);
                    },
                    onXQChanged: function () {

                    },
                    onYXChanged: function () {
                        appObj.ZYKey = null;
                        appObj.BJKey = null;
                        abp.services.app.basicDataMaintenance.getCascaderByKeywordParent('ZYKey', appObj.YXKey, null).done(function (data) {
                            appObj.ZYKeys = data;
                        });
                    },
                    onZYChanged: function () {
                        appObj.BJKey = null;
                        abp.services.app.basicDataMaintenance.getCascaderByKeywordParent('BJKey', appObj.ZYKey, null).done(function (data) {
                            appObj.BJKeys = data;
                        });
                    },
                }
            });
            parent.$refs.generateForm.hide(listToHide);
            appObj.loading = false;
            abp.ui.clearBusy('#lockRow');
        });
    });
});
function ChangedSomeQuestion(listWhere, field, value, formData, listToShow, listToHide) {
    for (var i = 0; i < listWhere.length; i++) {//全部的条件
        if (listWhere[i].parentName == field) {//存在一样de
            for (var j = 0; j < formJson.list.length; j++) {//判断表单内的类型
                if (formJson.list[j].name == field) {//一样则进入
                    if (formJson.list[j].type == 'rate') {//评分型进入
                        if (listWhere[i].showCondition == 1) {//大于判断模式
                            if (value >= listWhere[i].targetValue) {
                                listToShow.push(listWhere[i].name);//超过目标值,显示
                                var childValue = formData[listWhere[i].name];
                                if (childValue.length > 0)
                                    ChangedSomeQuestion(listWhere, listWhere[i].name, childValue[0], formData);
                            }
                            else {
                                listToHide.push(listWhere[i].name);//未超过目标值,不显示
                                //获取父级为被隐藏的选项一并隐藏
                                for (var k = 0; k < listWhere.length; k++) {
                                    if (listWhere[k].parentName == listWhere[i].name) {
                                        listToHide.push(listWhere[k].name);
                                        var childValue = formData[listWhere[i].name];
                                        if (childValue.length > 0)
                                            ChangedSomeQuestion(listWhere, listWhere[i].name, childValue[0], formData);
                                    }
                                }
                            }
                        }
                        else if (listWhere[i].showCondition == 2) {//小于
                            if (value < listWhere[i].targetValue) {
                                listToShow.push(listWhere[i].name);//低于目标值,显示
                                var childValue = formData[listWhere[i].name];
                                if (childValue.length > 0)
                                    ChangedSomeQuestion(listWhere, listWhere[i].name, childValue[0], formData, listToShow, listToHide);
                            }
                            else {
                                listToHide.push(listWhere[i].name);//未低于目标值,不显示
                                //获取父级为被隐藏的选项一并隐藏
                                for (var k = 0; k < listWhere.length; k++) {
                                    if (listWhere[k].parentName == listWhere[i].name) {
                                        listToHide.push(listWhere[k].name);
                                        var childValue = formData[listWhere[i].name];
                                        if (childValue.length > 0)
                                            ChangedSomeQuestion(listWhere, listWhere[i].name, childValue[0], formData, listToShow, listToHide);
                                    }
                                }
                            }
                        }
                    }
                    else {//否则未文本型
                        if (value.indexOf(listWhere[i].targetValue) != -1) {//包含型
                            listToShow.push(listWhere[i].name);//超过目标值,显示
                            var childValue = formData[listWhere[i].name];
                            if (childValue.length > 0)
                                ChangedSomeQuestion(listWhere, listWhere[i].name, childValue[0], formData);
                        }
                        else {
                            listToHide.push(listWhere[i].name);//未超过目标值,不显示
                            var childValue = formData[listWhere[i].name];
                            if (childValue.length > 0)
                                ChangedSomeQuestion(listWhere, listWhere[i].name, childValue[0], formData);
                        }
                    }
                }
            }
        }
    }
}

function Submit() {
    if (appObj.XQKey == null) {
        parent.$message({ message: '请选择校区', type: 'warning' });
        return;
    }
    if (appObj.YXKey == null) {
        parent.$message({ message: '请选择院系', type: 'warning' });
        return;
    }
    if (appObj.ZYKey == null) {
        parent.$message({ message: '请选择专业', type: 'warning' });
        return;
    }
    if (appObj.BJKey == null) {
        parent.$message({ message: '请选择班级', type: 'warning' });
        return;
    }
    parent.$refs.generateForm.getData().then(data => {
        dto.questionnaireItemId = id;
        dto.questionnaireAnswer = JSON.stringify(data);
        dto.xQKey = appObj.XQKey;
        dto.yXKey = appObj.YXKey;
        dto.zYKey = appObj.ZYKey;
        dto.bJKey = appObj.BJKey;
        abp.services.app.questionnaire.submitQuestionnaireAnswer(dto, null).done(function (data) {
            abp.message.success('提交成功', '', false);
            window.location.href = "/Survey/TanksForAnswer";

        }).fail(function (data) {
            debugger;
            abp.message.error('提交失败', '', false);
        });
        // 数据获取成功
    }).catch(e => {
        //debugger;
        abp.message.error('请完成必填项', '', false);
        // 数据校验失败
    });
}