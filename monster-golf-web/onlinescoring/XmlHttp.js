// #pragma NoCompStart
/**
	* Copyright (c) 2003-2013 DocuSign Incorporated
	* All rights reserved.
	
	* The information contained herein is confidential and proprietary to
	* Docusign Incorporated, and considered a trade secret
	* as defined under civil and criminal statutes.  Docusign
	* Incorporated shall pursue its civil and criminal remedies in the event
	* of unauthorized use or misappropriation of its trade secrets.  Use
	* of this information by anyone other than authorized employees of
	* Docusign Incorporated is granted only under a written
	* non-disclosure agreement, expressly prescribing the scope and manner
	* of such use.
**/
// #pragma NoCompEnd

var XmlLoaderCount = 0;

//-- XmlLoader --------------------------------------------------
function XmlLoader(u) {
    // hooks for the user
    this.onload = donothing;
    this.onerror = donothing;
    this.ontimeout = donothing;

    // the following variables can be changed before the sendGet is called
    this.url = u;
    this.timeoutSeconds = 120;

    // private variables
    var running = false;
    var timeoutId = null;
    var THIS = this;

    // you can change these, but I wouldn't
    this.sendGet = mySendGet;
    this.sendPost = mySendPost;
    this.sendDOMPost = mySendDOMPost;
    this.sendGetSynchronous = mySendGetSynchronous;
    this.sendPostSynchronous = mySendPostSynchronous;
    this.halt = myHalt;
    this.getResponseXML = myGetResponseXML;
    this.getXMLString = myGetXMLString;

    // this is a hook for adding diagnostics
    this.tracer = null;
    var id = XmlLoaderCount++;

    getXmlHttpRequest();

    // implementation functions
    function getXmlHttpRequest() {
        //cs: also needed to change this to add a check  for IE 7 since it now has native xmlhttp 
        if (currBrowserVer > -1) {
            //do IE stuff
            THIS.xml = new IEXmlLoader();
        }
        else {
            //do Mozilla stuff
            THIS.xml = new MoXmlLoader();
        }
    }

    function mySendGet() {
        if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendGet ' + this.url);
        myHalt();

        if (THIS.xml == null) getXmlHttpRequest();
        THIS.xml.request.open('GET', this.url, true);
        THIS.xml.request.onreadystatechange = myProcessReqChange;

        running = true;
        timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000));

        THIS.xml.sendGet();
    }

    function mySendGetSynchronous() {
        if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendGet ' + this.url);
        myHalt();

        if (THIS.xml == null) getXmlHttpRequest();
        THIS.xml.request.open('GET', this.url, false);
        THIS.xml.request.onreadystatechange = myProcessReqChange;

        running = true;
        timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000));

        THIS.xml.sendGet();
        myProcessReqChange();
    }

    function mySendPostSynchronous(contentType, contentData) {
        if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendPost ' + this.url);
        myHalt();

        if (THIS.xml == null) getXmlHttpRequest();
        THIS.xml.request.open('POST', this.url, false);
        THIS.xml.request.setRequestHeader("Content-type", contentType);
        if (THIS.xml.ie) {
            THIS.xml.request.setRequestHeader("Content-length", contentData.length);
            THIS.xml.request.setRequestHeader("Connection", "close");
        }
        THIS.xml.request.onreadystatechange = myProcessReqChange;

        running = true;
        timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000));

        THIS.xml.sendPost(contentData);
        myProcessReqChange();
    }

    function mySendDOMPost(contentDOM) {
        var content = null;
        if (contentDOM.innerHTML) {
            content = "<" + contentDOM.tagName + ">";
            content += contentDOM.innerHTML;
            content += "</" + contentDOM.tagName + ">";
        }
        else if (typeof (XMLSerializer) != "undefined") content = (new XMLSerializer()).serializeToString(contentDOM);
        else if (contentDOM.xml) content = contentDOM.xml;
        if (content) {
            if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendPost ' + this.url);
            myHalt();

            if (THIS.xml == null) getXmlHttpRequest();
            THIS.xml.request.open('POST', THIS.url, true);
            THIS.xml.request.setRequestHeader("Content-type", "text/xml");
            if (THIS.xml.ie) {
                THIS.xml.request.setRequestHeader("Content-length", content.length);
                THIS.xml.request.setRequestHeader("Connection", "close");
            }
            THIS.xml.request.onreadystatechange = myProcessReqChange;

            running = true;
            timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000));
            THIS.xml.sendPost(content);
        }
    }
    function mySendPost(contentType, contentData, contentHeaders) {
        if (THIS.tracer) THIS.tracer.trace(stamp() + 'mySendPost ' + this.url);
        myHalt();

        if (THIS.xml == null) getXmlHttpRequest();
        THIS.xml.request.open('POST', THIS.url, true);
        // content type, examples: "application/x-www-form-urlencoded", "text/xml"
        THIS.xml.request.setRequestHeader("Content-type", contentType);
        if (contentHeaders && contentHeaders.length > 0) {
            for (var x = 0; x < contentHeaders.length; x++) {
                THIS.xml.request.setRequestHeader(contentHeaders[x][0], contentHeaders[x][1]);
            }
        }
        if (THIS.xml.ie) {
            THIS.xml.request.setRequestHeader("Content-length", contentData.length);
            THIS.xml.request.setRequestHeader("Connection", "close");
        }
        THIS.xml.request.onreadystatechange = myProcessReqChange;

        running = true;
        timeoutId = setTimeout(myTimeout, (this.timeoutSeconds * 1000));
        THIS.xml.sendPost(contentData);
    }

    function myGetResponseXML() {
        if (THIS.xml && THIS.xml.request) return THIS.xml.request.responseXML;
        else return "";
    }
    function myGetXMLString() {
        var content = "";
        var respXML = myGetResponseXML();
        // user currBrowserVer check for non-IE as IE9 beta shows undefined interface error in IE9 standard mode
        if (respXML && typeof(XMLSerializer) != "undefined" && currBrowserVer == -1) content = (new XMLSerializer()).serializeToString(respXML);
        else if (THIS.xml.request.responseText) content = THIS.xml.request.responseText;
        else if (respXML.xml) content = respXML.xml;
        return content;
    }

    function myHalt() {
        if (THIS.tracer) THIS.tracer.trace(stamp() + 'myHalt running=' + running);
        if (running) {
            stopTimer();

            // I'm getting some funny errors in Mo after I abort. I think it's because there's a delay after you
            // issue the abort before it takes effect. The only workaround I've found is to
            // create a new xmlhttprequest for each abort. -roby 2005/06/23
            if (THIS.xml) {
                var oldXml = THIS.xml;
                THIS.xml = null;
                oldXml.request.onreadystatechange = function () { };
                oldXml.request.abort();
            }
        }
        else {
            stopTimer();
        }
        // do nothing else; halted requests are just abandoned
    }


    function myTimeout() {
        if (THIS.tracer) THIS.tracer.trace(stamp() + 'myTimeout');
        myHalt();
        THIS.ontimeout();
    }

    var check200LoadedCount = 0;

    function my200FullyLoaded() {
        if (THIS.xml && THIS.xml.request && (THIS.xml.request.responseXML || THIS.xml.request.responseText)) THIS.onload();
        else {
            if (check200LoadedCount < 30) {
                check200LoadedCount++;
                setTimeout(my200FullyLoaded, 500);
            } else THIS.onerror();
        }
    }

    function myProcessReqChange() {
        if (!running) {
            if (THIS.tracer) THIS.tracer.trace(stamp() + 'myProcessReqChange (not running)');
            // not running, do nothing
        }
        else if (THIS.xml && THIS.xml.request && THIS.xml.request.readyState != 4) {
            if (THIS.tracer) THIS.tracer.trace(stamp() + 'myProcessReqChange (not done) ' + THIS.xml.request.readyState);
            // not done, wait...
        }
        else if (THIS.xml && THIS.xml.request && (THIS.xml.request.status == 200 || THIS.xml.request.status == 201)) {
            if (THIS.tracer) THIS.tracer.trace(stamp() + 'myProcessReqChange (success) ' + THIS.xml.request.status);
            stopTimer();
            my200FullyLoaded();
        }
        else {
            if (THIS.tracer) THIS.tracer.trace(stamp() + 'myProcessReqChange (error)');
            stopTimer();
            THIS.onerror();
        }
    }

    function stopTimer() {
        if (THIS.tracer) THIS.tracer.trace(stamp() + 'stopTimer');
        running = false;
        if (timeoutId != null) {
            clearTimeout(timeoutId);
            timeoutId = null;
        }
    }


    function stamp() {
        return new Date() + " [xl" + id + "] ";
    }

    function donothing() {
        // do... well... nothing
    }
}

