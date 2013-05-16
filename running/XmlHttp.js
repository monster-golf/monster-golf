var XmlLoaderCount = 0; function XmlLoader(u) { this.onload = donothing; this.onerror = donothing; this.ontimeout = donothing; this.url = u; this.timeoutSeconds = 120; var running = false; var timeoutId = null; var THIS = this; this.sendGet = mySendGet; this.sendPost = mySendPost; this.sendDOMPost = mySendDOMPost; this.sendGetSynchronous = mySendGetSynchronous; this.halt = myHalt; this.getResponseXML = myGetResponseXML; this.getXMLString = myGetXMLString; this.tracer = null; var id = XmlLoaderCount++; getXmlHttpRequest(); function getXmlHttpRequest() { if (currBrowserVer > -1) { THIS.xml = new IEXmlLoader(); } else { THIS.xml = new MoXmlLoader(); } } function mySendGet() { if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendGet ' + this.url); myHalt(); if (THIS.xml == null) getXmlHttpRequest(); THIS.xml.request.open('GET', this.url, true); THIS.xml.request.onreadystatechange = myProcessReqChange; running = true; timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000)); THIS.xml.sendGet(); } function mySendGetSynchronous() { if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendGet ' + this.url); myHalt(); if (THIS.xml == null) getXmlHttpRequest(); THIS.xml.request.open('GET', this.url, false); THIS.xml.request.onreadystatechange = myProcessReqChange; running = true; timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000)); THIS.xml.sendGet(); myProcessReqChange(); } function mySendDOMPost(contentDOM) { var content = null; if (contentDOM.innerHTML) { content = "<" + contentDOM.tagName + ">"; content += contentDOM.innerHTML; content += "</" + contentDOM.tagName + ">"; } else if (typeof (XMLSerializer) != "undefined") content = (new XMLSerializer()).serializeToString(contentDOM); else if (contentDOM.xml) content = contentDOM.xml; if (content) { if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendPost ' + this.url); myHalt(); if (THIS.xml == null) getXmlHttpRequest(); THIS.xml.request.open('POST', THIS.url, true); THIS.xml.request.setRequestHeader("Content-type", "text/xml"); if (THIS.xml.ie) { THIS.xml.request.setRequestHeader("Content-length", content.length); THIS.xml.request.setRequestHeader("Connection", "close"); } THIS.xml.request.onreadystatechange = myProcessReqChange; running = true; timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000)); THIS.xml.sendPost(content); } } function mySendPost(contentType, contentData) { if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendPost ' + this.url); myHalt(); if (THIS.xml == null) getXmlHttpRequest(); THIS.xml.request.open('POST', THIS.url, true); THIS.xml.request.setRequestHeader("Content-type", contentType); if (THIS.xml.ie) { THIS.xml.request.setRequestHeader("Content-length", contentData.length); THIS.xml.request.setRequestHeader("Connection", "close"); } THIS.xml.request.onreadystatechange = myProcessReqChange; running = true; timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000)); THIS.xml.sendPost(contentData); } function myGetResponseXML() { if (THIS.xml && THIS.xml.request) return THIS.xml.request.responseXML; else return ""; } function myGetXMLString() { var content = ""; var respXML = myGetResponseXML(); if (typeof (XMLSerializer) != "undefined" && currBrowserVer == -1) content = (new XMLSerializer()).serializeToString(respXML); else if (THIS.xml.request.responseText) content = THIS.xml.request.responseText; else if (respXML.xml) content = respXML.xml; return content; } function myHalt() { if (THIS.tracer) THIS.tracer.trace(stamp() + 'myHalt running=' + running); if (running) { stopTimer(); if (THIS.xml) { var oldXml = THIS.xml; THIS.xml = null; oldXml.request.onreadystatechange = function() { }; oldXml.request.abort(); } } else { stopTimer(); } } function myTimeout() { if (THIS.tracer) THIS.tracer.trace(stamp() + 'myTimeout'); myHalt(); THIS.ontimeout(); } var check200LoadedCount = 0; function my200FullyLoaded() { if (THIS.xml && THIS.xml.request && THIS.xml.request.responseXML) THIS.onload(); else { if (check200LoadedCount < 30) { check200LoadedCount++; setTimeout(my200FullyLoaded, 500); } else THIS.onerror(); } } function myProcessReqChange() { if (!running) { if (THIS.tracer) THIS.tracer.trace(stamp() + 'myProcessReqChange (not running)'); } else if (THIS.xml && THIS.xml.request && THIS.xml.request.readyState != 4) { if (THIS.tracer) THIS.tracer.trace(stamp() + 'myProcessReqChange (not done) ' + THIS.xml.request.readyState); } else if (THIS.xml && THIS.xml.request && THIS.xml.request.status == 200) { if (THIS.tracer) THIS.tracer.trace(stamp() + 'myProcessReqChange (success) ' + THIS.xml.request.status); stopTimer(); my200FullyLoaded(); } else { if (THIS.tracer) THIS.tracer.trace(stamp() + 'myProcessReqChange (error)'); stopTimer(); THIS.onerror(); } } function stopTimer() { if (THIS.tracer) THIS.tracer.trace(stamp() + 'stopTimer'); running = false; if (timeoutId != null) { clearTimeout(timeoutId); timeoutId = null; } } function stamp() { return new Date() + " [xl" + id + "] "; } function donothing() { } } XmlLoader.prototype.getResponse = function() { if (loader && loader.xml && loader.xml.request) return loader.xml.request.responseXML; else return null; }; XmlLoader.prototype.selectSingleNode = function(xp) { if (this && this.xml) return this.xml.selectSingleNode(xp); else return null; }; XmlLoader.prototype.selectNodes = function(xp) { if (this && this.xml) return this.xml.selectNodes(xp); else return null; }; XmlLoader.prototype.toRightCase = function(s) { if (this && this.xml) return this.xml.toRightCase(s); else return null; }; function IEXmlLoader() { this.ie = true; if (window.XMLHttpRequest) { this.request = new XMLHttpRequest(); } else if (window.ActiveXObject) { this.request = new ActiveXObject("Microsoft.XMLHTTP"); } } IEXmlLoader.prototype.sendGet = function() { this.request.send(); }; IEXmlLoader.prototype.sendPost = function(content) { this.request.send(content); }; IEXmlLoader.prototype.selectSingleNode = function(xp) { if (this && this.request && this.request.responseXML) return this.request.responseXML.selectSingleNode(xp); else return null; }; IEXmlLoader.prototype.selectNodes = function(xp) { if (this && this.request && this.request.responseXML) return this.request.responseXML.selectNodes(xp); else return null; }; IEXmlLoader.prototype.toRightCase = function(s) { return s; }; function MoXmlLoader() { this.ie = false; if (window.XMLHttpRequest) { this.request = new XMLHttpRequest(); } else if (window.ActiveXObject) { this.request = new ActiveXObject("Microsoft.XMLHTTP"); } this.wrapper = new MOXmlWrapper(''); } MoXmlLoader.prototype.sendGet = function() { this.request.send(null); }; MoXmlLoader.prototype.sendPost = function(content) { this.request.send(content); }; MoXmlLoader.prototype.selectSingleNode = function(xp) { if (this && this.request && this.request.responseXML) { this.wrapper.xml = this.request.responseXML; return this.wrapper.selectSingleNode(xp); } else return null; }; MoXmlLoader.prototype.selectNodes = function(xp) { if (this && this.request && this.request.responseXML) { this.wrapper.xml = this.request.responseXML; return this.wrapper.selectNodes(xp); } else return null; }; MoXmlLoader.prototype.toRightCase = function(s) { if (this && this.wrapper) return this.wrapper.toRightCase(s); else return null; }; var currBrowserVer = -1; if (navigator.appName == 'Microsoft Internet Explorer') { var ua = navigator.userAgent; var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})"); if (re.exec(ua) != null) currBrowserVer = parseFloat(RegExp.$1); } function XmlWrapper(id) { if (currBrowserVer > -1) { this.wrapper = new IEXmlWrapper(id); } else { this.wrapper = new MOXmlWrapper(id); } } XmlWrapper.prototype.selectSingleNode = function(xpath) { if (this && this.wrapper) return this.wrapper.selectSingleNode(xpath); else return null; }; XmlWrapper.prototype.selectNodes = function(xpath) { if (this && this.wrapper) return this.wrapper.selectNodes(xpath); else return null; }; XmlWrapper.prototype.toRightCase = function(id) { if (this && this.wrapper) return this.wrapper.toRightCase(id); else return null; }; function XmlWrapperFromXml(textXml) { if (currBrowserVer > -1) { this.wrapper = new IEXmlWrapperFromXml(textXml); } else { this.wrapper = new MOXmlWrapperFromXml(textXml); } } XmlWrapperFromXml.prototype.selectSingleNode = function(xpath) { if (this && this.wrapper) return this.wrapper.selectSingleNode(xpath); else return null; }; XmlWrapperFromXml.prototype.selectNodes = function(xpath) { if (this && this.wrapper) return this.wrapper.selectNodes(xpath); else return null; }; XmlWrapperFromXml.prototype.toRightCase = function(id) { if (this && this.wrapper) return this.wrapper.toRightCase(id); else return null; }; function IEXmlWrapper(id) { this.xml = document.getElementById(id); } IEXmlWrapper.prototype.selectSingleNode = function(xpath) { if (this && this.xml) return this.xml.selectSingleNode(xpath); else return null; }; IEXmlWrapper.prototype.selectNodes = function(xpath) { if (this && this.xml) return this.xml.selectNodes(xpath); else return null; }; IEXmlWrapper.prototype.toRightCase = function(id) { return id; }; function IEXmlWrapperFromXml(textXml) { this.xml = new ActiveXObject("Microsoft.XMLDOM"); this.xml.async = "false"; this.xml.loadXML(textXml); } IEXmlWrapperFromXml.prototype.selectSingleNode = function(xpath) { if (this && this.xml) return this.xml.selectSingleNode(xpath); else return null; }; IEXmlWrapperFromXml.prototype.selectNodes = function(xpath) { if (this && this.xml) return this.xml.selectNodes(xpath); else return null; }; IEXmlWrapperFromXml.prototype.toRightCase = function(id) { return id; }; function MOXmlWrapper(id) { if (id != null && id != '') { var n = document.getElementById(id); var p = new DOMParser(); var sxml = n.innerHTML; if (sxml.indexOf("&nbsp;") != -1) sxml = sxml.replace(/&nbsp;/g, "&amp;nbsp;"); this.xml = p.parseFromString(sxml, 'text/xml'); this.makeLowercase = true; } else { this.makeLowercase = false; } } MOXmlWrapper.prototype.selectSingleNode = function(xpath) { var x = (this.makeLowercase) ? xpath.toLowerCase() : xpath; if (this.xml && this.xml.evaluate) { var result = this.xml.evaluate(x, this.xml, null, 9, null); if (result) return result.singleNodeValue; } return null; }; MOXmlWrapper.prototype.selectNodes = function(xpath) { var x = this.toRightCase(xpath); if (typeof (XPathEvaluator) != "undefined") { var e = new XPathEvaluator; if (typeof (e.evaluate) != "undefined") { var result = e.evaluate(x, this.xml.documentElement, null, XPathResult.ORDERED_NODE_ITERATOR_TYPE, null); var a = new Array(); var n; while ((n = result.iterateNext())) a[a.length] = n; return a; } } return null; }; MOXmlWrapper.prototype.toRightCase = function(id) { return (this.makeLowercase) ? id.toLowerCase() : id; }; function intro(o) { var s = ''; if (o) { for (p in o) s += ' ' + p; } return s; } function MOXmlWrapperFromXml(textXml) { var parser = new DOMParser(); this.xml = parser.parseFromString(textXml, "text/xml"); } MOXmlWrapperFromXml.prototype.selectSingleNode = function(xpath) { var x = (this.makeLowercase) ? xpath.toLowerCase() : xpath; if (this.xml && this.xml.evaluate) { var result = this.xml.evaluate(x, this.xml, null, 9, null); if (result) return result.singleNodeValue; } return null; }; MOXmlWrapperFromXml.prototype.selectNodes = function(xpath) { var x = this.toRightCase(xpath); if (typeof (XPathEvaluator) != "undefined") { var e = new XPathEvaluator; if (typeof (e.evaluate) != "undefined") { var result = e.evaluate(x, this.xml.documentElement, null, XPathResult.ORDERED_NODE_ITERATOR_TYPE, null); var a = new Array(); var n; while ((n = result.iterateNext())) a[a.length] = n; return a; } } return null; }; MOXmlWrapperFromXml.prototype.toRightCase = function(id) { return (this.makeLowercase) ? id.toLowerCase() : id; }; function intro(o) { var s = ''; if (o) { for (p in o) s += ' ' + p; } return s; } function WindowTracer() { this.auto = (document.location.href.indexOf('trace=auto') > -1); this.clear(); } WindowTracer.prototype.trace = function(s) { this.history += s; this.history += '\n'; }; WindowTracer.prototype.clear = function() { this.history = ''; }; function SpanTracer(s) { this.textSpan = s; this.clear(); } SpanTracer.prototype.trace = function(s) { this.textSpan.innerHTML = s; }; SpanTracer.prototype.clear = function() { this.textSpan.innerHTML = ''; }; function GetURLTimeStamp() { var d = new Date(); ts = escape(d.getTime().toString()); var t = "&ts=" + ts; return t; }

