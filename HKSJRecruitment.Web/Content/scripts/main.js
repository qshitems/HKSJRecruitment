$.ajaxSetup({
    cache: false
});
//修改默认验证提示
$.map(['validatebox', 'textbox', 'filebox', 'searchbox',
		'combo', 'combobox', 'combogrid', 'combotree',
		'datebox', 'datetimebox', 'numberbox',
		'spinner', 'numberspinner', 'timespinner', 'datetimespinner'], function (plugin) {
		    if ($.fn[plugin]) {
		        $.fn[plugin].defaults.missingMessage = function () {
		            var td = $(this).closest("td").prev("td");
		            if (td.length == 0) {
		                return "该输入项为必输项";
		            }
		            else {
		                return td.text().replace(/[:|：]$/i, "") + "不能为空";
		            }
		        }
		    }
		});

//防止超出浏览器边界
var easyuiPanelOnMove = function (left, top) {/* 防止超出浏览器边界 */
    var parentObj = $(this).panel('panel').parent();
    if (left < 0) {
        $(this).window('move', {
            left: 1
        });
    }
    if (top < 0) {
        $(this).window('move', {
            top: 1
        });
    }
    var width = $(this).panel('options').width;
    var height = $(this).panel('options').height;
    var right = left + width;
    var buttom = top + height;
    var parentWidth = parentObj.width();
    var parentHeight = parentObj.height();
    if (parentObj.css("overflow") == "hidden") {
        if (left > parentWidth - width) {
            $(this).window('move', {
                "left": parentWidth - width

            });
        }
        if (top > parentHeight - height) {
            $(this).window('move', {
                "top": parentHeight - height
            });
        }
    }
}
$.fn.panel.defaults.onMove = easyuiPanelOnMove;
$.fn.window.defaults.onMove = easyuiPanelOnMove;
$.fn.dialog.defaults.onMove = easyuiPanelOnMove;

$.fn.combotree.defaults.editable = true;

$.fn.combo.defaults.panelHeight = "auto";
$.fn.combobox.defaults.panelHeight = "auto";
$.fn.combotree.defaults.panelHeight = "auto";

$.fn.combo.defaults.panelMaxHeight = "240px";
$.fn.combobox.defaults.panelMaxHeight = "240px";
$.fn.combotree.defaults.panelMaxHeight = "240px";

//下拉列表点击列表框就可以显示数据,跟验证提示相冲突,editable=true的情况下，验证不通过，没有提示
//$.fn.combobox.defaults.editable = false;
////日期控件点击列表框就可以显示数据 
//$.fn.datebox.defaults.editable = false;