// --- the following methods are for getting the results

XmlLoader.prototype.getResponse = function () {
    if (loader && loader.xml && loader.xml.request) return loader.xml.request.responseXML;
    else return null;
};


XmlLoader.prototype.selectSingleNode = function (xp) {
    if (this && this.xml && typeof (this.xml.selectSingleNode) != "undefined") return this.xml.selectSingleNode(xp);
    else return null;
};


XmlLoader.prototype.selectNodes = function (xp) {
    if (this && this.xml && typeof (this.xml.selectNodes) != "undefined") return this.xml.selectNodes(xp);
    else return null;
};


XmlLoader.prototype.toRightCase = function (s) {
    if (this && this.xml) return this.xml.toRightCase(s);
    else return null;
};

//-- IE -----------------------------------------------------
function IEXmlLoader() {
    this.ie = true;
    if (window.XMLHttpRequest) {
        //use the native xmlhttp object if available
        this.request = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        this.request = new ActiveXObject("Microsoft.XMLHTTP");
    }
}


IEXmlLoader.prototype.sendGet = function () {
    this.request.send();
};

IEXmlLoader.prototype.sendPost = function (content) {
    this.request.send(content);
};

IEXmlLoader.prototype.selectSingleNode = function (xp) {
    if (this && this.request && this.request.responseXML && typeof (this.request.responseXML.selectSingleNode) != "undefined") return this.request.responseXML.selectSingleNode(xp);
    else return null;
};