function EAttr(obj, attr) {
    var val = "";
    if (obj && typeof (obj.getAttribute) != "undefined") {
        val = obj.getAttribute(attr);
        if (!val) val = "";
    }
    return val;
}
function IsE(obj, id) {
    return (obj && obj.id && obj.id.indexOf(id) > -1);
}
function CE(e) {
    if (!e) e = window.event;
    if (e) {
        if (e.cancelBubble != null) e.cancelBubble = true;
        if (e.returnValue != null) e.returnValue = false;
        if (e.preventDefault) e.preventDefault();
        if (e.stopPropagation) e.stopPropagation();
    }
    return false;
}
function FocusNext(obj) {
    var formLen = obj.form.length;
    var x;
    var foundobj = false;
    for (x = 0; x < formLen; x++) {
        if (obj.form[x] == obj) { foundobj = true; x++; }
        if (foundobj && obj.form[x].type != "hidden") break;
    }
    if (x > formLen) x = 0;
    TxtFocus(obj.form[x]);
    return obj.form[x];
}
function TxtFocus(txt) {
    if (typeof (txt) == "string") txt = document.getElementById(txt);
    if (txt) {
        try { txt.focus(); } catch (ex) { }
    }
}
function EvKN(e) {
    if (!e) e = window.event;
    if (e && e.keyCode) return e.keyCode;
    if (e && e.which) e.which;
    return -1;
}
function EvK(e, key) { return EvKN(e) == key; }
function EvIsT(e, type) {
    if (!e) e = window.event;
    type = (type) ? type.toLowerCase() : "";
    var evtype = (e && e.type) ? e.type.toLowerCase() : "";
    return evtype == type || evtype == "on" + type;
}
function KeyEntToTab(obj, e) {
    if (!e) e = window.event;
    if (EvK(e, 13)) {
        FocusNext(obj);
        return CE(e);
    }
    return true;
}
function KeyEntToSearch(obj, e) {
    if (!e) e = window.event;
    if (EvK(e, 13)) {
        if (IsE(obj, "txtFindCourse")) CourseSearch(obj);
        else if (IsE(obj, "score_name_")) PlayerSearch(GetFirstItem(obj.parentNode.firstChild, "score_search_"));
        return CE(e);
    } else if (!EvK(e, 9) && IsE(obj, "score_name_")) {
        if (obj.value.length >= 3) PlayerSearch(GetFirstItem(obj.parentNode.firstChild, "score_search_")); 
    }
    return true;
}
function KeyEntKill(e) {
    if (!e) e = window.event;
    if (EvK(e, 13)) return CE(e);
    return true;
}
var asyncSend, asyncSendDone, asyncSendErr;
var asyncHTMLDone, asyncHTML, asyncHTMLErrFunc;
var htmlAsyncHold = new Array();
var sendFormHold = new Array();
var htmlHoldInt, sendHoldInt;
var xmlurl = "RunningXml.aspx";
function GetTime(question) {
    var d = new Date();
    ts = escape(d.getTime().toString());
    var t = ((question) ? "?" : "&") + "tm=" + ts;
    return t;
}
function GetURL(q) {
    var url = xmlurl;
    url += GetTime(true);
    if (!q) q = "";
    else q = q.toString();
    if (q.indexOf("&") != 0) url += "&";
    url += q;
    if (document.forms[0].hdnRunnerId) url += "&runnerid=" + document.forms[0].hdnRunnerId.value;
    return url;
}
function asyncNext(q, func, errfunc) {
    this.query = q;
    this.donefunc = func;
    this.errfunc = errfunc;
    this.formdata = "";
    this.updaterow = null;
    this.errfunc = null;

    this.addNext = function() {
        htmlAsyncHold.push(this);
        if (!htmlHoldInt) htmlHoldInt = window.setInterval(this.runNext, 500);
    };
    this.addNextForm = function() {
        sendFormHold.push(this);
        if (!sendHoldInt) sendHoldInt = window.setInterval(this.runNextForm, 500);
    };
    this.runNext = function() {
        if (!asyncHTML) {
            var htN = htmlAsyncHold.shift();
            if (htN == null && htmlHoldInt) {
                window.clearInterval(htmlHoldInt);
                htmlHoldInt = null;
            }
            else GetHTMLAsync(htN.query, htN.donefunc, htN.errfunc);
        }
    };
    this.runNextForm = function() {
        if (!asyncSend) {
            var seN = sendFormHold.shift();
            if (seN == null && sendHoldInt) {
                window.clearInterval(sendHoldInt);
                sendHoldInt = null;
            } else SendForm(seN.formdata, seN.query, seN.donefunc, seN.errfunc);
        }
    };
}
function SendForm(formdata, q, updone, uperr) {
    if (formdata != "") {
        if (!asyncSend) {
            if (window.Loading) window.Loading();
            asyncSend = new XmlLoader(GetURL(q));
            asyncSendDone = updone;
            asyncSendErr = uperr;
            asyncSend.onload = SendFormDone;
            asyncSend.timeout = SendFormErr;
            asyncSend.onerror = SendFormErr;
            asyncSend.sendPost("application/x-www-form-urlencoded", formdata);
        } else {
            var next = new asyncNext(q, updone);
            next.formdata = formdata;
            next.errfunc = uperr;
            next.addNextForm();
        }
    }
}
function SendFormDone() {
    if (typeof(asyncSendDone) == "function") asyncSendDone(asyncSend.getXMLString());
    if (window.Loading) window.Loading(true);
    asyncSend = null;
}
function SendFormErr() {
    if (typeof (asyncSendErr) == "function") asyncSendErr();
    if (window.Loading) window.Loading(true);
    alert("Save Failed");
    asyncSend = null;
}
function GetHTMLAsyncDone() {
    if (asyncHTML) {
        if (typeof (asyncHTMLDone) != "undefined") {
            asyncHTMLDone(asyncHTML.getXMLString());
        }
        asyncHTML = null;
    }
    if (window.Loading) window.Loading(true);
}
function GetHTMLErr() {
    asyncHTML = null;
    if (window.Loading) window.Loading(true);
}
function GetHTMLAsync(query, donefunc, errfunc) {
    if (!asyncHTML) {
        if (window.Loading) window.Loading();
        asyncHTMLDone = donefunc;
        asyncHTMLErrFunc = errfunc;
        asyncHTML = new XmlLoader(GetURL(query));
        asyncHTML.onerror = GetHTMLErr;
        asyncHTML.ontimeout = GetHTMLErr;
        asyncHTML.onload = GetHTMLAsyncDone;
        asyncHTML.sendGet();
    } else {
        var next = new asyncNext(query, donefunc, errfunc);
        next.addNext();
    }
}
function GetFirstItem(firstitem, itemid, up) {
    var item = firstitem;
    while (item) {
        if (IsE(item, itemid)) break;
        item = (up) ? item.previousSibling : item.nextSibling;
    }
    return item;
}
