$.extend($.fn.validatebox.defaults.rules, {
    alphanumericunderline: {// 字母数字下划线 
        validator: function (value) {
            return /^[a-zA-Z][a-zA-Z0-9_]+$/i.test(value);
        },
        message: '请输入字母数字下划线并且以字母开头'
    },
    chsalphanumericunderline: {// 中文字母数字下划线 
        validator: function (value) {
            return /^[a-zA-Z\u0391-\uFFE5][\u0391-\uFFE5\w]+$/.test(value);
        },
        message: '请输入中文字符字母数字下划线并且以字母或中文字符开头'
    },
    chs: {// 验证中文 
        validator: function (value) {
            return /^[\u0391-\uFFE5]+$/i.test(value);
        },
        message: '请输入中文字符'
    },
    number: {// 验证整数 
        validator: function (value) {
            //return /^[0-9]\d*|0$]+$/i.test(value);
            return /^[0-9]\d*$/i.test(value);
        },
        message: '请输入正确的整数'
    },
    phone: {// 验证电话号码 
        validator: function (value) {
            return /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
        },
        message: '格式不正确,请使用下面格式:0755-8888888'
    },
    mobile: {// 验证手机号码 
        validator: function (value) {
            //return /^(13|15|18)\d{9}$/i.test(value);
            return /^[1]\d{10}$/i.test(value);
        },
        message: '手机号码格式不正确'
    },
    qq: {// 验证QQ,从10000开始 
        validator: function (value) {
            return /^[1-9]\d{4,9}$/i.test(value);
        },
        message: 'QQ号码格式不正确'
    },
    zip: {// 验证邮政编码 
        validator: function (value) {
            return /^[1-9]\d{5}$/i.test(value);
        },
        message: '邮政编码格式不正确'
    },
    email: {// 验证邮箱 
        validator: function (value) {
            return /^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$/i.test(value);
        },
        message: '邮箱格式不正确'
    },
    equalto: {
        validator: function (value, param) {
            if ($(param[0]).val() != "" && value != "") {
                return $(param[0]).val() == value;
            } else {
                return true;
            }
        },
        message: '两次输入不一致！'
    },
    idcard: {// 验证身份证号码 
        validator: function (value) {
            return IdCardValidate(value);
        },
        message: '身份证格式不正确！'
    }
});