IEXmlLoader.prototype.selectNodes = function (xp) {
    if (this && this.request && this.request.responseXML && typeof (this.request.responseXML.selectNodes) != "undefined") return this.request.responseXML.selectNodes(xp);
    else return null;
};


IEXmlLoader.prototype.toRightCase = function (s) {
    return s;
};

//-- Mo -----------------------------------------------------
function MoXmlLoader() {
    this.ie = false;
    if (window.XMLHttpRequest) {
        //use the native xmlhttp object if available
        this.request = new XMLHttpRequest();
    }
    else if (window.ActiveXObject) {
        this.request = new ActiveXObject("Microsoft.XMLHTTP");
    }
    this.wrapper = new MOXmlWrapper('');
}


MoXmlLoader.prototype.sendGet = function () {
    this.request.send(null);
};

MoXmlLoader.prototype.sendPost = function (content) {
    this.request.send(content);
};

MoXmlLoader.prototype.selectSingleNode = function (xp) {
    if (this && this.request && this.request.responseXML) {
        this.wrapper.xml = this.request.responseXML;
        return this.wrapper.selectSingleNode(xp);
    } else return null;
};


MoXmlLoader.prototype.selectNodes = function (xp) {
    if (this && this.request && this.request.responseXML) {
        this.wrapper.xml = this.request.responseXML;
        return this.wrapper.selectNodes(xp);
    } else return null;
};


