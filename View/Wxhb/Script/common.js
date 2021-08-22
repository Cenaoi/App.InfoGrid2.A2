
//加载srcipt
function loadScript(url, callback) {


    var script = document.createElement("script");
    script.type = "text/javascript";
    script.onload = function (e) {
        if (callback) {
            callback(e);
        }
    };

    script.src = url;

    var head = document.head || document.getElementsByTagName("head")[0];

    head.appendChild(script);
};

//加载样式
function loadCss(url) {

    var css = document.createElement("link");
    css.rel = "stylesheet";

    css.href = url;

    var head = document.head || document.getElementsByTagName("head")[0];

    head.appendChild(css);


}

//加载style样式数据
function loadStyle(text) {

    var style_a = document.createElement("style");

    style_a.textContent += text;

    var head = document.head || document.getElementsByTagName("head")[0];


    head.appendChild(style_a);



}


loadCss("/Core/Scripts/SUI/sm.css");

loadCss("/Core/Scripts/SUI/sm-extend.min.css");

loadCss("/Core/Scripts/webuploader-0.1.5/webuploader.css");




//加载主要srcipt
loadScript("/Core/Scripts/m5/M5.min.js", function () {


    loadScript("/Core/Scripts/vue/vue-2.0.1.js", function () {

        loadScript("/Core/Scripts/moment/moment-2.9.js");
        loadScript("/Core/Scripts/XYF/xyfUtil.js");


        //     Zepto.js
        //     (c) 2010-2016 Thomas Fuchs
        //     Zepto.js may be freely distributed under the MIT license.

        (function ($) {
            // Create a collection of callbacks to be fired in a sequence, with configurable behaviour
            // Option flags:
            //   - once: Callbacks fired at most one time.
            //   - memory: Remember the most recent context and arguments
            //   - stopOnFalse: Cease iterating over callback list
            //   - unique: Permit adding at most one instance of the same callback
            $.Callbacks = function (options) {
                options = $.extend({}, options)

                var memory, // Last fire value (for non-forgettable lists)
                    fired,  // Flag to know if list was already fired
                    firing, // Flag to know if list is currently firing
                    firingStart, // First callback to fire (used internally by add and fireWith)
                    firingLength, // End of the loop when firing
                    firingIndex, // Index of currently firing callback (modified by remove if needed)
                    list = [], // Actual callback list
                    stack = !options.once && [], // Stack of fire calls for repeatable lists
                    fire = function (data) {
                        memory = options.memory && data
                        fired = true
                        firingIndex = firingStart || 0
                        firingStart = 0
                        firingLength = list.length
                        firing = true
                        for (; list && firingIndex < firingLength ; ++firingIndex) {
                            if (list[firingIndex].apply(data[0], data[1]) === false && options.stopOnFalse) {
                                memory = false
                                break
                            }
                        }
                        firing = false
                        if (list) {
                            if (stack) stack.length && fire(stack.shift())
                            else if (memory) list.length = 0
                            else Callbacks.disable()
                        }
                    },

                    Callbacks = {
                        add: function () {
                            if (list) {
                                var start = list.length,
                                    add = function (args) {
                                        $.each(args, function (_, arg) {
                                            if (typeof arg === "function") {
                                                if (!options.unique || !Callbacks.has(arg)) list.push(arg)
                                            }
                                            else if (arg && arg.length && typeof arg !== 'string') add(arg)
                                        })
                                    }
                                add(arguments)
                                if (firing) firingLength = list.length
                                else if (memory) {
                                    firingStart = start
                                    fire(memory)
                                }
                            }
                            return this
                        },
                        remove: function () {
                            if (list) {
                                $.each(arguments, function (_, arg) {
                                    var index
                                    while ((index = $.inArray(arg, list, index)) > -1) {
                                        list.splice(index, 1)
                                        // Handle firing indexes
                                        if (firing) {
                                            if (index <= firingLength)--firingLength
                                            if (index <= firingIndex)--firingIndex
                                        }
                                    }
                                })
                            }
                            return this
                        },
                        has: function (fn) {
                            return !!(list && (fn ? $.inArray(fn, list) > -1 : list.length))
                        },
                        empty: function () {
                            firingLength = list.length = 0
                            return this
                        },
                        disable: function () {
                            list = stack = memory = undefined
                            return this
                        },
                        disabled: function () {
                            return !list
                        },
                        lock: function () {
                            stack = undefined
                            if (!memory) Callbacks.disable()
                            return this
                        },
                        locked: function () {
                            return !stack
                        },
                        fireWith: function (context, args) {
                            if (list && (!fired || stack)) {
                                args = args || []
                                args = [context, args.slice ? args.slice() : args]
                                if (firing) stack.push(args)
                                else fire(args)
                            }
                            return this
                        },
                        fire: function () {
                            return Callbacks.fireWith(this, arguments)
                        },
                        fired: function () {
                            return !!fired
                        }
                    }

                return Callbacks
            }
        })(Zepto)

        //     Zepto.js
        //     (c) 2010-2016 Thomas Fuchs
        //     Zepto.js may be freely distributed under the MIT license.
        //
        //     Some code (c) 2005, 2013 jQuery Foundation, Inc. and other contributors

        ; (function ($) {
            var slice = Array.prototype.slice

            function Deferred(func) {
                var tuples = [
                      // action, add listener, listener list, final state
                      ["resolve", "done", $.Callbacks({ once: 1, memory: 1 }), "resolved"],
                      ["reject", "fail", $.Callbacks({ once: 1, memory: 1 }), "rejected"],
                      ["notify", "progress", $.Callbacks({ memory: 1 })]
                ],
                    state = "pending",
                    promise = {
                        state: function () {
                            return state
                        },
                        always: function () {
                            deferred.done(arguments).fail(arguments)
                            return this
                        },
                        then: function (/* fnDone [, fnFailed [, fnProgress]] */) {
                            var fns = arguments
                            return Deferred(function (defer) {
                                $.each(tuples, function (i, tuple) {
                                    var fn = $.isFunction(fns[i]) && fns[i]
                                    deferred[tuple[1]](function () {
                                        var returned = fn && fn.apply(this, arguments)
                                        if (returned && $.isFunction(returned.promise)) {
                                            returned.promise()
                                              .done(defer.resolve)
                                              .fail(defer.reject)
                                              .progress(defer.notify)
                                        } else {
                                            var context = this === promise ? defer.promise() : this,
                                                values = fn ? [returned] : arguments
                                            defer[tuple[0] + "With"](context, values)
                                        }
                                    })
                                })
                                fns = null
                            }).promise()
                        },

                        promise: function (obj) {
                            return obj != null ? $.extend(obj, promise) : promise
                        }
                    },
                    deferred = {}

                $.each(tuples, function (i, tuple) {
                    var list = tuple[2],
                        stateString = tuple[3]

                    promise[tuple[1]] = list.add

                    if (stateString) {
                        list.add(function () {
                            state = stateString
                        }, tuples[i ^ 1][2].disable, tuples[2][2].lock)
                    }

                    deferred[tuple[0]] = function () {
                        deferred[tuple[0] + "With"](this === deferred ? promise : this, arguments)
                        return this
                    }
                    deferred[tuple[0] + "With"] = list.fireWith
                })

                promise.promise(deferred)
                if (func) func.call(deferred, deferred)
                return deferred
            }

            $.when = function (sub) {
                var resolveValues = slice.call(arguments),
                    len = resolveValues.length,
                    i = 0,
                    remain = len !== 1 || (sub && $.isFunction(sub.promise)) ? len : 0,
                    deferred = remain === 1 ? sub : Deferred(),
                    progressValues, progressContexts, resolveContexts,
                    updateFn = function (i, ctx, val) {
                        return function (value) {
                            ctx[i] = this
                            val[i] = arguments.length > 1 ? slice.call(arguments) : value
                            if (val === progressValues) {
                                deferred.notifyWith(ctx, val)
                            } else if (!(--remain)) {
                                deferred.resolveWith(ctx, val)
                            }
                        }
                    }

                if (len > 1) {
                    progressValues = new Array(len)
                    progressContexts = new Array(len)
                    resolveContexts = new Array(len)
                    for (; i < len; ++i) {
                        if (resolveValues[i] && $.isFunction(resolveValues[i].promise)) {
                            resolveValues[i].promise()
                              .done(updateFn(i, resolveContexts, resolveValues))
                              .fail(deferred.reject)
                              .progress(updateFn(i, progressContexts, progressValues))
                        } else {
                            --remain
                        }
                    }
                }
                if (!remain) deferred.resolveWith(resolveContexts, resolveValues)
                return deferred.promise()
            }

            $.Deferred = Deferred
        })(Zepto)

        loadScript("/Core/Scripts/webuploader-0.1.5/webuploader.js");



    });

    


});

//加载微信js
loadScript("http://res.wx.qq.com/open/js/jweixin-1.0.0.js");

//加载高德地图脚本
loadScript("http://webapi.amap.com/maps?v=1.3&key=5f8a5ddf2ff1ff6633742b80b9f55e4a");