//列表页
$(function () {
    if (typeof (dgKey) == "undefined") {
        window.dgKey = "id";
    }
    if (typeof (tgFlag) == "undefined") {
        window.tgFlag = false;
    }
    if (!tgFlag && typeof (dgid) != "undefined") {
        $(dgid).datagrid({
            url: urlList,
            title: titleSuffix + "列表",
            idField: dgKey,
            queryParams: {},
            fit: true,
            fitColumns: true,
            striped: true,
            singleSelect: true,
            checkOnSelect: true,
            selectOnCheck: true,
            remoteSort: true,
            multiSort: true,
            pageSize: 20,
            pagination: true,
            method: 'post',
            toolbar: '#tb',
            showFooter: true,
            onHeaderContextMenu: function (e, field) {
                e.preventDefault();
                if (!headerContextMenu) {
                    createheaderContextMenu();
                }
                headerContextMenu.menu("show", {
                    left: e.pageX,
                    top: e.pageY
                });
            },
            onRowContextMenu: function (e, rowIndex, rowData) {
                e.preventDefault();
                if (!rowContextMenu) {
                    createrowContextMenu();
                }
                rowContextMenu.data("rowData", rowData);
                rowContextMenu.menu("show", {
                    left: e.pageX,
                    top: e.pageY
                });
            },
            onLoadSuccess: function (data) {
                if (typeof (dgLoadSuccess) === "function") {
                    dgLoadSuccess(data);
                }
                return false;
            },
            rowStyler: function (index, row) {
                if (typeof (dgRowStyler) === "function") {
                    return dgRowStyler(index, row);
                }
            },
            loadFilter: function (data) {
                if (typeof (dgloadFilter) === "function") {
                    return dgloadFilter(data);
                }
                return data;
            }
        });
    }
    $("#btnAdd").click(function () {
        add_click();
    });

    $("#btnEdit").click(function () {
        edit_click();
    });
    $("#btnDetail").click(function () {
        detail_click();
    });
    $("#btnDelete").click(function () {
        delete_click();
    });
    $("#btnSearch").click(function () {
        search_click();
    });
    $("#btnReload").click(function () {
        reload_click();
    });
    $("#btnCheckModel").click(function () {
        var model = $(this).attr("data-type");
        if (model == 1) {
            $(this).attr("data-type", "0");
            $(this).linkbutton({ text: "单选" });
            if (typeof (dgid) != "undefined") {
                $(dgid).datagrid({
                    singleSelect: true
                });
            }
        } else {
            $(this).attr("data-type", "1");
            $(this).linkbutton({ text: "多选" });
            if (typeof (dgid) != "undefined") {
                $(dgid).datagrid({
                    singleSelect: false
                });
            }
        }
    });
});
//编辑页
$(function () {
    $("#btnWinEditOk").bind("click", function () {
        var url = urlAdd;
        if ($(editid + " #id").val() > 0) {
            url = urlEdit;
        }
        $.messager.progress({ title: "请稍候..." });
        if (typeof (ValidateBefore) == "function") {
            ValidateBefore();
        }
        var isVidate = $(editid + " form").form('validate');

        if (typeof (ValidateAfer) == "function") {

            isVidate = ValidateAfer(isVidate);
        }
        if (!isVidate) {
            $.messager.progress("close");
            $.messager.show({ title: '表单验证', msg: '表单验证失败' });
            return;
        }
        $.ajax({
            type: "POST",
            url: url,
            data: $(editid + " form").serialize(),
            dataType: "json",
            success: function (data) {
                $.messager.progress("close");
                var result = data;
                if (typeof (EditFinish) == "function") {
                    EditFinish(result);
                    return;
                }
                if (result.result) {
                    $.messager.show({ title: '编辑提示', msg: (result.msg == undefined || result.msg == '') ? '编辑成功' : result.msg });
                    $(editid).window("close");
                    if ($(editid).data("edit")) {
                        if (tgFlag) {
                            $(dgid).treegrid('update', { id: result.dto[dgKey], row: result.dto });
                        } else {
                            var rowIndex = $(dgid).datagrid('getRowIndex', result.dto[dgKey]);
                            $(dgid).datagrid('updateRow', { index: rowIndex, row: result.dto });
                        }
                    } else {
                        if (tgFlag) {
                            $(dgid).treegrid('reload');
                        }
                        else {
                            $(dgid).datagrid('insertRow', { index: 0, row: result.dto });
                        }
                    }
                } else {
                 
                    $.messager.show({ title: '编辑提示', msg: (result.msg == undefined || result.msg == '') ? '编辑失败' : "编辑失败" + result.msg });
                }
            }
        });
        //$(editid + " form").form("submit", {
        //    url: url,
        //    queryParams: { "X-Requested-With": "XMLHttpRequest" },
        //    onSubmit: function (param) {
        //        if (typeof (ValidateBefore) == "function") {
        //            ValidateBefore();
        //        }
        //        var isVidate = $(this).form('validate');
        //        if (typeof (ValidateAfer) == "function") {
        //            isVidate = ValidateAfer(isVidate);
        //        }
        //        if (!isVidate) {
        //            $.messager.progress("close");
        //            $.messager.show({ title: '表单验证', msg: '表单验证失败' });
        //        }               
        //        return isVidate;
        //    },
        //    success: function (data) {
        //        $.messager.progress("close");
        //        var result = null;
        //        try {
        //            result = JSON.parse(data);
        //        } catch (e) {
        //            $.messager.show({ title: '编辑提示222', msg: data });
        //            return;
        //        }
        //        if (typeof (EditFinish) == "function") {
        //            EditFinish(result);
        //            return;
        //        }
        //        if (result.result) {
        //            $.messager.show({ title: '编辑提示', msg: (result.msg == undefined || result.msg == '') ? '编辑成功' : result.msg });
        //            $(editid).window("close");
        //            if ($(editid).data("edit")) {
        //                if (tgFlag) {
        //                    $(dgid).treegrid('update', { id: result.dto[dgKey], row: result.dto });
        //                } else {
        //                    var rowIndex = $(dgid).datagrid('getRowIndex', result.dto[dgKey]);
        //                    $(dgid).datagrid('updateRow', { index: rowIndex, row: result.dto });
        //                }
        //            } else {
        //                if (tgFlag) {
        //                    $(dgid).treegrid('reload');
        //                }
        //                else {
        //                    $(dgid).datagrid('insertRow', { index: 0, row: result.dto });
        //                }
        //            }
        //        } else {
        //            $.messager.show({ title: '编辑提示', msg: (result.msg == undefined || result.msg == '') ? '编辑失败' : "编辑失败," + result.msg });
        //        }
        //    }
        //});
    });
    $("#btnWinEditCancel").bind("click", function () {
        $(editid).window("close");
    });
});
//搜索页
$(function () {
    $("#btnWinQueryOk").bind("click", function () {
        var queryParams = {};
        var arr = $("#winQuery form").serializeArray();
        $.each(arr, function () {
            if (this.value) {
                queryParams[this.name] = this.value + "";
            }
        });
        $(dgid).datagrid("unselectAll");
        $(dgid).datagrid('load', queryParams);
        $(searchid).window("close");
    });
    $("#btnWinQueryReset").bind("click", function () {
        $(searchid + " form")[0].reset();
        if (typeof (SearchReset) === "function") {
            SearchReset();
        }
    });
    $("#btnWinQueryCancel").bind("click", function () {
        $(searchid).window("close");
    });
});
//详情页
$(function () {
    $("#btnWinDetailClose").bind("click", function () {
        $(detailid).window("close");
    });
});