MoXmlLoader.prototype.toRightCase = function (s) {
    if (this && this.wrapper) return this.wrapper.toRightCase(s);
    else return null;
};


//-- XML Wrapper -----------------------------------------------------
// this wrapper is a utility because the IE XML methods are a lot
// friendlier, tho non-standard, compared to Mozilla

//cs:added browser sniffing code to allow for the inclusion of IE 7 +  and the native xmlhttp object
var currBrowserVer = -1; // Return value assumes failure- non IE browser
if (navigator.appName == 'Microsoft Internet Explorer') {
    var ua = navigator.userAgent;
    var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
    if (re.exec(ua) != null)
        currBrowserVer = parseFloat(RegExp.$1);
}
//end browser sniffer
function XmlWrapper(id) {
    //cs:changed the conditionals here because IE 7 now has the native XMLHttp object as well and we were misdetecing it for Mozilla
    if (currBrowserVer > -1) {
        //do IE stuff
        this.wrapper = new IEXmlWrapper(id);
    }
    else {
        //do Mozilla stuff
        this.wrapper = new MOXmlWrapper(id);
    }
}


XmlWrapper.prototype.selectSingleNode = function (xpath) {
    if (this && this.wrapper && typeof (this.wrapper.selectSingleNode) != "undefined") return this.wrapper.selectSingleNode(xpath);
    else return null;
};


XmlWrapper.prototype.selectNodes = function (xpath) {
    if (this && this.wrapper && typeof (this.wrapper.selectNodes) != "undefined") return this.wrapper.selectNodes(xpath);
    else return null;
};


XmlWrapper.prototype.toRightCase = function (id) {
    if (this && this.wrapper) return this.wrapper.toRightCase(id);
    else return null;
};



function XmlWrapperFromXml(textXml) {
    //cs:changed the conditionals here because IE 7 now has the native XMLHttp object as well and we were misdetecing it for Mozilla
    if (currBrowserVer > -1) {
        //do IE stuff
        this.wrapper = new IEXmlWrapperFromXml(textXml);
    }
    else {
        //do Mozilla stuff
        this.wrapper = new MOXmlWrapperFromXml(textXml);
    }
}



XmlWrapperFromXml.prototype.selectSingleNode = function (xpath) {
    if (this && this.wrapper && typeof (this.wrapper.selectSingleNode) != "undefined") return this.wrapper.selectSingleNode(xpath);
    else return null;
};


XmlWrapperFromXml.prototype.selectNodes = function (xpath) {
    if (this && this.wrapper && typeof (this.wrapper.selectNodes) != "undefined") return this.wrapper.selectNodes(xpath);
    else return null;
};


XmlWrapperFromXml.prototype.toRightCase = function (id) {
    if (this && this.wrapper) return this.wrapper.toRightCase(id);
    else return null;
};


//-- XML Wrapper IE -----------------------------------------------------
function IEXmlWrapper(id) {
    this.xml = document.getElementById(id);
}

IEXmlWrapper.prototype.selectSingleNode = function (xpath) {
    if (this && this.xml && typeof (this.xml.selectSingleNode) != "undefined") return this.xml.selectSingleNode(xpath);
    else return null;
};


IEXmlWrapper.prototype.selectNodes = function (xpath) {
    if (this && this.xml && typeof (this.xml.selectNodes) != "undefined") return this.xml.selectNodes(xpath);
    else return null;
};


IEXmlWrapper.prototype.toRightCase = function (id) {
    return id;
};



function IEXmlWrapperFromXml(textXml) {
    this.xml = new ActiveXObject("Microsoft.XMLDOM");
    this.xml.async = "false";
    this.xml.loadXML(textXml);
}


IEXmlWrapperFromXml.prototype.selectSingleNode = function (xpath) {
    if (this && this.xml && typeof (this.xml.selectSingleNode) != "undefined") return this.xml.selectSingleNode(xpath);
    else return null;
};


