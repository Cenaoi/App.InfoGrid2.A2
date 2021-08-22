/// <reference path="../../EaselJs/easeljs-NEXT.combined.js" />
/// <reference path="../../jquery/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../Mini2/dev/Mini.js" />

Mini2.define('Mini2.flow.Event', {


    bind: function (eventName, fun, data, owner) {
        "use strict";
        var me = this,
            evtSet = me.eventSet = me.eventSet || {},
            evts = evtSet[eventName] = evtSet[eventName] || []

        evts.push({
            fun: fun,
            owner: owner,
            data: data
        });

    },



    onPre: function (eventName, data) {
        var me = this;

        me.on(eventName, data);
    },

    on: function (eventName, data) {
        "use strict";

        if (!this.eventSet) {
            return;
        }


        var me = this,
            i,
            evt,
            fun,
            evtData,
            evtSet = me.eventSet,
            evts = evtSet[eventName] || []

        for (i = 0; i < evts.length; i++) {

            evt = evts[i];

            fun = evt.fun;

            if (evt.data) {
                evtData = Mini2.clone(evt.data);
            }
            else {
                evtData = {};
            }


            evtData = Mini2.applyIf(evtData, data)

            fun.call(evt.owner || me, me, evtData);

        }

    },


});