//加载图片预览组件 
window.addEventListener("load", function () {


    'use strict';


    ; (function ($) {
        "use strict";
        var Swiper = function (container, params) {
            // if (!(this instanceof Swiper)) return new Swiper(container, params);
            var defaults = this.defaults;
            var initalVirtualTranslate = params && params.virtualTranslate;

            params = params || {};
            for (var def in defaults) {
                if (typeof params[def] === 'undefined') {
                    params[def] = defaults[def];
                }
                else if (typeof params[def] === 'object') {
                    for (var deepDef in defaults[def]) {
                        if (typeof params[def][deepDef] === 'undefined') {
                            params[def][deepDef] = defaults[def][deepDef];
                        }
                    }
                }
            }

            // Swiper
            var s = this;

            // Params
            s.params = params;

            // Classname
            s.classNames = [];

            // Export it to Swiper instance
            s.$ = $;
            /*=========================
              Preparation - Define Container, Wrapper and Pagination
              ===========================*/
            s.container = $(container);
            if (s.container.length === 0) return;
            if (s.container.length > 1) {
                s.container.each(function () {
                    new $.Swiper(this, params);
                });
                return;
            }

            // Save instance in container HTML Element and in data
            s.container[0].swiper = s;
            s.container.data('swiper', s);

            s.classNames.push('swiper-container-' + s.params.direction);

            if (s.params.freeMode) {
                s.classNames.push('swiper-container-free-mode');
            }
            if (!s.support.flexbox) {
                s.classNames.push('swiper-container-no-flexbox');
                s.params.slidesPerColumn = 1;
            }
            // Enable slides progress when required
            if (s.params.parallax || s.params.watchSlidesVisibility) {
                s.params.watchSlidesProgress = true;
            }
            // Coverflow / 3D
            if (['cube', 'coverflow'].indexOf(s.params.effect) >= 0) {
                if (s.support.transforms3d) {
                    s.params.watchSlidesProgress = true;
                    s.classNames.push('swiper-container-3d');
                }
                else {
                    s.params.effect = 'slide';
                }
            }
            if (s.params.effect !== 'slide') {
                s.classNames.push('swiper-container-' + s.params.effect);
            }
            if (s.params.effect === 'cube') {
                s.params.resistanceRatio = 0;
                s.params.slidesPerView = 1;
                s.params.slidesPerColumn = 1;
                s.params.slidesPerGroup = 1;
                s.params.centeredSlides = false;
                s.params.spaceBetween = 0;
                s.params.virtualTranslate = true;
                s.params.setWrapperSize = false;
            }
            if (s.params.effect === 'fade') {
                s.params.slidesPerView = 1;
                s.params.slidesPerColumn = 1;
                s.params.slidesPerGroup = 1;
                s.params.watchSlidesProgress = true;
                s.params.spaceBetween = 0;
                if (typeof initalVirtualTranslate === 'undefined') {
                    s.params.virtualTranslate = true;
                }
            }

            // Grab Cursor
            if (s.params.grabCursor && s.support.touch) {
                s.params.grabCursor = false;
            }

            // Wrapper
            s.wrapper = s.container.children('.' + s.params.wrapperClass);

            // Pagination
            if (s.params.pagination) {
                s.paginationContainer = $(s.params.pagination);
                if (s.params.paginationClickable) {
                    s.paginationContainer.addClass('swiper-pagination-clickable');
                }
            }

            // Is Horizontal
            function isH() {
                return s.params.direction === 'horizontal';
            }

            // RTL
            s.rtl = isH() && (s.container[0].dir.toLowerCase() === 'rtl' || s.container.css('direction') === 'rtl');
            if (s.rtl) {
                s.classNames.push('swiper-container-rtl');
            }

            // Wrong RTL support
            if (s.rtl) {
                s.wrongRTL = s.wrapper.css('display') === '-webkit-box';
            }

            // Columns
            if (s.params.slidesPerColumn > 1) {
                s.classNames.push('swiper-container-multirow');
            }

            // Check for Android
            if (s.device.android) {
                s.classNames.push('swiper-container-android');
            }

            // Add classes
            s.container.addClass(s.classNames.join(' '));

            // Translate
            s.translate = 0;

            // Progress
            s.progress = 0;

            // Velocity
            s.velocity = 0;

            // Locks, unlocks
            s.lockSwipeToNext = function () {
                s.params.allowSwipeToNext = false;
            };
            s.lockSwipeToPrev = function () {
                s.params.allowSwipeToPrev = false;
            };
            s.lockSwipes = function () {
                s.params.allowSwipeToNext = s.params.allowSwipeToPrev = false;
            };
            s.unlockSwipeToNext = function () {
                s.params.allowSwipeToNext = true;
            };
            s.unlockSwipeToPrev = function () {
                s.params.allowSwipeToPrev = true;
            };
            s.unlockSwipes = function () {
                s.params.allowSwipeToNext = s.params.allowSwipeToPrev = true;
            };


            /*=========================
              Set grab cursor
              ===========================*/
            if (s.params.grabCursor) {
                s.container[0].style.cursor = 'move';
                s.container[0].style.cursor = '-webkit-grab';
                s.container[0].style.cursor = '-moz-grab';
                s.container[0].style.cursor = 'grab';
            }
            /*=========================
              Update on Images Ready
              ===========================*/
            s.imagesToLoad = [];
            s.imagesLoaded = 0;

            s.loadImage = function (imgElement, src, checkForComplete, callback) {
                var image;
                function onReady() {
                    if (callback) callback();
                }
                if (!imgElement.complete || !checkForComplete) {
                    if (src) {
                        image = new Image();
                        image.onload = onReady;
                        image.onerror = onReady;
                        image.src = src;
                    } else {
                        onReady();
                    }

                } else {//image already loaded...
                    onReady();
                }
            };
            s.preloadImages = function () {
                s.imagesToLoad = s.container.find('img');
                function _onReady() {
                    if (typeof s === 'undefined' || s === null) return;
                    if (s.imagesLoaded !== undefined) s.imagesLoaded++;
                    if (s.imagesLoaded === s.imagesToLoad.length) {
                        if (s.params.updateOnImagesReady) s.update();
                        s.emit('onImagesReady', s);
                    }
                }
                for (var i = 0; i < s.imagesToLoad.length; i++) {
                    s.loadImage(s.imagesToLoad[i], (s.imagesToLoad[i].currentSrc || s.imagesToLoad[i].getAttribute('src')), true, _onReady);
                }
            };

            /*=========================
              Autoplay
              ===========================*/
            s.autoplayTimeoutId = undefined;
            s.autoplaying = false;
            s.autoplayPaused = false;
            function autoplay() {
                s.autoplayTimeoutId = setTimeout(function () {
                    if (s.params.loop) {
                        s.fixLoop();
                        s._slideNext();
                    }
                    else {
                        if (!s.isEnd) {
                            s._slideNext();
                        }
                        else {
                            if (!params.autoplayStopOnLast) {
                                s._slideTo(0);
                            }
                            else {
                                s.stopAutoplay();
                            }
                        }
                    }
                }, s.params.autoplay);
            }
            s.startAutoplay = function () {
                if (typeof s.autoplayTimeoutId !== 'undefined') return false;
                if (!s.params.autoplay) return false;
                if (s.autoplaying) return false;
                s.autoplaying = true;
                s.emit('onAutoplayStart', s);
                autoplay();
            };
            s.stopAutoplay = function () {
                if (!s.autoplayTimeoutId) return;
                if (s.autoplayTimeoutId) clearTimeout(s.autoplayTimeoutId);
                s.autoplaying = false;
                s.autoplayTimeoutId = undefined;
                s.emit('onAutoplayStop', s);
            };
            s.pauseAutoplay = function (speed) {
                if (s.autoplayPaused) return;
                if (s.autoplayTimeoutId) clearTimeout(s.autoplayTimeoutId);
                s.autoplayPaused = true;
                if (speed === 0) {
                    s.autoplayPaused = false;
                    autoplay();
                }
                else {
                    s.wrapper.transitionEnd(function () {
                        s.autoplayPaused = false;
                        if (!s.autoplaying) {
                            s.stopAutoplay();
                        }
                        else {
                            autoplay();
                        }
                    });
                }
            };
            /*=========================
              Min/Max Translate
              ===========================*/
            s.minTranslate = function () {
                return (-s.snapGrid[0]);
            };
            s.maxTranslate = function () {
                return (-s.snapGrid[s.snapGrid.length - 1]);
            };
            /*=========================
              Slider/slides sizes
              ===========================*/
            s.updateContainerSize = function () {
                s.width = s.container[0].clientWidth;
                s.height = s.container[0].clientHeight;
                s.size = isH() ? s.width : s.height;
            };

            s.updateSlidesSize = function () {
                s.slides = s.wrapper.children('.' + s.params.slideClass);
                s.snapGrid = [];
                s.slidesGrid = [];
                s.slidesSizesGrid = [];

                var spaceBetween = s.params.spaceBetween,
                    slidePosition = 0,
                    i,
                    prevSlideSize = 0,
                    index = 0;
                if (typeof spaceBetween === 'string' && spaceBetween.indexOf('%') >= 0) {
                    spaceBetween = parseFloat(spaceBetween.replace('%', '')) / 100 * s.size;
                }

                s.virtualSize = -spaceBetween;
                // reset margins
                if (s.rtl) s.slides.css({ marginLeft: '', marginTop: '' });
                else s.slides.css({ marginRight: '', marginBottom: '' });

                var slidesNumberEvenToRows;
                if (s.params.slidesPerColumn > 1) {
                    if (Math.floor(s.slides.length / s.params.slidesPerColumn) === s.slides.length / s.params.slidesPerColumn) {
                        slidesNumberEvenToRows = s.slides.length;
                    }
                    else {
                        slidesNumberEvenToRows = Math.ceil(s.slides.length / s.params.slidesPerColumn) * s.params.slidesPerColumn;
                    }
                }

                // Calc slides
                var slideSize;
                for (i = 0; i < s.slides.length; i++) {
                    slideSize = 0;
                    var slide = s.slides.eq(i);
                    if (s.params.slidesPerColumn > 1) {
                        // Set slides order
                        var newSlideOrderIndex;
                        var column, row;
                        var slidesPerColumn = s.params.slidesPerColumn;
                        var slidesPerRow;
                        if (s.params.slidesPerColumnFill === 'column') {
                            column = Math.floor(i / slidesPerColumn);
                            row = i - column * slidesPerColumn;
                            newSlideOrderIndex = column + row * slidesNumberEvenToRows / slidesPerColumn;
                            slide
                                .css({
                                    '-webkit-box-ordinal-group': newSlideOrderIndex,
                                    '-moz-box-ordinal-group': newSlideOrderIndex,
                                    '-ms-flex-order': newSlideOrderIndex,
                                    '-webkit-order': newSlideOrderIndex,
                                    'order': newSlideOrderIndex
                                });
                        }
                        else {
                            slidesPerRow = slidesNumberEvenToRows / slidesPerColumn;
                            row = Math.floor(i / slidesPerRow);
                            column = i - row * slidesPerRow;

                        }
                        slide
                            .css({
                                'margin-top': (row !== 0 && s.params.spaceBetween) && (s.params.spaceBetween + 'px')
                            })
                            .attr('data-swiper-column', column)
                            .attr('data-swiper-row', row);

                    }
                    if (slide.css('display') === 'none') continue;
                    if (s.params.slidesPerView === 'auto') {
                        slideSize = isH() ? slide.outerWidth(true) : slide.outerHeight(true);
                    }
                    else {
                        slideSize = (s.size - (s.params.slidesPerView - 1) * spaceBetween) / s.params.slidesPerView;
                        if (isH()) {
                            s.slides[i].style.width = slideSize + 'px';
                        }
                        else {
                            s.slides[i].style.height = slideSize + 'px';
                        }
                    }
                    s.slides[i].swiperSlideSize = slideSize;
                    s.slidesSizesGrid.push(slideSize);


                    if (s.params.centeredSlides) {
                        slidePosition = slidePosition + slideSize / 2 + prevSlideSize / 2 + spaceBetween;
                        if (i === 0) slidePosition = slidePosition - s.size / 2 - spaceBetween;
                        if (Math.abs(slidePosition) < 1 / 1000) slidePosition = 0;
                        if ((index) % s.params.slidesPerGroup === 0) s.snapGrid.push(slidePosition);
                        s.slidesGrid.push(slidePosition);
                    }
                    else {
                        if ((index) % s.params.slidesPerGroup === 0) s.snapGrid.push(slidePosition);
                        s.slidesGrid.push(slidePosition);
                        slidePosition = slidePosition + slideSize + spaceBetween;
                    }

                    s.virtualSize += slideSize + spaceBetween;

                    prevSlideSize = slideSize;

                    index++;
                }
                s.virtualSize = Math.max(s.virtualSize, s.size);

                var newSlidesGrid;

                if (
                    s.rtl && s.wrongRTL && (s.params.effect === 'slide' || s.params.effect === 'coverflow')) {
                    s.wrapper.css({ width: s.virtualSize + s.params.spaceBetween + 'px' });
                }
                if (!s.support.flexbox || s.params.setWrapperSize) {
                    if (isH()) s.wrapper.css({ width: s.virtualSize + s.params.spaceBetween + 'px' });
                    else s.wrapper.css({ height: s.virtualSize + s.params.spaceBetween + 'px' });
                }

                if (s.params.slidesPerColumn > 1) {
                    s.virtualSize = (slideSize + s.params.spaceBetween) * slidesNumberEvenToRows;
                    s.virtualSize = Math.ceil(s.virtualSize / s.params.slidesPerColumn) - s.params.spaceBetween;
                    s.wrapper.css({ width: s.virtualSize + s.params.spaceBetween + 'px' });
                    if (s.params.centeredSlides) {
                        newSlidesGrid = [];
                        for (i = 0; i < s.snapGrid.length; i++) {
                            if (s.snapGrid[i] < s.virtualSize + s.snapGrid[0]) newSlidesGrid.push(s.snapGrid[i]);
                        }
                        s.snapGrid = newSlidesGrid;
                    }
                }

                // Remove last grid elements depending on width
                if (!s.params.centeredSlides) {
                    newSlidesGrid = [];
                    for (i = 0; i < s.snapGrid.length; i++) {
                        if (s.snapGrid[i] <= s.virtualSize - s.size) {
                            newSlidesGrid.push(s.snapGrid[i]);
                        }
                    }
                    s.snapGrid = newSlidesGrid;
                    if (Math.floor(s.virtualSize - s.size) > Math.floor(s.snapGrid[s.snapGrid.length - 1])) {
                        s.snapGrid.push(s.virtualSize - s.size);
                    }
                }
                if (s.snapGrid.length === 0) s.snapGrid = [0];

                if (s.params.spaceBetween !== 0) {
                    if (isH()) {
                        if (s.rtl) s.slides.css({ marginLeft: spaceBetween + 'px' });
                        else s.slides.css({ marginRight: spaceBetween + 'px' });
                    }
                    else s.slides.css({ marginBottom: spaceBetween + 'px' });
                }
                if (s.params.watchSlidesProgress) {
                    s.updateSlidesOffset();
                }
            };
            s.updateSlidesOffset = function () {
                for (var i = 0; i < s.slides.length; i++) {
                    s.slides[i].swiperSlideOffset = isH() ? s.slides[i].offsetLeft : s.slides[i].offsetTop;
                }
            };

            /*=========================
              Slider/slides progress
              ===========================*/
            s.updateSlidesProgress = function (translate) {
                if (typeof translate === 'undefined') {
                    translate = s.translate || 0;
                }
                if (s.slides.length === 0) return;
                if (typeof s.slides[0].swiperSlideOffset === 'undefined') s.updateSlidesOffset();

                var offsetCenter = s.params.centeredSlides ? -translate + s.size / 2 : -translate;
                if (s.rtl) offsetCenter = s.params.centeredSlides ? translate - s.size / 2 : translate;

                // Visible Slides
                s.slides.removeClass(s.params.slideVisibleClass);
                for (var i = 0; i < s.slides.length; i++) {
                    var slide = s.slides[i];
                    var slideCenterOffset = (s.params.centeredSlides === true) ? slide.swiperSlideSize / 2 : 0;
                    var slideProgress = (offsetCenter - slide.swiperSlideOffset - slideCenterOffset) / (slide.swiperSlideSize + s.params.spaceBetween);
                    if (s.params.watchSlidesVisibility) {
                        var slideBefore = -(offsetCenter - slide.swiperSlideOffset - slideCenterOffset);
                        var slideAfter = slideBefore + s.slidesSizesGrid[i];
                        var isVisible =
                            (slideBefore >= 0 && slideBefore < s.size) ||
                            (slideAfter > 0 && slideAfter <= s.size) ||
                            (slideBefore <= 0 && slideAfter >= s.size);
                        if (isVisible) {
                            s.slides.eq(i).addClass(s.params.slideVisibleClass);
                        }
                    }
                    slide.progress = s.rtl ? -slideProgress : slideProgress;
                }
            };
            s.updateProgress = function (translate) {
                if (typeof translate === 'undefined') {
                    translate = s.translate || 0;
                }
                var translatesDiff = s.maxTranslate() - s.minTranslate();
                if (translatesDiff === 0) {
                    s.progress = 0;
                    s.isBeginning = s.isEnd = true;
                }
                else {
                    s.progress = (translate - s.minTranslate()) / (translatesDiff);
                    s.isBeginning = s.progress <= 0;
                    s.isEnd = s.progress >= 1;
                }
                if (s.isBeginning) s.emit('onReachBeginning', s);
                if (s.isEnd) s.emit('onReachEnd', s);

                if (s.params.watchSlidesProgress) s.updateSlidesProgress(translate);
                s.emit('onProgress', s, s.progress);
            };
            s.updateActiveIndex = function () {
                var translate = s.rtl ? s.translate : -s.translate;
                var newActiveIndex, i, snapIndex;
                for (i = 0; i < s.slidesGrid.length; i++) {
                    if (typeof s.slidesGrid[i + 1] !== 'undefined') {
                        if (translate >= s.slidesGrid[i] && translate < s.slidesGrid[i + 1] - (s.slidesGrid[i + 1] - s.slidesGrid[i]) / 2) {
                            newActiveIndex = i;
                        }
                        else if (translate >= s.slidesGrid[i] && translate < s.slidesGrid[i + 1]) {
                            newActiveIndex = i + 1;
                        }
                    }
                    else {
                        if (translate >= s.slidesGrid[i]) {
                            newActiveIndex = i;
                        }
                    }
                }
                // Normalize slideIndex
                if (newActiveIndex < 0 || typeof newActiveIndex === 'undefined') newActiveIndex = 0;
                // for (i = 0; i < s.slidesGrid.length; i++) {
                // if (- translate >= s.slidesGrid[i]) {
                // newActiveIndex = i;
                // }
                // }
                snapIndex = Math.floor(newActiveIndex / s.params.slidesPerGroup);
                if (snapIndex >= s.snapGrid.length) snapIndex = s.snapGrid.length - 1;

                if (newActiveIndex === s.activeIndex) {
                    return;
                }
                s.snapIndex = snapIndex;
                s.previousIndex = s.activeIndex;
                s.activeIndex = newActiveIndex;
                s.updateClasses();
            };

            /*=========================
              Classes
              ===========================*/
            s.updateClasses = function () {
                s.slides.removeClass(s.params.slideActiveClass + ' ' + s.params.slideNextClass + ' ' + s.params.slidePrevClass);
                var activeSlide = s.slides.eq(s.activeIndex);
                // Active classes
                activeSlide.addClass(s.params.slideActiveClass);
                activeSlide.next('.' + s.params.slideClass).addClass(s.params.slideNextClass);
                activeSlide.prev('.' + s.params.slideClass).addClass(s.params.slidePrevClass);

                // Pagination
                if (s.bullets && s.bullets.length > 0) {
                    s.bullets.removeClass(s.params.bulletActiveClass);
                    var bulletIndex;
                    if (s.params.loop) {
                        bulletIndex = Math.ceil(s.activeIndex - s.loopedSlides) / s.params.slidesPerGroup;
                        if (bulletIndex > s.slides.length - 1 - s.loopedSlides * 2) {
                            bulletIndex = bulletIndex - (s.slides.length - s.loopedSlides * 2);
                        }
                        if (bulletIndex > s.bullets.length - 1) bulletIndex = bulletIndex - s.bullets.length;
                    }
                    else {
                        if (typeof s.snapIndex !== 'undefined') {
                            bulletIndex = s.snapIndex;
                        }
                        else {
                            bulletIndex = s.activeIndex || 0;
                        }
                    }
                    if (s.paginationContainer.length > 1) {
                        s.bullets.each(function () {
                            if ($(this).index() === bulletIndex) $(this).addClass(s.params.bulletActiveClass);
                        });
                    }
                    else {
                        s.bullets.eq(bulletIndex).addClass(s.params.bulletActiveClass);
                    }
                }

                // Next/active buttons
                if (!s.params.loop) {
                    if (s.params.prevButton) {
                        if (s.isBeginning) {
                            $(s.params.prevButton).addClass(s.params.buttonDisabledClass);
                            if (s.params.a11y && s.a11y) s.a11y.disable($(s.params.prevButton));
                        }
                        else {
                            $(s.params.prevButton).removeClass(s.params.buttonDisabledClass);
                            if (s.params.a11y && s.a11y) s.a11y.enable($(s.params.prevButton));
                        }
                    }
                    if (s.params.nextButton) {
                        if (s.isEnd) {
                            $(s.params.nextButton).addClass(s.params.buttonDisabledClass);
                            if (s.params.a11y && s.a11y) s.a11y.disable($(s.params.nextButton));
                        }
                        else {
                            $(s.params.nextButton).removeClass(s.params.buttonDisabledClass);
                            if (s.params.a11y && s.a11y) s.a11y.enable($(s.params.nextButton));
                        }
                    }
                }
            };

            /*=========================
              Pagination
              ===========================*/
            s.updatePagination = function () {
                if (!s.params.pagination) return;
                if (s.paginationContainer && s.paginationContainer.length > 0) {
                    var bulletsHTML = '';
                    var numberOfBullets = s.params.loop ? Math.ceil((s.slides.length - s.loopedSlides * 2) / s.params.slidesPerGroup) : s.snapGrid.length;
                    for (var i = 0; i < numberOfBullets; i++) {
                        if (s.params.paginationBulletRender) {
                            bulletsHTML += s.params.paginationBulletRender(i, s.params.bulletClass);
                        }
                        else {
                            bulletsHTML += '<span class="' + s.params.bulletClass + '"></span>';
                        }
                    }
                    s.paginationContainer.html(bulletsHTML);
                    s.bullets = s.paginationContainer.find('.' + s.params.bulletClass);
                }
            };
            /*=========================
              Common update method
              ===========================*/
            s.update = function (updateTranslate) {
                s.updateContainerSize();
                s.updateSlidesSize();
                s.updateProgress();
                s.updatePagination();
                s.updateClasses();
                if (s.params.scrollbar && s.scrollbar) {
                    s.scrollbar.set();
                }
                function forceSetTranslate() {
                    newTranslate = Math.min(Math.max(s.translate, s.maxTranslate()), s.minTranslate());
                    s.setWrapperTranslate(newTranslate);
                    s.updateActiveIndex();
                    s.updateClasses();
                }
                if (updateTranslate) {
                    var translated, newTranslate;
                    if (s.params.freeMode) {
                        forceSetTranslate();
                    }
                    else {
                        if (s.params.slidesPerView === 'auto' && s.isEnd && !s.params.centeredSlides) {
                            translated = s.slideTo(s.slides.length - 1, 0, false, true);
                        }
                        else {
                            translated = s.slideTo(s.activeIndex, 0, false, true);
                        }
                        if (!translated) {
                            forceSetTranslate();
                        }
                    }

                }
            };

            /*=========================
              Resize Handler
              ===========================*/
            s.onResize = function () {
                s.updateContainerSize();
                s.updateSlidesSize();
                s.updateProgress();
                if (s.params.slidesPerView === 'auto' || s.params.freeMode) s.updatePagination();
                if (s.params.scrollbar && s.scrollbar) {
                    s.scrollbar.set();
                }
                if (s.params.freeMode) {
                    var newTranslate = Math.min(Math.max(s.translate, s.maxTranslate()), s.minTranslate());
                    s.setWrapperTranslate(newTranslate);
                    s.updateActiveIndex();
                    s.updateClasses();
                }
                else {
                    s.updateClasses();
                    if (s.params.slidesPerView === 'auto' && s.isEnd && !s.params.centeredSlides) {
                        s.slideTo(s.slides.length - 1, 0, false, true);
                    }
                    else {
                        s.slideTo(s.activeIndex, 0, false, true);
                    }
                }

            };

            /*=========================
              Events
              ===========================*/

            //Define Touch Events
            var desktopEvents = ['mousedown', 'mousemove', 'mouseup'];
            if (window.navigator.pointerEnabled) desktopEvents = ['pointerdown', 'pointermove', 'pointerup'];
            else if (window.navigator.msPointerEnabled) desktopEvents = ['MSPointerDown', 'MSPointerMove', 'MSPointerUp'];
            s.touchEvents = {
                start: s.support.touch || !s.params.simulateTouch ? 'touchstart' : desktopEvents[0],
                move: s.support.touch || !s.params.simulateTouch ? 'touchmove' : desktopEvents[1],
                end: s.support.touch || !s.params.simulateTouch ? 'touchend' : desktopEvents[2]
            };


            // WP8 Touch Events Fix
            if (window.navigator.pointerEnabled || window.navigator.msPointerEnabled) {
                (s.params.touchEventsTarget === 'container' ? s.container : s.wrapper).addClass('swiper-wp8-' + s.params.direction);
            }

            // Attach/detach events
            s.initEvents = function (detach) {
                var actionDom = detach ? 'off' : 'on';
                var action = detach ? 'removeEventListener' : 'addEventListener';
                var touchEventsTarget = s.params.touchEventsTarget === 'container' ? s.container[0] : s.wrapper[0];
                var target = s.support.touch ? touchEventsTarget : document;

                var moveCapture = s.params.nested ? true : false;

                //Touch Events
                if (s.browser.ie) {
                    touchEventsTarget[action](s.touchEvents.start, s.onTouchStart, false);
                    target[action](s.touchEvents.move, s.onTouchMove, moveCapture);
                    target[action](s.touchEvents.end, s.onTouchEnd, false);
                }
                else {
                    if (s.support.touch) {
                        touchEventsTarget[action](s.touchEvents.start, s.onTouchStart, false);
                        touchEventsTarget[action](s.touchEvents.move, s.onTouchMove, moveCapture);
                        touchEventsTarget[action](s.touchEvents.end, s.onTouchEnd, false);
                    }
                    if (params.simulateTouch && !s.device.ios && !s.device.android) {
                        touchEventsTarget[action]('mousedown', s.onTouchStart, false);
                        target[action]('mousemove', s.onTouchMove, moveCapture);
                        target[action]('mouseup', s.onTouchEnd, false);
                    }
                }
                window[action]('resize', s.onResize);

                // Next, Prev, Index
                if (s.params.nextButton) {
                    $(s.params.nextButton)[actionDom]('click', s.onClickNext);
                    if (s.params.a11y && s.a11y) $(s.params.nextButton)[actionDom]('keydown', s.a11y.onEnterKey);
                }
                if (s.params.prevButton) {
                    $(s.params.prevButton)[actionDom]('click', s.onClickPrev);
                    if (s.params.a11y && s.a11y) $(s.params.prevButton)[actionDom]('keydown', s.a11y.onEnterKey);
                }
                if (s.params.pagination && s.params.paginationClickable) {
                    $(s.paginationContainer)[actionDom]('click', '.' + s.params.bulletClass, s.onClickIndex);
                }

                // Prevent Links Clicks
                if (s.params.preventClicks || s.params.preventClicksPropagation) touchEventsTarget[action]('click', s.preventClicks, true);
            };
            s.attachEvents = function () {
                s.initEvents();
            };
            s.detachEvents = function () {
                s.initEvents(true);
            };

            /*=========================
              Handle Clicks
              ===========================*/
            // Prevent Clicks
            s.allowClick = true;
            s.preventClicks = function (e) {
                if (!s.allowClick) {
                    if (s.params.preventClicks) e.preventDefault();
                    if (s.params.preventClicksPropagation) {
                        e.stopPropagation();
                        e.stopImmediatePropagation();
                    }
                }
            };
            // Clicks
            s.onClickNext = function (e) {
                e.preventDefault();
                s.slideNext();
            };
            s.onClickPrev = function (e) {
                e.preventDefault();
                s.slidePrev();
            };
            s.onClickIndex = function (e) {
                e.preventDefault();
                var index = $(this).index() * s.params.slidesPerGroup;
                if (s.params.loop) index = index + s.loopedSlides;
                s.slideTo(index);
            };

            /*=========================
              Handle Touches
              ===========================*/
            function findElementInEvent(e, selector) {
                var el = $(e.target);
                if (!el.is(selector)) {
                    if (typeof selector === 'string') {
                        el = el.parents(selector);
                    }
                    else if (selector.nodeType) {
                        var found;
                        el.parents().each(function (index, _el) {
                            if (_el === selector) found = selector;
                        });
                        if (!found) return undefined;
                        else return selector;
                    }
                }
                if (el.length === 0) {
                    return undefined;
                }
                return el[0];
            }
            s.updateClickedSlide = function (e) {
                var slide = findElementInEvent(e, '.' + s.params.slideClass);
                if (slide) {
                    s.clickedSlide = slide;
                    s.clickedIndex = $(slide).index();
                }
                else {
                    s.clickedSlide = undefined;
                    s.clickedIndex = undefined;
                    return;
                }
                if (s.params.slideToClickedSlide && s.clickedIndex !== undefined && s.clickedIndex !== s.activeIndex) {
                    var slideToIndex = s.clickedIndex,
                        realIndex;
                    if (s.params.loop) {
                        realIndex = $(s.clickedSlide).attr('data-swiper-slide-index');
                        if (slideToIndex > s.slides.length - s.params.slidesPerView) {
                            s.fixLoop();
                            slideToIndex = s.wrapper.children('.' + s.params.slideClass + '[data-swiper-slide-index="' + realIndex + '"]').eq(0).index();
                            setTimeout(function () {
                                s.slideTo(slideToIndex);
                            }, 0);
                        }
                        else if (slideToIndex < s.params.slidesPerView - 1) {
                            s.fixLoop();
                            var duplicatedSlides = s.wrapper.children('.' + s.params.slideClass + '[data-swiper-slide-index="' + realIndex + '"]');
                            slideToIndex = duplicatedSlides.eq(duplicatedSlides.length - 1).index();
                            setTimeout(function () {
                                s.slideTo(slideToIndex);
                            }, 0);
                        }
                        else {
                            s.slideTo(slideToIndex);
                        }
                    }
                    else {
                        s.slideTo(slideToIndex);
                    }
                }
            };

            var isTouched,
                isMoved,
                touchStartTime,
                isScrolling,
                currentTranslate,
                startTranslate,
                allowThresholdMove,
                // Form elements to match
                formElements = 'input, select, textarea, button',
                // Last click time
                lastClickTime = Date.now(), clickTimeout,
                //Velocities
                velocities = [],
                allowMomentumBounce;

            // Animating Flag
            s.animating = false;

            // Touches information
            s.touches = {
                startX: 0,
                startY: 0,
                currentX: 0,
                currentY: 0,
                diff: 0
            };

            // Touch handlers
            var isTouchEvent, startMoving;
            s.onTouchStart = function (e) {
                if (e.originalEvent) e = e.originalEvent;
                isTouchEvent = e.type === 'touchstart';
                if (!isTouchEvent && 'which' in e && e.which === 3) return;
                if (s.params.noSwiping && findElementInEvent(e, '.' + s.params.noSwipingClass)) {
                    s.allowClick = true;
                    return;
                }
                if (s.params.swipeHandler) {
                    if (!findElementInEvent(e, s.params.swipeHandler)) return;
                }
                isTouched = true;
                isMoved = false;
                isScrolling = undefined;
                startMoving = undefined;
                s.touches.startX = s.touches.currentX = e.type === 'touchstart' ? e.targetTouches[0].pageX : e.pageX;
                s.touches.startY = s.touches.currentY = e.type === 'touchstart' ? e.targetTouches[0].pageY : e.pageY;
                touchStartTime = Date.now();
                s.allowClick = true;
                s.updateContainerSize();
                s.swipeDirection = undefined;
                if (s.params.threshold > 0) allowThresholdMove = false;
                if (e.type !== 'touchstart') {
                    var preventDefault = true;
                    if ($(e.target).is(formElements)) preventDefault = false;
                    if (document.activeElement && $(document.activeElement).is(formElements)) {
                        document.activeElement.blur();
                    }
                    if (preventDefault) {
                        e.preventDefault();
                    }
                }
                s.emit('onTouchStart', s, e);
            };

            s.onTouchMove = function (e) {
                if (e.originalEvent) e = e.originalEvent;
                if (isTouchEvent && e.type === 'mousemove') return;
                if (e.preventedByNestedSwiper) return;
                if (s.params.onlyExternal) {
                    isMoved = true;
                    s.allowClick = false;
                    return;
                }
                if (isTouchEvent && document.activeElement) {
                    if (e.target === document.activeElement && $(e.target).is(formElements)) {
                        isMoved = true;
                        s.allowClick = false;
                        return;
                    }
                }

                s.emit('onTouchMove', s, e);

                if (e.targetTouches && e.targetTouches.length > 1) return;

                s.touches.currentX = e.type === 'touchmove' ? e.targetTouches[0].pageX : e.pageX;
                s.touches.currentY = e.type === 'touchmove' ? e.targetTouches[0].pageY : e.pageY;

                if (typeof isScrolling === 'undefined') {
                    var touchAngle = Math.atan2(Math.abs(s.touches.currentY - s.touches.startY), Math.abs(s.touches.currentX - s.touches.startX)) * 180 / Math.PI;
                    isScrolling = isH() ? touchAngle > s.params.touchAngle : (90 - touchAngle > s.params.touchAngle);
                }
                if (isScrolling) {
                    s.emit('onTouchMoveOpposite', s, e);
                }
                if (typeof startMoving === 'undefined' && s.browser.ieTouch) {
                    if (s.touches.currentX !== s.touches.startX || s.touches.currentY !== s.touches.startY) {
                        startMoving = true;
                    }
                }
                if (!isTouched) return;
                if (isScrolling) {
                    isTouched = false;
                    return;
                }
                if (!startMoving && s.browser.ieTouch) {
                    return;
                }
                s.allowClick = false;
                s.emit('onSliderMove', s, e);
                e.preventDefault();
                if (s.params.touchMoveStopPropagation && !s.params.nested) {
                    e.stopPropagation();
                }

                if (!isMoved) {
                    if (params.loop) {
                        s.fixLoop();
                    }
                    startTranslate = s.getWrapperTranslate();
                    s.setWrapperTransition(0);
                    if (s.animating) {
                        s.wrapper.trigger('webkitTransitionEnd transitionend oTransitionEnd MSTransitionEnd msTransitionEnd');
                    }
                    if (s.params.autoplay && s.autoplaying) {
                        if (s.params.autoplayDisableOnInteraction) {
                            s.stopAutoplay();
                        }
                        else {
                            s.pauseAutoplay();
                        }
                    }
                    allowMomentumBounce = false;
                    //Grab Cursor
                    if (s.params.grabCursor) {
                        s.container[0].style.cursor = 'move';
                        s.container[0].style.cursor = '-webkit-grabbing';
                        s.container[0].style.cursor = '-moz-grabbin';
                        s.container[0].style.cursor = 'grabbing';
                    }
                }
                isMoved = true;

                var diff = s.touches.diff = isH() ? s.touches.currentX - s.touches.startX : s.touches.currentY - s.touches.startY;

                diff = diff * s.params.touchRatio;
                if (s.rtl) diff = -diff;

                s.swipeDirection = diff > 0 ? 'prev' : 'next';
                currentTranslate = diff + startTranslate;

                var disableParentSwiper = true;
                if ((diff > 0 && currentTranslate > s.minTranslate())) {
                    disableParentSwiper = false;
                    if (s.params.resistance) currentTranslate = s.minTranslate() - 1 + Math.pow(-s.minTranslate() + startTranslate + diff, s.params.resistanceRatio);
                }
                else if (diff < 0 && currentTranslate < s.maxTranslate()) {
                    disableParentSwiper = false;
                    if (s.params.resistance) currentTranslate = s.maxTranslate() + 1 - Math.pow(s.maxTranslate() - startTranslate - diff, s.params.resistanceRatio);
                }

                if (disableParentSwiper) {
                    e.preventedByNestedSwiper = true;
                }

                // Directions locks
                if (!s.params.allowSwipeToNext && s.swipeDirection === 'next' && currentTranslate < startTranslate) {
                    currentTranslate = startTranslate;
                }
                if (!s.params.allowSwipeToPrev && s.swipeDirection === 'prev' && currentTranslate > startTranslate) {
                    currentTranslate = startTranslate;
                }

                if (!s.params.followFinger) return;

                // Threshold
                if (s.params.threshold > 0) {
                    if (Math.abs(diff) > s.params.threshold || allowThresholdMove) {
                        if (!allowThresholdMove) {
                            allowThresholdMove = true;
                            s.touches.startX = s.touches.currentX;
                            s.touches.startY = s.touches.currentY;
                            currentTranslate = startTranslate;
                            s.touches.diff = isH() ? s.touches.currentX - s.touches.startX : s.touches.currentY - s.touches.startY;
                            return;
                        }
                    }
                    else {
                        currentTranslate = startTranslate;
                        return;
                    }
                }
                // Update active index in free mode
                if (s.params.freeMode || s.params.watchSlidesProgress) {
                    s.updateActiveIndex();
                }
                if (s.params.freeMode) {
                    //Velocity
                    if (velocities.length === 0) {
                        velocities.push({
                            position: s.touches[isH() ? 'startX' : 'startY'],
                            time: touchStartTime
                        });
                    }
                    velocities.push({
                        position: s.touches[isH() ? 'currentX' : 'currentY'],
                        time: (new Date()).getTime()
                    });
                }
                // Update progress
                s.updateProgress(currentTranslate);
                // Update translate
                s.setWrapperTranslate(currentTranslate);
            };
            s.onTouchEnd = function (e) {
                if (e.originalEvent) e = e.originalEvent;
                s.emit('onTouchEnd', s, e);
                if (!isTouched) return;
                //Return Grab Cursor
                if (s.params.grabCursor && isMoved && isTouched) {
                    s.container[0].style.cursor = 'move';
                    s.container[0].style.cursor = '-webkit-grab';
                    s.container[0].style.cursor = '-moz-grab';
                    s.container[0].style.cursor = 'grab';
                }

                // Time diff
                var touchEndTime = Date.now();
                var timeDiff = touchEndTime - touchStartTime;

                // Tap, doubleTap, Click
                if (s.allowClick) {
                    s.updateClickedSlide(e);
                    s.emit('onTap', s, e);
                    if (timeDiff < 300 && (touchEndTime - lastClickTime) > 300) {
                        if (clickTimeout) clearTimeout(clickTimeout);
                        clickTimeout = setTimeout(function () {
                            if (!s) return;
                            if (s.params.paginationHide && s.paginationContainer.length > 0 && !$(e.target).hasClass(s.params.bulletClass)) {
                                s.paginationContainer.toggleClass(s.params.paginationHiddenClass);
                            }
                            s.emit('onClick', s, e);
                        }, 300);

                    }
                    if (timeDiff < 300 && (touchEndTime - lastClickTime) < 300) {
                        if (clickTimeout) clearTimeout(clickTimeout);
                        s.emit('onDoubleTap', s, e);
                    }
                }

                lastClickTime = Date.now();
                setTimeout(function () {
                    if (s && s.allowClick) s.allowClick = true;
                }, 0);

                if (!isTouched || !isMoved || !s.swipeDirection || s.touches.diff === 0 || currentTranslate === startTranslate) {
                    isTouched = isMoved = false;
                    return;
                }
                isTouched = isMoved = false;

                var currentPos;
                if (s.params.followFinger) {
                    currentPos = s.rtl ? s.translate : -s.translate;
                }
                else {
                    currentPos = -currentTranslate;
                }
                if (s.params.freeMode) {
                    if (currentPos < -s.minTranslate()) {
                        s.slideTo(s.activeIndex);
                        return;
                    }
                    else if (currentPos > -s.maxTranslate()) {
                        s.slideTo(s.slides.length - 1);
                        return;
                    }

                    if (s.params.freeModeMomentum) {
                        if (velocities.length > 1) {
                            var lastMoveEvent = velocities.pop(), velocityEvent = velocities.pop();

                            var distance = lastMoveEvent.position - velocityEvent.position;
                            var time = lastMoveEvent.time - velocityEvent.time;
                            s.velocity = distance / time;
                            s.velocity = s.velocity / 2;
                            if (Math.abs(s.velocity) < 0.02) {
                                s.velocity = 0;
                            }
                            // this implies that the user stopped moving a finger then released.
                            // There would be no events with distance zero, so the last event is stale.
                            if (time > 150 || (new Date().getTime() - lastMoveEvent.time) > 300) {
                                s.velocity = 0;
                            }
                        } else {
                            s.velocity = 0;
                        }

                        velocities.length = 0;
                        var momentumDuration = 1000 * s.params.freeModeMomentumRatio;
                        var momentumDistance = s.velocity * momentumDuration;

                        var newPosition = s.translate + momentumDistance;
                        if (s.rtl) newPosition = -newPosition;
                        var doBounce = false;
                        var afterBouncePosition;
                        var bounceAmount = Math.abs(s.velocity) * 20 * s.params.freeModeMomentumBounceRatio;
                        if (newPosition < s.maxTranslate()) {
                            if (s.params.freeModeMomentumBounce) {
                                if (newPosition + s.maxTranslate() < -bounceAmount) {
                                    newPosition = s.maxTranslate() - bounceAmount;
                                }
                                afterBouncePosition = s.maxTranslate();
                                doBounce = true;
                                allowMomentumBounce = true;
                            }
                            else {
                                newPosition = s.maxTranslate();
                            }
                        }
                        if (newPosition > s.minTranslate()) {
                            if (s.params.freeModeMomentumBounce) {
                                if (newPosition - s.minTranslate() > bounceAmount) {
                                    newPosition = s.minTranslate() + bounceAmount;
                                }
                                afterBouncePosition = s.minTranslate();
                                doBounce = true;
                                allowMomentumBounce = true;
                            }
                            else {
                                newPosition = s.minTranslate();
                            }
                        }
                        //Fix duration
                        if (s.velocity !== 0) {
                            if (s.rtl) {
                                momentumDuration = Math.abs((-newPosition - s.translate) / s.velocity);
                            }
                            else {
                                momentumDuration = Math.abs((newPosition - s.translate) / s.velocity);
                            }
                        }

                        if (s.params.freeModeMomentumBounce && doBounce) {
                            s.updateProgress(afterBouncePosition);
                            s.setWrapperTransition(momentumDuration);
                            s.setWrapperTranslate(newPosition);
                            s.onTransitionStart();
                            s.animating = true;
                            s.wrapper.transitionEnd(function () {
                                if (!allowMomentumBounce) return;
                                s.emit('onMomentumBounce', s);

                                s.setWrapperTransition(s.params.speed);
                                s.setWrapperTranslate(afterBouncePosition);
                                s.wrapper.transitionEnd(function () {
                                    s.onTransitionEnd();
                                });
                            });
                        } else if (s.velocity) {
                            s.updateProgress(newPosition);
                            s.setWrapperTransition(momentumDuration);
                            s.setWrapperTranslate(newPosition);
                            s.onTransitionStart();
                            if (!s.animating) {
                                s.animating = true;
                                s.wrapper.transitionEnd(function () {
                                    s.onTransitionEnd();
                                });
                            }

                        } else {
                            s.updateProgress(newPosition);
                        }

                        s.updateActiveIndex();
                    }
                    if (!s.params.freeModeMomentum || timeDiff >= s.params.longSwipesMs) {
                        s.updateProgress();
                        s.updateActiveIndex();
                    }
                    return;
                }

                // Find current slide
                var i, stopIndex = 0, groupSize = s.slidesSizesGrid[0];
                for (i = 0; i < s.slidesGrid.length; i += s.params.slidesPerGroup) {
                    if (typeof s.slidesGrid[i + s.params.slidesPerGroup] !== 'undefined') {
                        if (currentPos >= s.slidesGrid[i] && currentPos < s.slidesGrid[i + s.params.slidesPerGroup]) {
                            stopIndex = i;
                            groupSize = s.slidesGrid[i + s.params.slidesPerGroup] - s.slidesGrid[i];
                        }
                    }
                    else {
                        if (currentPos >= s.slidesGrid[i]) {
                            stopIndex = i;
                            groupSize = s.slidesGrid[s.slidesGrid.length - 1] - s.slidesGrid[s.slidesGrid.length - 2];
                        }
                    }
                }

                // Find current slide size
                var ratio = (currentPos - s.slidesGrid[stopIndex]) / groupSize;

                if (timeDiff > s.params.longSwipesMs) {
                    // Long touches
                    if (!s.params.longSwipes) {
                        s.slideTo(s.activeIndex);
                        return;
                    }
                    if (s.swipeDirection === 'next') {
                        if (ratio >= s.params.longSwipesRatio) s.slideTo(stopIndex + s.params.slidesPerGroup);
                        else s.slideTo(stopIndex);

                    }
                    if (s.swipeDirection === 'prev') {
                        if (ratio > (1 - s.params.longSwipesRatio)) s.slideTo(stopIndex + s.params.slidesPerGroup);
                        else s.slideTo(stopIndex);
                    }
                }
                else {
                    // Short swipes
                    if (!s.params.shortSwipes) {
                        s.slideTo(s.activeIndex);
                        return;
                    }
                    if (s.swipeDirection === 'next') {
                        s.slideTo(stopIndex + s.params.slidesPerGroup);

                    }
                    if (s.swipeDirection === 'prev') {
                        s.slideTo(stopIndex);
                    }
                }
            };
            /*=========================
              Transitions
              ===========================*/
            s._slideTo = function (slideIndex, speed) {
                return s.slideTo(slideIndex, speed, true, true);
            };
            s.slideTo = function (slideIndex, speed, runCallbacks, internal) {
                if (typeof runCallbacks === 'undefined') runCallbacks = true;
                if (typeof slideIndex === 'undefined') slideIndex = 0;
                if (slideIndex < 0) slideIndex = 0;
                s.snapIndex = Math.floor(slideIndex / s.params.slidesPerGroup);
                if (s.snapIndex >= s.snapGrid.length) s.snapIndex = s.snapGrid.length - 1;

                var translate = -s.snapGrid[s.snapIndex];

                // Stop autoplay

                if (s.params.autoplay && s.autoplaying) {
                    if (internal || !s.params.autoplayDisableOnInteraction) {
                        s.pauseAutoplay(speed);
                    }
                    else {
                        s.stopAutoplay();
                    }
                }
                // Update progress
                s.updateProgress(translate);

                // Normalize slideIndex
                for (var i = 0; i < s.slidesGrid.length; i++) {
                    if (-translate >= s.slidesGrid[i]) {
                        slideIndex = i;
                    }
                }

                if (typeof speed === 'undefined') speed = s.params.speed;
                s.previousIndex = s.activeIndex || 0;
                s.activeIndex = slideIndex;

                if (translate === s.translate) {
                    s.updateClasses();
                    return false;
                }
                s.onTransitionStart(runCallbacks);
                if (speed === 0) {
                    s.setWrapperTransition(0);
                    s.setWrapperTranslate(translate);
                    s.onTransitionEnd(runCallbacks);
                }
                else {
                    s.setWrapperTransition(speed);
                    s.setWrapperTranslate(translate);
                    if (!s.animating) {
                        s.animating = true;
                        s.wrapper.transitionEnd(function () {
                            s.onTransitionEnd(runCallbacks);
                        });
                    }

                }
                s.updateClasses();
                return true;
            };

            s.onTransitionStart = function (runCallbacks) {
                if (typeof runCallbacks === 'undefined') runCallbacks = true;
                if (s.lazy) s.lazy.onTransitionStart();
                if (runCallbacks) {
                    s.emit('onTransitionStart', s);
                    if (s.activeIndex !== s.previousIndex) {
                        s.emit('onSlideChangeStart', s);
                    }
                }
            };
            s.onTransitionEnd = function (runCallbacks) {
                s.animating = false;
                s.setWrapperTransition(0);
                if (typeof runCallbacks === 'undefined') runCallbacks = true;
                if (s.lazy) s.lazy.onTransitionEnd();
                if (runCallbacks) {
                    s.emit('onTransitionEnd', s);
                    if (s.activeIndex !== s.previousIndex) {
                        s.emit('onSlideChangeEnd', s);
                    }
                }
                if (s.params.hashnav && s.hashnav) {
                    s.hashnav.setHash();
                }

            };
            s.slideNext = function (runCallbacks, speed, internal) {
                if (s.params.loop) {
                    if (s.animating) return false;
                    s.fixLoop();
                    return s.slideTo(s.activeIndex + s.params.slidesPerGroup, speed, runCallbacks, internal);
                }
                else return s.slideTo(s.activeIndex + s.params.slidesPerGroup, speed, runCallbacks, internal);
            };
            s._slideNext = function (speed) {
                return s.slideNext(true, speed, true);
            };
            s.slidePrev = function (runCallbacks, speed, internal) {
                if (s.params.loop) {
                    if (s.animating) return false;
                    s.fixLoop();
                    return s.slideTo(s.activeIndex - 1, speed, runCallbacks, internal);
                }
                else return s.slideTo(s.activeIndex - 1, speed, runCallbacks, internal);
            };
            s._slidePrev = function (speed) {
                return s.slidePrev(true, speed, true);
            };
            s.slideReset = function (runCallbacks, speed) {
                return s.slideTo(s.activeIndex, speed, runCallbacks);
            };

            /*=========================
              Translate/transition helpers
              ===========================*/
            s.setWrapperTransition = function (duration, byController) {
                s.wrapper.transition(duration);
                if (s.params.effect !== 'slide' && s.effects[s.params.effect]) {
                    s.effects[s.params.effect].setTransition(duration);
                }
                if (s.params.parallax && s.parallax) {
                    s.parallax.setTransition(duration);
                }
                if (s.params.scrollbar && s.scrollbar) {
                    s.scrollbar.setTransition(duration);
                }
                if (s.params.control && s.controller) {
                    s.controller.setTransition(duration, byController);
                }
                s.emit('onSetTransition', s, duration);
            };
            s.setWrapperTranslate = function (translate, updateActiveIndex, byController) {
                var x = 0, y = 0, z = 0;
                if (isH()) {
                    x = s.rtl ? -translate : translate;
                }
                else {
                    y = translate;
                }
                if (!s.params.virtualTranslate) {
                    if (s.support.transforms3d) s.wrapper.transform('translate3d(' + x + 'px, ' + y + 'px, ' + z + 'px)');
                    else s.wrapper.transform('translate(' + x + 'px, ' + y + 'px)');
                }

                s.translate = isH() ? x : y;

                if (updateActiveIndex) s.updateActiveIndex();
                if (s.params.effect !== 'slide' && s.effects[s.params.effect]) {
                    s.effects[s.params.effect].setTranslate(s.translate);
                }
                if (s.params.parallax && s.parallax) {
                    s.parallax.setTranslate(s.translate);
                }
                if (s.params.scrollbar && s.scrollbar) {
                    s.scrollbar.setTranslate(s.translate);
                }
                if (s.params.control && s.controller) {
                    s.controller.setTranslate(s.translate, byController);
                }
                s.emit('onSetTranslate', s, s.translate);
            };

            s.getTranslate = function (el, axis) {
                var matrix, curTransform, curStyle, transformMatrix;

                // automatic axis detection
                if (typeof axis === 'undefined') {
                    axis = 'x';
                }

                if (s.params.virtualTranslate) {
                    return s.rtl ? -s.translate : s.translate;
                }

                curStyle = window.getComputedStyle(el, null);
                if (window.WebKitCSSMatrix) {
                    // Some old versions of Webkit choke when 'none' is passed; pass
                    // empty string instead in this case
                    transformMatrix = new WebKitCSSMatrix(curStyle.webkitTransform === 'none' ? '' : curStyle.webkitTransform);
                }
                else {
                    transformMatrix = curStyle.MozTransform || curStyle.OTransform || curStyle.MsTransform || curStyle.msTransform || curStyle.transform || curStyle.getPropertyValue('transform').replace('translate(', 'matrix(1, 0, 0, 1,');
                    matrix = transformMatrix.toString().split(',');
                }

                if (axis === 'x') {
                    //Latest Chrome and webkits Fix
                    if (window.WebKitCSSMatrix)
                        curTransform = transformMatrix.m41;
                        //Crazy IE10 Matrix
                    else if (matrix.length === 16)
                        curTransform = parseFloat(matrix[12]);
                        //Normal Browsers
                    else
                        curTransform = parseFloat(matrix[4]);
                }
                if (axis === 'y') {
                    //Latest Chrome and webkits Fix
                    if (window.WebKitCSSMatrix)
                        curTransform = transformMatrix.m42;
                        //Crazy IE10 Matrix
                    else if (matrix.length === 16)
                        curTransform = parseFloat(matrix[13]);
                        //Normal Browsers
                    else
                        curTransform = parseFloat(matrix[5]);
                }
                if (s.rtl && curTransform) curTransform = -curTransform;
                return curTransform || 0;
            };
            s.getWrapperTranslate = function (axis) {
                if (typeof axis === 'undefined') {
                    axis = isH() ? 'x' : 'y';
                }
                return s.getTranslate(s.wrapper[0], axis);
            };

            /*=========================
              Observer
              ===========================*/
            s.observers = [];
            function initObserver(target, options) {
                options = options || {};
                // create an observer instance
                var ObserverFunc = window.MutationObserver || window.WebkitMutationObserver;
                var observer = new ObserverFunc(function (mutations) {
                    mutations.forEach(function (mutation) {
                        s.onResize();
                        s.emit('onObserverUpdate', s, mutation);
                    });
                });

                observer.observe(target, {
                    attributes: typeof options.attributes === 'undefined' ? true : options.attributes,
                    childList: typeof options.childList === 'undefined' ? true : options.childList,
                    characterData: typeof options.characterData === 'undefined' ? true : options.characterData
                });

                s.observers.push(observer);
            }
            s.initObservers = function () {
                if (s.params.observeParents) {
                    var containerParents = s.container.parents();
                    for (var i = 0; i < containerParents.length; i++) {
                        initObserver(containerParents[i]);
                    }
                }

                // Observe container
                initObserver(s.container[0], { childList: false });

                // Observe wrapper
                initObserver(s.wrapper[0], { attributes: false });
            };
            s.disconnectObservers = function () {
                for (var i = 0; i < s.observers.length; i++) {
                    s.observers[i].disconnect();
                }
                s.observers = [];
            };
            /*=========================
              Loop
              ===========================*/
            // Create looped slides
            s.createLoop = function () {
                // Remove duplicated slides
                s.wrapper.children('.' + s.params.slideClass + '.' + s.params.slideDuplicateClass).remove();

                var slides = s.wrapper.children('.' + s.params.slideClass);
                s.loopedSlides = parseInt(s.params.loopedSlides || s.params.slidesPerView, 10);
                s.loopedSlides = s.loopedSlides + s.params.loopAdditionalSlides;
                if (s.loopedSlides > slides.length) {
                    s.loopedSlides = slides.length;
                }

                var prependSlides = [], appendSlides = [], i;
                slides.each(function (index, el) {
                    var slide = $(this);
                    if (index < s.loopedSlides) appendSlides.push(el);
                    if (index < slides.length && index >= slides.length - s.loopedSlides) prependSlides.push(el);
                    slide.attr('data-swiper-slide-index', index);
                });
                for (i = 0; i < appendSlides.length; i++) {
                    s.wrapper.append($(appendSlides[i].cloneNode(true)).addClass(s.params.slideDuplicateClass));
                }
                for (i = prependSlides.length - 1; i >= 0; i--) {
                    s.wrapper.prepend($(prependSlides[i].cloneNode(true)).addClass(s.params.slideDuplicateClass));
                }
            };
            s.destroyLoop = function () {
                s.wrapper.children('.' + s.params.slideClass + '.' + s.params.slideDuplicateClass).remove();
                s.slides.removeAttr('data-swiper-slide-index');
            };
            s.fixLoop = function () {
                var newIndex;
                //Fix For Negative Oversliding
                if (s.activeIndex < s.loopedSlides) {
                    newIndex = s.slides.length - s.loopedSlides * 3 + s.activeIndex;
                    newIndex = newIndex + s.loopedSlides;
                    s.slideTo(newIndex, 0, false, true);
                }
                    //Fix For Positive Oversliding
                else if ((s.params.slidesPerView === 'auto' && s.activeIndex >= s.loopedSlides * 2) || (s.activeIndex > s.slides.length - s.params.slidesPerView * 2)) {
                    newIndex = -s.slides.length + s.activeIndex + s.loopedSlides;
                    newIndex = newIndex + s.loopedSlides;
                    s.slideTo(newIndex, 0, false, true);
                }
            };
            /*=========================
              Append/Prepend/Remove Slides
              ===========================*/
            s.appendSlide = function (slides) {
                if (s.params.loop) {
                    s.destroyLoop();
                }
                if (typeof slides === 'object' && slides.length) {
                    for (var i = 0; i < slides.length; i++) {
                        if (slides[i]) s.wrapper.append(slides[i]);
                    }
                }
                else {
                    s.wrapper.append(slides);
                }
                if (s.params.loop) {
                    s.createLoop();
                }
                if (!(s.params.observer && s.support.observer)) {
                    s.update(true);
                }
            };
            s.prependSlide = function (slides) {
                if (s.params.loop) {
                    s.destroyLoop();
                }
                var newActiveIndex = s.activeIndex + 1;
                if (typeof slides === 'object' && slides.length) {
                    for (var i = 0; i < slides.length; i++) {
                        if (slides[i]) s.wrapper.prepend(slides[i]);
                    }
                    newActiveIndex = s.activeIndex + slides.length;
                }
                else {
                    s.wrapper.prepend(slides);
                }
                if (s.params.loop) {
                    s.createLoop();
                }
                if (!(s.params.observer && s.support.observer)) {
                    s.update(true);
                }
                s.slideTo(newActiveIndex, 0, false);
            };
            s.removeSlide = function (slidesIndexes) {
                if (s.params.loop) {
                    s.destroyLoop();
                }
                var newActiveIndex = s.activeIndex,
                    indexToRemove;
                if (typeof slidesIndexes === 'object' && slidesIndexes.length) {
                    for (var i = 0; i < slidesIndexes.length; i++) {
                        indexToRemove = slidesIndexes[i];
                        if (s.slides[indexToRemove]) s.slides.eq(indexToRemove).remove();
                        if (indexToRemove < newActiveIndex) newActiveIndex--;
                    }
                    newActiveIndex = Math.max(newActiveIndex, 0);
                }
                else {
                    indexToRemove = slidesIndexes;
                    if (s.slides[indexToRemove]) s.slides.eq(indexToRemove).remove();
                    if (indexToRemove < newActiveIndex) newActiveIndex--;
                    newActiveIndex = Math.max(newActiveIndex, 0);
                }

                if (!(s.params.observer && s.support.observer)) {
                    s.update(true);
                }
                s.slideTo(newActiveIndex, 0, false);
            };
            s.removeAllSlides = function () {
                var slidesIndexes = [];
                for (var i = 0; i < s.slides.length; i++) {
                    slidesIndexes.push(i);
                }
                s.removeSlide(slidesIndexes);
            };


            /*=========================
              Effects
              ===========================*/
            s.effects = {
                fade: {
                    fadeIndex: null,
                    setTranslate: function () {
                        for (var i = 0; i < s.slides.length; i++) {
                            var slide = s.slides.eq(i);
                            var offset = slide[0].swiperSlideOffset;
                            var tx = -offset;
                            if (!s.params.virtualTranslate) tx = tx - s.translate;
                            var ty = 0;
                            if (!isH()) {
                                ty = tx;
                                tx = 0;
                            }
                            var slideOpacity = s.params.fade.crossFade ?
                                    Math.max(1 - Math.abs(slide[0].progress), 0) :
                                    1 + Math.min(Math.max(slide[0].progress, -1), 0);
                            if (slideOpacity > 0 && slideOpacity < 1) {
                                s.effects.fade.fadeIndex = i;
                            }
                            slide
                                .css({
                                    opacity: slideOpacity
                                })
                                .transform('translate3d(' + tx + 'px, ' + ty + 'px, 0px)');

                        }
                    },
                    setTransition: function (duration) {
                        s.slides.transition(duration);
                        if (s.params.virtualTranslate && duration !== 0) {
                            var fadeIndex = s.effects.fade.fadeIndex !== null ? s.effects.fade.fadeIndex : s.activeIndex;
                            s.slides.eq(fadeIndex).transitionEnd(function () {
                                var triggerEvents = ['webkitTransitionEnd', 'transitionend', 'oTransitionEnd', 'MSTransitionEnd', 'msTransitionEnd'];
                                for (var i = 0; i < triggerEvents.length; i++) {
                                    s.wrapper.trigger(triggerEvents[i]);
                                }
                            });
                        }
                    }
                },
                cube: {
                    setTranslate: function () {
                        var wrapperRotate = 0, cubeShadow;
                        if (s.params.cube.shadow) {
                            if (isH()) {
                                cubeShadow = s.wrapper.find('.swiper-cube-shadow');
                                if (cubeShadow.length === 0) {
                                    cubeShadow = $('<div class="swiper-cube-shadow"></div>');
                                    s.wrapper.append(cubeShadow);
                                }
                                cubeShadow.css({ height: s.width + 'px' });
                            }
                            else {
                                cubeShadow = s.container.find('.swiper-cube-shadow');
                                if (cubeShadow.length === 0) {
                                    cubeShadow = $('<div class="swiper-cube-shadow"></div>');
                                    s.container.append(cubeShadow);
                                }
                            }
                        }
                        for (var i = 0; i < s.slides.length; i++) {
                            var slide = s.slides.eq(i);
                            var slideAngle = i * 90;
                            var round = Math.floor(slideAngle / 360);
                            if (s.rtl) {
                                slideAngle = -slideAngle;
                                round = Math.floor(-slideAngle / 360);
                            }
                            var progress = Math.max(Math.min(slide[0].progress, 1), -1);
                            var tx = 0, ty = 0, tz = 0;
                            if (i % 4 === 0) {
                                tx = -round * 4 * s.size;
                                tz = 0;
                            }
                            else if ((i - 1) % 4 === 0) {
                                tx = 0;
                                tz = -round * 4 * s.size;
                            }
                            else if ((i - 2) % 4 === 0) {
                                tx = s.size + round * 4 * s.size;
                                tz = s.size;
                            }
                            else if ((i - 3) % 4 === 0) {
                                tx = -s.size;
                                tz = 3 * s.size + s.size * 4 * round;
                            }
                            if (s.rtl) {
                                tx = -tx;
                            }

                            if (!isH()) {
                                ty = tx;
                                tx = 0;
                            }

                            var transform = 'rotateX(' + (isH() ? 0 : -slideAngle) + 'deg) rotateY(' + (isH() ? slideAngle : 0) + 'deg) translate3d(' + tx + 'px, ' + ty + 'px, ' + tz + 'px)';
                            if (progress <= 1 && progress > -1) {
                                wrapperRotate = i * 90 + progress * 90;
                                if (s.rtl) wrapperRotate = -i * 90 - progress * 90;
                            }
                            slide.transform(transform);
                            if (s.params.cube.slideShadows) {
                                //Set shadows
                                var shadowBefore = isH() ? slide.find('.swiper-slide-shadow-left') : slide.find('.swiper-slide-shadow-top');
                                var shadowAfter = isH() ? slide.find('.swiper-slide-shadow-right') : slide.find('.swiper-slide-shadow-bottom');
                                if (shadowBefore.length === 0) {
                                    shadowBefore = $('<div class="swiper-slide-shadow-' + (isH() ? 'left' : 'top') + '"></div>');
                                    slide.append(shadowBefore);
                                }
                                if (shadowAfter.length === 0) {
                                    shadowAfter = $('<div class="swiper-slide-shadow-' + (isH() ? 'right' : 'bottom') + '"></div>');
                                    slide.append(shadowAfter);
                                }
                                if (shadowBefore.length) shadowBefore[0].style.opacity = -slide[0].progress;
                                if (shadowAfter.length) shadowAfter[0].style.opacity = slide[0].progress;
                            }
                        }
                        s.wrapper.css({
                            '-webkit-transform-origin': '50% 50% -' + (s.size / 2) + 'px',
                            '-moz-transform-origin': '50% 50% -' + (s.size / 2) + 'px',
                            '-ms-transform-origin': '50% 50% -' + (s.size / 2) + 'px',
                            'transform-origin': '50% 50% -' + (s.size / 2) + 'px'
                        });

                        if (s.params.cube.shadow) {
                            if (isH()) {
                                cubeShadow.transform('translate3d(0px, ' + (s.width / 2 + s.params.cube.shadowOffset) + 'px, ' + (-s.width / 2) + 'px) rotateX(90deg) rotateZ(0deg) scale(' + (s.params.cube.shadowScale) + ')');
                            }
                            else {
                                var shadowAngle = Math.abs(wrapperRotate) - Math.floor(Math.abs(wrapperRotate) / 90) * 90;
                                var multiplier = 1.5 - (Math.sin(shadowAngle * 2 * Math.PI / 360) / 2 + Math.cos(shadowAngle * 2 * Math.PI / 360) / 2);
                                var scale1 = s.params.cube.shadowScale,
                                    scale2 = s.params.cube.shadowScale / multiplier,
                                    offset = s.params.cube.shadowOffset;
                                cubeShadow.transform('scale3d(' + scale1 + ', 1, ' + scale2 + ') translate3d(0px, ' + (s.height / 2 + offset) + 'px, ' + (-s.height / 2 / scale2) + 'px) rotateX(-90deg)');
                            }
                        }
                        var zFactor = (s.isSafari || s.isUiWebView) ? (-s.size / 2) : 0;
                        s.wrapper.transform('translate3d(0px,0,' + zFactor + 'px) rotateX(' + (isH() ? 0 : wrapperRotate) + 'deg) rotateY(' + (isH() ? -wrapperRotate : 0) + 'deg)');
                    },
                    setTransition: function (duration) {
                        s.slides.transition(duration).find('.swiper-slide-shadow-top, .swiper-slide-shadow-right, .swiper-slide-shadow-bottom, .swiper-slide-shadow-left').transition(duration);
                        if (s.params.cube.shadow && !isH()) {
                            s.container.find('.swiper-cube-shadow').transition(duration);
                        }
                    }
                },
                coverflow: {
                    setTranslate: function () {
                        var transform = s.translate;
                        var center = isH() ? -transform + s.width / 2 : -transform + s.height / 2;
                        var rotate = isH() ? s.params.coverflow.rotate : -s.params.coverflow.rotate;
                        var translate = s.params.coverflow.depth;
                        //Each slide offset from center
                        for (var i = 0, length = s.slides.length; i < length; i++) {
                            var slide = s.slides.eq(i);
                            var slideSize = s.slidesSizesGrid[i];
                            var slideOffset = slide[0].swiperSlideOffset;
                            var offsetMultiplier = (center - slideOffset - slideSize / 2) / slideSize * s.params.coverflow.modifier;

                            var rotateY = isH() ? rotate * offsetMultiplier : 0;
                            var rotateX = isH() ? 0 : rotate * offsetMultiplier;
                            // var rotateZ = 0
                            var translateZ = -translate * Math.abs(offsetMultiplier);

                            var translateY = isH() ? 0 : s.params.coverflow.stretch * (offsetMultiplier);
                            var translateX = isH() ? s.params.coverflow.stretch * (offsetMultiplier) : 0;

                            //Fix for ultra small values
                            if (Math.abs(translateX) < 0.001) translateX = 0;
                            if (Math.abs(translateY) < 0.001) translateY = 0;
                            if (Math.abs(translateZ) < 0.001) translateZ = 0;
                            if (Math.abs(rotateY) < 0.001) rotateY = 0;
                            if (Math.abs(rotateX) < 0.001) rotateX = 0;

                            var slideTransform = 'translate3d(' + translateX + 'px,' + translateY + 'px,' + translateZ + 'px)  rotateX(' + rotateX + 'deg) rotateY(' + rotateY + 'deg)';

                            slide.transform(slideTransform);
                            slide[0].style.zIndex = -Math.abs(Math.round(offsetMultiplier)) + 1;
                            if (s.params.coverflow.slideShadows) {
                                //Set shadows
                                var shadowBefore = isH() ? slide.find('.swiper-slide-shadow-left') : slide.find('.swiper-slide-shadow-top');
                                var shadowAfter = isH() ? slide.find('.swiper-slide-shadow-right') : slide.find('.swiper-slide-shadow-bottom');
                                if (shadowBefore.length === 0) {
                                    shadowBefore = $('<div class="swiper-slide-shadow-' + (isH() ? 'left' : 'top') + '"></div>');
                                    slide.append(shadowBefore);
                                }
                                if (shadowAfter.length === 0) {
                                    shadowAfter = $('<div class="swiper-slide-shadow-' + (isH() ? 'right' : 'bottom') + '"></div>');
                                    slide.append(shadowAfter);
                                }
                                if (shadowBefore.length) shadowBefore[0].style.opacity = offsetMultiplier > 0 ? offsetMultiplier : 0;
                                if (shadowAfter.length) shadowAfter[0].style.opacity = (-offsetMultiplier) > 0 ? -offsetMultiplier : 0;
                            }
                        }

                        //Set correct perspective for IE10
                        if (s.browser.ie) {
                            var ws = s.wrapper[0].style;
                            ws.perspectiveOrigin = center + 'px 50%';
                        }
                    },
                    setTransition: function (duration) {
                        s.slides.transition(duration).find('.swiper-slide-shadow-top, .swiper-slide-shadow-right, .swiper-slide-shadow-bottom, .swiper-slide-shadow-left').transition(duration);
                    }
                }
            };

            /*=========================
              Images Lazy Loading
              ===========================*/
            s.lazy = {
                initialImageLoaded: false,
                loadImageInSlide: function (index) {
                    if (typeof index === 'undefined') return;
                    if (s.slides.length === 0) return;

                    var slide = s.slides.eq(index);
                    var img = slide.find('img.swiper-lazy:not(.swiper-lazy-loaded):not(.swiper-lazy-loading)');
                    if (img.length === 0) return;

                    img.each(function () {
                        var _img = $(this);
                        _img.addClass('swiper-lazy-loading');

                        var src = _img.attr('data-src');

                        s.loadImage(_img[0], src, false, function () {
                            _img.attr('src', src);
                            _img.removeAttr('data-src');
                            _img.addClass('swiper-lazy-loaded').removeClass('swiper-lazy-loading');
                            slide.find('.swiper-lazy-preloader, .preloader').remove();

                            s.emit('onLazyImageReady', s, slide[0], _img[0]);
                        });

                        s.emit('onLazyImageLoad', s, slide[0], _img[0]);
                    });

                },
                load: function () {
                    if (s.params.watchSlidesVisibility) {
                        s.wrapper.children('.' + s.params.slideVisibleClass).each(function () {
                            s.lazy.loadImageInSlide($(this).index());
                        });
                    }
                    else {
                        if (s.params.slidesPerView > 1) {
                            for (var i = s.activeIndex; i < s.activeIndex + s.params.slidesPerView ; i++) {
                                if (s.slides[i]) s.lazy.loadImageInSlide(i);
                            }
                        }
                        else {
                            s.lazy.loadImageInSlide(s.activeIndex);
                        }
                    }
                    if (s.params.lazyLoadingInPrevNext) {
                        var nextSlide = s.wrapper.children('.' + s.params.slideNextClass);
                        if (nextSlide.length > 0) s.lazy.loadImageInSlide(nextSlide.index());

                        var prevSlide = s.wrapper.children('.' + s.params.slidePrevClass);
                        if (prevSlide.length > 0) s.lazy.loadImageInSlide(prevSlide.index());
                    }
                },
                onTransitionStart: function () {
                    if (s.params.lazyLoading) {
                        if (s.params.lazyLoadingOnTransitionStart || (!s.params.lazyLoadingOnTransitionStart && !s.lazy.initialImageLoaded)) {
                            s.lazy.initialImageLoaded = true;
                            s.lazy.load();
                        }
                    }
                },
                onTransitionEnd: function () {
                    if (s.params.lazyLoading && !s.params.lazyLoadingOnTransitionStart) {
                        s.lazy.load();
                    }
                }
            };


            /*=========================
              Scrollbar
              ===========================*/
            s.scrollbar = {
                set: function () {
                    if (!s.params.scrollbar) return;
                    var sb = s.scrollbar;
                    sb.track = $(s.params.scrollbar);
                    sb.drag = sb.track.find('.swiper-scrollbar-drag');
                    if (sb.drag.length === 0) {
                        sb.drag = $('<div class="swiper-scrollbar-drag"></div>');
                        sb.track.append(sb.drag);
                    }
                    sb.drag[0].style.width = '';
                    sb.drag[0].style.height = '';
                    sb.trackSize = isH() ? sb.track[0].offsetWidth : sb.track[0].offsetHeight;

                    sb.divider = s.size / s.virtualSize;
                    sb.moveDivider = sb.divider * (sb.trackSize / s.size);
                    sb.dragSize = sb.trackSize * sb.divider;

                    if (isH()) {
                        sb.drag[0].style.width = sb.dragSize + 'px';
                    }
                    else {
                        sb.drag[0].style.height = sb.dragSize + 'px';
                    }

                    if (sb.divider >= 1) {
                        sb.track[0].style.display = 'none';
                    }
                    else {
                        sb.track[0].style.display = '';
                    }
                    if (s.params.scrollbarHide) {
                        sb.track[0].style.opacity = 0;
                    }
                },
                setTranslate: function () {
                    if (!s.params.scrollbar) return;
                    var sb = s.scrollbar;
                    var newPos;

                    var newSize = sb.dragSize;
                    newPos = (sb.trackSize - sb.dragSize) * s.progress;
                    if (s.rtl && isH()) {
                        newPos = -newPos;
                        if (newPos > 0) {
                            newSize = sb.dragSize - newPos;
                            newPos = 0;
                        }
                        else if (-newPos + sb.dragSize > sb.trackSize) {
                            newSize = sb.trackSize + newPos;
                        }
                    }
                    else {
                        if (newPos < 0) {
                            newSize = sb.dragSize + newPos;
                            newPos = 0;
                        }
                        else if (newPos + sb.dragSize > sb.trackSize) {
                            newSize = sb.trackSize - newPos;
                        }
                    }
                    if (isH()) {
                        if (s.support.transforms3d) {
                            sb.drag.transform('translate3d(' + (newPos) + 'px, 0, 0)');
                        }
                        else {
                            sb.drag.transform('translateX(' + (newPos) + 'px)');
                        }
                        sb.drag[0].style.width = newSize + 'px';
                    }
                    else {
                        if (s.support.transforms3d) {
                            sb.drag.transform('translate3d(0px, ' + (newPos) + 'px, 0)');
                        }
                        else {
                            sb.drag.transform('translateY(' + (newPos) + 'px)');
                        }
                        sb.drag[0].style.height = newSize + 'px';
                    }
                    if (s.params.scrollbarHide) {
                        clearTimeout(sb.timeout);
                        sb.track[0].style.opacity = 1;
                        sb.timeout = setTimeout(function () {
                            sb.track[0].style.opacity = 0;
                            sb.track.transition(400);
                        }, 1000);
                    }
                },
                setTransition: function (duration) {
                    if (!s.params.scrollbar) return;
                    s.scrollbar.drag.transition(duration);
                }
            };

            /*=========================
              Controller
              ===========================*/
            s.controller = {
                setTranslate: function (translate, byController) {
                    var controlled = s.params.control;
                    var multiplier, controlledTranslate;
                    if (s.isArray(controlled)) {
                        for (var i = 0; i < controlled.length; i++) {
                            if (controlled[i] !== byController && controlled[i] instanceof Swiper) {
                                translate = controlled[i].rtl && controlled[i].params.direction === 'horizontal' ? -s.translate : s.translate;
                                multiplier = (controlled[i].maxTranslate() - controlled[i].minTranslate()) / (s.maxTranslate() - s.minTranslate());
                                controlledTranslate = (translate - s.minTranslate()) * multiplier + controlled[i].minTranslate();
                                if (s.params.controlInverse) {
                                    controlledTranslate = controlled[i].maxTranslate() - controlledTranslate;
                                }
                                controlled[i].updateProgress(controlledTranslate);
                                controlled[i].setWrapperTranslate(controlledTranslate, false, s);
                                controlled[i].updateActiveIndex();
                            }
                        }
                    }
                    else if (controlled instanceof Swiper && byController !== controlled) {
                        translate = controlled.rtl && controlled.params.direction === 'horizontal' ? -s.translate : s.translate;
                        multiplier = (controlled.maxTranslate() - controlled.minTranslate()) / (s.maxTranslate() - s.minTranslate());
                        controlledTranslate = (translate - s.minTranslate()) * multiplier + controlled.minTranslate();
                        if (s.params.controlInverse) {
                            controlledTranslate = controlled.maxTranslate() - controlledTranslate;
                        }
                        controlled.updateProgress(controlledTranslate);
                        controlled.setWrapperTranslate(controlledTranslate, false, s);
                        controlled.updateActiveIndex();
                    }
                },
                setTransition: function (duration, byController) {
                    var controlled = s.params.control;
                    if (s.isArray(controlled)) {
                        for (var i = 0; i < controlled.length; i++) {
                            if (controlled[i] !== byController && controlled[i] instanceof Swiper) {
                                controlled[i].setWrapperTransition(duration, s);
                            }
                        }
                    }
                    else if (controlled instanceof Swiper && byController !== controlled) {
                        controlled.setWrapperTransition(duration, s);
                    }
                }
            };

            /*=========================
              Parallax
              ===========================*/
            function setParallaxTransform(el, progress) {
                el = $(el);
                var p, pX, pY;

                p = el.attr('data-swiper-parallax') || '0';
                pX = el.attr('data-swiper-parallax-x');
                pY = el.attr('data-swiper-parallax-y');
                if (pX || pY) {
                    pX = pX || '0';
                    pY = pY || '0';
                }
                else {
                    if (isH()) {
                        pX = p;
                        pY = '0';
                    }
                    else {
                        pY = p;
                        pX = '0';
                    }
                }
                if ((pX).indexOf('%') >= 0) {
                    pX = parseInt(pX, 10) * progress + '%';
                }
                else {
                    pX = pX * progress + 'px';
                }
                if ((pY).indexOf('%') >= 0) {
                    pY = parseInt(pY, 10) * progress + '%';
                }
                else {
                    pY = pY * progress + 'px';
                }
                el.transform('translate3d(' + pX + ', ' + pY + ',0px)');
            }
            s.parallax = {
                setTranslate: function () {
                    s.container.children('[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]').each(function () {
                        setParallaxTransform(this, s.progress);

                    });
                    s.slides.each(function () {
                        var slide = $(this);
                        slide.find('[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]').each(function () {
                            var progress = Math.min(Math.max(slide[0].progress, -1), 1);
                            setParallaxTransform(this, progress);
                        });
                    });
                },
                setTransition: function (duration) {
                    if (typeof duration === 'undefined') duration = s.params.speed;
                    s.container.find('[data-swiper-parallax], [data-swiper-parallax-x], [data-swiper-parallax-y]').each(function () {
                        var el = $(this);
                        var parallaxDuration = parseInt(el.attr('data-swiper-parallax-duration'), 10) || duration;
                        if (duration === 0) parallaxDuration = 0;
                        el.transition(parallaxDuration);
                    });
                }
            };


            /*=========================
              Plugins API. Collect all and init all plugins
              ===========================*/
            s._plugins = [];
            for (var plugin in s.plugins) {
                if (s.plugins.hasOwnProperty(plugin)) {
                    var p = s.plugins[plugin](s, s.params[plugin]);
                    if (p) s._plugins.push(p);
                }
            }
            // Method to call all plugins event/method
            s.callPlugins = function (eventName) {
                for (var i = 0; i < s._plugins.length; i++) {
                    if (eventName in s._plugins[i]) {
                        s._plugins[i][eventName](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
                    }
                }
            };

            /*=========================
              Events/Callbacks/Plugins Emitter
              ===========================*/
            function normalizeEventName(eventName) {
                if (eventName.indexOf('on') !== 0) {
                    if (eventName[0] !== eventName[0].toUpperCase()) {
                        eventName = 'on' + eventName[0].toUpperCase() + eventName.substring(1);
                    }
                    else {
                        eventName = 'on' + eventName;
                    }
                }
                return eventName;
            }
            s.emitterEventListeners = {

            };
            s.emit = function (eventName) {
                // Trigger callbacks
                if (s.params[eventName]) {
                    s.params[eventName](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
                }
                var i;
                // å›¾ç‰‡æµè§ˆå™¨ç‚¹å‡»å…³é—­åŽï¼Œswiperä¹Ÿå…³é—­äº†ï¼Œä½†ä¼šæ‰§è¡Œåˆ°æ­¤å¤„
                if (!s) return;
                // Trigger events
                if (s.emitterEventListeners[eventName]) {
                    for (i = 0; i < s.emitterEventListeners[eventName].length; i++) {
                        s.emitterEventListeners[eventName][i](arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
                    }
                }
                // Trigger plugins
                if (s.callPlugins) s.callPlugins(eventName, arguments[1], arguments[2], arguments[3], arguments[4], arguments[5]);
            };
            s.on = function (eventName, handler) {
                eventName = normalizeEventName(eventName);
                if (!s.emitterEventListeners[eventName]) s.emitterEventListeners[eventName] = [];
                s.emitterEventListeners[eventName].push(handler);
                return s;
            };
            s.off = function (eventName, handler) {
                var i;
                eventName = normalizeEventName(eventName);
                if (typeof handler === 'undefined') {
                    // Remove all handlers for such event
                    s.emitterEventListeners[eventName] = [];
                    return s;
                }
                if (!s.emitterEventListeners[eventName] || s.emitterEventListeners[eventName].length === 0) return;
                for (i = 0; i < s.emitterEventListeners[eventName].length; i++) {
                    if (s.emitterEventListeners[eventName][i] === handler) s.emitterEventListeners[eventName].splice(i, 1);
                }
                return s;
            };
            s.once = function (eventName, handler) {
                eventName = normalizeEventName(eventName);
                var _handler = function () {
                    handler(arguments[0], arguments[1], arguments[2], arguments[3], arguments[4]);
                    s.off(eventName, _handler);
                };
                s.on(eventName, _handler);
                return s;
            };

            // Accessibility tools
            s.a11y = {
                makeFocusable: function ($el) {
                    $el[0].tabIndex = '0';
                    return $el;
                },
                addRole: function ($el, role) {
                    $el.attr('role', role);
                    return $el;
                },

                addLabel: function ($el, label) {
                    $el.attr('aria-label', label);
                    return $el;
                },

                disable: function ($el) {
                    $el.attr('aria-disabled', true);
                    return $el;
                },

                enable: function ($el) {
                    $el.attr('aria-disabled', false);
                    return $el;
                },

                onEnterKey: function (event) {
                    if (event.keyCode !== 13) return;
                    if ($(event.target).is(s.params.nextButton)) {
                        s.onClickNext(event);
                        if (s.isEnd) {
                            s.a11y.notify(s.params.lastSlideMsg);
                        }
                        else {
                            s.a11y.notify(s.params.nextSlideMsg);
                        }
                    }
                    else if ($(event.target).is(s.params.prevButton)) {
                        s.onClickPrev(event);
                        if (s.isBeginning) {
                            s.a11y.notify(s.params.firstSlideMsg);
                        }
                        else {
                            s.a11y.notify(s.params.prevSlideMsg);
                        }
                    }
                },

                liveRegion: $('<span class="swiper-notification" aria-live="assertive" aria-atomic="true"></span>'),

                notify: function (message) {
                    var notification = s.a11y.liveRegion;
                    if (notification.length === 0) return;
                    notification.html('');
                    notification.html(message);
                },
                init: function () {
                    // Setup accessibility
                    if (s.params.nextButton) {
                        var nextButton = $(s.params.nextButton);
                        s.a11y.makeFocusable(nextButton);
                        s.a11y.addRole(nextButton, 'button');
                        s.a11y.addLabel(nextButton, s.params.nextSlideMsg);
                    }
                    if (s.params.prevButton) {
                        var prevButton = $(s.params.prevButton);
                        s.a11y.makeFocusable(prevButton);
                        s.a11y.addRole(prevButton, 'button');
                        s.a11y.addLabel(prevButton, s.params.prevSlideMsg);
                    }

                    $(s.container).append(s.a11y.liveRegion);
                },
                destroy: function () {
                    if (s.a11y.liveRegion && s.a11y.liveRegion.length > 0) s.a11y.liveRegion.remove();
                }
            };


            /*=========================
              Init/Destroy
              ===========================*/
            s.init = function () {
                if (s.params.loop) s.createLoop();
                s.updateContainerSize();
                s.updateSlidesSize();
                s.updatePagination();
                if (s.params.scrollbar && s.scrollbar) {
                    s.scrollbar.set();
                }
                if (s.params.effect !== 'slide' && s.effects[s.params.effect]) {
                    if (!s.params.loop) s.updateProgress();
                    s.effects[s.params.effect].setTranslate();
                }
                if (s.params.loop) {
                    s.slideTo(s.params.initialSlide + s.loopedSlides, 0, s.params.runCallbacksOnInit);
                }
                else {
                    s.slideTo(s.params.initialSlide, 0, s.params.runCallbacksOnInit);
                    if (s.params.initialSlide === 0) {
                        if (s.parallax && s.params.parallax) s.parallax.setTranslate();
                        if (s.lazy && s.params.lazyLoading) s.lazy.load();
                    }
                }
                s.attachEvents();
                if (s.params.observer && s.support.observer) {
                    s.initObservers();
                }
                if (s.params.preloadImages && !s.params.lazyLoading) {
                    s.preloadImages();
                }
                if (s.params.autoplay) {
                    s.startAutoplay();
                }
                if (s.params.keyboardControl) {
                    if (s.enableKeyboardControl) s.enableKeyboardControl();
                }
                if (s.params.mousewheelControl) {
                    if (s.enableMousewheelControl) s.enableMousewheelControl();
                }
                if (s.params.hashnav) {
                    if (s.hashnav) s.hashnav.init();
                }
                if (s.params.a11y && s.a11y) s.a11y.init();
                s.emit('onInit', s);
            };

            // Cleanup dynamic styles
            s.cleanupStyles = function () {
                // Container
                s.container.removeClass(s.classNames.join(' ')).removeAttr('style');

                // Wrapper
                s.wrapper.removeAttr('style');

                // Slides
                if (s.slides && s.slides.length) {
                    s.slides
                        .removeClass([
                          s.params.slideVisibleClass,
                          s.params.slideActiveClass,
                          s.params.slideNextClass,
                          s.params.slidePrevClass
                        ].join(' '))
                        .removeAttr('style')
                        .removeAttr('data-swiper-column')
                        .removeAttr('data-swiper-row');
                }

                // Pagination/Bullets
                if (s.paginationContainer && s.paginationContainer.length) {
                    s.paginationContainer.removeClass(s.params.paginationHiddenClass);
                }
                if (s.bullets && s.bullets.length) {
                    s.bullets.removeClass(s.params.bulletActiveClass);
                }

                // Buttons
                if (s.params.prevButton) $(s.params.prevButton).removeClass(s.params.buttonDisabledClass);
                if (s.params.nextButton) $(s.params.nextButton).removeClass(s.params.buttonDisabledClass);

                // Scrollbar
                if (s.params.scrollbar && s.scrollbar) {
                    if (s.scrollbar.track && s.scrollbar.track.length) s.scrollbar.track.removeAttr('style');
                    if (s.scrollbar.drag && s.scrollbar.drag.length) s.scrollbar.drag.removeAttr('style');
                }
            };

            // Destroy
            s.destroy = function (deleteInstance, cleanupStyles) {
                // Detach evebts
                s.detachEvents();
                // Stop autoplay
                s.stopAutoplay();
                // Destroy loop
                if (s.params.loop) {
                    s.destroyLoop();
                }
                // Cleanup styles
                if (cleanupStyles) {
                    s.cleanupStyles();
                }
                // Disconnect observer
                s.disconnectObservers();
                // Disable keyboard/mousewheel
                if (s.params.keyboardControl) {
                    if (s.disableKeyboardControl) s.disableKeyboardControl();
                }
                if (s.params.mousewheelControl) {
                    if (s.disableMousewheelControl) s.disableMousewheelControl();
                }
                // Disable a11y
                if (s.params.a11y && s.a11y) s.a11y.destroy();
                // Destroy callback
                s.emit('onDestroy');
                // Delete instance
                if (deleteInstance !== false) s = null;
            };

            s.init();



            // Return swiper instance
            return s;
        };
        /*==================================================
            Prototype
        ====================================================*/
        Swiper.prototype = {
            defaults: {
                direction: 'horizontal',
                touchEventsTarget: 'container',
                initialSlide: 0,
                speed: 300,
                // autoplay
                autoplay: false,
                autoplayDisableOnInteraction: true,
                // Free mode
                freeMode: false,
                freeModeMomentum: true,
                freeModeMomentumRatio: 1,
                freeModeMomentumBounce: true,
                freeModeMomentumBounceRatio: 1,
                // Set wrapper width
                setWrapperSize: false,
                // Virtual Translate
                virtualTranslate: false,
                // Effects
                effect: 'slide', // 'slide' or 'fade' or 'cube' or 'coverflow'
                coverflow: {
                    rotate: 50,
                    stretch: 0,
                    depth: 100,
                    modifier: 1,
                    slideShadows: true
                },
                cube: {
                    slideShadows: true,
                    shadow: true,
                    shadowOffset: 20,
                    shadowScale: 0.94
                },
                fade: {
                    crossFade: false
                },
                // Parallax
                parallax: false,
                // Scrollbar
                scrollbar: null,
                scrollbarHide: true,
                // Keyboard Mousewheel
                keyboardControl: false,
                mousewheelControl: false,
                mousewheelForceToAxis: false,
                // Hash Navigation
                hashnav: false,
                // Slides grid
                spaceBetween: 0,
                slidesPerView: 1,
                slidesPerColumn: 1,
                slidesPerColumnFill: 'column',
                slidesPerGroup: 1,
                centeredSlides: false,
                // Touches
                touchRatio: 1,
                touchAngle: 45,
                simulateTouch: true,
                shortSwipes: true,
                longSwipes: true,
                longSwipesRatio: 0.5,
                longSwipesMs: 300,
                followFinger: true,
                onlyExternal: false,
                threshold: 0,
                touchMoveStopPropagation: true,
                // Pagination
                pagination: null,
                paginationClickable: false,
                paginationHide: false,
                paginationBulletRender: null,
                // Resistance
                resistance: true,
                resistanceRatio: 0.85,
                // Next/prev buttons
                nextButton: null,
                prevButton: null,
                // Progress
                watchSlidesProgress: false,
                watchSlidesVisibility: false,
                // Cursor
                grabCursor: false,
                // Clicks
                preventClicks: true,
                preventClicksPropagation: true,
                slideToClickedSlide: false,
                // Lazy Loading
                lazyLoading: false,
                lazyLoadingInPrevNext: false,
                lazyLoadingOnTransitionStart: false,
                // Images
                preloadImages: true,
                updateOnImagesReady: true,
                // loop
                loop: false,
                loopAdditionalSlides: 0,
                loopedSlides: null,
                // Control
                control: undefined,
                controlInverse: false,
                // Swiping/no swiping
                allowSwipeToPrev: true,
                allowSwipeToNext: true,
                swipeHandler: null, //'.swipe-handler',
                noSwiping: true,
                noSwipingClass: 'swiper-no-swiping',
                // NS
                slideClass: 'swiper-slide',
                slideActiveClass: 'swiper-slide-active',
                slideVisibleClass: 'swiper-slide-visible',
                slideDuplicateClass: 'swiper-slide-duplicate',
                slideNextClass: 'swiper-slide-next',
                slidePrevClass: 'swiper-slide-prev',
                wrapperClass: 'swiper-wrapper',
                bulletClass: 'swiper-pagination-bullet',
                bulletActiveClass: 'swiper-pagination-bullet-active',
                buttonDisabledClass: 'swiper-button-disabled',
                paginationHiddenClass: 'swiper-pagination-hidden',
                // Observer
                observer: false,
                observeParents: false,
                // Accessibility
                a11y: false,
                prevSlideMessage: 'Previous slide',
                nextSlideMessage: 'Next slide',
                firstSlideMessage: 'This is the first slide',
                lastSlideMessage: 'This is the last slide',
                // Callbacks
                runCallbacksOnInit: true,
                /*
                Callbacks:
                onInit: function (swiper)
                onDestroy: function (swiper)
                onClick: function (swiper, e)
                onTap: function (swiper, e)
                onDoubleTap: function (swiper, e)
                onSliderMove: function (swiper, e)
                onSlideChangeStart: function (swiper)
                onSlideChangeEnd: function (swiper)
                onTransitionStart: function (swiper)
                onTransitionEnd: function (swiper)
                onImagesReady: function (swiper)
                onProgress: function (swiper, progress)
                onTouchStart: function (swiper, e)
                onTouchMove: function (swiper, e)
                onTouchMoveOpposite: function (swiper, e)
                onTouchEnd: function (swiper, e)
                onReachBeginning: function (swiper)
                onReachEnd: function (swiper)
                onSetTransition: function (swiper, duration)
                onSetTranslate: function (swiper, translate)
                onAutoplayStart: function (swiper)
                onAutoplayStop: function (swiper),
                onLazyImageLoad: function (swiper, slide, image)
                onLazyImageReady: function (swiper, slide, image)
                */

            },
            isSafari: (function () {
                var ua = navigator.userAgent.toLowerCase();
                return (ua.indexOf('safari') >= 0 && ua.indexOf('chrome') < 0 && ua.indexOf('android') < 0);
            })(),
            isUiWebView: /(iPhone|iPod|iPad).*AppleWebKit(?!.*Safari)/i.test(navigator.userAgent),
            isArray: function (arr) {
                return Object.prototype.toString.apply(arr) === '[object Array]';
            },
            /*==================================================
            Browser
            ====================================================*/
            browser: {
                ie: window.navigator.pointerEnabled || window.navigator.msPointerEnabled,
                ieTouch: (window.navigator.msPointerEnabled && window.navigator.msMaxTouchPoints > 1) || (window.navigator.pointerEnabled && window.navigator.maxTouchPoints > 1),
            },
            /*==================================================
            Devices
            ====================================================*/
            device: (function () {
                var ua = navigator.userAgent;
                var android = ua.match(/(Android);?[\s\/]+([\d.]+)?/);
                var ipad = ua.match(/(iPad).*OS\s([\d_]+)/);
                var iphone = !ipad && ua.match(/(iPhone\sOS)\s([\d_]+)/);
                return {
                    ios: ipad || iphone || ipad,
                    android: android
                };
            })(),
            /*==================================================
            Feature Detection
            ====================================================*/
            support: {
                touch: (window.Modernizr && Modernizr.touch === true) || (function () {
                    return !!(('ontouchstart' in window) || window.DocumentTouch && document instanceof DocumentTouch);
                })(),

                transforms3d: (window.Modernizr && Modernizr.csstransforms3d === true) || (function () {
                    var div = document.createElement('div').style;
                    return ('webkitPerspective' in div || 'MozPerspective' in div || 'OPerspective' in div || 'MsPerspective' in div || 'perspective' in div);
                })(),

                flexbox: (function () {
                    var div = document.createElement('div').style;
                    var styles = ('alignItems webkitAlignItems webkitBoxAlign msFlexAlign mozBoxAlign webkitFlexDirection msFlexDirection mozBoxDirection mozBoxOrient webkitBoxDirection webkitBoxOrient').split(' ');
                    for (var i = 0; i < styles.length; i++) {
                        if (styles[i] in div) return true;
                    }
                })(),

                observer: (function () {
                    return ('MutationObserver' in window || 'WebkitMutationObserver' in window);
                })()
            },
            /*==================================================
            Plugins
            ====================================================*/
            plugins: {}
        };
        $.Swiper = Swiper;
    }(Zepto))

    ; (function ($) {
        'use strict';
        $.Swiper.prototype.defaults.pagination = '.page-current .swiper-pagination';

        $.swiper = function (container, params) {
            return new $.Swiper(container, params);
        };
        $.fn.swiper = function (params) {
            return new $.Swiper(this, params);
        };
        $.initSwiper = function (pageContainer) {
            var page = $(pageContainer || document.body);
            var swipers = page.find('.swiper-container');
            if (swipers.length === 0) return;
            function destroySwiperOnRemove(slider) {
                function destroySwiper() {
                    slider.destroy();
                    page.off('pageBeforeRemove', destroySwiper);
                }
                page.on('pageBeforeRemove', destroySwiper);
            }
            for (var i = 0; i < swipers.length; i++) {
                var swiper = swipers.eq(i);
                var params;
                if (swiper.data('swiper')) {
                    swiper.data("swiper").update(true);
                    continue;
                }
                else {
                    params = swiper.dataset();
                }
                var _slider = $.swiper(swiper[0], params);
                destroySwiperOnRemove(_slider);
            }
        };
        $.reinitSwiper = function (pageContainer) {
            var page = $(pageContainer || '.page-current');
            var sliders = page.find('.swiper-container');
            if (sliders.length === 0) return;
            for (var i = 0; i < sliders.length; i++) {
                var sliderInstance = sliders[0].swiper;
                if (sliderInstance) {
                    sliderInstance.update(true);
                }
            }
        };

    }(Zepto))


    var PhotoBrowser = function (params) {

        var pb = this, i;

        var defaults = this.defaults;

        params = params || {};
        for (var def in defaults) {
            if (typeof params[def] === 'undefined') {
                params[def] = defaults[def];
            }
        }

        pb.params = params;

        var navbarTemplate = pb.params.navbarTemplate ||
                            '<header class="bar bar-nav">' +
                              '<a class="icon icon-left pull-left photo-browser-close-link' + (pb.params.type === 'popup' ? " close-popup" : "") + '"></a>' +
                              '<h1 class="title"><div class="center sliding"><span class="photo-browser-current"></span> <span class="photo-browser-of">' + pb.params.ofText + '</span> <span class="photo-browser-total"></span></div></h1>' +
                            '</header>';

        var toolbarTemplate = pb.params.toolbarTemplate ||
                            '<nav class="bar bar-tab">' +
                              '<a class="tab-item photo-browser-prev" href="#">' +
                                '<i class="icon icon-prev"></i>' +
                              '</a>' +
                              '<a class="tab-item photo-browser-next" href="#">' +
                                '<i class="icon icon-next"></i>' +
                              '</a>' +
                            '</nav>';

        var template = pb.params.template ||
                        '<div class="photo-browser photo-browser-' + pb.params.theme + '">' +
                            '{{navbar}}' +
                            '{{toolbar}}' +
                            '<div data-page="photo-browser-slides" class="content">' +
                                '{{captions}}' +
                                '<div class="photo-browser-swiper-container swiper-container">' +
                                    '<div class="photo-browser-swiper-wrapper swiper-wrapper">' +
                                        '{{photos}}' +
                                    '</div>' +
                                '</div>' +
                            '</div>' +
                        '</div>';

        var photoTemplate = !pb.params.lazyLoading ?
            (pb.params.photoTemplate || '<div class="photo-browser-slide swiper-slide"><span class="photo-browser-zoom-container"><img src="{{url}}"></span></div>') :
            (pb.params.photoLazyTemplate || '<div class="photo-browser-slide photo-browser-slide-lazy swiper-slide"><div class="preloader' + (pb.params.theme === 'dark' ? ' preloader-white' : '') + '"></div><span class="photo-browser-zoom-container"><img data-src="{{url}}" class="swiper-lazy"></span></div>');

        var captionsTheme = pb.params.captionsTheme || pb.params.theme;
        var captionsTemplate = pb.params.captionsTemplate || '<div class="photo-browser-captions photo-browser-captions-' + captionsTheme + '">{{captions}}</div>';
        var captionTemplate = pb.params.captionTemplate || '<div class="photo-browser-caption" data-caption-index="{{captionIndex}}">{{caption}}</div>';

        var objectTemplate = pb.params.objectTemplate || '<div class="photo-browser-slide photo-browser-object-slide swiper-slide">{{html}}</div>';
        var photosHtml = '';
        var captionsHtml = '';
        for (i = 0; i < pb.params.photos.length; i++) {
            var photo = pb.params.photos[i];
            var thisTemplate = '';

            //check if photo is a string or string-like object, for backwards compatibility
            if (typeof (photo) === 'string' || photo instanceof String) {

                //check if "photo" is html object
                if (photo.indexOf('<') >= 0 || photo.indexOf('>') >= 0) {
                    thisTemplate = objectTemplate.replace(/{{html}}/g, photo);
                } else {
                    thisTemplate = photoTemplate.replace(/{{url}}/g, photo);
                }

                //photo is a string, thus has no caption, so remove the caption template placeholder
                //otherwise check if photo is an object with a url property
            } else if (typeof (photo) === 'object') {

                //check if "photo" is html object
                if (photo.hasOwnProperty('html') && photo.html.length > 0) {
                    thisTemplate = objectTemplate.replace(/{{html}}/g, photo.html);
                } else if (photo.hasOwnProperty('url') && photo.url.length > 0) {
                    thisTemplate = photoTemplate.replace(/{{url}}/g, photo.url);
                }

                //check if photo has a caption
                if (photo.hasOwnProperty('caption') && photo.caption.length > 0) {
                    captionsHtml += captionTemplate.replace(/{{caption}}/g, photo.caption).replace(/{{captionIndex}}/g, i);
                } else {
                    thisTemplate = thisTemplate.replace(/{{caption}}/g, '');
                }
            }

            photosHtml += thisTemplate;

        }

        var htmlTemplate = template
                            .replace('{{navbar}}', (pb.params.navbar ? navbarTemplate : ''))
                            .replace('{{noNavbar}}', (pb.params.navbar ? '' : 'no-navbar'))
                            .replace('{{photos}}', photosHtml)
                            .replace('{{captions}}', captionsTemplate.replace(/{{captions}}/g, captionsHtml))
                            .replace('{{toolbar}}', (pb.params.toolbar ? toolbarTemplate : ''));

        pb.activeIndex = pb.params.initialSlide;
        pb.openIndex = pb.activeIndex;
        pb.opened = false;

        pb.open = function (index) {
            if (typeof index === 'undefined') index = pb.activeIndex;
            index = parseInt(index, 10);
            if (pb.opened && pb.swiper) {
                pb.swiper.slideTo(index);
                return;
            }
            pb.opened = true;
            pb.openIndex = index;
            // pb.initialLazyLoaded = false;
            if (pb.params.type === 'standalone') {
                $(pb.params.container).append(htmlTemplate);
            }
            if (pb.params.type === 'popup') {
                pb.popup = $.popup('<div class="popup photo-browser-popup">' + htmlTemplate + '</div>');
                $(pb.popup).on('closed', pb.onPopupClose);
            }
            if (pb.params.type === 'page') {
                $(document).on('pageBeforeInit', pb.onPageBeforeInit);
                $(document).on('pageBeforeRemove', pb.onPageBeforeRemove);
                if (!pb.params.view) pb.params.view = $.mainView;
                pb.params.view.loadContent(htmlTemplate);
                return;
            }
            pb.layout(pb.openIndex);
            if (pb.params.onOpen) {
                pb.params.onOpen(pb);
            }

        };
        pb.close = function () {
            pb.opened = false;

            if (!pb.swiperContainer || pb.swiperContainer.length === 0) {
                return;
            }
            if (pb.params.onClose) {
                pb.params.onClose(pb);
            }
            // Detach events
            pb.attachEvents(true);
            // Delete from DOM
            if (pb.params.type === 'standalone') {
                pb.container.removeClass('photo-browser-in').addClass('photo-browser-out').transitionEnd(function () {
                    pb.container.remove();
                });
            }
            // Destroy slider
            pb.swiper.destroy();
            // Delete references
            pb.swiper = pb.swiperContainer = pb.swiperWrapper = pb.slides = gestureSlide = gestureImg = gestureImgWrap = undefined;

            //过半秒清除掉图片浏览节点
            setTimeout(function () {


                $(".photo-browser-light").remove();


            }, 0.5 * 1000);

        };

        pb.onPopupClose = function () {
            pb.close();
            $(pb.popup).off('pageBeforeInit', pb.onPopupClose);
        };
        pb.onPageBeforeInit = function (e) {
            if (e.detail.page.name === 'photo-browser-slides') {
                pb.layout(pb.openIndex);
            }
            $(document).off('pageBeforeInit', pb.onPageBeforeInit);
        };
        pb.onPageBeforeRemove = function (e) {
            if (e.detail.page.name === 'photo-browser-slides') {
                pb.close();
            }
            $(document).off('pageBeforeRemove', pb.onPageBeforeRemove);
        };

        pb.onSliderTransitionStart = function (swiper) {
            pb.activeIndex = swiper.activeIndex;

            var current = swiper.activeIndex + 1;
            var total = swiper.slides.length;
            if (pb.params.loop) {
                total = total - 2;
                current = current - swiper.loopedSlides;
                if (current < 1) current = total + current;
                if (current > total) current = current - total;
            }
            pb.container.find('.photo-browser-current').text(current);
            pb.container.find('.photo-browser-total').text(total);

            $('.photo-browser-prev, .photo-browser-next').removeClass('photo-browser-link-inactive');

            if (swiper.isBeginning && !pb.params.loop) {
                $('.photo-browser-prev').addClass('photo-browser-link-inactive');
            }
            if (swiper.isEnd && !pb.params.loop) {
                $('.photo-browser-next').addClass('photo-browser-link-inactive');
            }

            // Update captions
            if (pb.captions.length > 0) {
                pb.captionsContainer.find('.photo-browser-caption-active').removeClass('photo-browser-caption-active');
                var captionIndex = pb.params.loop ? swiper.slides.eq(swiper.activeIndex).attr('data-swiper-slide-index') : pb.activeIndex;
                pb.captionsContainer.find('[data-caption-index="' + captionIndex + '"]').addClass('photo-browser-caption-active');
            }


            // Stop Video
            var previousSlideVideo = swiper.slides.eq(swiper.previousIndex).find('video');
            if (previousSlideVideo.length > 0) {
                if ('pause' in previousSlideVideo[0]) previousSlideVideo[0].pause();
            }
            // Callback
            if (pb.params.onSlideChangeStart) pb.params.onSlideChangeStart(swiper);
        };
        pb.onSliderTransitionEnd = function (swiper) {
            // Reset zoom
            if (pb.params.zoom && gestureSlide && swiper.previousIndex !== swiper.activeIndex) {
                gestureImg.transform('translate3d(0,0,0) scale(1)');
                gestureImgWrap.transform('translate3d(0,0,0)');
                gestureSlide = gestureImg = gestureImgWrap = undefined;
                scale = currentScale = 1;
            }
            if (pb.params.onSlideChangeEnd) pb.params.onSlideChangeEnd(swiper);
        };

        pb.layout = function (index) {
            if (pb.params.type === 'page') {
                pb.container = $('.photo-browser-swiper-container').parents('.view');
            }
            else {
                pb.container = $('.photo-browser');
            }
            if (pb.params.type === 'standalone') {
                pb.container.addClass('photo-browser-in');
                // $.sizeNavbars(pb.container);
            }
            pb.swiperContainer = pb.container.find('.photo-browser-swiper-container');
            pb.swiperWrapper = pb.container.find('.photo-browser-swiper-wrapper');
            pb.slides = pb.container.find('.photo-browser-slide');
            pb.captionsContainer = pb.container.find('.photo-browser-captions');
            pb.captions = pb.container.find('.photo-browser-caption');

            var sliderSettings = {
                nextButton: pb.params.nextButton || '.photo-browser-next',
                prevButton: pb.params.prevButton || '.photo-browser-prev',
                indexButton: pb.params.indexButton,
                initialSlide: index,
                spaceBetween: pb.params.spaceBetween,
                speed: pb.params.speed,
                loop: pb.params.loop,
                lazyLoading: pb.params.lazyLoading,
                lazyLoadingInPrevNext: pb.params.lazyLoadingInPrevNext,
                lazyLoadingOnTransitionStart: pb.params.lazyLoadingOnTransitionStart,
                preloadImages: pb.params.lazyLoading ? false : true,
                onTap: function (swiper, e) {
                    if (pb.params.onTap) pb.params.onTap(swiper, e);
                },
                onClick: function (swiper, e) {
                    if (pb.params.exposition) pb.toggleExposition();
                    if (pb.params.onClick) pb.params.onClick(swiper, e);
                },
                onDoubleTap: function (swiper, e) {
                    pb.toggleZoom($(e.target).parents('.photo-browser-slide'));
                    if (pb.params.onDoubleTap) pb.params.onDoubleTap(swiper, e);
                },
                onTransitionStart: function (swiper) {
                    pb.onSliderTransitionStart(swiper);
                },
                onTransitionEnd: function (swiper) {
                    pb.onSliderTransitionEnd(swiper);
                },
                onLazyImageLoad: function (swiper, slide, img) {
                    if (pb.params.onLazyImageLoad) pb.params.onLazyImageLoad(pb, slide, img);
                },
                onLazyImageReady: function (swiper, slide, img) {
                    $(slide).removeClass('photo-browser-slide-lazy');
                    if (pb.params.onLazyImageReady) pb.params.onLazyImageReady(pb, slide, img);
                }
            };

            if (pb.params.swipeToClose && pb.params.type !== 'page') {
                sliderSettings.onTouchStart = pb.swipeCloseTouchStart;
                sliderSettings.onTouchMoveOpposite = pb.swipeCloseTouchMove;
                sliderSettings.onTouchEnd = pb.swipeCloseTouchEnd;
            }

            pb.swiper = $.swiper(pb.swiperContainer, sliderSettings);
            if (index === 0) {
                pb.onSliderTransitionStart(pb.swiper);
            }
            pb.attachEvents();
        };
        pb.attachEvents = function (detach) {
            var action = detach ? 'off' : 'on';
            // Slide between photos

            if (pb.params.zoom) {
                var target = pb.params.loop ? pb.swiper.slides : pb.slides;
                // Scale image
                target[action]('gesturestart', pb.onSlideGestureStart);
                target[action]('gesturechange', pb.onSlideGestureChange);
                target[action]('gestureend', pb.onSlideGestureEnd);
                // Move image
                target[action]('touchstart', pb.onSlideTouchStart);
                target[action]('touchmove', pb.onSlideTouchMove);
                target[action]('touchend', pb.onSlideTouchEnd);
            }
            pb.container.find('.photo-browser-close-link')[action]('click', pb.close);
        };

        // Expose
        pb.exposed = false;
        pb.toggleExposition = function () {
            if (pb.container) pb.container.toggleClass('photo-browser-exposed');
            if (pb.params.expositionHideCaptions) pb.captionsContainer.toggleClass('photo-browser-captions-exposed');
            pb.exposed = !pb.exposed;
        };
        pb.enableExposition = function () {
            if (pb.container) pb.container.addClass('photo-browser-exposed');
            if (pb.params.expositionHideCaptions) pb.captionsContainer.addClass('photo-browser-captions-exposed');
            pb.exposed = true;
        };
        pb.disableExposition = function () {
            if (pb.container) pb.container.removeClass('photo-browser-exposed');
            if (pb.params.expositionHideCaptions) pb.captionsContainer.removeClass('photo-browser-captions-exposed');
            pb.exposed = false;
        };

        // Gestures
        var gestureSlide, gestureImg, gestureImgWrap, scale = 1, currentScale = 1, isScaling = false;
        pb.onSlideGestureStart = function () {
            if (!gestureSlide) {
                gestureSlide = $(this);
                gestureImg = gestureSlide.find('img, svg, canvas');
                gestureImgWrap = gestureImg.parent('.photo-browser-zoom-container');
                if (gestureImgWrap.length === 0) {
                    gestureImg = undefined;
                    return;
                }
            }
            gestureImg.transition(0);
            isScaling = true;
        };
        pb.onSlideGestureChange = function (e) {
            if (!gestureImg || gestureImg.length === 0) return;
            scale = e.scale * currentScale;
            if (scale > pb.params.maxZoom) {
                scale = pb.params.maxZoom - 1 + Math.pow((scale - pb.params.maxZoom + 1), 0.5);
            }
            if (scale < pb.params.minZoom) {
                scale = pb.params.minZoom + 1 - Math.pow((pb.params.minZoom - scale + 1), 0.5);
            }
            gestureImg.transform('translate3d(0,0,0) scale(' + scale + ')');
        };
        pb.onSlideGestureEnd = function () {
            if (!gestureImg || gestureImg.length === 0) return;
            scale = Math.max(Math.min(scale, pb.params.maxZoom), pb.params.minZoom);
            gestureImg.transition(pb.params.speed).transform('translate3d(0,0,0) scale(' + scale + ')');
            currentScale = scale;
            isScaling = false;
            if (scale === 1) gestureSlide = undefined;
        };
        pb.toggleZoom = function () {
            if (!gestureSlide) {
                gestureSlide = pb.swiper.slides.eq(pb.swiper.activeIndex);
                gestureImg = gestureSlide.find('img, svg, canvas');
                gestureImgWrap = gestureImg.parent('.photo-browser-zoom-container');
            }
            if (!gestureImg || gestureImg.length === 0) return;
            gestureImgWrap.transition(300).transform('translate3d(0,0,0)');
            if (scale && scale !== 1) {
                scale = currentScale = 1;
                gestureImg.transition(300).transform('translate3d(0,0,0) scale(1)');
                gestureSlide = undefined;
            }
            else {
                scale = currentScale = pb.params.maxZoom;
                gestureImg.transition(300).transform('translate3d(0,0,0) scale(' + scale + ')');
            }
        };

        var imageIsTouched, imageIsMoved, imageCurrentX, imageCurrentY, imageMinX, imageMinY, imageMaxX, imageMaxY, imageWidth, imageHeight, imageTouchesStart = {}, imageTouchesCurrent = {}, imageStartX, imageStartY, velocityPrevPositionX, velocityPrevTime, velocityX, velocityPrevPositionY, velocityY;

        pb.onSlideTouchStart = function (e) {
            if (!gestureImg || gestureImg.length === 0) return;
            if (imageIsTouched) return;
            if ($.device.os === 'android') e.preventDefault();
            imageIsTouched = true;
            imageTouchesStart.x = e.type === 'touchstart' ? e.targetTouches[0].pageX : e.pageX;
            imageTouchesStart.y = e.type === 'touchstart' ? e.targetTouches[0].pageY : e.pageY;
        };
        pb.onSlideTouchMove = function (e) {
            if (!gestureImg || gestureImg.length === 0) return;
            pb.swiper.allowClick = false;
            if (!imageIsTouched || !gestureSlide) return;

            if (!imageIsMoved) {
                imageWidth = gestureImg[0].offsetWidth;
                imageHeight = gestureImg[0].offsetHeight;
                imageStartX = $.getTranslate(gestureImgWrap[0], 'x') || 0;
                imageStartY = $.getTranslate(gestureImgWrap[0], 'y') || 0;
                gestureImgWrap.transition(0);
            }
            // Define if we need image drag
            var scaledWidth = imageWidth * scale;
            var scaledHeight = imageHeight * scale;

            if (scaledWidth < pb.swiper.width && scaledHeight < pb.swiper.height) return;

            imageMinX = Math.min((pb.swiper.width / 2 - scaledWidth / 2), 0);
            imageMaxX = -imageMinX;
            imageMinY = Math.min((pb.swiper.height / 2 - scaledHeight / 2), 0);
            imageMaxY = -imageMinY;

            imageTouchesCurrent.x = e.type === 'touchmove' ? e.targetTouches[0].pageX : e.pageX;
            imageTouchesCurrent.y = e.type === 'touchmove' ? e.targetTouches[0].pageY : e.pageY;

            if (!imageIsMoved && !isScaling) {
                if (
                    (Math.floor(imageMinX) === Math.floor(imageStartX) && imageTouchesCurrent.x < imageTouchesStart.x) ||
                    (Math.floor(imageMaxX) === Math.floor(imageStartX) && imageTouchesCurrent.x > imageTouchesStart.x)
                    ) {
                    imageIsTouched = false;
                    return;
                }
            }
            e.preventDefault();
            e.stopPropagation();
            imageIsMoved = true;
            imageCurrentX = imageTouchesCurrent.x - imageTouchesStart.x + imageStartX;
            imageCurrentY = imageTouchesCurrent.y - imageTouchesStart.y + imageStartY;

            if (imageCurrentX < imageMinX) {
                imageCurrentX = imageMinX + 1 - Math.pow((imageMinX - imageCurrentX + 1), 0.8);
            }
            if (imageCurrentX > imageMaxX) {
                imageCurrentX = imageMaxX - 1 + Math.pow((imageCurrentX - imageMaxX + 1), 0.8);
            }

            if (imageCurrentY < imageMinY) {
                imageCurrentY = imageMinY + 1 - Math.pow((imageMinY - imageCurrentY + 1), 0.8);
            }
            if (imageCurrentY > imageMaxY) {
                imageCurrentY = imageMaxY - 1 + Math.pow((imageCurrentY - imageMaxY + 1), 0.8);
            }

            //Velocity
            if (!velocityPrevPositionX) velocityPrevPositionX = imageTouchesCurrent.x;
            if (!velocityPrevPositionY) velocityPrevPositionY = imageTouchesCurrent.y;
            if (!velocityPrevTime) velocityPrevTime = Date.now();
            velocityX = (imageTouchesCurrent.x - velocityPrevPositionX) / (Date.now() - velocityPrevTime) / 2;
            velocityY = (imageTouchesCurrent.y - velocityPrevPositionY) / (Date.now() - velocityPrevTime) / 2;
            if (Math.abs(imageTouchesCurrent.x - velocityPrevPositionX) < 2) velocityX = 0;
            if (Math.abs(imageTouchesCurrent.y - velocityPrevPositionY) < 2) velocityY = 0;
            velocityPrevPositionX = imageTouchesCurrent.x;
            velocityPrevPositionY = imageTouchesCurrent.y;
            velocityPrevTime = Date.now();

            gestureImgWrap.transform('translate3d(' + imageCurrentX + 'px, ' + imageCurrentY + 'px,0)');
        };
        pb.onSlideTouchEnd = function () {
            if (!gestureImg || gestureImg.length === 0) return;
            if (!imageIsTouched || !imageIsMoved) {
                imageIsTouched = false;
                imageIsMoved = false;
                return;
            }
            imageIsTouched = false;
            imageIsMoved = false;
            var momentumDurationX = 300;
            var momentumDurationY = 300;
            var momentumDistanceX = velocityX * momentumDurationX;
            var newPositionX = imageCurrentX + momentumDistanceX;
            var momentumDistanceY = velocityY * momentumDurationY;
            var newPositionY = imageCurrentY + momentumDistanceY;

            //Fix duration
            if (velocityX !== 0) momentumDurationX = Math.abs((newPositionX - imageCurrentX) / velocityX);
            if (velocityY !== 0) momentumDurationY = Math.abs((newPositionY - imageCurrentY) / velocityY);
            var momentumDuration = Math.max(momentumDurationX, momentumDurationY);

            imageCurrentX = newPositionX;
            imageCurrentY = newPositionY;

            // Define if we need image drag
            var scaledWidth = imageWidth * scale;
            var scaledHeight = imageHeight * scale;
            imageMinX = Math.min((pb.swiper.width / 2 - scaledWidth / 2), 0);
            imageMaxX = -imageMinX;
            imageMinY = Math.min((pb.swiper.height / 2 - scaledHeight / 2), 0);
            imageMaxY = -imageMinY;
            imageCurrentX = Math.max(Math.min(imageCurrentX, imageMaxX), imageMinX);
            imageCurrentY = Math.max(Math.min(imageCurrentY, imageMaxY), imageMinY);

            gestureImgWrap.transition(momentumDuration).transform('translate3d(' + imageCurrentX + 'px, ' + imageCurrentY + 'px,0)');
        };

        // Swipe Up To Close
        var swipeToCloseIsTouched = false;
        var allowSwipeToClose = true;
        var swipeToCloseDiff, swipeToCloseStart, swipeToCloseCurrent, swipeToCloseStarted = false, swipeToCloseActiveSlide, swipeToCloseTimeStart;
        pb.swipeCloseTouchStart = function () {
            if (!allowSwipeToClose) return;
            swipeToCloseIsTouched = true;
        };
        pb.swipeCloseTouchMove = function (swiper, e) {
            if (!swipeToCloseIsTouched) return;
            if (!swipeToCloseStarted) {
                swipeToCloseStarted = true;
                swipeToCloseStart = e.type === 'touchmove' ? e.targetTouches[0].pageY : e.pageY;
                swipeToCloseActiveSlide = pb.swiper.slides.eq(pb.swiper.activeIndex);
                swipeToCloseTimeStart = (new Date()).getTime();
            }
            e.preventDefault();
            swipeToCloseCurrent = e.type === 'touchmove' ? e.targetTouches[0].pageY : e.pageY;
            swipeToCloseDiff = swipeToCloseStart - swipeToCloseCurrent;
            var opacity = 1 - Math.abs(swipeToCloseDiff) / 300;
            swipeToCloseActiveSlide.transform('translate3d(0,' + (-swipeToCloseDiff) + 'px,0)');
            pb.swiper.container.css('opacity', opacity).transition(0);
        };
        pb.swipeCloseTouchEnd = function () {
            swipeToCloseIsTouched = false;
            if (!swipeToCloseStarted) {
                swipeToCloseStarted = false;
                return;
            }
            swipeToCloseStarted = false;
            allowSwipeToClose = false;
            var diff = Math.abs(swipeToCloseDiff);
            var timeDiff = (new Date()).getTime() - swipeToCloseTimeStart;
            if ((timeDiff < 300 && diff > 20) || (timeDiff >= 300 && diff > 100)) {
                setTimeout(function () {
                    if (pb.params.type === 'standalone') {
                        pb.close();
                    }
                    if (pb.params.type === 'popup') {
                        $.closeModal(pb.popup);
                    }
                    if (pb.params.onSwipeToClose) {
                        pb.params.onSwipeToClose(pb);
                    }
                    allowSwipeToClose = true;
                }, 0);
                return;
            }
            if (diff !== 0) {
                swipeToCloseActiveSlide.addClass('transitioning').transitionEnd(function () {
                    allowSwipeToClose = true;
                    swipeToCloseActiveSlide.removeClass('transitioning');
                });
            }
            else {
                allowSwipeToClose = true;
            }
            pb.swiper.container.css('opacity', '').transition('');
            swipeToCloseActiveSlide.transform('');
        };

        return pb;
    };

    PhotoBrowser.prototype = {
        defaults: {
            photos: [],
            container: 'body',
            initialSlide: 0,
            spaceBetween: 20,
            speed: 300,
            zoom: true,
            maxZoom: 3,
            minZoom: 1,
            exposition: true,
            expositionHideCaptions: false,
            type: 'standalone',
            navbar: true,
            toolbar: true,
            theme: 'light',
            swipeToClose: true,
            backLinkText: 'Close',
            ofText: 'of',
            loop: false,
            lazyLoading: false,
            lazyLoadingInPrevNext: false,
            lazyLoadingOnTransitionStart: false,
            /*
            Callbacks:
            onLazyImageLoad(pb, slide, img)
            onLazyImageReady(pb, slide, img)
            onOpen(pb)
            onClose(pb)
            onSlideChangeStart(swiper)
            onSlideChangeEnd(swiper)
            onTap(swiper, e)
            onClick(swiper, e)
            onDoubleTap(swiper, e)
            onSwipeToClose(pb)
            */
        }
    };

    $.photoBrowser = function (params) {
        $.extend(params, $.photoBrowser.prototype.defaults);
        return new PhotoBrowser(params);
    };

    $.photoBrowser.prototype = {
        defaults: {}
    };


})

//加载vue组件
window.addEventListener("load", function () {


    if (!Vue) {
        console.warn("没有Vue对象");
        return;
    }

    //这个图片列表组件
    Vue.component('img-list', {
        // 
        props: ['imgs'],
        template: '<div class="row" style="padding:10px 20px;background-color:white;margin-top:10px;">\
                        <div class="col-50" v-show="imgs.read_only !== true" >图片</div>\
                        <div class="col-100" v-show="imgs.read_only === true" >图片</div>\
                        <div class="col-50" v-show="imgs.read_only !== true" >\
                            <div class="wu-example">\
                                    <div class="btns">\
                                    <div :id="imgs.btn_id">选择文件</div>\
                                    </div>\
                            </div>\
                        </div>\
                        <div class="col-25" style="position:relative;margin-top:20px;" v-for="i in imgs.data">\
                            <div style="overflow: hidden;">\
                                <img :src="i.thumb_url" style="width:100%;" @click="img_click(i)" />\
                            </div>\
                            <div v-show="imgs.read_only !== true" class="div_delete_btn" style="height:20px; width:20px; position:absolute;cursor:pointer;right:-6px;top:-11px;background-color:red;border-radius:10px; color:white;font-size:0.6rem;text-align:center; font-weight:bold;" @click="img_delete(i)">X</div>\
                        </div>\
                    </div>',
        methods: {
            img_delete: function (img) {

                var my_vue = this;

                var new_url = my_vue.imgs.delete_img_url;


                $.confirm("确定要删除图片吗？", "", function () {

                    var new_imgs = my_vue.imgs.data.concat();

                    var index = new_imgs.indexOf(img);

                    new_imgs.splice(index, 1);

                    $.post(new_url, {
                        action: 'DELETE_ITEM',
                        table_name: my_vue.imgs.table_name,
                        row_id: my_vue.imgs.row_id,
                        field_text: my_vue.imgs.field_text,
                        file_json: JSON.stringify(new_imgs),
                        delete_code: img.code
                    }, function (result) {

                        if (!result.success) {
                            $.alert(result.error_msg);
                            return;
                        }

                        var index = my_vue.imgs.data.indexOf(img);

                        my_vue.imgs.data.splice(index, 1);

                    }, 'json');


                });


            },
            //图片点击事件
            img_click: function (img) {

                console.info("图片点击了！");

                var my_vue = this;

                var img_urls = [];

                var initialSlide = 0;

                my_vue.imgs.data.forEach(function (v, i) {

                    img_urls.push(v.url);

                    if (v === img) {

                        initialSlide = i;

                    }

                });


                /*=== 默认为 standalone ===*/
                var myPhotoBrowserStandalone = $.photoBrowser({
                    photos: img_urls,
                    initialSlide: initialSlide
                });

                myPhotoBrowserStandalone.open();


            }
        },
        //组件加载到界面上了
        mounted: function () {

            var my_vue = this;

            if (!WebUploader) {
                console.warn("没有WebUploader对象");
                return;
            }

            //只读的话，上传控件就不要初始化了！
            if (my_vue.imgs.read_only) {
                return;
            }

            var uploader = WebUploader.create({

                // swf文件路径
                swf: '/Core/scripts/webuploader-0.1.5/Uploader.swf',

                // 文件接收服务端。
                server: my_vue.imgs.server_url,

                // 选择文件的按钮。可选。
                pick: {
                    id: '#' + my_vue.imgs.btn_id,
                    multiple: true

                },
                //限制只上传什么类型的 只接受图片类型
                accept: {
                    title: 'Images',
                    extensions: 'gif,jpg,jpeg,bmp,png',
                    mimeTypes: 'image/*'
                },
                // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                resize: false,
                auto: true,
                //是否可以上传相同文件
                duplicate: true
            });

            // 文件上传成功，给item添加成功class, 用样式标记上传成功。
            uploader.on('uploadSuccess', function (file, response) {

                var obj = {
                    url: '',
                    name: '',
                    thumb_url: '',
                    code: ''
                }

                for (var name in obj) {

                    obj[name] = response[name];

                }

                my_vue.imgs.data.push(obj);

                var file_json = JSON.stringify(my_vue.imgs.data);

                $.post(my_vue.imgs.delete_img_url, {
                    action: "SAVE_FILE_INFO_BY_FIELD",
                    file_json: file_json,
                    table_name: my_vue.imgs.table_name,
                    field_text: my_vue.imgs.field_text,
                    row_id: my_vue.imgs.row_id
                }, function (result) {

                    if (!result.success) {

                        $.alert(result.error_msg);

                        return;

                    }


                }, "json");

            });

        }

    });
    //附件列表组件
    Vue.component('annex-list', {
        // 
        props: ['annexs'],
        template: ' <div class="list-block" style="margin-top:0.5rem;margin-bottom:0px;">\
                        <ul>\
                            <li>\
                                <div class="item-content">\
                                <div class="item-inner">\
                                        <div class="item-title label">附件</div>\
                                        <div class="item-input" v-show="annexs.read_only !== true">\
                                        <div class="wu-example">\
                                            <div class="btns">\
                                                <div :id="annexs.btn_id">选择文件</div>\
                                            </div>\
                                            </div>\
                                        </div>\
                                    </div>\
                                </div>\
                            </li>\
                            <li v-for="a in annexs.data">\
                                <div class="item-content">\
                                    <div class="item-inner">\
                                        <div class="item-title"  @click="annex_click(a)" >{{a.name}}<span style="color:#9E9E9E;font-size:0.5rem;display:none;">({{a.FILE_SIZE_STR}})</span></div>\
                                        <div v-show="annexs.read_only !== true" class="item-after" style="color:red;cursor:pointer;" @click="delete_annex(a)">删除</div>\
                                    </div>\
                                </div>\
                            </li>\
                        </ul>\
                    </div>',
        methods: {

            delete_annex: function (annex) {

                var my_vue = this;

                var new_url = my_vue.annexs.delete_annex_url;

                $.confirm("确定要删除附件吗？", "", function () {

                    var new_imgs = my_vue.annexs.data.concat();

                    var index = new_imgs.indexOf(annex);

                    new_imgs.splice(index, 1);

                    $.post(new_url, {
                        action: 'DELETE_ITEM',
                        table_name: my_vue.annexs.table_name,
                        row_id: my_vue.annexs.row_id,
                        field_text: my_vue.annexs.field_text,
                        file_json: JSON.stringify(new_imgs),
                        delete_code: annex.code
                    }, function (result) {

                        if (!result.success) {
                            $.alert(result.error_msg);
                            return;
                        }

                        var index = my_vue.annexs.data.indexOf(annex);

                        my_vue.annexs.data.splice(index, 1);

                    }, 'json');


                });

            },
            //附件点击事件
            annex_click: function (a) {

                var my_vue = this;

                window.open(a.url);

            }

        },
        //组件加载到界面上了
        mounted: function () {

            var my_vue = this;

            console.log(my_vue.annexs);

            if (!WebUploader) {
                console.warn("没有WebUploader对象");
                return;
            }



            //如果是只读的组件，就不要初始化上传控件了！
            if (my_vue.annexs.read_only) {
                return;
            }



            var uploader = WebUploader.create({

                // swf文件路径
                swf: '/Core/scripts/webuploader-0.1.5/Uploader.swf',

                // 文件接收服务端。
                server: my_vue.annexs.server_url,

                // 选择文件的按钮。可选。
                pick: {
                    id: '#' + my_vue.annexs.btn_id,
                    multiple: true

                },
                // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
                resize: false,
                auto: true,
                //是否可以上传相同文件
                duplicate: true
            });

            // 文件上传成功，给item添加成功class, 用样式标记上传成功。
            uploader.on('uploadSuccess', function (file, response) {

                var obj = {
                    url: '',
                    name: '',
                    thumb_url: '',
                    code: ''
                }

                for (var name in obj) {

                    obj[name] = response[name];

                }

                my_vue.annexs.data.push(obj);

                var file_json = JSON.stringify(my_vue.annexs.data);

                console.log("附件的保存上传地址：");

                console.log(my_vue.annexs.delete_annex_url);


                $.post(my_vue.annexs.delete_annex_url, {
                    action: "SAVE_FILE_INFO_BY_FIELD",
                    file_json: file_json,
                    table_name: my_vue.annexs.table_name,
                    field_text: my_vue.annexs.field_text,
                    row_id: my_vue.annexs.row_id
                }, function (result) {

                    if (!result.success) {

                        $.alert(result.error_msg);

                        return;

                    }


                }, "json");

            });

        }

    });

    //这是常用的自动跳地址的 带图片的列表
    Vue.component('my-list-link', {
        template: '<div class="list-block media-list" style="margin:0px;" >\
                    <ul>\
                    <li class="list-group-title" style="color:black;" v-if="title_text">{{title_text}}</li>\
                    <li v-for="l in list_data">\
                        <div class="item-content item-link" @click="item_click(l)" style="padding-left:0.5rem;">\
                            <div class="item-media" v-if="l.img_src">\
                                <img :src="l.img_src" style="width: 2.5rem;" />\
                            </div>\
                            <div class="item-inner">\
                                <div class="item-title-row">\
                                    <div class="item-title">{{l.title}}</div>\
                                    <div class="item-after">{{date_format(l.create_date)}}</div>\
                                </div>\
                                <div class="item-subtitle">{{l.text}}</div>\
                                <div class="item-subtitle" v-show="l.sub_text" >{{l.sub_text}}</div>\
                            </div>\
                        </div>\
                    </li>\
                    <li v-if="list_data.length === 0">\
                        <div class="item-content " style="padding-left:0.5rem;">\
                            <div class="item-inner">\
                                <div class="item-subtitle" style="text-align:center;">没有数据...</div>\
                            <div>\
                        </div>\
                    </li>\
                    <li v-if="load_more">\
                            <div class="item-content " style="padding-left:0.5rem;" @click="load_more_fun()">\
                            <div class="item-inner">\
                                <div class="item-subtitle" style="text-align:center;" >加载更多</div>\
                            <div>\
                        </div>\
                    </li>\
                    </ul>\
                </div>',
        // title_text 标题文字   list_data  列表数据集合   load_more  是否要显示加载更多按钮  get_data_url  获取数据路径  get_data_parm 获取数据参数
        props: ['title_text', 'list_data', 'load_more', 'load_size', 'get_data_url', 'get_data_parm'],

        computed: {
            //传进来的加载更多参数 和 一些默认参数
            parm_obj: function () {

                var me = this;

                var obj = me.get_data_parm || {};

                var parm = obj.clone();

                //加载多少条数据 没有从外面传进来就默认20条
                parm.page_size = me.load_size || 20;

                //当前数据索引
                parm.page_index = 0;

                return obj.clone();

            }

        },
        data: function () {

            return {

                //加载更多状态
                load_more_sid: false

            }


        },
        methods: {
            //项点击事件
            item_click: function (obj) {

                var me = this;

                console.log("组件里面的点击事件：url:" + obj.url);

                $.router.load(obj.url, true);



            },
            //时间格式化
            date_format: function (date_text) {

                console.log("进来组件的格式化时间函数这里了！");

                console.log(date_text);

                //时间字符串等于空
                if (!date_text || date_text.length === 0) {

                    return "";
                }



                try {

                    return moment(date_text).fromNow();

                } catch (ex) {

                    console.log("格式化日期出错了！");
                    console.log(ex);

                    return date_text;


                }


            },
            //加载更多函数
            load_more_fun: function () {

                var me = this;

                console.log("列表组件点击了加载更多事件！");

                //不让加载更多执行很多次
                if (me.load_more_sid) {

                    console.log("上一个加载更多函数都没有执行完！");
                    return;

                }

                //不让加载更多执行很多次
                me.load_more_sid = true;

                $.post(me.get_data_url, me.parm_obj, function (result) {

                    //不让加载更多执行很多次
                    me.load_more_sid = false;

                    if (!result.success) {

                        console.info("这个是组件加载更多数据时出错了！");
                        console.error(result.error_msg);

                        return;

                    }

                    //循环添加进集合里面
                    result.data.forEach(function (v, i) {

                        me.list_data.push(v);
                    });

                    //当前页自加一
                    me.page_size++;

                }, 'json').error(function () {

                    //不让加载更多执行很多次
                    me.load_more_sid = false;

                    console.log("这个是执行post函数错了！");

                });


            }

        }

    });

    //简单的列表组件
    Vue.component('my-list', {
        template: '<div class="list-block media-list" style="margin:0px;" >\
                    <ul>\
                    <li class="list-group-title" style="color:black;" v-if="title_text">{{title_text}}</li>\
                    <li v-for="l in list_data">\
                        <div class="item-content " :class="{\'item-link\':l.item_link}" @click="item_click(l)" style="padding-left:0.5rem;">\
                            <div class="item-media" v-if="l.img_src">\
                                <img :src="l.img_src" style="width: 2.5rem;" />\
                            </div>\
                            <div class="item-inner">\
                                <div class="item-title-row">\
                                    <div class="item-title">{{l.title}}</div>\
                                    <div class="item-after">{{date_format(l.create_date)}}</div>\
                                </div>\
                                <div class="item-subtitle">{{l.text}}</div>\
                            </div>\
                        </div>\
                    </li>\
                    <li v-if="list_data.length === 0">\
                        <div class="item-content " style="padding-left:0.5rem;">\
                            <div class="item-inner">\
                                <div class="item-subtitle" style="text-align:center;">没有数据...</div>\
                            <div>\
                        </div>\
                    </li>\
                    </ul>\
                </div>',
        props: ['title_text', 'list_data'],
        methods: {
            //项点击事件
            item_click: function (obj) {

                var me = this;
                //这是给触发给外面父组件用的
                me.$emit('item_click', obj);

            },
            //时间格式化
            date_format: function (date_text) {

                console.log("进来组件的格式化时间函数这里了！");

                console.log(date_text);

                //时间字符串等于空
                if (!date_text || date_text.length === 0) {

                    return "";
                }



                try {

                    return moment(date_text).fromNow();

                } catch (ex) {

                    console.log("格式化日期出错了！");
                    console.log(ex);

                    return date_text;


                }


            }

        }

    });

    //微信上传图片组件
    Vue.component('wx-img-upload',{

        props: ['imgs','left_text'],
        template: '<div class="row" style="padding:0px 20px;background-color:white;margin-top:10px;">\
                        <div class="col-50" style="margin-top: 0.5rem;padding-left: 0.5rem;">{{left_text}}</div>\
                        <div class="col-50" @click="img_upload()" style="margin: 0px;font-size: 1.3rem;text-align: right;padding-right: 1rem;">\
                            <span class="icon icon-picture"></span>\
                        </div>\
                        <div class="col-25" style="position:relative;margin-top:20px;" v-for="i in imgs.data">\
                            <div style="overflow: hidden;">\
                                <img :src="i.thumb_url" style="width:100%;" @click="img_click(i)" />\
                            </div>\
                            <div  class="div_delete_btn" style="height:20px; width:20px; position:absolute;cursor:pointer;right:-6px;top:-11px;background-color:red;border-radius:10px; color:white;font-size:0.6rem;text-align:center; font-weight:bold;" @click="img_delete(i)">X</div>\
                        </div>\
                    </div>',
     
        methods: {
            img_delete: function (img) {

                var my_vue = this;

                var new_url = my_vue.imgs.delete_img_url;


                $.confirm("确定要删除图片吗？", "", function () {

                    var new_imgs = my_vue.imgs.data.concat();

                    var index = new_imgs.indexOf(img);

                    new_imgs.splice(index, 1);

                    $.post(new_url, {
                        action: 'DELETE_ITEM',
                        table_name: my_vue.imgs.table_name,
                        row_id: my_vue.imgs.row_id,
                        field_text: my_vue.imgs.field_text,
                        file_json: JSON.stringify(new_imgs),
                        delete_code: img.code
                    }, function (result) {

                        if (!result.success) {
                            $.alert(result.error_msg);
                            return;
                        }

                        var index = my_vue.imgs.data.indexOf(img);

                        my_vue.imgs.data.splice(index, 1);

                    }, 'json');


                });


            },
            //图片点击事件
            img_click: function (img) {

                console.info("图片点击了！");

                var my_vue = this;

                var img_urls = [];

                var initialSlide = 0;

                my_vue.imgs.data.forEach(function (v, i) {

                    img_urls.push(v.url);

                    if (v === img) {

                        initialSlide = i;

                    }

                });


                /*=== 默认为 standalone ===*/
                var myPhotoBrowserStandalone = $.photoBrowser({
                    photos: img_urls,
                    initialSlide: initialSlide
                });

                myPhotoBrowserStandalone.open();


            },
            //图片上传点击事件
            img_upload: function () {

                var my_vue = this;

                //判断微信全局对象是否存在
                if (!wx) {
                    $.toast("哦噢，微信对象没有！");
                    return;
                }

                wx.chooseImage({
                    count: 9, // 默认9
                    sizeType: ['original', 'compressed'], // 可以指定是原图还是压缩图，默认二者都有
                    sourceType: ['album', 'camera'], // 可以指定来源是相册还是相机，默认二者都有
                    success: function (res) {

                        var localIds = res.localIds; // 返回选定照片的本地ID列表，localId可以作为img标签的src属性显示图片

                        localIds.forEach(function (v, i) {

                            wx.uploadImage({
                                localId: v, // 需要上传的图片的本地ID，由chooseImage接口获得
                                isShowProgressTips: 1, // 默认为1，显示进度提示
                                success: function (res) {

                                    var serverId = res.serverId; // 返回图片的服务器端ID

                                    $.post(my_vue.imgs.delete_img_url, {
                                        action: 'UPLOAD_IMG',
                                        media_id: serverId,
                                        table_name: my_vue.imgs.table_name,
                                        tag_code: my_vue.imgs.tag_code,
                                        item_id: my_vue.imgs.row_id,
                                        field_text: my_vue.imgs.field_text
                                    }, function (result) {

                                        if (!result.success) {

                                            $.alert(result.error_msg);

                                            return;

                                        }

                                        var data = result.data;

                                        var obj = {
                                            url: '',
                                            name: '',
                                            thumb_url: '',
                                            code: ''
                                        }

                                        for (var name in obj) {

                                            obj[name] = data[name];

                                        }

                                        my_vue.imgs.data.push(obj);

                                        var file_json = JSON.stringify(my_vue.imgs.data);

                                        $.post(my_vue.imgs.delete_img_url, {
                                            action: "SAVE_FILE_INFO_BY_FIELD",
                                            file_json: file_json,
                                            table_name: my_vue.imgs.table_name,
                                            field_text: my_vue.imgs.field_text,
                                            row_id: my_vue.imgs.row_id
                                        }, function (result) {

                                            if (!result.success) {

                                                $.alert(result.error_msg);

                                                return;

                                            }


                                        }, "json");


                                    }, "json");

                                }
                            });
                        });
                    }
                });

            }

        },
        //创建成功！
        mounted: function () {

            var my_vue = this;

            console.log("主键初始化成功了！");
            console.log(my_vue.left_text);



        }
    });

    console.info("vue加载完成了！");

});


//界面加载完成事件   这是给禁止苹果浏览器的动态效果
window.addEventListener("load", function () {


    window.onerror = function (msg, url, l) {

        txt = "There was an error on this page.\n\n"
        txt += "Error: " + msg + "\n"
        txt += "URL: " + url + "\n"
        txt += "Line: " + l + "\n\n"
        txt += "Click OK to continue.\n\n"
        alert(txt)
    };

    //FastClick.attach(document.body);

    $("body").height($(window).height());


    $("body").width($(window).width());

    $.router = Mini2.create('Mini2.ui.PageRoute', {});

    //这个是在ios系统上隐藏标题栏的返回按钮的
    //if ($.device.ios) {

    //    loadStyle(" .page .bar .back {   display:none;  } ");

    //}
})


//界面加载完成事件  这是界面随着window的高度和宽度改变而改变
window.addEventListener("resize", function () {

    $("body").height($(window).height());

    $("body").width($(window).width());

})