IEXmlWrapperFromXml.prototype.selectNodes = function (xpath) {
    if (this && this.xml && typeof (this.xml.selectNodes) != "undefined") return this.xml.selectNodes(xpath);
    else return null;
};


IEXmlWrapperFromXml.prototype.toRightCase = function (id) {
    return id;
};


//-- XML Wrapper MO -----------------------------------------------------
function MOXmlWrapper(id) {
    if (id != null && id != '') {
        var n = document.getElementById(id);
        var p = new DOMParser();
        var sxml = n.innerHTML;
        if (sxml.indexOf("&nbsp;") != -1) sxml = sxml.replace(/&nbsp;/g, "&amp;nbsp;");
        this.xml = p.parseFromString(sxml, 'text/xml');
        this.makeLowercase = true;
    }
    else {
        this.makeLowercase = false;
    }
}


/* selectSingleNode and selectNodes does not work on Android phones
    Need to write a function to traverse this.request.responseXML
    as a DOM document using getElementsByTagName */
MOXmlWrapper.prototype.selectSingleNode = function (xpath) {
    var x = (this.makeLowercase) ? xpath.toLowerCase() : xpath;
    if (this.xml && this.xml.evaluate) {
        var result = this.xml.evaluate(x, this.xml, null, 9, null);
        if (result) return result.singleNodeValue;
    }
    return null;
};


MOXmlWrapper.prototype.selectNodes = function (xpath) {
    var x = this.toRightCase(xpath);
    if (typeof (XPathEvaluator) != "undefined") {
        var e = new XPathEvaluator;
        if (typeof (e.evaluate) != "undefined") {
            var result = e.evaluate(x, this.xml.documentElement, null, XPathResult.ORDERED_NODE_ITERATOR_TYPE, null);
            var a = new Array();
            var n;
            while ((n = result.iterateNext())) a[a.length] = n;
            return a;
        }
    }
    return null;
};


MOXmlWrapper.prototype.toRightCase = function (id) {
    return (this.makeLowercase) ? id.toLowerCase() : id;
};

// diagnostic functions
function intro(o) {
    var s = '';
    if (o) {
        for (p in o) s += ' ' + p;
    }
    return s;
}


function MOXmlWrapperFromXml(textXml) {
    var parser = new DOMParser();
    this.xml = parser.parseFromString(textXml, "text/xml");
}



/* selectSingleNode and selectNodes does not work on Android phones
    Need to write a function to traverse this.request.responseXML
    as a DOM document using getElementsByTagName */
MOXmlWrapperFromXml.prototype.selectSingleNode = function (xpath) {
    var x = (this.makeLowercase) ? xpath.toLowerCase() : xpath;
    if (this.xml && this.xml.evaluate) {
        var result = this.xml.evaluate(x, this.xml, null, 9, null);
        if (result) return result.singleNodeValue;
    }
    return null;
};


MOXmlWrapperFromXml.prototype.selectNodes = function (xpath) {
    var x = this.toRightCase(xpath);
    if (typeof (XPathEvaluator) != "undefined") {
        var e = new XPathEvaluator;
        if (typeof (e.evaluate) != "undefined") {
            var result = e.evaluate(x, this.xml.documentElement, null, XPathResult.ORDERED_NODE_ITERATOR_TYPE, null);
            var a = new Array();
            var n;
            while ((n = result.iterateNext())) a[a.length] = n;
            return a;
        }
    }
    return null;
};


MOXmlWrapperFromXml.prototype.toRightCase = function (id) {
    return (this.makeLowercase) ? id.toLowerCase() : id;
};

// diagnostic functions
function intro(o) {
    var s = '';
    if (o) {
        for (p in o) s += ' ' + p;
    }
    return s;
}



function WindowTracer() {
    this.auto = (document.location.href.indexOf('trace=auto') > -1);
    this.clear();
}