//右键菜单
var headerContextMenu;
var rowContextMenu;
function createheaderContextMenu() {
    headerContextMenu = $("#conextMenuHeader");
    headerContextMenu.menu({
        onClick: function (item) {
            if (item.iconCls == 'icon-ok') {
                $(dgid).datagrid('hideColumn', item.name);
                headerContextMenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-empty'
                });
            } else {
                $(dgid).datagrid('showColumn', item.name);
                headerContextMenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-ok'
                });
            }
        }
    });
    var fields = $(dgid).datagrid("getColumnFields");
    for (var i = 1; i < fields.length; i++) {
        var field = fields[i];
        var col = $(dgid).datagrid("getColumnOption", field);
        headerContextMenu.menu("appendItem", {
            text: col.title,
            name: col.field,
            iconCls: function () {
                if (col.hidden) return "icon-empty";
                else return "icon-ok";
            }
        });
    }
}
function createrowContextMenu() {
    rowContextMenu = $("#contextMenuRow");
    rowContextMenu.menu({});
    $("#tb a").each(function () {
        var $this = $(this);
        var linkOpt = $(this).linkbutton("options");
        rowContextMenu.menu("appendItem", {
            text: linkOpt.text,
            name: "menu" + linkOpt.id,
            id: "menu" + linkOpt.id,
            iconCls: linkOpt.iconCls,
            onclick: function () {
                $this.click();
            }
        });
    });
    if (typeof (initContextMenu) === "function") {
        initContextMenu(rowContextMenu);
    }
}
function Detail(rowData) {
    $.post(urlDetail + '/' + rowData[dgKey], function (data) {
        if (data.result) {
            data = data.dto;
            for (var item in data) {
                if (data[item] == null) continue;
                $("#sp" + item).text(data[item]);
            }
            if (typeof (LoadDetailDataSuccess) === 'function') {
                LoadDetailDataSuccess(data);
            }
        } else {
            $.messager.show({ title: titleSuffix, msg: '加载数据失败，' + data.msg + '!' });
        }
    }, "json");
    $(detailid).window("setTitle", titleSuffix + "详情");
    $(detailid).window("open");
}
function EditRow(rowData) {
    $(editid).data("edit", true);
    if (typeof (initEdit) === "function") {
        initEdit();
    }
    $.post(urlDetail + '/' + rowData[dgKey], function (data) {
        if (data.result) {
            try {
                $(editid + " form").form('load', data.dto);
            } catch (e) {
                alert(e);
            }
            if (typeof (LoadEditDataSuccess) === 'function') {
                LoadEditDataSuccess(data);
            }
        } else {
            $.messager.show({ title: '加载提示', msg: '加载数据失败，' + data.msg + '!' });
        }
    }, "json");
    var title = "修改" + titleSuffix;
    if (typeof (intEditWindowTitle) === 'function') {
        title = intEditWindowTitle();
    }
    $(editid).window("setTitle", title);
    $(editid).window("open");
}
function DeleteRows() {
    var title = '删除';
    if (typeof (intDeleteTitle) === 'function') {
        title = intDeleteTitle();
    }
    var datas = $(dgid).datagrid('getChecked');
    var ids = "";
    $.each(datas, function () {
        ids += this[dgKey] + ",";
    });
    if (ids == "") {
        $.messager.alert(titleSuffix, '请选择要删除的行!');
        return;
    }
    $.messager.confirm(title, '确定要' + title + '选中行的数据吗?', function (result) {
        if (result) {
            var url = urlDeleteMulti + '/' + ids;
            var param = {};
            $.post(url, param, function (data) {
                if (data.result) {
                    $.messager.show({ title: title + '提示', msg: title + '成功' });
                    $(ids.split(',')).each(function (index, item) {
                        if (item) {
                            if (tgFlag) {
                                $(dgid).treegrid('remove', item);
                            }
                            else {
                                var rowIndex = $(dgid).datagrid('getRowIndex', item);
                                $(dgid).datagrid('deleteRow', rowIndex);
                            }
                        }
                    });
                } else {
                    $.messager.show({ title: title + '提示', msg: title + '失败,' + data.msg });
                }
            }, 'json');
        }
    });
}
function Search() {
    $(searchid).window("setTitle", titleSuffix + "查询");
    $(searchid).window("open");
}
function Reload() {
    if (tgFlag) {
        $(dgid).treegrid('unselectAll');
        $(dgid).treegrid("reload");
    } else {
        $(dgid).datagrid('unselectAll');
        $(dgid).datagrid("reload");
    }
    document.URL = location.href;


}
//////////////////////////////////////////////////
function add_click() {
    $("#id").val("0");
    $(editid + " form")[0].reset();
    $(editid).data("edit", false);
    $(editid).window("setTitle", "新增" + titleSuffix);
    $(editid).window("open");
    if (typeof (initEdit) === "function") {
        initEdit();
    }
}
function edit_click() {
    $(editid + " form")[0].reset();
    var rowData = null;
    if (tgFlag) {
        if ($(dgid).treegrid("getSelections").length > 1) {
            $.messager.alert(titleSuffix, '编辑时一次只能选择一行!');
            return;
        }
        rowData = $(dgid).treegrid("getSelected");
    } else {
        if ($(dgid).datagrid("getSelections").length > 1) {
            $.messager.alert(titleSuffix, '编辑时一次只能选择一行!');
            return;
        }
        rowData = $(dgid).datagrid("getSelected");
    }
    if (rowData == null) {
        $.messager.alert(titleSuffix, '请选择要编辑的行!');
        return;
    }
    EditRow(rowData);
}
function detail_click() {
    $("#winDetail label[id]").text("");
    var rowData = null;
    if (tgFlag) {
        rowData = $(dgid).treegrid("getSelected");
    } else {
        rowData = $(dgid).datagrid("getSelected");
    }
    if (rowData == null) {
        $.messager.alert(titleSuffix, '请选择要查看详情的行!');
        return;
    }
    Detail(rowData);
}
function delete_click() {
    DeleteRows();
}
function search_click() {
    Search();
}
function reload_click() {
   
    Reload();
}

