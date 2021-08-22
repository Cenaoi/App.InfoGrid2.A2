/***********************************************
* Floating image script- By Virtual_Max (http://www.geocities.com/siliconvalley/lakes/8620)
* Modified by Dynamic Drive for various improvements
* Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
***********************************************/


var vmin = 1;
var vmax = 3;
var vr = 2;
var timer1;

function iecompattest() {
    return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
}

function Chip(chipname, width, height) {
    this.named = chipname;
    this.vx = vmin + vmax * Math.random();
    this.vy = vmin + vmax * Math.random();
    this.w = width ;
    this.h = height;
    this.xx = window.innerWidth * Math.random();
    this.yy = window.innerWidth * Math.random();
    this.timer1 = null;

    this.stoped = false;
}

function movechip(chipname) {
    if (document.getElementById) {
        eval("chip=" + chipname);

        if (window.innerWidth || window.opera) {
            pageX = window.pageXOffset - 100;
            pageW = window.innerWidth + 150;
            pageY = window.pageYOffset - 100;
            pageH = window.innerHeight + 150;

        }
        else if (document.body) {
            pageX = iecompattest().scrollLeft;
            pageW = iecompattest().offsetWidth - 40;
            pageY = iecompattest().scrollTop;
            pageH = iecompattest().offsetHeight - 20;
        }

        if (chip.stoped) {
            return;
        }

        chip.xx = chip.xx + chip.vx;
        chip.yy = chip.yy + chip.vy;

        chip.vx += vr * (Math.random() - 0.5);
        chip.vy += vr * (Math.random() - 0.5);
        if (chip.vx > (vmax + vmin)) chip.vx = (vmax + vmin) * 2 - chip.vx;
        if (chip.vx < (-vmax - vmin)) chip.vx = (-vmax - vmin) * 2 - chip.vx;
        if (chip.vy > (vmax + vmin)) chip.vy = (vmax + vmin) * 2 - chip.vy;
        if (chip.vy < (-vmax - vmin)) chip.vy = (-vmax - vmin) * 2 - chip.vy;

        if (chip.xx <= pageX) {
            chip.xx = pageX;
            chip.vx = vmin + vmax * Math.random();
        }
        if (chip.xx >= pageX + pageW - chip.w) {
            chip.xx = pageX + pageW - chip.w;
            chip.vx = -vmin - vmax * Math.random();
        }
        if (chip.yy <= pageY) {
            chip.yy = pageY;
            chip.vy = vmin + vmax * Math.random();
        }
        if (chip.yy >= pageY + pageH - chip.h) {
            chip.yy = pageY + pageH - chip.h;
            chip.vy = -vmin - vmax * Math.random();
        }

        var chipEl = document.getElementById(chip.named);

        //有这个节点吃执行
        if (chipEl) {

            var elStyle = chipEl.style;

            elStyle.left = chip.xx + "px";
            elStyle.top = chip.yy + "px";


            chip.timer1 = setTimeout("movechip('" + chip.named + "')", 50);
        }
    }
}