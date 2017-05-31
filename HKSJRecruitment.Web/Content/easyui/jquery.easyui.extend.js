/**
 * jQuery EasyUI 1.4.2
 * 
 * Copyright (c) 2009-2015 www.jeasyui.com. All rights reserved.
 *
 * jQuery EasyUI 扩展方法
 * Author: caoxilong
 */

$.extend($.fn.combobox.methods, {
    //清空原来的数据
    clearItem: function (jq) {
        $(jq).combobox('panel').html("");
    },
    //检查数据为选择项
    checkData: function (jq) {
        var obj = $(jq);
        var data = obj.combobox("getData");
        var value = obj.combobox("getValue");
        var text = obj.combobox("getText");
        var options = obj.combobox("options");
        var valueField = options.valueField;
        var textField = options.textField;
        var isExist = false;
        for (var i = 0; i < data.length; i++) {
            if (data[i][valueField] == value && data[i][textField] == text) {
                isExist = true;
            }
        }
        if (!isExist) {
            obj.combobox("clear");
        }
    }
});
$.extend($.fn.combotree.methods, {
    //检查数据为选择项
    checkData: function (jq) {
        var obj = $(jq);
        var options = obj.combotree("options");
        var data = getTreeDataValueArray(options.data);
        var text = obj.combotree("getText");
        var value = obj.combotree("getValue");
        var isExist = false;
        for (var i = 0; i < data.length; i++) {
            if (data[i].text == text) {
                isExist = true;
            }
        }
        if (!isExist) {
            obj.combotree("clear");
        }
        function getTreeDataValueArray(treeData) {
            var data = [];
            for (var j = 0; j < treeData.length; j++) {
                data.push({ id: treeData[j].id, text: treeData[j].text });
                if (treeData[j].children != undefined && treeData[j].children.length > 0) {
                    data = data.concat(getTreeDataValueArray(treeData[j].children));
                }
            }
            return data;
        }
    }
});
$.extend($.fn.datebox.methods, {
    //检查数据为选择项
    checkData: function (jq) {
        var obj = $(jq);
        if (!dataCheck(obj.datebox("getValue"))) {
            obj.datebox("clear");
        }
        function dataCheck(date) {
            var result = date.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
            if (result == null)
                return false;
            var d = new Date(result[1], result[3] - 1, result[4]);
            return (d.getFullYear() == result[1] && (d.getMonth() + 1) == result[3] && d.getDate() == result[4]);
        }
    }
});