WindowTracer.prototype.trace = function (s) {
    this.history += s;
    this.history += '\n';
};

WindowTracer.prototype.clear = function () {
    this.history = '';
};


function SpanTracer(s) {
    this.textSpan = s;
    this.clear();
}


SpanTracer.prototype.trace = function (s) {
    this.textSpan.innerHTML = s;
};


SpanTracer.prototype.clear = function () {
    this.textSpan.innerHTML = '';
};

function GetURLTimeStamp() {
    var d = new Date();
    ts = escape(d.getTime().toString());
    var t = "&ts=" + ts;
    return t;
}

function xDom(xmlObj) {
    var xDOM = null;
    if (xmlObj.xml && xmlObj.xml.request) xDOM = xmlObj.xml.request.responseXML;
    else if (xmlObj.xml && !xmlObj.xml.request) xDOM = xmlObj.xml;
    else if (!xmlObj.xml && xmlObj.wrapper && xmlObj.wrapper.xml) xDOM = xmlObj.wrapper.xml;
    return xDOM;
}
function SingleNode(xp, tagname, xmlObj) {
    var node = null;
    if (typeof (xmlObj.selectSingleNode) != "undefined") node = xmlObj.selectSingleNode(xp);
    if (!node) {
        node = xSelectNodes(xp, tagname, xmlObj);
        if (node && node.length > 0) node = node[0];
    }
    return node;
}
function SingleNodeT(xp, tagname, xmlObj) {
    var nodetext = "";
    var node = SingleNode(xp, tagname, xmlObj);
    if (node) {
        if (node.nodeValue) nodetext = node.nodeValue;
        else if (node.innerHTML) nodetext = node.innerHTML;
        else if (node.textContent) nodetext = node.textContent;
    }
    return nodetext;
}
function xSelectNodes(xp, tagname, xmlObj) {
    var nodes = null;
    if (typeof (xmlObj.selectNodes) != "undefined") nodes = xmlObj.selectNodes(xp);
    if (!nodes) {
        var xDOM = xDom(xmlObj);
        if (xDOM && typeof (xDOM.getElementsByTagName) != "undefined") {
            nodes = xDOM.getElementsByTagName(tagname);
        }
    }
    return nodes;
}

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
var xmlurl = "ScoresXml.aspx";
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
    SendForm(formdata, q, updone, updone, "application/x-www-form-urlencoded", null);
}
function SendFormType(formdata, q, updone, uperr, contenttype, headers) {
    if (formdata != "") {
        if (!asyncSend) {
            if (document.forms[0] && document.forms[0].tourneyId) {
                formdata += "&tourney=" + document.forms[0].tourneyId.value;
            }
            if (document.forms[0] && document.forms[0].roundNum) {
                formdata += "&round=" + document.forms[0].roundNum.value;
            }
            if (window.Loading) window.Loading();
            asyncSend = new XmlLoader(GetURL(q));
            asyncSendDone = updone;
            asyncSendErr = uperr;
            asyncSend.onload = SendFormDone;
            asyncSend.timeout = SendFormErr;
            asyncSend.onerror = SendFormErr;
            asyncSend.sendPost(contenttype, formdata, headers);
        } else {
            var next = new asyncNext(q, updone);
            next.formdata = formdata;
            next.errfunc = uperr;
            next.addNextForm();
        }
    }
}
function SendFormDone() {
    if (typeof (asyncSendDone) == "function") asyncSendDone(asyncSend.getXMLString());
    SendFormClr();
}
function SendFormErr() {
    if (typeof (asyncSendErr) == "function") asyncSendErr();
    else alert("Save Failed");
    SendFormClr();
}
function SendFormClr() {
    if (window.Loading) window.Loading(true);
    asyncSend = null;
    asyncSendDone = null;
    asyncSendErr = null;
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
function AlterResult(html) {
    alert(html.replace("<r>", "").replace("</r>", ""));
}
