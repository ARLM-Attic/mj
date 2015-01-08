﻿var timerlen = 5;
var slideAniLen = 500;

var timerID = new Array();
var startTime = new Array();
var obj = new Array();
var endHeight = new Array();
var moving = new Array();
var dir = new Array();

function slidedown(objname) {
    if (moving[objname])
        return;

    if (document.getElementById(objname).style.display != "none")
        return; // cannot slide down something that is already visible

    moving[objname] = true;
    dir[objname] = "down";
    startslide(objname);
    var element = document.getElementById('UP');
    element.style.display = 'block';
    element = document.getElementById('Down');
    element.style.display = 'none';
    document.Form1.SelOpen2.value = "O";
}

function slideup(objname) {
    if (moving[objname])
        return;

    if (document.getElementById(objname).style.display == "none")
        return; // cannot slide up something that is already hidden

    moving[objname] = true;
    dir[objname] = "up";
    startslide(objname);
    var element = document.getElementById('Down');
    element.style.display = 'block';
    element = document.getElementById('UP');
    element.style.display = 'none';
    document.Form1.SelOpen2.value = "";

}

function startslide(objname) {
    obj[objname] = document.getElementById(objname);

    endHeight[objname] = parseInt(obj[objname].style.height);
    startTime[objname] = (new Date()).getTime();

    if (dir[objname] == "down") {
        obj[objname].style.height = "1px";
        obj[objname].style.display = "block";
    }

    timerID[objname] = setInterval('slidetick(\'' + objname + '\');', timerlen);

    if (dir[objname] == "up") {
        obj[objname].style.display = "none";
    }
}

function slidetick(objname) {
    var elapsed = (new Date()).getTime() - startTime[objname];

    if (elapsed > slideAniLen)
        endSlide(objname);
    else {
        var d = Math.round(elapsed / slideAniLen * endHeight[objname]);
        if (dir[objname] == "up")
            d = endHeight[objname] - d;

        obj[objname].style.height = d + "px";
    }

    return;
}

function endSlide(objname) {
    clearInterval(timerID[objname]);

    if (dir[objname] == "up")
    
        obj[objname].style.display = "none";

    obj[objname].style.height = endHeight[objname] + "px";

    delete (moving[objname]);
    delete (timerID[objname]);
    delete (startTime[objname]);
    delete (endHeight[objname]);
    delete (obj[objname]);
    delete (dir[objname]);

    return;
}

var divWidth = '';
var divHeight = '';
var txtFirstButton = 'OK';
var txtSecondButton = 'Cancel';

function DisplayCalendar(divId) {
    // Set default dialogbox width if null

    var divObj = document.getElementById(divId);
    var btnId = "btn" + divId;

    if (divObj.style.display == 'block') {
        divObj.style.display = 'none';
        document.Form1.SelOpen2.value = '';
    }
    else {
        divWidth = 180;

        // Set default dialogBox height if null

        divHeight = 90;

        // Set dialogbox height and width
        SetHeightWidth(divObj);
        // Set dialogbox top and left
        SetTopLeft(divObj, btnId);

        // Show the div layer
        divObj.style.display = 'block';
        // Change the location and reset the width and height if window is resized
        window.onresize = function () {
            if (divObj.style.display == 'block') {
                SetTopLeft(divObj, btnId);
                SetHeightWidth(divObj);
            }
        };
        document.Form1.SelOpen2.value = divId;
    }
}

function SetTopLeft(divLayer, btnId) {
    // Get the dialogbox height
    var divHeightPer = divLayer.style.height.split('px')[0];

    // Set the top variable
    var top = (parseInt(document.body.offsetHeight) / 2) - (divHeightPer / 2);
    // Get the dialog box width
    var divWidthPix = divLayer.style.width.split('px')[0];

    // Get the left variable
    var left = (parseInt(document.body.offsetWidth) / 2) - (parseInt(divWidthPix) / 2);
    // set the dialogbox position to abosulute
    divLayer.style.position = 'absolute';

    var CalButton = document.getElementById(btnId);

    // Set the div top to the height
    divLayer.style.top = CalButton.style.top + 143;

    // Set the div Left to the height
    divLayer.style.left = CalButton.style.left + 280;
}

function SetHeightWidth(divLayer) {
    // Set the dialogbox width
    divLayer.style.width = divWidth + 'px';
    // Set the dialogbox Height
    divLayer.style.height = divHeight + 'px';
}
         