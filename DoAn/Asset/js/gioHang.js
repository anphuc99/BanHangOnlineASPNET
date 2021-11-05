let list = new Array();
class GioHang {    
    constructor(id_sp, sl) {
        this.id_sp = id_sp
        this.sl = sl
    }
    get getID_sp() {
        return this.id_sp
    }
    set setID_sp(i) {
        this.id_sp = i
    }
    get getSL() {
        return this.sl
    }
    set setSL(i) {
        this.sl = i
    }
    toString() {
        return this.id_sp + "|" + this.sl
    }
}
function getGioHang() {    
    if (getCookie("GioHang") == null || getCookie("GioHang")=="" ) return new Array();
    let gioHang = new Array();    
    let GioHangs = getCookie("GioHang").split(',');
    GioHangs.forEach(function (item, index) {
        let gh = item.split('|');
        gioHang.push({ id_sp: gh[0], sl: gh[1]})
    })
    return gioHang
}
function setGioHang(gh) {
    if (getCookie("confirm") !== "true") {
        $("#dialog-confirm").dialog("open")
        return
    }
    let str = "";
    gh.forEach(function (item, index) {
        str += item.id_sp + "|" + item.sl+","
    })
    setCookie("GioHang", str.substring(0, str.length - 1))
    $("#badge").text(getGioHang().length)
}
function addGioHang(id_sp, sl) {   
    let gioHang = getGioHang()
    let coSP = false
    if (gioHang !== null) {
        gioHang.forEach(function (item, index) {
            if (item.id_sp == id_sp) {
                coSP = true
                item.sl = sl
            }
            
        })
    }    
    if (!coSP) {
        gioHang.push({ id_sp: id_sp, sl: sl })
    }
    setGioHang(gioHang)
}

function increaseGioHang(id_sp) {
    debugger
    let gioHang = getGioHang()
    let coSP = false
    if (gioHang !== null) {
        gioHang.forEach(function (item, index) {
            if (item.id_sp == id_sp) {
                coSP = true
                item.sl++
            }

        })
    }
    if (!coSP) {
        gioHang.push({ id_sp: id_sp, sl: 1 })
    }
    setGioHang(gioHang)
}
function reductionGioHang(id_sp) {
    let gioHang = getGioHang()
    if (gioHang !== null) {
        gioHang.forEach(function (item, index) {
            if (item.id_sp == id_sp) {
                coSP = true
                if (item.sl > 0)
                    item.sl--
            }

        })
    }
    setGioHang(gioHang)
}
function dropGioHang(id_sp) {
    let gioHang = getGioHang()
    if (gioHang !== null) {
        gioHang.forEach(function (item, index) {
            if (item.id_sp == id_sp) {
                gioHang.splice(index,1)
            }
        })
    }
    setGioHang(gioHang)
}